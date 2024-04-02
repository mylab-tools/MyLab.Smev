using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace MyLab.SmevClient.Smev;

/// <summary>
/// Заголовок вложения
/// </summary>
public class AttachmentHeader : IXmlSerializable
{
    /// <summary>
    /// Идентификатор вложения
    /// </summary>
    public string Id { get; }
    /// <summary>
    /// Тип содержимого
    /// </summary>
    public string MimeType { get; }
    /// <summary>
    /// Подпись PKCS7 в формате BASE64
    /// </summary>
    public string SignatureBase64 { get; set; }

    public AttachmentHeader(string id, string mimeType)
    {
        Id = id;
        MimeType = mimeType;
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
        writer.WriteStartElement("contentId", Smev3NameSpaces.MessageExchangeTypesBasic11);
        writer.WriteString(Id);
        writer.WriteEndElement();

        writer.WriteStartElement("MimeType", Smev3NameSpaces.MessageExchangeTypesBasic11);
        writer.WriteString(MimeType);
        writer.WriteEndElement();

        if (SignatureBase64 != null)
        {
            writer.WriteStartElement("SignaturePKCS7", Smev3NameSpaces.MessageExchangeTypesBasic11);
            writer.WriteString(SignatureBase64);
            writer.WriteEndElement();
        }
    }
}