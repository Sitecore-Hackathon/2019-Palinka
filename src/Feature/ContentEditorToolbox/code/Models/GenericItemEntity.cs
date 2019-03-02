using Sitecore.Services.Core;

namespace Feature.ContentEditorToolbox.Models
{
    public class GenericItemEntity : Sitecore.Services.Core.Model.EntityIdentity
    {
        public string Name { get; set; }

        public string Path { get; set; }
        public string[] Languages { get; set; }
        public string TemplateName { get; set; }
        public string WorkflowState { get; set; }

        public bool HasPresentation { get; set; }

        public string IconPath { get; set; }

        public bool IsPublished { get; set; }
    }
}