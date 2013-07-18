namespace ReaderForArcGISNews.Rss
{
    using System;
    using System.Linq;
    using System.Xml.Linq;

    public static class Extensions
    {
        public static string OrNoneProvided(this string value, string @default = "None provided")
        {
            if (value != null && value.Trim().Length > 0)
            {
                return value;
            }

            return @default;
        }

        public static string GetLink(this XElement item, string rel)
        {
            var links = item.Elements(item.GetDefaultNamespace() + "link");
            var link = from l in links where l.Attribute("rel").Value == rel select l.Attribute("href").Value;
            return link.FirstOrDefault();
        }

        public static DateTime? GetSafeElementDate(this XElement item, string elementName)
        {
            DateTime date;
            var element = item.Element(item.GetDefaultNamespace() + elementName);
            if (element == null)
            {
                return null;
            }
            if (DateTime.TryParse(element.Value, out date))
            {
                return date;
            }
            else
            {
                return null;
            }
        }

        public static string GetSafeElementString(this XElement item, string elementName)
        {
            var value = item.Element(item.GetDefaultNamespace() + elementName);
            return value != null ? value.Value : String.Empty;
        }

        public static string GetSafeElementString(this XElement item, string elementName, XNamespace xmlns)
        {
            XName name;
            if (item.Name.NamespaceName == String.Empty)
            {
                name = xmlns + item.Name.LocalName;
            }
            else
            {
                name = item.Name;
            }
            var element = new XElement(name, from e in item.Elements() select e.WithDefaultXmlNamespace(xmlns));

            var value = element.Element(item.GetDefaultNamespace() + elementName);
            return value != null ? value.Value : String.Empty;
        }

        public static string GetSafeElementString(this XElement item, XName elementName)
        {
            var value = item.Element(elementName);

            return value != null ? value.Value : String.Empty;
        }

        public static XElement WithDefaultXmlNamespace(this XElement xelem, XNamespace xmlns)
        {
            XName name;
            
            if (xelem.Name.NamespaceName == String.Empty)
            {
                name = xmlns + xelem.Name.LocalName;
            }
            else
            {
                name = xelem.Name;
            }
            return new XElement(name, from e in xelem.Elements() select e.WithDefaultXmlNamespace(xmlns));
        }
    }
}