///*
// * Copyright (c) Dominick Baier.  All rights reserved.
// * see license.txt
// */

//using System.IO;
//using System.Reflection;
//using System.Runtime.Serialization.Json;
//using System.Web;
//using System.Web.Mvc;

//namespace Thinktecture.IdentityServer.Protocols.JSNotify
//{
//    public class JSNotifyResult : ActionResult
//    {
//        AccessTokenResponse _response;

//        public JSNotifyResult(AccessTokenResponse response)
//        {
//            _response = response;
//        }

//        public override void ExecuteResult(ControllerContext context)
//        {
//            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Thinktecture.IdentityServer.Protocols.JSNotify.jsnotify.htm");
//            var html = new StreamReader(stream).ReadToEnd();

//            var ser = new DataContractJsonSerializer(typeof(AccessTokenResponse));
//            var ms = new MemoryStream();
//            ser.WriteObject(ms, _response);
//            ms.Seek(0, SeekOrigin.Begin);

//            var dtoString = new StreamReader(ms).ReadToEnd();
//            var responseString = string.Format(html, dtoString);

//            context.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
//            context.HttpContext.Response.Cache.SetNoStore();

//            context.HttpContext.Response.Write(responseString);
//            context.HttpContext.Response.Flush();
//        }
//    }
//}