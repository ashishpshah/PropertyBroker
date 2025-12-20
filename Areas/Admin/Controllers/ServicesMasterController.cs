using Broker.Controllers;
using Broker.Infra;
using Broker.Models;
using Microsoft.AspNetCore.Mvc;

namespace Broker.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServicesMasterController : BaseController<ResponseModel<ServicesMaster>>
    {
        public ServicesMasterController(IRepositoryWrapper repository) : base(repository) { }


        public ActionResult Index()
        {
            CommonViewModel.ObjList = new List<ServicesMaster>();
            CommonViewModel.ObjList = DataContext_Command.ServicesMaster_Get(0).ToList();

            return View(CommonViewModel);
        }

        public ActionResult Partial_AddEditForm(long Id = 0)
        {
            CommonViewModel.Obj = new ServicesMaster();

            if (Id > 0)
            {
                CommonViewModel.Obj = DataContext_Command.ServicesMaster_Get(Id).FirstOrDefault();
            }
            return PartialView("_Partial_AddEditForm", CommonViewModel);
        }

        [HttpPost]
        //[CustomAuthorizeAttribute(AccessType_Enum.Write)]
        public ActionResult Save(ServicesMaster viewModel)
        {
            try
            {
                if (viewModel != null && viewModel != null)
                {
                    #region Validation

                    if (viewModel.ServiceId > 0 && viewModel.ImageFile == null)
                    {
                        var old = DataContext_Command.ServicesMaster_Get(viewModel.ServiceId)
                                   .FirstOrDefault();

                        if (old != null)
                        {
                            viewModel.ResumeFile = old.ResumeFile;
                            viewModel.ImageName = old.ImageName;
                        }
                    }

                    if (string.IsNullOrEmpty(viewModel.ServiceTitle))
                    {
                        CommonViewModel.IsSuccess = false;
                        CommonViewModel.StatusCode = ResponseStatusCode.Error;
                        CommonViewModel.Message = "Please enter Service Title.";

                        return Json(CommonViewModel);
                    }

                    if (string.IsNullOrEmpty(viewModel.ShortDescription))
                    {
                        CommonViewModel.IsSuccess = false;
                        CommonViewModel.StatusCode = ResponseStatusCode.Error;
                        CommonViewModel.Message = "Please enter Short Description.";

                        return Json(CommonViewModel);
                    }

                    if (string.IsNullOrEmpty(viewModel.FullDescription))
                    {
                        CommonViewModel.IsSuccess = false;
                        CommonViewModel.StatusCode = ResponseStatusCode.Error;
                        CommonViewModel.Message = "Please enter Full Description.";

                        return Json(CommonViewModel);
                    }

                    if (viewModel.DisplayOrder == 0)
                    {
                        CommonViewModel.IsSuccess = false;
                        CommonViewModel.StatusCode = ResponseStatusCode.Error;
                        CommonViewModel.Message = "Please enter Display Order.";

                        return Json(CommonViewModel);
                    }

                    if (!viewModel.IsFeatured.HasValue)
                    {
                        CommonViewModel.IsSuccess = false;
                        CommonViewModel.StatusCode = ResponseStatusCode.Error;
                        CommonViewModel.Message = "Please select IsFeatured.";

                        return Json(CommonViewModel);
                    }


                    if (viewModel.ServiceId == 0 && (viewModel.ImageFile == null || viewModel.ImageFile.Length == 0))
                    {
                        CommonViewModel.IsSuccess = false;
                        CommonViewModel.StatusCode = ResponseStatusCode.Error;
                        CommonViewModel.Message = "Please upload an image.";

                        return Json(CommonViewModel);
                    }

                    if (viewModel.ImageFile != null && viewModel.ImageFile.Length > 0)
                    {
                        var allowedExt = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                        var ext = Path.GetExtension(viewModel.ImageFile.FileName).ToLower();

                        if (!allowedExt.Contains(ext))
                        {
                            CommonViewModel.IsSuccess = false;
                            CommonViewModel.Message = "Invalid image type";
                            return Json(CommonViewModel);
                        }

                        using (var ms = new MemoryStream())
                        {
                            viewModel.ImageFile.CopyTo(ms);
                            viewModel.ResumeFile = ms.ToArray();   
                        }

                        viewModel.ImageName = viewModel.ImageFile.FileName;
                    }
                    #endregion

                    #region Database-Transaction

                    var (IsSuccess, response, Id) = DataContext_Command.ServicesMaster_Save(viewModel);
                    viewModel.Id = Id;

                    CommonViewModel.IsConfirm = IsSuccess;
                    CommonViewModel.IsSuccess = IsSuccess;
                    CommonViewModel.StatusCode = IsSuccess ? ResponseStatusCode.Success : ResponseStatusCode.Error;
                    CommonViewModel.Message = response;
                    CommonViewModel.RedirectURL = Url.Action("Index", "ServicesMaster", new { area = "Admin" });


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
        public ActionResult DeleteConfirmed(long Id)
        {
            try
            {

                if (true)
                {
                    var (IsSuccess, response) = DataContext_Command.ServicesMaster_Delete(Id);

                    CommonViewModel.IsConfirm = IsSuccess;
                    CommonViewModel.IsSuccess = IsSuccess;
                    CommonViewModel.StatusCode = IsSuccess ? ResponseStatusCode.Success : ResponseStatusCode.Error;
                    CommonViewModel.Message = response;
                    CommonViewModel.RedirectURL = Url.Action("Index", "ServicesMaster", new { area = "Admin" });

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
