#region using
using System;
using System.Runtime.Serialization;
#endregion using

namespace MtHoodMiata.Web
{
    /// <summary>
    /// The exception that is thrown when a ReportsWeb error occurs.
    /// </summary>
    /// <remarks>
    /// <c>MtHoodMiataException</c> doesn't provide any additional properties; 
    /// it is used to indicate an exception specific to the Mt. Hood Miata web site.
    /// </remarks>
    [Serializable]
    public class MtHoodMiataException : Exception
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MtHoodMiataException"/> class.
        /// </summary>
        public MtHoodMiataException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MtHoodMiataException"/> 
        /// class with a specified warning message.
        /// </summary>
        /// <param name="message">
        /// The message that describes the warning.
        /// </param>
        public MtHoodMiataException( string message )
            : base( message )
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MtHoodMiataException"/> 
        /// class with a specified warning message and a reference to the inner 
        /// exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">
        /// The warning message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null 
        /// reference if no inner exception is specified.
        /// </param>
        public MtHoodMiataException( string message, Exception innerException )
            : base( message, innerException )
        { }

        /// <summary>
        /// Initializes a new instance of the MtHoodMiataException class with 
        /// serialized data.
        /// </summary>
        /// <param name="info">
        /// The object that holds the serialized object data.
        /// </param>
        /// <param name="context">
        /// The contextual information about the source or destination.
        /// </param>
        protected MtHoodMiataException( SerializationInfo info, StreamingContext context )
            : base( info, context )
        { }
        #endregion Constructors
    }
}
