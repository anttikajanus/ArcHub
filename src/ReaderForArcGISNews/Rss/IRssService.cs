namespace ReaderForArcGISNews.Rss
{
    using System.Xml;
    using System.Xml.Linq;

    using ReaderForArcGISNews.Models;

    public interface IRssService
    {
        RssFeed GetFeedDataFromReader(XmlReader reader);

        void LoadAtomFeed(XDocument doc, RssFeed feed);

        void LoadRssFeed(XDocument doc, RssFeed feed);
    }
}