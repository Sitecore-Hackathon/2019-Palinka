using Sitecore.Services.Core;
using System.Collections.Generic;

namespace Feature.ContentEditorToolbox.Interfaces
{
    /// <summary>
    /// Interface extension for the custom repository
    /// </summary>
    /// <typeparam name="T">The entity item type</typeparam>
    public interface ICustomRepository<T> : Sitecore.Services.Core.IRepository<T> where T : IEntityIdentity
    {
        /// <summary>
        /// Get the user bookmarks
        /// </summary>
        /// <returns>The bookmarked pages</returns>
        IEnumerable<T> GetBookmarks();

        /// <summary>
        /// Gets the recent modification
        /// </summary>
        /// <returns>The modified pages</returns>
        IEnumerable<T> GetRecentModifications();

        /// <summary>
        /// Get the user bookmarks
        /// </summary>
        /// <returns>The bookmarked pages</returns>
        IEnumerable<T> GetMyLockedItems();

        /// <summary>
        /// Unlock the item
        /// </summary>
        /// <param name="entity">The item</param>
        void Unlock(T entity);
    }
}
