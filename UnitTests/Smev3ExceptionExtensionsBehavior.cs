using MyLab.SmevClient;
using Xunit.Sdk;

namespace UnitTests;

public class Smev3ExceptionExtensionsBehavior
{
    [Fact]
    public void ShouldDetectNotFoundFault()
    {
        //Arrange
        const string faultFragment = "<detail><ns3:TargetMessageIsNotFound xmlns:ns10=\"urn://x-artefacts-smev-gov-ru/services/message-exchange/types/routing/1.3\" xmlns:ns11=\"urn://x-artefacts-smev-gov-ru/services/message-exchange/types/faults/1.3\" xmlns:ns12=\"urn://x-artefacts-smev-gov-ru/services/message-exchange/types/1.3\" xmlns:ns2=\"urn://x-artefacts-smev-gov-ru/services/message-exchange/types/basic/1.1\" xmlns:ns3=\"urn://x-artefacts-smev-gov-ru/services/message-exchange/types/faults/1.1\" xmlns:ns4=\"urn://x-artefacts-smev-gov-ru/services/message-exchange/types/1.1\" xmlns:ns5=\"urn://x-artefacts-smev-gov-ru/services/message-exchange/types/basic/1.2\" xmlns:ns6=\"urn://x-artefacts-smev-gov-ru/services/message-exchange/types/faults/1.2\" xmlns:ns7=\"urn://x-artefacts-smev-gov-ru/services/message-exchange/types/1.2\" xmlns:ns8=\"urn://x-artefacts-smev-gov-ru/services/message-exchange/types/directive/1.3\" xmlns:ns9=\"urn://x-artefacts-smev-gov-ru/services/message-exchange/types/basic/1.3\" /></detail>";

        var exception = new Smev3Exception()
        {
            FaultInfo = new MyLab.SmevClient.Soap.SoapFault
            {
                DetailXmlFragment = faultFragment
            }   
        };

        //Act
        var isNotFound = exception.IsMessageNotFound();

        //Assert
        Assert.True(isNotFound);
    }

    [Fact]
    public void ShouldNotDetectNotFoundIfITAbsent()
    {
        //Arrange
        const string faultFragment = "<detail><SomeOneElse/></detail>";

        var exception = new Smev3Exception()
        {
            FaultInfo = new MyLab.SmevClient.Soap.SoapFault
            {
                DetailXmlFragment = faultFragment
            }   
        };

        //Act
        var isNotFound = exception.IsMessageNotFound();

        //Assert
        Assert.False(isNotFound);
    }
}
