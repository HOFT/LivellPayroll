using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;

using System.Data;
using Microsoft.Win32;
using System.Xml;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
//using System.Reflection;

using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System.Drawing.Printing;
using System.Globalization;
using System.Security.Cryptography;
//using PdfSharp.Pdf;
//using PdfSharp.Drawing;

namespace LivellPayroll
{
    public class Function
    {
       
        protected internal static int a(string s)
        {
           
         
       
            
            string b = s.Trim().Replace(" ", "").Replace("-", "");
            if ((b.Length != 59) && (b.Length != 16))
            {
                return 0; //not valid key
            }
            int int1 = GetSafeInt(b.Substring(9, 1));
            int int2 = GetSafeInt(b.Substring(10, 1));
            if ((int1 + int2) != 14)
            {
                return 0; //not valid key
            }
            int int7 = GetSafeInt(b.Substring(0, 1));
            int int8 = GetSafeInt(b.Substring(1, 1));
            if ((int7 + int8) != 9)
            {
                return 0; //not valid key
            }

            try
            {
                int sum = 0;
                for (int i = 0; i < 16; i++)
                {
                    int d = Convert.ToInt32(b.Substring(i, 1));
                    sum = sum + d;

                }
                if (sum != (sum / 10) * 10) //check check sum
                {
                    return 0; //not valid key
                }
            }
            catch
            {
                return 0; //not valid key
            }

            int int3 = GetSafeInt(b.Substring(5, 1));
            int int4 = GetSafeInt(b.Substring(6, 1));
            int int5 = int3 + int4;
            if (int5 == 12)
            {
                return 1; //Single version
            }
            else if (int5 == 13)
            {
                return 2; //network version
            }
            else
            {
                return 0; //not valid key
            }
        }
        public static string ConvertDoubleToString(double dInput)
        {
            try
            {
                return string.Format("{0:F2}", dInput);
            }
            catch
            {
                return dInput.ToString();
            }
        }
      
        public static void FormateDblFmt(ref string strInput)
        {
            try
            {
                double num = Convert.ToDouble((string)strInput);
                strInput = string.Format("{0:F2}", num);
            }
            catch (Exception)
            {
                strInput = "0.00";
            }
        }
        public static bool IsNullOrEmpty(string value)
        {
            if (value != null)
            {
                return (value.Length == 0);
            }
            return true;
        }

        public static DateTime ConvertToDateTime(string strInput)
        {
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", false).DateTimeFormat;
            return Convert.ToDateTime(strInput, dateTimeFormat);
        }
        public static void UniformDoubleFormat(ref string strInput)
        {
            try
            {
                double num = Convert.ToDouble((string)strInput);
                strInput = string.Format("{0:F2}", num);
            }
            catch (Exception)
            {
                strInput = "0.00";
            }
        }

        public static string UniformDoubleFormat(string strInput)
        {
            try
            {
                double num = Convert.ToDouble((string)strInput);
                strInput = string.Format("{0:F2}", num);
            }
            catch (Exception)
            {
                strInput = "0.00";
            }
            return strInput;
        }
        public static double ConvertStringToDouble(string strInput)
        {
            try
            {
                return Convert.ToDouble(strInput);
            }
            catch
            {
                return 0.0;
            }
        }
        public static string QuoteSQLString(string str)
        {
            if (!string.IsNullOrEmpty(str))
                return str.Replace("'", "''");
            else
                return "";
        }
        public static string ConvertToHour(object min)
        {
            int totalmin = Convert.ToInt32(min);
            string hourstring = "";
            int hour = totalmin / 60;
            int minleft = totalmin - hour * 60;
            if (hour != 0)
            {
                hourstring = hour.ToString() + "h " + minleft.ToString() + "m";
            }
            else
            {
                hourstring = minleft.ToString() + "m";

            }
            return hourstring;
        }
        public static int GetSafeInt(object value)
        {
            int rtn = 0;
            if (value is DBNull)
            {
                return 0;
            }
            if (value == null)
            {
                return 0;
            }
            try
            {
                rtn = Convert.ToInt32(value);
            }
            catch
            {
            }
            return rtn;
        }
        public static DateTime GetSafeDateTime(object value)
        {
            DateTime rtn = DateTime.Now.ToUniversalTime();
            if (value is DBNull)
            {
                return DateTime.Now.ToUniversalTime();
            }
            if (value == null)
            {
                return DateTime.Now.ToUniversalTime();
            }
            try
            {
                rtn = Convert.ToDateTime(value);
            }
            catch
            {
            }
            return rtn;
        }

        public static decimal GetSafeDecimal(object value)
        {
            decimal rtn = 0;
            if (value is DBNull)
            {
                return 0;
            }
            if (value == null)
            {
                return 0;
            }
            try
            {
                rtn = Convert.ToDecimal(value);
            }
            catch
            {
            }
            return rtn;
        }
        public static string GetSafeString(object value)
        {
            string rtn = "";
            if (value is DBNull)
            {
                return "";
            }
            if (value == null)
            {
                return "";
            }
            try
            {
                rtn = Convert.ToString(value);
            }
            catch
            {
            }
            return rtn;
        }

