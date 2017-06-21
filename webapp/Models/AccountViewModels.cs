#region Using

using System.ComponentModel.DataAnnotations;

#endregion

namespace LivellPayRoll.Models
{
    public class AccountLoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class AccountForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

    public class AccountResetPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }
        public string Code { get; set; }
        public string UserId { get; set; }
    }

    public class AccountRegistrationModel
    {
        [StringLength(128)]
        [Required]
        public string CompanyName { get; set; }
        [StringLength(256)]
        [Required]
        public string Address { set; get; }
        [StringLength(64)]
        [Required]
        public string City { set; get; }
        [StringLength(2)]
        [Required]
        public string State { set; get; }
        [StringLength(16)]
        public string Telphone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [StringLength(8)]
        [Required]
        public string TimeZone { set; get; }    //时区
        //[StringLength(64)]
        //[Required]
        //public string ContactName { get; set; }  //联络人（注册用户的管理员）
    }
    public class CompleteRegistModel
    {
        public string code { set; get; }
        [StringLength(128)]
        public string CompanyName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { set; get; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }
        [StringLength(64)]
        [Display(Name = "Admin Name")]
        public string ContactName { get; set; }  //联络人（注册用户的管理员）
        [StringLength(128)]
        [EmailAddress]
        [Required]
        public string Email { set; get; }  //公司邮箱
    }
}