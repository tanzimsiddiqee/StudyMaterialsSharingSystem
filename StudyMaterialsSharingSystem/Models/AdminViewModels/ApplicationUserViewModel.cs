using StudyMaterialsSharingSystem.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudyMaterialsSharingSystem.Models.AdminViewModels
{
    public class ApplicationUserViewModel
    {
        public IEnumerable<ApplicationUser> Users { get; set; }
        public IEnumerable<ApplicationUser> Managers { get; set; }
        public IEnumerable<ApplicationUser> Administrators { get; set; }
    }
}
