using System.Web.UI;

namespace SharePointKnowledgeBase.WebPartCode.Controls
{
    public class Search : Control
    {
        protected override void CreateChildControls()
        {
            Controls.Add(new LiteralControl("<div class=\"spkb-heading\">Search the Knowledge Base</div>"));
        }
    }
}