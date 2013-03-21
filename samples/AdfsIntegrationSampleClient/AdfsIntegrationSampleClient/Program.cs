using Microsoft.IdentityModel.Tokens.JWT;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;
using Thinktecture.IdentityModel;
using Thinktecture.IdentityModel.Constants;
using Thinktecture.IdentityModel.Extensions;
using Thinktecture.IdentityModel.WSTrust;

namespace AdfsIntegrationSampleClient
{
    class Program
    {
        static string username = "bob";
        static string password = "bob";
        static string adfsEndpoint = "https://adserver.local/adfs/services/trust/13/usernamemixed";
        static string idsrvEndpoint = "https://idsrv.local/issue/adfs";
        static string realm = "http://test/bearer";
        static X509Certificate2 signingCert;

        static void Main(string[] args)
        {
            signingCert = X509.LocalMachine.My.SubjectDistinguishedName.Find("CN=sts", false).First();

            TestUsername();
            TestSaml();
            TestJwt();

            Console.ReadLine();
        }

        private static void TestJwt()
        {
            Console.WriteLine("Testing JWT token endpoint. Hit enter to continue.");
            Console.ReadLine();

            Console.WriteLine("Getting SAML token directly from ADFS.");
            var token = RequestSamlToken();

            Console.WriteLine("Converting SAML to JWT");
            var jwt = SamlToJwt(token);

            // turn JWT into a JWT (using IdSrv ADFS integration)
            Console.WriteLine("Sending JWT to ADFS integration endpoint in IdentityServer.");
            var jwt2 = JwtToJwt(jwt);

            Console.WriteLine("Token acquired:");
            Console.WriteLine(jwt2);
            Console.WriteLine();

            ValidateJwt(jwt2);
        }

        private static string TestSaml()
        {
            Console.WriteLine("Testing SAML token endpoint. Hit enter to continue.");
            Console.ReadLine();

            // request initial token from ADFS
            Console.WriteLine("Getting SAML token directly from ADFS.");
            var token = RequestSamlToken();

            // turn SAML token into a JWT (using IdSrv ADFS integration)
            Console.WriteLine("Sending SAML token to ADFS integration endpoint in IdentityServer.");
            var jwt = SamlToJwt(token);

            Console.WriteLine("Token acquired:");
            Console.WriteLine(jwt);
            Console.WriteLine();

            ValidateJwt(jwt);
            return jwt;
        }

        private static void TestUsername()
        {
            Console.WriteLine("Testing Username token endpoint. Hit enter to continue.");
            Console.ReadLine();
            
            // use ADFS integration uname/password endoint
            RequestUserNameToken();
        }

        private static string RequestUserNameToken()
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

            var response = client.PostAsync("", form).Result;
            response.EnsureSuccessStatusCode();

            var tokenResponse = response.Content.ReadAsStringAsync().Result;

            var json = JObject.Parse(tokenResponse);
            var jwt = json["access_token"].ToString();
            Console.WriteLine("Token acquired:");
            Console.WriteLine(jwt);
            Console.WriteLine();

            ValidateJwt(jwt);

            return jwt;
        }

        private static string JwtToJwt(string jwt)
        {
            var client = new HttpClient { BaseAddress = new Uri(idsrvEndpoint) };

            var values = new Dictionary<string, string>
            {
                { OAuth2Constants.GrantType, "urn:ietf:params:oauth:grant-type:jwt-bearer" },
                { OAuth2Constants.Assertion, jwt },
                { OAuth2Constants.Scope, realm }
            };

            var form = new FormUrlEncodedContent(values);

            var response = client.PostAsync("", form).Result;
            response.EnsureSuccessStatusCode();

            var tokenResponse = response.Content.ReadAsStringAsync().Result;
            var json = JObject.Parse(tokenResponse);
            return json["access_token"].ToString();
        }

        private static void ValidateJwt(string jwt)
        {
            var handler = new JWTSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters()
            {
                AllowedAudience = realm,
                SigningToken = new X509SecurityToken(signingCert),
                ValidIssuer = "http://idsrv.local/trust",
                ValidateExpiration = true
            };

            var principal = handler.ValidateToken(jwt, validationParameters);

            Console.WriteLine("Token validated. Claims from token:");
            foreach (var claim in principal.Claims)
            {
                Console.WriteLine("{0}\n {1}", claim.Type, claim.Value);
            }
            Console.WriteLine();
        }

        private static string SamlToJwt(SecurityToken token)
        {
            var client = new HttpClient { BaseAddress = new Uri(idsrvEndpoint) };

            var values = new Dictionary<string, string>
            {
                { OAuth2Constants.GrantType, "urn:ietf:params:oauth:grant-type:saml2-bearer" },
                { OAuth2Constants.Assertion, Convert.ToBase64String(Encoding.UTF8.GetBytes(token.ToTokenXmlString())) },
                { OAuth2Constants.Scope, realm }
            };

            var form = new FormUrlEncodedContent(values);

            var response = client.PostAsync("", form).Result;
            response.EnsureSuccessStatusCode();

            var tokenResponse = response.Content.ReadAsStringAsync().Result;
            var json = JObject.Parse(tokenResponse);
            return json["access_token"].ToString();
        }


        private static SecurityToken RequestSamlToken()
        {
            var factory = new WSTrustChannelFactory(
                new UserNameWSTrustBinding(SecurityMode.TransportWithMessageCredential),
                new EndpointAddress(adfsEndpoint));
            factory.TrustVersion = TrustVersion.WSTrust13;

            factory.Credentials.UserName.UserName = username;
            factory.Credentials.UserName.Password = password;

            var rst = new RequestSecurityToken
            {
                RequestType = RequestTypes.Issue,
                KeyType = KeyTypes.Bearer,
                AppliesTo = new EndpointReference(realm)
            };

            return factory.CreateChannel().Issue(rst);
        }
    }
}
