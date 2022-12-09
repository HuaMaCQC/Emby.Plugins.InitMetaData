using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Configuration;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Logging;
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

                newName = SeasonNameRule.RemovePrefix(newName);

                result.Item.Name = SeasonNameRule.GetSeasonName(newName);
                result.Item.SortName = SeasonNameRule.GetSortName(newName);
                result.Item.IndexNumber = GetIndexNumber(newName);

                string[] tags = TagRule.GetTag(newName);
                result.Item.SetTags(tags);
            }

            result.HasMetadata = true;
            return result;
        }

        int? GetIndexNumber(string name)
        {
            string[] n = name.Trim(' ').Split(' ');

            int index = int.Parse(n[0]);

            if (n.Length > 1)
            {
                index = index * 10;
            }

            return index;
        }
    }
}
