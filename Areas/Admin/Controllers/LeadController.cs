using Broker.Controllers;
using Broker.Infra;
using Broker.Infra.Services;
using Broker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Broker.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LeadController : BaseController<ResponseModel<Lead>>
    {
        public LeadController(IRepositoryWrapper repository) : base(repository) { }
        public ActionResult Index(string Status = "NEW")
        {
            CommonViewModel.ObjList = new List<Lead>();
            CommonViewModel.Obj = new Lead() { Selected_Status = Status};      
            CommonViewModel.ObjList = DataContext_Command.Lead_Get(0 , Status).ToList();
            
            var list = new List<SelectListItem_Custom>();
            var dt = new DataTable();
            var sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Lov_Column", SqlDbType.VarChar) { Value = "LEADSTATUS" });
            dt = DataContext_Command.ExecuteStoredProcedure_DataTable("SP_Leads_Status_Combo", sqlParameters, true);

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {

                    list.Add(new SelectListItem_Custom(Convert.ToString(dr["Lov_Code"]), Convert.ToString(dr["Lov_Desc"]),  Convert.ToString(dr["Lead_Count"]) , Convert.ToString(dr["DisplayOrder"]) , Convert.ToString(dr["Lov_Column"]))
                    {
                        Value = dr["Lov_Code"] != DBNull.Value ? Convert.ToString(dr["Lov_Code"]) : "",
                        Text = dr["Lov_Desc"] != DBNull.Value ? Convert.ToString(dr["Lov_Desc"]) : "",
                        Group = dr["Lov_Column"] != DBNull.Value ? Convert.ToString(dr["Lov_Column"]) : "",
                        Value2 = dr["Lead_Count"] != DBNull.Value ? Convert.ToString(dr["Lead_Count"]) : "",
                        Value3 = dr["DisplayOrder"] != DBNull.Value ? Convert.ToString(dr["DisplayOrder"]) : "",
                    });
                }
            }

            CommonViewModel.SelectListItems = list;
            return View(CommonViewModel);
        }


        //[CustomAuthorizeAttribute(AccessType_Enum.Read)]
        public ActionResult Partial_AddEditForm(long Id = 0)
        {
            CommonViewModel.Obj = new Lead() { Preferred_City_Id = 0, Preferred_Area_Id = 0, PropertyType = 0, LeadSource_Value = "0" };
            var list = new List<SelectListItem_Custom>();
            var dt = new DataTable();
            var sqlParameters = new List<SqlParameter>();
            if (Id > 0)
            {
                CommonViewModel.Obj = DataContext_Command.Lead_Get(Id).FirstOrDefault();

                dt = new DataTable();
                sqlParameters = new List<SqlParameter>();
                sqlParameters.Add(new SqlParameter("@Id", SqlDbType.VarChar) { Value = CommonViewModel.Obj.Preferred_City_Id });
                dt = DataContext_Command.ExecuteStoredProcedure_DataTable("SP_GetAreaList", sqlParameters, true);

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        list.Add(new SelectListItem_Custom(Convert.ToString(dr["Id"]), Convert.ToString(dr["Name"]), "Area")
                        {
                            Value = dr["Id"] != DBNull.Value ? Convert.ToString(dr["Id"]) : "",
                            Text = dr["Name"] != DBNull.Value ? Convert.ToString(dr["Name"]) : "",

                        });
                    }
                }
            }

          
            dt = new DataTable();
            //sqlParameters = new List<SqlParameter>();
            //sqlParameters.Add(new SqlParameter("@Id", SqlDbType.VarChar) { Value = 0 });
            dt = DataContext_Command.ExecuteStoredProcedure_DataTable("SP_Property_Type_Combo", null, true);

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new SelectListItem_Custom(Convert.ToString(dr["TypeId"]), Convert.ToString(dr["PropertyType"]), "PropertyType")
                    {
                        Value = dr["TypeId"] != DBNull.Value ? Convert.ToString(dr["TypeId"]) : "",
                        Text = dr["PropertyType"] != DBNull.Value ? Convert.ToString(dr["PropertyType"]) : "",

                    });
                }
            }

            dt = new DataTable();
            sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Lov_Column", SqlDbType.VarChar) { Value = "LEADSOURCE" });
            dt = DataContext_Command.ExecuteStoredProcedure_DataTable("SP_Multiple_Lov_Combo", sqlParameters, true);

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new SelectListItem_Custom(Convert.ToString(dr["Lov_Code"]), Convert.ToString(dr["Lov_Desc"]), Convert.ToString(dr["Lov_Column"]))
                    {
                        Value = dr["Lov_Code"] != DBNull.Value ? Convert.ToString(dr["Lov_Code"]) : "",
                        Text = dr["Lov_Desc"] != DBNull.Value ? Convert.ToString(dr["Lov_Desc"]) : "",
                        Group = dr["Lov_Column"] != DBNull.Value ? Convert.ToString(dr["Lov_Column"]) : "",
                    });
                }
            }

            dt = new DataTable();
            sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Id", SqlDbType.VarChar) { Value = 0 });
            dt = DataContext_Command.ExecuteStoredProcedure_DataTable("SP_Cities_Get", null, true);

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new SelectListItem_Custom(Convert.ToString(dr["Id"]), Convert.ToString(dr["Name"]), "City")
                    {
                        Value = dr["Id"] != DBNull.Value ? Convert.ToString(dr["Id"]) : "",
                        Text = dr["Name"] != DBNull.Value ? Convert.ToString(dr["Name"]) : "",

                    });
                }
            }

            CommonViewModel.SelectListItems = list;





            return PartialView("_Partial_AddEditForm", CommonViewModel);
        }
        public IActionResult GetArea(long Id = 0)
        {
            var list = new List<SelectListItem_Custom>();

            List<SqlParameter> oParams = new List<SqlParameter>();

            var dt = new DataTable();

            try
            {


                oParams = new List<SqlParameter>();
                oParams.Add(new SqlParameter("@Id", SqlDbType.BigInt) { Value = Id });

                dt = DataContext_Command.ExecuteStoredProcedure_DataTable("SP_GetAreaList", oParams);
                if (dt != null && dt.Rows.Count > 0)
                    foreach (DataRow dr in dt.Rows)
                        list.Add(new SelectListItem_Custom(Convert.ToString(dr["Id"]), Convert.ToString(dr["Name"]), "Area")
                        {
                            Value = dr["Id"] != DBNull.Value ? Convert.ToString(dr["Id"]) : "",
                            Text = dr["Name"] != DBNull.Value ? Convert.ToString(dr["Name"]) : "",

                        });





            }
            catch (Exception ex) { LogService.LogInsert(GetCurrentAction(), "", ex); }

            return Json(list);
        }

        [HttpPost]
        //[CustomAuthorizeAttribute(AccessType_Enum.Write)]
        public ActionResult Save(Lead viewModel)
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

                    if (string.IsNullOrEmpty(viewModel.Mobile))
                    {
                        CommonViewModel.IsSuccess = false;
                        CommonViewModel.StatusCode = ResponseStatusCode.Error;
                        CommonViewModel.Message = "Please enter mobile no.";

                        return Json(CommonViewModel);
                    }
                    if (!string.IsNullOrEmpty(viewModel.Mobile) && !ValidateField.IsValidMobileNo_D10(viewModel.Mobile))
                    {
                        CommonViewModel.IsSuccess = false;
                        CommonViewModel.StatusCode = ResponseStatusCode.Error;
                        CommonViewModel.Message = "Please enter valid 10 digit owner mobile no.";

                        return Json(CommonViewModel);
                    }
                    if (string.IsNullOrEmpty(viewModel.Email))
                    {
                        CommonViewModel.IsSuccess = false;
                        CommonViewModel.StatusCode = ResponseStatusCode.Error;
                        CommonViewModel.Message = "Please enter email.";

                        return Json(CommonViewModel);
                    }
                    if (!string.IsNullOrEmpty(viewModel.Email) && !ValidateField.IsValidEmail(viewModel.Email))
                    {
                        CommonViewModel.IsSuccess = false;
                        CommonViewModel.StatusCode = ResponseStatusCode.Error;
                        CommonViewModel.Message = "Please enter valid email.";

                        return Json(CommonViewModel);
                    }
                    if (string.IsNullOrEmpty(viewModel.Requirement))
                    {
                        CommonViewModel.IsSuccess = false;
                        CommonViewModel.StatusCode = ResponseStatusCode.Error;
                        CommonViewModel.Message = "Please enter requirement.";

                        return Json(CommonViewModel);
                    }

                    if (viewModel.Preferred_City_Id == 0)
                    {
                        CommonViewModel.IsSuccess = false;
                        CommonViewModel.StatusCode = ResponseStatusCode.Error;
                        CommonViewModel.Message = "Please select preferred city";

                        return Json(CommonViewModel);
                    }
                    if (viewModel.Preferred_Area_Id == 0)
                    {
                        CommonViewModel.IsSuccess = false;
                        CommonViewModel.StatusCode = ResponseStatusCode.Error;
                        CommonViewModel.Message = "Please select preferred area.";

                        return Json(CommonViewModel);
                    }                  
                    if (viewModel.PropertyType == 0)
                    {
                        CommonViewModel.IsSuccess = false;
                        CommonViewModel.StatusCode = ResponseStatusCode.Error;
                        CommonViewModel.Message = "Please select property type.";

                        return Json(CommonViewModel);
                    }
                    if (viewModel.LeadSource_Value == "0")
                    {
                        CommonViewModel.IsSuccess = false;
                        CommonViewModel.StatusCode = ResponseStatusCode.Error;
                        CommonViewModel.Message = "Please select lead source.";

                        return Json(CommonViewModel);
                    }                   
                    #endregion

                    #region Database-Transaction

                    var (IsSuccess, response, Id) = DataContext_Command.Leads_Save(viewModel);
                    viewModel.Id = Id;

                    CommonViewModel.IsConfirm = IsSuccess;
                    CommonViewModel.IsSuccess = IsSuccess;
                    CommonViewModel.StatusCode = IsSuccess ? ResponseStatusCode.Success : ResponseStatusCode.Error;
                    CommonViewModel.Message = response;
                    CommonViewModel.RedirectURL = Url.Action("Index", "Lead", new { area = "Admin" });








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

        //[HttpPost]
        ////[CustomAuthorizeAttribute(AccessType_Enum.Delete)]
        //public ActionResult DeleteConfirmed(long Id, long ParentId = 0)
        //{
        //    try
        //    {
        //        //if (Common.IsAdmin() && !_context.Using<UserRoleMapping>().Any(x => x.EmployeeId == Id)
        //        //	&& _context.Employees.Any(x => x.Id > 1 && x.Id == Id))
        //        if (true)
        //        {
        //            var (IsSuccess, response) = DataContext_Command.PropertyType_Delete(Id, ParentId);

        //            CommonViewModel.IsConfirm = IsSuccess;
        //            CommonViewModel.IsSuccess = IsSuccess;
        //            CommonViewModel.StatusCode = IsSuccess ? ResponseStatusCode.Success : ResponseStatusCode.Error;
        //            CommonViewModel.Message = response;
        //            CommonViewModel.RedirectURL = Url.Action("Index", "Property", new { area = "Admin" });

        //            //var obj = _context.Employees.GetByCondition(x => x.Id == Id).FirstOrDefault();

        //            //_context.Entry(obj).State = EntityState.Deleted;
        //            //_context.SaveChanges();

        //            //CommonViewModel.IsConfirm = true;
        //            //CommonViewModel.IsSuccess = true;
        //            //CommonViewModel.StatusCode = ResponseStatusCode.Success;
        //            //CommonViewModel.Message = ResponseStatusMessage.Delete;

        //            //CommonViewModel.RedirectURL = Url.Action("Index", "Employee", new { area = "Admin" });

        //            return Json(CommonViewModel);
        //        }
        //    }
        //    catch (Exception ex) { }

        //    CommonViewModel.IsSuccess = false;
        //    CommonViewModel.StatusCode = ResponseStatusCode.Error;
        //    CommonViewModel.Message = ResponseStatusMessage.Unable_Delete;

        //    return Json(CommonViewModel);
        //}
    }
}
