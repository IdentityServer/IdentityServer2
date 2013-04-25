namespace Thinktecture.IdentityServer.Core.Repositories.Migrations.SqlServer
{
    using System.Data.Entity.Migrations;
    
    public partial class AdfsIntegration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdfsIntegrationConfiguration",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Enabled = c.Boolean(nullable: false),
                        UsernameAuthenticationEnabled = c.Boolean(nullable: false),
                        SamlAuthenticationEnabled = c.Boolean(nullable: false),
                        JwtAuthenticationEnabled = c.Boolean(nullable: false),
                        PassThruAuthenticationToken = c.Boolean(nullable: false),
                        AuthenticationTokenLifetime = c.Int(nullable: false),
                        UserNameAuthenticationEndpoint = c.String(),
                        FederationEndpoint = c.String(),
                        IssuerUri = c.String(),
                        IssuerThumbprint = c.String(),
                        EncryptionCertificate = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AdfsIntegrationConfiguration");
        }
    }
}
