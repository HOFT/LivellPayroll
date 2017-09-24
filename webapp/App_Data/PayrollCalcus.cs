using System;

using System.Data;
using System.Data.OleDb;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using LivellPayRoll.Models;
using LivellPayRoll.App_Helpers;

namespace LivellPayroll
{

    internal class PayrollCalcs
	{
		// Methods
		protected internal PayrollCalcs(DateTime YTDDate, string strSSN)
		{
			
			this.mYTDDate = YTDDate;
			this.mdtPayrollSumsYTD = new DataTable();
			this.mdtPayrollData = new DataTable();
			this.mdtPayrollSumsYTD = this.PayrollSumsYTD();
            this.mdtAccuraHoursYTD = this.AccuralSumsYTD();
			this.mstrSSN = strSSN;

			
		}
 
		protected internal PayrollCalcs(ref DataRow drPayrolldata,Company company,string strDate)
		{
            this.company = company;
            this.strDate = strDate;
            this.mYTDDate = DateTime.Now;
			this.mdtPayrollSumsYTD = new DataTable();
			this.mdtPayrollData = new DataTable();
			//this.mcnnPayroll = cnnPayroll;
			//this.mcnnTaxTables = cnnTaxTables;
			//this.mWeeklyConvFactor = this.WeeklyConvFactor();
			this.mdtPayrollSumsYTD = this.PayrollSumsYTD();
            this.mdtAccuraHoursYTD = this.AccuralSumsYTD();
			DataRow row1 = drPayrolldata;
			this.mblnCollectFedIncomeTax = Function.GetSafeBool(row1["F114"]);
			this.mblnCollectFedUnempTax = Function.GetSafeBool(row1["F115"]);
			this.mblnCollectMedicareTax = Function.GetSafeBool(row1["F119"]);
			this.mblnCollectSocialSecurityTax = Function.GetSafeBool(row1["F118"]);
			this.mblnCollectStateIncomeTax = Function.GetSafeBool(row1["F116"]);
			this.mblnCollectStateUnempTax = Function.GetSafeBool(row1["F117"]);
			this.msngFedIncomeTaxAdd = Function.GetSafeSingle(row1["F121"]);
			this.msngStateIncomeTaxAdd = Function.GetSafeSingle(row1["F122"]);
			this.msngStateTaxPctFedTax = Function.GetSafeSingle(row1["F120"]);
			this.msngNumOfAllow = Function.GetSafeSingle(row1["F107"]);
			this.msngStateNumOfAllow = Function.GetSafeSingle(row1["F112"]);
			this.msngStateNumOfAllow1 = Function.GetSafeSingle(row1["F113"]);
			this.mstrMaritalStatus = row1["F106"].ToString();
			this.mstrStateMaritalStatus = row1["F110"].ToString();
			this.mstrResidentStatusState = row1["F111"].ToString();
			//this.mstrStateAbbr = row1["EmploymentState"].ToString();
			this.mstrStateAbbr = row1["State"].ToString();
			this.mstrCountyAbbr = row1["F109"].ToString();
			this.mstrSSN = row1["SSN"].ToString();
			this.msngUD1Input = Function.GetSafeSingle(row1["F1231"]);
            this.msngUD2Input = Function.GetSafeSingle(row1["F1232"]);
            this.msngUD3Input = Function.GetSafeSingle(row1["F1233"]);
            this.msngUD4Input = Function.GetSafeSingle(row1["F1234"]);
            this.msngUD5Input = Function.GetSafeSingle(row1["F1235"]);
            this.msngUD6Input = Function.GetSafeSingle(row1["F1236"]);
            this.msngUD7Input = Function.GetSafeSingle(row1["F1237"]);

            this.msngStateUnempTaxRate = Function.GetSafeSingle(row1["S116"]);
            this.msngStateUnempWageLimit = Function.GetSafeSingle(row1["S117"]);
            this.msngSalaryAnnual = Function.GetSafeSingle(row1["F102"]);
            this.msngPayRatePerHr = Function.GetSafeSingle(row1["F100"]);

            this.msngPayRateSickHr = Function.GetSafeSingle(row1["SickRate"]);
            this.msngPayRateVacationHr = Function.GetSafeSingle(row1["VacationRate"]);

            this.msngOTMultiplier = Function.GetSafeSingle(row1["F101"]);
            this.msngHrsRegular = Function.GetSafeSingle(row1["S100"]);
            this.msngHrsOT = Function.GetSafeSingle(row1["S101"]);
            this.msngHrsSick = Function.GetSafeSingle(row1["S103"]);
            this.msngHrsVacation = Function.GetSafeSingle(row1["S102"]);
            this.mblnSalary = Function.GetSafeBool(row1["F125"]);
            this.msngAdvanceEIC = Function.GetSafeSingle(row1["AdvanceEIC"]);
            this.msngLocalIncomeTax = Function.GetSafeSingle(row1["S113"]);

            this.msngPTOAccRate = Function.GetSafeSingle(row1["PTOAccRate"]);
            this.msngVacAccRate = Function.GetSafeSingle(row1["VacAccRate"]);
            this.msngPTOCap = Function.GetSafeSingle(row1["PTOCapHours"]);
            this.msngVacCap = Function.GetSafeSingle(row1["VacCapHours"]);

            
            row1 = null;
		}

