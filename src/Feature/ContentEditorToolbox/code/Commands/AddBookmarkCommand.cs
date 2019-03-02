using Feature.ContentEditorToolbox.Services;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.SecurityModel;
using Sitecore.Shell.Framework.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Feature.ContentEditorToolbox.Commands
{
    public class AddBookmarkCommand : Command
    {
        private UserActivityService service = new UserActivityService();

        public override void Execute(CommandContext context)
        {
            var contextItem = context.Items.FirstOrDefault();
            if (contextItem == null)
                return;

            if (!service.CheckUser())
                return;

            if (service.IsBookmarked(contextItem))
            {
                service.UnBookmark(contextItem);
            }
            else
            {
                service.Bookmark(contextItem);
            }
        }

        public override string GetHeader(CommandContext context, string header)
        {
            var contextItem = context.Items.FirstOrDefault();
            return service.IsBookmarked(contextItem) ? "Un-" + header : header;
        }
    }
}