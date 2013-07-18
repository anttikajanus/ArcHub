namespace ReaderForArcGISNews.Models
{
    public class FeedCategory
    {
        public string CategoryTitle { get; set; }

        public string CategorySubTitle { get; set; }

        public string FeedUrl
        {
            get
            {
                var category = this.CategoryTitle.Replace(" ", "-").ToLowerInvariant();

                if (category.Contains("&"))
                {
                    category = category.Replace("&", "and");
                }

                return string.Format("http://blogs.esri.com/esri/arcgis/category/{0}/feed", category);
            }
        }
    }
}
