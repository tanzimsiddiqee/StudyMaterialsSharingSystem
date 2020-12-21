using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudyMaterialsSharingSystem.Models.SoftwareViewModels
{
    public class SoftwareInfoViewModel2
    {
        [DataType(DataType.Upload)]
        public IFormFile ImageUoload { get; set; }

        public string ImagePath { get; set; }

        public string OwnerID { get; set; }

        [Required]
        [Display(Name = "Download Address")]
        [DataType(DataType.Url)]
        public string DownloadAddress { get; set; }

        [Required]
        [Display(Name = "Downloading Process")]
        public string DownloadingProcess { get; set; }

        [Required]
        [Display(Name = "Installing Process")]
        public string InstallingProcess { get; set; }


        public SoftwareInfoViewModel SoftwareInfo { get; set; }
    }
}

