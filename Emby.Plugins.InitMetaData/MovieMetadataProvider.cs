using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Configuration;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Logging;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Emby.Plugins.InitMetaData
{
    public class MovieMetadataProvider : ILocalMetadataProvider<Movie>
    {
        public readonly ILogger Logger;
        public string Name => ProviderNames.Name;

        public MovieMetadataProvider(ILogManager logManager)
        {
            Logger = logManager.GetLogger(Name);
        }

        public async Task<MetadataResult<Movie>> GetMetadata(ItemInfo info, LibraryOptions libraryOptions, IDirectoryService directoryService, CancellationToken cancellationToken)
        {
            var result = new MetadataResult<Movie>();
            result.Item = new Movie();

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

            if(s.Length < 2)
            {
                return result;
            }

            string newName = s[s.Length - 2];

            result.Item.Name = GetName(newName);
            string[] tags = TagRule.GetTag(newName);

            result.Item.SetTags(tags);
            result.Item.SetGenres(new string[0]);
            result.HasMetadata = true;

            return result;
        }

        public static string GetName(string val)
        {
            return val.Split('(')[0].Trim(' ');
        }
    }
}
