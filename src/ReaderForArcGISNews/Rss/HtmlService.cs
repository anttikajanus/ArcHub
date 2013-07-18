namespace ReaderForArcGISNews.Rss
{
    using System;
    using System.IO;
    using System.Windows;
    using System.Windows.Media;

    using Microsoft.Phone.Controls;

    using ReaderForArcGISNews.Models;
    using ReaderForArcGISNews.Resources;

    public class HtmlService
    {
        #region HtmlContentProperty

        public static readonly DependencyProperty HtmlContentProperty = DependencyProperty.RegisterAttached(
            "HtmlContent", typeof(string), typeof(WebBrowser), new PropertyMetadata(OnHtmlContentPropertyChanged));

        public static void SetHtmlContent(UIElement element, string value)
        {
            element.SetValue(HtmlContentProperty, value);
        }

        public static string GetHtmlContent(UIElement element)
        {
            return (string)element.GetValue(HtmlContentProperty);
        }

        private static void OnHtmlContentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var webBrowser = d as WebBrowser;
            
            if (webBrowser == null)
            {
                throw new ArgumentException("DependencyObject was not a WebBrowser.");
            }
            
            webBrowser.NavigateToString(e.NewValue.ToString());
        }

        #endregion // HtmlContentProperty

        /// <summary>
        /// assemble an html string for the web browser to display the contents of the selected feed item
        /// </summary>
        /// <returns></returns>
        public string BuildHtmlForSelectedItem(RssItem item)
        {
            string html;

            // is your preview window not showing up? Make sure the PreviewEnabled setting
            // is set to "true" in the Settings.xml configuration file.

            // retrieve the HTML from the included file
            using (var stream = Application.GetResourceStream(new Uri("Resources/preview.html", UriKind.Relative)).Stream)
            using (var reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }

            // replace bits of the HTML via tokens with data from our settings and selected feed item
            var content = item.Content;
            if (content.Trim().Length == 0)
            {
                content = AppResources.NoFeedContent;
            }

            html = html.Replace("{{body.foreground}}", ForegroundColor);
            html = html.Replace("{{body.background}}", BackgroundColor);
            html = html.Replace("{{head.title}}", item.Title);
            html = html.Replace("{{body.content}}", content);

            return html;
        }


        public static string BackgroundColor
        {
            get
            {
                if (((SolidColorBrush)Application.Current.Resources["PhoneBackgroundBrush"]).Color == Colors.Black)
                {
                    return "Black";
                }

                return "White";
            }
        }

        public static string ForegroundColor
        {
            get
            {
                if (((SolidColorBrush)Application.Current.Resources["PhoneBackgroundBrush"]).Color == Colors.Black)
                {
                    return "White";
                }

                return "Black";
            }
        }
 
    }
}