		protected internal float CalcAdvanceEIC()
		{
			//float single2 = (this.msngOTMultiplier * this.msngHrsOT) * this.msngPayRatePerHr;
			
			return (float) Math.Round((double) msngAdvanceEIC, 2, MidpointRounding.AwayFromZero);
		}
		protected internal float CalcLocalIncomeTax()
		{
			//float single2 = (this.msngOTMultiplier * this.msngHrsOT) * this.msngPayRatePerHr;

            return (float)Math.Round((double)msngLocalIncomeTax, 2, MidpointRounding.AwayFromZero);
		}
 
		protected internal object CalcDeduction(int intItem, float sngGrossIncome, DateTime dtDate)
		{
			bool flag1=false;
			float single1=0;
			float single2=0;
			float single3=0;
            Random random = new Random();
            single2 = (float)(random.Next(0, 10000))/100;

            
            return Math.Round((double)single2, 2, MidpointRounding.AwayFromZero);
		}
 
		protected internal float CalcFedIncometax(float sngGrossIncome, int intTaxTableYear)
		{
			float single3;
            //OleDbCommand command1 = new OleDbCommand();
            Random random = new Random();
            single3 = (float) (random.Next(0, 10000)) / 100;

            return (float)Math.Round((double)single3, 2, MidpointRounding.AwayFromZero);
		}
 


		protected internal float CalcFedUnempTax(object sngGrossIncome, int intTaxTableYear, float futarate, float futauplimit)
		{
			float single4;
           
            Random random = new Random();
            single4 = (float) (random.Next(0, 10000)) / 100;

            return (float)Math.Round((double)single4, 2, MidpointRounding.AwayFromZero);
		}
 
		//protected internal float CalcGrossFICATaxable(object sngGrossIncome, object sngUD1Val, object sngUD2Val, object sngUD3Val, object sngUD4Val, object sngUD5Val, object sngUD6Val, object sngUD7Val, object sngUD8Val, DateTime dtDate)
        protected internal float CalcGrossFICATaxable(object sngGrossIncome, object sngUD1Val, object sngUD2Val, object sngUD3Val, object sngUD4Val, object sngUD5Val, object sngUD6Val, object sngUD7Val, DateTime dtDate)
		{
			
		
            Random random = new Random();
            float single2 = (float) (random.Next(0, 10000)) / 100;

            return (float)Math.Round((double)single2, 2, MidpointRounding.AwayFromZero);
		}
 
