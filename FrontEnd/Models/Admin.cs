using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrontEnd.Models
{
    public class Admin
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string EmployeeEmail { get; set; }
        public string Password { get; set; }
        public string OrganizationName { get; set; }
        public int EmployeeId { get; set; }
    }
}