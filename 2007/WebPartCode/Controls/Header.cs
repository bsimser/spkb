using System.Web.UI;

namespace SharePointKnowledgeBase.WebPartCode.Controls
{
    public class Header : Control
    {
        protected override void CreateChildControls()
        {
            Controls.Add(new LiteralControl("SharePoint Knowledge Base"));
        }        
    }
}