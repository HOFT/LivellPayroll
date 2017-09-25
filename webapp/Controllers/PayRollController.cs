using LivellPayroll;
using LivellPayRoll.App_Helpers;
using LivellPayRoll.Enum;
using LivellPayRoll.Infrastructure;
using LivellPayRoll.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LivellPayRoll.Controllers
{
    [Authorize]
    [CustomAuthorize]
    public class PayRollController : Controller
    {
        private float summary1;
        private float summary2;
        private float summary3;
        private float summary4;
        private float summary5;
        private float summary6;
        private float summary7;
        private float summary8;
        private float summary9;
        private float summary10;
        private float summary11;
        private float summary12;
        private float summary13;
        private float summary14;
        private float summary15;
        private float summary16;
        private float summary16X2;
        private float summary17;
        private float summary18;
        private float summary19;
        private float summary20;
        private float summary21;
        private float summary22;
        private float summary23;
        private float summary24;
        private float summary25;
        private float summary26;
        private float summary27;
        private float summary28;
        private float summary29;
        private float summary30;
        private float summary31;
        private float summary32;

        private bool lbSalary = false;
        private bool txtSalaryAnnual = false;
        private bool label15 = false;
        private bool txtPaySalary = false;
        private bool txtPaySalaryYTD = false;

        private bool lbRegularHourly = false;
        private bool txtPayHourly = false;
        private bool txtHrsRegular = false;
        private bool txtPayRegular = false;
        private bool txtPayRegularYTD = false;


        private bool lbOvertimeHourly = false;
        private bool txtOTMultiplier = false;
        private bool txtHrsOT = false;
        private bool txtPayOT = false;
        private bool txtPayOTYTD = false;

        private bool lbVacationHourly = false;
        private bool txtPayHourlyVacation = false;
        private bool txtHrsVacation = false;
        private bool txtPayVacation = false;
        private bool txtPayVacationYTD = false;

        private bool lbDoubleHourly = false;
        private bool txtDoublePayHourly = false;
        private bool txtHrsDT = false;
        private bool txtPayDT = false;
        private bool txtPayDTYTD = false;

        private bool lbSickHourly = false;
        private bool txtPayHourlySick = false;
        private bool txtHrsSick = false;
        private bool txtPaySick = false;
        private bool txtPaySickYTD = false;

        private bool lbAdvanceEIC = false;
        private bool label36 = false;
        private bool label39 = false;
        private bool txtAdvanceEIC = false;
        private bool txtAdvanceEICYTD = false;

        private bool lbUDPay1Desc = false;
        private bool label31 = false;
        private bool label16 = false;
        private bool txtUDPay1Val = false;
        private bool txtUDPay1ValYTD = false;

        private bool lbUDPay2Desc = false;
        private bool label32 = false;
        private bool label17 = false;
        private bool txtUDPay2Val = false;
        private bool txtUDPay2ValYTD = false;

        private bool lbUDPay3Desc = false;
        private bool label33 = false;
        private bool label18 = false;
        private bool txtUDPay3Val = false;
        private bool txtUDPay3ValYTD = false;

        private bool lbUDPay4Desc = false;
        private bool label34 = false;
        private bool label19 = false;
        private bool txtUDPay4Val = false;
        private bool txtUDPay4ValYTD = false;

        private bool lbUDPay5Desc = false;
        private bool label35 = false;
        private bool label20 = false;
        private bool txtUDPay5Val = false;
        private bool txtUDPay5ValYTD = false;

        private bool lbUD1 = false;
        private bool txtUD1Input = false;
        private bool label23 = false;
        private bool txtUD1Val = false;
        private bool lbUD1ValYTD = false;

        private bool lbUD2 = false;
        private bool txtUD2Input = false;
        private bool label24 = false;
        private bool txtUD2Val = false;
        private bool lbUD2ValYTD = false;

        private bool lbUD3 = false;
        private bool txtUD3Input = false;
        private bool label25 = false;
        private bool txtUD3Val = false;
        private bool lbUD3ValYTD = false;

        private bool lbUD4 = false;
        private bool txtUD4Input = false;
        private bool label26 = false;
        private bool txtUD4Val = false;
        private bool lbUD4ValYTD = false;

        private bool lbUD5 = false;
        private bool txtUD5Input = false;
        private bool label27 = false;
        private bool txtUD5Val = false;
        private bool lbUD5ValYTD = false;

        private bool lbUD6 = false;
        private bool txtUD6Input = false;
        private bool label43 = false;
        private bool txtUD6Val = false;
        private bool lbUD6ValYTD = false;

        private bool lbUD7 = false;
        private bool txtUD7Input = false;
        private bool label47 = false;
        private bool txtUD7Val = false;
        private bool lbUD7ValYTD = false;

        public ActionResult PayRollList()
        {
            AppUser user = LoginUser;
            var t105 = db.T105.Where(t => t.CompanyId == user.CompanyId).OrderBy(t => t.DateSubmitted).ToList();
            if (Request["EmpId"] != null)
            {
                Guid EmpId = new Guid(Request["EmpId"]);
                t105 = db.T105.Where(t => t.CompanyId == user.CompanyId && t.EmployeeId == EmpId).OrderBy(t => t.DateSubmitted).ToList();
            };
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Id");
            dataTable.Columns.Add("Ord");
            dataTable.Columns.Add("CheckNum");
            dataTable.Columns.Add("Name");
            dataTable.Columns.Add("SSN");
            dataTable.Columns.Add("CheckAmount");
            dataTable.Columns.Add("PayPeriodEnding");
            dataTable.Columns.Add("CheckDate");
            dataTable.Columns.Add("Memo");
            foreach (var t in t105) {
                DataRow dr = dataTable.NewRow();
                dr["Id"] = t.Id;
                dr["Ord"] = t105.IndexOf(t) + 1;
                dr["CheckNum"] = t.CheckNo;
                dr["Name"] = t.FName + " " + t.LName;
                dr["SSN"] = t.SSN;
                dr["CheckAmount"] = t.S122;
                dr["PayPeriodEnding"] = t.DateEndPeriod.ToString("yyyy-MM-dd");
                dr["PayPeriodEnding"] = t.DateSubmitted.ToString("yyyy-MM-dd");
                dr["Memo"] = t.Memo;
                dataTable.Rows.Add(dr);
            }
            dataTable.DefaultView.Sort = "Ord asc";

            return View(dataTable);
        }
        public ActionResult PayRollDelete(System.Guid Id) {
            T105 t = db.T105.Find(Id);
            db.Entry<T105>(t).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();

            var dirpath = Server.MapPath(ConfigurationManager.AppSettings["QRCodePath"]);
            ZXingHelpers.FileDelte(dirpath + Id.ToString() + ".png");
            return Json(new { code = "1", message = "success!" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PayRollInput()
        {
            Company c = db.Company.Find(LoginUser.Company.CompanyId);
            //ViewBag.Period = ((Period)(int.Parse(c.PayFreq))).ToString();
            ViewBag.Period = c.PayFreq;

            return View();
        }
        public JsonResult GetEmployee()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            var Employees = db.Employee.Where<Employee>(e => e.CompanyId == LoginUser.Company.CompanyId && e.F124 == 0).ToList();
            foreach (var e in Employees)
            {
                items.Add(new SelectListItem() { Text = e.LName + " " + e.FName + " [" + e.Email + "]", Value = e.EmployeeId.ToString() });
            }

            return Json(items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetEmpDes() {
            if (Request["EmpId"] == null || Request["EmpId"] == "")
                return Json(new { dicValue = "", dicVisible = "" }, JsonRequestBehavior.AllowGet);
            Guid EmpId = new Guid(Request["EmpId"]);

            T108 t = LoadEmployee(EmpId);
            t = CalcPayrollAll(t);
            Dictionary<string, object> dicValue = DisplayControlInfo(t);
            Dictionary<string, bool> dicVisible = DisplayControls(t);

            return Json(new { dicValue = dicValue, dicVisible = dicVisible }, JsonRequestBehavior.AllowGet);
        }
        public T108 FullIncomeHous(T108 t108,Guid EmpId,DateTime StartDate,DateTime EndDate) {
            float Regulartime = 0;
            float Overtime = 0;
            float Doubletime = 0;
            float Holidaytime = 0;
            float NightShifttime = 0;
            Company c = db.Company.Find(LoginUser.CompanyId);
            var TimeSheetList = db.TimeSheet.Where(t => t.EmployeeId == EmpId && t.StartDate >= StartDate && t.StartDate <= EndDate && t.Status == "2").OrderBy(t => t.StartDate).ToList();

            Regulartime = (float)TimeSheetList.Sum(t => t.RegulaWorkTime);
            Overtime = (float)TimeSheetList.Sum(t => t.OverTimeWorkTime);
            Doubletime = (float)TimeSheetList.Sum(t => t.DoubleWorkTime);
            Holidaytime = (float)TimeSheetList.Where(t => t.TimeSheetType == 5).Sum(t => t.TotalWorkTime);
            NightShifttime = (float)TimeSheetList.Where(t => t.TimeSheetType == 6).Sum(t => t.TotalWorkTime);
            t108.S100 = Regulartime / 60;
            t108.S101 = Overtime / 60;
            t108.S1002 = Doubletime / 60;
            t108.S103 = Holidaytime / 60;
            t108.S102 = NightShifttime / 60;
            return t108;
        }
        public JsonResult GetIncomeHous() {
            float Regulartime = 0;
            float Overtime = 0;
            float Doubletime = 0;
            float Holidaytime = 0;
            float NightShifttime = 0;
            Guid EmpId = new Guid(Request["EmpId"]);
            DateTime StartDate = DateTime.Parse(Request["StartDate"]);
            DateTime EndDate = DateTime.Parse(Request["EndDate"]).AddDays(1);
            Company c = db.Company.Find(LoginUser.CompanyId);

            var TimeSheetList = db.TimeSheet.Where(t => t.EmployeeId == EmpId && t.StartDate >= StartDate && t.StartDate <= EndDate && t.Status == "2").OrderBy(t => t.StartDate).ToList();

            Regulartime = (float)TimeSheetList.Sum(t => t.RegulaWorkTime);
            Overtime = (float)TimeSheetList.Sum(t => t.OverTimeWorkTime);
            Doubletime = (float)TimeSheetList.Sum(t => t.DoubleWorkTime);
            Holidaytime = (float)TimeSheetList.Where(t=>t.TimeSheetType==5).Sum(t => t.TotalWorkTime);
            NightShifttime = (float)TimeSheetList.Where(t => t.TimeSheetType == 6).Sum(t => t.TotalWorkTime);

            Dictionary<string, float> dic = new Dictionary<string, float>();
            dic.Add("txtHrsRegular", Regulartime / 60);
            dic.Add("txtHrsOT", Overtime / 60);
            dic.Add("txtHrsDT", Doubletime / 60);
            dic.Add("txtHrsSick", Holidaytime / 60);
            dic.Add("txtHrsVacation", NightShifttime / 60);
            return Json(dic, JsonRequestBehavior.AllowGet);
        }
        public JsonResult PayrollInputSaveCheck() {
            Guid EmpId = new Guid(Request["EmpId"]);
            T108 t = LoadPageData();
            t = CalcPayrollAll(t);
            //先决条件判断
            if (t.DateEndPeriod < t.DateStartPeriod)
            {
                return Json(new { code = 2, status = "Error", message = "The check ending date is earlier than starting date. Please correct the date!" }, JsonRequestBehavior.AllowGet);
            };
            var table1 = db.T105.Where(s => s.EmployeeId == EmpId && s.DateEndPeriod >= t.DateEndPeriod).ToList();
            if (table1.Count > 0)
            {
                var table2 = db.T105.Where(s => s.EmployeeId == EmpId && s.DateEndPeriod >= t.DateEndPeriod).GroupBy(s => s.Id).Select(s => s.Min(o => o.DateEndPeriod)).ToList();
                DateTime MaxDateEndPeriod = t.DateEndPeriod.AddYears(-1);
                foreach (var p in table2)
                {
                    MaxDateEndPeriod = p;
                }
                if (MaxDateEndPeriod.ToString("yyyy-MM-dd") == t.DateEndPeriod.ToString("yyyy-MM-dd"))
                {
                    return Json(new { code = 3, status = "Error", message = "The check with ending date " + t.DateEndPeriod.ToString("yyyy-MM-dd") + " exists.Do you want to overwrite the existing check ?" }, JsonRequestBehavior.AllowGet);
                }
                else {
                    string message = "There are existing checks with ending date after " + t.DateEndPeriod.ToString("yyyy-MM-dd") + " . This may cause ezPayCheck to create a check that is out of sequentital order. "
                        + "< br />  This may cause calculation problems for taxes and deductions with wagebase and cutoff limits. Do you want to continue save this check?";
                    return Json(new { code = 3, status = "Error", message = message }, JsonRequestBehavior.AllowGet);
                }
            };
            return Json(new { code = 1, status = "success", message = "pass!" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult PayrollMemoSave(){
            Guid Id = new Guid(Request["Id"]);
            string Memo = Request["Memo"];
            T105 t = db.T105.Find(Id);
            t.Memo = Memo;
            db.Entry<T105>(t).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Json(new { code = 1, status = "success", message = "success" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult PayrollInputSave() {

            T108 t = LoadPageData();
            t = CalcPayrollAll(t);

            T105 t105 = new T105
            {
                Id = Guid.NewGuid(),
                DateStartPeriod = t.DateStartPeriod,
                DateEndPeriod = t.DateEndPeriod,
                DateSubmitted = t.DateSubmitted,
                EmployeeId = t.EID,
                LName = t.LName,
                FName = t.FName,
                MInit = t.MInit,
                SSN = t.SSN,
                F99 = t.F99,
                Address1 = t.Address1,
                Address2 = t.Address2,
                City = t.City,
                ZipCode = t.ZipCode,
                State = t.State,
                Phone = t.Phone,
                F100 = t.F100,
                F101 = t.F101,
                F102 = t.F102,
                F1002 = t.F1002,
                F103 = t.F103,
                F104 = t.F104,
                F105 = t.F105,
                S100 = t.S100,
                S1002 = t.S1002,
                S101 = t.S101,
                S102 = t.S102,
                S103 = t.S103,
                S104 = t.S104,
                S105 = t.S105,
                S1052 = t.S1052,
                S106 = t.S106,
                S107 = t.S107,
                S108 = t.S108,
                S109 = t.S109,
                S110 = t.S110,
                S111 = t.S111,
                S112 = t.S112,
                S113 = t.S113,
                S114 = t.S114,
                S115 = t.S115,
                S116 = t.S116,
                S117 = t.S117,
                S118 = t.S118,
                S119 = t.S119,
                S120 = t.S120,
                S121 = t.S121,
                S122 = t.S122,
                F106 = t.F106,
                F107 = t.F107,
                F108 = t.F108,
                F109 = t.F109,
                F110 = t.F110,
                F111 = t.F111,
                F112 = t.F112,
                F113 = t.F113,
                F114 = t.F114,
                F115 = t.F115,
                F116 = t.F116,
                F117 = t.F117,
                F118 = t.F118,
                F119 = t.F119,
                F120 = t.F120,
                F121 = t.F121,
                F122 = t.F122,
                S1231 = t.S1231,
                S1241 = t.S1241,
                S1232 = t.S1232,
                S1242 = t.S1242,
                S1233 = t.S1233,
                S1243 = t.S1243,
                S1234 = t.S1234,
                S1244 = t.S1244,
                S1235 = t.S1235,
                S1245 = t.S1245,
                S1251 = t.S1251,
                S1261 = t.S1261,
                S1252 = t.S1252,
                S1262 = t.S1262,
                S1253 = t.S1253,
                S1263 = t.S1263,
                S1254 = t.S1254,
                S1264 = t.S1264,
                S1255 = t.S1255,
                S1265 = t.S1265,
                S127 = t.S127,
                F1231 = t.F1231,
                F1232 = t.F1232,
                F1233 = t.F1233,
                F1234 = t.F1234,
                F1235 = t.F1235,
                F124 = t.F124,
                F125 = t.F125,
                IsW2StatutoryEmployee = t.IsW2StatutoryEmployee,
                IsW2RetirementPlan = t.IsW2RetirementPlan,
                DoesReceiveAdvanceEIC = t.DoesReceiveAdvanceEIC,
                EmployerSocialSecurity = t.EmployerSocialSecurity,
                EmployerMedicare = t.EmployerMedicare,
                AdvanceEIC = t.AdvanceEIC,
                CheckNo = t.CheckNo,
                DateLastEdit = t.DateLastEdit,
                Checked = 0,
                S1236 = t.S1236,
                S1246 = t.S1246,
                S1237 = t.S1237,
                S1247 = t.S1247,
                F1236 = t.F1236,
                F1237 = t.F1237,
                HourlyPay1 = t.HourlyPay1,
                HourlyPay2 = t.HourlyPay2,
                HourlyPay3 = t.HourlyPay3,
                HourlyPay4 = t.HourlyPay4,
                HourlyPay5 = t.HourlyPay5,
                SickRate = t.SickRate,
                VacationRate = t.VacationRate,
                IsPaid = 0,
                is1099Employee = t.is1099Employee,
                PTOAcchours = t.PTOAcchours,
                VacAccHours = t.VacAccHours,
                printPTOStub = t.printPTOStub,
                PTOAccRate = t.PTOAccRate,
                VacAccRate = t.VacAccRate,
                PTOCapHours = t.PTOCapHours,
                VacCapHours = t.VacCapHours,
                isPayrollSetup = 0,
                Memo = t.Memo,
                CompanyId = LoginUser.CompanyId

            };

            Dictionary<string, object> dicValue = DisplayControlInfo(t);

            var table = db.T105.Where(s => s.EmployeeId == t.EID && s.DateEndPeriod == t.DateEndPeriod).ToList();
            if (table.Count > 0) {
                foreach(var a in table) {
                    db.Entry<T105>(a).State = System.Data.Entity.EntityState.Deleted;
                    var Deletepath = Server.MapPath(ConfigurationManager.AppSettings["QRCodePath"]);
                    ZXingHelpers.FileDelte(Deletepath + a.Id + ".png");
                }
            };

            db.T105.Add(t105);
            db.SaveChanges();

            string content = "http://" + Request.Url.Host + ":" + Request.Url.Port + "/Payroll/MPayroll?Id="+ t105.Id.ToString();
            string dirpath = Server.MapPath(ConfigurationManager.AppSettings["QRCodePath"])+t105.Id.ToString() + ".png";
            ZXingHelpers.Generate1(content, dirpath);

            return Json(new { dicValue = dicValue, code = 1, status = "success", message = "success!" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Calculate()
        {
            T108 t = LoadPageData();

            t = CalcPayrollAll(t);
            Dictionary<string, object> dicValue = DisplayControlInfo(t);

            return Json(new { dicValue = dicValue, code = 1, status = "success" }, JsonRequestBehavior.AllowGet);
        }
        private T108 LoadEmployee(Guid EmpId) {
            Company c = db.Company.Find(LoginUser.Company.CompanyId);
            string Period = c.PayFreq;
            Employee e = db.Employee.Where(x => x.EmployeeId == EmpId).SingleOrDefault();
            T108 t = new T108();
            t.EID = e.EmployeeId;
            t.LName = e.LName;
            t.FName = e.FName;
            t.MInit = e.MInit;
            t.SSN = e.SSN;
            t.F99 = e.F99;
            t.Address1 = e.Address1;
            t.Address2 = e.Address2;
            t.City = e.City;
            t.ZipCode = e.ZipCode;
            t.State = e.State;
            t.Phone = e.Phone;
            t.F100 = e.F100;
            t.F101 = e.F101;
            t.F102 = e.F102;
            t.F1002 = e.F1002;
            t.F103 = e.F103;
            t.F104 = e.F104;
            t.F105 = e.F105;
            t.F106 = e.F106;
            t.F107 = e.F107;
            t.F108 = e.F108;
            t.F109 = e.F109;
            t.F110 = e.F110;
            t.F111 = e.F111;
            t.F112 = e.F112;
            t.F113 = e.F113;
            t.F114 = e.F114;
            t.F115 = e.F115;
            t.F116 = e.F116;
            t.F117 = e.F117;
            t.F118 = e.F118;
            t.F119 = e.F119;
            t.F120 = e.F120;
            t.F121 = e.F121;
            t.F122 = e.F122;
            t.F1231 = e.F1231;
            t.F1232 = e.F1232;
            t.F1233 = e.F1233;
            t.F1234 = e.F1234;
            t.F1235 = e.F1235;
            t.F1236 = e.F1236;
            t.F1237 = e.F1237;
            t.F124 = e.F124;
            t.F125 = e.F125;
            t.IsW2StatutoryEmployee = e.IsW2StatutoryEmployee;
            t.IsW2RetirementPlan = e.IsW2RetirementPlan;
            t.DoesReceiveAdvanceEIC = e.DoesReceiveAdvanceEIC;
            t.F1236 = e.F1236;
            t.F1237 = e.F1237;
            t.HourlyPay1 = e.HourlyPay1;
            t.HourlyPay2 = e.HourlyPay2;
            t.HourlyPay3 = e.HourlyPay3;
            t.HourlyPay4 = e.HourlyPay4;
            t.SickRate = e.SickRate;
            t.VacationRate = e.VacationRate;
            t.is1099Employee = e.is1099Employee;
            t.PTOAcchours = e.PTOAcchours;
            t.VacAccHours = e.VacAccHours;
            t.printPTOStub = e.printPTOStub;
            t.PTOAccRate = e.PTOAccRate;
            t.VacAccRate = e.VacAccRate;
            t.PTOCapHours = e.PTOCapHours;
            t.VacCapHours = e.VacCapHours;

            t.DateStartPeriod = Convert.ToDateTime(GetStartDate(Period));
            t.DateEndPeriod = DateTime.Now;
            t.DateSubmitted = DateTime.Now;
            t.DateLastEdit = DateTime.Now;

            t = LoadUDDescIntoTempWageDataTbl(t);

            t.S127 = c.PayFreq;

            t.S116 = c.SUTA;
            t.S117 = c.StateUnemWage;

            //获取Incomes Hours
            t = FullIncomeHous(t, t.EID, t.DateStartPeriod, t.DateEndPeriod);

            return t;
        }
        private T108 LoadPageData()
        {
            Guid EmpId = new Guid(Request["EmpId"]);
            float F102 = Function.GetSafeSingle(Request["F102"]);
            float F100 = Function.GetSafeSingle(Request["F100"]);
            float F1002 = Function.GetSafeSingle(Request["F1002"]);
            float S100 = Function.GetSafeSingle(Request["S100"]);
            float S1002 = Function.GetSafeSingle(Request["S1002"]);
            float F101 = Function.GetSafeSingle(Request["F101"]);
            float VacationRate = Function.GetSafeSingle(Request["VacationRate"]);
            float SickRate = Function.GetSafeSingle(Request["SickRate"]);
            float S101 = Function.GetSafeSingle(Request["S101"]);
            float S111 = Function.GetSafeSingle(Request["S111"]);
            float S109 = Function.GetSafeSingle(Request["S109"]);
            float S110 = Function.GetSafeSingle(Request["S110"]);
            float S112 = Function.GetSafeSingle(Request["S112"]);
            float S113 = Function.GetSafeSingle(Request["S113"]);
            float EmployerSocialSecurity = Function.GetSafeSingle(Request["EmployerSocialSecurity"]);
            float EmployerMedicare = Function.GetSafeSingle(Request["EmployerMedicare"]);
            float S114 = Function.GetSafeSingle(Request["S114"]);
            float S115 = Function.GetSafeSingle(Request["S115"]);
            float AdvanceEIC = Function.GetSafeSingle(Request["AdvanceEIC"]);
            float S102 = Function.GetSafeSingle(Request["S102"]);
            float S103 = Function.GetSafeSingle(Request["S103"]);
            float S1261 = Function.GetSafeSingle(Request["S1261"]);
            float S1262 = Function.GetSafeSingle(Request["S1262"]);
            float S1263 = Function.GetSafeSingle(Request["S1263"]);
            float S1264 = Function.GetSafeSingle(Request["S1264"]);
            float S1265 = Function.GetSafeSingle(Request["S1265"]);
            float F1231 = Function.GetSafeSingle(Request["F1231"]);
            float F1232 = Function.GetSafeSingle(Request["F1232"]);
            float F1233 = Function.GetSafeSingle(Request["F1233"]);
            float F1234 = Function.GetSafeSingle(Request["F1234"]);
            float F1235 = Function.GetSafeSingle(Request["F1235"]);
            float F1236 = Function.GetSafeSingle(Request["F1236"]);
            float F1237 = Function.GetSafeSingle(Request["F1237"]);
            string Memo = Request["Memo"];

            int CheckNo = int.Parse(Request["CheckNo"]);
            DateTime DateSubmitted = Convert.ToDateTime(Request["DateSubmitted"]);
            DateTime DateStartPeriod = Convert.ToDateTime(Request["DateStartPeriod"]);
            DateTime DateEndPeriod = Convert.ToDateTime(Request["DateEndPeriod"]);

            T108 t = LoadEmployee(EmpId);

            t.F102 = (F102 != 0) ? F102 : t.F102;
            t.F100 = (F100 != 0) ? F100 : t.F100;
            t.F1002 = (F1002 != 0) ? F1002 : t.F1002;
            t.S100 = (S100 != 0) ? S100 : t.S100;
            t.S1002 = (S1002 != 0) ? S1002 : t.S1002;
            t.F101 = (F101 != 0) ? F101 : t.F101;
            t.VacationRate = (VacationRate != 0) ? VacationRate : t.VacationRate;
            t.SickRate = (SickRate != 0) ? SickRate : t.SickRate;
            t.S101 = (S101 != 0) ? S101 : t.S101;
            t.S111 = (S111 != 0) ? S111 : t.S111;
            t.S109 = (S109 != 0) ? S109 : t.S109;
            t.S110 = (S110 != 0) ? S110 : t.S110;
            t.S112 = (S112 != 0) ? S112 : t.S112;
            t.S113 = (S113 != 0) ? S113 : t.S113;
            t.EmployerSocialSecurity = (EmployerSocialSecurity != 0) ? EmployerSocialSecurity : t.EmployerSocialSecurity;
            t.EmployerMedicare = (EmployerMedicare != 0) ? EmployerMedicare : t.EmployerMedicare;
            t.S114 = (S114 != 0) ? S114 : t.S114;
            t.S115 = (S115 != 0) ? S115 : t.S115;
            t.AdvanceEIC = (AdvanceEIC != 0) ? AdvanceEIC : t.AdvanceEIC;
            t.S102 = (S102 != 0) ? S102 : t.S102;
            t.S103 = (S103 != 0) ? S103 : t.S103;
            t.S1261 = (S1261 != 0) ? S1261 : t.S1261;
            t.S1262 = (S1262 != 0) ? S1262 : t.S1262;
            t.S1263 = (S1263 != 0) ? S1263 : t.S1263;
            t.S1264 = (S1264 != 0) ? S1264 : t.S1264;
            t.S1265 = (S1265 != 0) ? S1265 : t.S1265;
            t.F1231 = (F1231 != 0) ? F1231 : t.F1231;
            t.F1232 = (F1232 != 0) ? F1232 : t.F1232;
            t.F1233 = (F1233 != 0) ? F1233 : t.F1233;
            t.F1234 = (F1234 != 0) ? F1234 : t.F1234;
            t.F1235 = (F1235 != 0) ? F1235 : t.F1235;
            t.F1236 = (F1236 != 0) ? F1236 : t.F1236;
            t.F1237 = (F1237 != 0) ? F1237 : t.F1237;
            t.Memo = (Memo != null) ? Memo : t.Memo;
            t.CheckNo = CheckNo;
            t.DateSubmitted = DateSubmitted;
            t.DateStartPeriod = DateStartPeriod;
            t.DateEndPeriod = DateEndPeriod;

            return t;
        }
        private Dictionary<string, bool> DisplayControls(T108 t) {
            
            Dictionary<string, bool> dic = new Dictionary<string, bool>();
            dic.Add("lbSalary", lbSalary);
            dic.Add("txtSalaryAnnual", txtSalaryAnnual);
            dic.Add("label15", label15);
            dic.Add("txtPaySalary", txtPaySalary);
            dic.Add("txtPaySalaryYTD", txtPaySalaryYTD);

            dic.Add("lbRegularHourly", lbRegularHourly);
            dic.Add("txtPayHourly", txtPayHourly);
            dic.Add("txtHrsRegular", txtHrsRegular);
            dic.Add("txtPayRegular", txtPayRegular);
            dic.Add("txtPayRegularYTD", txtPayRegularYTD);

            dic.Add("lbOvertimeHourly", lbOvertimeHourly);
            dic.Add("txtOTMultiplier", txtOTMultiplier);
            dic.Add("txtHrsOT", txtHrsOT);
            dic.Add("txtPayOT", txtPayOT);
            dic.Add("txtPayOTYTD", txtPayOTYTD);

            dic.Add("lbDoubleHourly", lbDoubleHourly);
            dic.Add("txtDoublePayHourly", txtDoublePayHourly);
            dic.Add("txtHrsDT", txtHrsDT);
            dic.Add("txtPayDT", txtPayDT);
            dic.Add("txtPayDTYTD", txtPayDTYTD);

            dic.Add("lbVacationHourly", lbVacationHourly);
            dic.Add("txtPayHourlyVacation", txtPayHourlyVacation);
            dic.Add("txtHrsVacation", txtHrsVacation);
            dic.Add("txtPayVacation", txtPayVacation);
            dic.Add("txtPayVacationYTD", txtPayVacationYTD);

            dic.Add("lbSickHourly", lbSickHourly);
            dic.Add("txtPayHourlySick", txtPayHourlySick);
            dic.Add("txtHrsSick", txtHrsSick);
            dic.Add("txtPaySick", txtPaySick);
            dic.Add("txtPaySickYTD", txtPaySickYTD);

            dic.Add("lbAdvanceEIC", lbAdvanceEIC);
            dic.Add("label36", label36);
            dic.Add("label39", label39);
            dic.Add("txtAdvanceEIC", txtAdvanceEIC);
            dic.Add("txtAdvanceEICYTD", txtAdvanceEICYTD);

            dic.Add("lbUDPay1Desc", lbUDPay1Desc);
            dic.Add("label31", label31);
            dic.Add("label16", label16);
            dic.Add("txtUDPay1Val", txtUDPay1Val);
            dic.Add("txtUDPay1ValYTD", txtUDPay1ValYTD);

            dic.Add("lbUDPay2Desc", lbUDPay2Desc);
            dic.Add("label32", label32);
            dic.Add("label17", label17);
            dic.Add("txtUDPay2Val", txtUDPay2Val);
            dic.Add("txtUDPay2ValYTD", txtUDPay2ValYTD);

            dic.Add("lbUDPay3Desc", lbUDPay3Desc);
            dic.Add("label33", label33);
            dic.Add("label18", label18);
            dic.Add("txtUDPay3Val", txtUDPay3Val);
            dic.Add("txtUDPay3ValYTD", txtUDPay3ValYTD);

            dic.Add("lbUDPay4Desc", lbUDPay4Desc);
            dic.Add("label34", label34);
            dic.Add("label19", label19);
            dic.Add("txtUDPay4Val", txtUDPay4Val);
            dic.Add("txtUDPay4ValYTD", txtUDPay4ValYTD);

            dic.Add("lbUDPay5Desc", lbUDPay5Desc);
            dic.Add("label35", label35);
            dic.Add("label20", label20);
            dic.Add("txtUDPay5Val", txtUDPay5Val);
            dic.Add("txtUDPay5ValYTD", txtUDPay5ValYTD);

            dic.Add("lbUD1", lbUD1);
            dic.Add("txtUD1Input", txtUD1Input);
            dic.Add("label23", label23);
            dic.Add("txtUD1Val", txtUD1Val);
            dic.Add("lbUD1ValYTD", lbUD1ValYTD);

            dic.Add("lbUD2", lbUD2);
            dic.Add("txtUD2Input", txtUD2Input);
            dic.Add("label24", label24);
            dic.Add("txtUD2Val", txtUD2Val);
            dic.Add("lbUD2ValYTD", lbUD2ValYTD);

            dic.Add("lbUD3", lbUD3);
            dic.Add("txtUD3Input", txtUD3Input);
            dic.Add("label25", label25);
            dic.Add("txtUD3Val", txtUD3Val);
            dic.Add("lbUD3ValYTD", lbUD3ValYTD);

            dic.Add("lbUD4", lbUD4);
            dic.Add("txtUD4Input", txtUD4Input);
            dic.Add("label26", label26);
            dic.Add("txtUD4Val", txtUD4Val);
            dic.Add("lbUD4ValYTD", lbUD4ValYTD);

            dic.Add("lbUD5", lbUD5);
            dic.Add("txtUD5Input", txtUD5Input);
            dic.Add("label27", label27);
            dic.Add("txtUD5Val", txtUD5Val);
            dic.Add("lbUD5ValYTD", lbUD5ValYTD);

            dic.Add("lbUD6", lbUD6);
            dic.Add("txtUD6Input", txtUD6Input);
            dic.Add("label43", label43);
            dic.Add("txtUD6Val", txtUD6Val);
            dic.Add("lbUD6ValYTD", lbUD6ValYTD);

            dic.Add("lbUD7", lbUD7);
            dic.Add("txtUD7Input", txtUD7Input);
            dic.Add("label47", label47);
            dic.Add("txtUD7Val", txtUD7Val);
            dic.Add("lbUD7ValYTD", lbUD7ValYTD);

            return dic;
        }
        public JsonResult PayrollView() {
            Guid Id = new Guid(Request["Id"]);
            T105 t = db.T105.Where(x => x.Id == Id).SingleOrDefault();
            if (t == null) {
                return Json(new { code = 2, status = "Error", message = "The currently selected view is not found!" }, JsonRequestBehavior.AllowGet);
            };

            return Json(new { IncomesData = IncomesData(t), DeductionsData = DeductionsData(t), TaxesSummary = TaxesSummary(t) , code = 1, status = "success" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PayrollPrint() {
            if (Request["Id"] == null || Request["Id"] == "")
                return RedirectToAction("PayRollList");
            Guid Id = new Guid(Request["Id"]);
            T105 t = db.T105.Where(x => x.Id == Id).SingleOrDefault();
            T100 t100 = db.T100.Where(x => x.CompanyId == LoginUser.CompanyId).SingleOrDefault();
            if (t == null)
            {
                return Json(new { code = 2, status = "Error", message = "The currently selected view is not found!" }, JsonRequestBehavior.AllowGet);
            };
            Dictionary<string, string> PrintDate = new Dictionary<string, string>();
            PrintDate.Add("P-Logo", t100.Logo);
            PrintDate.Add("P-Company1", t100.Company1);
            PrintDate.Add("P-Company2", t100.Company2);
            PrintDate.Add("P-Company3", t100.Company3);
            PrintDate.Add("P-Company4", t100.Company4);
            PrintDate.Add("P-BankName", t100.BankName);
            PrintDate.Add("P-BankInfo1", t100.BankInfo1);
            PrintDate.Add("P-BankInfo2", t100.BankInfo2);
            PrintDate.Add("P-BankInfo3", t100.BankInfo3);
            PrintDate.Add("P-CheckNum", t.CheckNo.ToString());
            PrintDate.Add("P-TransitCode", t100.TransitCode);
            PrintDate.Add("P-DateSubmitted", t.DateSubmitted.ToString("d"));
            PrintDate.Add("P-DateStartPeriod", t.DateStartPeriod.ToString("d"));
            PrintDate.Add("P-DateEndPeriod", t.DateEndPeriod.ToString("d"));

            PrintDate.Add("ExField1", t100.ExField1);
            string SSN = t.SSN;
            if (t100.ExField2)
            {
                SSN = "***-**-" + SSN.Substring(7, 4);
            };
            PrintDate.Add("P-SSN", SSN);
            PrintDate.Add("P-Filing", t.F106);
            PrintDate.Add("P-Name", t.FName + " " + t.LName);
            PrintDate.Add("P-CheckAmount", t.S122.ToString("C"));

            string CheckAmountDesc = "";
            if (t.S122 < 0)
            {
                CheckAmountDesc = "( Minus ) " + AmountHelpers.GetAmount((0 - t.S122));
            }
            else {
                CheckAmountDesc = AmountHelpers.GetAmount(t.S122);
            }

            PrintDate.Add("P-CheckAmountDesc", CheckAmountDesc);
            PrintDate.Add("P-EAddress", t.Address1);
            PrintDate.Add("P-ECityState", t.City + ", " + t.State + " " + t.ZipCode);
            PrintDate.Add("P-EPhone", t.Phone);
            PrintDate.Add("P-Memo", t.Memo);
            PrintDate.Add("P-Signature", t100.Signature);
            PrintDate.Add("P-PayCode", "C" + t.CheckNo.ToString() + "CA" + t100.BankRouteNo + "A4" + t100.BankAccountNo + "C");

            Dictionary<string, object> PrV = PayrollValues(t);
            PrintDate.Add("P-GrossPay", PrV["txtTotalIncome"].ToString());
            PrintDate.Add("P-NetPay", PrV["lbNetPay"].ToString());
            PrintDate.Add("P-GrossPayYTD", PrV["txtTotalIncomeYTD"].ToString());
            PrintDate.Add("P-NetPayYTD", PrV["lbNetPayYTD"].ToString());

            ViewBag.PrintDate = PrintDate;
            ViewBag.IncomesData = IncomesData(t);
            ViewBag.DeductionsData = DeductionsData(t);
            ViewBag.TaxesData = Taxes(t);
            //return Json(new { IncomesData = IncomesData(t), DeductionsData = DeductionsData(t), PrintDate = PrintDate, TaxesData = Taxes(t), code = 1, status = "success" }, JsonRequestBehavior.AllowGet);
            return View();
        }
        [AllowAnonymous]
        public ActionResult MPayroll() {
            if (Request["Id"] == null || Request["Id"] == "")
                return Content("Error : Id is null!");
            Guid Id = new Guid(Request["Id"]);
            T105 t = db.T105.Where(x => x.Id == Id).SingleOrDefault();
            List<IncomesDeductionsView> IncomesList = IncomesData(t);
            List<IncomesDeductionsView> DeductionsList = DeductionsData(t);
            Dictionary<string, string> TaxesSummaryDic = TaxesSummary(t);
            ViewBag.IncomesList = IncomesList;
            ViewBag.DeductionsList = DeductionsList;
            ViewBag.TaxesSummaryDic = TaxesSummaryDic;
            return View();
        }
        private List<IncomesDeductionsView> Taxes(T105 t) {
            Dictionary<string, object> PrV = PayrollValues(t);
            List<IncomesDeductionsView> TaxesData = new List<IncomesDeductionsView>();
            TaxesData.Add(new IncomesDeductionsView() { Text = "Federal Tax", V1 = "", V2 = "", V3 = t.S111.ToString("f2"), V4 = PrV["txtIncomeTaxYTD"].ToString() });
            TaxesData.Add(new IncomesDeductionsView() { Text = "Employee Social Security", V1 = "", V2 = "", V3 = t.S109.ToString("f2"), V4 = PrV["txtSStaxYTD"].ToString() });
            TaxesData.Add(new IncomesDeductionsView() { Text = "Employee Medicare", V1 = "", V2 = "", V3 = t.S110.ToString("f2"), V4 = PrV["txtMedicareTaxYTD"].ToString() });
            TaxesData.Add(new IncomesDeductionsView() { Text = "State Tax", V1 = "", V2 = "", V3 = t.S112.ToString("f2"), V4 = PrV["txtStateIncomeTaxYTD"].ToString() });
            TaxesData.Add(new IncomesDeductionsView() { Text = "Local Tax", V1 = "", V2 = "", V3 = t.S113.ToString("f2"), V4 = PrV["txtLocalTaxYTD"].ToString() });
            return TaxesData;
        }
        private List<IncomesDeductionsView> DeductionsData(T105 t) {
            Dictionary<string, object> PrV = PayrollValues(t);
            List<IncomesDeductionsView> DeductionsData = new List<IncomesDeductionsView>();

            if (t.S1231 != "")
            {
                string lb = t.S1231;
                int len = lb.Length;
                int message = Convert.ToInt16(lb.Substring(len - 1, 1));
                string Text = lb.Substring(0, len - 1);
                if (message >= 4)
                {
                    message = message - 4;
                    Text = Text + " (%)";
                }
                else
                {
                    Text = Text + " ($)";
                }
                DeductionsData.Add(new IncomesDeductionsView() { Text = Text, V1 = t.F1231.ToString("f2"), V2 = "--", V3 = t.S1241.ToString("f2"), V4 = PrV["lbUD1ValYTD"].ToString() });

            }
            if (t.S1232 != "")
            {
                string lb = t.S1232;
                int len = lb.Length;
                int message = Convert.ToInt16(lb.Substring(len - 1, 1));
                string Text = lb.Substring(0, len - 1);
                if (message >= 4)
                {
                    message = message - 4;
                    Text = Text + " (%)";
                }
                else
                {
                    Text = Text + " ($)";
                }

                DeductionsData.Add(new IncomesDeductionsView() { Text = Text, V1 = t.F1232.ToString("f2"), V2 = "--", V3 = t.S1242.ToString("f2"), V4 = PrV["lbUD2ValYTD"].ToString() });
            }
            if (t.S1233 != "")
            {
                string lb = t.S1233;
                int len = lb.Length;
                int message = Convert.ToInt16(lb.Substring(len - 1, 1));
                string Text = lb.Substring(0, len - 1);
                if (message >= 4)
                {
                    message = message - 4;
                    Text = Text + " (%)";
                }
                else
                {
                    Text = Text + " ($)";
                }

                DeductionsData.Add(new IncomesDeductionsView() { Text = Text, V1 = t.F1233.ToString("f2"), V2 = "--", V3 = t.S1243.ToString("f2"), V4 = PrV["lbUD3ValYTD"].ToString() });

            }
            if (t.S1234 != "")
            {
                string lb = t.S1234;
                int len = lb.Length;
                int message = Convert.ToInt16(lb.Substring(len - 1, 1));
                string Text = lb.Substring(0, len - 1);
                if (message >= 4)
                {
                    message = message - 4;
                    Text = Text + " (%)";
                }
                else
                {
                    Text = Text + " ($)";
                }
                DeductionsData.Add(new IncomesDeductionsView() { Text = Text, V1 = t.F1234.ToString("f2"), V2 = "--", V3 = t.S1244.ToString("f2"), V4 = PrV["lbUD4ValYTD"].ToString() });
            }
            if (t.S1235 != "")
            {
                string lb = t.S1235;
                int len = lb.Length;
                int message = Convert.ToInt16(lb.Substring(len - 1, 1));
                string Text = lb.Substring(0, len - 1);
                if (message >= 4)
                {
                    message = message - 4;
                    Text = Text + " (%)";
                }
                else
                {
                    Text = Text + " ($)";
                }
                DeductionsData.Add(new IncomesDeductionsView() { Text = Text, V1 = t.F1235.ToString("f2"), V2 = "--", V3 = t.S1245.ToString("f2"), V4 = PrV["lbUD5ValYTD"].ToString() });
            }

            if (t.S1236 != "")
            {
                string lb = t.S1236;
                int len = lb.Length;
                int message = Convert.ToInt16(lb.Substring(len - 1, 1));
                string Text = lb.Substring(0, len - 1);
                if (message >= 4)
                {
                    message = message - 4;
                    Text = Text + " (%)";
                }
                else
                {
                    Text = Text + " ($)";
                }
                DeductionsData.Add(new IncomesDeductionsView() { Text = Text, V1 = t.F1236.ToString("f2"), V2 = "--", V3 = t.S1246.ToString("f2"), V4 = PrV["lbUD6ValYTD"].ToString() });
            }
            if (t.S1237 != "")
            {
                string lb = t.S1237;
                int len = lb.Length;
                int message = Convert.ToInt16(lb.Substring(len - 1, 1));
                string Text = lb.Substring(0, len - 1);
                if (message >= 4)
                {
                    message = message - 4;
                    Text = Text + " (%)";
                }
                else
                {
                    Text = Text + " ($)";
                }
                DeductionsData.Add(new IncomesDeductionsView() { Text = Text, V1 = t.F1237.ToString("f2"), V2 = "--", V3 = t.S1247.ToString("f2"), V4 = PrV["lbUD7ValYTD"].ToString() });
            }
            return DeductionsData;
        }
        private List<IncomesDeductionsView> IncomesData(T105 t) {
            Dictionary<string, object> PrV = PayrollValues(t);
            List<IncomesDeductionsView> IncomesData = new List<IncomesDeductionsView>();
            if (Convert.ToBoolean(t.F125))
            {
                IncomesData.Add(new IncomesDeductionsView() { Text = "Yealy Salary", V1 = t.F102.ToString("f2"), V2 = "--", V3 = t.S104.ToString("f2"),V4 = PrV["txtPaySalaryYTD"].ToString() });
            }

            if (t.HourlyPay1 != "")
            {
                IncomesData.Add(new IncomesDeductionsView() { Text = t.HourlyPay1, V1 = t.F100.ToString("f2"), V2 = t.S100.ToString("f2"), V3 = t.S105.ToString("f2"), V4 = PrV["txtPayRegularYTD"].ToString() });
            }

            if (t.HourlyPay2 != "")
            {
                IncomesData.Add(new IncomesDeductionsView() { Text = t.HourlyPay2, V1 = t.F101.ToString("f2"), V2 = t.S101.ToString("f2"), V3 = t.S106.ToString("f2"), V4 = PrV["txtPayOTYTD"].ToString() });
            }

            if (t.HourlyPay3 != "")
            {
                IncomesData.Add(new IncomesDeductionsView() { Text = t.HourlyPay3, V1 = t.F1002 .ToString("f2"), V2 = t.S1002.ToString("f2"), V3 = t.S1052.ToString("f2"), V4 = PrV["txtPayDTYTD"].ToString() });
            }

            if (t.HourlyPay4 != "")
            {
                IncomesData.Add(new IncomesDeductionsView() { Text = t.HourlyPay4, V1 = t.SickRate.ToString("f2"), V2 = t.S103.ToString("f2"), V3 = t.S108.ToString("f2"), V4 = PrV["txtPaySickYTD"].ToString() });
            }

            if (t.HourlyPay5 != "")
            {
                IncomesData.Add(new IncomesDeductionsView() { Text = t.HourlyPay5, V1 = t.VacationRate.ToString("f2"), V2 = t.S102.ToString("f2"), V3 = t.S107.ToString("f2"), V4 = PrV["txtPayVacationYTD"].ToString() });
            }

            if (t.S1251 != "")
            {
                IncomesData.Add(new IncomesDeductionsView() { Text = t.S1251, V1 = "--", V2 = "--", V3 = t.S1261.ToString("f2"), V4 = PrV["txtUDPay1ValYTD"].ToString() });
            }

            if (t.S1252 != "")
            {
                IncomesData.Add(new IncomesDeductionsView() { Text = t.S1252, V1 = "--", V2 = "--", V3 = t.S1262.ToString("f2"), V4 = PrV["txtUDPay2ValYTD"].ToString() });
            }

            if (t.S1253 != "")
            {
                IncomesData.Add(new IncomesDeductionsView() { Text = t.S1253, V1 = "--", V2 = "--", V3 = t.S1263.ToString("f2"), V4 = PrV["txtUDPay3ValYTD"].ToString() });
            }

            if (t.S1254 != "")
            {
                IncomesData.Add(new IncomesDeductionsView() { Text = t.S1254, V1 = "--", V2 = "--", V3 = t.S1264.ToString("f2"), V4 = PrV["txtUDPay4ValYTD"].ToString() });
            }

            if (t.S1255 != "")
            {
                IncomesData.Add(new IncomesDeductionsView() { Text = t.S1255, V1 = "--", V2 = "--", V3 = t.S1265.ToString("f2"), V4 = PrV["txtUDPay5ValYTD"].ToString() });
            }

            if (Convert.ToBoolean(t.DoesReceiveAdvanceEIC))
            {
                IncomesData.Add(new IncomesDeductionsView() { Text = "Advance EIC", V1 = "--", V2 = "--", V3 = t.AdvanceEIC.ToString("f2"), V4 = PrV["txtAdvanceEICYTD"].ToString() });
            }
            return IncomesData;
        }
        private Dictionary<string, string> TaxesSummary(T105 t) {
            Dictionary<string, string> TaxesSummary = new Dictionary<string, string>();
            TaxesSummary.Add("dtpStartDate", t.DateStartPeriod.ToString("yyyy-MM-dd"));
            TaxesSummary.Add("dtpEndDate", t.DateEndPeriod.ToString("yyyy-MM-dd"));
            TaxesSummary.Add("dtpPayrollDate", t.DateSubmitted.ToString("yyyy-MM-dd"));
            TaxesSummary.Add("txtPayperiodDesc", t.S127);
            TaxesSummary.Add("txtSSN", t.SSN);
            TaxesSummary.Add("EmployeeName", t.FName + " " + t.LName);

            TaxesSummary.Add("txtIncomeTax", t.S111.ToString("f2"));
            TaxesSummary.Add("txtSStax", t.S109.ToString("f2"));
            TaxesSummary.Add("txtMedicareTax", t.S110.ToString("f2"));
            TaxesSummary.Add("txtStateIncomeTax", t.S112.ToString("f2"));
            TaxesSummary.Add("txtLocalTax", t.S113.ToString("f2"));

            TaxesSummary.Add("txtEmployerSocialSecurity", t.EmployerSocialSecurity.ToString("f2"));
            TaxesSummary.Add("txtEmployerMedicare", t.EmployerMedicare.ToString("f2"));
            TaxesSummary.Add("txtEmployerFedUnemployment", t.S114.ToString("f2"));
            TaxesSummary.Add("txtEmployerStateUnemployment", t.S115.ToString("f2"));

            TaxesSummary.Add("txtTotalIncome", t.S118.ToString("f2"));
            TaxesSummary.Add("lbTotalTaxableIncome", t.S119.ToString("f2"));
            TaxesSummary.Add("lbFICATaxableIncome", t.S120.ToString("f2"));
            TaxesSummary.Add("txtTotalTax", (t.S111 + t.S109 + t.S110 + t.S112 + t.S1241).ToString("f2"));
            TaxesSummary.Add("lbTotalEmployerTax", (t.EmployerSocialSecurity + t.EmployerMedicare + t.S114 + t.S115).ToString("f2"));
            TaxesSummary.Add("txtTotalDeduction", (t.S1241 + t.S1242 + t.S1243 + t.S1244 + t.S1245 + t.S1246 + t.S1247).ToString("f2"));
            TaxesSummary.Add("lbNetPay", t.S122.ToString("f2"));
            TaxesSummary.Add("txtMemo", t.Memo);
            TaxesSummary.Add("QRCodePath", ConfigurationManager.AppSettings["QRCodePath"] + t.Id.ToString() + ".png");
            return TaxesSummary;
        }
        private Dictionary<string, object> PayrollValues(T105 t) {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            T108 t108 = new T108
            {
                Address1 = t.Address1,
                Address2 = t.Address2,
                AdvanceEIC = t.AdvanceEIC,
                CheckNo = t.CheckNo,
                City = t.City,
                DateEndPeriod = t.DateEndPeriod,
                DateLastEdit = t.DateLastEdit,
                DateStartPeriod = t.DateStartPeriod,
                DateSubmitted = t.DateSubmitted,
                DoesReceiveAdvanceEIC = t.DoesReceiveAdvanceEIC,
                EID = t.EmployeeId,
                EmployerMedicare = t.EmployerMedicare,
                EmployerSocialSecurity = t.EmployerSocialSecurity,
                F100 = t.F100,
                F1002 = t.F1002,
                F101 = t.F101,
                F102 = t.F102,
                F103 = t.F103,
                F104 = t.F104,
                F105 = t.F105,
                S100 = t.S100,
                S1002 =t.S1002,
                S101 = t.S101,
                S102 = t.S102,
                S103 = t.S103,
                S104 = t.S104,
                S105 = t.S105,
                S1052 = t.S1052,
                S106 = t.S106,
                S107 = t.S107,
                S108 = t.S108,
                S109 = t.S109,
                S110 = t.S110,
                S111 = t.S111,
                S112 = t.S112,
                S113 = t.S113,
                S114 = t.S114,
                S115 = t.S115,
                S116 = t.S116,
                S117 = t.S117,
                S118 = t.S118,
                S119 = t.S119,
                S120 = t.S120,
                S121 = t.S121,
                S122 = t.S122,
                F106 = t.F106,
                F107 = t.F107,
                F108 = t.F108,
                F109 = t.F109,
                F110 = t.F110,
                F111 = t.F111,
                F112 = t.F112,
                F113 = t.F113,
                F114 = t.F114,
                F115 = t.F115,
                F116 = t.F116,
                F117 = t.F117,
                F118 = t.F118,
                F119 = t.F119,
                F120 = t.F120,
                F121 = t.F121,
                F122 = t.F122,
                S1231 = t.S1231,
                S1241 = t.S1241,
                S1232 = t.S1232,
                S1242 = t.S1242,
                S1233 = t.S1233,
                S1243 = t.S1243,
                S1234 = t.S1234,
                S1244 = t.S1244,
                S1235 = t.S1235,
                S1245 = t.S1245,
                S1251 = t.S1251,
                S1261 = t.S1261,
                S1252 = t.S1252,
                S1262 = t.S1262,
                S1253 = t.S1253,
                S1263 = t.S1263,
                S1254 = t.S1254,
                S1264 = t.S1264,
                S1255 = t.S1255,
                S1265 = t.S1265,
                S127 = t.S127,
                F1231 = t.F1231,
                F1232 = t.F1232,
                F1233 = t.F1233,
                F1234 = t.F1234,
                F1235 = t.F1235,
                F124 = t.F124,
                F125 = t.F125,
                IsW2StatutoryEmployee = t.IsW2StatutoryEmployee,
                IsW2RetirementPlan = t.IsW2RetirementPlan,
                S1236 = t.S1236,
                S1246 = t.S1246,
                S1237 = t.S1237,
                S1247 = t.S1247,
                F1236 = t.F1236,
                F1237 = t.F1237,
                HourlyPay1 = t.HourlyPay1,
                HourlyPay2 = t.HourlyPay2,
                HourlyPay3 = t.HourlyPay3,
                HourlyPay4 = t.HourlyPay4,
                HourlyPay5 = t.HourlyPay5,
                SickRate = t.SickRate,
                VacationRate = t.VacationRate,
                is1099Employee = t.is1099Employee,
                PTOAcchours = t.PTOAcchours,
                VacAccHours = t.VacAccHours,
                printPTOStub = t.printPTOStub,
                PTOAccRate = t.PTOAccRate,
                VacAccRate = t.VacAccRate,
                PTOCapHours = t.PTOCapHours,
                VacCapHours = t.VacCapHours,
                F99 = t.F99,
                FName = t.FName,
                LName = t.LName,
                Memo = t.Memo,
                MInit = t.MInit,
                Phone = t.Phone,
                SSN = t.SSN,
                State = t.State,
                ZipCode = t.ZipCode
            };
            Dictionary<string, object> dicValue = DisplayControlInfo(t108);
            return dicValue;
        }
        private Dictionary<string, object> DisplayControlInfo(T108 t) {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("dtpStartDate", t.DateStartPeriod.ToString("yyyy-MM-dd"));
            dic.Add("dtpEndDate", t.DateEndPeriod.ToString("yyyy-MM-dd"));
            dic.Add("dtpPayrollDate", t.DateSubmitted.ToString("yyyy-MM-dd"));
            
            dic.Add("txtSSN",t.SSN);
            //dic.Add("txtPayperiodDesc", ((Period)(int.Parse(t.S127))).ToString());
            dic.Add("txtPayperiodDesc", t.S127);
            dic.Add("txtSalaryAnnual", t.F102);
            dic.Add("txtPaySalary", t.S104.ToString("f2"));

            dic.Add("txtPayHourly", t.F100);
            dic.Add("txtHrsRegular", t.S100);
            dic.Add("txtPayRegular", t.S105.ToString("f2"));

            dic.Add("txtDoublePayHourly", t.F1002);
            dic.Add("txtHrsDT", t.S1002);
            dic.Add("txtPayDT", t.S1052.ToString("f2"));

            dic.Add("txtOTMultiplier", t.F101);
            dic.Add("txtPayHourlySick", t.SickRate);
            dic.Add("txtPayHourlyVacation", t.VacationRate);

            dic.Add("txtHrsOT", t.S101);
            dic.Add("txtPayOT", t.S106.ToString("f2"));

            dic.Add("txtHrsVacation", t.S102);
            dic.Add("txtPayVacation", t.S107.ToString("f2"));

            dic.Add("txtHrsSick", t.S103);
            dic.Add("txtPaySick", t.S108.ToString("f2"));
            dic.Add("lbUDPay1Desc", t.S1251);
            dic.Add("lbUDPay2Desc", t.S1252);
            dic.Add("lbUDPay3Desc", t.S1253);
            dic.Add("lbUDPay4Desc", t.S1254);
            dic.Add("lbUDPay5Desc", t.S1255);

            dic.Add("txtUDPay1Val", t.S1261);
            dic.Add("txtUDPay2Val", t.S1262);
            dic.Add("txtUDPay3Val", t.S1263);
            dic.Add("txtUDPay4Val", t.S1264);
            dic.Add("txtUDPay5Val", t.S1265);

            dic.Add("txtIncomeTax", t.S111.ToString("f2"));
            dic.Add("txtSStax", t.S109.ToString("f2"));
            dic.Add("txtMedicareTax", t.S110.ToString("f2"));
            dic.Add("txtStateIncomeTax", t.S112.ToString("f2"));
            dic.Add("txtLocalTax", t.S113.ToString("f2"));

            dic.Add("txtEmployerSocialSecurity", t.EmployerSocialSecurity.ToString("f2"));
            dic.Add("txtEmployerMedicare", t.EmployerMedicare.ToString("f2"));
            dic.Add("txtEmployerFedUnemployment", t.S114.ToString("f2"));
            dic.Add("txtEmployerStateUnemployment", t.S115.ToString("f2"));

            dic.Add("txtUD1Input", t.F1231);
            dic.Add("txtUD2Input", t.F1232.ToString("f2"));
            dic.Add("txtUD3Input", t.F1233.ToString("f2"));
            dic.Add("txtUD4Input", t.F1234);
            dic.Add("txtUD5Input", t.F1235);
            dic.Add("txtUD6Input", t.F1236);
            dic.Add("txtUD7Input", t.F1237);

            dic.Add("txtUD1Val", t.S1241.ToString("f2"));
            dic.Add("txtUD2Val", t.S1242.ToString("f2"));
            dic.Add("txtUD3Val", t.S1243.ToString("f2"));
            dic.Add("txtUD4Val", t.S1244.ToString("f2"));
            dic.Add("txtUD5Val", t.S1245.ToString("f2"));
            dic.Add("txtUD6Val", t.S1246.ToString("f2"));
            dic.Add("txtUD7Val", t.S1247.ToString("f2"));

            dic.Add("txtAdvanceEIC", t.AdvanceEIC);

            //设置视图控件显示
            int yloc = 8;
            if (Convert.ToBoolean(t.F125))
            {
                lbSalary = true;
                txtSalaryAnnual = true;
                label15 = true;
                txtPaySalary = true;
                txtPaySalaryYTD = true;

            }

            if (t.HourlyPay1 != "")
            {
                lbRegularHourly = true;
                txtPayHourly = true;
                txtHrsRegular = true;
                txtPayRegular = true;
                txtPayRegularYTD = true;
                dic.Add("lbRegularHourly", t.HourlyPay1);
            }

            if (t.HourlyPay2 != "")
            {

                lbOvertimeHourly = true;
                txtOTMultiplier = true;
                txtHrsOT = true;
                txtPayOT = true;
                txtPayOTYTD = true;
                dic.Add("lbOvertimeHourly", t.HourlyPay2);
            }

            if (t.HourlyPay3 != "")
            {

                lbDoubleHourly = true;
                txtDoublePayHourly = true;
                txtHrsDT = true;
                txtPayDT = true;
                txtPayDTYTD = true;
                dic.Add("lbDoubleHourly", t.HourlyPay3);
            }

            if (t.HourlyPay4 != "")
            {
                lbSickHourly = true;
                txtPayHourlySick = true;
                txtHrsSick = true;
                txtPaySick = true;
                txtPaySickYTD = true;
                dic.Add("lbSickHourly", t.HourlyPay4);

            }

            if (t.HourlyPay5 != "")
            {

                lbVacationHourly = true;
                txtPayHourlyVacation = true;
                txtHrsVacation = true;
                txtPayVacation = true;
                txtPayVacationYTD = true;
                dic.Add("lbVacationHourly", t.HourlyPay5);

            }

            if (Convert.ToBoolean(t.DoesReceiveAdvanceEIC))
            {
                yloc += 16;
                lbAdvanceEIC = true;
                label36 = true;
                label39 = true;
                txtAdvanceEIC = true;
                txtAdvanceEICYTD = true;

            }
            if (t.S1251 != "")
            {
                yloc += 16;
                lbUDPay1Desc = true;
                label31 = true;
                label16 = true;
                txtUDPay1Val = true;
                txtUDPay1ValYTD = true;
            }

            if (t.S1252 != "")
            {
                yloc += 16;
                lbUDPay2Desc = true;
                label32 = true;
                label17 = true;
                txtUDPay2Val = true;
                txtUDPay2ValYTD = true;
            }

            if (t.S1253 != "")
            {
                yloc += 16;
                lbUDPay3Desc = true;
                label33 = true;
                label18 = true;
                txtUDPay3Val = true;
                txtUDPay3ValYTD = true;
            }

            if (t.S1254 != "")
            {
                yloc += 16;
                lbUDPay4Desc = true;
                label34 = true;
                label19 = true;
                txtUDPay4Val = true;
                txtUDPay4ValYTD = true;
            }

            if (t.S1255 != "")
            {
                yloc += 16;
                lbUDPay5Desc = true;
                label35 = true;
                label20 = true;
                txtUDPay5Val = true;
                txtUDPay5ValYTD = true;
            }

            if (t.S1231 != "")
            {
                yloc += 16;
                lbUD1 = true;
                txtUD1Input = true;
                label23 = true;
                txtUD1Val = true;
                lbUD1ValYTD = true;

                string lb = t.S1231;
                int len = lb.Length;
                int message = Convert.ToInt16(lb.Substring(len - 1, 1));
                //this.lbUD1.Text = lb.Substring(0, len-1);
                dic.Add("lbUD1", lb.Substring(0, len - 1));
                if (message >= 4)
                {
                    message = message - 4;
                    dic["lbUD1"] = dic["lbUD1"].ToString() + " (%)";
                }
                else
                {
                    dic["lbUD1"] = dic["lbUD1"].ToString() + " ($)";
                }

            }
            if (t.S1232 != "")
            {
                yloc += 16;
                lbUD2 = true;
                txtUD2Input = true;
                label24 = true;
                txtUD2Val = true;
                lbUD2ValYTD = true;

                string lb = t.S1232;
                int len = lb.Length;
                int message = Convert.ToInt16(lb.Substring(len - 1, 1));
                dic.Add("lbUD2", lb.Substring(0, len - 1));
                if (message >= 4)
                {
                    message = message - 4;
                    dic["lbUD2"] = dic["lbUD2"].ToString() + " (%)";
                }
                else
                {
                    dic["lbUD2"] = dic["lbUD2"].ToString() + " ($)";
                }

            }
            if (t.S1233 != "")
            {
                yloc += 16;
                lbUD3 = true;
                txtUD3Input = true;
                label25 = true;
                txtUD3Val = true;
                lbUD3ValYTD = true;

                string lb = t.S1233;
                int len = lb.Length;
                int message = Convert.ToInt16(lb.Substring(len - 1, 1));
                dic.Add("lbUD3", lb.Substring(0, len - 1));
                if (message >= 4)
                {
                    message = message - 4;
                    dic["lbUD3"] = dic["lbUD3"].ToString() + " (%)";
                }
                else
                {
                    dic["lbUD3"] = dic["lbUD3"].ToString() + " ($)";
                }

            }
            if (t.S1234 != "")
            {
                yloc += 16;
                lbUD4 = true;
                txtUD4Input = true;
                label26 = true;
                txtUD4Val = true;
                lbUD4ValYTD = true;

                string lb = t.S1234;
                int len = lb.Length;
                int message = Convert.ToInt16(lb.Substring(len - 1, 1));
                dic.Add("lbUD4", lb.Substring(0, len - 1));
                if (message >= 4)
                {
                    message = message - 4;
                    dic["lbUD4"] = dic["lbUD4"].ToString() + " (%)";
                }
                else
                {
                    dic["lbUD4"] = dic["lbUD4"].ToString() + " ($)";
                }
            }
            if (t.S1235 != "")
            {
                yloc += 16;
                lbUD5 = true;
                txtUD5Input = true;
                label27 = true;
                txtUD5Val = true;
                lbUD5ValYTD = true;

                string lb = t.S1235;
                int len = lb.Length;
                int message = Convert.ToInt16(lb.Substring(len - 1, 1));
                dic.Add("lbUD5", lb.Substring(0, len - 1));
                if (message >= 4)
                {
                    message = message - 4;
                    dic["lbUD5"] = dic["lbUD5"].ToString() + " (%)";
                }
                else
                {
                    dic["lbUD5"] = dic["lbUD5"].ToString() + " ($)";
                }
            }

            if (t.S1236 != "")
            {
                yloc += 16;
                lbUD6 = true;
                txtUD6Input = true;
                label43 = true;
                txtUD6Val = true;
                lbUD6ValYTD = true;

                string lb = t.S1236;
                int len = lb.Length;
                int message = Convert.ToInt16(lb.Substring(len - 1, 1));
                //this.lbUD1.Text = lb.Substring(0, len-1);
                dic.Add("lbUD6", lb.Substring(0, len - 1));
                if (message >= 4)
                {
                    message = message - 4;
                    dic["lbUD6"] = dic["lbUD6"].ToString() + " (%)";
                }
                else
                {
                    dic["lbUD6"] = dic["lbUD6"].ToString() + " ($)";
                }
            }
            if (t.S1237 != "")
            {
                yloc += 16;
                lbUD7 = true;
                txtUD7Input = true;
                label47 = true;
                txtUD7Val = true;
                lbUD7ValYTD = true;

                string lb = t.S1237;
                int len = lb.Length;
                int message = Convert.ToInt16(lb.Substring(len - 1, 1));
                //this.lbUD1.Text = lb.Substring(0, len-1);
                dic.Add("lbUD7", lb.Substring(0, len - 1));
                if (message >= 4)
                {
                    message = message - 4;
                    dic["lbUD7"] = dic["lbUD7"].ToString() + " (%)";
                }
                else
                {
                    dic["lbUD7"] = dic["lbUD7"].ToString() + " ($)";
                }
            }


            //LoadText()
            float tmp1;
            double num1;
            if (Convert.ToBoolean(t.F125))
            {
                num1 = Function.GetSafeSingle(summary15) + Function.GetSafeSingle(t.S104);
                dic.Add("txtPaySalaryYTD", num1.ToString("f2"));

            }
            if (t.HourlyPay1 != "")
            {
                num1 = Function.GetSafeSingle(summary16) + Function.GetSafeSingle(t.S105);
                dic.Add("txtPayRegularYTD", num1.ToString("f2"));
            }
            if (t.HourlyPay2 != "")
            {
                num1 = Function.GetSafeSingle(summary17) + Function.GetSafeSingle(t.S106);
                dic.Add("txtPayOTYTD", num1.ToString("f2"));

            }
            if (t.HourlyPay3 != "")
            {
                num1 = Function.GetSafeSingle(summary16X2) + Function.GetSafeSingle(t.S1052);
                dic.Add("txtPayDTYTD", num1.ToString("f2"));
            }

            if (t.HourlyPay4 != "")
            {
                num1 = Function.GetSafeSingle(summary19) + Function.GetSafeSingle(t.S108);
                dic.Add("txtPaySickYTD", num1.ToString("f2"));
            }

            if (t.HourlyPay5 != "")
            {
                num1 = Function.GetSafeSingle(summary18) + Function.GetSafeSingle(t.S107);
                dic.Add("txtPayVacationYTD", num1.ToString("f2"));
            }

            if (Convert.ToBoolean(t.DoesReceiveAdvanceEIC))
            {
                num1 = Function.GetSafeSingle(summary25) + Function.GetSafeSingle(t.AdvanceEIC);
                dic.Add("txtAdvanceEICYTD", num1.ToString("f2"));
            }
            if (t.S1251 != "")
            {
                num1 = Function.GetSafeSingle(summary20) + Function.GetSafeSingle(t.S1261);
                dic.Add("txtUDPay1ValYTD", num1.ToString("f2"));
            }
            if (t.S1252 != "")
            {
                num1 = Function.GetSafeSingle(summary21) + Function.GetSafeSingle(t.S1262);
                dic.Add("txtUDPay2ValYTD", num1.ToString("f2"));
            }
            if (t.S1253 != "")
            {
                num1 = Function.GetSafeSingle(summary22) + Function.GetSafeSingle(t.S1263);
                dic.Add("txtUDPay3ValYTD", num1.ToString("f2"));
            }
            if (t.S1254 != "")
            {
                num1 = Function.GetSafeSingle(summary23) + Function.GetSafeSingle(t.S1264);
                dic.Add("txtUDPay4ValYTD", num1.ToString("f2"));
            }
            if (t.S1255 != "")
            {
                num1 = Function.GetSafeSingle(summary24) + Function.GetSafeSingle(t.S1265);
                dic.Add("txtUDPay5ValYTD", num1.ToString("f2"));
            }

            num1 = Function.GetSafeSingle(t.S118);
            dic.Add("txtTotalIncome", num1.ToString("f2"));
            num1 = Function.GetSafeSingle(t.S120);
            dic.Add("lbFICATaxableIncome", num1.ToString("f2"));
            num1 = Function.GetSafeSingle(t.S119);
            dic.Add("lbTotalTaxableIncome", num1.ToString("f2"));
            num1 = Function.GetSafeSingle(t.S122);
            dic.Add("lbNetPay", num1.ToString("f2"));

            num1 = 0;

            if (dic["txtUD7Val"].ToString() != "")
            {
                tmp1 = Function.GetSafeSingle(t.S1247);
                num1 += tmp1;
                tmp1 = summary30 + tmp1;
                dic.Add("lbUD7ValYTD", tmp1.ToString("f2"));

            }
            if (dic["txtUD6Val"].ToString() != "")
            {
                tmp1 = Function.GetSafeSingle(t.S1246);
                num1 += tmp1;
                tmp1 = summary29 + tmp1;
                dic.Add("lbUD6ValYTD", tmp1.ToString("f2"));

            }
            if (dic["txtUD5Val"].ToString() != "")
            {
                tmp1 = Function.GetSafeSingle(t.S1245);
                num1 += tmp1;
                tmp1 = summary5 + tmp1;
                dic.Add("lbUD5ValYTD", tmp1.ToString("f2"));

            }
            if (dic["txtUD4Val"].ToString() != "")
            {
                tmp1 = Function.GetSafeSingle(t.S1244);
                num1 += tmp1;
                tmp1 = summary4 + tmp1;
                dic.Add("lbUD4ValYTD", tmp1.ToString("f2"));
            }
            if (dic["txtUD3Val"].ToString() != "")
            {
                tmp1 = Function.GetSafeSingle(t.S1243);
                num1 += tmp1;
                tmp1 = summary3 + tmp1;
                dic.Add("lbUD3ValYTD", tmp1.ToString("f2"));
            }
            if (dic["txtUD2Val"].ToString() != "")
            {
                tmp1 = Function.GetSafeSingle(t.S1242);
                num1 += tmp1;
                tmp1 = summary2 + tmp1;
                dic.Add("lbUD2ValYTD", tmp1.ToString("f2"));
            }
            if (dic["txtUD1Val"].ToString() != "")
            {
                tmp1 = Function.GetSafeSingle(t.S1241);
                num1 += tmp1;
                tmp1 = summary1 + tmp1;
                dic.Add("lbUD1ValYTD", tmp1.ToString("f2"));

            }

            dic.Add("txtTotalDeduction", num1.ToString("f2"));
            num1 = Function.GetSafeSingle(t.S111) + Function.GetSafeSingle(t.S109) + Function.GetSafeSingle(t.S110) + Function.GetSafeSingle(t.S112) + Function.GetSafeSingle(dic["txtUD1Val"]);
            dic.Add("txtTotalTax", num1.ToString("f2"));
            num1 = Function.GetSafeSingle(t.EmployerSocialSecurity) + Function.GetSafeSingle(t.EmployerMedicare) + Function.GetSafeSingle(t.S114) + Function.GetSafeSingle(t.S115);
            dic.Add("lbTotalEmployerTax", num1.ToString("f2"));

            num1 = Function.GetSafeSingle(summary6) + Function.GetSafeSingle(t.S111);
            dic.Add("txtIncomeTaxYTD", num1.ToString("f2"));

            num1 = Function.GetSafeSingle(summary7) + Function.GetSafeSingle(t.S109);
            dic.Add("txtSStaxYTD", num1.ToString("f2"));

            num1 = Function.GetSafeSingle(summary8) + Function.GetSafeSingle(t.S110);
            dic.Add("txtMedicareTaxYTD", num1.ToString("f2"));

            num1 = Function.GetSafeSingle(summary9) + Function.GetSafeSingle(t.S112);
            dic.Add("txtStateIncomeTaxYTD", num1.ToString("f2"));

            num1 = Function.GetSafeSingle(summary10) + Function.GetSafeSingle(dic["txtLocalTax"]);
            dic.Add("txtLocalTaxYTD", num1.ToString("f2"));

            num1 = Function.GetSafeSingle(summary11) + Function.GetSafeSingle(t.EmployerSocialSecurity);
            dic.Add("txtEmployerSocialSecurityYTD", num1.ToString("f2"));

            num1 = Function.GetSafeSingle(summary12) + Function.GetSafeSingle(t.EmployerMedicare);
            dic.Add("txtEmployerMedicareYTD", num1.ToString("f2"));

            num1 = Function.GetSafeSingle(summary13) + Function.GetSafeSingle(t.S114);
            dic.Add("txtEmployerFedUnemploymentYTD", num1.ToString("f2"));

            num1 = Function.GetSafeSingle(summary14) + Function.GetSafeSingle(t.S115);
            dic.Add("txtEmployerStateUnemploymentYTD", num1.ToString("f2"));

            num1 = Function.GetSafeSingle(summary27) + Function.GetSafeSingle(t.S118);
            dic.Add("txtTotalIncomeYTD", num1.ToString("f2"));

            num1 = Function.GetSafeSingle(summary26) + Function.GetSafeSingle(t.S122);
            dic.Add("lbNetPayYTD", num1.ToString("f2"));


            num1 = Function.GetSafeSingle(summary31) + Function.GetSafeSingle(t.PTOAcchours);
            dic.Add("txtPTOYTD", Math.Round(num1, 2).ToString());

            dic.Add("txtAccurePTO", Convert.ToString(Function.GetSafeSingle(t.PTOAcchours)));


            num1 = Function.GetSafeSingle(summary32) + Function.GetSafeSingle(t.VacAccHours);
            dic.Add("txtVacYTD", Math.Round(num1, 2).ToString());

            dic.Add("txtAccureVac", Convert.ToString(Function.GetSafeSingle(t.VacAccHours)));

            
            return dic;
        }
        private T108 CalcPayrollAll(T108 t) {
            Company c = db.Company.Find(LoginUser.Company.CompanyId);
            ModelHandler<T108> modelHandler = new ModelHandler<T108>();
            List<T108> list = new List<T108>();
            list.Add(t);
            DataTable dateTable = modelHandler.FillDataTable(list);
            DataRow row1 = dateTable.Rows[0];

            DateTime time1;
            PayrollCalcs calcs1 = new PayrollCalcs(ref row1,c,strDate());
            if (strDate() == "DateEndPeriod")
            {
                time1 = t.DateEndPeriod;
            }
            else if (strDate() == "DateSubmitted")
            {
                time1 = t.DateSubmitted;
            }
            else
            {
                time1 = DateTime.Now;
            }
            int num1 = time1.Year;
            float single9 = calcs1.CalcPaySalary();
            float single8 = calcs1.CalcPayRegular();
            float single7 = calcs1.CalcPayOT();
            float single10 = calcs1.CalcPaySick();
            float single11 = calcs1.CalcPayVacation();
            float single8X2 = calcs1.CalcPayDouble();
            t.S104 = single9;
            t.S105 = single8;
            t.S1052 = single8X2;
            t.S106 = single7;
            t.S108 = single10;
            t.S107 = single11;
            float single25 = Convert.ToSingle(t.S1261);
            float single26 = Convert.ToSingle(t.S1262);
            float single27 = Convert.ToSingle(t.S1263);
            float single28 = Convert.ToSingle(t.S1264);
            float single29 = Convert.ToSingle(t.S1265);
            float single30 = 0f;
            single30 = (((((((((single9 + single8) + single7) + single11) + single8X2) + single10) + single25) + single26) + single27) + single28) + single29;

            t.S118 = single30;
            float single16 = Convert.ToSingle((double)calcs1.CalcDeduction(1, single30, time1));
            float single18 = Convert.ToSingle((double)calcs1.CalcDeduction(2, single30, time1));
            float single20 = Convert.ToSingle((double)calcs1.CalcDeduction(3, single30, time1));
            float single22 = Convert.ToSingle((double)calcs1.CalcDeduction(4, single30, time1));
            float single24 = Convert.ToSingle((double)calcs1.CalcDeduction(5, single30, time1));
            float single225 = Convert.ToSingle((double)calcs1.CalcDeduction(6, single30, time1));
            float single226 = Convert.ToSingle((double)calcs1.CalcDeduction(7, single30, time1));

            t.S1241 = single16;
            t.S1242 = single18;
            t.S1243 = single20;
            t.S1244 = single22;
            t.S1245 = single24;
            t.S1246 = single225;
            t.S1247 = single226;
            float futarate = 0;
            futarate = Convert.ToSingle(c.FUTA / 100);

            float single1 = calcs1.CalcFedUnempTax(single30, num1, futarate, 7000f);
            float single14 = calcs1.CalcStateUnempTax(single30, time1);
            float single4 = calcs1.CalcGrossTaxable(single30, single16, single18, single20, single22, single24, single225, single226);
            float single2 = calcs1.CalcGrossFICATaxable(single30, single16, single18, single20, single22, single24, single225, single226, time1);
            float single3 = calcs1.CalcGrossSSTaxable(single30, single16, single18, single20, single22, single24, single225, single226, time1);

            float single12 = calcs1.CalcSSTax(single2, num1);
            float single6 = calcs1.CalcMedicareTax(single2, num1);
            float fYTDTaxable = calcs1.GrossFICATaxableIncomeYTD(num1); //Use FICA as gross taxable imcome YTD??? Could be GrossTaxable Income YTD for extra medicare greater then 200,000 compare
            float fMediTaxExtra = calcs1.CalcMedicareTaxExtra(single2, num1, fYTDTaxable);
            float single40 = calcs1.CalcAdvanceEIC();
            float single41 = calcs1.CalcLocalIncomeTax();
            //float single5 = calcs1.CalcFedIncometax(single30, num1);
            float single5 = calcs1.CalcFedIncometax(single4, num1);
            //float single13 = calcs1.CalcStateIncomeTax(single30, single5, single12, single6, num1);
            float single13 = calcs1.CalcStateIncomeTax(single4, single5, single12, single6, num1);
            t.S109 = single12;
            //Hardcode for employer ss tax
            //table2.Rows[intCount]["EmployerSocialSecurity"] = single12;
            t.EmployerSocialSecurity = calcs1.CalcSSTaxEmployer(single2, num1);
            t.S110 = single6 + fMediTaxExtra;
            t.EmployerMedicare = single6;
            t.S114 = single1;
            t.S115 = single14;
            t.S111 = single5;
            t.S112 = single13;
            t.S119 = single4;
            t.S120 = single2;
            t.S121 = single3;

            float single31 = 0f;
            single31 = ((((((((single30 + single40 - single5) - single13) - single12) - single6 - fMediTaxExtra) - single16) - single18) - single20) - single22) - single24 - single225 - single226 - single41;
            t.S122 = single31;
            t.DateLastEdit = DateTime.Now;

            float single42 = calcs1.CalcPTOAcc(num1);
            t.PTOAcchours = single42;

            float single43 = calcs1.CalcVacAcc(num1);
            t.VacAccHours = single43;

            summary1 = calcs1.UDValYTD(1, num1);
            summary2 = calcs1.UDValYTD(2, num1);
            summary3 = calcs1.UDValYTD(3, num1);
            summary4 = calcs1.UDValYTD(4, num1);
            summary5 = calcs1.UDValYTD(5, num1);
            summary29 = calcs1.UDValYTD(6, num1);
            summary30 = calcs1.UDValYTD(7, num1);
            summary6 = calcs1.FederalTaxYTD(num1);
            summary7 = calcs1.SSTaxYTD(num1);
            summary8 = calcs1.MedicareTaxYTD(num1);
            summary9 = calcs1.StateTaxYTD(num1);
            summary10 = calcs1.LocalTaxYTD(num1);
            summary11 = calcs1.EmployerSSTaxYTD(num1);
            summary12 = calcs1.EmployerMedicareYTD(num1);
            summary13 = calcs1.FedUnemptaxYTD(num1);
            summary14 = calcs1.StateUnemptaxYTD(num1);
            summary15 = calcs1.PaySalaryYTD(num1);
            summary16 = calcs1.PayRegularYTD(num1);
            summary16X2 = calcs1.PayDTYTD(num1);
            summary17 = calcs1.PayOTYTD(num1);
            summary18 = calcs1.PayVacationYTD(num1);
            summary19 = calcs1.PaySickYTD(num1);
            summary20 = calcs1.UDPay1ValYTD(num1);
            summary21 = calcs1.UDPay2ValYTD(num1);
            summary22 = calcs1.UDPay3ValYTD(num1);
            summary23 = calcs1.UDPay4ValYTD(num1);
            summary24 = calcs1.UDPay5ValYTD(num1);
            summary25 = calcs1.AdvanceEICYTD(num1);
            summary26 = calcs1.NetIncomeYTD(num1);
            summary27 = calcs1.GrossIncomeYTD(num1);
            summary28 = calcs1.GrossSSIncomeYTD(num1);

            summary31 = calcs1.PTOYTD(num1);
            summary32 = calcs1.VacYTD(num1);

            return t;
        }
        private string strDate() {
            Company c = db.Company.Find(LoginUser.Company.CompanyId);
            string strDate;
            if (c.PayReportByEndingDate)
            {
                strDate = "DateEndPeriod";
            }
            else
            {
                strDate = "DateSubmitted";
            }
            return strDate;
        }
        private T108 LoadUDDescIntoTempWageDataTbl(T108 t) {
            var arrT102 = db.T102.Where(x=>x.CompanyId==LoginUser.CompanyId).OrderBy(x=>x.ItemId).ToList();
            string[] textArray1 = new string[arrT102.Count];
            foreach (var t102 in arrT102) {
                int i = arrT102.IndexOf(t102);

                bool flag2 = t102.Taxable;
                bool flag1 = t102.FICATaxable;
                bool flag3 = t102.PctofIncome;
                if (t102.Enabled)
                {
                    int text2 = 0;
                    if (!flag2)
                    {
                        text2 = text2 + 1;
                    }
                    if (!flag1)
                    {
                        text2 = text2 + 2;
                    }

                    if (flag3)
                    {
                        text2 = text2 + 4;
                    }
                    string strTemp = t102.Description;
                    if (strTemp.Length > 48)
                    {
                        strTemp = strTemp.Substring(0, 48);
                    }
                    textArray1[i] = strTemp;
                    textArray1[i] = textArray1[i] + " " + text2.ToString();
                }
                else
                {
                    textArray1[i] = "";

                }

            }

            var arrT201 = db.T201.Where(x => x.CompanyId == LoginUser.CompanyId).OrderBy(x => x.Ord).ToList();
            string[] textArray2 = new string[arrT201.Count];
            foreach (var t201 in arrT201) {
                int i = arrT201.IndexOf(t201);

                bool flag2 = t201.Enabled;
                if (flag2)
                {
                    textArray2[i] = t201.Description;
                }
                else
                {
                    textArray2[i] = "";
                }
            }

            t.S1231 = textArray1[0];
            t.S1232 = textArray1[1];
            t.S1233 = textArray1[2];
            t.S1234 = textArray1[3];
            t.S1235 = textArray1[4];
            t.S1236 = textArray1[5];
            t.S1237 = textArray1[6];
            t.HourlyPay1 = textArray2[1];
            t.HourlyPay2 = textArray2[2];
            t.HourlyPay3 = textArray2[3];
            t.HourlyPay4 = textArray2[4];
            t.HourlyPay5 = textArray2[5];
            t.S1251 = textArray2[6];
            t.S1252 = textArray2[7];
            t.S1253 = textArray2[8];
            t.S1254 = textArray2[9];
            t.S1255 = textArray2[10];

            return t;
        }
        private T108 GetPayrollInput() {
            T108 t108 = new T108();

            return t108;

        }
        public JsonResult GetDeductions() {
            var t102 = db.T102.Where(t => t.CompanyId == LoginUser.CompanyId).Select(s=>new { ItemId = s.ItemId ,AnnualLimit = s.AnnualLimit }).ToList();
            return Json(t102, JsonRequestBehavior.AllowGet);
        }
        private string GetStartDate(string obj1)
        {
            string text3 = "";
            DateTime currentTime = DateTime.Now;
            if (obj1 == "Daily")
            {
                text3 = currentTime.AddDays(-1).ToString("yyyy-MM-dd");
            }
            else if (obj1 == "Weekly")
            {
                //text3 = Convert.ToString(Convert.ToDateTime(text2, info1).AddDays (-6));
                text3 = currentTime.AddDays(-6).ToString("yyyy-MM-dd");
            }
            else if (obj1 == "Biweekly")
            {
                //text3 =  Convert.ToString(Convert.ToDateTime(text2, info1).AddDays (-13));
                text3 = currentTime.AddDays(-13).ToString("yyyy-MM-dd");
            }
            else if (obj1 == "Semimonthly")
            {
                int num3;
                int num4;
                if (currentTime.Day > 15)
                {
                    num4 = currentTime.Month;
                    num3 = currentTime.Year;
                    text3 = num3.ToString() + "-" + num4.ToString() + "-16";
                }
                if (currentTime.Day <= 15)
                {
                    num4 = currentTime.Month;
                    num3 = currentTime.Year;
                    text3 = num3.ToString() + "-" + num4.ToString() + "-1";
                }
            }
            else if (obj1 == "Monthly")
            {
                //text3 = Convert.ToString(Convert.ToDateTime(text2, info1).Addonths (-1).AddDays(1));
                text3 = currentTime.AddMonths(-1).AddDays(-1).ToString("yyyy-MM-dd");

            }
            return text3;
        }
        public JsonResult PayRollShow() {
            Dictionary<string, bool> dic = new Dictionary<string, bool>();
            var List102 = db.T102.Where<T102>(t => t.CompanyId == LoginUser.Company.CompanyId).OrderBy(t => t.ItemId).Select(x => new { x.ItemId, x.CodeMap, x.Description, x.Enabled }).OrderBy(t => t.ItemId).ToList();
            var List201 = db.T201.Where<T201>(t => t.CompanyId == LoginUser.Company.CompanyId).Select(x=>new { x.Ord,x.CodeMap,x.Description,x.Enabled }) .OrderBy(t => t.Ord).ToList();
            //foreach (var r in List102) {
            //    dic.Add(r.CodeMap, r.Enabled);
            //}
            //foreach (var r in List201)
            //{
            //    dic.Add(r.CodeMap, r.Enabled);
            //}
            return Json(new { t102 = List102, t201 = List201 }, JsonRequestBehavior.AllowGet);
        }
        private static int getWeekNumInMonth(DateTime daytime)
        {
            int dayInMonth = daytime.Day;
            //本月第一天  
            DateTime firstDay = daytime.AddDays(1 - daytime.Day);
            //本月第一天是周几  
            int weekday = (int)firstDay.DayOfWeek == 0 ? 7 : (int)firstDay.DayOfWeek;
            //本月第一周有几天  
            int firstWeekEndDay = 7 - (weekday - 1);
            //当前日期和第一周之差  
            int diffday = dayInMonth - firstWeekEndDay;
            diffday = diffday > 0 ? diffday : 1;
            //当前是第几周,如果整除7就减一天  
            int WeekNumInMonth = ((diffday % 7) == 0
             ? (diffday / 7 - 1)
             : (diffday / 7)) + 1 + (dayInMonth > firstWeekEndDay ? 1 : 0);
            return WeekNumInMonth;
        }
        private AppIdentityDbContext db
        {
            get
            {
                return HttpContext.GetOwinContext().Get<AppIdentityDbContext>();
            }
        }
        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }
        private AppUser LoginUser
        {
            get
            {
                //AppUser user = HttpContext.User;
                AppUser user = UserManager.FindByName(HttpContext.User.Identity.Name);
                return user;
            }
        }
    }
}