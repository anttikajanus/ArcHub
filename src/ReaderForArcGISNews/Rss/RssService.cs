namespace ReaderForArcGISNews.Rss
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;

    using ReaderForArcGISNews.Framework.MVVM;
    using ReaderForArcGISNews.Models;

    public class RssService : NotificationBase, IRssService
    {
        private static readonly XNamespace NS_MEDIA = "http://search.yahoo.com/mrss/";

        private static readonly XNamespace NS_ITUNES = "http://www.itunes.com/dtds/podcast-1.0.dtd";

        private static readonly XNamespace NS_RSSCONTENT = "http://purl.org/rss/1.0/modules/content/";

        public RssFeed GetFeedDataFromReader(XmlReader reader)
        {
            var doc = XDocument.Load(reader);

            var feed = new RssFeed();

            var defaultNamespace = doc.Root.GetDefaultNamespace();
            if (defaultNamespace.NamespaceName.EndsWith("Atom"))
            {
                LoadAtomFeed(doc, feed);
            }
            else
            {
                LoadRssFeed(doc, feed);
            }

            return feed;
        }

        public void LoadAtomFeed(XDocument doc, RssFeed feed)
        {
            SetMediaImage(feed, doc);
            feed.Description = doc.Root.GetSafeElementString("summary");
            feed.Items = new ObservableCollection<RssItem>();
            feed.LastBuildDate = doc.Root.GetSafeElementDate("updated");
            feed.Link = doc.Root.GetLink("self");

            foreach (var item in doc.Root.Elements(doc.Root.GetDefaultNamespace() + "entry"))
            {
                var newItem = new RssItem()
                                  {
                                      Title = item.GetSafeElementString("title"),
                                      Description = item.GetSafeElementString("description"),
                                      PublishDate = item.GetSafeElementDate("published"),
                                      Guid = item.GetSafeElementString("id"),
                                      Link = item.GetLink("alternate")
                                  };
                feed.Items.Add(newItem);
            }
        }

        public void LoadRssFeed(XDocument doc, RssFeed feed)
        {
            SetMediaImage(feed, doc);
            feed.Description = doc.Root.Element("channel").GetSafeElementString("description");
            feed.Items = new ObservableCollection<RssItem>();
            feed.Link = doc.Root.Element("channel").GetSafeElementString("link");

            foreach (var item in doc.Descendants("item"))
            {
                var newItem = new RssItem()
                                  {
                                      Title = item.GetSafeElementString("title"),
                                      Link = item.GetSafeElementString("link"),
                                      Description = item.GetSafeElementString("description"),
                                      PublishDate = item.GetSafeElementDate("pubDate"),
                                      Guid = item.GetSafeElementString("guid")
                                  };

                var content = item.Descendants(NS_RSSCONTENT + "encoded");
                foreach (var xElement in content)
                {
                    newItem.Content = xElement.Value;
                }

                feed.Items.Add(newItem);
            }

            var lastItem = feed.Items.OrderByDescending(i => i.PublishDate).FirstOrDefault();
            if (lastItem != null)
            {
                feed.LastBuildDate = lastItem.PublishDate;
            }
        }

        private void SetMediaImage(RssFeed feed, XDocument doc)
        {
            var image = doc.Root.Descendants(NS_ITUNES + "image").FirstOrDefault();
            if (image != null)
            {
                feed.ImageUri = new Uri(image.Attribute("href").Value);
                return;
            }

            var thumbnail = doc.Element(NS_MEDIA + "thumbnail");
            if (thumbnail != null)
            {
                feed.ImageUri = new Uri(thumbnail.Value);
                return;
            }

            // if all else fails...
            feed.ImageUri = new Uri("/Resources/Images/RssFeed.png", UriKind.Relative);
        }
    }
}