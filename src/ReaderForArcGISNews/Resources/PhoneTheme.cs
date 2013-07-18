namespace ReaderForArcGISNews.Resources
{
    using System.Windows;
    using System.Windows.Media;

    public static class HtmlTheme
    {
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
