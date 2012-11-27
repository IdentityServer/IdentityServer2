using System.Collections.Generic;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels
{
    public class ChildMenuViewModel
    {
        public IEnumerable<ChildMenuItem> Items { get; set; }
    }

    public class ChildMenuItem
    {
        public string Title { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public object RouteValues { get; set; }
    }
}