		//protected internal float CalcGrossSSTaxable(float sngGrossIncome, float sngUD1Val, float sngUD2Val, float sngUD3Val, float sngUD4Val, float sngUD5Val, float sngUD6Val, float sngUD7Val, float sngUD8Val,DateTime dtDate)
        protected internal float CalcGrossSSTaxable(float sngGrossIncome, float sngUD1Val, float sngUD2Val, float sngUD3Val, float sngUD4Val, float sngUD5Val, float sngUD6Val, float sngUD7Val, DateTime dtDate)
		{
            Random random = new Random();
            float single2 = (float) (random.Next(0, 10000)) / 100;
            return (float)Math.Round((double)single2, 2, MidpointRounding.AwayFromZero);
		}
 
		//protected internal float CalcGrossTaxable(object sngGrossIncome, object sngUD1Val, object sngUD2Val, object sngUD3Val, object sngUD4Val, object sngUD5Val, object sngUD6Val, object sngUD7Val, object sngUD8Val)
		protected internal float CalcGrossTaxable(object sngGrossIncome, object sngUD1Val, object sngUD2Val, object sngUD3Val, object sngUD4Val, object sngUD5Val, object sngUD6Val, object sngUD7Val )
		{
            Random random = new Random();
            float single2 = (float) (random.Next(0, 10000)) / 100;
            return (float)Math.Round((double)single2, 2, MidpointRounding.AwayFromZero);
		}
 
		protected internal float CalcMedicareTax(object sngGrossIncome, int intTaxTableYear)
		{
			float single3;
            //OleDbCommand command1 = new OleDbCommand();
            Random random = new Random();
            single3 = (float) (random.Next(0, 10000)) / 100;
            return (float)Math.Round((double)single3, 2, MidpointRounding.AwayFromZero);
		}
        protected internal float CalcMedicareTaxExtra(object sngGrossIncome, int intTaxTableYear, float fYTDTaxable)
        {
            float single3 = 0;
            //OleDbCommand command1 = new OleDbCommand();
            Random random = new Random();
            single3 = (float) (random.Next(0, 10000)) / 100;
            return (float)Math.Round((double)single3, 2, MidpointRounding.AwayFromZero);
        }
		protected internal float CalcPayOT()
		{
			//float single2 = (this.msngOTMultiplier * this.msngHrsOT) * this.msngPayRatePerHr;
			float single2 = (this.msngOTMultiplier * this.msngHrsOT);
            return (float)Math.Round((double)single2, 2, MidpointRounding.AwayFromZero);
		}


 
		protected internal float CalcPayRegular()
		{
			
            Random random = new Random();
            float single2 = (float) (random.Next(0, 10000)) / 100;
            return (float) Math.Round((double) single2, 2, MidpointRounding.AwayFromZero);
		}
        
        protected internal float CalcPayDouble()
        {

            Random random = new Random();
            float single2 = (float)(random.Next(0, 10000)) / 100;
            return (float)Math.Round((double)single2, 2, MidpointRounding.AwayFromZero);
        }

        protected internal float CalcPaySalary()
		{
            Random random = new Random();
            float single2 = (float) (random.Next(0, 10000)) / 100;
            return (float)Math.Round((double)single2, 2, MidpointRounding.AwayFromZero);
		}
 
		protected internal float CalcPaySick()
		{
            Random random = new Random();
            float single2 = (float) (random.Next(0, 10000)) / 100;
            return (float)Math.Round((double)single2, 2, MidpointRounding.AwayFromZero);
		}
 
		protected internal float CalcPayVacation()
		{
            Random random = new Random();
            float single2 = (float) (random.Next(0, 10000)) / 100;
            return (float)Math.Round((double)single2, 2, MidpointRounding.AwayFromZero);
		}
 
		protected internal float CalcSSTax(object sngGrossIncome, int intTaxTableYear)
		{
            Random random = new Random();
            float single4 = (float) (random.Next(0, 10000)) / 100;
            return (float)Math.Round((double)single4, 2, MidpointRounding.AwayFromZero);
		}
 

		protected internal float CalcSSTaxEmployer(object sngGrossIncome, int intTaxTableYear)
		{
            Random random = new Random();
            float single4 = (float) (random.Next(0, 10000)) / 100;
            return (float)Math.Round((double)single4, 2, MidpointRounding.AwayFromZero);
		}
 

