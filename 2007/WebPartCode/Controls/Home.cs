namespace SharePointKnowledgeBase.WebPartCode.Controls
{
    public class Home : BaseControl
    {
        protected override void CreateChildControls()
        {
            Controls.Add(new Trail());
            Controls.Add(new Message());
            Controls.Add(new BrowseByCategory());
            Controls.Add(new Search());
            Controls.Add(new ViewArticlesByCategory());
            Controls.Add(new FeaturedArticles());
            Controls.Add(new MostPopularArticles());
            Controls.Add(new RecentEntries());
        }
    }
}