using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;
using SharePointKnowledgeBase.Application;
using SharePointKnowledgeBase.Controls;
using SharePointKnowledgeBase.WebPartCode.Application;
using SharePointKnowledgeBase.WebPartCode.Controls;
using WebPart=Microsoft.SharePoint.WebPartPages.WebPart;

namespace SharePointKnowledgeBase
{
    [Guid("9ad44ade-e84a-4729-8738-ea21cc1c2692")]
    public class SharePointKnowledgeBaseWebPart : WebPart
    {
        private const string LanguageFilter = "*.lng.xml";
        private const string DefaultLanguage = "1033";
        private string _currentLanguage = DefaultLanguage;
        private XmlDocument _resourceFile;
        private bool _error;

        public SharePointKnowledgeBaseWebPart()
        {
            setupExportMode();
        }

        private void setupExportMode()
        {
            ExportMode = WebPartExportMode.All;
        }

        protected override void CreateChildControls()
        {
            if (!_error)
            {
                var webId = SPContext.Current.Web.ID;
                var siteId = SPContext.Current.Site.ID;

                using (Identity.ImpersonateAppPool())
                {
                    using (var site = new SPSite(siteId))
                    {
                        using (site.OpenWeb(webId))
                        {
                            if (featureNotActivated()) return;

                            renderCssHeader();

                            SharePointForumControls childControl;

                            try
                            {
                                base.CreateChildControls();
                                childControl = (SharePointForumControls)Enum.Parse(typeof (SharePointForumControls), Page.Request.QueryString["control"], true);
                            }
                            catch
                            {
                                childControl = SharePointForumControls.Home;
                            }

                            var control = createDynamicControl(childControl);

                            if (control != null)
                            {
                                Controls.Add(control);
                            }
                        }
                    }
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!_error)
            {
                try
                {
                    base.OnLoad(e);
                    EnsureChildControls();
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
        }

        private bool featureNotActivated()
        {
            try
            {
//                var list = ForumApplication.Instance.SpWeb.Lists[ForumConstants.ListsPosts];
            }
            catch (Exception)
            {
                Controls.Add(new LiteralControl("Sorry, this Feature is not yet activated correctly. Please contact the site administrator."));
                return true;
            }

            return false;
        }

        private void renderCssHeader()
        {
            var cssFileUrl = string.Format("{0}/styles.css", getResourceDirectory());
            Controls.Add(
                new LiteralControl(string.Format("<link rel=\"stylesheet\" type=\"text/css\" href=\"{0}\"/>",
                                                 cssFileUrl)));
        }

        protected override void OnInit(EventArgs e)
        {
            var webId = SPContext.Current.Web.ID;
            var siteId = SPContext.Current.Site.ID;
//            ForumApplication.Instance.SpUser = SPContext.Current.Web.CurrentUser;

            using (Identity.ImpersonateAppPool())
            {
                using (var site = new SPSite(siteId))
                {
                    using (var web = site.OpenWeb(webId))
                    {
                        loadResourceFileStrings();
                        initializeApplication(web);
                    }
                }
            }
        }

        private void initializeApplication(SPWeb web)
        {
            var query = new UrlQuery(Page.Request.Url.ToString());
//            ForumApplication.Instance.BasePath = SPEncode.UrlEncodeAsUrl(query.Url);
//            ForumApplication.Instance.Title = Name;
//            ForumApplication.Instance.ForumCache = Page.Cache;
//            ForumApplication.Instance.ClassResourcePath = ClassResourcePath;
//            ForumApplication.Instance.SpWeb = web;
        }

        private void loadResourceFileStrings()
        {
            try
            {
                var resources = getResourceDirectory();
                var directoryInfo = new DirectoryInfo(Page.Server.MapPath(resources));
                var languageFileInfoArray = directoryInfo.GetFiles(LanguageFilter);

                SPWeb web;
                for (var n = 0; n < languageFileInfoArray.Length; n++)
                {
                    var fileInfo = languageFileInfoArray[n];
                    web = SPControl.GetContextWeb(Context);
                    if (fileInfo.Name == (web.Language + ".lng.xml"))
                    {
                        _currentLanguage = web.Language.ToString();
                    }
                }

                if (_currentLanguage == "")
                {
                    _currentLanguage = "1033";
                }

                _resourceFile = new XmlDocument();
                var reader =
                    new XmlTextReader(
                        Page.Server.MapPath(string.Format("{0}/{1}.lng.xml", resources, _currentLanguage)));
                _resourceFile.Load(reader);
                reader.Close();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private static string getResourceDirectory()
        {
            return "/_layouts/BilSimser/KnowledgeBase";
        }

        public void HandleException(Exception ex)
        {

            _error = true;
            Controls.Clear();

            Controls.Add(
                new LiteralControl(
                    "We're sorry but an error has occurred with the SharePoint Knowledge Base Web Part.<br/><br/>"));
            Controls.Add(new LiteralControl("Please See the details below for more information.<br/><br/>"));

            var sb = new StringBuilder();
            sb.AppendFormat("Message:<br/>{0}<br/><br/>", ex.Message);
            sb.AppendFormat("StackTrace:<br/>{0}<br/><br/>", ex.StackTrace);
            Controls.Add(new LiteralControl(string.Format("<div class=\"ms-alerttext\">{0}</div>", sb)));

//            Controls.Add(
//                new LiteralControl(
//                    string.Format(
//                        "</p><strong><a href=\"mailto:{0}?Subject=Forum Error&Body={1}\">Email error information to support</a></strong></p>",
//                        ForumConstants.ConfigAuthorEmail, sb)));

//            Controls.Add(
//                new LiteralControl(string.Format("<a href=\"{1}\">Return to {0} Home</a>", Name,
//                                                 ForumApplication.Instance.GetLink(
//                                                     SharePointForumControls.Home))));

        }

        private BaseControl createDynamicControl(SharePointForumControls childControl)
        {
            BaseControl control = null;

            try
            {
                var controlTypeName = string.Format("{0}.{1}", ForumConstants.ControlNamespace, childControl);
                var controlType = Type.GetType(controlTypeName);
                control = Activator.CreateInstance(controlType) as BaseControl;
                if (control != null) control.WebPartParent = this;
            }
            catch (ArgumentNullException ex)
            {
                HandleException(ex);
            }

            return control;
        }
    }
}