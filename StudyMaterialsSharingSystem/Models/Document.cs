using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StudyMaterialsSharingSystem.Models
{
    public class Document
    {
        [Key]
        public string DocumentID { get; set; }

        [Display(Name = "Document Name")]
        public string DocumentName { get; set; }

        [Display(Name = "Format")]
        public string DocumentFormat { get; set; }

        public string ImagePath { get; set; }

        public string OwnerID { get; set; }

        [Display(Name = "Download Address")]
        public string DownloadAddress { get; set; }

        public string Description { get; set; }

        public DateTime AdTime { get; set; }

        public DocumentStatus Status { get; set; }
    }
    public enum DocumentStatus
    {
        Submitted,
        Approved,
        Rejected
    }
}
