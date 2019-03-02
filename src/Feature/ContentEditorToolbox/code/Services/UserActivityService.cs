using Sitecore.Configuration;
using Sitecore.Data.Managers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Feature.ContentEditorToolbox.Services
{
    public class UserActivityService
    {
        private string profileFieldName = "BookmarkList";

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

        public bool IsBookmarked(Sitecore.Data.Items.Item item)
        {
            var profile = Sitecore.Context.User.Profile;
            string raw = profile.GetCustomProperty(profileFieldName);

            return raw.Contains(item.ID.ToString());
        }

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

        public IEnumerable<string> GetRecentActivities()
        {
            var database = Factory.GetDatabase("master");
            var records = HistoryManager.GetHistory(database, DateTime.Today.AddDays(-7), DateTime.Now);

            return records
                .Where(t => string.Equals(t.UserName, Sitecore.Context.User.Name))
                .OrderByDescending(t => t.Created)
                .Select(t => t.ItemId.ToString())
                .ToArray();
        }
    }
}