namespace ReaderForArcGISNews.RssFeedBrowser
{
    using System.Windows;

    using ReaderForArcGISNews.Framework.MVVM;

    public partial class RssFeedBrowserView 
    {
        public RssFeedBrowserView()
        {
            this.InitializeComponent();
        }

        private void WebBrowser_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            this.mask.Visibility = Visibility.Collapsed;
            (this.DataContext as ViewModelBase).IsLoading = false;
        }
    }
}