		protected internal float CalcStateIncomeTax(object sngGrossIncome, object sngIncometax, object sngSSTax, object sngMedicareTax, int intTaxTableYear)
		{
            float single4 = 0;
            float single5 = 0;
            //OleDbCommand command1 = new OleDbCommand();
            sngGrossIncome = RuntimeHelpers.GetObjectValue(this.NumberCheck(RuntimeHelpers.GetObjectValue(sngGrossIncome)));
            sngIncometax = RuntimeHelpers.GetObjectValue(this.NumberCheck(RuntimeHelpers.GetObjectValue(sngIncometax)));
            sngSSTax = RuntimeHelpers.GetObjectValue(this.NumberCheck(RuntimeHelpers.GetObjectValue(sngSSTax)));
            sngMedicareTax = RuntimeHelpers.GetObjectValue(this.NumberCheck(RuntimeHelpers.GetObjectValue(sngMedicareTax)));
            string text3 = this.mstrStateAbbr;
            string text1 = this.mstrCountyAbbr;
            string text4 = this.mstrStateMaritalStatus;
            string text5 = this.mstrResidentStatusState;
            float single6 = this.msngStateNumOfAllow;
            float single7 = this.msngStateNumOfAllow1;
            float single8 = this.msngStateTaxPctFedTax;
            //chenxi 03/21/2009
            single5 = this.msngStateIncomeTaxAdd;
            //single5 = single5;
            float single2 = this.MedicareTaxYTD(intTaxTableYear);
            float single3 = this.SSTaxYTD(intTaxTableYear);
            //command1.Connection = this.mcnnTaxTables;
            //StateTaxCalcs calcs1 = new StateTaxCalcs(this.mcnnPayroll, this.mcnnTaxTables);
            StateTaxCalcs calcs1 = new StateTaxCalcs(this.mcnnPayroll,this.company);
            if (this.mblnCollectStateIncomeTax)
            {
                if (single6 == 99) //Manually input state tax with 99 claims
                {
                    single4 += single5;
                }
                else
                {
                    bool flag1 = true;
                    calcs1.StateWithHoldings(text3, text1, text4, text5, single6, single7, SingleType.FromObject(sngGrossIncome), SingleType.FromObject(sngIncometax), SingleType.FromObject(sngSSTax), SingleType.FromObject(sngMedicareTax), single8, intTaxTableYear, ref single4, single3, single2, ref flag1);
                    single4 += single5;
                    if (!flag1)
                    {
                        float single1 = 0;
                        return single1;
                    }
                }
            }
            else
            {
                single4 = 0f;
            }
            //command1.Dispose();
            if (single4 < 0f)
            {
                single4 = 0f;
            }
            return (float)Math.Round((double)single4, 2, MidpointRounding.AwayFromZero);
		}

        protected internal float CalcPTOAcc(int intTaxTableYear)
        {
            Random random = new Random();
            float single2 = (float) (random.Next(0, 10000)) / 100;
            return (float)Math.Round((double)single2, 2, MidpointRounding.AwayFromZero);
        }

        protected internal float CalcVacAcc(int intTaxTableYear)
        {
            Random random = new Random();
            float single2 = (float) (random.Next(0, 10000)) / 100;
            return (float)Math.Round((double)single2, 2, MidpointRounding.AwayFromZero);
        }
		protected internal float CalcStateUnempTax(object sngGrossIncome, DateTime dtDate)
		{
            Random random = new Random();
            float single2 = (float) (random.Next(0, 10000)) / 100;
            return (float)Math.Round((double)single2, 2, MidpointRounding.AwayFromZero);
		}
 
		public float FedUnemptaxYTD(int intYear)
		{
            Random random = new Random();
            float single2 = (float) (random.Next(0, 10000)) / 100;
            return single2;
		}

		public float MedicareTaxYTD(int intYear)
		{
            Random random = new Random();
            float single2 = (float) (random.Next(0, 10000)) / 100;
            return single2;
		}
 
