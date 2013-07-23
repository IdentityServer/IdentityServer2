namespace Thinktecture.IdentityServer.Core.Repositories.Migrations.SqlCe
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RPTokenType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RelyingParties", "TokenType", c => c.Int(nullable:true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RelyingParties", "TokenType");
        }
    }
}
