using Broker.Controllers;
using Broker.Infra;
using Broker.Models;
using Microsoft.AspNetCore.Mvc;

namespace Broker.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LeadFollowUpController : BaseController<ResponseModel<LeadFollowup>>
    {
        public LeadFollowUpController(IRepositoryWrapper repository) : base(repository) { }
        public ActionResult Index(long Id = 0 , string Status = "")
        {
            CommonViewModel.ObjList = new List<LeadFollowup>();
            CommonViewModel.Obj = new LeadFollowup() { LeadId = Id , Status = Status };
            CommonViewModel.ObjList = DataContext_Command.LeadFollowUp_Get(0, Id).ToList();

            var dt = DataContext_Command.ExecuteQuery("select Name from Leads where Id=" + Id);

            CommonViewModel.Obj.Name = dt.Rows[0]["Name"].ToString();

            return View(CommonViewModel);
        }
        public ActionResult Partial_AddEditForm(long Id = 0, long LeadId = 0 , string Status = "")
        {
            CommonViewModel.Obj = new LeadFollowup() { LeadId = LeadId  , Status = Status, NextFollowupDate = DateTime.Today.AddDays(7) };

            if (Id > 0)
            {
                CommonViewModel.Obj = DataContext_Command.LeadFollowUp_Get(Id, LeadId , Status).FirstOrDefault();
            }



            return PartialView("_Partial_AddEditForm", CommonViewModel);
        }
       
        [HttpPost]
        //[CustomAuthorizeAttribute(AccessType_Enum.Write)]
        public ActionResult Save(LeadFollowup viewModel)
        {
            try
            {
                if (viewModel != null && viewModel != null)
                {
                    #region Validation

                    //if (!Common.IsAdmin())
                    //{
                    //	CommonViewModel.IsSuccess = false;
                    //	CommonViewModel.StatusCode = ResponseStatusCode.Error;
                    //	CommonViewModel.Message = ResponseStatusMessage.UnAuthorize;

                    //	return Json(CommonViewModel);
                    //}

                    if (viewModel.NextFollowupDate == null)
                    {
                        CommonViewModel.IsSuccess = false;
                        CommonViewModel.StatusCode = ResponseStatusCode.Error;
                        CommonViewModel.Message = "Please enter next follow up date";

                        return Json(CommonViewModel);
                    }
                    if (viewModel.ReminderDatetime == null)
                    {
                        CommonViewModel.IsSuccess = false;
                        CommonViewModel.StatusCode = ResponseStatusCode.Error;
                        CommonViewModel.Message = "Please enter reminder date";

                        return Json(CommonViewModel);
                    }
                    if(viewModel.ReminderDatetime > viewModel.NextFollowupDate)
                    {
                        CommonViewModel.IsSuccess = false;
                        CommonViewModel.StatusCode = ResponseStatusCode.Error;
                        CommonViewModel.Message = "The reminder date must be earlier than or equal to the next follow-up date.";

                        return Json(CommonViewModel);
                    }

                    #endregion

                    #region Database-Transaction

                    var (IsSuccess, response, Id) = DataContext_Command.LeadFollowUp_Save(viewModel);
                    viewModel.Id = Id;

                    CommonViewModel.IsConfirm = IsSuccess;
                    CommonViewModel.IsSuccess = IsSuccess;
                    CommonViewModel.StatusCode = IsSuccess ? ResponseStatusCode.Success : ResponseStatusCode.Error;
                    CommonViewModel.Message = response;
                    CommonViewModel.RedirectURL = Url.Action("Index", "LeadFollowUp", new { area = "Admin", Id = viewModel.LeadId,Status = viewModel.Status});








                    return Json(CommonViewModel);



                    #endregion
                }
            }
            catch (Exception ex) { }

            CommonViewModel.Message = ResponseStatusMessage.Error;
            CommonViewModel.IsSuccess = false;
            CommonViewModel.StatusCode = ResponseStatusCode.Error;

            return Json(CommonViewModel);
        }
        [HttpPost]
        //[CustomAuthorizeAttribute(AccessType_Enum.Delete)]
        public ActionResult LeadStatus_Change(long Id = 0 , string Remarks = "" , string Status = "")
        {
            try
            {
                //if (Common.IsAdmin() && !_context.Using<UserRoleMapping>().Any(x => x.EmployeeId == Id)
                //	&& _context.Employees.Any(x => x.Id > 1 && x.Id == Id))

                //if (string.IsNullOrEmpty(Remarks))
                //{
                //    CommonViewModel.IsSuccess = false;
                //    CommonViewModel.StatusCode = ResponseStatusCode.Error;
                //    CommonViewModel.Message = "Please enter remarks";

                //    return Json(CommonViewModel);
                //}
                if (true)
                {
                    var (IsSuccess, response) = DataContext_Command.Lead_Status_Change(Id, Remarks,Status);

                    CommonViewModel.IsConfirm = IsSuccess;
                    CommonViewModel.IsSuccess = IsSuccess;
                    CommonViewModel.StatusCode = IsSuccess ? ResponseStatusCode.Success : ResponseStatusCode.Error;
                    CommonViewModel.Message = response;
                    CommonViewModel.RedirectURL = Url.Action("Index", "LeadFollowUp", new { area = "Admin", Id = Id});

                    //var obj = _context.Employees.GetByCondition(x => x.Id == Id).FirstOrDefault();

                    //_context.Entry(obj).State = EntityState.Deleted;
                    //_context.SaveChanges();

                    //CommonViewModel.IsConfirm = true;
                    //CommonViewModel.IsSuccess = true;
                    //CommonViewModel.StatusCode = ResponseStatusCode.Success;
                    //CommonViewModel.Message = ResponseStatusMessage.Delete;

                    //CommonViewModel.RedirectURL = Url.Action("Index", "Employee", new { area = "Admin" });

                    return Json(CommonViewModel);
                }
            }
            catch (Exception ex) { }

            CommonViewModel.IsSuccess = false;
            CommonViewModel.StatusCode = ResponseStatusCode.Error;
            CommonViewModel.Message = ResponseStatusMessage.Unable_Delete;

            return Json(CommonViewModel);
        }
    }
}
