using System.ComponentModel.Composition.Hosting;
using System.Windows;
using Thinktecture.IdentityServer.Repositories;

namespace InMemoryHost
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Container.Current = new CompositionContainer(new RepositoryExportProvider());
        }

    }
}
