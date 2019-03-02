using Feature.ContentEditorToolbox.Interfaces;
using Feature.ContentEditorToolbox.Services;
using Sitecore.Shell.Framework.Commands;
using System.Linq;

namespace Feature.ContentEditorToolbox.Commands
{
    /// <summary>
    /// The add bookmark command
    /// </summary>
    public class AddBookmarkCommand : Command
    {
        /// <summary>
        /// The user activity service
        /// </summary>
        private IUserActivityService service = new UserActivityService();

        /// <summary>
        /// Execute the command - bookmarkd or un-bookmark the page
        /// </summary>
        /// <param name="context">The context</param>
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

        /// <summary>
        /// Gets the command text
        /// </summary>
        /// <param name="context"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        public override string GetHeader(CommandContext context, string header)
        {
            var contextItem = context.Items.FirstOrDefault();
            return service.IsBookmarked(contextItem) ? "Un-" + header : header;
        }
    }
}