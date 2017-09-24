using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace LivellPayRoll.App_Helpers
{
    public class PageHelper
    {
        /// <summary>  
        /// DataTable分页  
        /// </summary>  
        /// <param name="dt">DataTable</param>  
        /// <param name="PageIndex">页索引,注意：从1开始</param>  
        /// <param name="PageSize">每页大小</param>  
        /// <returns>分好页的DataTable数据</returns>              第1页        每页10条  
        public static DataTable GetPagedTable(DataTable dt, int PageIndex, int PageSize)
        {
            if (PageIndex == 0) { return dt; }
            DataTable newdt = dt.Copy();
            newdt.Clear();
            int rowbegin = (PageIndex - 1) * PageSize;
            int rowend = PageIndex * PageSize;

            if (rowbegin >= dt.Rows.Count)
            { return newdt; }

            if (rowend > dt.Rows.Count)
            { rowend = dt.Rows.Count; }
            for (int i = rowbegin; i <= rowend - 1; i++)
            {
                DataRow newdr = newdt.NewRow();
                DataRow dr = dt.Rows[i];
                foreach (DataColumn column in dt.Columns)
                {
                    newdr[column.ColumnName] = dr[column.ColumnName];
                }
                newdt.Rows.Add(newdr);
            }
            return newdt;
        }

        /// <summary>  
        /// 返回分页的页数  
        /// </summary>  
        /// <param name="count">总条数</param>  
        /// <param name="pageye">每页显示多少条</param>  
        /// <returns>如果 结尾为0：则返回1</returns>  
        public static int PageCount(int count, int pageye)
        {
            int page = 0;
            int sesepage = pageye;
            if (count % sesepage == 0) { page = count / sesepage; }
            else { page = (count / sesepage) + 1; }
            if (page == 0) { page += 1; }
            return page;
        }
    }
}