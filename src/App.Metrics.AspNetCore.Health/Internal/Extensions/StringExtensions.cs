﻿// <copyright file="StringExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

// ReSharper disable CheckNamespace
namespace System
// ReSharper restore CheckNamespace
{
    [ExcludeFromCodeCoverage]
    internal static class StringExtensions
    {
        [DebuggerStepThrough]
        internal static bool IsMissing(this string value) { return string.IsNullOrWhiteSpace(value); }

        [DebuggerStepThrough]
        internal static bool IsPresent(this string value) { return !string.IsNullOrWhiteSpace(value); }

        [DebuggerStepThrough]
        internal static string EnsureLeadingSlash(this string url)
        {
            if (!url.StartsWith("/"))
            {
                return "/" + url;
            }

            return url;
        }
    }
}