using System.Web.UI;

namespace SharePointKnowledgeBase.WebPartCode.Controls
{
    public class BrowseByCategory : Control
    {
        protected override void CreateChildControls()
        {
            Controls.Add(new LiteralControl("<div class=\"spkb-heading\">Browse by Category</div>"));
        }
    }
}