using System.Xml.Serialization;

namespace MyLab.SmevClient.Soap
{
    public class SoapEnvelopeHeader
    {
        [XmlElement(
            Namespace = "http://schemas.microsoft.com/ws/2005/05/addressing/none",
            ElementName = "Action"
        )]
        public SoapAction Action { get; set; }
    }
}
