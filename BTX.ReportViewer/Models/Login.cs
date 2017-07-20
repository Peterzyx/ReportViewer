using System.ComponentModel.DataAnnotations;

namespace BTX.ReportViewer.Models
{
    public class Login
    {
        [Required(ErrorMessage = "User name required", AllowEmptyStrings = false)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password required", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}