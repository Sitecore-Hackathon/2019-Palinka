using Feature.ContentEditorToolbox.Interfaces;
using Feature.ContentEditorToolbox.Models;
using Feature.ContentEditorToolbox.Repositories;
using Sitecore.Services.Core;
using Sitecore.Services.Infrastructure.Sitecore.Services;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Feature.ContentEditorToolbox.Controllers
{
    [ServicesController]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EditorToolboxController : EntityService<GenericItemEntity>
    {
        /// <summary>
        /// The custom repository
        /// </summary>
        private ICustomRepository<GenericItemEntity> customRepositoryActions;

        /// <summary>
        /// Initializes a new instance
        /// </summary>
        /// <param name="repository">The repository</param>
        public EditorToolboxController(ICustomRepository<GenericItemEntity> repository)
            : base(repository)
        {
            customRepositoryActions = repository;
        }

        /// <summary>
        /// Initializes a new instance
        /// </summary>
        public EditorToolboxController()
            : base(new CustomItemRepository())
        {
        }

        /// <summary>
        /// Gets the bookmarks of the user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetBookmarks")]
        public IEnumerable<GenericItemEntity> GetBookmarks()
        {
            return customRepositoryActions.GetBookmarks();
        }


        /// <summary>
        /// Gets the recent modification of the user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetRecentModifications")]
        public IEnumerable<GenericItemEntity> GetRecentModifications()
        {
            return customRepositoryActions.GetRecentModifications();
        }

        /// <summary>
        /// Gets the locked items of the user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetMyLockedItems")]
        public IEnumerable<GenericItemEntity> GetMyLockedItems()
        {
            return customRepositoryActions.GetMyLockedItems();
        }

        /// <summary>
        /// Unlock the specified item
        /// </summary>
        /// <param name="entity">The item</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("UnLock")]
        public HttpResponseMessage UnLock([FromBody]GenericItemEntity entity)
        {
            customRepositoryActions.Unlock(entity);
            return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        }

        /// <summary>
        /// Unlock all locked item of the user
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("UnLockAll")]
        public HttpResponseMessage UnLockAll()
        {
            var lockedItems = customRepositoryActions.GetMyLockedItems();
            foreach(var lockedItem in lockedItems)
            {
                customRepositoryActions.Unlock(lockedItem);
            }

            return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        }

        /// <summary>
        /// Publish the specified item
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("PublishItem")]
        public HttpResponseMessage PublishItem([FromBody]GenericItemEntity entity)
        {
            customRepositoryActions.Publish(entity);
            return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        }

        /// <summary>
        /// Health check
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("IsOk")]
        ///sitecore/api/ssc/Feature-ContentEditorToolbox-Controllers/EditorToolbox/-/IsOk
        public string IsOk()
        {
            return "Ok";
        }
    }
}