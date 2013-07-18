namespace ReaderForArcGISNews.Framework.MVVM
{
    using System.Net.NetworkInformation;

    using Caliburn.Micro;

    /// <summary>
    /// Base class for view models.
    /// </summary>
    public abstract class ViewModelBase : Conductor<IScreen>
    {
        private bool isLoading;

        private string loadingText;

        private bool isOffline;

        /// <summary>
        /// Gets or sets the text for the loading indicators
        /// </summary>
        public string LoadingText
        {
            get
            {
                return this.loadingText;
            }

            set
            {
                this.loadingText = value;
                this.NotifyOfPropertyChange(() => this.LoadingText);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the routes are being loaded
        /// </summary>
        public bool IsLoading
        {
            get
            {
                return this.isLoading;
            }

            set
            {
                this.isLoading = value;
                this.NotifyOfPropertyChange(() => this.IsLoading);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the application has network connection.
        /// </summary>
        public bool IsOffline
        {
            get
            {
                return this.isOffline;
            }

            private set
            {
                this.isOffline = value;
                this.NotifyOfPropertyChange(() => this.IsOffline);
            }
        }

        protected override void OnActivate()
        {
            // Check if netowork is available and let the views know.
            this.IsOffline = !NetworkInterface.GetIsNetworkAvailable();

            base.OnActivate();
        }
    }
}
