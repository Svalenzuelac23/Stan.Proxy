using System.Collections.Generic;

namespace Cross.Proxy.Configs
{
    public class ProxySettings
    {
        public ProxySettings()
        {
            Clients = new List<ProxyClients>();
        }
        public List<ProxyClients> Clients { get; }
    }
}
