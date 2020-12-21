using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudyMaterialsSharingSystem.Areas.Identity.Data;
using StudyMaterialsSharingSystem.Data;

[assembly: HostingStartup(typeof(StudyMaterialsSharingSystem.Areas.Identity.IdentityHostingStartup))]
namespace StudyMaterialsSharingSystem.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}