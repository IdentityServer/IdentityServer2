namespace Thinktecture.IdentityServer.Core.Repositories.Migrations.SqlServer
{
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class SqlServerConfiguration : DbMigrationsConfiguration<Thinktecture.IdentityServer.Repositories.Sql.IdentityServerConfigurationContext>
    {
        public SqlServerConfiguration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Thinktecture.IdentityServer.Repositories.Sql.IdentityServerConfigurationContext context)
        {
            //  This method will be called after migrating to the latest version.
            Thinktecture.IdentityServer.Repositories.Sql.ConfigurationDatabaseInitializer.SeedContext(context);

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
