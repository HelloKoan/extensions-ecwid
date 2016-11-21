﻿// Licensed under the GPL License, Version 3.0. See LICENSE in the git repository root for license information.

using System;

namespace Ecwid
{
    /// <summary>
    /// Throws when legacy client HTTP limit is overhead.
    /// </summary>
    public class EcwidLimitException : EcwidException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EcwidLimitException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        internal EcwidLimitException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EcwidLimitException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        internal EcwidLimitException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}