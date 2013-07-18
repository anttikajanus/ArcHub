namespace ReaderForArcGISNews.RssFeedDetails
{
    using System;

    using Caliburn.Micro;

    using Microsoft.Phone.Tasks;

    using ReaderForArcGISNews.Framework.MVVM;
    using ReaderForArcGISNews.Models;
    using ReaderForArcGISNews.Resources;
    using ReaderForArcGISNews.Rss;
    using ReaderForArcGISNews.RssFeedBrowser;

    public class RssFeedDetailViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;

        private readonly HtmlService htmlService;

        private RssItem feedItem;

        private string feedItemHtmlContent;
        
        public RssFeedDetailViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            this.htmlService = new HtmlService();
        }

        public string FeedItemHtmlContent
        {
            get
            {
                return this.feedItemHtmlContent;
            }

            set
            {
                if (this.feedItemHtmlContent == value)
                {
                    return;
                }

                this.feedItemHtmlContent = value;
                this.NotifyOfPropertyChange(() => this.FeedItemHtmlContent);
            }
        }

        public RssItem FeedItem
        {
            get
            {
                return this.feedItem;
            }

            set
            {
                if (this.feedItem == value)
                {
                    return;
                }
                this.feedItem = value;
                this.NotifyOfPropertyChange(() => this.FeedItem);
            }
        }

        public void OpenFeedInBrowser()
        {
            this.navigationService.UriFor<RssFeedBrowserViewModel>()
                .WithParam(target => target.Link, this.FeedItem.Link)
                .Navigate();
        }

        public void ShareCurrentFeedItem()
        {
            const string Format = "{0} (via {1})";
           
            var task = new ShareLinkTask
            {
                LinkUri = new Uri(this.FeedItem.Link),
                Title = this.FeedItem.Title,
                Message = string.Format(Format, this.FeedItem.Title, string.Format("#{0}", AppResources.ApplicationName))
            };

            task.Show();
        }

        protected override void OnActivate()
        {
            if (ParameterSink.Parameter != null)
            {
                var param = ParameterSink.Parameter as RssItem;
                if (param != null)
                {
                    this.FeedItem = param;
                }
            }

            this.FeedItemHtmlContent = this.htmlService.BuildHtmlForSelectedItem(this.feedItem);

            base.OnActivate();
        }
    }
}
