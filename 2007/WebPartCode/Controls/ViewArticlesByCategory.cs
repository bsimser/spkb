using System.Web.UI;

namespace SharePointKnowledgeBase.WebPartCode.Controls
{
    public class ViewArticlesByCategory : Control
    {
        protected override void CreateChildControls()
        {
            Controls.Add(new LiteralControl("<div class=\"spkb-heading\"><img src=\"/_layouts/images/FOLDER.GIF\"/> View Articles by Category <img src=\"/_layouts/images/rss.gif\"/></div>"));
        }
    }
}