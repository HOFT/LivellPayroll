using LivellPayroll;
using LivellPayRoll.App_Helpers;
using LivellPayRoll.Enum;
using LivellPayRoll.Models;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc.Html;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LivellPayRoll.Reports
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["dtpStartDate"]==null || Session["dtpEndDate"]==null)
               Response.Redirect("/Reports/EmployerReport");
            string dtpStartDate = Session["dtpStartDate"].ToString();
            string dtpEndDate = Session["dtpEndDate"].ToString();
            string State = Session["State"].ToString();
            string EmployeeId = Session["EmployeeId"].ToString();
            string Type = Session["Type"].ToString();
            int Year = int.Parse(Session["Year"].ToString());
            Double UserTimeZone = double.Parse(Session["TimeZone"].ToString());
            int CompanyId = int.Parse(Session["CompanyId"].ToString());
            string strDate = PayReportOrEndingDate(CompanyId);

            if (!IsPostBack) {
                if (Type == "EmployerReport")
                {
                    EmployerReport_Data_Binding(CompanyId, State, strDate, DateTime.Parse(dtpStartDate), DateTime.Parse(dtpEndDate));
                }
                else if (Type == "EmployeeSummary")
                {
                    EmployeeSummary_Data_Binding(CompanyId, strDate, DateTime.Parse(dtpStartDate), DateTime.Parse(dtpEndDate));
                }
                else if (Type == "EmployerDetail")
                {
                    EmployeeDetail_Data_Binding(CompanyId, EmployeeId, strDate, DateTime.Parse(dtpStartDate), DateTime.Parse(dtpEndDate));
                }
                else if (Type == "1" || Type == "2") {
                    EmployeePTO_Data_Binding(CompanyId, EmployeeId, strDate, Year, Type,UserTimeZone);
                }

            }

        }
        private void EmployerReport_Data_Binding(int CompanyId, string cboState,string strDate,DateTime fromDate,DateTime endDate) {
            string strNumberofEmployee;
            DataTable ReportSumary;
            DataTable dtSSTips;
            DataTable dtDedu;

            this.ReportViewer1.Reset();
            this.ReportViewer1.LocalReport.Dispose();

            string sLeft = cboState;
            string strEmployeeCount = "";
            if (sLeft == "All")
                strEmployeeCount = "select distinct SSN from T105 where " + strDate + ">='" + fromDate + "' and " + strDate + "<'" + endDate.AddDays(1.0).ToShortDateString() + "' and CompanyId = " + CompanyId;
            else
                strEmployeeCount = "select distinct SSN from T105 where " + strDate + ">='" + fromDate + "' and " + strDate + "<'" + endDate.AddDays(1.0).ToShortDateString() + "' and State='" + sLeft + "' and CompanyId = " + CompanyId;

            DataTable table2 = DBHelper.getAllEntity(strEmployeeCount);

            strNumberofEmployee = table2.Rows.Count.ToString();

            string strSummary = "";
            if (sLeft == "All")
                strSummary = @"select sum(s119) as totalincome, sum(s121) as socialwage, sum(s120) as mediwage, sum(s111) + sum(s109) + sum(s110) + sum(EmployerMedicare) + sum(EmployerSocialSecurity) as totaltax, sum(s111) as fedincometax, sum(advanceEic) as advanceeic, sum(s109) as socialtax, sum(s109) + sum(EmployerSocialSecurity) as totalsocialtax, sum(s110) as medtax, sum(s110) + sum(EmployerMedicare) as totalmedtax, sum(s112) as statetax, sum(s113) as localtax, sum(EmployerSocialSecurity) as employersocialtax, sum(EmployerMedicare) as employermedicare,
									sum(s114) as fedunemploymenttax, sum(s115) as stateunemploymenttax, sum(s1241) as udedu1, sum(s1242) as udedu2, sum(s1243) as udedu3, sum(s1244) as udedu4, sum(s1245) as udedu5, sum(s1246) as udedu6, sum(s1247) as udedu7 from T105 where " + strDate + ">='" + fromDate + "' and " + strDate + "<'" + endDate.AddDays(1.0).ToShortDateString() + "' and CompanyId = " + CompanyId;
            else
                strSummary = @"select sum(s119) as totalincome, sum(s121) as socialwage, sum(s120) as mediwage, sum(s111) + sum(s109) + sum(s110) + sum(EmployerMedicare) + sum(EmployerSocialSecurity) as totaltax, sum(s111) as fedincometax, sum(advanceEic) as advanceeic, sum(s109) as socialtax, sum(s109) + sum(EmployerSocialSecurity) as totalsocialtax, sum(s110) as medtax, sum(s110) + sum(EmployerMedicare) as totalmedtax, sum(s112) as statetax, sum(s113) as localtax, sum(EmployerSocialSecurity) as employersocialtax, sum(EmployerMedicare) as employermedicare,
									sum(s114) as fedunemploymenttax, sum(s115) as stateunemploymenttax, sum(s1241) as udedu1, sum(s1242) as udedu2, sum(s1243) as udedu3, sum(s1244) as udedu4, sum(s1245) as udedu5, sum(s1246) as udedu6, sum(s1247) as udedu7 from T105 where " + strDate + ">='" + fromDate + "' and " + strDate + "<'" + endDate.AddDays(1.0).ToShortDateString() + "' and State='" + sLeft + "' and CompanyId = " + CompanyId;

            ReportSumary = DBHelper.getAllEntity(strSummary);

            string text1 = "SELECT * FROM T102 where CompanyId = '" + CompanyId + "'";
            dtDedu = DBHelper.getAllEntity(text1);

            string strSummarySSTips = "";
            if (sLeft == "All")
                strSummarySSTips = @"select sum(s121) as socialwage, sum(s1263) as tips from T105 where " + strDate + ">='" + fromDate + "' and " + strDate + "<'" + endDate.AddDays(1.0).ToShortDateString() + "' and s121>0 and CompanyId = " + CompanyId;
            else
                strSummarySSTips = @"select sum(s121) as socialwage, sum(s1263) as tips from T105 where " + strDate + ">='" + fromDate + "' and " + strDate + "<'" + endDate.AddDays(1.0).ToShortDateString() + "' and s121>0 and State='" + sLeft + "' and CompanyId=" + CompanyId;

            dtSSTips = DBHelper.getAllEntity(strSummarySSTips);
            ReportParameter title = new ReportParameter("ReportType", "Accrual Basis");
            string currentCompany = getCompanyName(CompanyId);
            //string currentCompany = MainForm.defDatabase;
            //if (currentCompany.Length > 4)
            //{
            //    currentCompany = MainForm.defDatabase.Substring(0, MainForm.defDatabase.Length - 4);
            //}
            string strFromDate = fromDate.ToShortDateString() + " 12:00:00 AM";
            string strToDate = endDate.ToShortDateString() + " 11:59:59 PM";
            string strEID = getFedTaxId(CompanyId);
            //if (Convert.ToBoolean(SettingHelper.RetrieveStringByKey("IsSSN")))
            //{
            //    strEID = SettingHelper.RetrieveStringByKey("SSN");
            //}
            string strStateID = cboState;
            string strWages = Function.GetSafeDecimal(ReportSumary.Rows[0]["totalincome"]).ToString();
            double sstips = 0;
            if (dtSSTips.Rows.Count > 0 && (dtSSTips.Rows[0]["tips"] != System.DBNull.Value))
            {
                sstips = Function.GetSafeDouble(dtSSTips.Rows[0]["tips"]);
            }

            string tmpString = strWages;
            if (ReportSumary.Rows[0]["socialwage"] != System.DBNull.Value)
            {
                double ss = Convert.ToDouble(ReportSumary.Rows[0]["socialwage"]);
                double dss = ss - sstips;
                tmpString = dss.ToString();
            }
            else
                tmpString = "0";

            string strSSWages = tmpString;
            string strSSTips = sstips.ToString();
            string strMedicareWages = Function.GetSafeDecimal(ReportSumary.Rows[0]["mediwage"]).ToString();
            string strAdvancedEIC = Function.GetSafeDecimal(ReportSumary.Rows[0]["advanceeic"]).ToString();
            string strFedTax = Function.GetSafeDecimal(ReportSumary.Rows[0]["fedincometax"]).ToString();
            string strEmployeeSSTax = Function.GetSafeDecimal(ReportSumary.Rows[0]["socialtax"]).ToString();
            string strEmployeeMedTax = Function.GetSafeDecimal(ReportSumary.Rows[0]["medtax"]).ToString();
            string strStateTax = Function.GetSafeDecimal(ReportSumary.Rows[0]["statetax"]).ToString();
            string strLocalTax = Function.GetSafeDecimal(ReportSumary.Rows[0]["localtax"]).ToString();
            string strEmployerSSTax = Function.GetSafeDecimal(ReportSumary.Rows[0]["employersocialtax"]).ToString();
            string strEmployerMedTax = Function.GetSafeDecimal(ReportSumary.Rows[0]["employermedicare"]).ToString();
            string strFedUnempTax = Function.GetSafeDecimal(ReportSumary.Rows[0]["fedunemploymenttax"]).ToString();
            string strStateUnempTax = Function.GetSafeDecimal(ReportSumary.Rows[0]["stateunemploymenttax"]).ToString();
            string strTotalTax = Function.GetSafeDecimal(ReportSumary.Rows[0]["totaltax"]).ToString();
            string strTotalMedicareTax = Function.GetSafeDecimal(ReportSumary.Rows[0]["totalmedtax"]).ToString();
            string strTotalSocialSecurityTax = Function.GetSafeDecimal(ReportSumary.Rows[0]["totalsocialtax"]).ToString();

            DataTable dt = new DataTable();
            dt.Columns.Add("Description");
            dt.Columns.Add("ExField3");
            if (Function.GetSafeBool(dtDedu.Rows[0]["Enabled"])) {
                DataRow row = dt.NewRow();
                row["Description"] = dtDedu.Rows[0]["Description"];
                row["ExField3"] = Function.GetSafeDecimal(ReportSumary.Rows[0]["udedu1"]).ToString("C");
                dt.Rows.Add(row);
            };
            if (Function.GetSafeBool(dtDedu.Rows[1]["Enabled"]))
            {
                DataRow row = dt.NewRow();
                row["Description"] = dtDedu.Rows[1]["Description"];
                row["ExField3"] = Function.GetSafeDecimal(ReportSumary.Rows[0]["udedu2"]).ToString("C");
                dt.Rows.Add(row);
            };
            if (Function.GetSafeBool(dtDedu.Rows[2]["Enabled"]))
            {
                DataRow row = dt.NewRow();
                row["Description"] = dtDedu.Rows[2]["Description"];
                row["ExField3"] = Function.GetSafeDecimal(ReportSumary.Rows[0]["udedu3"]).ToString("C");
                dt.Rows.Add(row);
            };
            if (Function.GetSafeBool(dtDedu.Rows[3]["Enabled"]))
            {
                DataRow row = dt.NewRow();
                row["Description"] = dtDedu.Rows[3]["Description"];
                row["ExField3"] = Function.GetSafeDecimal(ReportSumary.Rows[0]["udedu4"]).ToString("C");
                dt.Rows.Add(row);
            };
            if (Function.GetSafeBool(dtDedu.Rows[4]["Enabled"]))
            {
                DataRow row = dt.NewRow();
                row["Description"] = dtDedu.Rows[4]["Description"];
                row["ExField3"] = Function.GetSafeDecimal(ReportSumary.Rows[0]["udedu5"]).ToString("C");
                dt.Rows.Add(row);
            };
            if (Function.GetSafeBool(dtDedu.Rows[5]["Enabled"]))
            {
                DataRow row = dt.NewRow();
                row["Description"] = dtDedu.Rows[5]["Description"];
                row["ExField3"] = Function.GetSafeDecimal(ReportSumary.Rows[0]["udedu6"]).ToString("C");
                dt.Rows.Add(row);
            };
            if (Function.GetSafeBool(dtDedu.Rows[6]["Enabled"]))
            {
                DataRow row = dt.NewRow();
                row["Description"] = dtDedu.Rows[6]["Description"];
                row["ExField3"] = Function.GetSafeDecimal(ReportSumary.Rows[0]["udedu7"]).ToString("C");
                dt.Rows.Add(row);
            };

            string State = sLeft;
            if (sLeft != "All") {
                Dictionary<string, string> dic = App_Helpers.EnumHelper.GetEnumItemDesc(typeof(States));
                State = dic[sLeft];
            }
            
            ReportParameter txtCompanyName = new ReportParameter("CompanyName", currentCompany);
            ReportParameter dtFromDate = new ReportParameter("FromDate", strFromDate);
            ReportParameter dtToDate = new ReportParameter("ToDate", strToDate);
            ReportParameter dtEID = new ReportParameter("EIN", strEID);
            ReportParameter dtStateID = new ReportParameter("StateID", strStateID);
            ReportParameter dtNumberOfEmployee = new ReportParameter("EmployeeNumber", strNumberofEmployee);
            ReportParameter dtWages = new ReportParameter("Wages", strWages);

            ReportParameter dtSSWages = new ReportParameter("SSWages", strSSWages);
            ReportParameter dtSSTip = new ReportParameter("SSTips", strSSTips);
            ReportParameter dtMedicareWages = new ReportParameter("MedicareWages", strMedicareWages);
            ReportParameter dtAdvancedEIC = new ReportParameter("AdvancedEIC", strAdvancedEIC);
            ReportParameter dtFedTax = new ReportParameter("FedTax", strFedTax);
            ReportParameter dtEmployeeSSTax = new ReportParameter("EmployeeSSTax", strEmployeeSSTax);
            ReportParameter dtEmployeeMedTax = new ReportParameter("EmployeeMedTax", strEmployeeMedTax);
            ReportParameter dtStateTax = new ReportParameter("StateTax", strStateTax);
            ReportParameter dtLocalTax = new ReportParameter("LocalTax", strLocalTax);
            ReportParameter dtEmployerSSTax = new ReportParameter("EmployerSSTax", strEmployerSSTax);
            ReportParameter dtEmployerMedTax = new ReportParameter("EmployerMedTax", strEmployerMedTax);
            ReportParameter dtFedUnempTax = new ReportParameter("FedUnempTax", strFedUnempTax);
            ReportParameter dtStateUnempTax = new ReportParameter("StateUnempTax", strStateUnempTax);
            ReportParameter dtTotalTax = new ReportParameter("TotalTax", strTotalTax);
            ReportParameter dtTotalSocTax = new ReportParameter("TotalSocTax", strTotalSocialSecurityTax);
            ReportParameter dtTotalMedTax = new ReportParameter("TotalMedTax", strTotalMedicareTax);

            ReportParameter dtState = new ReportParameter("State", State);

            this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/EmployerReport.rdlc");
            ReportDataSource source = new ReportDataSource("DataSet_EmployerReport", dt);
            this.ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(source);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] {
                txtCompanyName,
                dtFromDate,
                dtToDate,
                dtEID,
                dtStateID,
                dtNumberOfEmployee,
                dtWages,
                dtSSWages,
                dtSSTip,
                dtMedicareWages,
                dtAdvancedEIC,
                dtFedTax,
                dtEmployeeSSTax,
                dtEmployeeMedTax,
                dtStateTax,
                dtLocalTax,
                dtEmployerSSTax,
                dtEmployerMedTax,
                dtFedUnempTax,
                dtStateUnempTax,
                dtTotalTax,
                dtTotalSocTax,
                dtTotalMedTax,
                dtState
            });
            this.ReportViewer1.LocalReport.Refresh();

            //缩放模式为百分比,以100%方式显示
            this.ReportViewer1.ZoomMode = ZoomMode.Percent;
            this.ReportViewer1.ZoomPercent = 100;
        }
        private void EmployeeSummary_Data_Binding(int CompanyId, string strDate, DateTime fromDate, DateTime endDate) {
            DataTable ReportSumary;
            this.ReportViewer1.Reset();
            this.ReportViewer1.LocalReport.Dispose();

            string strSummary = "select sum(s118) as S118, sum(s122) as S122, FName, LName, SSN, sum(s114) as S114, sum(s115) as S115 from T105 where " + strDate + ">='" + fromDate + "' and " + strDate + "<'" + endDate.AddDays(1.0).ToShortDateString() + "' and CompanyId = " + CompanyId + " group by SSN, FName, LName";// +"# Order by " + orderby;
            ReportSumary = DBHelper.getAllEntity(strSummary);

            ReportParameter title = new ReportParameter("ReportType", "Accrual Basis");
            string currentCompany = getCompanyName(CompanyId);
            string strFromDate = fromDate.ToShortDateString() + " 12:00:00 AM";
            string strToDate = endDate.ToShortDateString() + " 11:59:59 PM";
            ReportParameter txtCompanyName = new ReportParameter("CompanyName", currentCompany);
            ReportParameter dtFromDate = new ReportParameter("FromDate", strFromDate);
            ReportParameter dtToDate = new ReportParameter("ToDate", strToDate);
            this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/EmployeeSummary.rdlc");
            ReportDataSource source = new ReportDataSource("EmployeeSummary_DataSet", ReportSumary);
            this.ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(source);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] {
                title,
                txtCompanyName,
                dtFromDate,
                dtToDate
            });
            this.ReportViewer1.LocalReport.Refresh();
            //缩放模式为百分比,以100%方式显示
            this.ReportViewer1.ZoomMode = ZoomMode.Percent;
            this.ReportViewer1.ZoomPercent = 100;

        }
        private void EmployeeDetail_Data_Binding(int CompanyId, string EmployeeId, string strDate, DateTime fromDate, DateTime endDate) {
            DataTable ReportSumary;
            this.ReportViewer1.Reset();
            this.ReportViewer1.LocalReport.Dispose();

            string strSummary = "";
            string sLeft = EmployeeId;
            if (sLeft == "All")
            {
                strSummary = @"select sum(s111) as fedtax,  sum(s109) as socialtax, sum(s110) as medtax, sum(s112) as statetax, sum(s113) as localtax, sum(AdvanceEIC) as advanceeicpay from T105 where " + strDate + ">='" + fromDate + "' and " + strDate + "<'" + endDate.AddDays(1.0).ToShortDateString() + "' and CompanyId = " + CompanyId;
            }
            else
            {
                strSummary = @"select sum(s111) as fedtax,  sum(s109) as socialtax, sum(s110) as medtax, sum(s112) as statetax, sum(s113) as localtax, sum(AdvanceEIC) as advanceeicpay from T105 where EmployeeId='" + EmployeeId + "' and  " + strDate + ">='" + fromDate + "' and " + strDate + "<'" + endDate.AddDays(1.0).ToShortDateString() + "' and CompanyId = " + CompanyId;
            }
            ReportSumary = DBHelper.getAllEntity(strSummary);
            if (sLeft == "All")
            {

                strSummary = "select CheckNo, S122, DateStartPeriod, DateEndPeriod, DateSubmitted, FName + ' ' + LName as FName from T105 where " + strDate + ">='" + fromDate + "' and " + strDate + "<'" + endDate.AddDays(1.0).ToShortDateString() + "' and CompanyId = " + CompanyId;
            }
            else
            {
                strSummary = "select CheckNo, S122, DateStartPeriod, DateEndPeriod, DateSubmitted, FName + ' ' + LName as FName from T105 where EmployeeId='" + EmployeeId + "' and  " + strDate + ">='" + fromDate + "' and " + strDate + "<'" + endDate.AddDays(1.0).ToShortDateString() + "' and CompanyId = " + CompanyId;

            }
            DataTable dt_AccountEntity = DBHelper.getAllEntity(strSummary);
            string strName = "Display All";
            string strSSN = "";
            if (EmployeeId != "All") {
                string text1 = "select FName + ' ' + LName as Name,SSN FROM Employee where EmployeeId = '" + EmployeeId + "'";
                DataTable EmployeeInfo = DBHelper.getAllEntity(text1);
                strName = (EmployeeInfo.Rows[0]["Name"]).ToString();
                strSSN = (EmployeeInfo.Rows[0]["SSN"]).ToString();
            };
            string currentCompany = getCompanyName(CompanyId);
            string strFromDate = fromDate.ToShortDateString() + " 12:00:00 AM";
            string strToDate = endDate.ToShortDateString() + " 11:59:59 PM";

            string fedtax = (ReportSumary.Rows[0]["fedtax"].ToString()=="")?"0": ReportSumary.Rows[0]["fedtax"].ToString();
            string socialtax = (ReportSumary.Rows[0]["socialtax"].ToString() == "")?"0": ReportSumary.Rows[0]["socialtax"].ToString();
            string medtax = (ReportSumary.Rows[0]["medtax"].ToString() == "")?"0": ReportSumary.Rows[0]["medtax"].ToString();
            string statetax = (ReportSumary.Rows[0]["statetax"].ToString() == "")?"0": ReportSumary.Rows[0]["statetax"].ToString();
            string localtax = (ReportSumary.Rows[0]["localtax"].ToString() == "")?"0": ReportSumary.Rows[0]["localtax"].ToString();
            string advanceeicpay = (ReportSumary.Rows[0]["advanceeicpay"].ToString() == "") ? "0" : ReportSumary.Rows[0]["advanceeicpay"].ToString();
            ReportParameter title = new ReportParameter("ReportType", "Accrual Basis");
            ReportParameter txtName = new ReportParameter("Name", strName);
            ReportParameter txtSSN = new ReportParameter("SSN", strSSN);
            ReportParameter txtCompanyName = new ReportParameter("CompanyName", currentCompany);
            ReportParameter dtFromDate = new ReportParameter("FromDate", strFromDate);
            ReportParameter dtToDate = new ReportParameter("ToDate", strToDate);

            ReportParameter Fedtax = new ReportParameter("Fedtax", fedtax);
            ReportParameter Socialtax = new ReportParameter("Socialtax", socialtax);
            ReportParameter Medtax = new ReportParameter("Medtax", medtax);
            ReportParameter Statetax = new ReportParameter("Statetax", statetax);
            ReportParameter Localtax = new ReportParameter("Localtax", localtax);
            ReportParameter Advanceeicpay = new ReportParameter("Advanceeicpay", advanceeicpay);
            this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/EmployeeDetail.rdlc");

            DataTable dtPayment = new DataTable();
            dtPayment.Columns.Add("Item");
            dtPayment.Columns.Add("TotalValue", typeof(Single));
            DataTable mdtPayMent = new DataTable();
            string strSql = "SELECT Ord , CodeMap, Description FROM T201 where CompanyId = " + CompanyId + "  and Enabled = 1 order by Ord";
            mdtPayMent = DBHelper.getAllEntity(strSql);
            foreach (DataRow dr in mdtPayMent.Rows)
            {
                string Description = dr["Description"].ToString();
                string CodeMap = dr["CodeMap"].ToString();
                string QuerrySql = "select sum(" + CodeMap + ") as TotalValue from T105 where " + strDate + ">='" + fromDate + "' and " + strDate + "<'" + endDate.AddDays(1.0).ToShortDateString() + "' and CompanyId = " + CompanyId;
                if (EmployeeId != "All")
                {
                    QuerrySql = QuerrySql + " and EmployeeId = '" + EmployeeId + "'";
                }
                string ItemValue = DBHelper.GetSingleValue(QuerrySql).ToString();
                float TotalValue = (ItemValue == "") ? 0 : float.Parse(ItemValue);
                DataRow Ndr = dtPayment.NewRow();
                Ndr["Item"] = Description;
                Ndr["TotalValue"] = TotalValue;
                dtPayment.Rows.Add(Ndr);
            }
            ReportDataSource source_payment = new ReportDataSource("EmployeeDetail_Payment_DataSet", dtPayment);

            DataTable dtDeduction = new DataTable();
            dtDeduction.Columns.Add("Item");
            dtDeduction.Columns.Add("TotalValue", typeof(Single));
            strSql = "SELECT ItemId , CodeMap, Description FROM T102 where CompanyId = " + CompanyId + "  and Enabled = 1 order by ItemId";
            DataTable mdtDeduction = DBHelper.getAllEntity(strSql);
            foreach (DataRow dr in mdtDeduction.Rows)
            {
                string Description = dr["Description"].ToString();
                string CodeMap = dr["CodeMap"].ToString();
                string QuerrySql = "select sum(" + CodeMap + ") as TotalValue from T105 where " + strDate + ">='" + fromDate + "' and " + strDate + "<'" + endDate.AddDays(1.0).ToShortDateString() + "' and CompanyId = " + CompanyId;
                if (EmployeeId != "All")
                {
                    QuerrySql = QuerrySql + " and EmployeeId = '" + EmployeeId + "'";
                }
                string ItemValue = DBHelper.GetSingleValue(QuerrySql).ToString();
                float TotalValue = (ItemValue == "") ? 0 : float.Parse(ItemValue);
                DataRow Ndr = dtDeduction.NewRow();
                Ndr["Item"] = Description;
                Ndr["TotalValue"] = TotalValue;
                dtDeduction.Rows.Add(Ndr);
            }
            ReportDataSource source_deduction = new ReportDataSource("EmployeeDetail_Deduction_DataSet", dtDeduction);

            ReportDataSource source_accountEntity = new ReportDataSource("EmployeeDetail_AccountEntity_DataSet", dt_AccountEntity);

            ReportDataSource source_null = new ReportDataSource("EmployeeDetail_Null_DataSet", new DataTable());

            this.ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(source_accountEntity);

            ReportViewer1.LocalReport.DataSources.Add(source_payment);
            ReportViewer1.LocalReport.DataSources.Add(source_deduction);
            ReportViewer1.LocalReport.DataSources.Add(source_null);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] {
                title,
                txtCompanyName,
                dtFromDate,
                dtToDate,
                txtName,
                txtSSN,
                Fedtax,
                Socialtax,
                Medtax,
                Statetax,
                Localtax,
                Advanceeicpay
            });
            this.ReportViewer1.LocalReport.Refresh();
            //缩放模式为百分比,以100%方式显示
            this.ReportViewer1.ZoomMode = ZoomMode.Percent;
            this.ReportViewer1.ZoomPercent = 100;

        }
        private void EmployeePTO_Data_Binding(int CompanyId, string EmployeeId,string strDate, int Year, string Type,double UserTimeZone) {
            DataTable ReportSumary;
            this.ReportViewer1.Reset();
            this.ReportViewer1.LocalReport.Dispose();
            this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/EmployeePTO.rdlc");

            DateTime PTOYearFromDate = TimeHelper.GetUTCTime(new DateTime(Year, 1, 1), UserTimeZone);
            DateTime PTOYearEndDate = TimeHelper.GetUTCTime(new DateTime(Year, 1, 1).AddMonths(12), UserTimeZone);
            string StrHours = "PTOAccHours";
            if (Type == "2") {
                StrHours = "VacAccHours";
            };
            string strSummary = "";
            if (EmployeeId == "All")
            {
                strSummary = "select Date,EmployeeName,Hours,Memo from AccrualTimeJournal where Date>='" + PTOYearFromDate + "' and Date<'" + PTOYearEndDate + "' and Type = " + Type + " Union All " +
                "select " + strDate + " as Date , FName + ' ' + LName as EmployeeName  ," + StrHours + " as Hours, Memo from T105 where " + strDate + " >= '" + PTOYearFromDate + "' and " + strDate + "<='" + PTOYearFromDate + "'";
            }
            else {
                strSummary = "select Date,EmployeeName,Hours,Memo from AccrualTimeJournal where Date>='" + PTOYearFromDate + "' and Date<'" + PTOYearEndDate + "' and Type = " + Type + " and EmployeeId='" + EmployeeId + "' Union All " +
                "select " + strDate + " as Date , FName + ' ' + LName as EmployeeName  ," + StrHours + " as Hours, Memo from T105 where " + strDate + " >= '" + PTOYearFromDate + "' and " + strDate + "<='" + PTOYearFromDate + "' and EmployeeId='" + EmployeeId + "'";
            };
            ReportSumary = DBHelper.getAllEntity(strSummary);

            ReportDataSource source = new ReportDataSource("EmployeePTO_DataSet", ReportSumary);
            this.ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(source);

            string currentCompany = getCompanyName(CompanyId);
            string strName = "Display All";
            string strSSN = "";
            if (EmployeeId != "All")
            {
                string text1 = "select FName + ' ' + LName as Name,SSN FROM Employee where EmployeeId = '" + EmployeeId + "'";
                DataTable EmployeeInfo = DBHelper.getAllEntity(text1);
                strName = (EmployeeInfo.Rows[0]["Name"]).ToString();
                strSSN = (EmployeeInfo.Rows[0]["SSN"]).ToString();
            };
            string strReportType = "Paid Time Off Report";
            if (Type == "2") {
                strReportType = "Paid Vacation Report";
            }
            ReportParameter title = new ReportParameter("ReportType", strReportType);
            ReportParameter txtName = new ReportParameter("Name", strName);
            ReportParameter txtSSN = new ReportParameter("SSN", strSSN);
            ReportParameter txtCompanyName = new ReportParameter("CompanyName", currentCompany);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] {
                title,
                txtName,
                txtSSN,
                txtCompanyName
            });

            this.ReportViewer1.LocalReport.Refresh();
            //缩放模式为百分比,以100%方式显示
            this.ReportViewer1.ZoomMode = ZoomMode.Percent;
            this.ReportViewer1.ZoomPercent = 100;
        }
        private DataTable getCompany(int Id) {
            string sql = @"select CompanyName,FedTaxId,PayReportByEndingDate from Company where CompanyId=" + Id;
            DataTable companyInfo = DBHelper.getAllEntity(sql);
            return companyInfo;
        }
        private string getCompanyName(int Id) {
            DataTable companyInfo = getCompany(Id);
            return (companyInfo.Rows[0]["CompanyName"]).ToString();
        }
        private string getFedTaxId(int Id)
        {
            DataTable companyInfo = getCompany(Id);
            return (companyInfo.Rows[0]["FedTaxId"]).ToString();
        }
        private string PayReportOrEndingDate(int Id) {
            DataTable companyInfo = getCompany(Id);
            bool PayReportByEndingDate = Boolean.Parse(companyInfo.Rows[0]["PayReportByEndingDate"].ToString());
            string field = "DateSubmitted";
            if (PayReportByEndingDate) {
                field = "DateEndPeriod";
            }
            return field;
        }
    }
}