﻿using System;
using System.Xml.Serialization;

namespace eZet.Eve.EveLib.Model.EveApi.Map {

    [Serializable]
    [XmlRoot("result", IsNullable = false)]
    public class SovereigntyStatus : XmlElement {

        [XmlElement("rowset")]
        public XmlRowSet<Structure> Structures { get; set; }

        public class Structure {
           // TODO Implement

            
        }
    }
}
