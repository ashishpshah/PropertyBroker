using Microsoft.AspNetCore.Mvc;

using Broker.Models;
using Broker.Infra;
using System.Data;
using Microsoft.Data.SqlClient;
using Broker.Controllers;

namespace Broker.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PropertyAmenityController : BaseController<ResponseModel<PropertyAmenity>>
    {
        public PropertyAmenityController(IRepositoryWrapper repository) : base(repository) { }
        public IActionResult Index()
        {
            try
            {
                var dt = new DataTable();
                CommonViewModel.ObjList = new List<PropertyAmenity>();
                CommonViewModel.Obj = new PropertyAmenity();
                List<SqlParameter> oParam = new List<SqlParameter>();
                oParam.Add(new SqlParameter("@AmenityId", SqlDbType.BigInt) { Value = 0 });

                dt = DataContext_Command.ExecuteStoredProcedure_DataTable("SP_PropertyAmenities_Get", oParam, true);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        CommonViewModel.ObjList.Add(new PropertyAmenity()
                        {
                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt64(dr["Id"]) : 0,
                            Name = dr["Name"] != DBNull.Value ? Convert.ToString(dr["Name"]) : "",
                            DisplayOrder = dr["DisplayOrder"] != DBNull.Value ? Convert.ToInt32(dr["DisplayOrder"]) : 0,
                            IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false
                        });
                    }
                }
            }

            catch (Exception ex) { LogService.LogInsert(GetCurrentAction(), "", ex); }

            return View(CommonViewModel);
        }
        [HttpGet]

        public IActionResult Partial_AddEditForm(int Id = 0)
        {

            var obj = new PropertyAmenity();

            var dt = new DataTable();

            var list = new List<SelectListItem_Custom>();
            List<SqlParameter> oParam = new List<SqlParameter>();

            try
            {
                if (Id > 0)
                {
                    oParam.Add(new SqlParameter("@AmenityId", SqlDbType.BigInt) { Value = Id == 0 ? null : Id });

                    dt = DataContext_Command.ExecuteStoredProcedure_DataTable("SP_PropertyAmenities_Get", oParam, true);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        obj = new PropertyAmenity()
                        {
                            Id = dt.Rows[0]["Id"] != DBNull.Value ? Convert.ToInt64(dt.Rows[0]["Id"]) : 0,
                            DisplayOrder = dt.Rows[0]["DisplayOrder"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["DisplayOrder"]) : 0,
                            Name = dt.Rows[0]["Name"] != DBNull.Value ? Convert.ToString(dt.Rows[0]["Name"]) : "",
                            IsActive = dt.Rows[0]["IsActive"] != DBNull.Value ? Convert.ToBoolean(dt.Rows[0]["IsActive"]) : false
                        };
                    }
                }
            }
            catch (Exception ex) { LogService.LogInsert(GetCurrentAction(), "", ex); }
            CommonViewModel.SelectListItems = list;
            CommonViewModel.Obj = obj;
            return PartialView("_PartialAddEditForm", CommonViewModel);
        }

        public JsonResult Save(PropertyAmenity viewModel)
        {
            try
            {
                if (string.IsNullOrEmpty(viewModel.Name))
                {
                    CommonViewModel.IsSuccess = false;
                    CommonViewModel.StatusCode = ResponseStatusCode.Error;
                    CommonViewModel.Message = "Please enter Area Name.";

                    return Json(CommonViewModel);
                }

                var (IsSuccess, response, Id) = (false, ResponseStatusMessage.Error, 0M);
                List<SqlParameter> oParams = new List<SqlParameter>();

                oParams.Add(new SqlParameter("@AmenityId", SqlDbType.BigInt) { Value = viewModel.Id });
                oParams.Add(new SqlParameter("@AmenityName", SqlDbType.VarChar) { Value = viewModel.Name ?? "" });
                oParams.Add(new SqlParameter("@DisplayOrder", SqlDbType.BigInt) { Value = viewModel.DisplayOrder});
                oParams.Add(new SqlParameter("@IsActive", SqlDbType.Bit) { Value = viewModel.IsActive });
                oParams.Add(new SqlParameter("@Operated_By", SqlDbType.BigInt) { Value = AppHttpContextAccessor.GetSession(SessionKey.KEY_USER_ID) });
                oParams.Add(new SqlParameter("@Action", SqlDbType.VarChar) { Value = viewModel.Id == 0 ? "INSERT" : "UPDATE" });

                (IsSuccess, response, Id) = DataContext_Command.ExecuteStoredProcedure("SP_PropertyAmenities_Save", oParams, true);

                CommonViewModel.IsConfirm = true;
                CommonViewModel.IsSuccess = IsSuccess;
                CommonViewModel.StatusCode = IsSuccess ? ResponseStatusCode.Success : ResponseStatusCode.Error;
                CommonViewModel.Message = response;
                CommonViewModel.RedirectURL = IsSuccess ? Url.Content("~/") + GetCurrentControllerUrl() + "/Index" : "";

                return Json(CommonViewModel);
            }
            catch (Exception ex)
            {
                LogService.LogInsert(GetCurrentAction(), "", ex);

                CommonViewModel.IsSuccess = false;
                CommonViewModel.StatusCode = ResponseStatusCode.Error;
                CommonViewModel.Message = ResponseStatusMessage.Error + " | " + ex.Message;
            }
            return Json(CommonViewModel);
        }
        public ActionResult DeleteConfirmed(long Id = 0)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@AmenityId", SqlDbType.Int) { Value = Id, Direction = ParameterDirection.Input });
            parameters.Add(new SqlParameter("@Operated_By", SqlDbType.Int) { Value = AppHttpContextAccessor.GetSession(SessionKey.KEY_USER_ID), Direction = ParameterDirection.Input });

            var response = DataContext_Command.ExecuteStoredProcedure("sp_PropertyAmenities_Delete", parameters.ToArray());

            var msgtype = response;

            if (msgtype.Contains("S"))
            {
                CommonViewModel.IsConfirm = true;
                CommonViewModel.IsSuccess = true;
                CommonViewModel.Message = msgtype;
                CommonViewModel.StatusCode = ResponseStatusCode.Success;
                CommonViewModel.RedirectURL = Url.Action("Index", "PropertyAmenity");
                return Json(CommonViewModel);
            }
            CommonViewModel.IsConfirm = true;
            CommonViewModel.IsSuccess = false;
            CommonViewModel.StatusCode = ResponseStatusCode.Error;
            return Json(CommonViewModel);
        }
    }
}
