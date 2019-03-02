using Feature.ContentEditorToolbox.Models;
using Feature.ContentEditorToolbox.Services;
using Sitecore.Data;
using Sitecore.Data.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Feature.ContentEditorToolbox.Repositories
{
    public class CustomItemRepository : Interfaces.ICustomRepository<GenericItemEntity>
    {
        private readonly Database database;
        private readonly Database liveDatabase;

        UserActivityService service = new UserActivityService();

        public CustomItemRepository()
        {
            this.database = Sitecore.Data.Database.GetDatabase("master");
            this.liveDatabase = Sitecore.Data.Database.GetDatabase("web");
        }

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

        public bool Exists(GenericItemEntity entity)
        {
            if (string.IsNullOrEmpty(entity.Id))
            {
                return false;
            }

            return this.database.GetItem(new ID(entity.Id)) != null;
        }

        public GenericItemEntity FindById(string id)
        {
            var sitecoreItem = this.database.GetItem(new Sitecore.Data.ID(id));
            if (sitecoreItem != null)
            {
                // what is the latest updated version by the user?
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

        public IQueryable<GenericItemEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(GenericItemEntity entity)
        {
            throw new NotImplementedException();
        }

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

        private bool HasPresentation(Sitecore.Data.Items.Item sitecoreItem)
        {
            return sitecoreItem.Visualization.Layout != null;
        }

        private bool IsLive(Sitecore.Data.Items.Item sitecoreItem)
        {
            return liveDatabase.GetItem(sitecoreItem.ID) != null;
        }

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

        private string GetWorkflowState(Sitecore.Data.Items.Item sitecoreItem)
        {
            var wf = database.WorkflowProvider.GetWorkflow(sitecoreItem);
            if (wf != null)
            {
                return wf.GetState(sitecoreItem)?.DisplayName;
            }

            return null;
        }
    }
}