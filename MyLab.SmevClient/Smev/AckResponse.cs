using System;
using System.Xml;
using System.Xml.Schema;
using MyLab.SmevClient.Soap;
using MyLab.SmevClient.Xml;

namespace MyLab.SmevClient.Smev
{

    public class AckResponse: ISoapEnvelopeBody
    {
        #region IXmlSerializable

        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(XmlReader reader)
        {
            reader.ReadElementSubtreeContent(
                "Body", SoapConsts.SOAP_NAMESPACE, required: true,
                (bodyReader) =>
                {
                    bodyReader.ReadElementSubtreeContent(
                        "AckResponse", Smev3NameSpaces.MessageExchangeTypes11, required: true,
                        (r) =>
                        {
                        });
                });
        }

        public void WriteXml(XmlWriter writer)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
