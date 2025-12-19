using Broker.Controllers;
using Broker.Infra;
using Broker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Broker.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PropertyTypesController : BaseController<ResponseModel<PropertyType>>
    {
        public PropertyTypesController(IRepositoryWrapper repository) : base(repository) { }

        // GET: Admin/Role
        public ActionResult Index()
        {
            CommonViewModel.ObjList = new List<PropertyType>();
            CommonViewModel.ObjList = DataContext_Command.PropertyType_Get(0).ToList();

            return View(CommonViewModel);
        }
        public ActionResult IndexSubType(long Parent_Id = 0)
        {
            CommonViewModel.ObjList = new List<PropertyType>();
            CommonViewModel.Obj = new PropertyType() { ParentId = Parent_Id};
            CommonViewModel.ObjList = DataContext_Command.Property_Sub_Type_Get(0 , Parent_Id).ToList();

            var dt  = DataContext_Command.ExecuteQuery("select Name from PropertyTypes where Id=" + Parent_Id);

            CommonViewModel.Obj.Name = dt.Rows[0]["Name"].ToString();

            return View(CommonViewModel);
        }

        //[CustomAuthorizeAttribute(AccessType_Enum.Read)]
        public ActionResult Partial_AddEditForm(long Id = 0)
        {
            CommonViewModel.Obj = new PropertyType();

            if (Id > 0)
            {
                CommonViewModel.Obj = DataContext_Command.PropertyType_Get(Id).FirstOrDefault();
            }           


           
            return PartialView("_Partial_AddEditForm", CommonViewModel);
        }
        public ActionResult Partial_AddEditForm_Sub_Type(long Id = 0 , long Parent_Id = 0)
        {
            CommonViewModel.Obj = new PropertyType() { ParentId = Parent_Id };

            if (Id > 0)
            {
                CommonViewModel.Obj = DataContext_Command.Property_Sub_Type_Get(Id, Parent_Id).FirstOrDefault();
            }



            return PartialView("_Partial_AddEditForm_SubType", CommonViewModel);
        }

        [HttpPost]
        //[CustomAuthorizeAttribute(AccessType_Enum.Write)]
        public ActionResult Save(PropertyType viewModel)
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

                    if (string.IsNullOrEmpty(viewModel.Name))
                    {
                        CommonViewModel.IsSuccess = false;
                        CommonViewModel.StatusCode = ResponseStatusCode.Error;
                        CommonViewModel.Message = "Please enter name.";

                        return Json(CommonViewModel);
                    }



                    #endregion

                    #region Database-Transaction

                    var (IsSuccess, response, Id) = DataContext_Command.PropertyType_Save(viewModel);
                    viewModel.Id = Id;

                    CommonViewModel.IsConfirm = IsSuccess;
                    CommonViewModel.IsSuccess = IsSuccess;
                    CommonViewModel.StatusCode = IsSuccess ? ResponseStatusCode.Success : ResponseStatusCode.Error;
                    CommonViewModel.Message = response;
                    CommonViewModel.RedirectURL = viewModel.ParentId > 0 ? Url.Action("IndexSubType", "PropertyTypes", new { area = "Admin", Parent_Id = viewModel.ParentId }) : Url.Action("Index", "PropertyTypes", new { area = "Admin" });








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
        public ActionResult DeleteConfirmed(long Id , long ParentId = 0)
        {
            try
            {
                //if (Common.IsAdmin() && !_context.Using<UserRoleMapping>().Any(x => x.EmployeeId == Id)
                //	&& _context.Employees.Any(x => x.Id > 1 && x.Id == Id))
                if (true)
                {
                    var (IsSuccess, response) = DataContext_Command.PropertyType_Delete(Id , ParentId);

                    CommonViewModel.IsConfirm = IsSuccess;
                    CommonViewModel.IsSuccess = IsSuccess;
                    CommonViewModel.StatusCode = IsSuccess ? ResponseStatusCode.Success : ResponseStatusCode.Error;
                    CommonViewModel.Message = response;
                    CommonViewModel.RedirectURL = ParentId > 0 ? Url.Action("IndexSubType", "PropertyTypes", new { area = "Admin", Parent_Id = ParentId }) : Url.Action("Index", "PropertyTypes", new { area = "Admin" });

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
