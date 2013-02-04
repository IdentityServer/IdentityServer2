//using System.ComponentModel.Composition;
//using System.Net;
//using Thinktecture.IdentityModel.Tokens.Http;
//using Thinktecture.IdentityServer.Repositories;

//namespace Thinktecture.IdentityServer.Protocols.OAuth2
//{
//    public class ClientValidator
//    {
//        [Import]
//        public IClientsRepository Clients { get; set; }

//        public ClientValidator()
//        {
//            Container.Current.SatisfyImportsOnce(this);
//        }

//        public ClientValidator(IClientsRepository clients)
//        {
//            Clients = clients;
//        }

//        private bool ValidateClient(string clientId, string clientSecret)
//        {
//            var success = Clients.ValidateClient(
//                                clientId,
//                                clientSecret);

//            if (!success)
//            {
//                Tracing.Error("Invalid client: " + clientId);
            
//                throw new AuthenticationException
//                {
//                    StatusCode = HttpStatusCode.BadRequest,
//                    ReasonPhrase = string.Format("{{ \"{0}\": \"{1}\" }}", 
//                        OAuth2Constants.Errors.Error, 
//                        OAuth2Constants.Errors.InvalidClient)
//                };
//            }

//            return success;
//        }
//    }
//}
