using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudyMaterialsSharingSystem.Models.BookViewModels
{
    public class BookInfoViewModel
    {
        [Required(ErrorMessage = "Select Category")]
        [Display(Name = "Category")]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Select Sub-Category")]
        [Display(Name = "Sub-Category")]
        [Range(1, int.MaxValue, ErrorMessage = "Select Sub-Category")]
        public int SubCategoryID { get; set; }

        public string BookID { get; set; }
        [Required]
        [Display(Name = "Book Name")]
        public string BookName { get; set; }

        [Required]
        public string Condition { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public string Price { get; set; }

        [Required]
        [Display(Name = "Price Type")]
        public string PriceType { get; set; }

        public string Description { get; set; }

    }
}
