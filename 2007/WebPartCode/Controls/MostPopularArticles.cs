using System.Web.UI;

namespace SharePointKnowledgeBase.WebPartCode.Controls
{
    public class MostPopularArticles : Control
    {
        protected override void CreateChildControls()
        {
            Controls.Add(new LiteralControl("<div class=\"spkb-heading\">Most Popular Articles</div>"));
        }
    }
}