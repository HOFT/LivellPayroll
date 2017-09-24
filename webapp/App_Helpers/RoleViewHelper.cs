using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace LivellPayRoll.App_Helpers
{
    public class RoleViewHelper
    {
        public static bool IsShowModule(string controllerName, string actionName)
        {
            string RoleId = SystemVariates.LoginRoleId;
            string roles = GetActionRoles(actionName, controllerName);
            if (!string.IsNullOrWhiteSpace(roles))
            {
                string[] AuthRoles = roles.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                if(!AuthRoles.Contains(RoleId)) {
                    return false;
                }
            }
            return true;
        }

        public static string GetActionRoles(string action, string controller)
        {
            XElement rootElement = XElement.Load(HttpContext.Current.Server.MapPath("~/App_Helpers/") + "ActionRoles.xml");
            XElement controllerElement = FindElementByAttribute(rootElement, "Controller", controller);
            if (controllerElement != null)
            {
                XElement actionElement = FindElementByAttribute(controllerElement, "Action", action);
                if (actionElement != null)
                {
                    return actionElement.Value.Trim();
                }
                return controllerElement.Value.Trim();
            }
            return "";
        }
        private static XElement FindElementByAttribute(XElement xElement, string tagName, string attribute)
        {
            return xElement.Elements(tagName).FirstOrDefault(x => x.Attribute("name").Value.Equals(attribute, StringComparison.OrdinalIgnoreCase));
        }
    }
}