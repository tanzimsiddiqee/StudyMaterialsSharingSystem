using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudyMaterialsSharingSystem.Models.DocViewModels
{
    public class DocInfoViewModel
    {
        public string DocumentID { get; set; }

        [Required]
        [Display(Name = "Document Name")]
        public string DocumentName { get; set; }

        [Required]
        [Display(Name = "Format")]
        public string DocumentFormat { get; set; }

        public string ImagePath { get; set; }

        public string OwnerID { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Download Address")]
        [DataType(DataType.Url)]
        public string DownloadAddress { get; set; }
    }
}
