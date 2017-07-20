using BTX.ReportViewer.EntityModel;
using System.Collections.Generic;

namespace BTX.ReportViewer.ViewModels
{
    public class ReportsModel
    {
        public string ReportPath;

        public bool IsValid;

        public string ErrorMessage;

        public string CurrentUser;
        public string CurrentUserFirstName;
        public string CurrentUserSurName;
        public string CurrentUserEmail;
        public string SamAccount;
        public Node ReportDirectoryHierarchy;
    }

    public static class LoginUser
    {
        public static string UserName { get; set; }

        public static string Password { get; set; }
    }
}