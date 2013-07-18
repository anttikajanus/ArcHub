namespace ReaderForArcGISNews
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Net;
    using System.Windows;
    using System.Xml;

    using Caliburn.Micro;

    using Microsoft.Phone.Tasks;

    using ReaderForArcGISNews.Framework.MVVM;
    using ReaderForArcGISNews.Models;
    using ReaderForArcGISNews.Rss;
    using ReaderForArcGISNews.RssCategoryFeed;
    using ReaderForArcGISNews.RssFeedDetails;

    public class MainViewModel : ViewModelBase
    {
        private const string FeedUrl = "http://blogs.esri.com/esri/arcgis/feed/";

        private readonly INavigationService navigationService;

        private readonly IRssService rssService;

        private RssFeed rssFeed;

        public MainViewModel(INavigationService navigationService, IRssService rssService)
        {
            this.navigationService = navigationService;
            this.rssService = rssService;

            this.FeedCategories = new ObservableCollection<RssFeed>
                { 
                    new RssFeed { Title = "Developer", RssUrl = @"http://blogs.esri.com/esri/arcgis/category/developer/feed" },
                    new RssFeed { Title = "ArcGIS Online", RssUrl = @"http://blogs.esri.com/esri/arcgis/category/arcgis-online/feed" },
                    new RssFeed { Title = "Editing", RssUrl = @"http://blogs.esri.com/esri/arcgis/category/editing/feed" },
                    new RssFeed { Title = "Mobile", RssUrl = @"http://blogs.esri.com/esri/arcgis/category/mobile/feed" },
                    new RssFeed { Title = "Mapping", RssUrl = @"http://blogs.esri.com/esri/arcgis/category/mapping/feed" },
                    new RssFeed { Title = "Analysis & Geoprocessing", RssUrl = @"http://blogs.esri.com/esri/arcgis/category/subject-analysis-and-geoprocessing/feed" },
                    new RssFeed { Title = "Services", RssUrl = @"http://blogs.esri.com/esri/arcgis/category/services/feed" },
                    new RssFeed { Title = "Web", RssUrl = @"http://blogs.esri.com/esri/arcgis/category/web/feed" },
                    new RssFeed { Title = "Geodata", RssUrl = @"http://blogs.esri.com/esri/arcgis/category/geodata/feed" },
                    new RssFeed { Title = "3D GIS", RssUrl = @"http://blogs.esri.com/esri/arcgis/category/subject-3d-gis/feed" },
                    new RssFeed { Title = "Imagery", RssUrl = @"http://blogs.esri.com/esri/arcgis/category/subject-imagery/feed" },
                    new RssFeed { Title = "Python", RssUrl = @"http://blogs.esri.com/esri/arcgis/category/subject-python/feed" }
                };

            this.LoadingText = string.Format("Loading feed...");

            this.rssFeed = new RssFeed { RssUrl = FeedUrl, Title = "ArcGIS Blogs" };

            if (!this.IsOffline)
            {
                this.LoadFeed();
            }
        }

        public ObservableCollection<RssFeed> FeedCategories { get; set; } 

        public RssFeed RssFeed
        {
            get
            {
                return this.rssFeed;
            }

            set
            {
                this.rssFeed = value;
                this.NotifyOfPropertyChange(() => this.RssFeed);
            }
        }

        public void RefreshFeed()
        {
            if (this.IsLoading || this.IsOffline)
            {
                return;
            }

            this.LoadFeed();
        }

        public void NavigateToFeedItem(RssItem selectedItem)
        {
            if (selectedItem == null)
            {
                return;
            }

            var item = this.rssFeed.Items.FirstOrDefault(x => x.Guid == selectedItem.Guid);

            ParameterSink.Parameter = item;

            this.navigationService.UriFor<RssFeedDetailViewModel>().Navigate();
        }

        public void NavigateToCategoryFeed(RssFeed selectedItem)
        {
            if (selectedItem == null)
            {
                return;
            }

            ParameterSink.Parameter = selectedItem;

            this.navigationService.UriFor<RssCategoryFeedViewModel>().Navigate();
        }

        public void About()
        {
            var marketplaceDetailTask = new MarketplaceDetailTask { ContentType = MarketplaceContentType.Applications };

            marketplaceDetailTask.Show();
        }

        private void LoadFeed()
        {
            if (this.IsOffline)
            {
                return;
            }

            this.IsLoading = true;

            // retrieve the feed and it's items from the internet
            var request = HttpWebRequest.CreateHttp(RssFeed.RssUrl) as HttpWebRequest;
            request.BeginGetResponse(
                (token) =>
                    {
                        try
                        {
                            // process the response
                            using (var response = request.EndGetResponse(token) as HttpWebResponse)
                            using (var stream = response.GetResponseStream())
                            using (var reader = XmlReader.Create(stream))
                            {
                                var feed = this.rssService.GetFeedDataFromReader(reader);
                                feed.RssUrl = request.RequestUri.AbsoluteUri;

                                RssFeed.Description = feed.Description.OrNoneProvided();
                                RssFeed.FeedType = feed.FeedType;
                                RssFeed.ImageUri = feed.ImageUri;
                                RssFeed.Items = feed.Items;
                                RssFeed.LastBuildDate = feed.LastBuildDate;
                                RssFeed.Link = feed.Link;
                                RssFeed.RefreshTimeStamp = DateTime.Now;
                                RssFeed.RssUrl = feed.RssUrl;

                                // unlock the UI
                                this.IsLoading = false;
                            }
                        }
                        catch (Exception)
                        {
                            Execute.OnUIThread(() =>
                                {
                                    MessageBox.Show("Cannot load Rss feed. Please try later again.");
                                    this.IsLoading = false;
                                });
                        }
                    },
                null);
        }
    }
}
