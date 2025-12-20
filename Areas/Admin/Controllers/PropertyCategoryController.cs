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
    public class PropertyCategoryController : BaseController<ResponseModel<PropertyCategory>>
    {
        public PropertyCategoryController(IRepositoryWrapper repository) : base(repository) { }

        // GET: Admin/Role
        public ActionResult Index()
        {
            CommonViewModel.ObjList = new List<PropertyCategory>();
            CommonViewModel.ObjList = DataContext_Command.PropertyCategory_Get(0).ToList();

            return View(CommonViewModel);
        }

        //[CustomAuthorizeAttribute(AccessType_Enum.Read)]
        public ActionResult Partial_AddEditForm(long Id = 0)
        {
            CommonViewModel.Obj = new PropertyCategory();

            if (Id > 0)
            {
                CommonViewModel.Obj = DataContext_Command.PropertyCategory_Get(Id).FirstOrDefault();
            }           


           
            return PartialView("_Partial_AddEditForm", CommonViewModel);
        }

        [HttpPost]
        //[CustomAuthorizeAttribute(AccessType_Enum.Write)]
        public ActionResult Save(PropertyCategory viewModel)
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
                        CommonViewModel.Message = "Please enter property category name.";

                        return Json(CommonViewModel);
                    }

                    

                    #endregion

                    #region Database-Transaction

                   
                      
                            

                            var (IsSuccess, response, Id) = DataContext_Command.PropertyCategory_Save(viewModel);
                            viewModel.Id = Id;

                            CommonViewModel.IsConfirm = IsSuccess;
                            CommonViewModel.IsSuccess = IsSuccess;
                            CommonViewModel.StatusCode = IsSuccess ? ResponseStatusCode.Success : ResponseStatusCode.Error;
                            CommonViewModel.Message = response;
                            CommonViewModel.RedirectURL = Url.Action("Index", "PropertyCategory", new { area = "Admin" });

                            //Employee obj = null;

                            ////if (viewModel != null && !(viewModel.DisplayOrder > 0))
                            ////	viewModel.DisplayOrder = (_context.Companies.AsNoTracking().Max(x => x.DisplayOrder) ?? 0) + 1;

                            //if (obj != null && Common.IsAdmin())
                            //{
                            //	obj.FirstName = viewModel.FirstName;
                            //	obj.MiddleName = viewModel.MiddleName;
                            //	obj.LastName = viewModel.LastName;
                            //	obj.UserType = viewModel.UserType;
                            //	obj.IsActive = (obj.Id == Common.LoggedUser_EmployeeId()) ? true : viewModel.IsActive;

                            //	_context.Entry(obj).State = EntityState.Modified;
                            //}
                            //else if (Common.IsAdmin())
                            //	_context.Employees.Add(viewModel);

                            //_context.SaveChanges();


                            //CommonViewModel.IsConfirm = true;
                            //CommonViewModel.IsSuccess = true;
                            //CommonViewModel.StatusCode = ResponseStatusCode.Success;
                            //CommonViewModel.Message = ResponseStatusMessage.Success;
                            //CommonViewModel.RedirectURL = Url.Action("Index", "Employee", new { area = "Admin" });

                            

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
        public ActionResult DeleteConfirmed(long Id)
        {
            try
            {
                //if (Common.IsAdmin() && !_context.Using<UserRoleMapping>().Any(x => x.EmployeeId == Id)
                //	&& _context.Employees.Any(x => x.Id > 1 && x.Id == Id))
                if (true)
                {
                    var (IsSuccess, response) = DataContext_Command.PropertyCategory_Delete(Id);

                    CommonViewModel.IsConfirm = IsSuccess;
                    CommonViewModel.IsSuccess = IsSuccess;
                    CommonViewModel.StatusCode = IsSuccess ? ResponseStatusCode.Success : ResponseStatusCode.Error;
                    CommonViewModel.Message = response;
                    CommonViewModel.RedirectURL = Url.Action("Index", "PropertyCategory", new { area = "Admin" });

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
