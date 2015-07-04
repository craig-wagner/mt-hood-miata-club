#region using
using System;
using System.IO;
#endregion using

namespace MtHoodMiata.Web
{
    public class HitCounter : IHitCounter
    {
        #region IHitCounter Members
        public int GetHitCount( string filePath )
        {
            int currentCount = 0;

            StreamReader countReader = null;

            try
            {
                countReader = new StreamReader( filePath );
                string count = countReader.ReadToEnd();
                currentCount = Convert.ToInt32( count );
            }
            catch( Exception )
            {
                // Swallow any exceptions... this really needs to be updated
                // to do some sort of logging
            }
            finally
            {
                if( countReader != null )
                {
                    countReader.Dispose();
                }
            }

            currentCount++;

            StreamWriter countWriter = null;

            try
            {
                countWriter = new StreamWriter( filePath, false );
                countWriter.Write( currentCount );
                countWriter.Flush();
            }
            catch( Exception )
            {
                // Swallow any exceptions... this really needs to be updated
                // to do some sort of logging
            }
            finally
            {
                if( countWriter != null )
                {
                    countWriter.Dispose();
                }
            }

            return currentCount;
        }
        #endregion IHitCounter Methods
    }
}