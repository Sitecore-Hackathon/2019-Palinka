using Sitecore.Services.Core;
using Sitecore.Services.Core.Model;
using System.Collections.Generic;

namespace Feature.ContentEditorToolbox.Interfaces
{
    public interface ICustomRepository<T> : Sitecore.Services.Core.IRepository<T> where T : IEntityIdentity
    {
        IEnumerable<T> GetBookmarks();

        IEnumerable<T> GetRecentModifications();
    }
}
