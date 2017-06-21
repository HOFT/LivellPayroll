﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using LivellPayRoll.Models;
using System.Security.Cryptography;

namespace LivellPayRoll.Infrastructure
{
    public class AppUserManager: UserManager<AppUser>
    {
        public AppUserManager(IUserStore<AppUser> store): base(store)
        {
            //采用老的加密程序
            this.PasswordHasher = new OldSystemPasswordHasher();
        }
        public static AppUserManager Create(IdentityFactoryOptions<AppUserManager> options,IOwinContext context)
        {
            AppIdentityDbContext db = context.Get<AppIdentityDbContext>();
        
            AppUserManager manager = new AppUserManager(new UserStore<AppUser>(db));
            //AppUserManager manager = new AppUserManager(new UserStore<AppUser>(db));
            //设置密码策略
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = true,
                RequireUppercase = true
            };
            //设置用户名策略
            manager.UserValidator = new UserValidator<AppUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = true
            };
            manager.EmailService = new EmailService();

            //Account Lockout锁住帐号
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            var dataProtectionProvider = options.DataProtectionProvider;
            if(dataProtectionProvider!= null) 
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<AppUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }

            return manager;
        }

        /// <summary> 
        /// Use Custom approach to verify password 
        /// </summary> 
        public class OldSystemPasswordHasher : PasswordHasher
        {

            /// <summary>
            /// 对密码进行Hash加密
            /// </summary>
            /// <param name="password"></param>
            /// <returns></returns>
            public override string HashPassword(string password)
            {
                byte[] salt;
                byte[] buffer2;
                if (password == null)
                {
                    throw new ArgumentNullException("password");
                }
                using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
                {
                    salt = bytes.Salt;
                    buffer2 = bytes.GetBytes(0x20);
                }
                byte[] dst = new byte[0x31];
                Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
                Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
                return Convert.ToBase64String(dst);
            }

            /// <summary>
            /// 重写验证密码的方法
            /// </summary>
            /// <param name="hashedPassword">加密后的密码</param>
            /// <param name="providedPassword">提供的密码</param>
            /// <returns></returns>
            public override PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
            {
                byte[] buffer4;
                if (hashedPassword == null)
                {
                    return PasswordVerificationResult.Failed;
                }
                if (string.IsNullOrEmpty(providedPassword))
                {
                    throw new ArgumentNullException("providedPassword");
                }
                byte[] src = Convert.FromBase64String(hashedPassword);
                if ((src.Length != 0x31) || (src[0] != 0))
                {
                    return PasswordVerificationResult.Failed;
                }
                byte[] dst = new byte[0x10];
                Buffer.BlockCopy(src, 1, dst, 0, 0x10);
                byte[] buffer3 = new byte[0x20];
                Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
                using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(providedPassword, dst, 0x3e8))
                {
                    buffer4 = bytes.GetBytes(0x20);
                }

                if (ByteEqual(buffer3, buffer4))
                {
                    return PasswordVerificationResult.Success;
                }
                else
                {
                    return PasswordVerificationResult.Failed;
                }
            }

            /// <summary>
            /// 比较两个字节数组
            /// </summary>
            /// <param name="b1"></param>
            /// <param name="b2"></param>
            /// <returns></returns>
            private static bool ByteEqual(byte[] b1, byte[] b2)
            {
                if (b1.Length != b2.Length) return false;
                if (b1 == null || b2 == null) return false;
                for (int i = 0; i < b1.Length; i++)
                {
                    if (b1[i] != b2[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        

    }
}