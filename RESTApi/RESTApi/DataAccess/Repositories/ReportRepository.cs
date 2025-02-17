﻿using RESTApi.DataAccess.Repositories.IRepository;
using RESTApi.Models;
using static RESTApi.Models.CustomModels;

namespace RESTApi.DataAccess.Repositories
{
    public class ReportRepository : Repository<Report>, IReportRepository
    {
        private ApplicationContext _db;

        public ReportRepository(ApplicationContext db) : base(db)
        {
            _db = db;
        }

        public Report Update(string ReportId, ReportAddOrUpdateModel reportUpdateModel)
        {
            Report reportExist = _db.Reports.Find(ReportId);

            reportExist.TotalAttemptsInEasyQuestions = reportUpdateModel.TotalAttemptsInEasyQuestions >= 0 ? 
                                                       reportUpdateModel.TotalAttemptsInEasyQuestions : reportExist.TotalAttemptsInEasyQuestions;

            reportExist.TotalAttemptsInMediumQuestions = reportUpdateModel.TotalAttemptsInMediumQuestions >= 0 ?
                                                         reportUpdateModel.TotalAttemptsInMediumQuestions : reportExist.TotalAttemptsInMediumQuestions;

            reportExist.TotalAttemptsInHardQuestions = reportUpdateModel.TotalAttemptsInHardQuestions >= 0 ?
                                                       reportUpdateModel.TotalAttemptsInHardQuestions : reportExist.TotalAttemptsInHardQuestions;

            reportExist.CorrectAttempsInEasyQuestions = reportUpdateModel.CorrectAttemptsInEasyQuestions >= 0 ?
                                                        reportUpdateModel.CorrectAttemptsInEasyQuestions : reportExist.CorrectAttempsInEasyQuestions;

            reportExist.CorrectAttemptsInMediumQuestions = reportUpdateModel.CorrectAttemptsInMediumQuestions >= 0 ?
                                                           reportUpdateModel.CorrectAttemptsInMediumQuestions : reportExist.CorrectAttemptsInMediumQuestions;

            reportExist.CorrectAttemptsInHardQuestions = reportUpdateModel.CorrectAttemptsInHardQuestions >= 0 ?
                                                         reportUpdateModel.CorrectAttemptsInHardQuestions : reportExist.CorrectAttemptsInHardQuestions;

            return reportExist;
        }
    }
}