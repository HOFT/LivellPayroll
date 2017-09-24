using LivellPayRoll.App_Helpers;
using LivellPayRoll.Enum;
using LivellPayRoll.Infrastructure;
using LivellPayRoll.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LivellPayRoll.Controllers
{
    [Authorize]
    [CustomAuthorize]
    public class HelpController : Controller
    {
        Dictionary<string, string> HelpTypeDic = EnumHelper.GetEnumItemValueDesc(typeof(HelpType));
        // GET: Help
        public ActionResult Collect()
        {
            int Index = (Request["PageIndex"]==null)?1: int.Parse(Request["PageIndex"]);
            string Type = (Request["Type"] == null) ? "Everything" : Request["Type"];
            string SearchDesc = Request["Search"];

            string url = "http://" + Request.Url.Authority.ToString();
            int PageSize = 10;     //每页显示行数  
            int nMax = 0;         //总记录数  
            int pageCount = 0;    //页数＝总记录数/每页显示行数  
            int PageIndex = Index;

            var helpList = db.HelpDesc.Where(t => true).OrderBy(t => t.Type).ToList();
            if (Type != null && Type != "Everything") {

                string indexType = "0";
                foreach (KeyValuePair<string, string> kvp in HelpTypeDic)
                {
                    if (kvp.Value.Equals(Type))
                    {
                        indexType = kvp.Key;  
                    }
                }

                helpList = helpList.Where(t=>t.Type == indexType).OrderBy(t => t.Type).ToList();
            };
            if (SearchDesc != null) {
                helpList = helpList.Where(t => t.Content.Contains(SearchDesc)||t.Keyword.Contains(SearchDesc)||t.Title.Contains(SearchDesc)).OrderBy(t => t.Type).ToList();
            };
            var PageList = helpList.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
            nMax = helpList.Count();
            pageCount = (int)Math.Ceiling((double)nMax / PageSize);
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Id");
            dataTable.Columns.Add("Title");
            dataTable.Columns.Add("Type");
            dataTable.Columns.Add("Keyword");
            dataTable.Columns.Add("Path");
            dataTable.Columns.Add("Content");
            foreach (var r in PageList)
            {
                DataRow dr = dataTable.NewRow();
                dr["Id"] = r.Id;
                dr["Title"] = r.Title;
                dr["Type"] = HelpTypeDic[r.Type];

                string KeywordStr = "";
                if (r.Keyword != null) {
                    string[] KeywordArray = r.Keyword.Split(',');   //字符串转数组
                    for (int i = 0; i < KeywordArray.Length; i++)
                    {
                        KeywordStr = KeywordStr + "<span class='badge badge-info search-content'>" + KeywordArray[i] + "</span>  ";
                    }
                };
                dr["Keyword"] = KeywordStr;
                dr["Path"] = url + "/" + r.Path;
                dr["Content"] = r.Content;
                dataTable.Rows.Add(dr);
            };

            ViewBag.Role = SystemVariates.LoginRoleId;
            ViewBag.nMax = nMax;
            ViewBag.PageIndex = PageIndex;
            ViewBag.Type = Type;
            ViewBag.PageCount = pageCount;
            ViewBag.HelpType = new SelectList(HelpTypeDic, "key", "value");
            return View(dataTable);
        }
        [Authorize(Roles = "Governor")]
        public ActionResult SaveHelp(HelpDesc HD, string ReturnUrl) {
            ReturnUrl = Request.UrlReferrer.ToString();

            HelpDesc h = new HelpDesc
            {
                Id = Guid.NewGuid(),
                Title = HD.Title,
                Type = HD.Type,
                Path = HelpTypeDic[HD.Type] + "/" + HD.Path,
                Keyword = HD.Keyword,
                Content = HD.Content
            };
            db.HelpDesc.Add(h);
            db.SaveChanges();
            return Redirect(ReturnUrl);
        }
        [Authorize(Roles = "Governor")]
        public ActionResult DeleteHelp(Guid Id) {
            HelpDesc h = db.HelpDesc.Find(Id);
            db.Entry<HelpDesc>(h).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Collect");
        }
        private AppIdentityDbContext db
        {
            get
            {
                return HttpContext.GetOwinContext().Get<AppIdentityDbContext>();
            }
        }
    }
}