using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LivellPayRoll.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity;

namespace LivellPayRoll.Infrastructure
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {
        public AppIdentityDbContext() : base("PayRollCon") { } 
        static AppIdentityDbContext()
        {
            Database.SetInitializer<AppIdentityDbContext>(new IdentityDbInit());
        }   
        public static AppIdentityDbContext Create()
        {
            DbContext AppIdentityDbContext = HttpContext.Current.Items["AppIdentityDbContext"] as DbContext;
            if (AppIdentityDbContext == null)
            {
                AppIdentityDbContext = new AppIdentityDbContext();
                HttpContext.Current.Items["AppIdentityDbContext"] = AppIdentityDbContext;
            }
            return (AppIdentityDbContext)AppIdentityDbContext;
            //return new AppIdentityDbContext();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //生成映射表为单数
            //modelBuilder.Entity<AppUser>().ToTable("SysUser");
            //modelBuilder.Entity<AppRole>().ToTable("SysRole");
            //modelBuilder.Entity<AppUserLogin>().ToTable("SysUserLogin");
            //modelBuilder.Entity<AppUserClaim>().ToTable("SysUserClaim");
            //modelBuilder.Entity<AppUserRole>().ToTable("SysUserRole");
            //使用Fluent API 去掉外键约束中的级联删除规则
            modelBuilder.Entity<Employee>().HasRequired(e => e.Company).WithMany(c =>c.Employee).HasForeignKey(e => e.CompanyId).WillCascadeOnDelete(false);
            modelBuilder.Entity<Job>().HasRequired(j => j.Company).WithMany(c => c.Job).HasForeignKey(e => e.CompanyId).WillCascadeOnDelete(false);
        }
        public DbSet<Company> Company { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Job> Job { get; set; }
        public DbSet<T102> T102 { get; set; }
        public DbSet<T105> T105 { get; set; }
        public DbSet<T201> T201 { get; set; }
        public DbSet<TimeSheet> TimeSheet { get; set; }
        public DbSet<TimeSheetLog> TimeSheetLog { get; set; }
        public DbSet<DM_TimeZone> DM_TimeZone { get; set; }
        
    }

        public class IdentityDbInit: DropCreateDatabaseIfModelChanges<AppIdentityDbContext>
        {

            protected override void Seed(AppIdentityDbContext context)
            {
                PerformInitialSetup(context);
                base.Seed(context);
            }

            public void PerformInitialSetup(AppIdentityDbContext context)
            {
            // initial configuration will go here
            // 初始化配置将放在这儿 eg:初始化数据
            context.Company.Add(new Company
            {
                CompanyName = "Hoya.Soft Company DTL",
                FedTaxId = "999999999",
                ContactName = "Hoya",
                Email= "hoya.xie@gmail.com",
                Address1 = "QLQZP",
                City = "QC",
                State = "9",
                PayFreq = "1",
                Country= "United States",
                TimeZone = "2",
                RoundTo = "15",
                WeekRule=false,
                WeekRuleValue=40,
                DayRule=true,
                DayRuleValue=8,
                DoubeRule=true,
                DoubeRuleValue=12,
                CaliforniaRule=true,
                allowedit=true,
                PayReportByEndingDate = true,
                PayRollRegTime = DateTime.Now,
                ControlNo= "w3eee",
                Establish= "rtete",
                FUTA=9,
                StateID= "retre",
                StateUnemWage=12,
                SUTA= 60000
            });
            context.SaveChanges();
            var company=context.Company.Where<Company>(c => true).FirstOrDefault();
            AppUserManager userMgr = new AppUserManager(new UserStore<AppUser>(context));
            AppRoleManager roleMgr = new AppRoleManager(new RoleStore<AppRole>(context));
            AppRole role01 = new AppRole { Id = "R01", Name = "Admin" };
            AppRole role02 = new AppRole { Id = "R02", Name = "Manager" };
            AppRole role03 = new AppRole { Id = "R03", Name = "Employee" };
            if (!roleMgr.RoleExists("Admin"))
            {
                roleMgr.Create(role01);
            }
            if (!roleMgr.RoleExists("Manager"))
            {
                roleMgr.Create(role02);
            }
            if (!roleMgr.RoleExists("Employee"))
            {
                roleMgr.Create(role03);
            }
            string userName = "hoya.xie@gmail.com";
            string password = "pay123456";
            string email = "hoya.xie@gmail.com";
            AppUser user = userMgr.FindByName(userName);
            if (user == null)
            {
                AppUser NewUser = new AppUser { Email = email, UserName = userName, Company = company, TimeZone = "-5", PayRollUser = "hoya.xie" };
                userMgr.Create(NewUser, password);
                user = userMgr.FindByName(userName);
            }
            if (!userMgr.IsInRole(user.Id, "Admin"))
            {
                userMgr.AddToRole(user.Id, "Admin");
            }
            Initialize_DmTimeZone(context);
            RollPayInitialize(context, company.CompanyId);
            context.SaveChanges();

        }
        /// <summary>
        /// 初始化Dm_States表数据
        /// </summary>
        /// <param name="context">上下文数据操作类</param>
        private void Initialize_DmTimeZone(AppIdentityDbContext context)
        {
            context.DM_TimeZone.Add(new DM_TimeZone { Code = "-12", TimeZone = "(GMT -12:00) Eniwetok, Kwajalein" });
            context.DM_TimeZone.Add(new DM_TimeZone { Code = "-11", TimeZone = "(GMT -11:00) Midway Island, Samoa" });
            context.DM_TimeZone.Add(new DM_TimeZone { Code = "-10", TimeZone = "(GMT -10:00) Hawaii" });
            context.DM_TimeZone.Add(new DM_TimeZone { Code = "-9", TimeZone = "(GMT -9:00) Alaska" });
            context.DM_TimeZone.Add(new DM_TimeZone { Code = "-8", TimeZone = "(GMT -8:00) Pacific Time (US &amp; Canada)" });
            context.DM_TimeZone.Add(new DM_TimeZone { Code = "-7", TimeZone = "(GMT -7:00) Mountain Time (US &amp; Canada)" });
            context.DM_TimeZone.Add(new DM_TimeZone { Code = "-6", TimeZone = "(GMT -6:00) Central Time (US &amp; Canada), Mexico City" });
            context.DM_TimeZone.Add(new DM_TimeZone { Code = "-5", TimeZone = "(GMT -5:00) Eastern Time (US &amp; Canada), Bogota, Lima" });
            context.DM_TimeZone.Add(new DM_TimeZone { Code = "-4", TimeZone = "(GMT -4:00) Eastern Time (US &amp; Canada), Bogota, Lima" });
            context.DM_TimeZone.Add(new DM_TimeZone { Code = "-3.5", TimeZone = "(GMT -3:30) Newfoundland" });
            context.DM_TimeZone.Add(new DM_TimeZone { Code = "-3", TimeZone = "(GMT -3:00) Brazil, Buenos Aires, Georgetown" });
            context.DM_TimeZone.Add(new DM_TimeZone { Code = "-2", TimeZone = "(GMT -2:00) Mid-Atlantic" });
            context.DM_TimeZone.Add(new DM_TimeZone { Code = "-2.5", TimeZone = "(GMT -1:00 hour) Azores, Cape Verde Islands" });
            context.DM_TimeZone.Add(new DM_TimeZone { Code = "0", TimeZone = "(GMT) Western Europe Time, London, Lisbon, Casablanca" });
            context.DM_TimeZone.Add(new DM_TimeZone { Code = "1", TimeZone = "(GMT +1:00 hour) Brussels, Copenhagen, Madrid, Paris" });
            context.DM_TimeZone.Add(new DM_TimeZone { Code = "2", TimeZone = "(GMT +2:00) Kaliningrad, South Africa" });
            context.DM_TimeZone.Add(new DM_TimeZone { Code = "3", TimeZone = "(GMT +3:00) Baghdad, Riyadh, Moscow, St. Petersburg" });
            context.DM_TimeZone.Add(new DM_TimeZone { Code = "3.5", TimeZone = "(GMT +3:30) Tehran" });
            context.DM_TimeZone.Add(new DM_TimeZone { Code = "4", TimeZone = "(GMT +4:00) Abu Dhabi, Muscat, Baku, Tbilisi" });
            context.DM_TimeZone.Add(new DM_TimeZone { Code = "4.5", TimeZone = "(GMT +4:30) Kabul" });
            context.DM_TimeZone.Add(new DM_TimeZone { Code = "5", TimeZone = "(GMT +5:00) Ekaterinburg, Islamabad, Karachi, Tashkent" });
            context.DM_TimeZone.Add(new DM_TimeZone { Code = "5.5", TimeZone = "(GMT +5:30) Bombay, Calcutta, Madras, New Delhi" });
            context.DM_TimeZone.Add(new DM_TimeZone { Code = "5.75", TimeZone = "(GMT +5:45) Kathmandu" });
            context.DM_TimeZone.Add(new DM_TimeZone { Code = "6", TimeZone = "(GMT +6:00) Almaty, Dhaka, Colombo" });
            context.DM_TimeZone.Add(new DM_TimeZone { Code = "7", TimeZone = "(GMT +7:00) Bangkok, Hanoi, Jakarta" });
            context.DM_TimeZone.Add(new DM_TimeZone { Code = "8", TimeZone = "(GMT +8:00) Beijing, Perth, Singapore, Hong Kong" });
            context.DM_TimeZone.Add(new DM_TimeZone { Code = "9", TimeZone = "(GMT +9:00) Tokyo, Seoul, Osaka, Sapporo, Yakutsk" });
            context.DM_TimeZone.Add(new DM_TimeZone { Code = "9.5", TimeZone = "(GMT +9:30) Adelaide, Darwin" });
            context.DM_TimeZone.Add(new DM_TimeZone { Code = "10", TimeZone = "(GMT +10:00) Eastern Australia, Guam, Vladivostok" });
            context.DM_TimeZone.Add(new DM_TimeZone { Code = "11", TimeZone = "(GMT +11:00) Magadan, Solomon Islands, New Caledonia" });
            context.DM_TimeZone.Add(new DM_TimeZone { Code = "12", TimeZone = "(GMT +12:00) Auckland, Wellington, Fiji, Kamchatka" });
            context.SaveChanges();
        }
        private void RollPayInitialize(AppIdentityDbContext Context,int CompanyId) {
            Context.T102.Add(new T102 { Id = Guid.NewGuid(), ItemId = 1, CodeMap = "F1231", Description= "Health Insurance", AnnualLimit=1000, CompanyId= CompanyId, Type=1 });
            Context.T102.Add(new T102 { Id = Guid.NewGuid(), ItemId = 2, CodeMap = "F1232", Description = "401K", AnnualLimit = 1000, CompanyId = CompanyId, Type=1 });
            Context.T102.Add(new T102 { Id = Guid.NewGuid(), ItemId = 3, CodeMap = "F1233", Description = "SDI (State Disablility)", AnnualLimit = 0, CompanyId = CompanyId, Type=2 });
            Context.T102.Add(new T102 { Id = Guid.NewGuid(), ItemId = 4, CodeMap = "F1234", Description = "Changeable Deduction", AnnualLimit = 955.85, CompanyId = CompanyId, Type = 2 });
            Context.T102.Add(new T102 { Id = Guid.NewGuid(), ItemId = 5, CodeMap = "F1235", Description = "Changeable Deduction", AnnualLimit = 0, CompanyId = CompanyId, Type = 2 });
            Context.T102.Add(new T102 { Id = Guid.NewGuid(), ItemId = 6, CodeMap = "F1236", Description = "Changeable Deduction", AnnualLimit = 0, CompanyId = CompanyId, Type = 2 });
            Context.T102.Add(new T102 { Id = Guid.NewGuid(), ItemId = 7, CodeMap = "F1237", Description = "Changeable Deduction", AnnualLimit = 120, CompanyId = CompanyId, Type = 2 });

            Context.T201.Add(new Models.T201 { Id = Guid.NewGuid(), Ord=1, CodeMap = "F102", Description = "Yealy Salary", Enabled = true, Type=1, CompanyId = CompanyId });
            Context.T201.Add(new Models.T201 { Id = Guid.NewGuid(), Ord=2, CodeMap = "F100", Description = "Regular Hourly Pay", Enabled = true, Type = 1, CompanyId = CompanyId });
            Context.T201.Add(new Models.T201 { Id = Guid.NewGuid(), Ord=3, CodeMap = "F101", Description = "Overtime Hourly Pay", Enabled = true, Type = 1, CompanyId = CompanyId });
            Context.T201.Add(new Models.T201 { Id = Guid.NewGuid(), Ord=4, CodeMap = "SickRate", Description = "Holiday Add", Enabled = false, Type = 1, CompanyId = CompanyId });
            Context.T201.Add(new Models.T201 { Id = Guid.NewGuid(), Ord=5, CodeMap = "VacationRate", Description = "Night Shift Add", Enabled = false, Type = 1, CompanyId = CompanyId });
            Context.T201.Add(new Models.T201 { Id = Guid.NewGuid(), Ord=6, CodeMap = "0000", Description = "Bonus", Enabled = false, Type = 2, CompanyId = CompanyId });
            Context.T201.Add(new Models.T201 { Id = Guid.NewGuid(), Ord=7, CodeMap = "0000", Description = "Director Fee", Enabled = false, Type = 2, CompanyId = CompanyId });
            Context.T201.Add(new Models.T201 { Id = Guid.NewGuid(), Ord=8, CodeMap = "0000", Description = "Tips", Enabled = false, Type = 2, CompanyId = CompanyId });
            Context.T201.Add(new Models.T201 { Id = Guid.NewGuid(), Ord=9, CodeMap = "0000", Description = "Wage1", Enabled = false, Type = 2, CompanyId = CompanyId });
            Context.T201.Add(new Models.T201 { Id = Guid.NewGuid(), Ord=10, CodeMap = "0000", Description = "Wage1", Enabled = false, Type = 2, CompanyId = CompanyId });
            Context.SaveChanges();
        }
    }
    }
    