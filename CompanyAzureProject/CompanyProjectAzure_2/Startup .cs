using CompanyProjectAzure_2;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(CompanyProjectAzure_2.Startup))]

namespace CompanyProjectAzure_2
{
    class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            string SqlConnection = Environment.GetEnvironmentVariable("SqlConnectionString");
            builder.Services.AddDbContext<BloggingContext>(
                options => options.UseSqlServer(SqlConnection));
        }
    }
}