        public static bool GetSafeBool(object value)
        {
            bool rtn = false;
            if (value is DBNull)
            {
                return false;
            }
            if (value == null)
            {
                return false;
            }
            try
            {
                rtn = Convert.ToBoolean(value);
            }
            catch
            {
            }
            return rtn;
        }
        public static float GetSafeSingle(object value)
        {
            float rtn = 0;
            if (value is DBNull)
            {
                return 0;
            }
            if (value == null)
            {
                return 0;
            }
            try
            {
                rtn = Convert.ToSingle(value);
            }
            catch
            {
            }
            return rtn;
        }
        public static double GetSafeDouble(object value)
        {
            double rtn = 0;
            if (value is DBNull)
            {
                return 0;
            }
            if (value == null)
            {
                return 0;
            }
            try
            {
                rtn = Convert.ToDouble(value);
            }
            catch
            {
            }
            return rtn;
        }
     
        public static int GetThisMonthQuarter(int nMonth)
        {
            if (nMonth < 4)
            {
                return 1;
            }
            if (nMonth < 7)
            {
                return 2;
            }
            if (nMonth < 10)
            {
                return 3;
            }
            return 4;
        }
        protected internal static DataTable GetCCTable()
        {
            DataTable table1 = new DataTable();
           
            table1.Columns.Add("Id", typeof(int));
            table1.Columns.Add("CC", typeof(string));
            DataRow row1 = table1.NewRow();
            row1["Id"] = 1;
            row1["CC"] = "American Express";
            table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["Id"] = 2;
            row1["CC"] = "Discover";
            table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["Id"] = 3;
            row1["CC"] = "Master";
            table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["Id"] = 4;
            row1["CC"] = "Visa";
            table1.Rows.Add(row1);
           
            row1 = row1.Table.NewRow();
            row1["Id"] = 5;
            row1["CC"] = "Dinners Club";
            table1.Rows.Add(row1);
          
         
            return table1;
        }
        protected internal static DataTable GetPosTable()
        {
            DataTable table1 = new DataTable();
            object obj1 = new DataColumn();
            DataRow row1 = table1.NewRow();
            table1.Columns.Add("Pos", typeof(string));
            row1["Pos"] = "Top";
            table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["Pos"] = "Middle";
            table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["Pos"] = "Bottom";
            table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["Pos"] = "3-per-Page";
            table1.Rows.Add(row1);
            return table1;
        }
        protected internal static DataTable GetFormNameTable()
        {
            DataTable table1 = new DataTable();

            table1.Columns.Add("Id", typeof(int));
            table1.Columns.Add("FormName", typeof(string));
            DataRow row1 = table1.NewRow();
            row1["Id"] = 1;
            row1["FormName"] = "Estimate";
            table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["Id"] = 2;
            row1["FormName"] = "Sales Order";
            table1.Rows.Add(row1);
            //row1 = row1.Table.NewRow();
            //row1["Id"] = 3;
            //row1["FormName"] = "Purchase Order";
            //table1.Rows.Add(row1);
            row1 = row1.Table.NewRow();
            row1["Id"] = 4;
            row1["FormName"] = "Invoice";
            table1.Rows.Add(row1);

            row1 = row1.Table.NewRow();
            row1["Id"] = 5;
            row1["FormName"] = "Receipt";
            table1.Rows.Add(row1);

            row1 = row1.Table.NewRow();
            row1["Id"] = 6;
            row1["FormName"] = "Packing Slip";
            table1.Rows.Add(row1);

            row1 = row1.Table.NewRow();
            row1["Id"] = 7;
            row1["FormName"] = "Credit/Refund";
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
        public static string LimitStringCharactersNumber(string strInput, int nLimit)
        {
            string tstrInput = strInput.Trim();
            if (tstrInput.Length > nLimit)
            {
                tstrInput = tstrInput.Remove(nLimit, tstrInput.Length - nLimit);
            }
            return tstrInput;
        }
        public static string getTextAmtString(string cVal)
        {
            string text1 = "";
            string[] textArray1 = new string[10];
            string[] textArray2 = new string[10];
            string[] textArray3 = new string[8];
            try
            {
                string text4 = "";
                double num1 = DoubleType.FromString(Strings.FormatNumber(DoubleType.FromString(cVal), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault));
                textArray1[0] = "Zero";
                textArray1[1] = "One";
                textArray1[2] = "Two";
                textArray1[3] = "Three";
                textArray1[4] = "Four";
                textArray1[5] = "Five";
                textArray1[6] = "Six";
                textArray1[7] = "Seven";
                textArray1[8] = "Eight";
                textArray1[9] = "Nine";
                textArray2[0] = "Ten";
                textArray2[1] = "Eleven";
                textArray2[2] = "Twelve";
                textArray2[3] = "Thirteen";
                textArray2[4] = "Fourteen";
                textArray2[5] = "Fifteen";
                textArray2[6] = "Sixteen";
                textArray2[7] = "Seventeen";
                textArray2[8] = "Eighteen";
                textArray2[9] = "Nineteen";
                textArray3[0] = "Twenty";
                textArray3[1] = "Thirty";
                //textArray3[2] = "Fourty";
                textArray3[2] = "Forty";
                textArray3[3] = "Fifty";
                textArray3[4] = "Sixty";
                textArray3[5] = "Seventy";
                textArray3[6] = "Eighty";
                textArray3[7] = "Ninety";
                string text2 = "Hundred";
                string text7 = "Thousand";
                string text3 = "Million";

                if (num1 > 0)
                {
                    if (num1 < 10)
                    {
                        //this is a bug when dollar amount large than .5 April 21, 2008
                        //text4 = textArray1[(int) Math.Round(num1)] + " Dollars and ";
                        //text4 = textArray1[(int) Math.Floor(num1)] + " Dollars and ";
                        text4 = textArray1[(int)Math.Floor(num1)] + " and ";
                    }
                    else
                    {
                        string text5;
                        if ((num1 >= 10) & (num1 < 20))
                        {
                            text5 = Strings.Mid(StringType.FromDouble(num1), 2, 1);
                            //							text4 = textArray2[IntegerType.FromString(text5)] + " Dollars and ";
                            text4 = textArray2[IntegerType.FromString(text5)] + " and ";
                        }
                        else if ((num1 >= 20) & (num1 < 100))
                        {
                            text5 = Strings.Mid(StringType.FromDouble(num1), 1, 1);
                            text4 = textArray3[(int)Math.Round((double)(DoubleType.FromString(text5) - 2))];
                            text5 = Strings.Mid(StringType.FromDouble(num1), 2, 1);
                            if (DoubleType.FromString(text5) > 0)
                            {
                                text4 = text4 + "-" + textArray1[IntegerType.FromString(text5)];
                            }
                            //							text4 = text4 + " Dollars and ";
                            text4 = text4 + " and ";
                        }
                        else if ((num1 >= 100) & (num1 < 1000))
                        {
                            text5 = Strings.Mid(StringType.FromDouble(num1), 1, 1);
                            text4 = textArray1[IntegerType.FromString(text5)] + " " + text2 + " ";
                            text5 = Strings.Mid(StringType.FromDouble(num1), 2, 1);
                            if (DoubleType.FromString(text5) > 1)
                            {
                                text4 = text4 + textArray3[(int)Math.Round((double)(DoubleType.FromString(text5) - 2))];
                                text5 = Strings.Mid(StringType.FromDouble(num1), 3, 1);
                                if (DoubleType.FromString(text5) > 0)
                                {
                                    text4 = text4 + "-" + textArray1[IntegerType.FromString(text5)];
                                }
                                text4 = text4;
                            }
                            else if (DoubleType.FromString(text5) == 1)
                            {
                                text5 = Strings.Mid(StringType.FromDouble(num1), 2, 2);
                                text4 = text4 + textArray2[(int)Math.Round((double)(DoubleType.FromString(text5) - 10))];
                            }
                            else if (DoubleType.FromString(text5) == 0)
                            {
                                text5 = Strings.Mid(StringType.FromDouble(num1), 2, 2);
                                if (DoubleType.FromString(text5) == 0)
                                {
                                    text4 = text4;
                                }
                                else
                                {
                                    text4 = text4 + textArray1[IntegerType.FromString(text5)];
                                }
                            }
                            //							text4 = text4 + " Dollars and ";
                            text4 = text4 + " and ";
                        }
                        else if ((num1 >= 1000) & (num1 < 10000))
                        {
                            text5 = Strings.Mid(StringType.FromDouble(num1), 1, 1);
                            text4 = textArray1[IntegerType.FromString(text5)] + " " + text7 + " ";
                            text5 = Strings.Mid(StringType.FromDouble(num1), 2, 1);
                            if (DoubleType.FromString(text5) > 0)
                            {
                                text4 = string.Concat(new string[] { text4, textArray1[IntegerType.FromString(text5)], " ", text2, " " });
                            }
                            text5 = Strings.Mid(StringType.FromDouble(num1), 3, 1);
                            if (DoubleType.FromString(text5) > 1)
                            {
                                text4 = text4 + textArray3[(int)Math.Round((double)(DoubleType.FromString(text5) - 2))];
                                text5 = Strings.Mid(StringType.FromDouble(num1), 4, 1);
                                if (DoubleType.FromString(text5) > 0)
                                {
                                    text4 = text4 + "-" + textArray1[IntegerType.FromString(text5)];
                                }
                                text4 = text4;
                            }
                            else if (DoubleType.FromString(text5) == 1)
                            {
                                text5 = Strings.Mid(StringType.FromDouble(num1), 3, 2);
                                text4 = text4 + textArray2[(int)Math.Round((double)(DoubleType.FromString(text5) - 10))];
                            }
                            else if (DoubleType.FromString(text5) == 0)
                            {
                                text5 = Strings.Mid(StringType.FromDouble(num1), 3, 2);
                                if (DoubleType.FromString(text5) == 0)
                                {
                                    text4 = text4;
                                }
                                else
                                {
                                    text4 = text4 + textArray1[IntegerType.FromString(text5)];
                                }
                            }
                            //							text4 = text4 + " Dollars and ";
                            text4 = text4 + " and ";
                        }
                        else if ((num1 >= 10000) & (num1 < 20000))
                        {
                            text5 = Strings.Mid(StringType.FromDouble(num1), 1, 2);
                            text4 = textArray2[(int)Math.Round((double)(DoubleType.FromString(text5) - 10))] + " " + text7 + " ";
                            text5 = Strings.Mid(StringType.FromDouble(num1), 3, 1);
                            if (DoubleType.FromString(text5) > 0)
                            {
                                text4 = string.Concat(new string[] { text4, textArray1[IntegerType.FromString(text5)], " ", text2, " " });
                            }
                            text5 = Strings.Mid(StringType.FromDouble(num1), 4, 1);
                            if (DoubleType.FromString(text5) > 1)
                            {
                                text4 = text4 + textArray3[(int)Math.Round((double)(DoubleType.FromString(text5) - 2))];
                                text5 = Strings.Mid(StringType.FromDouble(num1), 5, 1);
                                if (DoubleType.FromString(text5) > 0)
                                {
                                    text4 = text4 + "-" + textArray1[IntegerType.FromString(text5)];
                                }
                                text4 = text4;
                            }
                            else if (DoubleType.FromString(text5) == 1)
                            {
                                text5 = Strings.Mid(StringType.FromDouble(num1), 4, 2);
                                text4 = text4 + textArray2[(int)Math.Round((double)(DoubleType.FromString(text5) - 10))];
                            }
                            else if (DoubleType.FromString(text5) == 0)
                            {
                                text5 = Strings.Mid(StringType.FromDouble(num1), 4, 2);
                                if (DoubleType.FromString(text5) == 0)
                                {
                                    text4 = text4;
                                }
                                else
                                {
                                    text4 = text4 + textArray1[IntegerType.FromString(text5)];
                                }
                            }
                            //							text4 = text4 + " Dollars and ";
                            text4 = text4 + " and ";
                        }
                        else if ((num1 >= 20000) & (num1 < 100000))
                        {
                            text5 = Strings.Mid(StringType.FromDouble(num1), 1, 1);
                            if (num1 >= 21000)
                            {
                                //Solve the problem of Fourty-zero thousands problem. Li July 29, 2008
                                //								text4 = textArray3[(int) Math.Round((double) (DoubleType.FromString(text5) - 2))] + "-";
                                //								text5 = Strings.Mid(StringType.FromDouble(num1), 2, 1);
                                //								text4 = string.Concat(new string[] { text4, textArray1[IntegerType.FromString(text5)], " ", text7, " " });
                                //New stuff
                                text4 = textArray3[(int)Math.Round((double)(DoubleType.FromString(text5) - 2))];
                                text5 = Strings.Mid(StringType.FromDouble(num1), 2, 1);
                                if (DoubleType.FromString(text5) > 0)
                                {
                                    text4 = string.Concat(new string[] { text4, "-", textArray1[IntegerType.FromString(text5)] });
                                }
                                text4 = text4 + " " + text7 + " ";
                                //End new stuff

                            }
                            else
                            {
                                text4 = textArray3[(int)Math.Round((double)(DoubleType.FromString(text5) - 2))] + " " + text7 + " ";
                            }
                            text5 = Strings.Mid(StringType.FromDouble(num1), 3, 1);
                            if (DoubleType.FromString(text5) > 0)
                            {
                                text4 = string.Concat(new string[] { text4, textArray1[IntegerType.FromString(text5)], " ", text2, " " });
                            }
                            text5 = Strings.Mid(StringType.FromDouble(num1), 4, 1);
                            if (DoubleType.FromString(text5) > 1)
                            {
                                text4 = text4 + textArray3[(int)Math.Round((double)(DoubleType.FromString(text5) - 2))];
                                text5 = Strings.Mid(StringType.FromDouble(num1), 5, 1);
                                if (DoubleType.FromString(text5) > 0)
                                {
                                    text4 = text4 + "-" + textArray1[IntegerType.FromString(text5)];
                                }
                                text4 = text4;
                            }
                            else if (DoubleType.FromString(text5) == 1)
                            {
                                text5 = Strings.Mid(StringType.FromDouble(num1), 4, 2);
                                text4 = text4 + textArray2[(int)Math.Round((double)(DoubleType.FromString(text5) - 10))];
                            }
                            else if (DoubleType.FromString(text5) == 0)
                            {
                                text5 = Strings.Mid(StringType.FromDouble(num1), 4, 2);
                                if (DoubleType.FromString(text5) == 0)
                                {
                                    text4 = text4;
                                }
                                else
                                {
                                    text4 = text4 + textArray1[IntegerType.FromString(text5)];
                                }
                            }
                            text4 = text4 + " and ";
                            //							text4 = text4 + " Dollars and ";
                        }
                        else if ((num1 >= 100000) & (num1 < 1000000))
                        {
                            text5 = Strings.Mid(StringType.FromDouble(num1), 1, 1);
                            text4 = textArray1[IntegerType.FromString(text5)] + " " + text2 + " ";
                            text5 = Strings.Mid(StringType.FromDouble(num1), 2, 1);
                            if (DoubleType.FromString(text5) > 1)
                            {
                                text4 = text4 + textArray3[(int)Math.Round((double)(DoubleType.FromString(text5) - 2))];
                                text5 = Strings.Mid(StringType.FromDouble(num1), 3, 1);
                                if (DoubleType.FromString(text5) > 0)
                                {
                                    text4 = text4 + "-" + textArray1[IntegerType.FromString(text5)];
                                }
                            }
                            else if (DoubleType.FromString(text5) == 1)
                            {
                                text5 = Strings.Mid(StringType.FromDouble(num1), 2, 2);
                                text4 = text4 + textArray2[(int)Math.Round((double)(DoubleType.FromString(text5) - 10))];
                            }
                            else if (DoubleType.FromString(text5) == 0)
                            {
                                text5 = Strings.Mid(StringType.FromDouble(num1), 2, 2);
                                if (DoubleType.FromString(text5) == 0)
                                {
                                    text4 = text4;
                                }
                                else
                                {
                                    text4 = text4 + textArray1[IntegerType.FromString(text5)];
                                }
                            }
                            text4 = text4 + " " + text7 + " ";
                            text5 = Strings.Mid(StringType.FromDouble(num1), 4, 1);
                            if (DoubleType.FromString(text5) > 0)
                            {
                                text4 = string.Concat(new string[] { text4, textArray1[IntegerType.FromString(text5)], " ", text2, " " });
                            }
                            text5 = Strings.Mid(StringType.FromDouble(num1), 5, 1);
                            if (DoubleType.FromString(text5) > 1)
                            {
                                text4 = text4 + textArray3[(int)Math.Round((double)(DoubleType.FromString(text5) - 2))];
                                text5 = Strings.Mid(StringType.FromDouble(num1), 6, 1);
                                if (DoubleType.FromString(text5) > 0)
                                {
                                    text4 = text4 + "-" + textArray1[IntegerType.FromString(text5)];
                                }
                                text4 = text4;
                            }
                            else if (DoubleType.FromString(text5) == 1)
                            {
                                text5 = Strings.Mid(StringType.FromDouble(num1), 5, 2);
                                text4 = text4 + textArray2[(int)Math.Round((double)(DoubleType.FromString(text5) - 10))];
                            }
                            else if (DoubleType.FromString(text5) == 0)
                            {
                                text5 = Strings.Mid(StringType.FromDouble(num1), 5, 2);
                                if (DoubleType.FromString(text5) == 0)
                                {
                                    text4 = text4;
                                }
                                else
                                {
                                    text4 = text4 + textArray1[IntegerType.FromString(text5)];
                                }
                            }
                            text4 = text4 + " and ";
                            //							text4 = text4 + " Dollars and ";
                        }
                        else if ((num1 >= 1000000) & (num1 < 10000000))
                        {
                            text5 = Strings.Mid(StringType.FromDouble(num1), 1, 1);
                            text4 = textArray1[IntegerType.FromString(text5)] + " " + text3 + " ";
                            text5 = Strings.Mid(StringType.FromDouble(num1), 2, 1);
                            if (DoubleType.FromString(text5) > 0)
                            {
                                text4 = string.Concat(new string[] { text4, textArray1[IntegerType.FromString(text5)], " ", text2, " " });
                            }
                            text5 = Strings.Mid(StringType.FromDouble(num1), 3, 1);
                            if (DoubleType.FromString(text5) > 1)
                            {
                                text4 = text4 + textArray3[(int)Math.Round((double)(DoubleType.FromString(text5) - 2))];
                                text5 = Strings.Mid(StringType.FromDouble(num1), 4, 1);
                                if (DoubleType.FromString(text5) > 0)
                                {
                                    text4 = text4 + "-" + textArray1[IntegerType.FromString(text5)];
                                }
                                text4 = text4;
                            }
                            else if (DoubleType.FromString(text5) == 1)
                            {
                                text5 = Strings.Mid(StringType.FromDouble(num1), 3, 2);
                                text4 = text4 + textArray2[(int)Math.Round((double)(DoubleType.FromString(text5) - 10))];
                            }
                            else if (DoubleType.FromString(text5) == 0)
                            {
                                text5 = Strings.Mid(StringType.FromDouble(num1), 3, 2);
                                if (DoubleType.FromString(text5) == 0)
                                {
                                    text4 = text4;
                                }
                                else
                                {
                                    text4 = text4 + textArray1[IntegerType.FromString(text5)];
                                }
                            }
                            text5 = Strings.Mid(StringType.FromDouble(num1), 2, 3);
                            if (DoubleType.FromString(text5) > 0)
                            {
                                text4 = text4 + " " + text7 + " ";
                            }
                            text5 = Strings.Mid(StringType.FromDouble(num1), 5, 1);
                            if (DoubleType.FromString(text5) > 0)
                            {
                                text4 = string.Concat(new string[] { text4, textArray1[IntegerType.FromString(text5)], " ", text2, " " });
                            }
                            text5 = Strings.Mid(StringType.FromDouble(num1), 6, 1);
                            if (DoubleType.FromString(text5) > 1)
                            {
                                text4 = text4 + textArray3[(int)Math.Round((double)(DoubleType.FromString(text5) - 2))];
                                text5 = Strings.Mid(StringType.FromDouble(num1), 7, 1);
                                if (DoubleType.FromString(text5) > 0)
                                {
                                    text4 = text4 + "-" + textArray1[IntegerType.FromString(text5)];
                                }
                                text4 = text4;
                            }
                            else if (DoubleType.FromString(text5) == 1)
                            {
                                text5 = Strings.Mid(StringType.FromDouble(num1), 6, 2);
                                text4 = text4 + textArray2[(int)Math.Round((double)(DoubleType.FromString(text5) - 10))];
                            }
                            else if (DoubleType.FromString(text5) == 0)
                            {
                                text5 = Strings.Mid(StringType.FromDouble(num1), 6, 2);
                                if (DoubleType.FromString(text5) == 0)
                                {
                                    text4 = text4;
                                }
                                else
                                {
                                    text4 = text4 + textArray1[IntegerType.FromString(text5)];
                                }
                            }
                            text4 = text4 + " and ";
                            //							text4 = text4 + " Dollars and ";
                        }
                        else if ((num1 >= 10000000) & (num1 < 20000000))
                        {
                            text5 = Strings.Mid(StringType.FromDouble(num1), 1, 1);
                            text4 = textArray2[(int)Math.Round((double)(DoubleType.FromString(text5) - 1))] + " " + text3 + " ";
                            text5 = Strings.Mid(StringType.FromDouble(num1), 3, 1);
                            if (DoubleType.FromString(text5) > 0)
                            {
                                text4 = string.Concat(new string[] { text4, textArray1[IntegerType.FromString(text5)], " ", text2, " " });
                            }
                            text5 = Strings.Mid(StringType.FromDouble(num1), 4, 1);
                            if (DoubleType.FromString(text5) > 1)
                            {
                                text4 = text4 + textArray3[(int)Math.Round((double)(DoubleType.FromString(text5) - 2))];
                                text5 = Strings.Mid(StringType.FromDouble(num1), 5, 1);
                                if (DoubleType.FromString(text5) > 0)
                                {
                                    text4 = text4 + "-" + textArray1[IntegerType.FromString(text5)];
                                }
                                text4 = text4;
                            }
                            else if (DoubleType.FromString(text5) == 1)
                            {
                                text5 = Strings.Mid(StringType.FromDouble(num1), 4, 2);
                                text4 = text4 + textArray2[(int)Math.Round((double)(DoubleType.FromString(text5) - 10))];
                            }
                            else if (DoubleType.FromString(text5) == 0)
                            {
                                text5 = Strings.Mid(StringType.FromDouble(num1), 4, 2);
                                if (DoubleType.FromString(text5) == 0)
                                {
                                    text4 = text4;
                                }
                                else
                                {
                                    text4 = text4 + textArray1[IntegerType.FromString(text5)];
                                }
                            }
                            text5 = Strings.Mid(StringType.FromDouble(num1), 3, 3);
                            if (DoubleType.FromString(text5) > 0)
                            {
                                text4 = text4 + " " + text7 + " ";
                            }
                            text5 = Strings.Mid(StringType.FromDouble(num1), 6, 1);
                            if (DoubleType.FromString(text5) > 0)
                            {
                                text4 = string.Concat(new string[] { text4, textArray1[IntegerType.FromString(text5)], " ", text2, " " });
                            }
                            text5 = Strings.Mid(StringType.FromDouble(num1), 7, 1);
                            if (DoubleType.FromString(text5) > 1)
                            {
                                text4 = text4 + textArray3[(int)Math.Round((double)(DoubleType.FromString(text5) - 2))];
                                text5 = Strings.Mid(StringType.FromDouble(num1), 8, 1);
                                if (DoubleType.FromString(text5) > 0)
                                {
                                    text4 = text4 + "-" + textArray1[IntegerType.FromString(text5)];
                                }
                                text4 = text4;
                            }
                            else if (DoubleType.FromString(text5) == 1)
                            {
                                text5 = Strings.Mid(StringType.FromDouble(num1), 7, 2);
                                text4 = text4 + textArray2[(int)Math.Round((double)(DoubleType.FromString(text5) - 10))];
                            }
                            else if (DoubleType.FromString(text5) == 0)
                            {
                                text5 = Strings.Mid(StringType.FromDouble(num1), 7, 2);
                                if (DoubleType.FromString(text5) == 0)
                                {
                                    text4 = text4;
                                }
                                else
                                {
                                    text4 = text4 + textArray1[IntegerType.FromString(text5)];
                                }
                            }
                            //text4 = text4 + " Dollars and ";
                            text4 = text4 + " and ";
                        }
                        else if ((num1 >= 20000000) & (num1 < 100000000))
                        {
                            text5 = Strings.Mid(StringType.FromDouble(num1), 1, 1);
                            text4 = textArray3[(int)Math.Round((double)(DoubleType.FromString(text5) - 2))];
                            text5 = Strings.Mid(StringType.FromDouble(num1), 2, 1);
                            if (DoubleType.FromString(text5) > 0)
                            {
                                text4 = text4 + "-" + textArray1[IntegerType.FromString(text5)];
                            }
                            text4 = text4 + " " + text3 + " ";
                            text5 = Strings.Mid(StringType.FromDouble(num1), 3, 1);
                            if (DoubleType.FromString(text5) > 0)
                            {
                                text4 = string.Concat(new string[] { text4, textArray1[IntegerType.FromString(text5)], " ", text2, " " });
                            }
                            text5 = Strings.Mid(StringType.FromDouble(num1), 4, 1);
                            if (DoubleType.FromString(text5) > 1)
                            {
                                text4 = text4 + textArray3[(int)Math.Round((double)(DoubleType.FromString(text5) - 2))];
                                text5 = Strings.Mid(StringType.FromDouble(num1), 5, 1);
                                if (DoubleType.FromString(text5) > 0)
                                {
                                    text4 = text4 + "-" + textArray1[IntegerType.FromString(text5)];
                                }
                                text4 = text4;
                            }
                            else if (DoubleType.FromString(text5) == 1)
                            {
                                text5 = Strings.Mid(StringType.FromDouble(num1), 4, 2);
                                text4 = text4 + textArray2[(int)Math.Round((double)(DoubleType.FromString(text5) - 10))];
                            }
                            else if (DoubleType.FromString(text5) == 0)
                            {
                                text5 = Strings.Mid(StringType.FromDouble(num1), 4, 2);
                                if (DoubleType.FromString(text5) == 0)
                                {
                                    text4 = text4;
                                }
                                else
                                {
                                    text4 = text4 + textArray1[IntegerType.FromString(text5)];
                                }
                            }
                            text5 = Strings.Mid(StringType.FromDouble(num1), 3, 3);
                            if (DoubleType.FromString(text5) > 0)
                            {
                                text4 = text4 + " " + text7 + " ";
                            }
                            text5 = Strings.Mid(StringType.FromDouble(num1), 6, 1);
                            if (DoubleType.FromString(text5) > 0)
                            {
                                text4 = string.Concat(new string[] { text4, textArray1[IntegerType.FromString(text5)], " ", text2, " " });
                            }
                            text5 = Strings.Mid(StringType.FromDouble(num1), 7, 1);
                            if (DoubleType.FromString(text5) > 1)
                            {
                                text4 = text4 + textArray3[(int)Math.Round((double)(DoubleType.FromString(text5) - 2))];
                                text5 = Strings.Mid(StringType.FromDouble(num1), 8, 1);
                                if (DoubleType.FromString(text5) > 0)
                                {
                                    text4 = text4 + "-" + textArray1[IntegerType.FromString(text5)];
                                }
                                text4 = text4;
                            }
                            else if (DoubleType.FromString(text5) == 1)
                            {
                                text5 = Strings.Mid(StringType.FromDouble(num1), 7, 2);
                                text4 = text4 + textArray2[(int)Math.Round((double)(DoubleType.FromString(text5) - 10))];
                            }
                            else if (DoubleType.FromString(text5) == 0)
                            {
                                text5 = Strings.Mid(StringType.FromDouble(num1), 7, 2);
                                if (DoubleType.FromString(text5) == 0)
                                {
                                    text4 = text4;
                                }
                                else
                                {
                                    text4 = text4 + textArray1[IntegerType.FromString(text5)];
                                }
                            }
                            //							text4 = text4 + " Dollars and ";
                            text4 = text4 + " and ";
                        }
                    }
                }
                text4 = text4 + Strings.Right(Strings.FormatNumber(num1, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault), 2) + "/100";
                text1 = text4;
            }
            catch (Exception exception2)
            {
            }
            return text1;
        }

        public static string EncryptString(string InputText, string Password)
        {
            RijndaelManaged RijndaelCipher = new RijndaelManaged();

            byte[] PlainText = System.Text.Encoding.Unicode.GetBytes(InputText);
            byte[] Salt = Encoding.ASCII.GetBytes(Password.Length.ToString());

            //This class uses an extension of the PBKDF1 algorithm defined in the PKCS#5 v2.0 
            //standard to derive bytes suitable for use as key material from a password. 
            //The standard is documented in IETF RRC 2898.

            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);
            //Creates a symmetric encryptor object. 
            ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));
            MemoryStream memoryStream = new MemoryStream();
            //Defines a stream that links data streams to cryptographic transformations
            CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(PlainText, 0, PlainText.Length);
            //Writes the final state and clears the buffer
            cryptoStream.FlushFinalBlock();
            byte[] CipherBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            string EncryptedData = Convert.ToBase64String(CipherBytes);
            return EncryptedData;

        }
        public static string DecryptString(string InputText, string Password)
        {

            RijndaelManaged RijndaelCipher = new RijndaelManaged();
            byte[] EncryptedData = Convert.FromBase64String(InputText);
            byte[] Salt = Encoding.ASCII.GetBytes(Password.Length.ToString());
            //Making of the key for decryption
            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);
            //Creates a symmetric Rijndael decryptor object.
            ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));
            MemoryStream memoryStream = new MemoryStream(EncryptedData);
            //Defines the cryptographics stream for decryption.THe stream contains decrpted data
            CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);
            byte[] PlainText = new byte[EncryptedData.Length];
            int DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);
            memoryStream.Close();
            cryptoStream.Close();
            //Converting to string
            string DecryptedData = Encoding.Unicode.GetString(PlainText, 0, DecryptedCount);
            return DecryptedData;

        }
        public static string LimitString(string str, int len)
        {
            if (str.Length > len)
            {
                return str.Substring(0, len);
            }
            return str;
        }
        public static string getPWD()
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            //byte[] d = encoding.GetBytes("%^&567tyu");
            //string s = Convert.ToBase64String(d);
            //"JV4mNTY3dHl1"

            string strBase64 = "JV4";
            strBase64 += "mNT";
            strBase64 += "Y3d";
            strBase64 += "Hl1";
            byte[] b = Convert.FromBase64String(strBase64);
            string c = encoding.GetString(b);
            return c;

        }
        //public static int GetCurrentCheckNo(int accountid)
        //{
        //    int currentCheckNo = 1000;
        //    try
        //    {
                
        //        object checkno = DBHelper.GetSingleValue("select CurrentCheckNo from Account where Id=" + accountid);
        //        if (checkno != null)
        //        {
        //            currentCheckNo = Convert.ToInt32(checkno);
                
        //        }


        //    }
        //    catch (Exception exception2)
        //    {
        //    }
        //    return currentCheckNo;
        //}
  //      public static void UpdateCheckNo(int accountid)
		//{

  //          try
  //          {
  //              int currentCheckNo = GetCurrentCheckNo(accountid);
  //              currentCheckNo++;

  //              string text1 = "update Account set CurrentCheckNo =" + currentCheckNo + " where Id=" + accountid;
  //              DBHelper.ExecNonQuery(text1);
  //          }
  //          catch (Exception exception2)
  //          {
  //          }

		//}
       
        internal static string[] MultiLineStringCharactersNumber(ref string strInput, int nLimit, int maxLine)
        {
            string[] al = new string[maxLine+1];

            string tmp = strInput.Trim().Replace("\r\n", "\r\n ");
            //string tmp = strInput.Trim();
            string[] stringArr = tmp.Split(new Char[] { ' ' });
            int intCt = 0;
            string strLine = "";
            int lineCt = 0;
            for (int i = 0; i < stringArr.Length; i++)
            {
                if (stringArr[i].IndexOf("\r\n") >= 0)
                {
                    intCt = intCt + stringArr[i].Replace("\r\n", "").Length + 1;
                    strLine = strLine + stringArr[i].Replace("\r\n", "") + " ";

                    al[lineCt] = strLine;
                    lineCt++;
                    if (lineCt >= maxLine)
                        break;
                    intCt = 0;
                    strLine = "";

                }
                else
                {
                    intCt = intCt + stringArr[i].Length + 1;
                    if (intCt <= nLimit)
                    {
                        strLine = strLine + stringArr[i] + " ";
                        al[lineCt] = strLine;
                    }
                    else
                    {
                        al[lineCt] = strLine;
                        lineCt++;
                        if (lineCt >= maxLine)
                            break;
                        intCt = stringArr[i].Length + 1;
                        strLine = stringArr[i] + " ";
                    }
                }

            }
            string[] al2 = new string[lineCt+1];
            strInput = "";
            for (int i = 0; i <= lineCt; i++)
            {
                strInput = strInput + al[i] + "\r\n";
                al2[i] = al[i];
            }
            return al2;
        }

    }

    public struct Address
    {
        public int intAddressTypeId;
        public string AddressType;
        public string Address1;
        public string Address2;
        public string Address3;
        public string City;
        public string State;
        public string Zip;
        public string Country;
        public string Note;
        public bool DefaultShipping;
    };

    public struct CheckDetails
    {
        public DateTime CheckDate;
        public decimal CheckAmount;
        public string PayeeName;
        public string Address1;
        public string Address2;
        public string Address3;
        public string Memo;
        public int CheckNo;
        public string City;
        public string State;
        public string Zip;
        public string Source;
        public int SourceId;
        public string Note1;
        public string Note2;
        public bool isDirectDeposit;
      
    };

  

    public class AddValue
    {
        private string m_Display;
        private char m_Value;
        public AddValue(string Display, char Value)
        {
            m_Display = Display;
            m_Value = Value;
        }
        public string Display
        {
            get { return m_Display; }
        }
        public char Value
        {
            get { return m_Value; }
        }
    }

    public class DBExport
    {
        public static void ExportDataTableToFile(DataTable DataTable, string FilePath, string DelimeterCharacter, string QualifierCharacter)
        {
            StreamWriter writer = File.CreateText(FilePath);
            foreach (DataColumn column in DataTable.Columns)
            {
                writer.Write(QualifierCharacter);
                writer.Write(column.Caption);
                writer.Write(QualifierCharacter);
                writer.Write(DelimeterCharacter);
            }
            writer.Write("\r\n");
            foreach (DataRow row in DataTable.Rows)
            {
                foreach (DataColumn column2 in row.Table.Columns)
                {
                    writer.Write(QualifierCharacter);
                    writer.Write(row[column2]);
                    writer.Write(QualifierCharacter);
                    if (DataTable.Columns.IndexOf(column2) < DataTable.Columns.Count)
                    {
                        writer.Write(DelimeterCharacter);
                    }
                }
                writer.Write("\r\n");
            }
            writer.Close();
        }

        public static void ExportDataTableToFile(DataTable DataTable, string FilePath, string DelimeterCharacter, string[] Columns, string QualifierCharacter)
        {
            StreamWriter writer = File.CreateText(FilePath);
            int num = 0;
            foreach (string str in Columns)
            {
                writer.Write(QualifierCharacter);
                writer.Write(DataTable.Columns[str].Caption);
                writer.Write(QualifierCharacter);
                if (num < (Columns.Length - 1))
                {

                    writer.Write(DelimeterCharacter);

                }
                num++;
            }
            writer.Write("\r\n");
            foreach (DataRow row in DataTable.Rows)
            {
                num = 0;
                foreach (string str2 in Columns)
                {
                    writer.Write(QualifierCharacter);
                    string tmp = Convert.ToString(row[str2]);
                    writer.Write(tmp.Replace(',', ' '));
                    writer.Write(QualifierCharacter);
                    if (num < (Columns.Length - 1))
                    {
                        writer.Write(DelimeterCharacter);
                    }
                    num++;
                }
                writer.Write("\r\n");
            }
            writer.Close();
        }
    }

}