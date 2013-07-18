namespace ReaderForArcGISNews.RssCategoryFeed
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Windows;
    using System.Xml;

    using Caliburn.Micro;

    using ReaderForArcGISNews.Framework.MVVM;
    using ReaderForArcGISNews.Models;
    using ReaderForArcGISNews.Rss;
    using ReaderForArcGISNews.RssFeedDetails;

    public class RssCategoryFeedViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;

        private readonly IRssService rssService;

        private RssFeed rssFeed;

        public RssCategoryFeedViewModel(INavigationService navigationService, IRssService rssService)
        {
            this.navigationService = navigationService;
            this.rssService = rssService;
        }

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

        protected override void OnActivate()
        {
            if (ParameterSink.Parameter != null && ParameterSink.Parameter is RssFeed)
            {
                this.RssFeed = ParameterSink.Parameter as RssFeed;

                if (!this.IsOffline)
                {
                    this.LoadFeed();
                }
            }

            base.OnActivate();
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
