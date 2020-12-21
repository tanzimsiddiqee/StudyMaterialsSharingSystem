using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StudyMaterialsSharingSystem.Models
{
    public class Software
    {
        [Key]
        public string SoftwareID { get; set; }

        [Display(Name = "Software Name")]
        public string SoftwareName { get; set; }

        public string Version { get; set; }

        [Display(Name = "Image")]
        public string ImagePath { get; set; }

        public string Description { get; set; }

        public string OwnerID { get; set; }

        [Display(Name = "DownloadAddress")]
        public string DownloadAddress { get; set; }

        [Display(Name = "DownloadingProcess")]
        public string DownloadingProcess { get; set; }

        [Display(Name = "Installing Process")]
        public string InstallingProcess { get; set; }

        public DateTime AdTime { get; set; }

        [Display(Name = "Category")]
        public int SoftwareTypeID { get; set; }
        public SoftwareType SoftwareType { get; set; }
        public SoftStatus Status { get; set; }
    }
    public enum SoftStatus
    {
        Submitted,
        Approved,
        Rejected
    }

    public class SoftwareType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TypeID { get; set; }

        public string TypeName { get; set; }

        public ICollection<Software> Softwares { get; set; }
    }
}
