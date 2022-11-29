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
                newName = RemovePrefix(newName);

                result.Item.Name = GetSeasonName(newName);
                result.Item.SortName = GetSortName(newName);

                string[] tags = Utils.GetTag(newName);
                result.Item.SetTags(tags);
            }

            result.HasMetadata = true;
            return result;
        }

        public string RemovePrefix (string val) {
            string newVal = val;

            if(Regex.IsMatch(newVal, @"^season", RegexOptions.IgnoreCase))
            {
                newVal = Regex.Replace(newVal, @"^season", "", RegexOptions.IgnoreCase);
            }
            else if (Regex.IsMatch(newVal, @"^s " , RegexOptions.IgnoreCase))
            {
                newVal = Regex.Replace(newVal, @"^s ", "", RegexOptions.IgnoreCase);
            }

            return newVal;
        }

        public string GetSeasonName(string val)
        {
            string newVal = val.Split('(')[0].Trim(' ');

            if (Regex.IsMatch(newVal, @"^[0-9]"))
            {
                string[] v = newVal.Split(' ');
                newVal = "第 " + v[0] + " 季";

                for (int i = 1; i < v.Length; i++)
                {
                    newVal += (" " + v[i]);
                }
            }

            return newVal;
        }

        public string GetSortName (string val)
        {
            return val.Split('(')[0].Trim(' ');
        }
    }
}
