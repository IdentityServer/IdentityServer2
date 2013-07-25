using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thinktecture.IdentityServer.Protocols.OpenIdConnect
{
    public class OidcViewModel
    {
        static string[] SupportedScopes =
            {
                OidcConstants.Scopes.Profile,
                OidcConstants.Scopes.Phone,
                OidcConstants.Scopes.Address,
                OidcConstants.Scopes.Email,
            };

        public ValidatedRequest ValidatedRequest { get; set; }
        public OidcViewModel(ValidatedRequest validatedRequest)
        {
            if (validatedRequest == null) throw new ArgumentNullException("validatedRequest");
            this.ValidatedRequest = validatedRequest;
            ValidateScopes(GetRawScopes());
        }

        void ValidateScopes(IEnumerable<string> scopes)
        {
            scopes = scopes ?? Enumerable.Empty<string>();

            var reminder = scopes
                .Except(SupportedScopes)
                .Except(new string[] { 
                    OidcConstants.Scopes.OpenId, 
                    OidcConstants.Scopes.OfflineAccess });
            
            if (reminder.Any())
            {
                throw new Exception("Unsupported Scopes Requested");
            }
        }

        public bool OfflineScope
        {
            get
            {
                return this.ValidatedRequest.Scopes.Contains(OidcConstants.Scopes.OfflineAccess);
            }
        }

        IEnumerable<string> GetRawScopes()
        {
            return this.ValidatedRequest.Scopes.Split(
                new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
        }
        void SetRawScopes(IEnumerable<string> scopes)
        {
            this.ValidatedRequest.Scopes = scopes.Aggregate((x, y) => x + " " + y);
        }

        public IEnumerable<string> GetDisplayScopes()
        {
            var vals =
                this.GetRawScopes()
                .Except(new string[] { 
                    OidcConstants.Scopes.OpenId, 
                    OidcConstants.Scopes.OfflineAccess })
                .Intersect(SupportedScopes);
            return vals;
        }
        
        public IEnumerable<string> GetScopes()
        {
            return this.GetRawScopes().Except(new string[] { OidcConstants.Scopes.OpenId });
        }

        public void SetScopes(IEnumerable<string> scopes)
        {
            scopes = scopes ?? Enumerable.Empty<string>();
            ValidateScopes(scopes);
            
            var intersection = GetScopes().Intersect(scopes);
            var newScopes = new List<string>()
            {
                OidcConstants.Scopes.OpenId
            };
            newScopes.AddRange(intersection);

            SetRawScopes(newScopes);
        }
    }
}
