using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudyMaterialsSharingSystem.Models.HouseViewModels
{
    public class HouseInfoViewModel
    {
        public string HouseID { get; set; }

        [Display(Name = "House Name")]
        public string HouseName { get; set; }

        [Required]
        [Display(Name = "Category")]
        public string Category { get; set; }

        [Required]
        [Display(Name = "For")]
        public string HouseType { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        [Display(Name = "From")]
        [DataType(DataType.Date)]
        public DateTime dateTime { get; set; }

        [Display(Name = "Available Seat")]
        public string AvailableSeat { get; set; }

        [Display(Name = "Available Room")]
        public string AvailableRoom { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Rent Per Seat")]
        public string RentPerSeat { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Rent Per Room")]
        public string RentPerRoom { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Service Charge")]
        public string ServiceCharge { get; set; }
    }
}
