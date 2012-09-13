using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.IdentityServer.Web.ViewModels
{
    public class ProtocolsInputModel
    {
        [Required]
        public bool[] Protocols { get; set; }

        public void Update(Repositories.IConfigurationRepository configurationRepository)
        {
            new ProtocolsViewModel(configurationRepository).Update(this.Protocols);
        }
    }

    public class ProtocolsViewModel
    {
        static List<Tuple<string, Func<IConfigurationRepository, bool>, Action<IConfigurationRepository, bool>>> protocolMap =
            new List<Tuple<string, Func<IConfigurationRepository, bool>, Action<IConfigurationRepository, bool>>>();
        static ProtocolsViewModel()
        {
            protocolMap.Add(new Tuple<string, Func<IConfigurationRepository, bool>, Action<IConfigurationRepository, bool>>(
                "WS-Federation",
                x => x.WSFederation.Enabled,
                (c, v) => { var data = c.WSFederation; data.Enabled = v; c.WSFederation = data; }));
            protocolMap.Add(new Tuple<string, Func<IConfigurationRepository, bool>, Action<IConfigurationRepository, bool>>(
                "Federation Metadata",
                x => x.FederationMetadata.Enabled,
                (c, v) => { var data = c.FederationMetadata; data.Enabled = v; c.FederationMetadata = data; }));
            protocolMap.Add(new Tuple<string, Func<IConfigurationRepository, bool>, Action<IConfigurationRepository, bool>>(
                "WS-Trust",
                x => x.WSTrust.Enabled,
                (c, v) => { var data = c.WSTrust; data.Enabled = v; c.WSTrust = data; }));
            protocolMap.Add(new Tuple<string, Func<IConfigurationRepository, bool>, Action<IConfigurationRepository, bool>>(
                "Simple HTTP",
                x => x.SimpleHttp.Enabled,
                (c, v) => { var data = c.SimpleHttp; data.Enabled = v; c.SimpleHttp = data; }));
            protocolMap.Add(new Tuple<string, Func<IConfigurationRepository, bool>, Action<IConfigurationRepository, bool>>(
                "OAuth2",
                x => x.OAuth2.Enabled,
                (c, v) => { var data= c.OAuth2; data.Enabled = v; c.OAuth2 = data; }));
        }

        private Repositories.IConfigurationRepository ConfigurationRepository;

        public ProtocolsViewModel(Repositories.IConfigurationRepository ConfigurationRepository)
        {
            this.ConfigurationRepository = ConfigurationRepository;
            
            for (int i = 0; i < protocolMap.Count; i++)
            {
                var item = protocolMap[i];
                var id = i;
                var name = item.Item1;
                var enabled = item.Item2(this.ConfigurationRepository);
                protocols.Add(new Protocol { ID = id, Name = name, Enabled = enabled });
            }
        }

        public class Protocol
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public bool Enabled { get; set; }
        }

        List<Protocol> protocols = new List<Protocol>();
        public IEnumerable<Protocol> Protocols
        {
            get
            {
                return protocols;
            }
        }

        internal void Update(bool[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                protocolMap[i].Item3(ConfigurationRepository, values[i]);
            }
        }
    }
}