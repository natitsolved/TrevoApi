using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Web;

namespace Trevo.API.Helper
{
    public class ImageUploadHelper
    {
        // set default size here
        public int Width { get; set; }

        public int Height { get; set; }

        // folder for the upload, you can put this in the web.config
        private string UploadPath = string.Empty;

        public ImageResult RenameUploadFile(MultipartFileData file, bool isMedium, string selectedCategory, string fileNameToSave,string folderName, Int32 counter = 0)
        {

            //string prepend = "item_";
            //string finalFileName = prepend + ((counter).ToString()) + "_" + fileName;
            if (isMedium)
            {
                UploadPath = "~/"+ folderName +"/MediumSize/";
            }
            else
            {
                UploadPath = "~/" + folderName + "/ThumbnailSize/";
            }

            string finalFileName = fileNameToSave;
            if (System.IO.File.Exists
                (HttpContext.Current.Request.MapPath(UploadPath + finalFileName)))
            {
                //file exists => add country try again
                return RenameUploadFile(file, isMedium, selectedCategory, fileNameToSave,folderName, ++counter);
            }
            //file doesn't exist, upload item but validate first
            return UploadFile(finalFileName, UploadPath,folderName,isMedium);
        }

        private ImageResult UploadFile(string fileName,string path,string folderName,bool isMedium)
        {
            ImageResult imageResult = new ImageResult { Success = true, ErrorMessage = null };
        var path1=  Path.Combine(HttpContext.Current.Request.MapPath(folderName+ "/OriginalSize"), fileName);
            string extension = Path.GetExtension(fileName);
            string newFileName = string.Empty;
            if (isMedium)
            {
                newFileName = fileName.Split('.')[0] + "_medium" + extension;
            }
            else
            {
                newFileName= fileName.Split('.')[0] + "_thumbnail" + extension; ;
            }
          

            try
            {
                //File.Create(fileName, path);
                string originalPath ="~/"+ folderName + "/OriginalSize";
                Image imgOriginal = Image.FromFile(path1);

                //pass in whatever value you want
                Image imgActual = Scale(imgOriginal);
                imgOriginal.Dispose();
                if(isMedium)
                path1 = Path.Combine(HttpContext.Current.Request.MapPath(folderName + "/MediumSize"), newFileName);
                else
                    path1 = Path.Combine(HttpContext.Current.Request.MapPath(folderName + "/ThumbnailSize"), newFileName);
                imgActual.Save(path1);
                imgActual.Dispose();

                imageResult.ImageName = newFileName;

                return imageResult;
            }
            catch (Exception ex)
            {
                // you might NOT want to show the exception error for the user
                // this is generally logging or testing

                imageResult.Success = false;
                imageResult.ErrorMessage = ex.Message;
                return imageResult;
            }
        }

        private bool ValidateExtension(string extension)
        {
            extension = extension.ToLower();
            switch (extension)
            {
                case ".jpg":
                    return true;
                case ".png":
                    return true;
                case ".jpeg":
                    return true;
                default:
                    return false;
            }
        }

        private Image Scale(Image imgPhoto)
        {
            float sourceWidth = imgPhoto.Width;
            float sourceHeight = imgPhoto.Height;
            float destHeight = 0;
            float destWidth = 0;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            // force resize, might distort image
            if (Width != 0 && Height != 0)
            {
                destWidth = Width;
                destHeight = Height;
            }
            // change size proportially depending on width or height
            else if (Height != 0)
            {
                destWidth = (float)(Height * sourceWidth) / sourceHeight;
                destHeight = Height;
            }
            else
            {
                destWidth = Width;
                destHeight = (float)(sourceHeight * Width / sourceWidth);
            }

            Bitmap bmPhoto = new Bitmap((int)destWidth, (int)destHeight,
                                        PixelFormat.Format32bppPArgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, (int)destWidth, (int)destHeight),
                new Rectangle(sourceX, sourceY, (int)sourceWidth, (int)sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();

            return bmPhoto;
        }
    }

    public class ImageResult
    {
        public bool Success { get; set; }
        public string ImageName { get; set; }
        public string ErrorMessage { get; set; }
    }
}