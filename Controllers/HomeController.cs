using Broker.Infra;
using Broker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.CodeAnalysis;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol.Core.Types;
using System.Data;
using System.Diagnostics;

namespace Broker.Controllers
{
	public class HomeController : BaseController<ResponseModel<LoginViewModel>>
	{
		public HomeController(IRepositoryWrapper repository) : base(repository) { }

		public IActionResult Index()
		{
			//if (Common.LoggedUser_Id() <= 0)
			//	return RedirectToAction("Account", "Home", new { Area = "Admin" });

			//var list = _context.Using<User>().GetAll().ToList();

			//var (IsSuccess, Message, Id) = (false, "", (long)0);

			//if (list == null || list.Count == 0)
			//{
			//	Common.Set_Session_Int(SessionKey.KEY_USER_ID, 1);

			//	var user = new User() { UserName = "Adnin", Password = Common.Encrypt("admin"), CreatedBy = 1 };
			//	user = _context.Using<User>().Add(user);
			//	//_context.SaveChanges();
			//	//_context.Entry(user).Reload();

			//	var role = new Role() { Name = "Super Admin", IsAdmin = true, CreatedBy = 1 };
			//	role = _context.Using<Role>().Add(role);
			//	//_context.SaveChanges();

			//	var userRole = new UserRoleMapping() { UserId = user.Id, RoleId = role.Id, CreatedBy = 1 };
			//	_context.Using<UserRoleMapping>().Add(userRole);
			//	//_context.SaveChanges();

			//	user = new User() { UserName = "Admin", Password = Common.Encrypt("admin"), CreatedBy = 1 };
			//	user = _context.Using<User>().Add(user);
			//	//_context.SaveChanges();
			//	//_context.Entry(user).Reload();

			//	role = new Role() { Name = "Admin", IsAdmin = true, CreatedBy = 1 };
			//	role = _context.Using<Role>().Add(role);
			//	//_context.SaveChanges();
			//	//_context.Entry(role).Reload();

			//	userRole = new UserRoleMapping() { UserId = user.Id, RoleId = role.Id, CreatedBy = 1 };
			//	_context.Using<UserRoleMapping>().Add(userRole);
			//	//_context.SaveChanges();

			//	var menu = new Menu() { ParentId = 0, Area = "", Controller = "", Name = "Configuration", IsSuperAdmin = false, IsAdmin = true, DisplayOrder = 1, CreatedBy = 1 };
			//	menu = _context.Using<Menu>().Add(menu);
			//	//_context.SaveChanges();
			//	//if (menu.Id <= 0) _context.Entry(menu).Reload();

			//	var userMenuAccess = new UserMenuAccess() { UserId = user.Id, RoleId = role.Id, MenuId = menu.Id, IsCreate = true, IsUpdate = true, IsRead = true, IsDelete = true, CreatedBy = 1 };
			//	_context.Using<UserMenuAccess>().Add(userMenuAccess);
			//	//_context.SaveChanges();

			//	List<Menu> listMenu_Child = new List<Menu>();

			//	listMenu_Child.Add(new Menu() { ParentId = menu.Id, Area = "Admin", Controller = "User", Name = "User", IsSuperAdmin = false, IsAdmin = true, DisplayOrder = 2, CreatedBy = 1 });
			//	listMenu_Child.Add(new Menu() { ParentId = menu.Id, Area = "Admin", Controller = "Role", Name = "Role", IsSuperAdmin = false, IsAdmin = true, DisplayOrder = 3, CreatedBy = 1 });
			//	listMenu_Child.Add(new Menu() { ParentId = menu.Id, Area = "Admin", Controller = "Access", Name = "User Access", IsSuperAdmin = false, IsAdmin = true, DisplayOrder = 4, CreatedBy = 1 });
			//	listMenu_Child.Add(new Menu() { ParentId = menu.Id, Area = "Admin", Controller = "Menu", Name = "Menu", IsSuperAdmin = true, IsAdmin = false, DisplayOrder = 5, CreatedBy = 1 });

			//	for (int i = 0; i < listMenu_Child.Count; i++)
			//	{
			//		listMenu_Child[i] = _context.Using<Menu>().Add(listMenu_Child[i]);
			//	}
			//	//foreach (var item in listMenu_Child)
			//	//{
			//	//    _context.Using<Menu>().Add(item);
			//	//    _context.SaveChanges();
			//	//    if (item.Id <= 0) _context.Entry(item).Reload();
			//	//}

			//	foreach (var item in listMenu_Child.OrderBy(x => x.ParentId).ThenBy(x => x.Id).ToList())
			//	{
			//		var roleMenuAccess = new RoleMenuAccess() { RoleId = role.Id, MenuId = item.Id, IsCreate = true, IsUpdate = true, IsRead = true, IsDelete = true, CreatedBy = 1 };
			//		_context.Using<RoleMenuAccess>().Add(roleMenuAccess);
			//		//_context.SaveChanges();
			//	}

			//	foreach (var item in listMenu_Child.OrderBy(x => x.ParentId).ThenBy(x => x.Id).ToList())
			//	{
			//		userMenuAccess = new UserMenuAccess() { UserId = user.Id, RoleId = role.Id, MenuId = item.Id, IsCreate = true, IsUpdate = true, IsRead = true, IsDelete = true, CreatedBy = 1 };
			//		_context.Using<UserMenuAccess>().Add(userMenuAccess);
			//		//_context.SaveChanges();
			//	}
			//}


			CommonViewModel.SelectListItems = new List<SelectListItem_Custom>();

			//var list = _context.Using<AreasMaster>().GetByCondition(x => x.Id > 1, x => x.City).OrderBy(x => x.Id)
			//			.Select(x => new SelectListItem_Custom(x.Id.ToString(), x.Name + ", " + x.City.Name, "L")).Distinct().ToList();
			var list = (from area in _context.Using<AreasMaster>().GetByCondition(x => x.IsActive == true).ToList()
						join city in _context.Using<City>().GetByCondition(x => x.IsActive == true).ToList()
						  on area.CityId equals city.Id into cityGroup
						from city in cityGroup.DefaultIfEmpty() // LEFT JOIN
						orderby area.Id
						select new SelectListItem_Custom(area.Id.ToString(), area.Name + (city != null ? ", " + city.Name : ""), "L")).Distinct().ToList();

			if (list != null && list.Count() > 0) CommonViewModel.SelectListItems.AddRange(list);

			list = _context.Using<PropertyCategory>().GetByCondition(x => x.IsActive == true).OrderBy(x => x.Name)
						.Select(x => new SelectListItem_Custom(x.Id.ToString(), x.Name, "PC")).Distinct().ToList();

			if (list != null && list.Count() > 0) CommonViewModel.SelectListItems.AddRange(list);

			list = _context.Using<PropertyType>().GetByCondition(x => x.IsActive == true).OrderBy(x => x.Name)
						.Select(x => new SelectListItem_Custom(x.Id.ToString(), x.Name, x.ParentId.ToString(), x.ImagePath, "PT")).Distinct().ToList();

			if (list != null && list.Count() > 0) CommonViewModel.SelectListItems.AddRange(list);

			//CommonViewModel.Data1 = (from parent in _context.Using<PropertyType>().GetByCondition(x => x.IsActive == true).ToList()
			//						 where parent.IsActive && parent.ParentId == 0
			//						 select new
			//						 {
			//							 TypeId = parent.Id,
			//							 TypeName = parent.Name,
			//							 ImagePath = parent.ImagePath,
			//							 Display_Seq_No = parent.Display_Seq_No == null?0: parent.Display_Seq_No,
			//							 PropertyCount = _context.Using<Properties>().GetByCondition(x => x.IsActive == true).ToList()
			//							 .Count(p => (p.TypeId == parent.Id || _context.Using<PropertyType>().GetByCondition(x => x.IsActive == true)
			//							 .Any(c => c.ParentId == parent.Id && c.IsActive && c.Id == p.TypeId)) && p.IsActive)
			//						 }).ToList();

			var listObj = new List<PropertyType>();

			var dt = DataContext_Command.ExecuteStoredProcedure_DataTable("SP_Property_Get_Type_Wise", null);

			if (dt != null && dt.Rows.Count > 0)
				foreach (DataRow dr in dt.Rows)
					listObj.Add(new PropertyType()
					{
						Id = dr["Id"] != DBNull.Value ? Convert.ToInt64(dr["Id"]) : 0,
						Name = dr["Name"] != DBNull.Value ? Convert.ToString(dr["Name"]) : "",
						ImagePath = dr["ImagePath"] != DBNull.Value ? Convert.ToString(dr["ImagePath"]) : "",
						PropertyCount = dr["PropertyCount"] != DBNull.Value ? Convert.ToInt32(dr["PropertyCount"]) : 0,
						Display_Seq_No = dr["Display_Seq_No"] != DBNull.Value ? Convert.ToInt32(dr["Display_Seq_No"]) : 0
					});

			CommonViewModel.Data1 = listObj;

			return View(CommonViewModel);
		}

