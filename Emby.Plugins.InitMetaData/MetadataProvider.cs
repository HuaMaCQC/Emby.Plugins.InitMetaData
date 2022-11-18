using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Library;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Controller.Resolvers;
using MediaBrowser.Model.Configuration;
using MediaBrowser.Model.Logging;
using MediaBrowser.Model.Providers;
using MediaBrowser.Model.Querying;
using MediaBrowser.Model.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Emby.Plugins.InitMetaData
{
    public class MetadataProvider : ILocalMetadataProvider<Season>
    {
        public readonly ILogger Logger;
        public string Name => ProviderNames.Name;
        public MetadataProvider(ILogManager logManager)
        {
            Logger = logManager.GetLogger(Name);
        }

        public async Task<MetadataResult<Season>> GetMetadata(ItemInfo info, LibraryOptions libraryOptions, IDirectoryService directoryService, CancellationToken cancellationToken)
        {
            var result = new MetadataResult<Season>();
            result.Item = new Season();

            if (string.IsNullOrEmpty(info.Path))
            {
                result.Item.Name = "全集";
            } 
            else
            {
                char[] separators = new char[] { ' ', '(', ')' };
                string[] s = info.Path.Split('\\');
                string newName = s[s.Length - 1];
                newName = newName.Replace("season", "");
                string[] newNames = newName.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                if (Regex.IsMatch(newNames[0], @"[0-99]"))
                {
                    result.Item.Name = "第 " + newNames[0] + "季";
                }
                else
                {
                    result.Item.Name = newNames[0];
                }
                Logger.Info("SupportsTags" + result.Item.SupportsTags);

                string[] tags = newNames.Skip(1).ToArray();

                for(int i = 0; i < tags.Length; i++)
                {
                    result.Item.AddTag(tags[i]);
                }
            }

            result.HasMetadata = true;
            


            return result;
        }
    }
}
