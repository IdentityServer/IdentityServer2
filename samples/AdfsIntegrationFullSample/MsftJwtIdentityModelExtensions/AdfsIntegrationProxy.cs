using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Thinktecture.IdentityModel.Constants;
using Thinktecture.IdentityModel.Extensions;

namespace MsftJwtIdentityModelExtensions
{
    public class AdfsIntegrationProxy
    {
        string idsrvEndpoint;
        public AdfsIntegrationProxy(string idsrvEndpoint)
        {
            this.idsrvEndpoint = idsrvEndpoint;
        }

        public async Task<string> UsernameToJwtAsync(string username, string password, string realm)
        {
            var client = new HttpClient { BaseAddress = new Uri(idsrvEndpoint) };

            var values = new Dictionary<string, string>
            {
                { OAuth2Constants.GrantType, OAuth2Constants.GrantTypes.Password },
                { OAuth2Constants.UserName, username },
                { OAuth2Constants.Password, password },
                { OAuth2Constants.Scope, realm }
            };

            var form = new FormUrlEncodedContent(values);

            var response = await client.PostAsync("", form);
            response.EnsureSuccessStatusCode();

            var tokenResponse = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(tokenResponse);
            return json["access_token"].ToString();
        }

        public async Task<string> SamlToJwtAsync(SecurityToken token, string realm)
        {
            var samlToken = token as SamlSecurityToken;
            if (samlToken == null) throw new ArgumentException("token not an instance of a SamlSecurityToken");

            return await SamlToJwtAsync(samlToken.ToTokenXmlString(), realm);
        }

        public async Task<string> SamlToJwtAsync(string samlToken, string realm)
        {
            var client = new HttpClient { BaseAddress = new Uri(idsrvEndpoint) };

            var values = new Dictionary<string, string>
            {
                { OAuth2Constants.GrantType, "urn:ietf:params:oauth:grant-type:saml2-bearer" },
                { OAuth2Constants.Assertion, Convert.ToBase64String(Encoding.UTF8.GetBytes(samlToken)) },
                { OAuth2Constants.Scope, realm }
            };

            var form = new FormUrlEncodedContent(values);

            var response = await client.PostAsync("", form);
            response.EnsureSuccessStatusCode();

            var tokenResponse = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(tokenResponse);
            return json["access_token"].ToString();
        }

        public async Task<string> JwtToJwtAsync(string jwt, string realm)
        {
            var client = new HttpClient { BaseAddress = new Uri(idsrvEndpoint) };

            var values = new Dictionary<string, string>
            {
                { OAuth2Constants.GrantType, "urn:ietf:params:oauth:grant-type:jwt-bearer" },
                { OAuth2Constants.Assertion, jwt },
                { OAuth2Constants.Scope, realm }
            };

            var form = new FormUrlEncodedContent(values);

            var response = await client.PostAsync("", form);
            response.EnsureSuccessStatusCode();

            var tokenResponse = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(tokenResponse);
            return json["access_token"].ToString();
        }
    }
}