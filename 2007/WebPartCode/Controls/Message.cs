using System;
using System.Web.UI;

namespace SharePointKnowledgeBase.WebPartCode.Controls
{
    public class Message : Control
    {
        protected override void CreateChildControls()
        {
            Controls.Add(new LiteralControl("<div class=\"spkb-message\">Welcome to our knowledge base. To find what you're after, use the search box  below or choose a category to view listed articles.</div>"));
        }
    }
}