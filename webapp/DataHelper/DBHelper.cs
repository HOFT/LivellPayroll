using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Configuration;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;
using System.Data.SqlClient;

namespace LivellPayroll
{
    public class DBHelper
    {
        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["PayRollCon"].ToString();
            }
        }
        public static DataTable getAllEntity(string strQuery)
        {
            DataTable tblData = new DataTable("DataTable1");

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();//打开数据库

                SqlDataAdapter adapter1 = new SqlDataAdapter(strQuery, conn); // 实例化适配器
                adapter1.Fill(tblData);


                conn.Close();//关闭数据库
            }

            return tblData;

        }

        public static void ExecNonQuery(string strQuery)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();//打开数据库

                SqlCommand command1 = new SqlCommand(strQuery, conn);
                command1.ExecuteNonQuery();

                conn.Close();//关闭数据库
            }

        }
        public static void UpdateAccountBalance(int accountId, decimal amt)
        {
            string strQuery = "update account set endingbalance = endingbalance + (" + amt + ") where Id = " + accountId;
            DBHelper.ExecNonQuery(strQuery);
        }
        public static decimal GetAccountSign(int intAccountId)
        {
            decimal rtn = 1;
            object o = GetSingleValue("select DisplayFactor from Account left join AccountType on Account.TypeId=AccountType.Id where Account.Id=" + intAccountId);
            rtn = Convert.ToDecimal(o);
            return rtn;
        }
        //public static int InsertEntity(string strQuery)
        //{
        //    int i = 0;
        //    string connString = ConfigurationManager.ConnectionStrings["LivellCompany"].ConnectionString;
        //    MySqlConnection connection1 = new MySqlConnection(connString);

        //    if (ConnectionTarget.Instance.IsAccessDB)
        //    {

        //        MySqlCommand command1 = new MySqlCommand(strQuery, connection1);
        //        command1.Connection.Open();
        //        command1.ExecuteNonQuery();
        //        command1.CommandText = "Select @@Identity";
        //        object o = command1.ExecuteScalar();
        //        i = Convert.ToInt32(o);
        //    }
        //    else
        //    {

        //        strQuery = strQuery + " Select Scope_Identity();";
        //        MySqlCommand command1 = new MySqlCommand(strQuery, connection1);
        //        command1.Connection.Open();
        //        object o = command1.ExecuteScalar();
        //        i = Convert.ToInt32(o);
        //    }


        //    connection1.Close();
        //    return i;

        //}
        public static bool CheckExist(string strQuery)
        {
            bool flag1 = false;
            DataTable table1 = getAllEntity(strQuery);

            if (table1.Rows.Count > 0)
            {
                flag1 = true;
            }
            return flag1;


        }
        public static object GetSingleValue(string strQuery)
        {
            object rtn = null;
            DataTable table1 = getAllEntity(strQuery);

            if (table1.Rows.Count > 0)
            {
                rtn = table1.Rows[0][0];
            }
            return rtn;
        }
        public static bool Update(string strQuery, DataTable dtTable)
        {
            bool bReturn = true;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();//打开数据库

                SqlDataAdapter adapter1 = new SqlDataAdapter(strQuery, conn); // 实例化适配器
                SqlCommandBuilder builder1 = new SqlCommandBuilder(adapter1);

                try
                {
                    adapter1.Update(dtTable);
                }
                catch (Exception exception2)
                {
                    System.Diagnostics.Debug.WriteLine(exception2.Message);
                    bReturn = false;
                }
                adapter1 = null;

                conn.Close();//关闭数据库
            }
            
            return bReturn;
        }
        internal static void GetTable(ref DataTable tblData, string strQuery)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();//打开数据库

                SqlDataAdapter adapter1 = new SqlDataAdapter(strQuery, conn); // 实例化适配器
                adapter1.Fill(tblData);
                try
                {
                    adapter1.Fill(tblData);
                }
                catch (Exception exception2)
                {
                    System.Diagnostics.Debug.Write(exception2.Message);
                }

                conn.Close();//关闭数据库
            }
        }
        internal static void GetTable(ref DataTable tblData, string strQuery, string strConn)
        {
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                conn.Open();//打开数据库

                SqlDataAdapter adapter1 = new SqlDataAdapter(strQuery, conn); // 实例化适配器
                adapter1.Fill(tblData);
                try
                {
                    adapter1.Fill(tblData);
                }
                catch (Exception exception2)
                {
                    System.Diagnostics.Debug.Write(exception2.Message);
                }

                conn.Close();//关闭数据库
            }
        }

        protected static internal void SaveDataTableToDB(ref DataTable tblSource, string strQuery)
        {
            if (tblSource != null)
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();//打开数据库

                    SqlDataAdapter adapter1 = new SqlDataAdapter(strQuery, conn); // 实例化适配器
                    SqlCommandBuilder builder1 = new SqlCommandBuilder(adapter1);

                    try
                    {
                        //For concurrency lock
                        DataTable t = null;

                        lock (tblSource)
                        {

                            t = tblSource.Copy();

                            tblSource.AcceptChanges();

                        }
                        //Application.DoEvents();
                        adapter1.Update(t);
                        //Application.DoEvents();
                    }
                    catch (Exception exception1)
                    {
                        System.Diagnostics.Debug.WriteLine("error" + exception1.Message);
                        //sendError a = new sendError(exception1.Message, "DBOperateion::104");
                        //a.ShowDialog();
                    }


                    conn.Close();//关闭数据库
                }
                
            }
        }
        public static object NumberCheck(object Expression)
        {
            double retNum;

            bool isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return retNum;
        }
        
        //protected static internal object NumberCheck(object varInput)
        //{
        //    if (Information.IsNumeric(RuntimeHelpers.GetObjectValue(varInput)))
        //    {
        //        return varInput;
        //    }
        //    return 0;
        //}
        protected static internal DataTable GetMaritalStatusTable()
        {
            DataTable table1 = new DataTable();
            object obj1 = new DataColumn();
            DataRow row1 = table1.NewRow();
            table1.Columns.Add("StatusAbbr", typeof(string));
            table1.Columns.Add("Status", typeof(string));
            row1["StatusAbbr"] = "S";
            row1["Status"] = "Single";
            table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["StatusAbbr"] = "M";
            row1["Status"] = "Married";
            table1.Rows.Add(row1);
            return table1;
        }
        protected internal static DataTable GetPayFreqTable()
        {
            DataTable table1 = new DataTable();
            object obj1 = new DataColumn();
            DataRow row1 = table1.NewRow();
            table1.Columns.Add("PayFreqDis", typeof(string));
            table1.Columns.Add("PayFreq", typeof(string));
            row1["PayFreqDis"] = "Weekly";
            row1["PayFreq"] = "Weekly";
            table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["PayFreqDis"] = "Biweekly";
            row1["PayFreq"] = "Biweekly";
            table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["PayFreqDis"] = "Daily";
            row1["PayFreq"] = "Daily";
            table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["PayFreqDis"] = "Semimonthly";
            row1["PayFreq"] = "Semimonthly";
            table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["PayFreqDis"] = "Monthly";
            row1["PayFreq"] = "Monthly";
            table1.Rows.Add(row1);
            return table1;
        }
        protected static internal DataTable GetPayTypeTable()
        {
            DataTable table1 = new DataTable();
            object obj1 = new DataColumn();
            DataRow row1 = table1.NewRow();
            table1.Columns.Add("Id", typeof(string));
            table1.Columns.Add("Type", typeof(string));
            row1["Id"] = 1;
            row1["Type"] = "Check";
            table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["Id"] = 2;
            row1["Type"] = "Direct Deposit";
            table1.Rows.Add(row1);
            return table1;
        }
        protected static internal DataTable GetMaritalStatusStatesTable_Letters(bool blnAtoE)
        {
            DataTable table1 = new DataTable();
            object obj1 = new DataColumn();
            DataRow row1 = table1.NewRow();
            table1.Columns.Add("StatusAbbr", typeof(string));
            table1.Columns.Add("Status", typeof(string));
            row1["StatusAbbr"] = "A";
            row1["Status"] = "A";
            table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["StatusAbbr"] = "B";
            row1["Status"] = "B";
            table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["StatusAbbr"] = "C";
            row1["Status"] = "C";
            table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["StatusAbbr"] = "D";
            row1["Status"] = "D";
            table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["StatusAbbr"] = "E";
            row1["Status"] = "E";
            table1.Rows.Add(row1);
            if (!blnAtoE)
            {
                row1 = row1.Table.NewRow();
                row1["StatusAbbr"] = "F";
                row1["Status"] = "F";
                table1.Rows.Add(row1);
            }
            return table1;
        }
        protected static internal DataTable GetMaritalStatusStatesTable(bool blnZeroExemptions, bool blnSingle, bool blnHeadOfHousehold, bool blnMarried1Income, bool blnMarried2Income, bool blnMarriedFileSeparate, bool blnMarriedFileJoint)
        {
            DataTable table1 = new DataTable();
            object obj1 = new DataColumn();
            DataRow row1 = table1.NewRow();
            table1.Columns.Add("StatusAbbr", typeof(string));
            table1.Columns.Add("Status", typeof(string));
            if (blnZeroExemptions)
            {
                row1 = row1.Table.NewRow();
                row1["StatusAbbr"] = "O";
                row1["Status"] = "Zero Exemptions";
                table1.Rows.Add(row1);
            }
            if (blnSingle)
            {
                row1 = row1.Table.NewRow();
                row1["StatusAbbr"] = "S";
                row1["Status"] = "Single";
                table1.Rows.Add(row1);
            }
            if (blnHeadOfHousehold)
            {
                row1 = row1.Table.NewRow();
                row1["StatusAbbr"] = "H";
                row1["Status"] = "Head of Houshold";
                table1.Rows.Add(row1);
            }
            if (blnMarried1Income)
            {
                row1 = row1.Table.NewRow();
                row1["StatusAbbr"] = "M";
                row1["Status"] = "Married";
                table1.Rows.Add(row1);
            }
            if (blnMarried2Income)
            {
                row1 = row1.Table.NewRow();
                row1["StatusAbbr"] = "B";
                row1["Status"] = "Married (Dual Incomes)";
                table1.Rows.Add(row1);
            }
            if (blnMarriedFileSeparate)
            {
                row1 = row1.Table.NewRow();
                row1["StatusAbbr"] = "M";
                row1["Status"] = "Married (Filing Seperate)";
                table1.Rows.Add(row1);
            }
            if (blnMarriedFileJoint)
            {
                row1 = row1.Table.NewRow();
                row1["StatusAbbr"] = "J";
                row1["Status"] = "Married (Filing Joint)";
                table1.Rows.Add(row1);
            }
            return table1;
        }
        protected static internal DataTable GetResidentStatusTable()
        {
            DataTable table1 = new DataTable();
            object obj1 = new DataColumn();
            DataRow row1 = table1.NewRow();
            table1.Columns.Add("StatusAbbr", typeof(string));
            table1.Columns.Add("Status", typeof(string));
            row1["StatusAbbr"] = "R";
            row1["Status"] = "Resident";
            table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["StatusAbbr"] = "N";
            row1["Status"] = "Non Resident";
            table1.Rows.Add(row1);
            return table1;
        }
        protected static internal DataTable GetCompanyTypeTable()
        {
            DataTable table1 = new DataTable();
            object obj1 = new DataColumn();
            DataRow row1 = table1.NewRow();
            table1.Columns.Add("Id", typeof(string));
            table1.Columns.Add("Type", typeof(string));
            row1["Id"] = "1";
            row1["Type"] = "Coporation";
            table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["Id"] = "2";
            row1["Type"] = "S Coporation";
            table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["Id"] = "3";
            row1["Type"] = "Partnership or LLP";
            table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["Id"] = "4";
            row1["Type"] = "Single Member LLC";
            table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["Id"] = "5";
            row1["Type"] = "Multi Member LLC";
            table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["Id"] = "6";
            row1["Type"] = "Sole Proprietorship";
            table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["Id"] = "7";
            row1["Type"] = "Non-Profit";
            table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["Id"] = "8";
            row1["Type"] = "Others/Empty";
            table1.Rows.Add(row1);
            return table1;
        }

        protected static internal DataTable GetCreditDebitTypeTable()
        {
            DataTable table1 = new DataTable();
            object obj1 = new DataColumn();
            
            table1.Columns.Add("Id", typeof(string));
            table1.Columns.Add("Type", typeof(string));
            DataRow row1 = table1.NewRow();
            row1["Id"] = "1";
            row1["Type"] = "Debit";
            table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["Id"] = "-1";
            row1["Type"] = "Credit";
            table1.Rows.Add(row1);
           
            return table1;
        }
        protected static internal DataTable GetCheckCreditCardTypeTable()
        {
            DataTable table1 = new DataTable();
            object obj1 = new DataColumn();

            table1.Columns.Add("Id", typeof(string));
            table1.Columns.Add("Type", typeof(string));
            DataRow row1 = table1.NewRow();
            row1["Id"] = "1";
            row1["Type"] = "Check";
            table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["Id"] = "2";
            row1["Type"] = "Credit Card";
            table1.Rows.Add(row1);

            row1 = row1.Table.NewRow();
            row1["Id"] = "3";
            row1["Type"] = "Cash";
            table1.Rows.Add(row1);

            return table1;
        }

        protected static internal DataTable GetCashFlowClassifictionTable()
        {
            DataTable table1 = new DataTable();
            object obj1 = new DataColumn();

            table1.Columns.Add("Id", typeof(string));
            table1.Columns.Add("Type", typeof(string));
            DataRow row1 = table1.NewRow();
            row1["Id"] = "1";
            row1["Type"] = "Operating";
            table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["Id"] = "2";
            row1["Type"] = "Investing";
            table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["Id"] = "3";
            row1["Type"] = "Financing";
            table1.Rows.Add(row1);
            return table1;
        }
        protected static internal DataTable GetTaxYearTable()
        {
            DataTable table1 = new DataTable();
            object obj1 = new DataColumn();
            DataRow row1 = table1.NewRow();
            table1.Columns.Add("Year", typeof(string));
            row1["Year"] = "2016";
            table1.Rows.Add(row1);
            //row1 = row1.Table.NewRow();
            //row1["Year"] = (DateTime.Now.Year - 1).ToString();
            //table1.Rows.Add(row1);
            return table1;
        }
        protected static internal DataTable GetMultiTaxYearTable()
        {
            DataTable table1 = new DataTable();
            object obj1 = new DataColumn();
            DataRow row1 = table1.NewRow();
            table1.Columns.Add("Year", typeof(string));
            row1["Year"] = "2016";
            table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["Year"] = "2015";
            table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["Year"] = "2014";
            table1.Rows.Add(row1);
            return table1;
        }
        protected static internal DataTable GetTaxLastYearTable()
        {
            DataTable table1 = new DataTable();
            object obj1 = new DataColumn();
            DataRow row1 = table1.NewRow();
            table1.Columns.Add("Year", typeof(string));
            row1["Year"] = "2016";
            table1.Rows.Add(row1);

            return table1;
        }
        protected static internal DataTable GetTaxTwoYearTable()
        {
            DataTable table1 = new DataTable();
            object obj1 = new DataColumn();
            DataRow row1 = table1.NewRow();
            table1.Columns.Add("Year", typeof(string));
            row1["Year"] = "2015";
            table1.Rows.Add(row1);

            return table1;
        }
        protected static internal DataTable GetTaxQuarterTable()
        {
            DataTable table1 = new DataTable();
            object obj1 = new DataColumn();
            DataRow row1 = table1.NewRow();
            table1.Columns.Add("Quarter", typeof(string));
            row1["Quarter"] = "1";
            table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["Quarter"] = "2";
            table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["Quarter"] = "3";
            table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["Quarter"] = "4";
            table1.Rows.Add(row1);
            return table1;
        }
        protected static internal DataTable GetTaxMonthTable()
        {
            DataTable table1 = new DataTable();
            object obj1 = new DataColumn();
            DataRow row1 = table1.NewRow();
            table1.Columns.Add("Month", typeof(string));
            row1["Month"] = "1";
            table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["Month"] = "2";
            table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["Month"] = "3";
            table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["Month"] = "4";
            table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["Month"] = "5";
            table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["Month"] = "6";
            table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["Month"] = "7";
            table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["Month"] = "8";
            table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["Month"] = "9";
            table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["Month"] = "10";
            table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["Month"] = "11";
            table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["Month"] = "12";
            table1.Rows.Add(row1);
            return table1;
        }
        protected internal static string ConvertToBool(string str)
        {
            string rbool = "1";
            if(str.ToLower() == "false")
            {
                rbool = "0";
            }
            return rbool;
        }
        protected internal static void SaveDataTableToDB(ref DataTable tblSource, string strQuery, string strcnn)
        {
            if (tblSource != null)
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();//打开数据库

                    SqlDataAdapter adapter1 = new SqlDataAdapter(strQuery, conn); // 实例化适配器
                    SqlCommandBuilder builder1 = new SqlCommandBuilder(adapter1);

                    try
                    {
                        adapter1.Update(tblSource);
                    }
                    catch (Exception exception1)
                    {
                        System.Diagnostics.Debug.WriteLine("error" + exception1.Message);
                        //					ProjectData.SetProjectError(exception1);
                        //					Interaction.MsgBox(Information.Err().Description, MsgBoxStyle.OKOnly, null);
                        //					ProjectData.ClearProjectError();

                        //System.Diagnostics.Debug.WriteLine (exception1.Message);
                    }

                    conn.Close();//关闭数据库
                }
                

            }
        }

    }
}
