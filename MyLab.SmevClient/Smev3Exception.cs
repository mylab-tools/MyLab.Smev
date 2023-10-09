using System;
using System.Net.Http;
using System.Runtime.Serialization;
using MyLab.SmevClient.Soap;

namespace MyLab.SmevClient
{
    public class Smev3Exception : Exception
    {
        public SoapFault FaultInfo { get; set; }

        public HttpResponseMessage ResponseMessage{get;set;}

        public Smev3Exception()
        {
        }

        public Smev3Exception(string message) : base(message)
        {
        }

        public Smev3Exception(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected Smev3Exception(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
