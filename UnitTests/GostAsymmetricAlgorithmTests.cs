using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyLab.SmevClient;
using MyLab.SmevClient.Crypt;
using MyLab.SmevClient.Smev;
using MyLab.SmevClient.Soap;

namespace UnitTests
{
    [TestClass]
    public class GostAsymmetricAlgorithmTests
    {
        [TestMethod]
        public void ShouldCreateFromMyStoreByThumbeprint()
        {
            var tumbprint = "0f52cfdaa3d592acb441e7920b6b37cfdf71c067";
            var provider = new ByTumbprintCertHandleProvider(StoreLocation.CurrentUser, tumbprint);
            var alg = new GostAsymmetricAlgorithm(provider);
            var signatureAlg = alg.SignatureAlgorithm;
            var signature = alg.CreateHashSignature(new byte[32]);
            var cert=new X509Certificate2(alg.CertRawData);
            Assert.IsNotNull(signature);
            Assert.AreEqual(signatureAlg, XmlDsigConsts.XmlDsigGost3410_2012_256Url);
            Assert.AreEqual(tumbprint.ToUpper(), cert.Thumbprint.ToUpper());
        }

    }
}
