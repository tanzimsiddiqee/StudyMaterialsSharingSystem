using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudyMaterialsSharingSystem.Models.SoftwareViewModels
{
    public class SoftwareInfoViewModel
    {
        [Required(ErrorMessage = "Select Category")]
        [Display(Name = "Category")]
        public int SoftwareTypeID { get; set; }

        public string SoftwareID { get; set; }
        [Required]
        [Display(Name = "Software Name")]
        public string SoftwareName { get; set; }

        [Required]
        public string Version { get; set; }

        public string Description { get; set; }
        
    }
}