		public float EmployerMedicareYTD(int intYear)
		{
            Random random = new Random();
            float single2 = (float) (random.Next(0, 10000)) / 100;
            return single2;
		}
		public float EmployerSSTaxYTD(int intYear)
		{
            Random random = new Random();
            float single2 = (float) (random.Next(0, 10000)) / 100;
            return single2;
		}
		public float LocalTaxYTD(int intYear)
		{
            Random random = new Random();
            float single2 = (float) (random.Next(0, 10000)) / 100;
            return single2;
		}
		public float StateTaxYTD(int intYear)
		{
            Random random = new Random();
            float single2 = (float) (random.Next(0, 10000)) / 100;
            return single2;
		}

		public float FederalTaxYTD(int intYear)
		{
            Random random = new Random();
            float single2 = (float) (random.Next(0, 10000)) / 100;
            return single2;
		}

		public float GrossIncomeYTD(int intYear)
		{
            Random random = new Random();
            float single2 = (float) (random.Next(0, 10000)) / 100;
            return single2;
		}
		public float GrossTaxableIncomeYTD(int intYear)
		{
            Random random = new Random();
            float single2 = (float) (random.Next(0, 10000)) / 100;
            return single2;
		}
		public float GrossFICATaxableIncomeYTD(int intYear)
		{
            Random random = new Random();
            float single2 = (float) (random.Next(0, 10000)) / 100;
            return single2;
		}
		public float GrossSSIncomeYTD(int intYear)
		{
            Random random = new Random();
            float single2 = (float) (random.Next(0, 10000)) / 100;
            return single2;
		}
        public float GrossSSTipIncomeYTD(int intYear)
        {
            Random random = new Random();
            float single2 = (float) (random.Next(0, 10000)) / 100;
            return single2;
        }
		public float SSTaxYTD(int intYear)
		{
            Random random = new Random();
            float single2 = (float) (random.Next(0, 10000)) / 100;
            return single2;
		}
 
		public float StateUnemptaxYTD(int intYear)
		{
            Random random = new Random();
            float single1 = (float) (random.Next(0, 10000)) / 100;
            return single1;
		}
 
		public float UDValYTD(int intItem, int intYear)
		{
            Random random = new Random();
            float single1 = (float) (random.Next(0, 10000)) / 100;
            return single1;
		}
 
		public float UDPay1ValYTD(int intYear)
		{
            Random random = new Random();
            float single1 = (float) (random.Next(0, 10000)) / 100;
            return single1;
		}
 
		public float UDPay2ValYTD(int intYear)
		{
            Random random = new Random();
            float single1 = (float) (random.Next(0, 10000)) / 100;
            return single1;
		}
		public float UDPay3ValYTD(int intYear)
		{
            Random random = new Random();
            float single1 = (float) (random.Next(0, 10000)) / 100;
            return single1;
		}
		public float UDPay4ValYTD(int intYear)
		{
            Random random = new Random();
            float single1 = (float) (random.Next(0, 10000)) / 100;
            return single1;
		}
		public float UDPay5ValYTD(int intYear)
		{
            Random random = new Random();
            float single1 = (float) (random.Next(0, 10000)) / 100;
            return single1;
		}
		public float AdvanceEICYTD(int intYear)
		{
            Random random = new Random();
            float single1 = (float) (random.Next(0, 10000)) / 100;
            return single1;
		}
		public float PaySalaryYTD(int intYear)
		{
            Random random = new Random();
            float single1 = (float) (random.Next(0, 10000)) / 100;
            return single1;
		}

        public float PTOYTD(int intYear)
        {
            Random random = new Random();
            float single1 = (float) (random.Next(0, 10000)) / 100;
            return single1;
        }

        public float VacYTD(int intYear)
        {
            float single1 = 0;
            float single2 = 0;
            Random random = new Random();
            single1 = (float) (random.Next(0, 10000)) / 100;
            return single1 + single2;
        }

		public float PayRegularYTD(int intYear)
		{
            Random random = new Random();
            float single1 = (float) (random.Next(0, 10000)) / 100;
            return single1;
		}

