using Microsoft.AspNetCore.Mvc;
using NetProject.Services;
using NetProject.ViewModels;

namespace NetProject.Controllers
{
    public class RequestMedController : Controller
    {
        private RequestMedService requestMedService;
        private ApiResponse response = new ApiResponse();

        public RequestMedController(RequestMedService requestMedService)
        {
            this.requestMedService = requestMedService;      
        }


        //public async Task<IActionResult> Index()
        //{
        //    List<ViewRequestMed> data = await requestMedService.GetAll();
        //    return View(data);
        //}

        public async Task<IActionResult> Index(SearchQuery queries)
        {
            queries.showData = queries.showData ?? 5;
            queries.pageNumber = queries.pageNumber ?? 1;

            // Set for next queries (placed on view)
            //ViewBag.nameSort = string.IsNullOrEmpty(queries.sortOrder) ? "name_desc" : "";

            // Set on view for display
            ViewBag.currentSort = queries.sortOrder;
            ViewBag.currentShowData = queries.showData;
            ViewBag.currentSearchTerm = queries.searchTerm;

            EntityPagination<ViewRequestMed> data = await requestMedService.GetAll(queries);

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveReq(ViewRequestMed dataForm)
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 1;
            dataForm.ModifiedBy = userId;
            ApiResponse response = await requestMedService.ApproveRequest(dataForm);

            return Json(new { dataResponse = response });
        }
    }
}
