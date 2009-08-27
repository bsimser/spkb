using System.Web.UI;
using System.Web.UI.WebControls;

namespace SharePointKnowledgeBase.WebPartCode.Controls
{
    public class Trail : Control
    {
        protected override void CreateChildControls()
        {
            Controls.Add(new LiteralControl("<div class=\"spkb-trail\">"));
            Controls.Add(new HyperLink { NavigateUrl = "#", Text = "Knowledge Base Home"});
            Controls.Add(new HyperLink { NavigateUrl = "#", Text = "Glossary"});
            Controls.Add(new HyperLink { NavigateUrl = "#", Text = "Favorites"});
            Controls.Add(new HyperLink { NavigateUrl = "#", Text = "Contact"});
            Controls.Add(new HyperLink { NavigateUrl = "#", Text = "Control Panel"});
            Controls.Add(new LiteralControl("</div>"));
        }
    }
}