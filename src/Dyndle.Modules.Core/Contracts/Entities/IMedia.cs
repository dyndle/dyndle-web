﻿using System;
using Dyndle.Modules.Core.Models;

namespace Dyndle.Modules.Core.Contracts.Entities
{
    /// <summary>
    /// Interface IMedia
    /// </summary>
    /// <seealso cref="IEntityModel" />
    public interface IMedia : IEntityModel
    {
        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>The URL.</value>
        new string Url { get; }
    }

    /// <summary>
    /// Interface IEclMedia
    /// </summary>
    /// <seealso cref="IMedia" />
    public interface IEclMedia : IMedia
    {
        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>System.String.</returns>
        string GetUrl<TEnum>(TEnum type) where TEnum : struct, IConvertible;

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="type"></param>
        /// <param name="preferredFileExtension"></param>
        /// <returns></returns>
        string GetUrl<TEnum>(TEnum type, string preferredFileExtension) where TEnum : struct, IConvertible;

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>The display name.</value>
        string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the file extension.
        /// </summary>
        /// <value>The file extension.</value>
        string FileExtension { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        int Width { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        int Height { get; set; }

        /// <summary>
        /// Gets or sets the size of the file.
        /// </summary>
        /// <value>The size of the file.</value>
        int FileSize { get; set; }

        /// <summary>
        /// True if the object is a video
        /// </summary>
        /// <value><c>true</c> if this instance is video; otherwise, <c>false</c>.</value>
        bool IsVideo { get; }
    }
}