        public float PayDTYTD(int intYear)
        {
            Random random = new Random();
            float single1 = (float)(random.Next(0, 10000)) / 100;
            return single1;
        }
        
        public float PayOTYTD(int intYear)
		{
            Random random = new Random();
            float single1 = (float) (random.Next(0, 10000)) / 100;
            return single1;
		}
		public float PayVacationYTD(int intYear)
		{
            Random random = new Random();
            float single1 = (float) (random.Next(0, 10000)) / 100;
            return single1;
		}
		public float PaySickYTD(int intYear)
		{
            Random random = new Random();
            float single1 = (float) (random.Next(0, 10000)) / 100;
            return single1;
		}
		public float NetIncomeYTD(int intYear)
		{
            Random random = new Random();
            float single1 = (float) (random.Next(0, 10000)) / 100;
            return single1;
		}
		private string GetPayrollType()
		{
            return company.PayFreq;
            //return SettingHelper.RetrieveStringByKey("PayFreq");
		}
 
		protected internal object NumberCheck(object varInput)
		{
			if (Information.IsNumeric(RuntimeHelpers.GetObjectValue(varInput)))
			{
				return varInput;
			}
			return 0;
		}
 
		private DataTable PayrollSumsYTD()
		{
			DataTable table1 = new DataTable();
            string text1 = "SELECT Year(" + strDate + ") AS TaxYear, SSN,  Sum(S118) AS SGI, Sum(S119) as SGTI,  Sum(S120) as SGFTI, Sum(S121) AS SGSI, Sum(S104) AS SPS, Sum(S105) AS SPR, Sum(S106) AS SPO, Sum(S107) AS SPV, Sum(S108) AS SPSK, Sum(S110) AS SMT, Sum(S114) AS SFU, Sum(S115) AS SSU, Sum(S109) AS SST, Sum(S111) AS SIT, Sum(S112) AS SSIT, Sum(S113) AS SLIT, Sum(EmployerSocialSecurity) AS SESS, Sum(EmployerMedicare) AS SEM, Sum(S1241) AS SU1V, Sum(S1242) AS SU2V, Sum(S1243) AS SU3V, Sum(S1244) AS SU4V, Sum(S1245) AS SU5V, Sum(S1246) AS SU6V, Sum(S1247) AS SU7V, Sum(S1261) AS SUP1V, Sum(S1262) AS SUP2V, Sum(S1263) AS SUP3V, Sum(S1264) AS SUP4V, Sum(S1265) AS SUP5V, Sum(AdvanceEIC) AS SumOfAdvanceEIC, Sum(S122) AS SumOfNetIncome, Sum(PTOAcchours) as SumOfPTO, Sum(VacAccHours) as SumOfVac FROM T105 Where " + strDate + " <= '" + mYTDDate.ToShortDateString() + "' GROUP BY Year(" + strDate + "), SSN ";
			DBHelper.GetTable(ref table1, text1);
			return table1;
		}

        private DataTable AccuralSumsYTD()
        {
            DataTable table1 = new DataTable();
             string text1 = "SELECT Year(Date) AS TaxYear, SSN,  Sum(Hours) AS TotalHours, Type FROM AccrualTimeJournal Where Date <= '" + mYTDDate.ToShortDateString() + "' GROUP BY Year(Date), SSN, Type ";
            DBHelper.GetTable(ref table1, text1);
            return table1;
        }

		private float SalaryConvFactor()
		{
			float single1=0;
			string text1 = this.GetPayrollType();
			switch(text1)
			{
				case "Daily":
					single1 = 0.002739726f;
					break;

				case "Weekly":
					single1 = 0.01923077f;
					break;

				case "Biweekly":
					single1 = 0.03846154f;
					break;

				case "Semimonthly":
					single1 = 0.04166667f;
					break;

				case "Monthly":
					single1 = 0.08333333f;
					break;

			}


			return single1;
		}

