﻿// ***********************************************************************
// Assembly         : EveLib.EveCrest
// Author           : Lars Kristian
// Created          : 12-16-2014
//
// Last Modified By : Lars Kristian
// Last Modified On : 12-17-2014
// ***********************************************************************
// <copyright file="ICrestResource.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using eZet.EveLib.EveCrestModule.Models.Links;

namespace eZet.EveLib.EveCrestModule.Models.Resources {
    /// <summary>
    ///     Interface ICrestResource
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICrestResource<out T> where T : class, ICrestResource<T> {
        /// <summary>
        ///     Gets or sets a value indicating whether this instance is deprecated.
        /// </summary>
        /// <value><c>true</c> if this instance is deprecated; otherwise, <c>false</c>.</value>
        bool IsDeprecated { get; set; }

        /// <summary>
        ///     Gets the version.
        /// </summary>
        /// <value>The version.</value>
        string Version { get; }

        /// <summary>
        ///     Gets or sets the crest.
        /// </summary>
        /// <value>The crest.</value>
        EveCrest Crest { get; set; }

        /// <summary>
        ///     Queries the resource asynchronous.
        /// </summary>
        /// <typeparam name="TOut">The type of the t out.</typeparam>
        /// <param name="objFunc">The object function.</param>
        /// <returns>Task&lt;TOut&gt;.</returns>
        Task<TOut> QueryAsync<TOut>(Func<T, Href<TOut>> objFunc)
            where TOut : class, ICrestResource<TOut>;

        /// <summary>
        ///     Queries the resource asynchronous.
        /// </summary>
        /// <typeparam name="TOut">The type of the t out.</typeparam>
        /// <param name="objFunc">The object function.</param>
        /// <returns>Task&lt;TOut&gt;.</returns>
        Task<TOut> QueryAsync<TOut>(Func<T, LinkedEntity<TOut>> objFunc)
            where TOut : class, ICrestResource<TOut>;
    }
}