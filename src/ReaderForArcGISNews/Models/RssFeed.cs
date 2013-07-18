namespace ReaderForArcGISNews.Models
{
    using System;
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;

    using ReaderForArcGISNews.Framework.MVVM;

    [DataContract]
    public class RssFeed : NotificationBase
    {
        private string feedType;
        private string subTitle;
        private string title;
        private string rssUrl;
        private DateTime? refreshTimeStamp;
        private DateTime? lastBuildDate;
        private string link;
        private string description;
        private Uri imageUri;
        private ObservableCollection<RssItem> items;

        [DataMember]
        public string Title
        {
            get
            {
                return this.title;
            }

            set
            {
                if (this.title == value)
                {
                    return;
                }

                this.title = value;
                this.NotifyOfPropertyChange(() => this.Title);
            }
        }

        [DataMember]
        public string SubTitle
        {
            get
            {
                return this.subTitle;
            }

            set
            {
                if (this.subTitle == value)
                {
                    return;
                }

                this.subTitle = value;
                this.NotifyOfPropertyChange(() => this.SubTitle);
            }
        }

        [DataMember]
        public string FeedType
        {
            get
            {
                return this.feedType;
            }

            set
            {
                if (this.feedType == value)
                {
                    return;
                }

                this.feedType = value;
                this.NotifyOfPropertyChange(() => this.FeedType);
            }
        }

        [DataMember]
        public string RssUrl
        {
            get
            {
                return this.rssUrl;
            }

            set
            {
                if (this.rssUrl == value)
                {
                    return;
                }

                this.rssUrl = value;
                this.NotifyOfPropertyChange(() => this.RssUrl);
            }
        }

        [DataMember]
        public DateTime? RefreshTimeStamp
        {
            get
            {
                return this.refreshTimeStamp;
            }

            set
            {
                if (this.refreshTimeStamp == value)
                {
                    return;
                }

                this.refreshTimeStamp = value;
                this.NotifyOfPropertyChange(() => this.RefreshTimeStamp);
            }
        }

        [DataMember]
        public DateTime? LastBuildDate
        {
            get
            {
                return this.lastBuildDate;
            }

            set
            {
                if (this.lastBuildDate == value)
                {
                    return;
                }

                this.lastBuildDate = value;
                this.NotifyOfPropertyChange(() => this.LastBuildDate);
            }
        }

        [DataMember]
        public string Link
        {
            get
            {
                return this.link;
            }

            set
            {
                if (this.link == value)
                {
                    return;
                }

                this.link = value;
                this.NotifyOfPropertyChange(() => this.Link);
            }
        }

        [DataMember]
        public string Description
        {
            get
            {
                return this.description;
            }

            set
            {
                if (this.description == value)
                {
                    return;
                }

                this.description = value;
                this.NotifyOfPropertyChange(() => this.Description);
            }
        }

        [DataMember]
        public Uri ImageUri
        {
            get
            {
                return this.imageUri;
            }

            set
            {
                if (this.imageUri == value)
                {
                    return;
                }

                this.imageUri = value;
                this.NotifyOfPropertyChange(() => this.ImageUri);
            }
        }

        [DataMember]
        public ObservableCollection<RssItem> Items
        {
            get
            {
                return this.items;
            }

            set
            {
                this.items = value;
                this.NotifyOfPropertyChange(() => this.Items);
            }
        }
    }
}