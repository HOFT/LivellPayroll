using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace LivellPayRoll.Configurations
{
    public class MailConfig : ConfigurationSection
    {
        /// <summary>  
        /// 注册时是否需要验证邮箱  
        /// </summary>  
        [ConfigurationProperty("requireValid", DefaultValue = "false", IsRequired = true)]
        public bool RequireValid
        {
            get
            {
                return (bool)this["requireValid"];
            }
            set
            {
                this["requireValid"] = value;
            }
        }
        /// <summary>  
        /// SMTP服务器  
        /// </summary>  
        [ConfigurationProperty("SmtpServer", IsRequired = true)]
        public string SmtpServer
        {
            get
            {
                return (string)this["SmtpServer"];
            }
            set
            {
                this["SmtpServer"] = value;
            }
        }
        /// <summary>  
        /// 默认端口25（设为-1让系统自动设置）  
        /// </summary>  
        [ConfigurationProperty("SmtpPort", DefaultValue = "25", IsRequired = true)]
        public int SmtpPort
        {
            get
            {
                return (int)this["SmtpPort"];
            }
            set
            {
                this["SmtpPort"] = value;
            }
        }
        /// <summary>  
        /// 地址  
        /// </summary>  
        [ConfigurationProperty("EmailAddress", IsRequired = true)]
        public string EmailAddress
        {
            get
            {
                return (string)this["EmailAddress"];
            }
            set
            {
                this["EmailAddress"] = value;
            }
        }
        /// <summary>  
        /// 账号  
        /// </summary>  
        [ConfigurationProperty("EmailUserName", IsRequired = true)]
        public string EmailUserName
        {
            get
            {
                return (string)this["EmailUserName"];
            }
            set
            {
                this["EmailUserName"] = value;
            }
        }
        /// <summary>  
        /// 密码  
        /// </summary>  
        [ConfigurationProperty("EmailPwd", IsRequired = true)]
        public string EmailPwd
        {
            get
            {
                return (string)this["EmailPwd"];
            }
            set
            {
                this["EmailPwd"] = value;
            }
        }
        /// <summary>  
        /// 是否使用SSL连接  
        /// </summary>  
        [ConfigurationProperty("EnableSSL", DefaultValue = "false", IsRequired = false)]
        public bool EnableSSL
        {
            get
            {
                return (bool)this["EnableSSL"];
            }
            set
            {
                this["EnableSSL"] = value;
            }
        }
        /// <summary>  
        ///   
        /// </summary>  
        [ConfigurationProperty("EnablePwdCheck", DefaultValue = "false", IsRequired = false)]
        public bool EnablePwdCheck
        {
            get
            {
                return (bool)this["EnablePwdCheck"];
            }
            set
            {
                this["EnablePwdCheck"] = value;
            }
        }
    }
}