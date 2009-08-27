using System.Web.UI;

namespace SharePointKnowledgeBase.WebPartCode.Controls
{
    public class FeaturedArticles : Control
    {
        protected override void CreateChildControls()
        {
            Controls.Add(new LiteralControl("<div class=\"spkb-heading\">Featured Articles</div>"));
        }
    }
}