using System.Web.UI;

namespace SharePointKnowledgeBase.WebPartCode.Controls
{
    public abstract class BaseControl : Control, INamingContainer
    {
        public SharePointKnowledgeBaseWebPart WebPartParent { get; set; }

        protected override void Render(HtmlTextWriter writer)
        {
            new Header().RenderControl(writer);

            foreach (Control control in Controls)
            {
                control.RenderControl(writer);
            }

            new Footer().RenderControl(writer);
        }
    }
}