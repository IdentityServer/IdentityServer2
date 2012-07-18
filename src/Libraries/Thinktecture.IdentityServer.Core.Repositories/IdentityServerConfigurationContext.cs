/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Thinktecture.IdentityServer.Repositories.Sql
{    
    public class IdentityServerConfigurationContext : DbContext
    {
        public DbSet<Global> Global { get; set; }

        public DbSet<Certificates> Certificates { get; set; }

        public DbSet<ClientCertificates> ClientCertificates { get; set; }

        public DbSet<Delegation> Delegation { get; set; }

        public DbSet<RelyingParties> RelyingParties { get; set; }

        public DbSet<Endpoints> Endpoints { get; set; }

        public DbSet<IdentityProvider> IdentityProviders { get; set; }

        public static Func<IdentityServerConfigurationContext> FactoryMethod { get; set; }

        public IdentityServerConfigurationContext()
        {            
        }

        public IdentityServerConfigurationContext(DbConnection conn) : base(conn, true)
        {
        }

        public IdentityServerConfigurationContext(IDatabaseInitializer<IdentityServerConfigurationContext> initializer)
        {
            Database.SetInitializer(initializer);
        }

        public static IdentityServerConfigurationContext Get()
        {
            if (FactoryMethod != null) return FactoryMethod();

            var cs = ConfigurationManager.ConnectionStrings["IdentityServerConfiguration"].ConnectionString;
            var conn = Database.DefaultConnectionFactory.CreateConnection(cs);
            return new IdentityServerConfigurationContext(conn);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}
