using Microsoft.Extensions.Configuration;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;

namespace EDU.Common.Helper
{
    public static class FileHelper
    {
        public static IConfiguration configuration;
        public static void DeleteIfExists(string filePath)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        public static string FormatImagePath(string imgPath)
        {
            return imgPath.Split(';')[0].Replace("\\", "/");
        }
       
        public static void DeleteAllFileInFolder(string folderPath, string extention = ".docx")
        {
            string[] filePaths = Directory.GetFiles(folderPath, extention, SearchOption.TopDirectoryOnly);
            foreach (var path in filePaths)
                DeleteIfExists(path);
        }
        public static string SavePdfFile(string RelativePathFileName, byte[] dataFile)
        {
            return WriteFileToAuthAccessFolder(RelativePathFileName, dataFile, null, null);
        }
        public static string SaveFileFromBase64STestimonialstring(string RelativePathFileName, string dataFromBase64String)
        {
            if (dataFromBase64String.Contains("base64,"))
            {
                dataFromBase64String = dataFromBase64String.Substring(dataFromBase64String.IndexOf("base64,", 0) + 7);
            }
            return WriteFileToAuthAccessFolderTestimonial(RelativePathFileName, null, null, dataFromBase64String);
        }
        public static string SaveFileFromBase64String(string RelativePathFileName, string dataFromBase64String)
        {
            if (dataFromBase64String.Contains("base64,"))
            {
                dataFromBase64String = dataFromBase64String.Substring(dataFromBase64String.IndexOf("base64,", 0) + 7);
            }
            return WriteFileToAuthAccessFolder(RelativePathFileName, null, null, dataFromBase64String);
        }
        public static string SaveFileFromBase64String120(string RelativePathFileName, string dataFromBase64String)
        {
            if (dataFromBase64String.Contains("base64,"))
            {
                dataFromBase64String = dataFromBase64String.Substring(dataFromBase64String.IndexOf("base64,", 0) + 7);
            }
            return WriteFileToAuthAccessFolder120(RelativePathFileName, null, null, dataFromBase64String);
        }
        public static string WriteFileToAuthAccessFolderTestimonial(string RelativePathFileName, byte[] dataFile, string jsonData, string base64StringData)
        {
            try
            {
                string result = "";
                string serverRootPathFolder = configuration["AppSettings:WEB_SERVER_FULL_PATH"].ToString();
                string fullPathFile = $@"{serverRootPathFolder}\{RelativePathFileName}";
                string fullPathFolder = System.IO.Path.GetDirectoryName(fullPathFile);
                if (!Directory.Exists(fullPathFolder))
                    Directory.CreateDirectory(fullPathFolder);
                if (dataFile != null)
                    File.WriteAllBytes(fullPathFile, dataFile);
                else if (!string.IsNullOrEmpty(jsonData))
                    File.WriteAllText(fullPathFile, jsonData, Encoding.UTF8);
                else
                {
                    File.WriteAllBytes(fullPathFile, Convert.FromBase64String(base64StringData));
                    Bitmap bmp = new Bitmap(fullPathFile);
                    Bitmap mp = ResizeImage(bmp, 100, 100);
                    try
                    {
                        var idx = fullPathFile.LastIndexOf(".");
                        var path_filename = fullPathFile.Substring(0, idx) + "_small" + fullPathFile.Substring(idx);
                        mp.Save(path_filename);
                    }
                    catch { }
                }
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static string WriteFileToAuthAccessFolder(string RelativePathFileName, byte[] dataFile, string jsonData, string base64StringData)
        {
            try
            {
                string result = "";
                string serverRootPathFolder = configuration["AppSettings:WEB_SERVER_FULL_PATH"].ToString(); 
                string fullPathFile = $@"{serverRootPathFolder}\{RelativePathFileName}";
                string fullPathFolder = System.IO.Path.GetDirectoryName(fullPathFile);
                if (!Directory.Exists(fullPathFolder))
                    Directory.CreateDirectory(fullPathFolder);
                if (dataFile != null)
                    File.WriteAllBytes(fullPathFile, dataFile);
                else if (!string.IsNullOrEmpty(jsonData))
                    File.WriteAllText(fullPathFile, jsonData, Encoding.UTF8);
                else
                {
                    File.WriteAllBytes(fullPathFile, Convert.FromBase64String(base64StringData));
                    Bitmap bmp = new Bitmap(fullPathFile);
                    Bitmap mp = ResizeImage(bmp, 570, 330);
                    try
                    {
                        var idx = fullPathFile.LastIndexOf(".");
                        var path_filename= fullPathFile.Substring(0, idx) + "_small" + fullPathFile.Substring(idx);
                        mp.Save(path_filename);
                    }
                    catch { }
                } 
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static string WriteFileToAuthAccessFolder120(string RelativePathFileName, byte[] dataFile, string jsonData, string base64StringData)
        {
            try
            {
                string result = "";
                string serverRootPathFolder = configuration["AppSettings:WEB_SERVER_FULL_PATH"].ToString();
                string fullPathFile = $@"{serverRootPathFolder}\{RelativePathFileName}";
                string fullPathFolder = System.IO.Path.GetDirectoryName(fullPathFile);
                if (!Directory.Exists(fullPathFolder))
                    Directory.CreateDirectory(fullPathFolder);
                if (dataFile != null)
                    File.WriteAllBytes(fullPathFile, dataFile);
                else if (!string.IsNullOrEmpty(jsonData))
                    File.WriteAllText(fullPathFile, jsonData, Encoding.UTF8);
                else
                {
                    File.WriteAllBytes(fullPathFile, Convert.FromBase64String(base64StringData));
                }
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static Bitmap ResizeImage(Bitmap bmp, int width, int height)
        {
            Size newSize = new Size(width, height);
            double ratioH = 0d;
            double ratioW = 0d;
            double myThumbWidth = 0d;
            double myThumbHeight = 0d;
            int x = 0;
            int y = 0;
            Bitmap bp = new Bitmap(newSize.Width, newSize.Height);
            ratioW = Convert.ToDouble(bmp.Width) / Convert.ToDouble(newSize.Width);
            ratioH = Convert.ToDouble(bmp.Height) / Convert.ToDouble(newSize.Height);
            myThumbHeight = Math.Ceiling(bmp.Height / ratioH);
            myThumbWidth = Math.Ceiling(bmp.Width / ratioW);
            Size thumbSize = new Size((int)myThumbWidth, (int)myThumbHeight);
            bp = new Bitmap(newSize.Width, thumbSize.Height);
            bp = new Bitmap(thumbSize.Width, newSize.Height);
            x = (newSize.Width - thumbSize.Width);
            y = (newSize.Height - thumbSize.Height);
            System.Drawing.Graphics g = Graphics.FromImage(bp);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            Rectangle rect = new Rectangle(0, 0, thumbSize.Width, thumbSize.Height);
            g.DrawImage(bmp, rect, 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel);
            return bp;
        }
        public static string CreatePathFile(string RelativePathFileName)
        {
            try
            {
                string serverRootPathFolder = configuration["AppSettings:WEB_SERVER_FULL_PATH"].ToString();
                string fullPathFile = $@"{serverRootPathFolder}\{RelativePathFileName}";
                string fullPathFolder = System.IO.Path.GetDirectoryName(fullPathFile);
                if (!Directory.Exists(fullPathFolder))
                    Directory.CreateDirectory(fullPathFolder);
                return fullPathFile;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static string GetPathFile()
        {
            try
            {
                string serverRootPathFolder = configuration["AppSettings:WEB_SERVER_FULL_PATH"].ToString();
                if (!Directory.Exists(serverRootPathFolder))
                    Directory.CreateDirectory(serverRootPathFolder);
                return serverRootPathFolder;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static void DeleteFile(string path_file)
        {
            try
            {
                string serverRootPathFolder = configuration["AppSettings:WEB_SERVER_FULL_PATH"].ToString();
                if (File.Exists(serverRootPathFolder + "/" + path_file))
                    File.Delete(serverRootPathFolder + "/" + path_file);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
