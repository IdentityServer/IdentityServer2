namespace Thinktecture.IdentityServer.Core.Repositories.Migrations.SqlServer
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StoredGrants : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StoredGrant",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GrantId = c.String(nullable: false),
                        GrantType = c.Int(nullable: false),
                        Subject = c.String(nullable: false),
                        Scopes = c.String(nullable: false),
                        ClientId = c.String(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Expiration = c.DateTime(nullable: false),
                        RefreshTokenExpiration = c.DateTime(),
                        RedirectUri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.StoredGrant");
        }
    }
}
