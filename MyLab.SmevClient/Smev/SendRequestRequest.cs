using System;
using System.Xml;
using System.Xml.Schema;
using MyLab.SmevClient.Soap;

namespace MyLab.SmevClient.Smev
{
    /// <summary>
    /// Тело метода SendRequest
    /// </summary>
    public class SendRequestRequest<T>:         
        ISoapEnvelopeBody,
        ISmev3Envelope 
        where T : new()
    {
        #region members

        private readonly ISmev3XmlSigner _signer;

        private readonly SenderProvidedRequestData<T> _requestData;

        private readonly SoapEnvelope<SendRequestRequest<T>> _soapEnvelope;

        #endregion

        public Smev3Methods SmevMethod => Smev3Methods.SendRequest;

        public AttachmentContentList Attachments { get; set; }

        public SendRequestRequest()
        {
        }

        public SendRequestRequest(
            SenderProvidedRequestData<T> requestData,
            ISmev3XmlSigner signer)
        {
            _signer = signer ?? throw new ArgumentNullException(nameof(signer));
            
            _requestData = requestData ?? throw new ArgumentNullException(nameof(requestData));

            _soapEnvelope = new SoapEnvelope<SendRequestRequest<T>>
            {
                Header = new SoapEnvelopeHeader
                {
                    Action = new SoapAction(nameof(Smev3Methods.SendRequest))
                },
                Body = this
            };
        }

        #region ISmev3Envelope

        public byte[] Get()
        {
            return _soapEnvelope.Serialize();
        }

        #endregion

        #region IXmlSerializable

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
            writer.WriteStartElement("SendRequestRequest", Smev3NameSpaces.MessageExchangeTypes11);
            
            _requestData.WriteXml(writer);

            if (Attachments != null)
            {
                writer.WriteStartElement("AttachmentContentList", Smev3NameSpaces.MessageExchangeTypesBasic11);
                Attachments.WriteXml(writer);
                writer.WriteEndElement();
            }

            writer.WriteStartElement("CallerInformationSystemSignature");

            _signer.SignXmlElement(
                    Smev3XmlSerializer.ToXmlElement(_requestData),
                    _requestData.Id)
                .WriteTo(writer);

            writer.WriteEndElement();

            writer.WriteEndElement();
        }
        
        #endregion
    }
}
