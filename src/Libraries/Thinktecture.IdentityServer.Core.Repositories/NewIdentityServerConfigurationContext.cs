/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Thinktecture.IdentityServer.Repositories.Sql.Configuration;

namespace Thinktecture.IdentityServer.Repositories.Sql
{    
    public class NewIdentityServerConfigurationContext : DbContext
    {
        public DbSet<GlobalConfiguration> GlobalConfiguration { get; set; }

        

        public DbSet<ClientCertificates> ClientCertificates { get; set; }

        public DbSet<Delegation> Delegation { get; set; }

        public DbSet<RelyingParties> RelyingParties { get; set; }

        public DbSet<IdentityProvider> IdentityProviders { get; set; }

        public static Func<NewIdentityServerConfigurationContext> FactoryMethod { get; set; }

        public NewIdentityServerConfigurationContext()
        {            
        }

        public NewIdentityServerConfigurationContext(DbConnection conn) : base(conn, true)
        {
        }

        public NewIdentityServerConfigurationContext(IDatabaseInitializer<IdentityServerConfigurationContext> initializer)
        {
            Database.SetInitializer(initializer);
        }

        public static NewIdentityServerConfigurationContext Get()
        {
            if (FactoryMethod != null) return FactoryMethod();

            var cs = ConfigurationManager.ConnectionStrings["IdentityServerConfiguration"].ConnectionString;
            var conn = Database.DefaultConnectionFactory.CreateConnection(cs);
            return new NewIdentityServerConfigurationContext(conn);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}
