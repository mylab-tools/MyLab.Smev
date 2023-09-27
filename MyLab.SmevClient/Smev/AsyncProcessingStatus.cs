using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using MyLab.SmevClient.Xml;

namespace MyLab.SmevClient.Smev
{
    public class AsyncProcessingStatus : IXmlSerializable
    {
        public Guid OriginalMessageId { get; set; }

        /// <summary>
        /// Категория статуса.
        /// </summary>
        public string StatusCategory { get; set; }

        /// <summary>
        /// Описание процессинга в человекочитаемом виде.
        /// </summary>
        public string StatusDetails { get; set; }

        public SmevFault Fault { get; set; }

        #region IXmlSerializable

        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(XmlReader reader)
        {
            reader.ReadElementSubtreeContent(
                "AsyncProcessingStatus", Smev3NameSpaces.MESSAGE_EXCHANGE_TYPES_1_2, required: true,
                (statusReader) =>
                {
                    statusReader.ReadElementIfItCurrentOrRequired(
                        "OriginalMessageId", Smev3NameSpaces.MESSAGE_EXCHANGE_TYPES_1_2, required: true,
                        (r) => OriginalMessageId = Guid.Parse((string)r.ReadElementContentAsString()));

                    statusReader.ReadElementIfItCurrentOrRequired(
                        "StatusCategory", Smev3NameSpaces.MESSAGE_EXCHANGE_TYPES_1_2, required: true,
                        (r) => StatusCategory = r.ReadElementContentAsString());

                    statusReader.ReadElementIfItCurrentOrRequired(
                        "StatusDetails", Smev3NameSpaces.MESSAGE_EXCHANGE_TYPES_1_2, required: false,
                        (r) => StatusDetails = r.ReadElementContentAsString());

                    statusReader.ReadElementIfItCurrentOrRequired(
                        "SmevFault", Smev3NameSpaces.MESSAGE_EXCHANGE_TYPES_1_2, required: false,
                        (r) => 
                        {
                            var fault = new SmevFault();

                            fault.ReadXml(r);

                            Fault = fault;
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
