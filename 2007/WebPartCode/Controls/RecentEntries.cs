using System.Web.UI;

namespace SharePointKnowledgeBase.WebPartCode.Controls
{
    public class RecentEntries : Control
    {
        protected override void CreateChildControls()
        {
            Controls.Add(new LiteralControl("<div class=\"spkb-heading\">Recent Entries</div>"));
        }
    }
}