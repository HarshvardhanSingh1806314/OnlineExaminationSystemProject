using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrontEnd.Models
{
    public class TestDropDown
    {
        public string TestName { set; get; }
        public List<SelectListItem> Items { set; get; }
    }
}