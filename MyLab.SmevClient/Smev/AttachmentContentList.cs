using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace MyLab.SmevClient.Smev;

/// <summary>
/// Список вложений
/// </summary>
public class AttachmentContentList : List<AttachmentContent>, IXmlSerializable
{
    public AttachmentContentList()
    {
        
    }

    public AttachmentContentList(IEnumerable<AttachmentContent> initial)
    {
        AddRange(initial);
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
        foreach (var item in this)
        {
            writer.WriteStartElement("AttachmentContent", Smev3NameSpaces.MessageExchangeTypesBasic11);
            item.WriteXml(writer);
            writer.WriteEndElement();
        }
    }
}