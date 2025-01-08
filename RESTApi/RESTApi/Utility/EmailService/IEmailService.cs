namespace RESTApi.Utility.EmailService
{
    interface IEmailService
    {
        void SendEmail(string to, string subject, string body);
    }
}
