﻿using System.Xml.Serialization;

namespace UnitTests
{
    [XmlRoot("FakeSmevServiceResponse", 
        Namespace = "urn://fake-smev-service-response",
        IsNullable = false)]
    public class FakeSmevServiceResponse
    {
        [XmlElement("Status")]
        public string? Status { get; set; }
    }
}
