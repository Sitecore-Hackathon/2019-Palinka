using Feature.ContentEditorToolbox.Interfaces;
using Feature.ContentEditorToolbox.Models;
using Feature.ContentEditorToolbox.Services;
using Sitecore.Data;
using Sitecore.Data.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Feature.ContentEditorToolbox.Repositories
{
    /// <summary>
    /// The custom item repository
    /// </summary>
    public class CustomItemRepository : Interfaces.ICustomRepository<GenericItemEntity>
    {
        /// <summary>
        /// The master database
        /// </summary>
        private readonly Database database;

        /// <summary>
        /// The live database
        /// </summary>
        private readonly Database liveDatabase;

        /// <summary>
        /// The user activity service
        /// </summary>
        private readonly IUserActivityService service;

        /// <summary>
        /// Intializes a new custom item repository
        /// </summary>
        public CustomItemRepository()
        {
            this.database = Sitecore.Data.Database.GetDatabase("master");
            this.liveDatabase = Sitecore.Data.Database.GetDatabase("web");
            this.service = new UserActivityService();
        }

        /// <summary>
        /// Bookmark new item
        /// </summary>
        /// <param name="entity">The item</param>
        public void Add(GenericItemEntity entity)
        {
            if (string.IsNullOrEmpty(entity.Id))
            {
                return;
            }

            var item = this.database.GetItem(entity.Id);
            if (item != null)
            {
                service.Bookmark(item);
            }
        }

        /// <summary>
        /// Remove (un-bookmark) the item
        /// </summary>
        /// <param name="entity">The item</param>
        public void Delete(GenericItemEntity entity)
        {
            if (string.IsNullOrEmpty(entity.Id))
            {
                return;
            }

            var item = this.database.GetItem(entity.Id);
            if (item != null)
            {
                service.UnBookmark(item);
            }
        }

        /// <summary>
        /// Check whether the item is exists (based on ID)
        /// </summary>
        /// <param name="entity">The item</param>
        /// <returns>Is exists</returns>
        public bool Exists(GenericItemEntity entity)
        {
            if (string.IsNullOrEmpty(entity.Id))
            {
                return false;
            }

            return this.database.GetItem(new ID(entity.Id)) != null;
        }

        /// <summary>
        /// Find the item by ID
        /// </summary>
        /// <param name="id">The id</param>
        /// <returns>The entity</returns>
        public GenericItemEntity FindById(string id)
        {
            var sitecoreItem = this.database.GetItem(new Sitecore.Data.ID(id));
            if (sitecoreItem != null)
            {
                // gets the latest updated version of the item
                var versions = GetLanguageVersions(sitecoreItem);
                var finalVersion = versions.OrderBy(t => t.Statistics.Updated).Last();

                GenericItemEntity item = new GenericItemEntity
                {
                    Name = sitecoreItem.Name,
                    Id = sitecoreItem.ID.ToString(),
                    Languages = versions.Select(t => t.Language.Name).ToArray(),
                    Path = sitecoreItem.Paths.FullPath,
                    TemplateName = sitecoreItem.TemplateName,
                    IconPath = GetIcon(sitecoreItem),
                    HasPresentation = HasPresentation(finalVersion),
                    WorkflowState = GetWorkflowState(finalVersion),
                    IsPublished = IsLive(sitecoreItem),
                    Updated = finalVersion.Statistics.Updated.ToString("yyyy-MM-dd HH:mm")
                };

                return item;
            }

            return null;
        }

        /// <summary>
        /// Get all items (not implemented)
        /// </summary>
        /// <returns>Throws exception</returns>
        public IQueryable<GenericItemEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates the item (not implemented)
        /// </summary>
        /// <returns>Throws exception</returns>
        public void Update(GenericItemEntity entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get the user bookmarks
        /// </summary>
        /// <returns>The bookmarked pages</returns>
        public IEnumerable<GenericItemEntity> GetBookmarks()
        {
            List<GenericItemEntity> entities = new List<GenericItemEntity>();

            var bookmarkIdList = service.GetBookmarkList();
            foreach (var id in bookmarkIdList)
            {
                var item = FindById(id);
                if (item != null)
                {
                    entities.Add(item);
                }
            }

            return entities;
        }

        /// <summary>
        /// Gets the recent modification
        /// </summary>
        /// <returns>The modified pages</returns>
        public IEnumerable<GenericItemEntity> GetRecentModifications()
        {
            List<GenericItemEntity> entities = new List<GenericItemEntity>();

            var bookmarkIdList = service.GetRecentUpdates(90);
            foreach (var id in bookmarkIdList)
            {
                var item = FindById(id);
                if (item != null)
                {
                    entities.Add(item);
                }
            }

            return entities;
        }

        /// <summary>
        /// Get the user bookmarks
        /// </summary>
        /// <returns>The bookmarked pages</returns>
        public IEnumerable<GenericItemEntity> GetMyLockedItems()
        {
            List<GenericItemEntity> entities = new List<GenericItemEntity>();

            var bookmarkIdList = service.GetMyLockedItems();
            foreach (var id in bookmarkIdList)
            {
                var item = FindById(id);
                if (item != null)
                {
                    entities.Add(item);
                }
            }

            return entities;
        }

        /// <summary>
        /// Unlock the item
        /// </summary>
        /// <param name="entity">The item</param>
        public void Unlock(GenericItemEntity entity)
        {
            if (string.IsNullOrEmpty(entity.Id))
            {
                return;
            }

            var item = this.database.GetItem(entity.Id);
            if (item != null)
            {
                var versions = GetLanguageVersions(item);
                foreach (var version in versions)
                {
                    if (version.Locking.IsLocked() && version.Access.CanWrite())
                    {
                        version.Locking.Unlock();
                    }
                }
            }
        }

        /// <summary>
        /// Publish the item
        /// </summary>
        /// <param name="entity">The item</param>
        public void Publish(GenericItemEntity entity)
        {
            if (string.IsNullOrEmpty(entity.Id))
            {
                return;
            }

            var item = this.database.GetItem(entity.Id);
            var targets = new Database[] { liveDatabase };

            Sitecore.Publishing.PublishManager.PublishItem(item, targets, item.Languages, false, false, false);
        }

        /// <summary>
        /// Gets the icon of the item
        /// </summary>
        /// <param name="sitecoreItem">The item</param>
        /// <returns>The icon path</returns>
        private string GetIcon(Sitecore.Data.Items.Item sitecoreItem)
        {
            string iconImageRaw = ThemeManager.GetIconImage(sitecoreItem, 32, 32, "", "");
            if (!string.IsNullOrWhiteSpace(iconImageRaw) && iconImageRaw.Contains("src="))
            {
                int i0 = iconImageRaw.IndexOf("src=");
                int i1 = iconImageRaw.IndexOf('"', i0 + 1);
                if (i1 < 0)
                {
                    return null;
                }

                int i2 = iconImageRaw.IndexOf('"', i1 + 1);
                if (i2 < 0)
                {
                    return null;
                }

                return iconImageRaw.Substring(i1, i2 - i1).Trim(' ', '"', '\\');
            }

            return null;
        }

        /// <summary>
        /// Check whether has prenetation
        /// </summary>
        /// <param name="sitecoreItem">The item</param>
        /// <returns>Has presentation</returns>
        private bool HasPresentation(Sitecore.Data.Items.Item sitecoreItem)
        {
            return sitecoreItem.Visualization.Layout != null;
        }

        /// <summary>
        /// Check whether it is published
        /// </summary>
        /// <param name="sitecoreItem">The item</param>
        /// <returns>It is published</returns>
        private bool IsLive(Sitecore.Data.Items.Item sitecoreItem)
        {
            return liveDatabase.GetItem(sitecoreItem.ID) != null;
        }

        /// <summary>
        /// Collect the latest language versions of the item
        /// </summary>
        /// <param name="sitecoreItem">The item</param>
        /// <returns>The language versions</returns>
        private Sitecore.Data.Items.Item[] GetLanguageVersions(Sitecore.Data.Items.Item sitecoreItem)
        {
            var languages = sitecoreItem.Languages;
            List<Sitecore.Data.Items.Item> versions = new List<Sitecore.Data.Items.Item>();
            foreach (var language in languages)
            {
                var finalVersion = sitecoreItem.Versions.GetLatestVersion(language);
                if (finalVersion.Statistics.Created > DateTime.MinValue)
                {
                    versions.Add(finalVersion);
                }
            }

            if (versions.Count == 0)
            {
                versions.Add(sitecoreItem);
            }

            return versions.ToArray();
        }

        /// <summary>
        /// Get the workflow state
        /// </summary>
        /// <param name="sitecoreItem">The item</param>
        /// <returns>The workflow state</returns>
        private string GetWorkflowState(Sitecore.Data.Items.Item sitecoreItem)
        {
            var wf = database.WorkflowProvider.GetWorkflow(sitecoreItem);
            if (wf != null)
            {
                return $"{wf.GetState(sitecoreItem)?.DisplayName} ({sitecoreItem.Language.Name})";
            }

            return "-";
        }
    }
}