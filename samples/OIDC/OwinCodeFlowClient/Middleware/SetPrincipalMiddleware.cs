//using Microsoft.Owin;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Web;

//namespace OwinDemo
//{
//    public class SetPrincipalMiddleware : OwinMiddleware
//    {
//        public SetPrincipalMiddleware(OwinMiddleware next) : base(next)
//        { }

//        public override async Task Invoke(OwinRequest request, OwinResponse response)
//        {
//            if (request.User != null && request.User.Identity.IsAuthenticated)
//            {
//                Thread.CurrentPrincipal = request.User;

//                if (HttpContext.Current != null)
//                {
//                    HttpContext.Current.User = request.User;
//                }
//            }

//            await Next.Invoke(request, response);
//        }

//        public override Task Invoke(IOwinContext context)
//        {
//            throw new System.NotImplementedException();
//        }
//    }
//}