		public IActionResult Features()
		{
			return View();
		}

		public IActionResult AboutUs()
		{
			return View();
		}

		[HttpGet]
		public IActionResult PropertyCategoryType_Get(long CategoryId = 0, long TypeId = 0)
		{
			List<PropertyCategoryTypeMapping> data = DataContext_Command.PropertyCategoryType_Get(CategoryId).ToList();

			return Json(data);
		}


		[HttpGet]
		public IActionResult Properties()
		{
			var viewModel = new PropertySearch()
			{
				FirstName = Request.Headers["firstname"].ToString(),
				LastName = Request.Headers["lastname"].ToString(),
				Email = Request.Headers["email"].ToString(),
				ContactNo = Request.Headers["contactno"].ToString(),
				Location = Convert.ToInt32(Request.Headers["location"]),
				PropertyCategory = Convert.ToInt32(Request.Headers["propertycategory"]),
				PropertyType_Parent = Convert.ToInt32(Request.Headers["propertytype_parent"]),
				PropertyType = Convert.ToInt32(Request.Headers["propertytype"])
			};

			ResponseModel<Properties> responseModel = new ResponseModel<Properties>
			{
				ObjList = new List<Properties>(), //_context.Using<Properties>().GetByCondition(x => x.IsActive == true).OrderByDescending(x => x.Id).ToList();
				Data1 = viewModel
			};

			try
			{
				responseModel.SelectListItems = new List<SelectListItem_Custom>();

				var requirement = "";

				//var list = _context.Using<AreasMaster>().GetByCondition(x => x.Id > 1, x => x.City).Include(x => x.City).OrderBy(x => x.Id)
				//			.Select(x => new SelectListItem_Custom(x.Id.ToString(), x.Name + ", " + x.City.Name, "L")).Distinct().ToList();
				var list = (from area in _context.Using<AreasMaster>().GetByCondition(x => x.IsActive == true).ToList()
							join city in _context.Using<City>().GetByCondition(x => x.IsActive == true).ToList()
							  on area.CityId equals city.Id into cityGroup
							from city in cityGroup.DefaultIfEmpty() // LEFT JOIN
							orderby area.Id
							select new SelectListItem_Custom(area.Id.ToString(), area.Name + (city != null ? ", " + city.Name : ""), "L")).Distinct().ToList();

				if (list != null && list.Count() > 0) responseModel.SelectListItems.AddRange(list);

				list = _context.Using<PropertyCategory>().GetByCondition(x => x.IsActive == true).OrderBy(x => x.Name)
							.Select(x => new SelectListItem_Custom(x.Id.ToString(), x.Name, "PC")).Distinct().ToList();

				if (list != null && list.Count() > 0) responseModel.SelectListItems.AddRange(list);

				if (viewModel.PropertyCategory > 0)
					requirement = "For " + (string)list.Where(x => x.Value == viewModel.PropertyCategory.ToString()).Select(x => x.Text).FirstOrDefault();

				responseModel.IsSuccess = true;

				if (!string.IsNullOrEmpty(viewModel.Email) || !string.IsNullOrEmpty(viewModel.ContactNo))
				{
					var lead = new Lead()
					{
						Name = viewModel.FirstName + " " + viewModel.LastName,
						Email = viewModel.Email,
						Mobile = viewModel.ContactNo,
						Requirement = requirement,
						PreferredAreaId = viewModel.Location,
						PropertyType = viewModel.PropertyType_Parent,
						LeadSource_Value = (string)_context.Using<LovMaster>().GetByCondition(x => x.LovDesc.ToLower().Contains("website")).Select(x => x.LovCode).FirstOrDefault()
					};

					var (IsSuccess, response, Id) = DataContext_Command.Leads_Save(lead);
				}

				list = _context.Using<PropertyType>().GetByCondition(x => x.IsActive == true).OrderBy(x => x.Name)
							.Select(x => new SelectListItem_Custom(x.Id.ToString(), x.Name, x.ParentId.ToString(), 1, "PT")).Distinct().ToList();

				if (list != null && list.Count() > 0) responseModel.SelectListItems.AddRange(list);

				try
				{
					var parameters = new List<SqlParameter>();
					parameters.Add(new SqlParameter("CategoryId", SqlDbType.BigInt) { Value = viewModel.PropertyCategory, Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("TypeId", SqlDbType.BigInt) { Value = viewModel.PropertyType <= 0 ? viewModel.PropertyType_Parent : viewModel.PropertyType, Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("CityId", SqlDbType.BigInt) { Value = 0, Direction = ParameterDirection.Input, IsNullable = true });
					parameters.Add(new SqlParameter("AreaId", SqlDbType.BigInt) { Value = viewModel.Location, Direction = ParameterDirection.Input, IsNullable = true });

					var dt = DataContext_Command.ExecuteStoredProcedure_DataTable("SP_Property_Get_Filter", parameters.ToList());

					if (dt != null && dt.Rows.Count > 0)
						foreach (DataRow dr in dt.Rows)
							responseModel.ObjList.Add(new Properties()
							{
								Id = dr["Id"] != DBNull.Value ? Convert.ToInt64(dr["Id"]) : 0,
								Title = dr["Title"] != DBNull.Value ? Convert.ToString(dr["Title"]) : "",
								Description = dr["Description"] != DBNull.Value ? Convert.ToString(dr["Description"]) : "",
								CityId = dr["CityId"] != DBNull.Value ? Convert.ToInt64(dr["CityId"]) : 0,
								AreaId = dr["AreaId"] != DBNull.Value ? Convert.ToInt64(dr["AreaId"]) : 0,
								Landmark = dr["Landmark"] != DBNull.Value ? Convert.ToString(dr["Landmark"]) : "",
								CategoryId = dr["CategoryId"] != DBNull.Value ? Convert.ToInt64(dr["CategoryId"]) : 0,
								TypeId = dr["TypeId"] != DBNull.Value ? Convert.ToInt64(dr["TypeId"]) : 0,
								Property_Type = dr["Property_Type"] != DBNull.Value ? Convert.ToString(dr["Property_Type"]) : "",
								Property_Category = dr["Property_Category"] != DBNull.Value ? Convert.ToString(dr["Property_Category"]) : "",
								Price = dr["Price"] != DBNull.Value ? Convert.ToDecimal(dr["Price"]) : 0,
								AreaSqft = dr["AreaSqft"] != DBNull.Value ? Convert.ToDecimal(dr["AreaSqft"]) : 0,
								OwnerName = dr["OwnerName"] != DBNull.Value ? Convert.ToString(dr["OwnerName"]) : "",
								OwnerMobile = dr["OwnerMobile"] != DBNull.Value ? Convert.ToString(dr["OwnerMobile"]) : "",
								BuilderName = dr["BuilderName"] != DBNull.Value ? Convert.ToString(dr["BuilderName"]) : "",
								FloorNo = dr["FloorNo"] != DBNull.Value ? Convert.ToInt32(dr["FloorNo"]) : 0,
								TotalFloors = dr["TotalFloors"] != DBNull.Value ? Convert.ToInt32(dr["TotalFloors"]) : 0,
								Facing = dr["Facing"] != DBNull.Value ? Convert.ToString(dr["Facing"]) : "",
								FurnishingStatus = dr["FurnishingStatus"] != DBNull.Value ? Convert.ToString(dr["FurnishingStatus"]) : "",
								FurnishingStatus_TEXT = dr["FurnishingStatus_TEXT"] != DBNull.Value ? Convert.ToString(dr["FurnishingStatus_TEXT"]) : "",
								AvailabilityStatus = dr["AvailabilityStatus"] != DBNull.Value ? Convert.ToString(dr["AvailabilityStatus"]) : "",
								AvailabilityStatus_TEXT = dr["AvailabilityStatus_TEXT"] != DBNull.Value ? Convert.ToString(dr["AvailabilityStatus_TEXT"]) : "",
								City_Name = dr["City_Name"] != DBNull.Value ? Convert.ToString(dr["City_Name"]) : "",
								Area_Name = dr["Area_Name"] != DBNull.Value ? Convert.ToString(dr["Area_Name"]) : "",
								Remark = dr["Remark"] != DBNull.Value ? Convert.ToString(dr["Remark"]) : "",
								IsFeatured = dr["IsFeatured"] != DBNull.Value ? Convert.ToBoolean(dr["IsFeatured"]) : false,
								IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false,
								ImagePath = dr["ImagePath"] != DBNull.Value ? Convert.ToString(dr["ImagePath"]) : ""
							});
				}
				catch (Exception ex) { /*LogService.LogInsert(GetCurrentAction(), "", ex);*/ }

				return View(responseModel);
			}
			catch (Exception ex) { responseModel.IsSuccess = false; }

			return BadRequest(responseModel);
		}

		public IActionResult ContactUs()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
