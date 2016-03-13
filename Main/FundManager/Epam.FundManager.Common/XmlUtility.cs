using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Epam.FundManager.Common
{
    public static class XmlUtility
    {
        #region Private

        private static bool IsCommittedNamespace(XmlElement element, string prefix, string value)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            string name = (prefix.Length > 0) ? ("xmlns:" + prefix) : "xmlns";
            return (element.HasAttribute(name) && (element.GetAttribute(name) == value));
        }

        private static bool HasNamespace(XmlElement element, string prefix, string value)
        {
            return (IsCommittedNamespace(element, prefix, value) || ((element.Prefix == prefix) && (element.NamespaceURI == value)));
        }

        private static bool IsRedundantNamespace(XmlElement element, string prefix, string value)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            for (XmlNode node = element.ParentNode; node != null; node = node.ParentNode)
            {
                var element2 = node as XmlElement;
                if ((element2 != null) && HasNamespace(element2, prefix, value))
                {
                    return true;
                }
            }
            return false;
        }

        private static void DeleteRedundantNamespaces(XmlElement elem)
        {
            if (elem == null)
            {
                return;
            }
            foreach (var element in elem.ChildNodes.OfType<XmlElement>())
            {
                DeleteRedundantNamespaces(element);
                if (IsRedundantNamespace(element, element.Prefix, element.NamespaceURI))
                {
                    string name = "xmlns:" + element.Prefix;
                    element.RemoveAttribute(name);
                }
            }
        }

        #endregion

        public static XmlDocument DeleteComments(XmlDocument doc)
        {
            XmlNodeList list = doc.SelectNodes("//comment()");
            if (list != null)
            {
                foreach (XmlNode node in list)
                {
                    if (node.ParentNode != null)
                    {
                        node.ParentNode.RemoveChild(node);
                    }
                }
            }
            return doc;
        }

        public static XmlDocument DeleteRedundantNamespaces(XmlDocument doc)
        {
            if (doc == null)
            {
                return null;
            }
            var document = new XmlDocument();
            document.LoadXml(doc.OuterXml);
            DeleteRedundantNamespaces(document.DocumentElement);
            return document;
        }

        public static XmlDocument AddXmlDecalaration(XmlDocument doc, Encoding encoding)
        {
            if ((doc != null) && (doc.DocumentElement != null))
            {
                if ((doc.FirstChild != null) && (doc.FirstChild.NodeType == XmlNodeType.XmlDeclaration))
                {
                    doc.RemoveChild(doc.FirstChild);
                }
                XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", encoding.BodyName, string.Empty);
                doc.InsertBefore(xmlDeclaration, doc.DocumentElement);
            }
            return doc;
        }

        public static XmlDocument AddXmlDecalaration(XmlDocument doc)
        {
            return AddXmlDecalaration(doc, Encoding.UTF8);
        }

        public static XmlDocument RemoveXmlDeclaration(XmlDocument doc)
        {
            if ((doc != null) && (doc.DocumentElement != null))
            {
                if ((doc.FirstChild != null) && (doc.FirstChild.NodeType == XmlNodeType.XmlDeclaration))
                {
                    doc.RemoveChild(doc.FirstChild);
                }
            }
            return doc;
        }

        public static XmlDocument ConvertEncoding(XmlDocument doc, Encoding encoding)
        {
            if (doc != null)
            {
                var sb = new StringBuilder(doc.OuterXml);
                using (var sw = new EncodedStringWriter(sb, encoding))
                {
                    doc.LoadXml(sw.ToString());
                }
            }
            return AddXmlDecalaration(doc, encoding);
        }

        public static XmlDocument ToUTF8(XmlDocument doc)
        {
            return ConvertEncoding(doc, Encoding.UTF8);
        }

        public static XmlDocument Canonize(XmlDocument doc, Encoding encoding)
        {
            if (doc == null)
            {
                return null;
            }
            var canonizedXml = new XmlDocument();
            canonizedXml.LoadXml(doc.OuterXml);
            canonizedXml = DeleteRedundantNamespaces(canonizedXml);
            canonizedXml = DeleteComments(canonizedXml);
            return ConvertEncoding(canonizedXml, encoding);
        }

        public static XmlDocument Canonize(XmlDocument doc)
        {
            return Canonize(doc, Encoding.UTF8);
        }

        public static XmlDocument Copy(XmlDocument from)
        {
            if (from == null)
            {
                return null;
            }
            var doc = new XmlDocument();
            doc.LoadXml(from.OuterXml);
            return doc;
        }

        public static byte[] ToByteArray(XmlDocument doc)
        {
            if (doc == null)
                throw new ArgumentNullException("doc");

            using (var ms = new MemoryStream())
            {
                doc.Save(ms);
                byte[] bytes = ms.ToArray();
                return bytes;
            }
        }
    }
}
