using NetProject.DataModels;
using NetProject.ViewModels;
using Newtonsoft.Json;
using System.Text;

namespace NetProject.Services
{
    public class MedicineService
    {
        private static readonly HttpClient client = new HttpClient();
        private IConfiguration configuration;
        private ApiResponse response = new();
        public string ROUTE_API = "";

        public MedicineService(IConfiguration _configuration)
        {
            configuration = _configuration;
            ROUTE_API = configuration["RouteApi"] ?? throw new ArgumentNullException(nameof(ROUTE_API), "RouteApi is missing in configuration");
        }

        //public async Task<List<ViewMedicine>> GetAll()
        //{
        //    string apiResponse = await client.GetStringAsync(ROUTE_API + $"apiMedicine/GetAll");

        //    ApiResponse response = JsonConvert.DeserializeObject<ApiResponse>(apiResponse);

        //    List<ViewMedicine> data = JsonConvert.DeserializeObject<List<ViewMedicine>>(response.Entity.ToString());
        //    return data;
        //}

        public async Task<EntityPagination<ViewMedicine>> GetAll(SearchQuery queries)
        {
            string queryParam = queries.generateQuery();
            string apiResponse = await client.GetStringAsync(ROUTE_API + $"apiMedicine/GetAll?" + queryParam);

            ApiResponse response = JsonConvert.DeserializeObject<ApiResponse>(apiResponse);
            EntityPagination<ViewMedicine> data = JsonConvert.DeserializeObject<EntityPagination<ViewMedicine>>(response.Entity.ToString());
            return data;
        }


        public async Task<ViewMedicine> GetById(long id)
        {
            string apiResponse = await client.GetStringAsync(ROUTE_API + $"apiMedicine/GetById/{id}");

            ApiResponse response = JsonConvert.DeserializeObject<ApiResponse>(apiResponse);

            ViewMedicine data = JsonConvert.DeserializeObject<ViewMedicine>(response.Entity.ToString());
            return data;
        }

        public async Task<ApiResponse> GetStats()
        {
            string apiResponse = await client.GetStringAsync(ROUTE_API + $"apiMedicine/Stats");

            ApiResponse response = JsonConvert.DeserializeObject<ApiResponse>(apiResponse);
            List<ViewGroupCount> data = JsonConvert.DeserializeObject<List<ViewGroupCount>>(response.Entity.ToString());
            response.Entity = data;
            return response;
        }

        public async Task<ApiResponse> Add(Medicine dataForm)
        {
            //Proses convert dari objext ke string
            string json = JsonConvert.SerializeObject(dataForm);

            //proses ubah string ke json
            StringContent content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var request = await client.PostAsync(ROUTE_API + "apiMedicine/Add", content);

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
