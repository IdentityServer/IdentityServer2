namespace Thinktecture.IdentityServer.Core.Repositories.Migrations.SqlCe
{
    using System.Data.Entity.Migrations;
    
    public partial class RefreshToken : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CodeToken",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(maxLength: 4000),
                        ClientId = c.Int(nullable: false),
                        UserName = c.String(maxLength: 4000),
                        Scope = c.String(maxLength: 4000),
                        Type = c.Int(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.OAuth2Configuration", "EnableCodeFlow", c => c.Boolean(nullable: false));
            AddColumn("dbo.Client", "AllowRefreshToken", c => c.Boolean(nullable: false));
            DropColumn("dbo.Client", "NativeClient");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Client", "NativeClient", c => c.Boolean(nullable: false));
            DropColumn("dbo.Client", "AllowRefreshToken");
            DropColumn("dbo.OAuth2Configuration", "EnableCodeFlow");
            DropTable("dbo.CodeToken");
        }
    }
}
