using System;
using System.Collections.Generic;

namespace Cross.Proxy.Configs
{
    public class ProxyClients
    {
        public ProxyClients()
        {
            TokenAuth = new BearerTokenAuth();
            //Headers = new KeyValuePair<string, string>[] { };
        }

        public string Name { get; set; }
        public Uri UriBase { get; set; }
        public BearerTokenAuth TokenAuth { get; }
        public KeyValuePair<string, string>[] Headers { get; set; }
    }
}
