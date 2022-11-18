using System;
using System.Collections.Generic;
using System.Text;
using MediaBrowser.Common.Plugins;

namespace Emby.Plugins.InitMetaData
{
    public class Plugin : BasePlugin
    {
        private Guid guid = new Guid("5d157593-a99c-4510-9fd9-0d154fcae8bf");

        public override string Name
        {
            get { return ProviderNames.Name; }
        }

        public override Guid Id
        {
            get { return guid; }
        }

    }
}
