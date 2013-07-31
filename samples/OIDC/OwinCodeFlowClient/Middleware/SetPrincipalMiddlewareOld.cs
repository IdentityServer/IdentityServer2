//using System;
//using System.Collections.Generic;
//using System.Security.Principal;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Web;

//namespace OwinDemo
//{
//    public class SetPrincipalMiddleware
//    {
//        private readonly Func<IDictionary<string, object>, Task> _next;

//        public SetPrincipalMiddleware(Func<IDictionary<string, object>, Task> next)
//        {
//            _next = next;
//        }

//        public async Task Invoke(IDictionary<string, object> env)
//        {
//            var principal = env["server.User"] as IPrincipal;
//            if (principal != null)
//            {
//                Thread.CurrentPrincipal = principal;

//                if (HttpContext.Current != null)
//                {
//                    HttpContext.Current.User = principal;
//                }
//            }
            
//            await _next(env);
//        }
//    }
//}