using System.Xml;

namespace MyLab.SmevClient
{
    public interface ISmev3XmlSigner
    {
        /// <summary>
        /// Возвращает подпись xml елемента
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        XmlElement SignXmlElement(XmlElement xml, string uri);
    }
}
