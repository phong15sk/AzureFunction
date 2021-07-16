using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyAzureProject.Model
{
    public class EmployeeModel
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
    }
}
