using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using MyLab.SmevClient.Soap;

namespace MyLab.SmevClient.Http
{
    internal static class HttpContentExtensions
    {
        internal static async Task<T> ReadSoapBodyAsAsync<T>(
            this HttpContent httpContent, CancellationToken cancellationToken)
            where T : ISoapEnvelopeBody, new()
        {
            var stream = await httpContent.ReadSoapBodyAsStreamAsync(cancellationToken);
            

            var reader = XmlReader.Create(stream, new XmlReaderSettings
                                                {
                                                    IgnoreWhitespace = true,
                                                    IgnoreProcessingInstructions = true
                                                });

            var serializer = new XmlSerializer(typeof(SoapEnvelope<T>));

            var envelope = (SoapEnvelope<T>)serializer.Deserialize(reader);

            return envelope.Body;
        }

        internal static async Task<string> ReadSoapBodyAsStringAsync(
            this HttpContent httpContent, CancellationToken cancellationToken)
        {
            var stream = await httpContent.ReadSoapBodyAsStreamAsync(cancellationToken);

            using var streamReader = new StreamReader(stream, Encoding.UTF8);

            return await streamReader.ReadToEndAsync();
        }

        private static async Task<Stream> ReadSoapBodyAsStreamAsync(
                    this HttpContent httpContent, CancellationToken cancellationToken)
        {
            var stream = await httpContent.ReadAsStreamAsync();

            if (stream.CanSeek && stream.Position != 0)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            var mediaTypeHeader = MediaTypeHeaderValue.Parse(httpContent.Headers.ContentType.ToString());

            if(mediaTypeHeader?.Type.Value?.ToLower() == "multipart")
            {
                if (mediaTypeHeader.Boundary.Value == null)
                    throw new InvalidOperationException("Не удаётся определить boundary для multypart содержимого ответа");

                var normBoundary = mediaTypeHeader.Boundary.Value.Trim('\"');
                var multipartReader = new MultipartReader(normBoundary, stream);

                var section = await multipartReader.ReadNextSectionAsync(cancellationToken);

                await section.Body.DrainAsync(cancellationToken);

                stream = section.Body;
            }

            if (stream.CanSeek && stream.Position != 0)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            return stream;
        }
    }
}
