using System.IO;
using System.Text;
using System.Xml;

namespace Epam.FundManager.Common
{
    public sealed class EncodedXmlTextWriter : XmlTextWriter
    {
        private readonly bool _avoidSelfClosedTags;

        public EncodedXmlTextWriter(Stream stream, Encoding encoding, bool avoidSelfClosedTags, Formatting formatting = Formatting.Indented)
            : base(stream, encoding)
        {
            Formatting = formatting;
            _avoidSelfClosedTags = avoidSelfClosedTags;
        }

        /// <summary>
        /// Avoid Self Closed Tags (no "<tag />")
        /// </summary>
        public override void WriteEndElement()
        {
            if (_avoidSelfClosedTags)
            {
                WriteFullEndElement();
            }
            else
            {
                base.WriteEndElement();
            }
        }
    }
}
