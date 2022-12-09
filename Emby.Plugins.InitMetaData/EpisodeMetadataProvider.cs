using MediaBrowser.Controller.Entities;
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
    public class EpisodeMetadataProvider : ILocalMetadataProvider<Episode> 
    {
        public readonly ILogger Logger;
        public string Name => ProviderNames.Name;
        public readonly ItemLookupInfo mySeasonInfo;
        public EpisodeMetadataProvider(ILogManager logManager, SeasonInfo SeasonInfo)
        {
            Logger = logManager.GetLogger(Name);
            mySeasonInfo = SeasonInfo;
        }

        public async Task<MetadataResult<Episode>> GetMetadata(ItemInfo info, LibraryOptions libraryOptions, IDirectoryService directoryService, CancellationToken cancellationToken)
        {
            var result = new MetadataResult<Episode>();
            result.Item = new Episode();
            result.Item.LockedFields = new MetadataFields[] {
                    MetadataFields.Genres,
                    MetadataFields.Tags,
                    MetadataFields.Name,
                    MetadataFields.SortName,
                    MetadataFields.Studios,
                };

            string[] s = info.Path.Split('\\');

            if (s.Length == 1)
            {
                s = info.Path.Split('/');
            }

            string newName = s[s.Length - 1];

            string newItemName = GetName(newName);
            result.HasMetadata = true;
            result.Item.Name = newItemName;

            for (int i = 0; i < StudioRule.Rule.Length; i++)
            {
                if (Regex.IsMatch(newName, StudioRule.Rule[i][0]))
                {
                    result.Item.AddStudio(StudioRule.Rule[i][1]);
                }
            }

            return result;
        }

        string GetName(string name)
        {
            string NewName = name;
            NewName = NewName.Split('.')[0];

            for (int i = 0; i < EpisodeNameRule.Rule.Length; i++)
            {
                if (Regex.IsMatch(name, EpisodeNameRule.Rule[i].Regular))
                {
                    Regex r = new Regex(EpisodeNameRule.Rule[i].Regular, RegexOptions.IgnoreCase);
                    Match m = r.Match(name);

                    if (!string.IsNullOrEmpty(m.Groups[0].ToString()))
                    {
                        return EpisodeNameRule.Rule[i].Replace(m.Groups[0].ToString());
                    }
                }
            };

            return NewName;
        }
    }
}


