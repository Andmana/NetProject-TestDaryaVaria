using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetProject.ViewModels
{
    public class ViewRequestMed
    {
        public long Id { get; set; }

        public long MedicineId { get; set; }

        public string? MedicineName { get; set; }

        public string? RequesterName { get; set; }

        public string? ApproverName {  get; set; }

        public string? Status { get; set; } = null!;

        public int Quantity { get; set; }

        public long CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public long? DeletedBy { get; set; }
    }
}
