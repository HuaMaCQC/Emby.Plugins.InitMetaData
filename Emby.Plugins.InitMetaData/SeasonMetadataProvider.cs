using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Configuration;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Logging;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Emby.Plugins.InitMetaData
{
    public class SeasonMetadataProvider : ILocalMetadataProvider<Season>
    {
        public readonly ILogger Logger;
        public string Name => ProviderNames.Name;
        public SeasonMetadataProvider(ILogManager logManager)
        {
            Logger = logManager.GetLogger(Name);
        }

        public async Task<MetadataResult<Season>> GetMetadata(ItemInfo info, LibraryOptions libraryOptions, IDirectoryService directoryService, CancellationToken cancellationToken)
        {
            var result = new MetadataResult<Season>();
            result.Item = new Season();
            result.Item.LockedFields = new MetadataFields[] {
                    MetadataFields.Genres,
                    MetadataFields.Tags,
                    MetadataFields.Name,
                    MetadataFields.SortName,
                };

            if (string.IsNullOrEmpty(info.Path))
            {
                result.Item.Name = "全集";
            } 
            else
            {
                string[] s = info.Path.Split('\\');

                if (s.Length == 1)
                {
                    s = info.Path.Split('/');
                }

                string newName = s[s.Length - 1];
                newName = newName.Replace("season", "");

                string[] newNames = newName.Split('(');

                if (Regex.IsMatch(newNames[0], @"[0-99]"))
                {
                    result.Item.Name = "第 " + newNames[0] + " 季";
                    result.Item.SortName = newNames[0];
                }
                else
                {
                    result.Item.Name = newNames[0].Trim(' ');
                    result.Item.SortName = newNames[0].Trim(' ');
                }

                string[] tags = newNames.Skip(1).ToArray();

                for(int i = 0; i < tags.Length; i++)
                {
                    string t = tags[i].Replace("(", "").Replace(")", "");
                    string[] ts = t.Split(' ');
                    result.Item.SetTags(ts);
                }
            }

            result.HasMetadata = true;

            return result;
        }
    }
}
