using Microsoft.EntityFrameworkCore;
using NetProject.DataModels;
using NetProject.ViewModels;

namespace NetProject.API.Services
{
    public class RequestMedService
    {
        private ApiResponse response = new();
        private readonly NetProjectContext db;

        public RequestMedService(NetProjectContext _db)
        {
            db = _db;
        }

        public async Task<ApiResponse> GetAllRequest()
        {
            List<ViewRequestMed> data = await (from rm in db.RequestMeds
                                         join req in db.UserAccounts
                                            on rm.CreatedBy equals req.Id
                                         join med in db.Medicines
                                            on rm.MedicineId equals med.Id
                                         join app in db.UserAccounts
                                            on rm.ModifiedBy equals app.Id into approver
                                         from app in approver.DefaultIfEmpty()
                                         //  order by status then
                                         orderby rm.CreatedOn descending
                                             //where rm.Status == "REQUEST"
                                         select new ViewRequestMed
                                         {
                                             Id = rm.Id,
                                             MedicineId = med.Id,
                                             MedicineName = med.Name,
                                             Quantity = rm.Quantity,
                                             RequesterName = req.Name,
                                             ApproverName = app.Name,
                                             CreatedOn = rm.CreatedOn,
                                             ModifiedBy = rm.ModifiedBy,
                                             ModifiedOn = rm.ModifiedOn,
                                             Status = rm.Status,

                                         }).ToListAsync();
            response.Entity = data;
            return response;
        }

        public async Task<ApiResponse> GetAll(SearchQuery queries)
        {
            var query = (from rm in db.RequestMeds
                         join req in db.UserAccounts
                                            on rm.CreatedBy equals req.Id
                         join med in db.Medicines
                            on rm.MedicineId equals med.Id
                         join app in db.UserAccounts
                            on rm.ModifiedBy equals app.Id into approver
                         from app in approver.DefaultIfEmpty()
                         select new ViewRequestMed
                         {
                             Id = rm.Id,
                             MedicineId = med.Id,
                             MedicineName = med.Name,
                             Quantity = rm.Quantity,
                             RequesterName = req.Name,
                             ApproverName = app.Name,
                             CreatedOn = rm.CreatedOn,
                             ModifiedBy = rm.ModifiedBy,
                             ModifiedOn = rm.ModifiedOn,
                             Status = rm.Status,

                         }).AsQueryable();

            if (!string.IsNullOrEmpty(queries.searchTerm))
            {
                query = query.Where(e => e.RequesterName.ToLower().Contains(queries.searchTerm.ToLower())  
                                      || e.MedicineName.ToLower().Contains(queries.searchTerm.ToLower())
                                      || e.ApproverName.ToLower().Contains(queries.searchTerm.ToLower())
                                      );  // Modify as per your entity properties
            }

            switch (queries.sortOrder)
            {
                case "req_desc":
                    query = query.OrderByDescending(a => a.RequesterName);
                    break;
                case "req":
                    query = query.OrderBy(a => a.RequesterName);
                    break;
                case "app_desc":
                    query = query.OrderByDescending(a => a.ApproverName);
                    break;
                case "app":
                    query = query.OrderBy(a => a.ApproverName);
                    break;
                case "create":
                    query = query.OrderBy(a => a.CreatedOn);
                    break;
                default:
                    query = query.OrderByDescending(a => a.CreatedOn);
                    break;
            }


            var totalData = await query.CountAsync();
            var items = await query.Skip((queries.pageNumber.Value - 1) * queries.showData.Value)
                                  .Take(queries.showData.Value)
                                  .ToListAsync();
            EntityPagination<ViewRequestMed> data = new EntityPagination<ViewRequestMed>(items, totalData, queries.pageNumber.Value, queries.showData.Value);
            response.Entity = data;
            return response;

        }

        public async Task<ApiResponse> RequestMed(ViewRequestMed input)
        {
            Medicine medicine = db.Medicines.FirstOrDefault(med => med.Id == input.MedicineId);
            if (medicine == null)
            {
                response.Message = "Invalid Medicine Id";
                response.Entity = input;
                response.Success = false;
                return response;
            }

            if (medicine.Stock < input.Quantity || input.Quantity <= 0)
            {
                response.Message = "Invalid Quantity";
                response.Entity = input;
                response.Success = false;
                return response;
            }

            RequestMed data = new();
            data.MedicineId = input.MedicineId;
            data.Quantity = input.Quantity;
            data.CreatedOn = DateTime.Now;
            data.CreatedBy = input.CreatedBy;
            data.Status = "REQUEST";

            medicine.Stock -= input.Quantity;
            try
            {
                db.AddAsync(data);
                db.Update(medicine);
                db.SaveChangesAsync();

                response.Message = "Medicine request created successfully";
                response.Entity = data;
            }catch (DbUpdateException ex)
            {
                response.Message = "Failed to create request : " + ex.Message;
                response.Success =false;
                response.Entity = input;
            }

            return response;

        }

        public async Task<ApiResponse> ApproveRequest(ViewRequestMed input)
        {
            RequestMed data = db.RequestMeds.FirstOrDefault(req => req.Id == input.Id);

            if (data == null)
            {
                response.Message = "Invalid Request Id";
                response.Entity = input;
                response.Success = false;
                return response;
            }

            try
            {
                data.ModifiedBy = input.ModifiedBy;
                data.ModifiedOn = DateTime.Now;
                data.Status = "APPROVED";

                db.Update(data);
                db.SaveChangesAsync();

                response.Message = "Request approved successfully";
                response.Entity = data;
            }catch(DbUpdateException ex)
            {
                response.Message = "Failed to approve request : " + ex.Message;
                response.Success = false;
                response.Entity = input;
            }

            return response;
        }
    }
}
