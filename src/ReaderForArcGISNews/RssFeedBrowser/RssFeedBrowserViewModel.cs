namespace ReaderForArcGISNews.RssFeedBrowser
{
    using ReaderForArcGISNews.Framework.MVVM;

    public class RssFeedBrowserViewModel : ViewModelBase
    {
        private string link;

        public RssFeedBrowserViewModel()
        {
            this.IsLoading = true;
        }

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
    }
}
