using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace OwinDemo
{
    public class AuthenticationMiddleware
    {
        private readonly Func<IDictionary<string, object>, Task> _next;

        public AuthenticationMiddleware(Func<IDictionary<string, object>, Task> next)
        {
            _next = next;
        }

        public async Task Invoke(IDictionary<string, object> env)
        {
            // inspect env and do credential validation
            env["server.User"] = CreatePrincipal();

            await _next(env);
        }

        private IPrincipal CreatePrincipal()
        {
            return new GenericPrincipal(
                        new GenericIdentity("dominick", "Basic"),
                        new string[0]);
        }
    }
}