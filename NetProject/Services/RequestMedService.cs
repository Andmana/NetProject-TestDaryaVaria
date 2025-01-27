using NetProject.DataModels;
using NetProject.ViewModels;
using Newtonsoft.Json;
using System.Text;

namespace NetProject.Services
{
    public class RequestMedService
    {
        private static readonly HttpClient client = new HttpClient();
        private IConfiguration configuration;
        private ApiResponse response = new();
        public string ROUTE_API = "";

        public RequestMedService(IConfiguration _configuration)
        {
            configuration = _configuration;
            ROUTE_API = configuration["RouteApi"] ?? throw new ArgumentNullException(nameof(ROUTE_API), "RouteApi is missing in configuration");
        }

        //public async Task<List<ViewRequestMed>> GetAll()
        //{
        //    string apiResponse = await client.GetStringAsync(ROUTE_API + $"apiRequestMed/GetAll");

        //    ApiResponse response = JsonConvert.DeserializeObject<ApiResponse>(apiResponse);

        //    List<ViewRequestMed> data = JsonConvert.DeserializeObject<List<ViewRequestMed>>(response.Entity.ToString());
        //    return data;
        //}

        public async Task<EntityPagination<ViewRequestMed>> GetAll(SearchQuery queries)
        {
            string queryParam = queries.generateQuery();
            string apiResponse = await client.GetStringAsync(ROUTE_API + $"apiRequestMed/GetAll?" + queryParam);

            ApiResponse response = JsonConvert.DeserializeObject<ApiResponse>(apiResponse);
            EntityPagination<ViewRequestMed> data = JsonConvert.DeserializeObject<EntityPagination<ViewRequestMed>>(response.Entity.ToString());
            return data;
        }

        public async Task<ApiResponse> RequestMed(ViewRequestMed dataForm)
        {
            //Proses convert dari objext ke string
            string json = JsonConvert.SerializeObject(dataForm);

            //proses ubah string ke json
            StringContent content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var request = await client.PostAsync(ROUTE_API + "apiRequestMed/RequestMed", content);

            if (request.IsSuccessStatusCode)
            {
                var apiResponse = await request.Content.ReadAsStringAsync();

                response = JsonConvert.DeserializeObject<ApiResponse>(apiResponse);
            }
            else
            {
                response.Success = false;
                response.Message = $"{request.StatusCode} : {request.ReasonPhrase}";
            }
            return response;


        }

        public async Task<ApiResponse> ApproveRequest(ViewRequestMed dataForm)
        {
            //Proses convert dari objext ke string
            string json = JsonConvert.SerializeObject(dataForm);

            //proses ubah string ke json
            StringContent content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var request = await client.PostAsync(ROUTE_API + "apiRequestMed/ApproveRequest", content);

            if (request.IsSuccessStatusCode)
            {
                var apiResponse = await request.Content.ReadAsStringAsync();

                response = JsonConvert.DeserializeObject<ApiResponse>(apiResponse);
            }
            else
            {
                response.Success = false;
                response.Message = $"{request.StatusCode} : {request.ReasonPhrase}";
            }
            return response;


        }
    }
}
