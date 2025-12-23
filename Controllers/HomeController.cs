using Broker.Infra;
using Broker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol.Core.Types;
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

			var list = _context.Using<AreasMaster>().GetByCondition(x => x.Id > 1, x => x.City).OrderBy(x => x.Id)
						.Select(x => new SelectListItem_Custom(x.Id.ToString(), x.Name + ", " + x.City.Name, "L")).Distinct().ToList();

			if (list != null && list.Count() > 0) CommonViewModel.SelectListItems.AddRange(list);

			list = _context.Using<PropertyCategory>().GetByCondition(x => x.IsActive == true).OrderBy(x => x.Name)
						.Select(x => new SelectListItem_Custom(x.Id.ToString(), x.Name, "PC")).Distinct().ToList();

			if (list != null && list.Count() > 0) CommonViewModel.SelectListItems.AddRange(list);

			list = _context.Using<PropertyType>().GetByCondition(x => x.IsActive == true).OrderBy(x => x.Name)
						.Select(x => new SelectListItem_Custom(x.Id.ToString(), x.Name, x.ParentId.ToString(), x.ImagePath, "PT")).Distinct().ToList();

			if (list != null && list.Count() > 0) CommonViewModel.SelectListItems.AddRange(list);

			CommonViewModel.Data1 = (from parent in _context.Using<PropertyType>().GetByCondition(x => x.IsActive == true).ToList()
									 where parent.IsActive && parent.ParentId == 0
									 select new
									 {
										 TypeId = parent.Id,
										 TypeName = parent.Name,
										 ImagePath = parent.ImagePath,
										 Display_Seq_No = parent.Display_Seq_No == null?0: parent.Display_Seq_No,
										 PropertyCount = _context.Using<Properties>().GetByCondition(x => x.IsActive == true).ToList()
										 .Count(p => (p.TypeId == parent.Id || _context.Using<PropertyType>().GetByCondition(x => x.IsActive == true)
										 .Any(c => c.ParentId == parent.Id && c.IsActive && c.Id == p.TypeId)) && p.IsActive)
									 }).ToList();

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
		public IActionResult Properties([FromQuery] PropertySerch viewModel)
		{
			viewModel ??= new PropertySerch();

			ResponseModel<Properties> responseModel = new ResponseModel<Properties>
			{
				ObjList = new List<Properties>(), //_context.Using<Properties>().GetByCondition(x => x.IsActive == true).OrderByDescending(x => x.Id).ToList();
				Data1 = viewModel
			};

			try
			{
				responseModel.IsSuccess = true;

				if (!string.IsNullOrEmpty(viewModel.Email) || !string.IsNullOrEmpty(viewModel.ContactNo))
				{
					var lead = new Lead()
					{
						Name = viewModel.FirstName + " " + viewModel.LastName,
						Email = viewModel.Email,
						Mobile = viewModel.ContactNo,
						Requirement = viewModel.PropertyFor,
						PropertyType = viewModel.PropertyType_Parent,
						LeadSource_Value = (string)_context.Using<LovMaster>().GetByCondition(x => x.LovDesc.ToLower().Contains("website")).Select(x => x.LovCode).FirstOrDefault()
					};

					//_context.Using<Lead>().Add(lead);
					var (IsSuccess, response, Id) = DataContext_Command.Leads_Save(lead);
				}

				responseModel.SelectListItems = new List<SelectListItem_Custom>();

				var list = _context.Using<AreasMaster>().GetByCondition(x => x.Id > 1, x => x.City).OrderBy(x => x.Id)
							.Select(x => new SelectListItem_Custom(x.Id.ToString(), x.Name + ", " + x.City.Name, "L")).Distinct().ToList();

				if (list != null && list.Count() > 0) responseModel.SelectListItems.AddRange(list);

				list = _context.Using<PropertyCategory>().GetByCondition(x => x.IsActive == true).OrderBy(x => x.Name)
							.Select(x => new SelectListItem_Custom(x.Id.ToString(), x.Name, "PC")).Distinct().ToList();

				if (list != null && list.Count() > 0) responseModel.SelectListItems.AddRange(list);

				list = _context.Using<PropertyType>().GetByCondition(x => x.IsActive == true).OrderBy(x => x.Name)
							.Select(x => new SelectListItem_Custom(x.Id.ToString(), x.Name, x.ParentId.ToString(), 1, "PT")).Distinct().ToList();

				if (list != null && list.Count() > 0) responseModel.SelectListItems.AddRange(list);

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
