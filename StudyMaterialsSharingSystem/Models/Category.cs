using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StudyMaterialsSharingSystem.Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryID { get; set; }

        public string CategoryName { get; set; }

        public ICollection<SubCategory> SubCategories { get; set; }
    }

    public class SubCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubCategoryID { get; set; }

        public string SubCategoryName { get; set; }

        [Display(Name = "Sub-Category")]
        public int CategoryID { get; set; }
        public Category Category { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}
