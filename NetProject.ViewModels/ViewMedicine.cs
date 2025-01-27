using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetProject.ViewModels
{
    public class ViewMedicine
    {
        public long Id { get; set; }

        public string? Name { get; set; }

        public int Stock { get; set; }

        public int Price { get; set; }

        public DateOnly ExpiredDate { get; set; }

        public string ExpiredDetail
        {
            get
            {

                var today = DateOnly.FromDateTime(DateTime.Now);
                var daysDifference = ExpiredDate.DayNumber - today.DayNumber;

                if (daysDifference < 7)
                {
                    return "EXPIRED";
                }
                else
                {
                    return "SAFE";
                }
            }
        }



        public long CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public long? DeletedBy { get; set; }
    }
}
