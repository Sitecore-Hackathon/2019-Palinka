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
        private ICustomRepository<GenericItemEntity> _customRepositoryActions;

        public EditorToolboxController(ICustomRepository<GenericItemEntity> repository)
            : base(repository)
        {
            _customRepositoryActions = repository;
        }

        public EditorToolboxController()
            : this(new CustomItemRepository())
        {
        }

        [HttpGet]
        [ActionName("GetBookmarks")]
        public IEnumerable<GenericItemEntity> GetBookmarks()
        {
            return _customRepositoryActions.GetBookmarks();
        }

        [HttpGet]
        [ActionName("GetRecentModifications")]
        public IEnumerable<GenericItemEntity> GetRecentModifications()
        {
            return _customRepositoryActions.GetRecentModifications();
        }

        [HttpGet]
        [ActionName("GetMyLockedItems")]
        public IEnumerable<GenericItemEntity> GetMyLockedItems()
        {
            return _customRepositoryActions.GetMyLockedItems();
        }

        [HttpPost]
        [ActionName("UnLock")]
        public HttpResponseMessage UnLock([FromBody]GenericItemEntity entity)
        {
            _customRepositoryActions.Unlock(entity);
            return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        }

        [HttpGet]
        [ActionName("IsOk")]
        ///sitecore/api/ssc/Feature-ContentEditorToolbox-Controllers/EditorToolbox/-/IsOk
        public string IsOk()
        {
            return "Ok";
        }
    }
}