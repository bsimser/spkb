using System;
using System.Reflection;
using System.Text;
using System.Web.UI;
using SharePointKnowledgeBase.WebPartCode.Application;

namespace SharePointKnowledgeBase.WebPartCode.Controls
{
    public class Footer : Control
    {
        protected override void CreateChildControls()
        {
            var footer = new StringBuilder();

            footer.AppendFormat("<p style=\"text-align:center;font-size:7pt\">");

            footer.AppendFormat("Powered by {0} version {1}",
                                String.Format(
                                    "<a title=\"SharePoint Knowledge Base Home Page\" href=\"{0}\">SharePoint Knowledge Base</a>",
                                    ForumConstants.ConfigAuthorWebSite),
                                String.Format("{0} - {1}",
                                              Assembly.GetExecutingAssembly().GetName().Version,
                                              DateTime.Now.ToShortDateString()));

            footer.AppendFormat(
                String.Format("<br/>Copyright &copy; {0} by <a href=\"mailto:{1}\">{2}</a>. All rights reserved.",
                              DateTime.Now.Year,
                              ForumConstants.ConfigAuthorEmail,
                              ForumConstants.ConfigAuthorName)
                );

            footer.AppendFormat("<br/>");

//            _forumTimer.Stop();
//            footer.AppendFormat("This page was generated in {0:N3} seconds.", _forumTimer.Duration);

//            displayDebugUserInformation(footer);

            footer.AppendFormat("</p>");
            
            Controls.Add(new LiteralControl(footer.ToString()));
        }
    }
}