using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudyMaterialsSharingSystem.Areas.Identity.Data;
using StudyMaterialsSharingSystem.Models;

namespace StudyMaterialsSharingSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }

        public DbSet<Document> Documents { get; set; }

        public DbSet<House> Houses { get; set; }

        public DbSet<Software> Softwares { get; set; }
        public DbSet<SoftwareType> SoftwareTypes { get; set; }

        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Reply> Replies { get; set; }
    }
}
