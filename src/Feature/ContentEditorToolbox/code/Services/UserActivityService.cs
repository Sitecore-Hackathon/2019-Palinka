using Sitecore.Configuration;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data.Managers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Feature.ContentEditorToolbox.Services
{
    public class UserActivityService
    {
        private string profileFieldName = "BookmarkList";

        private string contentRootPath = "/sitecore/content";

        private int maxItemCount = 100;

        private string defaultMasterIndex = "sitecore_master_index";

        public bool CheckUser()
        {
            if (Sitecore.Context.User != null &&
                Sitecore.Context.User.IsAuthenticated &&
                Sitecore.Context.User.GetDomainName() == "sitecore")
            {
                return true;
            }

            return false;
        }

        public string GetUserName()
        {
            if (!CheckUser())
            {
                return null;
            }

            return Sitecore.Context.User.LocalName;
        }

        public string GetUserFullName()
        {
            if (!CheckUser())
            {
                return null;
            }

            return Sitecore.Context.User.Name;
        }

        public IEnumerable<string> GetBookmarkList()
        {
            var profile = Sitecore.Context.User.Profile;
            string raw = profile.GetCustomProperty(profileFieldName) ?? string.Empty;
            if (string.IsNullOrWhiteSpace(raw))
            {
                return new string[0];
            }

            return raw.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Check whether the item is bookmarked
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool IsBookmarked(Sitecore.Data.Items.Item item)
        {
            var profile = Sitecore.Context.User.Profile;
            string raw = profile.GetCustomProperty(profileFieldName);

            return raw.Contains(item.ID.ToString());
        }

        /// <summary>
        /// Bookmark the item
        /// </summary>
        /// <param name="item">The item</param>
        public void Bookmark(Sitecore.Data.Items.Item item)
        {
            if (item == null)
                return;

            var profile = Sitecore.Context.User.Profile;
            var list = GetBookmarkList().ToList();
            if (!list.Contains(item.ID.ToString()))
            {
                list.Add(item.ID.ToString());
                profile.SetCustomProperty(profileFieldName, string.Join("|", list));
                profile.Save();
            }
        }

        /// <summary>
        /// Revert the item bookmark
        /// </summary>
        /// <param name="item">The item</param>
        public void UnBookmark(Sitecore.Data.Items.Item item)
        {
            if (item == null)
                return;

            var profile = Sitecore.Context.User.Profile;
            var list = GetBookmarkList().ToList();
            if (list.Contains(item.ID.ToString()))
            {
                list.Remove(item.ID.ToString());
                profile.SetCustomProperty(profileFieldName, string.Join("|", list));
                profile.Save();
            }
        }

        public IEnumerable<string> GetRecentUpdates(int days)
        {
            var index = GetSearchIndex();
            DateTime dateTime = DateTime.Today.AddDays(-days);
            string userName = $"sitecore{GetUserName()}".ToLower();

            using (var context = index.CreateSearchContext())
            {
                var results = context.GetQueryable<SearchResultItem>()
                                      .Where(item => item.Updated > dateTime && item.UpdatedBy.Equals(userName))
                                      .OrderByDescending(item => item.Updated)
                                      .Take(maxItemCount)
                                      .ToArray();

                return results.Select(t => t.ItemId.ToString()).Distinct();
            }
        }

        public IEnumerable<string> GetChangesFromHistory(int days)
        {
            var database = Factory.GetDatabase("master");
            var records = HistoryManager.GetHistory(database, DateTime.Today.AddDays(-7), DateTime.Now);

            return records
                .Where(t => string.Equals(t.UserName, Sitecore.Context.User.Name))
                .OrderByDescending(t => t.Created)
                .Select(t => t.ItemId.ToString())
                .ToArray();
        }

        private ISearchIndex GetSearchIndex()
        {
            var database = Factory.GetDatabase("master");
            var contentItem = database.GetItem(contentRootPath);

            SitecoreIndexableItem indexableItem = new SitecoreIndexableItem(contentItem);
            var index = ContentSearchManager.GetIndex(indexableItem);
            if (index == null)
            {
                index = ContentSearchManager.GetIndex(defaultMasterIndex);
            }

            return index;
        }
    }
}