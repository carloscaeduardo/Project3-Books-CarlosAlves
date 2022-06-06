using System.Web;
using System.Web.Mvc;

namespace Project3_Books_CarlosAlves
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
