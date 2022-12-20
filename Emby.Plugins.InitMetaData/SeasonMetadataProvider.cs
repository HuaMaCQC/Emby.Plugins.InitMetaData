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
            string newVal = name.Split('(')[0].Trim(' ');
            string[] names = newVal.Split(' ');
            int index = 0;

            // 判斷第一位置
            if(names.Length > 0)
            {
                index = GetFirstIndex(names[0]);
            }

            // 判斷第二位置
            if(names.Length > 1)
            {
                // 第一位置是數字
                if(index > 0 && index < 100)
                {
                    int i = GetSecondIndex(names[1]);

                    if(i > 0 && index > 10)
                    {
                        index = index * 10 + i;
                    }
                    else if(i > 0 && index < 10)
                    {
                        index = index * 100 + i;
                    }
                }
                // 第一位置是代號
                else if(index > 100)
                {
                    int i = GetThirdIndex(names[1]);

                    index = i > 0 ? index * 10 + i : index;
                }
            }

            // 判斷第三位置
            if (names.Length > 2 && index > 100)
            {
                int i = GetThirdIndex(names[2]);

                index = i > 0 ? index * 10 + i : index;
            }

            if(index > 0)
            {
                return index;
            }

            return null;
        }

        int GetSecondIndex(string name)
        {
            if (Regex.IsMatch(name, @"^ova", RegexOptions.IgnoreCase))
            {
                return 1;
            }
            else if (Regex.IsMatch(name, @"^oad", RegexOptions.IgnoreCase))
            {
                return 2;
            }
            else if (
               Regex.IsMatch(name, @"^MOVIE", RegexOptions.IgnoreCase)
               || Regex.IsMatch(name, @"^劇場", RegexOptions.IgnoreCase)
               || Regex.IsMatch(name, @"^電影", RegexOptions.IgnoreCase)
            )
            {
                return 3;
            }
            else if (Regex.IsMatch(name, @"^extra", RegexOptions.IgnoreCase))
            {
                return 4;
            }
            else if (Regex.IsMatch(name, @"^sp", RegexOptions.IgnoreCase) || Regex.IsMatch(name, @"^特別", RegexOptions.IgnoreCase))
            {
                return 9;
            }

            return 0;

        }

        int GetThirdIndex(string name)
        {
            if (Regex.IsMatch(name, @"^[0-9]{1,2}"))
            {
                Regex r = new Regex(@"^[0-9]{1,2}");
                string m = r.Match(name).Groups[0].ToString();

                if (!string.IsNullOrEmpty(m))
                {
                    int index = int.Parse(m);

                    if (index > 0)
                    {
                        return index;
                    }
                }
            } 
            else if (Regex.IsMatch(name, @"^sp", RegexOptions.IgnoreCase) || Regex.IsMatch(name, @"^特別", RegexOptions.IgnoreCase))
            {
                return 9;
            }

            return 0;
        }

        int GetFirstIndex(string name)
        {
            if (Regex.IsMatch(name, @"^[0-9]{1,2}"))
            {
                Regex r = new Regex(@"^[0-9]{1,2}");
                string m = r.Match(name).Groups[0].ToString();

                if (!string.IsNullOrEmpty(m))
                {
                    int index = int.Parse(m);

                    if (index > 0)
                    {
                        return index;
                    }
                }
            }
            else if (Regex.IsMatch(name, @"^ova", RegexOptions.IgnoreCase))
            {
                return 101;
            }
            else if (Regex.IsMatch(name, @"^oad", RegexOptions.IgnoreCase))
            {
                return 102;
            }
            else if (
                Regex.IsMatch(name, @"^MOVIE", RegexOptions.IgnoreCase)
                || Regex.IsMatch(name, @"^劇場", RegexOptions.IgnoreCase)
                || Regex.IsMatch(name, @"^電影", RegexOptions.IgnoreCase)
            )
            {
                return 103;
            }
            else if (Regex.IsMatch(name, @"^extra", RegexOptions.IgnoreCase))
            {
                return 104;
            }
            else if (Regex.IsMatch(name, @"^sp", RegexOptions.IgnoreCase) || Regex.IsMatch(name, @"^特別", RegexOptions.IgnoreCase))
            {
                return 109;
            }

            return 0;
        }
    }
}
