﻿// ***********************************************************************
// Assembly         : EveLib.EveCrest
// Author           : Lars Kristian
// Created          : 12-17-2014
//
// Last Modified By : Lars Kristian
// Last Modified On : 12-17-2014
// ***********************************************************************
// <copyright file="TournamentTypeBanCollection.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Runtime.Serialization;
using eZet.EveLib.EveCrestModule.Models.Shared;

namespace eZet.EveLib.EveCrestModule.Models.Resources.Tournaments {
    /// <summary>
    ///     Class TournamentTypeBanCollection. This class cannot be inherited.
    /// </summary>
    [DataContract]
    public sealed class TournamentTypeBanCollection : CollectionResource<TournamentTypeBanCollection, BanEntry> {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TournamentTypeBanCollection" /> class.
        /// </summary>
        public TournamentTypeBanCollection() {
            ContentType = "application/vnd.ccp.eve.TournamentTypeBanCollection-v1+json";
        }
    }
}