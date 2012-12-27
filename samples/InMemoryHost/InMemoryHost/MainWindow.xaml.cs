using System;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Windows;
using System.Xml.Linq;
using Thinktecture.IdentityModel;
using Thinktecture.IdentityModel.Constants;
using Thinktecture.IdentityModel.Extensions;
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
        }

        private void CreateTokenButton_Click(object sender, RoutedEventArgs e)
        {
            var principal = Principal.Create("InMemory",
                new Claim(ClaimTypes.Name, UserName.Text));
            
            var sts = new STS();
            SecurityToken token;

            var success = sts.TryIssueToken(
                new EndpointReference(Realm.Text),
                principal,
                TokenTypes.Saml2TokenProfile11,
                out token);

            if (success)
            {
                var xml = token.ToTokenXmlString();
                Output.Text = XElement.Parse(xml).ToString();
            }
            else
            {
                throw new Exception("Error");
            }
        }
    }
}
