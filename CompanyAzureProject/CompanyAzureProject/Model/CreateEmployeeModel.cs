using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyAzureProject.Model
{
    public class CreateEmployeeModel
    {
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public string Name { get; set; }
        public string Position { get; set; }
    }
}
