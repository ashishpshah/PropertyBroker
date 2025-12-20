using Microsoft.AspNetCore.Mvc;

using Broker.Models;
using Broker.Infra;
using System.Data;
using Microsoft.Data.SqlClient;
using Broker.Controllers;


namespace Broker.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AreasMasterController : BaseController<ResponseModel<AreasMaster>>
    {
        public AreasMasterController(IRepositoryWrapper repository) : base(repository) { }
        public IActionResult Index(int cityId = 0)
        {
            try
            {
                var dt = new DataTable();
                CommonViewModel.ObjList = new List<AreasMaster>();
                CommonViewModel.Obj = new AreasMaster() { CityId = cityId };
                List<SqlParameter> oParam = new List<SqlParameter>();
                oParam.Add(new SqlParameter("@AreaId", SqlDbType.BigInt) { Value = 0 });
                oParam.Add(new SqlParameter("@CityId", SqlDbType.BigInt) { Value = cityId });

                dt = DataContext_Command.ExecuteStoredProcedure_DataTable("SP_Areas_Get", oParam, true);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        CommonViewModel.ObjList.Add(new AreasMaster()
                        {
                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt64(dr["Id"]) : 0,
                            AreaName = dr["Name"] != DBNull.Value ? Convert.ToString(dr["Name"]) : "",
                            CityId = dr["CityId"] != DBNull.Value ? Convert.ToInt64(dr["CityId"]) : cityId,
                            IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false
                        });
                    }
                }
            }
            catch (Exception ex) { LogService.LogInsert(GetCurrentAction(), "", ex); }

            var CityName = DataContext_Command.ExecuteQuery("SELECT Name FROM Cities WHERE Id =" + cityId);
            CommonViewModel.Obj.CityName = CityName.Rows.Count > 0 ? Convert.ToString(CityName.Rows[0]["Name"]) : "";
            return View(CommonViewModel);
        }

        [HttpGet]

        public IActionResult Partial_AddEditForm(int cityId = 0 ,int Id = 0)
        {

            var obj = new AreasMaster() { CityId = cityId};

            var dt = new DataTable();

            var list = new List<SelectListItem_Custom>();
            List<SqlParameter> oParam = new List<SqlParameter>();

            try
            {
                if (Id > 0)
                {
                    oParam.Add(new SqlParameter("@AreaId", SqlDbType.BigInt) { Value = Id == 0 ? null : Id });
                    oParam.Add(new SqlParameter("@CityId", SqlDbType.BigInt) { Value = cityId});

                    dt = DataContext_Command.ExecuteStoredProcedure_DataTable("SP_Areas_Get", oParam, true);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        obj = new AreasMaster()
                        {
                            Id = dt.Rows[0]["Id"] != DBNull.Value ? Convert.ToInt64(dt.Rows[0]["Id"]) : 0,
                            AreaName = dt.Rows[0]["Name"] != DBNull.Value ? Convert.ToString(dt.Rows[0]["Name"]) : "",
                            CityId = dt.Rows[0]["CityId"] != DBNull.Value ? Convert.ToInt64(dt.Rows[0]["CityId"]) : cityId,
                            IsActive = dt.Rows[0]["IsActive"] != DBNull.Value ? Convert.ToBoolean(dt.Rows[0]["IsActive"]) : false
                        };
                    }
                }
            }
            catch (Exception ex) { LogService.LogInsert(GetCurrentAction(), "", ex); }
            CommonViewModel.SelectListItems = list;
            CommonViewModel.Obj = obj;
            var CityName = DataContext_Command.ExecuteQuery("SELECT Name FROM Cities WHERE Id =" + cityId);
            CommonViewModel.Obj.CityName = CityName.Rows.Count > 0 ? Convert.ToString(CityName.Rows[0]["Name"]) : "";
            return PartialView("_PartialAddEditForm", CommonViewModel);
        }
        [HttpPost]

        public JsonResult Save(AreasMaster viewModel)
        {
            try
            {
                if (string.IsNullOrEmpty(viewModel.AreaName))
                {
                    CommonViewModel.IsSuccess = false;
                    CommonViewModel.StatusCode = ResponseStatusCode.Error;
                    CommonViewModel.Message = "Please enter Area Name.";

                    return Json(CommonViewModel);
                }

                var (IsSuccess, response, Id) = (false, ResponseStatusMessage.Error, 0M);
                List<SqlParameter> oParams = new List<SqlParameter>();

                oParams.Add(new SqlParameter("@AreaId", SqlDbType.BigInt) { Value = viewModel.Id });
                oParams.Add(new SqlParameter("@AreaName", SqlDbType.VarChar) { Value = viewModel.AreaName ?? "" });
                oParams.Add(new SqlParameter("@CityId", SqlDbType.BigInt) { Value = viewModel.CityId });
                oParams.Add(new SqlParameter("@IsActive", SqlDbType.Bit) { Value = viewModel.IsActive });
                oParams.Add(new SqlParameter("@Operated_By", SqlDbType.BigInt) { Value = AppHttpContextAccessor.GetSession(SessionKey.KEY_USER_ID) });
                oParams.Add(new SqlParameter("@Action", SqlDbType.VarChar) { Value = viewModel.Id == 0 ? "INSERT" : "UPDATE" });

                (IsSuccess, response, Id) = DataContext_Command.ExecuteStoredProcedure("SP_Areas_Save", oParams, true);

                CommonViewModel.IsConfirm = true;
                CommonViewModel.IsSuccess = IsSuccess;
                CommonViewModel.StatusCode = IsSuccess ? ResponseStatusCode.Success : ResponseStatusCode.Error;
                CommonViewModel.Message = response;
                CommonViewModel.RedirectURL = IsSuccess ? Url.Content("~/") + GetCurrentControllerUrl() + "/Index?cityId= " + viewModel.CityId : "";

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
        public ActionResult DeleteConfirmed(long Id = 0 , long CityId = 0)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@AreaId", SqlDbType.Int) { Value = Id, Direction = ParameterDirection.Input });
            parameters.Add(new SqlParameter("@CityId", SqlDbType.Int) { Value = CityId, Direction = ParameterDirection.Input });
            parameters.Add(new SqlParameter("@OperatedBy", SqlDbType.Int) { Value = AppHttpContextAccessor.GetSession(SessionKey.KEY_USER_ID), Direction = ParameterDirection.Input });

            var response = DataContext_Command.ExecuteStoredProcedure("sp_Areas_Delete", parameters.ToArray());

            var msgtype = response;

            if (msgtype.Contains("S"))
            {
                CommonViewModel.IsConfirm = true;
                CommonViewModel.IsSuccess = true;
                CommonViewModel.Message = msgtype;
                CommonViewModel.StatusCode = ResponseStatusCode.Success;
                CommonViewModel.RedirectURL = Url.Action("Index", "AreasMaster");
                return Json(CommonViewModel);
            }
            CommonViewModel.IsConfirm = true;
            CommonViewModel.IsSuccess = false;
            CommonViewModel.StatusCode = ResponseStatusCode.Error;
            return Json(CommonViewModel);
        }
    }
}
