namespace Thinktecture.IdentityServer.Core.Repositories.Migrations.SqlCe
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
                        GrantId = c.String(nullable: false, maxLength: 4000),
                        GrantType = c.Int(nullable: false),
                        Subject = c.String(nullable: false, maxLength: 4000),
                        Scopes = c.String(nullable: false, maxLength: 4000),
                        ClientId = c.String(nullable: false, maxLength: 4000),
                        Created = c.DateTime(nullable: false),
                        Expiration = c.DateTime(nullable: false),
                        RefreshTokenExpiration = c.DateTime(),
                        RedirectUri = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.StoredGrant");
        }
    }
}
