using Microsoft.AspNetCore.Mvc;
using NetProject.DataModels;
using NetProject.Services;
using NetProject.ViewModels;
using Newtonsoft.Json;
using NuGet.Protocol;
using System.Text;

namespace NetProject.Controllers
{
    public class MedicineController : Controller
    {

        private MedicineService medicineService;
        private RequestMedService requestMedService;
        private ApiResponse response = new ApiResponse();

        public MedicineController(MedicineService medicineService, RequestMedService requestMedService)
        {
            this.medicineService = medicineService;
            this.requestMedService = requestMedService;
        }

        //public async Task<IActionResult>Index()
        //{
        //    List<ViewMedicine> data = await medicineService.GetAll();
        //    return View(data);
        //}


        public async Task<IActionResult> Index (SearchQuery queries)
        {
            queries.showData = queries.showData ?? 5;
            queries.pageNumber = queries.pageNumber ?? 1;

            // Set for next queries (placed on view)
            ViewBag.nameSort = string.IsNullOrEmpty(queries.sortOrder) ? "name_desc" : "";
            
            // Set on view for display
            ViewBag.currentSort = queries.sortOrder;
            ViewBag.currentShowData = queries.showData;
            ViewBag.currentSearchTerm = queries.searchTerm;

            EntityPagination<ViewMedicine> data = await medicineService.GetAll(queries);

            return View(data);
            //return View(EntityPagination<ViewMedicine>.(data, query.pageNumber ?? 1, query.showData ?? 5));
        }

        public async Task<IActionResult> Add()
        {
            Medicine data = new Medicine();

            return PartialView(data);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Medicine dataForm)
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            dataForm.CreatedBy = userId;
            ApiResponse response = await medicineService.Add(dataForm);

            return Json(new { dataResponse = response });
        }

        public async Task<IActionResult> Request(long id)
        {
            ViewBag.Medicine = await medicineService.GetById(id);
            ViewRequestMed data = new();
            return PartialView(data);
        }

        [HttpPost]
        public async Task<IActionResult> Request(ViewRequestMed dataForm)
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 1;
            dataForm.CreatedBy = userId;
            ApiResponse response = await requestMedService.RequestMed(dataForm);

            return Json(new { dataResponse = response });
        }

        public async Task<IActionResult> GetStats()
        {
            ApiResponse data = await medicineService.GetStats();
            return Json(new { dataResponse = data });

        }
    }
}