        private float HourConvFactor()
        {
            float single1 = 0;
            string text1 = this.GetPayrollType();
            switch (text1)
            {
                case "Daily":
                    single1 = 8f;
                    break;

                case "Weekly":
                    single1 = 40f;
                    break;

                case "Biweekly":
                    single1 = 80f;
                    break;

                case "Semimonthly":
                    single1 = 86.6667f;
                    break;

                case "Monthly":
                    single1 = 173.3334f;
                    break;

            }

            return single1;
        }
		protected internal float SSIncomeLimit(int intTaxYear)
		{
			//OleDbCommand command1 = new OleDbCommand();
			//command1.Connection = this.mcnnTaxTables;
			float single1 = 0f;
			//string text1 = ("SELECT TOP 1 [TAXYEAR],*  FROM TR6  Where [TR124] = 'SS' And [TaxYear]<=" + StringType.FromInteger(intTaxYear)) + "  Order by [TaxYear] DESC";
			string text1 = "[TR124] = 'SS' And [TaxYear]<=" + StringType.FromInteger(intTaxYear);
			//command1.CommandText = text1;
			//OleDbDataReader reader1 = command1.ExecuteReader();
            DataRow[] Rows = GlobalClass.TaxTable.Tables["TR6"].Select(text1);
			//while (reader1.Read())
			{
				//single1 = SingleType.FromObject(reader1["TR127"]);
				single1 = SingleType.FromObject(Rows[0]["TR127"]);
			}
			//reader1.Close();
			//command1.Dispose();
			return single1;
		}
 

		private float WeeklyConvFactor()
		{
			float single1=0;
			string text1 = this.GetPayrollType();
			switch(text1)
			{
				case "Daily":
					single1 = 7f;
					break;

				case "Weekly":
					single1 = 1f;
					break;

				case "Biweekly":
					single1 = 0.5f;
					break;

				case "Semimonthly":
					single1 = 0.4615384f;
					break;

				case "Monthly":
					single1 = 0.2307692f;
					break;

			}


			return single1;
		}
 
 


		// Fields
	
		private bool mblnCollectFedIncomeTax;
		private bool mblnCollectFedUnempTax;
		private bool mblnCollectMedicareTax;
		private bool mblnCollectSocialSecurityTax;
		private bool mblnCollectStateIncomeTax;
		private bool mblnCollectStateUnempTax;
		private bool mblnSalary;
		private OleDbConnection mcnnPayroll;
		//private OleDbConnection mcnnTaxTables;
//		private DataRow[] mdrPayrollData;
		private DataTable mdtPayrollData;
		private DataTable mdtPayrollSumsYTD;
        private DataTable mdtAccuraHoursYTD;
		private float msngFedIncomeTaxAdd;

        private float msngPTOAccRate;
        private float msngVacAccRate;
        private float msngPTOCap;
        private float msngVacCap;


		private float msngHrsOT;
		private float msngHrsRegular;
		private float msngHrsSick;
		private float msngAdvanceEIC;
		private float msngHrsVacation;
		public float msngNumOfAllow;
		private float msngOTMultiplier;
		private float msngPayRatePerHr;
        private float msngPayRateSickHr;
        private float msngPayRateVacationHr;
		private float msngSalaryAnnual;
		private float msngStateIncomeTaxAdd;
		private float msngStateNumOfAllow;
		private float msngStateNumOfAllow1;
		private float msngStateTaxPctFedTax;
		private float msngStateUnempTaxRate;
		private float msngStateUnempWageLimit;
		private float msngLocalIncomeTax;
		private float msngUD1Input;
		private float msngUD2Input;
		private float msngUD3Input;
		private float msngUD4Input;
		private float msngUD5Input;
		private DateTime mYTDDate;
        private float msngUD6Input;
        private float msngUD7Input;
//		private float msngUD8Input;
		private string mstrCountyAbbr;
		private string mstrMaritalStatus;
		private string mstrResidentStatusState;
		private string mstrSSN;
		private string mstrStateAbbr;
		private string mstrStateMaritalStatus;
        private Company company;
        private string strDate;
        //private float mWeeklyConvFactor;
    }
}