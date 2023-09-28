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
    public class CertHandleProviderFactoryTests
    {
        [TestMethod]
        public void ShouldCreateByTumbprintCertHandleProvider()
        {
            var tumbprint = "0f52cfdaa3d592acb441e7920b6b37cfdf71c067";
            var provider = CertHandleProviderFactory.Create(new MyLab.SmevClient.Options.ClientCertificateOptions()
            {
                Thumbprint = tumbprint
            });

            Assert.IsNotNull(provider);
            Assert.AreEqual(provider.GetType(), typeof(ByTumbprintCertHandleProvider));
        }

    }
}
