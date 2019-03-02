using Feature.ContentEditorToolbox.Interfaces;
using Sitecore.Configuration;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data.Managers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Feature.ContentEditorToolbox.Services
{
    /// <summary>
    /// The user activity service
    /// </summary>
    public class UserActivityService : IUserActivityService
    {
        /// <summary>
        /// The bookmark list profile field
        /// </summary>
        private string profileFieldName = "BookmarkList";

        /// <summary>
        /// The content root path - used for retrieving the relevant index
        /// </summary>
        private string contentRootPath = "/sitecore/content";

        /// <summary>
        /// The maximum item count in the result
        /// </summary>
        private int maxItemCount = 60;

        /// <summary>
        /// The default master index name
        /// </summary>
        private string defaultMasterIndex = "sitecore_master_index";

        /// <summary>
        /// Check has logged in Sitecore user
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the logged in user name
        /// </summary>
        /// <returns>The user name</returns>
        public string GetUserName()
        {
            if (!this.CheckUser())
            {
                return null;
            }

            return Sitecore.Context.User.LocalName;
        }

        /// <summary>
        /// Gets the logged in user full name
        /// </summary>
        /// <returns>The user name</returns>
        public string GetUserFullName()
        {
            if (!this.CheckUser())
            {
                return null;
            }

            return Sitecore.Context.User.Name;
        }

        /// <summary>
        /// Gets the bookmark list as Ids from user profile
        /// </summary>
        /// <returns>The ID list</returns>
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
        /// <param name="item">The item</param>
        /// <returns>Is bookmarked</returns>
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

        /// <summary>
        /// Gets the recent updates
        /// </summary>
        /// <param name="days">The number of days</param>
        /// <returns>The updated items</returns>
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

        /// <summary>
        /// Gets the recent updates
        /// </summary>
        /// <param name="days">The number of days</param>
        /// <returns>The updated items</returns>
        public IEnumerable<string> GetMyLockedItems()
        {
            var index = GetSearchIndex();
            string userName = $"sitecore{GetUserName()}".ToLower();

            using (var context = index.CreateSearchContext())
            {
                var results = context.GetQueryable<SearchResultItem>()
                                      .Where(item => item.LockOwner.Equals(userName))
                                      .OrderBy(item => item.Updated)
                                      .Take(maxItemCount)
                                      .ToArray();

                return results.Select(t => t.ItemId.ToString()).Distinct();
            }
        }

        /// <summary>
        /// Gets the recent updates from history engine
        /// </summary>
        /// <param name="days">The number of days</param>
        /// <returns>The updated items</returns>
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

        /// <summary>
        /// Gets the master search index
        /// </summary>
        /// <returns>The master search index</returns>
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