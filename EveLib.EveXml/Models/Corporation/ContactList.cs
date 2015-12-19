﻿// ***********************************************************************
// Assembly         : EveLib.EveOnline
// Author           : Lars Kristian
// Created          : 03-06-2014
//
// Last Modified By : Lars Kristian
// Last Modified On : 06-19-2014
// ***********************************************************************
// <copyright file="ContactList.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Diagnostics.Contracts;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using eZet.EveLib.EveXmlModule.Util;

namespace eZet.EveLib.EveXmlModule.Models.Corporation {
    /// <summary>
    ///     Class ContactList.
    /// </summary>
    [XmlRoot("result", IsNullable = false)]
    public class ContactList : IXmlSerializable {
        /// <summary>
        ///     Gets or sets the corporation contacts.
        /// </summary>
        /// <value>The corporation contacts.</value>
        [XmlElement("rowset")]
        public EveXmlRowCollection<Contact> CorporationContacts { get; set; }

        /// <summary>
        ///     Gets or sets the alliance contacts.
        /// </summary>
        /// <value>The alliance contacts.</value>
        [XmlElement("rowset")]
        public EveXmlRowCollection<Contact> AllianceContacts { get; set; }


        /// <summary>
        ///     This method is reserved and should not be used. When implementing the IXmlSerializable interface, you should return
        ///     null (Nothing in Visual Basic) from this method, and instead, if specifying a custom schema is required, apply the
        ///     <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute" /> to the class.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Xml.Schema.XmlSchema" /> that describes the XML representation of the object that is
        ///     produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)" /> method
        ///     and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)" />
        ///     method.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public XmlSchema GetSchema() {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader" /> stream from which the object is deserialized.</param>
        public void ReadXml(XmlReader reader) {
            var xml = new XmlHelper(reader);
            CorporationContacts = xml.deserializeRowSet<Contact>("corporateContactList");
            AllianceContacts = xml.deserializeRowSet<Contact>("allianceContactList");
        }

        /// <summary>
        ///     Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter" /> stream to which the object is serialized.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void WriteXml(XmlWriter writer) {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Class Contact.
        /// </summary>
        [XmlRoot("row")]
        public class Contact {
            /// <summary>
            ///     Gets or sets the contact identifier.
            /// </summary>
            /// <value>The contact identifier.</value>
            [XmlAttribute("contactID")]
            public long ContactId { get; set; }

            /// <summary>
            ///     Gets or sets the name of the contact.
            /// </summary>
            /// <value>The name of the contact.</value>
            [XmlAttribute("contactName")]
            public string ContactName { get; set; }

            /// <summary>
            ///     Gets or sets the standing.
            /// </summary>
            /// <value>The standing.</value>
            [XmlAttribute("standing")]
            public double Standing { get; set; }

            /// <summary>
            ///     Gets or sets a value indicating whether [in watchlist].
            /// </summary>
            /// <value><c>true</c> if [in watchlist]; otherwise, <c>false</c>.</value>
            [XmlIgnore]
            public bool InWatchlist { get; set; }

            /// <summary>
            ///     Gets or sets the in watchlist as string.
            /// </summary>
            /// <value>The in watchlist as string.</value>
            [XmlAttribute("inWatchlist")]
            public string InWatchlistAsString {
                get { return InWatchlist.ToString(); }
                set {
                    Contract.Requires(value != null);
                    InWatchlist = (value.ToLower() == "true");
                }
            }
        }
    }
}