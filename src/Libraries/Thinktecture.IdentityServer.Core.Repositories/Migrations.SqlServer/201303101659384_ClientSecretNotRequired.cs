namespace Thinktecture.IdentityServer.Core.Repositories.Migrations.SqlServer
{
    using System.Data.Entity.Migrations;
    
    public partial class ClientSecretNotRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Client", "ClientSecret", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Client", "ClientSecret", c => c.String(nullable: false));
        }
    }
}
