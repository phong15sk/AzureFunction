using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionHaveEnityFrameWorks.Model
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> dbContextOptions) : base(dbContextOptions) { }
        public DbSet<Employee> Employees
        {
            get;
            set;
        }
    }
}
