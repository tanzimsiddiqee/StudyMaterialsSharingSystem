using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StudyMaterialsSharingSystem.Models
{
    public class Book
    {
        [Key]
        public string BookID { get; set; }

        [Display(Name = "Book Name")]
        public string BookName { get; set; }

        public string Condition { get; set; }

        [Display(Name = "Image")]
        public string ImagePath { get; set; }

        public string Description { get; set; }

        public string OwnerID { get; set; }

        public string Price { get; set; }

        [Display(Name = "Price Type")]
        public string PriceType { get; set; }

        public string Email { get; set; }

        [Display(Name = "Phone number")]
        public string Phone { get; set; }

        public string Location { get; set; }

        [DisplayFormat(DataFormatString = "{0:F}")]
        public DateTime AdTime { get; set; }

        [Display(Name = "Sub-Category")]
        public int SubCategoryID { get; set; }
        public SubCategory SubCategory { get; set; }

        public BookStatus Status { get; set; }
    }
    public enum BookStatus
    {
        Submitted,
        Approved,
        Rejected
    }
}
