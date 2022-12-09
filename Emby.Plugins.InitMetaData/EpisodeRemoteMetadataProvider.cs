using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Logging;
using MediaBrowser.Model.Providers;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Emby.Plugins.InitMetaData
{
    public class EpisodeRemoteMetadataProvider : IRemoteMetadataProvider<Episode, EpisodeInfo>
    {
        public readonly ILogger Logger;
        private readonly IHttpClient _httpClient;
        public string Name => ProviderNames.Name;
        public EpisodeRemoteMetadataProvider(ILogManager logManager)
        {
            Logger = logManager.GetLogger(Name);
        }

        public async Task<MetadataResult<Episode>> GetMetadata(EpisodeInfo info, CancellationToken cancellationToken)
        {
            var result = new MetadataResult<Episode>();

            result.Item = new Episode();
            result.HasMetadata = true;
            result.Item.IndexNumber = GetIndexNumber(info.Name);
            result.Item.ParentIndexNumber = info.ParentIndexNumber;

            Logger.Info("繼承上層 季: :" + info.ParentIndexNumber.ToString());

            return result;
        }

        int? GetIndexNumber(string episodeName)
        {
            if (Regex.IsMatch(episodeName, @"第[0-9].集"))
            {
                Regex r = new Regex(@"[0-9].", RegexOptions.IgnoreCase);
                Match m = r.Match(episodeName);

                if (!string.IsNullOrEmpty(m.Groups[0].ToString()))
                {
                    Logger.Info("繼承上層 集:" + m.Groups[0].ToString());
                    return int.Parse(m.Groups[0].ToString());
                }
            }

            return null;
        }

        public async Task<IEnumerable<RemoteSearchResult>> GetSearchResults(EpisodeInfo searchInfo, CancellationToken cancellationToken)
        {
            var results = new Dictionary<string, RemoteSearchResult>();
            return results.Values;
        }

        public Task<HttpResponseInfo> GetImageResponse(string url, CancellationToken cancellationToken)
        {
            return _httpClient.GetResponse(new HttpRequestOptions
            {
                CancellationToken = cancellationToken,
                Url = url
            });
        }

    }
}
