using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Configuration;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Logging;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Emby.Plugins.InitMetaData
{
    public class TVShowMetadataProvider : ILocalMetadataProvider<Series>
    {
        public readonly ILogger Logger;
        public string Name => ProviderNames.Name;
        public TVShowMetadataProvider(ILogManager logManager)
        {
            Logger = logManager.GetLogger(Name);
        }

        public async Task<MetadataResult<Series>> GetMetadata(ItemInfo info, LibraryOptions libraryOptions, IDirectoryService directoryService, CancellationToken cancellationToken)
        {
            var result = new MetadataResult<Series>();
            result.Item = new Series();
            result.Item.LockedFields = new MetadataFields[] {
                    MetadataFields.Genres,
                    MetadataFields.Tags,
                    MetadataFields.Name,
                    MetadataFields.SortName,
                };

            string[] s = info.Path.Split('\\');

            if (s.Length == 1)
            {
                s = info.Path.Split('/');
            }

            string newName = s[s.Length - 1];
            result.Item.Name = GetName(newName);
            string[] tags = Utils.GetTag(newName);

            result.Item.SetTags(tags);
            result.Item.SetGenres(new string[0]);
            result.HasMetadata = true;

            return result;
        }

        public string GetName(string val)
        {
            return Regex.Replace(val, @"\(.*\)", "").Trim(' ');
        }
    }
}
