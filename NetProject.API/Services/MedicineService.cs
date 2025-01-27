using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NetProject.DataModels;
using NetProject.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NetProject.API.Services
{
    public class MedicineService
    {

        private readonly NetProjectContext db;
        private ApiResponse response = new();
        public MedicineService(NetProjectContext db)
        {
            this.db = db;
        }

        public async Task<ApiResponse> GetAll()
        {

            //var query = db.Medicines.AsQueryable();
            List<ViewMedicine> data = await (from med in db.Medicines
                                       select new ViewMedicine
                                       {
                                           Id = med.Id,
                                           Name = med.Name,
                                           Stock = med.Stock,
                                           Price = med.Price,
                                           ExpiredDate = med.ExpiredDate,

                                       }).ToListAsync();
            response.Entity = data;
            return response;
        }

        public async Task<ApiResponse> GetAll(SearchQuery queries)
        {
            var query = (from med in db.Medicines
                         select new ViewMedicine
                         {
                             Id = med.Id,
                             Name = med.Name,
                             Stock = med.Stock,
                             Price = med.Price,
                             ExpiredDate = med.ExpiredDate,

                         }).AsQueryable();

            if (!string.IsNullOrEmpty(queries.searchTerm))
            {
                query = query.Where(e => e.Name.ToLower().Contains(queries.searchTerm.ToLower()));  // Modify as per your entity properties
            }

            switch (queries.sortOrder)
            {
                case "name_desc":
                    query = query.OrderByDescending(a => a.Name);
                    break;
                default:
                    query = query.OrderBy(a => a.Name);
                    break;
            }


            var totalData = await query.CountAsync();
            var items = await query.Skip((queries.pageNumber.Value - 1) * queries.showData.Value)
                                  .Take(queries.showData.Value)
                                  .ToListAsync();
            EntityPagination<ViewMedicine> data = new EntityPagination<ViewMedicine>(items,totalData, queries.pageNumber.Value, queries.showData.Value);
            response.Entity = data;
            return response;

        }

        public async Task<ApiResponse> GetById(long id)
        {
            ViewMedicine? data = await (from med in db.Medicines
                                       where med.Id == id
                                       select new ViewMedicine
                                       {
                                           Id = med.Id,
                                           Name = med.Name,
                                           Stock = med.Stock,
                                           Price = med.Price,
                                           ExpiredDate = med.ExpiredDate,

                                       }).FirstOrDefaultAsync();
            if (data == null)
            {
                response.Success = false;
                response.Message = "Medicine not found";
                return response;
            }
            response.Entity = data;
            return response;
        }



        public async Task<ApiResponse> Add(Medicine data)
        {
            data.CreatedOn = DateTime.Now;
            try
            {
                await db.AddAsync(data);
                await db.SaveChangesAsync();
                response.Message = "Tambah data berhasil";
                response.Entity = data;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Tambah data gagal : " + ex.Message;
                response.Entity = data;
            }
            return response;
        }

        public async Task<ApiResponse> Stats()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);

            var expiredCount = (from m in db.Medicines
                                where m.ExpiredDate < today.AddDays(7)
                                select m.Id).Count();

            var safeCount = (from m in db.Medicines
                             where m.ExpiredDate > today.AddDays(7)
                             select m.Id).Count();

            var lessCount = (from m in db.Medicines
                             where m.Stock < 10
                             select m.Id).Count();

            var moreCount = (from m in db.Medicines
                             where m.Stock >= 10
                             select m.Id).Count();

            List<ViewGroupCount> data = new List<ViewGroupCount>()
            {
                new("EXPIRED", expiredCount),
                new("SAFE", safeCount),
                new("LESS", lessCount),
                new("MORE", moreCount)
            };
            response.Entity = data;
            return response;

        }

        //public async Task<ApiResponse> Stats()
        //{
        //    var today = DateOnly.FromDateTime(DateTime.Now);


        //    List<ViewMedicine> meds = (from med in db.Medicines
        //                                   select new ViewMedicine
        //                                   {
        //                                       Id = med.Id,
        //                                       Name = med.Name,
        //                                       Stock = med.Stock,
        //                                       Price = med.Price,
        //                                       ExpiredDate = med.ExpiredDate,

        //                                   }).ToList();

        //    List<ViewGroupCount> expGroup = meds.GroupBy(v => v.ExpiredDetail)
        //                                    .Select(group => new ViewGroupCount
        //                                    {
        //                                        label = group.Key,
        //                                        value = group.Count()
        //                                    }).ToList();

        //    List<ViewGroupCount> exGroup = (from med in db.Medicines
        //                                    where med.ExpiredDate < today.AddDays(7))

        //    List<ViewGroupCount> stockGroup = db.Medicines
        //                                        .GroupBy(v => v.Stock < 10 ? "Less" : "More")  // Group by Stock being less than or greater/equal to 10
        //                                        .Select(group => new ViewGroupCount
        //                                        {
        //                                            label = group.Key,  // The group label ("Stock < 10" or "Stock >= 10")
        //                                            value = group.Count()  // Count of medicines in this group
        //                                        })
        //                                        .ToList();

        //    List<ViewGroupCount> data = expGroup.Concat(stockGroup).ToList();

        //    response.Entity = data;
        //    return response;

        //    //int safeCount = meds.Count(med => med.ExpiredDetail == "SAFE");
        //    //int expiredCount = meds.Count(med => med.ExpiredDetail == "EXPIRED");

        //    //int lessCount = db.Medicines.Count(med => med.Stock < 10);
        //    //int sufficientCount = db.Medicines.Count(med => med.Stock >= 10);

        //    //var data = new
        //    //{
        //    //    group = "expired,safe,less,sufficient",
        //    //    value = $"{expiredCount},{safeCount},{lessCount},{sufficientCount}"
        //    //};
        //    //return data;
        //}

    }
}
