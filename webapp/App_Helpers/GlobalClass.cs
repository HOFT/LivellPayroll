using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace LivellPayRoll.App_Helpers
{
    public class GlobalClass
    {
        public static DataSet gTaxTable;
        private static string gstrDate;
        public static DataSet TaxTable
        {
            get
            {
                if (gTaxTable == null)
                {
                    gTaxTable = new DataSet();
                    gTaxTable.ReadXml(HostingEnvironment.MapPath("/") + "/App_Data/DataTableXML.xml");
                }


                return gTaxTable;

            }
            set
            {
                gTaxTable = value;

            }

        }
    }
}