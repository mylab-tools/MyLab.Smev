using System.Security.Cryptography.X509Certificates;
using MyLab.SmevClient.Crypt;

namespace UnitTests
{
    public class GostAsymmetricAlgorithmTests
    {
        [Fact]
        public void ShouldCreateFromMyStoreByThumbprint()
        {
            const string thumbprint = "0f52cfdaa3d592acb441e7920b6b37cfdf71c067";
            var provider = new ByTumbprintCertHandleProvider(StoreLocation.CurrentUser, thumbprint);
            var alg = new GostAsymmetricAlgorithm(provider);
            var signatureAlg = alg.SignatureAlgorithm;
            var signature = alg.CreateHashSignature(new byte[32]);
            var cert=new X509Certificate2(alg.CertRawData);
            Assert.NotNull(signature);
            Assert.Equal(signatureAlg, XmlDsigConsts.XmlDsigGost3410_2012_256Url);
            Assert.Equal(thumbprint.ToUpper(), cert.Thumbprint.ToUpper());
        }

    }
}
