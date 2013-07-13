using System;
using System.Collections.Generic;
using Thinktecture.IdentityModel.Constants;

namespace Thinktecture.IdentityServer.Models
{
    public class Grant
    {
        public string HandleId { get; set; }
        public string GrantType { get; set; }
        
        public string Subject { get; set; }
        public IEnumerable<string> Scopes { get; set; }
        public string ClientId { get; set; }
        public string RedirectUri { get; set; }
        public DateTime? Expiration { get; set; }

        public Grant()
        {
            HandleId = Guid.NewGuid().ToString("N");
        }

        public static Grant CreateAuthorizationCode(string clientId, string subject, IEnumerable<string> scopes, string redirectUri)
        {
            return new Grant
            {
                GrantType = OAuth2Constants.GrantTypes.AuthorizationCode,
                
                ClientId = clientId,
                Subject = subject,
                Scopes = scopes,
                RedirectUri = redirectUri,
                Expiration = DateTime.UtcNow.AddMinutes(60)
            };
        }
    }
}
