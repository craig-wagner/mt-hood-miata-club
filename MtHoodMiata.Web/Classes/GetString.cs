#region using
using System;
using System.Linq.Expressions;
#endregion using

namespace MtHoodMiata.Web
{
    public static class GetString
    {
        #region Public Methods
        /// <summary>
        /// Returns a string representation of an expression that takes no arguments
        /// and returns a value of type T. Also takes a parameter indicating if we want the 
        /// expressions fully qualified name or if we want just the immediate name.
        /// </summary>
        /// <typeparam name="T">
        /// The type returned by the expression. This can generally be omitted if the
        /// compiler can infer the type.
        /// </typeparam>
        /// <param name="expression">
        /// The expression for which a string representation is wanted. This can be a
        /// simple variable or a method that returns a value of type T. It takes the 
        /// form "() => expression." For example:
        /// <code>
        ///     () => myVariable
        ///     () => SomeMethodThatReturnsAValue()
        ///     () => SomeMethodThatReturnsAValueAndTakesArguments( 123 )
        /// </code>
        /// </param>
        /// <param name="returnFullName">boolean value indicating if we want
        /// just the immediate name or the fully qualified name. For example:
        /// <code>
        ///     if true: foo.bar
        ///     if false: bar
        /// </code>
        /// </param>
        /// <returns>
        /// String representation of the expression.
        /// </returns>
        public static string Of<T>( Expression<Func<T>> expression, bool returnFullName = true )
        {
            string name = GetMemberName( expression.Body );

            if( !returnFullName )
            {
                name = StripFullName( name );
            }
            return name;
        }

        /// <summary>
        /// Returns a string representation of an expression that takes no arguments
        /// and returns no value. Also takes a parameter indicating if we want the 
        /// expressions fully qualified name or if we want just the immediate name.
        /// </summary>
        /// <param name="expression">
        /// The expression for which a string representation is wanted. This is a method
        /// method that takes no arguments and does not return a value. It takes the 
        /// form "() => expression." For example:
        /// <code>
        ///     () => SomeMethodThatTakesNoArgsAndReturnsNoValue()
        /// </code>
        /// </param>
        /// <param name="returnFullName">boolean value indicating if we want
        /// just the immediate name or the fully qualified name. For example:
        /// <code>
        ///     if true: foo.bar
        ///     if false: bar
        /// </code>
        /// </param>
        /// <returns>
        /// String representation of the expression.
        /// </returns>
        public static string Of( Expression<Action> expression, bool returnFullName = true )
        {
            string name = GetMemberName( expression.Body );

            if( !returnFullName )
            {
                name = StripFullName( name );
            }
            return name;
        }

        /// <summary>
        /// Returns the string representation of type T. This is equivalent to calling
        /// typeof( T ).Name.
        /// </summary>
        /// <typeparam name="T">
        /// The type for which the name should be returned.
        /// </typeparam>
        /// <returns>
        /// String representation of the type.
        /// </returns>
        public static string Of<T>()
        {
            return typeof( T ).Name;
        }
        #endregion Public Methods

        #region Private Methods
        private static string StripFullName( string name )
        {
            if( name != null )
            {
                int index = name.LastIndexOf( "." );
                if( index >= 0 )
                {
                    name = name.Substring( index + 1, name.Length - index - 1 );
                }
            }
            return name;
        }

        private static string GetMemberName( Expression expression )
        {
            string returnValue = String.Empty;

            switch( expression.NodeType )
            {
                case ExpressionType.MemberAccess:
                    var memberExpression = (MemberExpression)expression;
                    var superName = GetMemberName( memberExpression.Expression );

                    if( String.IsNullOrWhiteSpace( superName ) )
                    {
                        returnValue = memberExpression.Member.Name;
                    }
                    else
                    {
                        returnValue = String.Concat( superName, '.', memberExpression.Member.Name );
                    }
                    break;

                case ExpressionType.Call:
                    var callExpression = (MethodCallExpression)expression;
                    returnValue = callExpression.Method.Name;
                    break;

                case ExpressionType.Convert:
                    var unaryExpression = (UnaryExpression)expression;
                    returnValue = GetMemberName( unaryExpression.Operand );
                    break;

                case ExpressionType.Constant:
                case ExpressionType.Parameter:
                    break;

                default:
                    throw new ArgumentException( "The expression is not a member access or method call expression" );
            }

            return returnValue;
        }
        #endregion Private Methods
    }
}
