using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FilmoweJanusze.Controllers
{
    public class ExtendedController : Controller
    {

        //REKORDY
        public string NoContentPhoto = "/Images/Brak_zdjecia.png";


        //WALIDACJA JSON
        public JsonResult CheckBirthdate(string Birthdate)
        {
            DateTime dateTime = new DateTime();
            if (DateTime.TryParse(Birthdate, out dateTime))
            {
                if (dateTime <= DateTime.Today && dateTime.Year >= 1900)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("Nieprawidłowy format daty", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult CheckMinReleaseDate(string ReleaseDate)
        {
            DateTime dateTime = new DateTime();
            if( DateTime.TryParse(ReleaseDate, out dateTime) )
            {
                if (dateTime.Year >= 1900)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("Nieprawidłowy format daty", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult CheckImageSelected(string RadioPhotoBtn)
        {
            if (RadioPhotoBtn == "FromFile")
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        //METODY UŻYTKOWE
        public bool SaveCroppedImage(Image image, int maxWidth, int maxHeight, string filePath)
        {
            //source:https://www.codeproject.com/Tips/683699/%2FTips%2F683699%2FCropping-an-Image-from-Center-i

            ImageCodecInfo jpgInfo = ImageCodecInfo.GetImageEncoders()
                                     .Where(codecInfo =>
                                     codecInfo.MimeType == "image/jpeg").First();
            Image finalImage = image;
            System.Drawing.Bitmap bitmap = null;
            try
            {
                int left = 0;
                int top = 0;
                int srcWidth = maxWidth;
                int srcHeight = maxHeight;
                bitmap = new System.Drawing.Bitmap(maxWidth, maxHeight);
                double croppedHeightToWidth = (double)maxHeight / maxWidth;
                double croppedWidthToHeight = (double)maxWidth / maxHeight;

                if (image.Width > image.Height)
                {
                    srcWidth = (int)(Math.Round(image.Height * croppedWidthToHeight));
                    if (srcWidth < image.Width)
                    {
                        srcHeight = image.Height;
                        left = (image.Width - srcWidth) / 2;
                    }
                    else
                    {
                        srcHeight = (int)Math.Round(image.Height * ((double)image.Width / srcWidth));
                        srcWidth = image.Width;
                        top = (image.Height - srcHeight) / 2;
                    }
                }
                else
                {
                    srcHeight = (int)(Math.Round(image.Width * croppedHeightToWidth));
                    if (srcHeight < image.Height)
                    {
                        srcWidth = image.Width;
                        top = (image.Height - srcHeight) / 2;
                    }
                    else
                    {
                        srcWidth = (int)Math.Round(image.Width * ((double)image.Height / srcHeight));
                        srcHeight = image.Height;
                        left = (image.Width - srcWidth) / 2;
                    }
                }
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    //g.SmoothingMode = SmoothingMode.HighQuality;
                    //g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    //g.CompositingQuality = CompositingQuality.HighQuality;
                    //g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(image, new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    new Rectangle(left, top, srcWidth, srcHeight), GraphicsUnit.Pixel);
                }
                finalImage = bitmap;
            }
            catch { }
            try
            {
                using (EncoderParameters encParams = new EncoderParameters(1))
                {
                    encParams.Param[0] = new EncoderParameter(Encoder.Quality, (long)100);
                    //quality should be in the range 
                    //[0..100] .. 100 for max, 0 for min (0 best compression)
                    finalImage.Save(filePath, jpgInfo, encParams);
                    return true;
                }
            }
            catch { }
            if (bitmap != null)
            {
                bitmap.Dispose();
            }
            return false;
        }

        public string SaveNewFile(string foldername, string filename, HttpPostedFileBase image)
        {
            return SaveNewFile(foldername, filename, image, true);
        }
        public string SaveNewFile(string foldername, string filename, HttpPostedFileBase image, bool overwite)
        {
            string root = Server.MapPath("~/");
            string folder = "/Images/" + foldername + "/";
            string name = filename;
            string ext = System.IO.Path.GetExtension(image.FileName);

            //tworzy folder
            System.IO.Directory.CreateDirectory(root + folder);

            if (System.IO.File.Exists(root + folder + name + ext) && overwite == false)
            {
                int i = 0;
                do
                {
                    i++;
                    name = filename + i.ToString();
                }
                while (System.IO.File.Exists(root + folder + name + ext));
            }

            //zapisanie zdjecia wg sciezki do konkretnego miejsca na dysku
            image.SaveAs(root + folder + name + ext);

            return folder + name + ext;
        }

        public void DeleteOldFile(string Url)
        {
            if(Url != NoContentPhoto)
                if (System.IO.File.Exists(Server.MapPath("~/") + Url))
                    System.IO.File.Delete(Server.MapPath("~/") + Url);
        }

        public string IfEmptySetEmptyPhoto(string Url)
        {
            if (String.IsNullOrEmpty(Url))
                return NoContentPhoto;
            else return Url;
        }

    }
}