///*
// * Copyright (c) Dominick Baier.  All rights reserved.
// * see license.txt
// */

//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading;
//using System.Web;
//using System.Web.Http;
//using Thinktecture.IdentityModel.Authorization;

//namespace Thinktecture.IdentityServer
//{
//    public class ApiClaimsAuthorize : AuthorizeAttribute
//    {
//        private string _resource;
//        private string _action;
//        private string[] _additionalResources;

//        /// <summary>
//        /// Default action claim type.
//        /// </summary>
//        public const string ActionType = "http://application/claims/authorization/action";

//        /// <summary>
//        /// Default resource claim type
//        /// </summary>
//        public const string ResourceType = "http://application/claims/authorization/resource";

//        /// <summary>
//        /// Additional resource claim type
//        /// </summary>
//        public const string AdditionalResourceType = "http://application/claims/authorization/additionalresource";

//        public ApiClaimsAuthorize(string action, string resource, params string[] additionalResources)
//        {
//            _action = action; 
//            _resource = resource;
//            _additionalResources = additionalResources;
//        }

//        public static bool CheckAccess(string action, string resource, params string[] additionalResources)
//        {
//            return CheckAccess(
//                ClaimsPrincipal.Current,
//                action,
//                resource,
//                additionalResources);
//        }

//        public static bool CheckAccess(ClaimsPrincipal principal, string action, string resource, params string[] additionalResources)
//        {
//            var context = CreateAuthorizationContext(
//                principal,
//                action,
//                resource,
//                additionalResources);

//            return ClaimsAuthorization.CheckAccess(context);
//        }

//        protected override bool IsAuthorized(System.Web.Http.Controllers.HttpActionContext actionContext)
//        {
//            return CheckAccess(_action, _resource, _additionalResources);
//        }

//        private static System.Security.Claims.AuthorizationContext CreateAuthorizationContext(ClaimsPrincipal principal, string action, string resource, params string[] additionalResources)
//        {
//            var actionClaims = new Collection<Claim>
//            {
//                new Claim(ActionType, action)
//            };

//            var resourceClaims = new Collection<Claim>
//            {
//                new Claim(ResourceType, resource)
//            };

//            if (additionalResources != null && additionalResources.Length > 0)
//            {
//                additionalResources.ToList().ForEach(ar => resourceClaims.Add(new Claim(AdditionalResourceType, ar)));
//            }

//            return new System.Security.Claims.AuthorizationContext(
//                principal,
//                resourceClaims,
//                actionClaims);
//        }
//    }
//}