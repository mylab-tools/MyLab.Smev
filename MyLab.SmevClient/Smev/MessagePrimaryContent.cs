using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using MyLab.SmevClient.Xml;

namespace MyLab.SmevClient.Smev
{
    public class MessagePrimaryContent<T> :
        IXmlSerializable where T: new()
    {
        public MessagePrimaryContent(){}

        public MessagePrimaryContent(T content)
        {
            Content = content;
        }

        public T Content { get; set; }

        #region IXmlSerializable

        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(XmlReader reader)
        {
            reader.ReadElementSubtreeContent(
                "MessagePrimaryContent", Smev3NameSpaces.MessageExchangeTypesBasic11, required: true,
                (contentReader) =>
                {
                    if(typeof(T) == typeof(MessagePrimaryContentXml))
                    {
                        var content = new T();

                        ((IXmlSerializable)content).ReadXml(contentReader);

                        Content = content;
                    }
                    else
                    {
                        var serializer = new XmlSerializer(typeof(T));

                        Content = (T)serializer.Deserialize((XmlReader)contentReader);
                    }
                });
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("MessagePrimaryContent",
                Smev3NameSpaces.MessageExchangeTypesBasic11);

            Smev3XmlSerializer.ToXmlElement(Content)
                .WriteTo(writer);

            writer.WriteEndElement();
        }

        #endregion
    }
}
