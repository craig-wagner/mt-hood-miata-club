#region using
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Google.Picasa;
using MtHoodMiata.Web.Models;
#endregion using

namespace MtHoodMiata.Web
{
    public class FileUploadHandler : IHttpHandler, IReadOnlySessionState
    {
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest( HttpContext context )
        {
            try
            {
                HttpPostedFile postedFile = context.Request.Files["Filedata"];

                string albumId = context.Request["AlbumId"];
                string photoType = context.Request["PhotoType"];
                string eventId = context.Request["EventId"];
                string albumName = context.Request["AlbumName"];

                using( Stream newImage = new MemoryStream() )
                {
                    ResizeImage( postedFile.InputStream, newImage );
                    newImage.Seek( 0, SeekOrigin.Begin );

                    PicasaApi picasa = new PicasaApi();

                    if( !String.IsNullOrEmpty( eventId ) )
                    {
                        MtHoodMiataRepository repository = new MtHoodMiataRepository();
                        EventInfo eventInfo = repository.GetEventInfoById( Int32.Parse( eventId ) );

                        albumName = String.Format( "{0} ({1:yyyy-MM-dd})", eventInfo.EventName, eventInfo.StartDate );
                    }

                    if( !String.IsNullOrEmpty( albumName ) )
                    {
                        Album album = picasa.FindAlbum( albumName );
                        IEnumerable<Album> albums = picasa.GetAlbums();

                        if( album == null )
                        {
                            album = picasa.AddAlbum( albumName );
                        }

                        albumId = album.Id;
                    }

                    if( !String.IsNullOrEmpty( albumId ) )
                    {
                        picasa.AddPhoto( albumId, newImage );
                    }
                }

                context.Response.Write( "1" );
            }
            catch( Exception ex )
            {
                context.Response.Write( "Error: " + ex.ToString() );
            }
        }

        private void ResizeImage( Stream fromStream, Stream toStream )
        {
            const double maxWidth = 640;
            const double maxHeight = 480;

            using( Image image = Image.FromStream( fromStream ) )
            {
                double widthScale = 1;

                if( image.Width > maxWidth )
                {
                    widthScale = maxWidth / image.Width;
                }

                double heightScale = 1;

                if( image.Height > maxHeight )
                {
                    heightScale = maxHeight / image.Height;
                }

                if( widthScale < 1 || heightScale < 1 )
                {
                    double scaleFactor = widthScale < heightScale ? widthScale : heightScale;

                    int newWidth = (int)(image.Width * scaleFactor);
                    int newHeight = (int)(image.Height * scaleFactor);
                    using( Bitmap thumbnailBitmap = new Bitmap( newWidth, newHeight ) )
                    {
                        using( Graphics thumbnailGraph = Graphics.FromImage( thumbnailBitmap ) )
                        {
                            thumbnailGraph.CompositingQuality = CompositingQuality.HighQuality;
                            thumbnailGraph.SmoothingMode = SmoothingMode.HighQuality;
                            thumbnailGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;

                            Rectangle imageRectangle = new Rectangle( 0, 0, newWidth, newHeight );
                            thumbnailGraph.DrawImage( image, imageRectangle );

                            ImageCodecInfo jpegCodec = ImageCodecInfo.GetImageEncoders()
                                .FirstOrDefault( c => c.FormatDescription == "JPEG" );
                            if( jpegCodec != null )
                            {
                                EncoderParameters encoderParameters = new EncoderParameters( 1 );
                                encoderParameters.Param[0] = new EncoderParameter( Encoder.Quality, 100L );

                                thumbnailBitmap.Save( toStream, jpegCodec, encoderParameters );
                            }
                            else
                            {
                                thumbnailBitmap.Save( toStream, ImageFormat.Jpeg );
                            }
                        }
                    }
                }
                else
                {
                    image.Save( toStream, ImageFormat.Jpeg );
                }
            }
        }
    }
}