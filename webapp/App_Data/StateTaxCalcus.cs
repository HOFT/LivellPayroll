using System;

using System.Data;
using System.Data.OleDb;
using Microsoft.VisualBasic.CompilerServices;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;
using LivellPayRoll.Models;
using LivellPayRoll.App_Helpers;

namespace LivellPayroll
{
    internal class StateTaxCalcs
	{
		protected internal StateTaxCalcs(OleDbConnection cnnPayroll, Company company)
		{
            this.company = company;
            this.dbh = new DBHelper();
			this.mcnnPayroll = cnnPayroll;
			//this.mcnnTaxTables = cnnTaxTables;
			this.mWeeklyConvFactor = this.WeeklyConvFactor();
		}

        private float AllowanceAdjustedWeeklyIncome()
        {
            Random random = new Random();
            float single19 = (float) (random.Next(0, 10000)) / 100;
            return (single19);
        }


        private float CalculateStateTaxes(string strQuery, float sngWeeklyIncome)
        {
           
            int num1 = 1;
            float single6 = 0f;
            float single9 = 0f;
            float single5 = 0f;
            float single1 = 0f;
            float single17 = 0f;
           
            string text1 = "(F106='" + this.mstrMStatus + "' or F106 is NULL or F106='' ) And [ST100]='" + this.mstrStateAbbr + "'";

            //command1.CommandText = text1;
            //OleDbDataReader reader1 = command1.ExecuteReader();
            DataRow[] Rows = GlobalClass.TaxTable.Tables["TR3"].Select(text1);
            //while (reader1.Read())
            {
                //				num1 = IntegerType.FromObject(this.NumberCheck(RuntimeHelpers.GetObjectValue(reader1["G100"])));
                //				single6 = SingleType.FromObject(ObjectType.DivObj(this.NumberCheck(RuntimeHelpers.GetObjectValue(reader1["TR104"])), num1));
                //				single9 = SingleType.FromObject(ObjectType.DivObj(this.NumberCheck(RuntimeHelpers.GetObjectValue(reader1["TR116"])), num1));
                //				single5 = SingleType.FromObject(ObjectType.DivObj(this.NumberCheck(RuntimeHelpers.GetObjectValue(reader1["TR117"])), num1));
                //				single1 = SingleType.FromObject(reader1["TR118"]);

                num1 = IntegerType.FromObject(this.NumberCheck(RuntimeHelpers.GetObjectValue(Rows[0]["G100"])));
                single6 = SingleType.FromObject(ObjectType.DivObj(this.NumberCheck(RuntimeHelpers.GetObjectValue(Rows[0]["TR104"])), num1));
                single9 = SingleType.FromObject(ObjectType.DivObj(this.NumberCheck(RuntimeHelpers.GetObjectValue(Rows[0]["TR116"])), num1));
                single5 = SingleType.FromObject(ObjectType.DivObj(this.NumberCheck(RuntimeHelpers.GetObjectValue(Rows[0]["TR117"])), num1));
                ////7/21/2014 For CA Low income only
                single17 = SingleType.FromObject(ObjectType.DivObj(this.NumberCheck(RuntimeHelpers.GetObjectValue(Rows[0]["TR105"])), num1));

                ////////////////////////////////////
                single1 = SingleType.FromObject(Rows[0]["TR118"]);
            }
            //reader1.Close();
            num1 = 1;
            float single3 = 0f;
            float single8 = 0f;
            float single4 = 0f;
            //command1.CommandText = strQuery;
            //OleDbDataReader reader2 = command1.ExecuteReader();
            DataRow[] Rows2 = GlobalClass.TaxTable.Tables[this.mstrStateAbbr].Select(strQuery);
            //while (reader2.Read())
            {
                num1 = IntegerType.FromObject(this.NumberCheck(RuntimeHelpers.GetObjectValue(Rows2[0]["G100"])));
                single3 = SingleType.FromObject(ObjectType.DivObj(this.NumberCheck(RuntimeHelpers.GetObjectValue(Rows2[0]["G103"])), num1));
                single8 = SingleType.FromObject(this.NumberCheck(RuntimeHelpers.GetObjectValue(Rows2[0]["G104"])));
                single4 = SingleType.FromObject(ObjectType.DivObj(this.NumberCheck(RuntimeHelpers.GetObjectValue(Rows2[0]["G101"])), num1));
            }
            //reader2.Close();
            //command1.Dispose();
            float single7 = ((single3 + ((sngWeeklyIncome - single4) * (single8 / 100f))) - (this.msngNumOfAllow1 * single6)) / this.mWeeklyConvFactor;
            if (single1 != 0f)
            {
                single7 = (float)Math.Round((double)single7, 0, MidpointRounding.AwayFromZero);
            }
            if ((single9 > 0f) & (single9 > single7))
            {
                single7 = 0f;
            }
            if ((single5 > 0f) & (single5 > sngWeeklyIncome))
            {
                ////7/21/2014 CA low income is before Standard deduction, the sngWeeklyIncome is after standard income, we need compensate it.
                if (this.mstrStateAbbr == "CA")
                {
                    if (single5 > sngWeeklyIncome + single17)
                    {
                        single7 = 0f;
                    }
                }
                else
                    single7 = 0f;
            }
            if (single7 < 0f)
            {
                single7 = 0f;
            }
            return single7;
        }
        
		private string GetPayrollType()
		{

            return company.PayFreq;
        }

