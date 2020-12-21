using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudyMaterialsSharingSystem.Models.HouseViewModels
{
    public class HouseInfoViewModel2
    {
        [DataType(DataType.Upload)]
        public IFormFile ImageUoload { get; set; }

        public string ImagePath { get; set; }

        public string Facilities { get; set; }

        public string Description { get; set; }

        public string OwnerID { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone number")]
        public string Phone { get; set; }

        public HouseInfoViewModel HouseInfo { get; set; }
    }
}
