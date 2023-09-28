using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Net.Http.Headers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class MediaTypeHeaderValueBehavior
    {
        [TestMethod]
        public void ShouldParseMultipartHeader()
        {
            //Arrange
            const string headerValue = "Multipart/Related; boundary=\"----=_Part_414733_1995688938.1695915852237\"; type=\"application/xop+xml\"; start-info=\"text/xml\"";
            //const string HeaderValue = "multipart/mixed; boundary=------------------------a9dd0ab37a224967";

            //Act
            var header = MediaTypeHeaderValue.Parse(headerValue);

            //Assert
            Assert.IsNotNull(header);
            //Assert.AreEqual("multipart", header.Type);
            //Assert.AreEqual("mixed", header.SubType);
            //Assert.AreEqual("------------------------a9dd0ab37a224967", header.Boundary);
            Assert.AreEqual("Multipart", header.Type);
            Assert.AreEqual("Related", header.SubType);
            Assert.AreEqual("----=_Part_414733_1995688938.1695915852237", header.Boundary);
        }

    }
}
