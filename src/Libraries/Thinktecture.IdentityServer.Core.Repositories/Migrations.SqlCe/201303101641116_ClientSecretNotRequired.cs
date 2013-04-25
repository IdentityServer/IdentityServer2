namespace Thinktecture.IdentityServer.Core.Repositories.Migrations.SqlCe
{
    using System.Data.Entity.Migrations;
    
    public partial class ClientSecretNotRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Client", "ClientSecret", c => c.String(nullable: true, maxLength: 4000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Client", "ClientSecret", c => c.String(nullable: false, maxLength: 4000));
        }
    }
}
