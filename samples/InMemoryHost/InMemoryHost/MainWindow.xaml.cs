using System;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using Thinktecture.IdentityModel;
using Thinktecture.IdentityModel.Constants;
using Thinktecture.IdentityModel.Extensions;
using Thinktecture.IdentityModel.Tokens;
using Thinktecture.IdentityServer.Protocols;

namespace InMemoryHost
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SelectedTokenType.SelectedItem = SelectedTokenType.Items[0];
        }

        private void CreateTokenButton_Click(object sender, RoutedEventArgs e)
        {
            var principal = Principal.Create("InMemory",
                new Claim(ClaimTypes.Name, UserName.Text));
            var tokenType = GetTokenType();
            
            var sts = new STS();
            SecurityToken token;

            var success = sts.TryIssueToken(
                new EndpointReference(Realm.Text),
                principal,
                tokenType,
                out token);

            if (success)
            {
                if (tokenType == TokenTypes.Saml2TokenProfile11 || tokenType == TokenTypes.Saml11TokenProfile11)
                {
                    var xml = token.ToTokenXmlString();
                    Output.Text = XElement.Parse(xml).ToString();
                }
                if (tokenType == TokenTypes.JsonWebToken)
                {
                    var tokenString = new JsonWebTokenHandler().WriteToken(token);
                    Output.Text = tokenString;
                }
            }
            else
            {
                throw new Exception("Error");
            }
        }

        private string GetTokenType()
        {
            var selected = SelectedTokenType.SelectedItem as ListBoxItem;

            switch (selected.Content as string)
            {
                case "SAML 1.1":
                    return TokenTypes.Saml11TokenProfile11;
                case "SAML 2.0":
                    return TokenTypes.Saml2TokenProfile11;
                case "JWT":
                    return TokenTypes.JsonWebToken;
                default:
                    throw new Exception("Invalid token type");
            }
        }
    }
}
