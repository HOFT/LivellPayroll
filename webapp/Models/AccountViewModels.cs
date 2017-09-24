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
        [StringLength(16)]
        [Required]
        public string Zip { set; get; }
        [StringLength(64)]
        public string TradeName { set; get; }
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
        [Required]
        [StringLength(64)]
        [Display(Name = "First Name")]
        public string FName { get; set; }
        [StringLength(64)]
        [Required]
        [Display(Name = "Last Name")]
        public string LName { get; set; }
        [StringLength(16)]
        [Required]
        public string SSN { get; set; }
        [StringLength(16)]
        [Required]
        public string Telephone { get; set; }
        [StringLength(128)]
        [EmailAddress]
        [Required]
        public string Email { set; get; }  //公司邮箱
    }
    public class GeneralSetModel {
        [StringLength(128)]
        [EmailAddress]
        public string Email { set; get; }
        [StringLength(128)]
        public string RoleId { set; get; }
        [StringLength(256)]
        public string PayRollUser { set; get; }
        [StringLength(8)]
        public string TimeZone { get; set; }
        [StringLength(256)]
        public string Address { get; set; }
        [StringLength(64)]
        public string City { get; set; }
        [StringLength(2)]
        public string State { get; set; }
        [StringLength(16)]
        public string ZipCode { get; set; }
        [StringLength(16)]
        public string Phone { get; set; }
        [StringLength(11)]
        public string SSN { get; set; }
        public int DefaultJob { get; set; }
    }
    public class PasswordSetModel {
        public string code { set; get; }
        public string UserId { set; get; }
        [DataType(DataType.Password)]
        public string Password { set; get; }
        [DataType(DataType.Password)]
        public string NewPassword { set; get; }
        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string PasswordConfirm { get; set; }
    }
    public class ManagerUserMode {
        public string UserId { set; get; }
        [Required]
        public string UserName { set; get; }
        [Required]
        [EmailAddress]
        public string Email { set; get; }
        public string Phone { set; get; }
    }
    public class UserConfirmModel
    {
        public string Code { set; get; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { set; get; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }
        [Required]
        public string Id { set; get; }  //公司邮箱
    }
    public class DataTableJobItemModel
    {
        public int JobId { set; get; }
        public string JobName { set; get; }
        public int PCount { set; get; }
        public double ThisTime { set; get; }
        public double TimeTotal { set; get; }
        public string PCT { set; get; }
        public string BarColor { set; get; }
    }

}