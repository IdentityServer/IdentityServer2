using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Data.Entity;
using System.Web.Http;
using System.Web.Http.SelfHost;
using Thinktecture.IdentityServer.Repositories;
using Thinktecture.IdentityServer.Repositories.Sql;

namespace SelfHostConsoleHost
{
    internal class SelfHostServer
    {
        private HttpSelfHostServer selfHost;

        [Import]
        public IConfigurationRepository ConfigurationRepository { get; set; }

        public async void Start(string baseAddress)
        {
            var httpConfig = new HttpSelfHostConfiguration(baseAddress);
            httpConfig.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            Database.SetInitializer(new ConfigurationDatabaseInitializer());

            Container.Current = new CompositionContainer(new RepositoryExportProvider());
            Container.Current.SatisfyImportsOnce(this);

            ProtocolConfig.RegisterProtocols(httpConfig, ConfigurationRepository);

            selfHost = new HttpSelfHostServer(httpConfig);
            await selfHost.OpenAsync();
        }

        public async void Stop()
        {
            if (selfHost != null)
            {
                await selfHost.CloseAsync();
            }
        }
    }
}