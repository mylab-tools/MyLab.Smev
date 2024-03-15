using System.Security.Cryptography.X509Certificates;
using MyLab.SmevClient.Crypt;

namespace UnitTests
{
    public class Pkcs7SignatureProcessorTests
    {
        [Fact]
        public void ShouldCreateDetachedSignature()
        {
            const string thumbprint = "0f52cfdaa3d592acb441e7920b6b37cfdf71c067";
            var certProvider = new ByTumbprintCertHandleProvider(StoreLocation.CurrentUser, thumbprint);
            var processor = new Pkcs7SignatureProcessor(certProvider);
            var data = new byte[] { 0, 1, 2, 3, 4 };
            var signature = processor.Detached(data);
            //File.WriteAllBytes(@"c:\app\test.data", data);
            //File.WriteAllBytes(@"c:\app\test.data.sig", signature);
            Assert.NotNull(signature);
        }

    }
}
