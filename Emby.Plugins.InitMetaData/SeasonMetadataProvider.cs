using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Configuration;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Library;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Controller.Resolvers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Globalization;
using MediaBrowser.Model.IO;
using MediaBrowser.Model.Logging;
using MediaBrowser.Model.Providers;
using MediaBrowser.Model.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Emby.Plugins.InitMetaData
{
    public class SeasonMetadataProvider: IRemoteMetadataProvider<Season, SeasonInfo>
    {
        public readonly ILogger Logger;
        private readonly IHttpClient _httpClient;
        private readonly ILibraryManager _libraryManager;
        public string Name => ProviderNames.Name;

        public SeasonMetadataProvider(ILibraryManager libraryManager, IHttpClient httpClient, ILogManager logManager)
        {
            _httpClient = httpClient;
            _libraryManager = libraryManager;
            Logger = logManager.GetLogger(Name);
        }

        public Task<HttpResponseInfo> GetImageResponse(string url, CancellationToken cancellationToken)
        {
            return _httpClient.GetResponse(new HttpRequestOptions
            {
                CancellationToken = cancellationToken,
                Url = url
            });
        }

        public async Task<MetadataResult<Season>> GetMetadata(SeasonInfo info, CancellationToken cancellationToken)
        {
            var result = new MetadataResult<Season>();
            
            Logger.Info("2 Name :" + info.Name);

            result.Item = new Season();

            result.HasMetadata = true;
            result.Item.Genres = new string[] { "風格一" };
            //result.Item.Tags = new string[] { "Tag一" };
            return result;
        }

        public async Task<IEnumerable<RemoteSearchResult>> GetSearchResults(SeasonInfo searchInfo, CancellationToken cancellationToken)
        {
            var results = new Dictionary<string, RemoteSearchResult>();
            Logger.Info("1" + searchInfo.Name);

            return results.Values;
        }
    }
}