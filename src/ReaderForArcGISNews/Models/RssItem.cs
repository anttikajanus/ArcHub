namespace ReaderForArcGISNews.Models
{
    using System;
    using System.Runtime.Serialization;

    using ReaderForArcGISNews.Framework.MVVM;

    [DataContract]
    public class RssItem : NotificationBase
    {
        private string guid;
        private string link;
        private string description;
        private DateTime? publishDate;
        private string title;

        private string content;

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
        public DateTime? PublishDate
        {
            get
            {
                return this.publishDate;
            }

            set
            {
                if (this.publishDate == value)
                {
                    return;
                }

                this.publishDate = value;
                this.NotifyOfPropertyChange(() => this.PublishDate);
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
        public string Guid
        {
            get
            {
                return this.guid;
            }

            set
            {
                if (this.guid == value)
                {
                    return;
                }

                this.guid = value;
                this.NotifyOfPropertyChange(() => this.Guid);
            }
        }

        [DataMember]
        public string Content
        {
            get
            {
                return this.content;
            }

            set
            {
                if (this.content == value)
                {
                    return;
                }

                this.content = value;
                this.NotifyOfPropertyChange(() => this.Content);
            }
        }
    }
}