using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetProject.API.Services;
using NetProject.DataModels;
using NetProject.ViewModels;

namespace NetProject.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class apiMedicineController : ControllerBase
    {
        private ApiResponse response = new();
        private readonly MedicineService medicineService;

        public apiMedicineController(MedicineService _medicineService)
        {
            medicineService = _medicineService;
        }

        //[HttpGet("GetAll")]
        //public async Task<ApiResponse> GetAll()
        //{
        //    response = await medicineService.GetAll();
        //    return response;
        //}

        [HttpGet("GetAll")]
        public async Task<ApiResponse> GetAll([FromQuery] SearchQuery queries)
        {
            response = await medicineService.GetAll(queries);
            return response;
        }


        [HttpPost("Add")]
        public async Task<ApiResponse> Add(Medicine data)
        {
            if (data == null)
            {
                response.Message = "Invalid User Data";
            }

            response = await medicineService.Add(data);
            return response;
        }

        [HttpGet("GetById/{id}")]
        public async Task<ApiResponse> GetById(long id)
        {
            response = await medicineService.GetById(id);
            return response;
        }

        [HttpGet("Stats")]
        public async Task<ApiResponse> Stats()
        {
            var response = await medicineService.Stats();
            return response;
        }
    }
}
