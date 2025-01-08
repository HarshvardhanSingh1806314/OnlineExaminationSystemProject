﻿using RESTApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTApi.DataAccess.Repositories.IRepository
{
    public interface IReportRepository : IRepository<Report>
    {
        Report Update(Report report);
    }
}
