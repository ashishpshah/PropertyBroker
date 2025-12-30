using Broker.Controllers;
using Broker.Infra;
using Broker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;

namespace Broker.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class HomeController : BaseController<ResponseModel<LoginViewModel>>
	{
		public HomeController(IRepositoryWrapper repository) : base(repository) { }

		public ActionResult Index()
		{
			if (Common.LoggedUser_Id() <= 0)
				return RedirectToAction("Account", "Home", new { Area = "Admin" });

				CommonViewModel.Obj = new LoginViewModel();
			    CommonViewModel.Obj.LeadPendingFollowUpList = new List<Lead>();
			    CommonViewModel.Obj.Count = 0;
            var dt = DataContext_Command.ExecuteQuery("select Count(*) AS Count from Property");
            CommonViewModel.Obj.LeadPendingFollowUpList = DataContext_Command.Pending_Lead_FollowUp_Get().ToList();
			CommonViewModel.Obj.Count = Convert.ToInt32(dt.Rows[0]["Count"]);
            return View(CommonViewModel);
		}
        public ActionResult PropertyList(long  Type_Id = 0)
        {
            

            CommonViewModel.Obj = new LoginViewModel();
            //CommonViewModel.Obj.LeadPendingFollowUpList = new List<Lead>();          
            
			CommonViewModel.Obj.PropertiesList = new List<Properties>();
			
            var list = new List<SelectListItem_Custom>();
            var dt = new DataTable();
            //var sqlParameters = new List<SqlParameter>();
            //sqlParameters.Add(new SqlParameter("@Id", SqlDbType.VarChar) { Value = 0 });
            dt = DataContext_Command.ExecuteStoredProcedure_DataTable("SP_PropertyTypes_Combo_ByProperties", null, true);
            long  defaultTypeId = 0;
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var item = new SelectListItem_Custom(
                        Convert.ToString(dr["Id"]),
                        Convert.ToString(dr["Name"]),
                        Convert.ToString(dr["Property_Count"]),
                        Convert.ToString(dr["Display_Seq_No"]),
                        "PropertyType")
                    {
                        Value = Convert.ToString(dr["Id"]),
                        Text = Convert.ToString(dr["Name"]),
                        Value2 = Convert.ToString(dr["Property_Count"]),                      
                        Value3 = dr["Display_Seq_No"] != DBNull.Value ? Convert.ToString(dr["Display_Seq_No"]) : "0"
                    };

                    list.Add(item);

                    // ⭐ Capture default type (Display_Seq_No = 1)
                    if (defaultTypeId == 0 && item.Value3 == "1")
                    {
                        defaultTypeId = Convert.ToInt64(item.Value);
                    }
                    
                }
            }
            long selectedTypeId = Type_Id > 0 ? Type_Id : defaultTypeId;
            CommonViewModel.Obj.PropertiesList = DataContext_Command.Property_Get(0, selectedTypeId);
			CommonViewModel.Obj.Selected_Type_Id = selectedTypeId;
            CommonViewModel.SelectListItems = list;
            return View(CommonViewModel);
        }
        public ActionResult Account()
		{
			Common.Clear_Session();

			return View(new ResponseModel<LoginViewModel>());
		}

        public ActionResult GetFollowUp_List(long Lead_Id = 0, string Status = "")
        {
			CommonViewModel.Obj = new LoginViewModel();
			CommonViewModel.Obj.LeadFollowupList = new List<LeadFollowup>();

			CommonViewModel.Obj.LeadFollowupList = DataContext_Command.LeadFollowUp_Get(0,Lead_Id, Status).ToList();

                return PartialView("_Partial_AddEditForm_FollowUp", CommonViewModel);
            

        }

        public ActionResult Partial_AddEditForm_PropertyDetail(long Id = 0)
        {
            CommonViewModel.Obj = new LoginViewModel();
            CommonViewModel.Obj.Properties = new Properties();

            CommonViewModel.Obj.Properties = DataContext_Command.Property_Get(Id , 0).FirstOrDefault();

            return PartialView("_Partial_AddEditForm_PropertyDetail", CommonViewModel);


        }
        [HttpPost]
		//[ValidateAntiForgeryToken]
		public JsonResult Login(LoginViewModel viewModel)
		{
			try
			{
				if (!string.IsNullOrEmpty(viewModel.UserName) && viewModel.UserName.Length > 0 && _context.Using<User>().GetAll().ToList().Any(x => x.UserName == viewModel.UserName))
				{
					viewModel.Password = Common.Encrypt(viewModel.Password);

					var obj = _context.Using<User>().GetByCondition(x => x.UserName == viewModel.UserName && x.Password == viewModel.Password).FirstOrDefault();

					if (obj != null && obj.IsActive == true && obj.IsDeleted == false)
					{
						var userRole = _context.Using<UserRoleMapping>().GetByCondition(x => x.UserId == obj.Id).FirstOrDefault();

						obj.RoleId = userRole != null ? userRole.RoleId : 0;

						List<UserMenuAccess> listMenuAccess = new List<UserMenuAccess>();
						List<UserMenuAccess> listMenuPermission = new List<UserMenuAccess>();

						Role role = _context.Using<Role>().GetByCondition(x => x.Id == obj.RoleId).FirstOrDefault();

						if (role == null)
						{
							CommonViewModel.IsSuccess = false;
							CommonViewModel.StatusCode = ResponseStatusCode.Error;
							CommonViewModel.Message = ResponseStatusMessage.Error;

							return Json(CommonViewModel);
						}
						else if (role != null && role.Id == 1)
						{
							listMenuAccess = (from y in _context.Using<Menu>().GetAll().ToList()
											  where y.IsActive == true && y.IsDeleted == false
											  select new UserMenuAccess() { Id = y.Id, ParentMenuId = y.ParentId, Area = y.Area, Controller = y.Controller, Url = y.Url, MenuName = y.Name, IsCreate = true, IsUpdate = true, IsRead = true, IsDelete = true, DisplayOrder = y.DisplayOrder, IsActive = y.IsActive, IsDeleted = y.IsDeleted }).ToList();
						}
						else if (role != null && (role.IsAdmin || role.Name.ToUpper() == "VENDOR"))
						{
							listMenuAccess = (from x in _context.Using<UserMenuAccess>().GetAll().ToList()
											  join y in _context.Using<Menu>().GetAll().ToList() on x.MenuId equals y.Id
											  where x.UserId == obj.Id && x.RoleId == obj.RoleId
											  && y.IsActive == true && y.IsDeleted == false && x.IsActive == true && x.IsDeleted == false && y.Name != "Menu"
											  && x.IsRead == true
											  select new UserMenuAccess() { Id = y.Id, ParentMenuId = y.ParentId, Area = y.Area, Controller = y.Controller, Url = y.Url, MenuName = y.Name, DisplayOrder = y.DisplayOrder, IsActive = x.IsActive, IsDeleted = x.IsDeleted }).ToList();
						}
						else if (role != null && !role.IsAdmin && role.IsActive && !role.IsDeleted)
						{
							listMenuAccess = (from x in _context.Using<UserMenuAccess>().GetAll().ToList()
											  join y in _context.Using<Menu>().GetAll().ToList() on x.MenuId equals y.Id
											  where x.UserId == obj.Id && x.RoleId == obj.RoleId
											  && y.IsActive == true && y.IsDeleted == false && x.IsActive == true && x.IsDeleted == false && y.Id != 1 && y.ParentId != 1 && y.Name != "Menu"
											  && x.IsRead == true
											  select new UserMenuAccess() { Id = y.Id, ParentMenuId = y.ParentId, Area = y.Area, Controller = y.Controller, Url = y.Url, MenuName = y.Name, DisplayOrder = y.DisplayOrder, IsActive = x.IsActive, IsDeleted = x.IsDeleted }).ToList();
						}

						if (role != null && role.Id == 1)
							listMenuPermission = listMenuAccess;
						else
							listMenuPermission = (from x in _context.Using<UserMenuAccess>().GetAll().ToList()
												  join y in _context.Using<Menu>().GetAll().ToList() on x.MenuId equals y.Id
												  where x.UserId == obj.Id && y.IsActive == true && y.IsDeleted == false && x.IsActive == true && x.IsDeleted == false
												  && listMenuAccess.Any(z => z.Id == y.Id)
												  select new UserMenuAccess() { MenuId = y.Id, ParentMenuId = y.ParentId, Area = y.Area, Controller = y.Controller, Url = y.Url, MenuName = y.Name, IsCreate = x.IsCreate, IsUpdate = x.IsUpdate, IsRead = x.IsRead, IsDelete = x.IsDelete, IsActive = x.IsActive, IsDeleted = x.IsDeleted }).ToList();

						Common.Configure_UserMenuAccess(listMenuAccess.Where(x => x.IsActive == true && x.IsDeleted == false).ToList(), listMenuPermission.Where(x => x.IsActive == true && x.IsDeleted == false).ToList());

						Common.Set_Session_Int(SessionKey.KEY_USER_ID, obj.Id);
						Common.Set_Session_Int(SessionKey.KEY_USER_ROLE_ID, obj.RoleId);

						Common.Set_Session(SessionKey.KEY_USER_NAME, obj.UserName);
						Common.Set_Session(SessionKey.KEY_USER_ROLE, role.Name);
						Common.Set_Session_Int(SessionKey.KEY_IS_ADMIN, (role.IsAdmin || obj.RoleId == 1 ? 1 : 0));
						Common.Set_Session_Int(SessionKey.KEY_IS_VENDOR, (role.Name.ToUpper() == "VENDOR" ? 1 : 0));
						Common.Set_Session_Int(SessionKey.KEY_IS_CUSTOMER, (role.Name.ToUpper() == "CUSTOMER" ? 1 : 0));
						Common.Set_Session_Int(SessionKey.KEY_IS_EMPLOYEE, (role.Name.ToUpper() == "EMPLOYEE" ? 1 : 0));
						Common.Set_Session_Int(SessionKey.KEY_IS_SUPER_USER, (obj.RoleId == 1 ? 1 : 0));

						long VendorId = 0;

						if (role.Name.ToUpper() == "EMPLOYEE")
						{
							VendorId = 0;

							var employee = _context.Using<Employee>().GetByCondition(x => x.UserId == obj.Id).FirstOrDefault();
							VendorId = employee != null ? employee.VendorId : 0;
							Common.Set_Session_Int(SessionKey.KEY_EMPLOYEE_ID, employee != null ? employee.Id : 0);

						}

						Common.Set_Session_Int(SessionKey.KEY_VENDOR_ID, VendorId);

						CommonViewModel.IsSuccess = true;
						CommonViewModel.StatusCode = ResponseStatusCode.Success;
						CommonViewModel.Message = ResponseStatusMessage.Success;

						CommonViewModel.RedirectURL = Url.Content("~/") + "Admin/" + this.ControllerContext.RouteData.Values["Controller"].ToString() + "/Index";

						return Json(CommonViewModel);
					}

				}

				CommonViewModel.IsSuccess = false;
				CommonViewModel.StatusCode = ResponseStatusCode.Error;
				CommonViewModel.Message = "User Id and Password does not Match";

			}
			catch (Exception ex)
			{
				CommonViewModel.IsSuccess = false;
				CommonViewModel.StatusCode = ResponseStatusCode.Error;
				CommonViewModel.Message = ResponseStatusMessage.Error + " | " + ex.Message;
			}

			return Json(CommonViewModel);
		}

		public ActionResult Logout()
		{
			Common.Clear_Session();

			return RedirectToAction("Account", "Home", new { Area = "Admin" });
		}

        [HttpGet]
        public PartialViewResult ForgotPassword()
        {
            return PartialView("_Partial_ForgotPassword", new ResponseModel<ForgotPassword>());
        }


        private static Dictionary<string, (string OTP, DateTime Expiry)> _otpStore
    = new Dictionary<string, (string OTP, DateTime Expiry)>();

        [HttpPost]
        public JsonResult ForgotPassword_SendOTP(string email)
        {
            var userExists = _context.Using<User>().Any(u => u.Email == email);
            if (!userExists)
                return Json(new { IsSuccess = false, Message = "Email does not exist." });

            // Generate OTP
            var newOtp = new Random().Next(100000, 999999).ToString();

            // Store in memory for 10 minutes
            _otpStore[email] = (newOtp, DateTime.Now.AddMinutes(10));

            // Send Email
            Common.SendEmail(
                "Your OTP Code",
                email,
                true,
                "",
                "otp_message",
                JsonConvert.SerializeObject(new { otp = newOtp })
            );

            return Json(new { IsSuccess = true, Message = "OTP sent to email." });
        }




        [HttpPost]
        public JsonResult ForgotPassword_VerifyOTP(string email, string otp)
        {
            //  Get latest OTP for user that is not used
            if (!_otpStore.ContainsKey(email))
                return Json(new { IsSuccess = false, Message = "OTP expired or not found." });

            var entry = _otpStore[email];
            if (entry.Expiry < DateTime.Now)
            {
                _otpStore.Remove(email);
                return Json(new { IsSuccess = false, Message = "OTP expired." });
            }

            if (entry.OTP != otp)
                return Json(new { IsSuccess = false, Message = "Invalid OTP." });

            // OTP verified — remove it
            _otpStore.Remove(email);

            return Json(new { IsSuccess = true, Message = "OTP verified." });

        }


        // Step 3: Reset Password
        [HttpPost]
        public JsonResult ForgotPassword_ResetPassword(string email, string newPassword)
        {
            // Load the existing user from DB
            var user = _context.Using<User>().GetByCondition(u => u.Email == email).FirstOrDefault();
            if (user == null)
                return Json(new { IsSuccess = false, Message = "User not found." });

            // Update password only
            user.Password = Common.Encrypt(newPassword);

            // Use your existing Update method
            _context.Using<User>().Update(user);

            return Json(new { IsSuccess = true, Message = "Password reset successfully." });
        }




    }
}