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

    public abstract class IRule
    {
        public abstract string Rule();
        public abstract string MyReplac(string str);
    }

    public class ItemMetadataProvider : ILocalMetadataProvider<Episode>
    {
        public readonly ILogger Logger;
        public string Name => ProviderNames.Name;
        public ItemMetadataProvider(ILogManager logManager)
        {
            Logger = logManager.GetLogger(Name);
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

            result.HasMetadata = true;
            result.Item.Name = GetName(newName);

            for (int i = 0; i < StudioRule.Rule.Length; i++)
            {
                if (Regex.IsMatch(newName, StudioRule.Rule[i][0]))
                {
                    result.Item.AddStudio(StudioRule.Rule[i][1]);
                }
            }
                

            for (int i = 0; i< TagRule.Rule.Length; i++)
            {
                if (Regex.IsMatch(newName, TagRule.Rule[i][0]))
                {
                    result.Item.AddTag(TagRule.Rule[i][1]);
                }
            }

            return result;
        }

        string GetName(string name)
        {
            string NewName = name;
            NewName = NewName.Split('.')[0];

            for (int i = 0; i < NameRule.Rule.Length; i++)
            {
                if (Regex.IsMatch(name, NameRule.Rule[i].Regular))
                {
                    Regex r = new Regex(NameRule.Rule[i].Regular, RegexOptions.IgnoreCase);
                    Match m = r.Match(name);

                    if (!string.IsNullOrEmpty(m.Groups[0].ToString()))
                    {
                        return NameRule.Rule[i].Replace(m.Groups[0].ToString());
                    }
                }
            };

            return NewName;
        }
    }
}


