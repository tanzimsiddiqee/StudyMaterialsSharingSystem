using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StudyMaterialsSharingSystem.Models
{
    public class House
    {
        [Key]
        public string HouseID { get; set; }

        [Display(Name = "House Name")]
        public string HouseName { get; set; }

        [Display(Name = "Category")]
        public string Category { get; set; }

        [Display(Name = "For")]
        public string HouseType { get; set; }

        public string Location { get; set; }

        [Display(Name = "From")]
        [DisplayFormat(DataFormatString = "{0:dd MMMM, yy}")]
        public DateTime dateTime { get; set; }

        public string AvailableSeat { get; set; }

        public string AvailableRoom { get; set; }

        public string RentPerSeat { get; set; }

        public string RentPerRoom { get; set; }

        public string ServiceCharge { get; set; }

        public string Facilities { get; set; }

        public string Description { get; set; }

        [Display(Name = "Image")]
        public string ImagePath { get; set; }

        public string OwnerID { get; set; }

        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        public DateTime AdTime { get; set; }

        public HouseStatus Status { get; set; }
    }

    public enum HouseStatus
    {
        Submitted,
        Approved,
        Rejected
    }
}
