using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace MyLab.SmevClient.Smev
{
    /// <summary>
    /// Содержит информацию о вложении
    /// </summary>
    public class AttachmentContent : IXmlSerializable
    {
        public string Id { get; }
        public string Base64Content { get; }

        /// <summary>
        /// Инициализирует новый объект класса <see cref="AttachmentContent"/>
        /// </summary>
        /// <param name="id">идентификатор вложения</param>
        /// <param name="base64Content">содержание в формате bas64</param>
        public AttachmentContent(string id, string base64Content)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Base64Content = base64Content ?? throw new ArgumentNullException(nameof(base64Content));
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("Id", Smev3NameSpaces.MessageExchangeTypesBasic11);
            writer.WriteString(Id);
            writer.WriteEndElement();

            writer.WriteStartElement("Content", Smev3NameSpaces.MessageExchangeTypesBasic11);
            writer.WriteString(Base64Content);
            writer.WriteEndElement();
        }
    }
}
