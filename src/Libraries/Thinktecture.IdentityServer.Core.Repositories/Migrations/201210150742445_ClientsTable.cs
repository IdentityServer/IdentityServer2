namespace Thinktecture.IdentityServer.Core.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClientsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Client",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 4000),
                        Description = c.String(nullable: false, maxLength: 4000),
                        ClientId = c.String(nullable: false, maxLength: 4000),
                        ClientSecret = c.String(nullable: false, maxLength: 4000),
                        RedirectUri = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Client");
        }
    }
}
