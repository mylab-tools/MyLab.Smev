using System.Xml.Linq;
using System.Xml.Serialization;

namespace MyLab.SmevClient
{
    public static class Smev3ExceptionExtyensions
    {
        public static bool IsMessageNotFound(this Smev3Exception exception)
        {
            if(exception.FaultInfo.DetailXmlFragment == null)
                return false;

            var faultDoc = XDocument.Parse(exception.FaultInfo.DetailXmlFragment);

            XName n = XName.Get("{urn://x-artefacts-smev-gov-ru/services/message-exchange/types/faults/1.1}TargetMessageIsNotFound");
            var notFoundElement = faultDoc.Element("detail")?.Element(n);
            
            return notFoundElement != null;
        }
    }
}