        private string GetStateWHTblQuery(float sngWeeklyIncome)
        {
            

            //Modified by LI on Feb 15, 2008 for if the weekly income is less than deduction, should use
            string text2 = "(F106='" + this.mstrMStatus + "' or F106 is NULL or F106='' ) AND (([G101]/[G100] <=" + StringType.FromSingle(sngWeeklyIncome) + " AND [G102]/[G100]>" + StringType.FromSingle(sngWeeklyIncome) + ")  OR  ([G101]/[G100] <=" + StringType.FromSingle(sngWeeklyIncome) + " AND [G102] =0))";
            if (((StringType.StrCmp(this.mstrStateAbbr, "OR", true) == 0) & (StringType.StrCmp(this.mstrMStatus, "S", true) == 0)) & (this.msngNumOfAllow1 >= 3f))
            {


                text2 = "(F106='M' or F106 is NULL or F106='' ) AND (([G101]/[G100] <=" + StringType.FromSingle(sngWeeklyIncome) + " AND [G102]/[G100]>" + StringType.FromSingle(sngWeeklyIncome) + ")  OR  ([G101]/[G100] <=" + StringType.FromSingle(sngWeeklyIncome) + " AND [G102] =0))";

            }
            return text2;
        }
 
		protected internal object NumberCheck(object varInput)
		{
			if (Information.IsNumeric(RuntimeHelpers.GetObjectValue(varInput)))
			{
				return varInput;
			}
			return 0;
		}

        protected internal void StateWithHoldings(string strStateAbbr, string strCountyCode, string strMStatus, string strResidentStatusState, float sngNumOfAllow1, float sngNumOfAllow2, float sngGrossIncome, float sngFedIncomeTax, float sngSSTax, float sngMedicare, float sngStateTaxPctFedTax, int intTaxTableYear, ref float sngPayPeriodIncomeTax, float sngSSTaxYTD, float sngMedicareYTD, ref bool blnSuccess)
        {
            blnSuccess = false;
            this.mstrStateAbbr = strStateAbbr;
            this.mstrCountyCode = strCountyCode;
            this.msngNumOfAllow1 = sngNumOfAllow1;
            this.msngNumOfAllow2 = sngNumOfAllow2;
            this.mintTaxTableYear = intTaxTableYear;
            this.mstrPayrollType = this.GetPayrollType();
            this.msngGrossIncome = sngGrossIncome;
            this.msngFedIncomeTax = sngFedIncomeTax;
            this.msngMedicare = sngMedicare;
            this.msngSSTax = sngSSTax;
            this.msngMedicareYTD = sngMedicareYTD;
            this.msngSSTaxYTD = sngSSTaxYTD;
            this.mstrMStatus = strMStatus;
            this.mstrResidentStatusState = strResidentStatusState;
            if (this.msngGrossIncome > 0f)
            {
                float single1;
                string text2 = this.mstrStateAbbr;
                int localnumber = 0;
                switch (text2)
                {
                    case "AR":
                    case "CA":
                    case "CO":
                    case "DC":
                    case "DE":
                    case "GA":
                    case "HI":
                    case "IA":
                    case "ID":
                    case "KS":
                    case "KY":
                    case "ME":
                    case "MI":
                    case "MN":
                    case "MS":
                    case "MO":
                    case "MT":
                    case "NC":
                        localnumber = 1;
                        break;
                    case "AZ":
                        localnumber = 2;
                        break;
                    case "CT":
                        localnumber = 3;
                        break;
                    case "LA":
                        localnumber = 4;
                        break;
                    case "MA":
                    case "IN":
                        localnumber = 5;
                        break;
                    default:

                        localnumber = 0;
                        break;

                }
                Random random = new Random();
                single1 = (float) (random.Next(0, 10000)) / 100;
                single1 = this.AllowanceAdjustedWeeklyIncome();
                sngPayPeriodIncomeTax = this.CalculateStateTaxes(this.GetStateWHTblQuery(single1), single1);
              
            }
            blnSuccess = true;
        }

        private float WeeklyConvFactor()
        {
            float single1 = 0;
            string text1 = this.GetPayrollType();
            switch (text1)
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

            //			if (StringType.StrCmp(text1, "Daily", true) == 0)
            //			{
            //				return 7f;
            //			}
            //			if (StringType.StrCmp(text1, "Weekly", true) == 0)
            //			{
            //				return 1f;
            //			}
            //			if (StringType.StrCmp(text1, "Biweekly", true) == 0)
            //			{
            //				return 0.5f;
            //			}
            //			if (StringType.StrCmp(text1, "Semimonthly", true) == 0)
            //			{
            //				return 0.4615384f;
            //				//return 0.4602740f;
            //
            //			}
            //			if (StringType.StrCmp(text1, "Monthly", true) == 0)
            //			{
            //				single1 = 0.2307692f;
            //				//single1 = 0.2301370f;
            //			}
            return single1;
        }

		private DBHelper dbh;
		private OleDbConnection mcnnPayroll;
		//private OleDbConnection mcnnTaxTables;
		private int mintTaxTableYear;
		private float msngFedIncomeTax;
		private float msngGrossIncome;
		private float msngMedicare;
		private float msngMedicareYTD;
		private float msngNumOfAllow1;
		private float msngNumOfAllow2;
		private float msngSSTax;
		private float msngSSTaxYTD;
		private string mstrCountyCode;
		private string mstrMStatus;
		private string mstrPayrollType;
		private string mstrResidentStatusState;
		private string mstrStateAbbr;
		private float mWeeklyConvFactor;
        private Company company;
    }
}