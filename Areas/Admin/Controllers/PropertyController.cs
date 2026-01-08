using Broker.Controllers;
using Broker.Infra;
using Broker.Infra.Services;
using Broker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Broker.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class PropertyController : BaseController<ResponseModel<Properties>>
	{
		public PropertyController(IRepositoryWrapper repository) : base(repository) { }

		public ActionResult Index()
		{
			CommonViewModel.ObjList = new List<Properties>();
			CommonViewModel.ObjList = DataContext_Command.Property_Get(0).ToList();

			return View(CommonViewModel);
		}

		public ActionResult Search()
		{
			var list = new List<SelectListItem_Custom>();
			var dt = new DataTable();
			var sqlParameters = new List<SqlParameter>();

			dt = new DataTable();
			sqlParameters = new List<SqlParameter>();
			sqlParameters.Add(new SqlParameter("@Id", SqlDbType.VarChar) { Value = 0 });
			dt = DataContext_Command.ExecuteStoredProcedure_DataTable("SP_PropertyCategories_Get", sqlParameters, true);

			if (dt != null && dt.Rows.Count > 0)
				foreach (DataRow dr in dt.Rows)
				{
					string name = Convert.ToString(dr["Name"]);
					name = name.Equals("Buy", StringComparison.OrdinalIgnoreCase) ? "Sell" : name.Equals("Sell", StringComparison.OrdinalIgnoreCase) ? "Buy" : name;

					list.Add(new SelectListItem_Custom(Convert.ToString(dr["Id"]), name, "PropertyCategory"));
				}


			dt = new DataTable();
			sqlParameters = new List<SqlParameter>();
			sqlParameters.Add(new SqlParameter("@Lov_Column", SqlDbType.VarChar) { Value = "FURNISHINGSTATUS" });
			dt = DataContext_Command.ExecuteStoredProcedure_DataTable("SP_Multiple_Lov_Combo", sqlParameters, true);

			if (dt != null && dt.Rows.Count > 0)
				foreach (DataRow dr in dt.Rows)
					list.Add(new SelectListItem_Custom(Convert.ToString(dr["Lov_Code"]), Convert.ToString(dr["Lov_Desc"]), Convert.ToString(dr["Lov_Column"])));

			dt = new DataTable();
			sqlParameters = new List<SqlParameter>();
			sqlParameters.Add(new SqlParameter("@Id", SqlDbType.VarChar) { Value = 0 });
			dt = DataContext_Command.ExecuteStoredProcedure_DataTable("SP_Cities_Get", null, true);

			if (dt != null && dt.Rows.Count > 0)
				foreach (DataRow dr in dt.Rows)
					list.Add(new SelectListItem_Custom(Convert.ToString(dr["Id"]), Convert.ToString(dr["Name"]), "City"));

			CommonViewModel.SelectListItems = list;

			return View(CommonViewModel);
		}

		[HttpPost]
		public ActionResult GetData_PropertyList(JqueryDatatableParam param)
		{
			if (Request.Form.ContainsKey("draw"))
				param.sEcho = Request.Form["draw"];

			if (Request.Form.ContainsKey("search[value]"))
				param.sSearch = Request.Form["search[value]"];

			if (Request.Form.ContainsKey("length"))
				param.iDisplayLength = Convert.ToInt32(Request.Form["length"]);

			if (Request.Form.ContainsKey("start"))
				param.iDisplayStart = Convert.ToInt32(Request.Form["start"]);

			if (Request.Form.ContainsKey("order[0][column]"))
				param.iSortCol_0 = Convert.ToInt32(Request.Form["order[0][column]"]);

			if (Request.Form.ContainsKey("order[0][dir]"))
				param.sSortDir_0 = Request.Form["order[0][dir]"];

			param.iDisplayLength = param.iDisplayLength <= 0 ? 10 : param.iDisplayLength;
			param.sSortDir_0 = string.IsNullOrEmpty(param.sSortDir_0) ? "asc" : param.sSortDir_0;

			long CityId = long.TryParse(Request.Form["CityId"], out var c) ? c : 0;
			long AreaId = long.TryParse(Request.Form["AreaId"], out var a) ? a : 0;
			long CategoryId = long.TryParse(Request.Form["CategoryId"], out var cat) ? cat : 0;
			long TypeId = long.TryParse(Request.Form["TypeId"], out var t) ? t : 0;
			long SubTypeId = long.TryParse(Request.Form["SubTypeId"], out var st) ? st : 0;

			decimal MinPrice = decimal.TryParse(Request.Form["MinPrice"], out var minP) ? minP : 0;
			decimal MaxPrice = decimal.TryParse(Request.Form["MaxPrice"], out var maxP) ? maxP : 0;
			decimal MinArea = decimal.TryParse(Request.Form["MinAreaSqft"], out var minA) ? minA : 0;
			decimal MaxArea = decimal.TryParse(Request.Form["MaxAreaSqft"], out var maxA) ? maxA : 0;

			List<Properties> list = DataContext_Command.Property_Get(0).ToList();

			int recordsTotal = list.Count;

			IEnumerable<Properties> query = list;

			if (CityId > 0)
				query = query.Where(x => x.CityId == CityId);

			if (AreaId > 0)
				query = query.Where(x => x.AreaId == AreaId);

			if (CategoryId > 0)
				query = query.Where(x => x.CategoryId == CategoryId);

			if (TypeId > 0)
				query = query.Where(x => x.TypeId == TypeId);

			if (SubTypeId > 0)
				query = query.Where(x => x.SubTypeId == SubTypeId);

			if (MinPrice > 0)
				query = query.Where(x => x.Price >= MinPrice);

			if (MaxPrice > 0)
				query = query.Where(x => x.Price <= MaxPrice);

			if (MinArea > 0)
				query = query.Where(x => x.AreaSqft >= MinArea);

			if (MaxArea > 0)
				query = query.Where(x => x.AreaSqft <= MaxArea);

			if (!string.IsNullOrWhiteSpace(param.sSearch))
			{
				string search = param.sSearch.Trim().ToLower();

				query = query.Where(x =>
					(x.Title ?? "").ToLower().Contains(search) ||
					(x.OwnerName ?? "").ToLower().Contains(search) ||
					(x.OwnerMobile ?? "").ToLower().Contains(search) ||
					(x.BuilderName ?? "").ToLower().Contains(search)
				);
			}

			int recordsFiltered = query.Count();

			bool asc = param.sSortDir_0 == "asc";

			query = param.iSortCol_0 switch
			{
				1 => asc ? query.OrderBy(x => x.Property_Category) : query.OrderByDescending(x => x.Property_Category),
				2 => asc ? query.OrderBy(x => x.Property_Type) : query.OrderByDescending(x => x.Property_Type),
				3 => asc ? query.OrderBy(x => x.Title) : query.OrderByDescending(x => x.Title),
				4 => asc ? query.OrderBy(x => x.Price) : query.OrderByDescending(x => x.Price),
				5 => asc ? query.OrderBy(x => x.AreaSqft) : query.OrderByDescending(x => x.AreaSqft),
				_ => query.OrderByDescending(x => x.Id)
			};

			var data = query
				.Skip(param.iDisplayStart)
				.Take(param.iDisplayLength)
				.ToList();

			return Json(new
			{
				draw = param.sEcho,
				recordsTotal = recordsTotal,
				recordsFiltered = recordsFiltered,
				data = data
			});
		}


		//[CustomAuthorizeAttribute(AccessType_Enum.Read)]
		public ActionResult Partial_AddEditForm(long Id = 0)
		{
			CommonViewModel.Obj = new Properties() { CityId = 0, AreaId = 0, TypeId = 0, CategoryId = 0, AvailabilityStatus = "0", FurnishingStatus = "0" };
			var list = new List<SelectListItem_Custom>();
			var dt = new DataTable();
			var sqlParameters = new List<SqlParameter>();
			if (Id > 0)
			{
				CommonViewModel.Obj = DataContext_Command.Property_Get(Id).FirstOrDefault();

				dt = new DataTable();
				sqlParameters = new List<SqlParameter>();
				sqlParameters.Add(new SqlParameter("@Id", SqlDbType.VarChar) { Value = CommonViewModel.Obj.CityId });
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
			sqlParameters = new List<SqlParameter>();
			sqlParameters.Add(new SqlParameter("@Id", SqlDbType.VarChar) { Value = 0 });
			dt = DataContext_Command.ExecuteStoredProcedure_DataTable("SP_PropertyCategories_Get", sqlParameters, true);

			if (dt != null && dt.Rows.Count > 0)
			{
				foreach (DataRow dr in dt.Rows)
				{
					list.Add(new SelectListItem_Custom(Convert.ToString(dr["Id"]), Convert.ToString(dr["Name"]), "PropertyCategory")
					{
						Value = dr["Id"] != DBNull.Value ? Convert.ToString(dr["Id"]) : "",
						Text = dr["Name"] != DBNull.Value ? Convert.ToString(dr["Name"]) : "",

					});
				}
			}

			dt = new DataTable();

			var listPropertyType = new List<PropertyCategoryTypeMapping>();

			if (CommonViewModel.Obj.CategoryId > 0)
			{
				var _listPropertyType = DataContext_Command.PropertyCategoryType_Get(CommonViewModel.Obj.CategoryId ?? 0, 0).ToList().ToList();
				if (_listPropertyType != null && _listPropertyType.Count() > 0) listPropertyType.AddRange(_listPropertyType);
			}

			if (CommonViewModel.Obj.TypeId > 0)
			{
				var _listPropertyType = DataContext_Command.PropertyCategoryType_Get(0, CommonViewModel.Obj.TypeId ?? 0).ToList();
				if (_listPropertyType != null && _listPropertyType.Count() > 0) listPropertyType.AddRange(_listPropertyType);
			}

			if (listPropertyType != null && listPropertyType.Count() > 0)
				foreach (var item in listPropertyType)
					list.Add(new SelectListItem_Custom(item.TypeId.ToString(), item.TypeName, item.ParentId.ToString(), "PropertyType"));
			else
			{
				var _listPropertyType = DataContext_Command.PropertyCategoryType_Get(0, 0).ToList()
							.Select(x => new SelectListItem_Custom(x.TypeId.ToString(), x.TypeName, x.ParentId.ToString(), "PropertyType")).Distinct().ToList();

				if (_listPropertyType != null && _listPropertyType.Count() > 0) list.AddRange(_listPropertyType);
			}

			dt = new DataTable();
			sqlParameters = new List<SqlParameter>();
			sqlParameters.Add(new SqlParameter("@Lov_Column", SqlDbType.VarChar) { Value = "FURNISHINGSTATUS" });
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

		public ActionResult Partial_PropertyDetail(long Id = 0)
		{
			CommonViewModel.Obj = new Properties();

			CommonViewModel.Obj = DataContext_Command.Property_Get(Id, 0).FirstOrDefault();

			return PartialView("_Partial_PropertyDetail", CommonViewModel);
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
		public ActionResult Save(Properties viewModel)
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

					if (string.IsNullOrEmpty(viewModel.Title))
					{
						CommonViewModel.IsSuccess = false;
						CommonViewModel.StatusCode = ResponseStatusCode.Error;
						CommonViewModel.Message = "Please enter title.";

						return Json(CommonViewModel);
					}

					if (string.IsNullOrEmpty(viewModel.Description))
					{
						CommonViewModel.IsSuccess = false;
						CommonViewModel.StatusCode = ResponseStatusCode.Error;
						CommonViewModel.Message = "Please enter description.";

						return Json(CommonViewModel);
					}
					if (viewModel.CityId == 0)
					{
						CommonViewModel.IsSuccess = false;
						CommonViewModel.StatusCode = ResponseStatusCode.Error;
						CommonViewModel.Message = "Please select city.";

						return Json(CommonViewModel);
					}
					if (viewModel.AreaId == 0)
					{
						CommonViewModel.IsSuccess = false;
						CommonViewModel.StatusCode = ResponseStatusCode.Error;
						CommonViewModel.Message = "Please select area.";

						return Json(CommonViewModel);
					}
					if (viewModel.CategoryId == 0)
					{
						CommonViewModel.IsSuccess = false;
						CommonViewModel.StatusCode = ResponseStatusCode.Error;
						CommonViewModel.Message = "Please select property category.";

						return Json(CommonViewModel);
					}
					if (viewModel.TypeId == 0)
					{
						CommonViewModel.IsSuccess = false;
						CommonViewModel.StatusCode = ResponseStatusCode.Error;
						CommonViewModel.Message = "Please select property type.";

						return Json(CommonViewModel);
					}

					if (viewModel.Price == null)
					{
						CommonViewModel.IsSuccess = false;
						CommonViewModel.StatusCode = ResponseStatusCode.Error;
						CommonViewModel.Message = "Please enter price.";

						return Json(CommonViewModel);
					}
					if (viewModel.AreaSqft == null)
					{
						CommonViewModel.IsSuccess = false;
						CommonViewModel.StatusCode = ResponseStatusCode.Error;
						CommonViewModel.Message = "Please enter area square ft.";

						return Json(CommonViewModel);
					}
					if (string.IsNullOrEmpty(viewModel.OwnerName))
					{
						CommonViewModel.IsSuccess = false;
						CommonViewModel.StatusCode = ResponseStatusCode.Error;
						CommonViewModel.Message = "Please enter owner name.";

						return Json(CommonViewModel);
					}

					if (string.IsNullOrEmpty(viewModel.OwnerMobile))
					{
						CommonViewModel.IsSuccess = false;
						CommonViewModel.StatusCode = ResponseStatusCode.Error;
						CommonViewModel.Message = "Please enter owner mobile no.";

						return Json(CommonViewModel);
					}
					if (!string.IsNullOrEmpty(viewModel.OwnerMobile) && !ValidateField.IsValidMobileNo_D10(viewModel.OwnerMobile))
					{
						CommonViewModel.IsSuccess = false;
						CommonViewModel.StatusCode = ResponseStatusCode.Error;
						CommonViewModel.Message = "Please enter valid 10 digit owner mobile no.";

						return Json(CommonViewModel);
					}

					if (string.IsNullOrEmpty(viewModel.BuilderName))
					{
						CommonViewModel.IsSuccess = false;
						CommonViewModel.StatusCode = ResponseStatusCode.Error;
						CommonViewModel.Message = "Please enter  builder name.";

						return Json(CommonViewModel);
					}
					#endregion

					#region Database-Transaction

					var (IsSuccess, response, Id) = DataContext_Command.Property_Save(viewModel);
					viewModel.Id = Id;

					CommonViewModel.IsConfirm = IsSuccess;
					CommonViewModel.IsSuccess = IsSuccess;
					CommonViewModel.StatusCode = IsSuccess ? ResponseStatusCode.Success : ResponseStatusCode.Error;
					CommonViewModel.Message = response;
					CommonViewModel.RedirectURL = Url.Action("Index", "Property", new { area = "Admin" });








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
