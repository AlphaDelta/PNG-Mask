using System;
using System.Collections.Generic;
using System.Text;

namespace PNGMask.GUI
{
    public class Provider
    {
        public string Name;
        public Type ProviderType;

        public Provider(string Name, Type ProviderType)
        {
            this.Name = Name;
            this.ProviderType = ProviderType;
        }
    }
}
