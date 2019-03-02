using System.Collections.Generic;
using Sitecore.Data.Items;

namespace Feature.ContentEditorToolbox.Interfaces
{
    /// <summary>
    /// The user activity service interface
    /// </summary>
    public interface IUserActivityService
    {
        /// <summary>
        /// Bookmark the item
        /// </summary>
        /// <param name="item">The item</param>
        void Bookmark(Item item);

        /// <summary>
        /// Check has logged in Sitecore user
        /// </summary>
        /// <returns></returns>
        bool CheckUser();

        /// <summary>
        /// Gets the bookmark list as Ids from user profile
        /// </summary>
        /// <returns>The ID list</returns>
        IEnumerable<string> GetBookmarkList();

        /// <summary>
        /// Gets the recent updates from history engine
        /// </summary>
        /// <param name="days">The number of days</param>
        /// <returns>The updated items</returns>
        IEnumerable<string> GetChangesFromHistory(int days);

        /// <summary>
        /// Gets the recent updates
        /// </summary>
        /// <param name="days">The number of days</param>
        /// <returns>The updated items</returns>
        IEnumerable<string> GetMyLockedItems();

        /// <summary>
        /// Gets the recent updates
        /// </summary>
        /// <param name="days">The number of days</param>
        /// <returns>The updated items</returns>
        IEnumerable<string> GetRecentUpdates(int days);

        /// <summary>
        /// Gets the logged in user full name
        /// </summary>
        /// <returns>The user name</returns>
        string GetUserFullName();

        /// <summary>
        /// Gets the logged in user name
        /// </summary>
        /// <returns>The user name</returns>
        string GetUserName();

        /// <summary>
        /// Check whether the item is bookmarked
        /// </summary>
        /// <param name="item">The item</param>
        /// <returns>Is bookmarked</returns>
        bool IsBookmarked(Item item);

        /// <summary>
        /// Revert the item bookmark
        /// </summary>
        /// <param name="item">The item</param>
        void UnBookmark(Item item);
    }
}