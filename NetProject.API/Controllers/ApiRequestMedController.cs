using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetProject.API.Services;
using NetProject.ViewModels;

namespace NetProject.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApiRequestMedController : ControllerBase
    {
        private ApiResponse response = new();
        private RequestMedService requestMedService;

        public ApiRequestMedController(RequestMedService requestMedService)
        {
            this.requestMedService = requestMedService;
        }

        //[HttpGet("GetAll")]
        //public async Task<ApiResponse> GetAll()
        //{
        //    response = await requestMedService.GetAllRequest();
        //    return response;
        //}

        [HttpGet("GetAll")]
        public async Task<ApiResponse> GetAll([FromQuery] SearchQuery queries)
        {
            response = await requestMedService.GetAll(queries);
            return response;
        }


        [HttpPost("RequestMed")]
        public async Task<ApiResponse> RequestMed(ViewRequestMed input)
        {
            response = await requestMedService.RequestMed(input);
            return response;
        }

        [HttpPost("ApproveRequest")]
        public async Task<ApiResponse> ApproveRequest(ViewRequestMed input)
        {
            response = await requestMedService.ApproveRequest(input);
            return response;
        }
    }
}
