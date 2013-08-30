namespace Thinktecture.IdentityServer.Core.Repositories.Migrations.SqlCe
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DisableSSL : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GlobalConfiguration", "DisableSSL", c => c.Boolean(nullable: false));
            AddColumn("dbo.GlobalConfiguration", "PublicHostName", c => c.String(maxLength: 4000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GlobalConfiguration", "PublicHostName");
            DropColumn("dbo.GlobalConfiguration", "DisableSSL");
        }
    }
}
