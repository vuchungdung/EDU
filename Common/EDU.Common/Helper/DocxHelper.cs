using DinkToPdf;
using DocumentFormat.OpenXml.Packaging;
using Novacode;
using OpenXmlPowerTools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace EDU.Common.Helper
{
    public class DocxHelper
    {
        public enum Document { DOCX, XLSX, PPTX }
        public static string fTempImg = "#img#";
        public static string fTempImg1 = "#img1#";
        public static string fTempBarcode = "#barcode#";
        public static string fTempQrCode = "#qrcode#";
        public static string fTempTableData = "#tbd";
        public static string space = "  ";
        public static void ReplaceTime(DocX doc, DateTime? dateprint)
        {
            if (dateprint == null)
                dateprint = DateTime.Now;
            doc.ReplaceText("{location}", "Constants.Location");
            doc.ReplaceText("{hour}", string.Format("{0:00}", dateprint.Value.Hour));
            doc.ReplaceText("{minute}", string.Format("{0:00}", dateprint.Value.Minute));
            doc.ReplaceText("{year}", dateprint.Value.Year.ToString());
            doc.ReplaceText("{month}", string.Format("{0:00}", dateprint.Value.Month));
            doc.ReplaceText("{day}", string.Format("{0:00}", dateprint.Value.Day));
        }
        public static bool IsPropertyExist(dynamic settings, string name)
        {
            if (settings is ExpandoObject)
                return ((IDictionary<string, object>)settings).ContainsKey(name);

            if (settings is Newtonsoft.Json.Linq.JObject)
            {
                return ((Newtonsoft.Json.Linq.JObject)settings).ContainsKey(name);
            }

            return settings.GetType().GetProperty(name) != null;
        }
        public static string ReplaceHexadecimalSymbols(string txt)
        {
            string r = "[\x00-\x08\x0B\x0C\x0E-\x1F\x26]";
            return Regex.Replace(txt, r, "", RegexOptions.Compiled);
        }
        public static void ReplaceData(Newtonsoft.Json.Linq.JObject dtInfo, DocX doc)
        {
            if (dtInfo != null)
            {
                foreach (var member in dtInfo.Properties().ToList())
                {
                    string value = "";
                    var lines = member.Value.ToString().Split(new[] { Environment.NewLine },
                        StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < lines.Length; i++)
                        if (i == 0)
                            value = lines[i];
                        else value = value + "\r" + lines[i];
                    if (member.Name == "age")
                        value = LabelHelper.GetAgeFromText(value);
                    doc.ReplaceText("{" + member.Name + "}", ReplaceHexadecimalSymbols(value));
                }
            }
        }
        public static void ReplaceData(Dictionary<string, string> dictionaryData, DataTable dtInfo, DocX doc, bool hidden = false)
        {
            if (dictionaryData != null && dictionaryData.Count > 0)
            {
                foreach (var item in dictionaryData)
                {
                    string value = "";
                    var lines = (item.Value + "").Split(new[] { Environment.NewLine },
                        StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < lines.Length; i++)
                        if (i == 0)
                            value = lines[i];
                        else value = value + "\r" + lines[i];
                    if (item.Key.StartsWith("wd_char"))
                    {
                        var format = new Formatting();
                        format.Size = item.Key.Split('|').Length > 1 ? int.Parse(item.Key.Split('|')[1]) : 15;
                        format.FontFamily = new Novacode.Font("Wingdings 2");
                        format.Hidden = false;
                        var tmp = int.Parse(value);
                        char tick = (char)tmp;
                        doc.ReplaceText("{" + item.Key.Split('|')[0].Replace("wd_char_", "") + "}", tick.ToString(), newFormatting: format);
                    }
                    else if (item.Key.StartsWith("wd1_char"))
                    {
                        var format = new Formatting();
                        format.Size = 9;
                        format.FontFamily = new Novacode.Font("Wingdings");
                        format.Hidden = false;
                        var tmp = int.Parse(value);
                        char tick = (char)tmp;
                        doc.ReplaceText("{" + item.Key.Replace("wd1_char_", "") + "}", tick.ToString(), newFormatting: format);
                    }
                    else
                    {
                        var format = new Formatting();
                        format.Hidden = false;
                        doc.ReplaceText("{" + item.Key + "}", ReplaceHexadecimalSymbols(value), newFormatting: hidden || value == "(" || value == ")" ? format : null);
                    }
                }
            }
            else if (dtInfo != null && dtInfo.Rows.Count > 0)
            {
                foreach (DataRow r in dtInfo.Rows)
                {
                    foreach (DataColumn c in dtInfo.Columns)
                    {
                        string value = "";
                        var lines = Convert.ToString(r[c.ColumnName]).Split(new[] { Environment.NewLine },
                            StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < lines.Length; i++)
                            if (i == 0)
                                value = lines[i];
                            else value = value + "\r" + lines[i];
                        if (c.ColumnName == "age")
                            value = LabelHelper.GetAgeFromText(value);
                        doc.ReplaceText("{" + c.ColumnName + "}", ReplaceHexadecimalSymbols(value));
                    }
                }
            }
        }
        public static Novacode.Table FindTableWithText(List<Novacode.Table> tables, string temp, out int row, out int cell)
        {
            row = -1;
            cell = -1;
            foreach (var table in tables)
            {
                for (int r = 0; r < table.RowCount; ++r)
                {
                    for (int c = 0; c < table.Rows[r].Cells.Count; ++c)
                    {
                        if (table.Rows[r].Cells[c].Xml.Value == temp)
                        {
                            row = r;
                            cell = c;
                            return table;
                        }
                    }
                }
            }
            return null;
        }
        public static Formatting Formatting(int fontSize = 10, bool isBold = false, bool italic = false, string fontFamily = "times new roman", string formatOptions = "")
        {
            Formatting formatting = new Formatting();
            if (formatOptions == "")
            {
                formatting.Bold = isBold;
                formatting.FontFamily = new Novacode.Font(fontFamily);
                formatting.Size = fontSize;
                formatting.Italic = italic;
                formatting.FontColor = System.Drawing.Color.FromArgb(31, 56, 100);
            }
            else
            {
                var options = formatOptions.Split(';');
                var arrColors = options[0].Split(',');
                formatting.Bold = Convert.ToBoolean(options[1]);
                formatting.FontFamily = new Novacode.Font(fontFamily);
                formatting.Size = Convert.ToDouble(options[2]); ;
                formatting.Italic = Convert.ToBoolean(options[3]);
                formatting.FontColor = System.Drawing.Color.FromArgb(int.Parse(arrColors[0]), int.Parse(arrColors[1]), int.Parse(arrColors[2]));
            }
            return formatting;
        }

        public static Uri FixUri(string brokenUri)
        {
            string newURI = string.Empty;
            if (brokenUri.Contains("mailto:"))
            {
                int mailToCount = "mailto:".Length;
                brokenUri = brokenUri.Remove(0, mailToCount);
                newURI = brokenUri;
            }
            else
            {
                newURI = " ";
            }
            return new Uri(newURI);
        }

        public static string ParseDOCX(FileInfo fileInfo, int margin)
        {
            try
            {
                byte[] byteArray = File.ReadAllBytes(fileInfo.FullName);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    memoryStream.Write(byteArray, 0, byteArray.Length);
                    using (WordprocessingDocument wDoc =
                                                WordprocessingDocument.Open(memoryStream, true))
                    {
                        int imageCounter = 0;
                        var pageTitle = fileInfo.FullName;
                        var part = wDoc.CoreFilePropertiesPart;
                        if (part != null)
                            pageTitle = (string)part.GetXDocument()
                                                    .Descendants(DC.title)
                                                    .FirstOrDefault() ?? fileInfo.FullName;

                        WmlToHtmlConverterSettings settings = new WmlToHtmlConverterSettings()
                        {
                            AdditionalCss = "body { margin: " + margin + "px " + margin + "px " + margin + "px " + margin + "px; padding: 0; }",
                            PageTitle = pageTitle,
                            FabricateCssClasses = true,
                            CssClassPrefix = "pt-",
                            RestrictToSupportedLanguages = false,
                            RestrictToSupportedNumberingFormats = false,
                            ImageHandler = imageInfo =>
                            {
                                ++imageCounter;
                                string extension = imageInfo.ContentType.Split('/')[1].ToLower();
                                ImageFormat imageFormat = null;
                                if (extension == "png") imageFormat = ImageFormat.Png;
                                else if (extension == "gif") imageFormat = ImageFormat.Gif;
                                else if (extension == "bmp") imageFormat = ImageFormat.Bmp;
                                else if (extension == "jpeg") imageFormat = ImageFormat.Jpeg;
                                else if (extension == "tiff")
                                {
                                    extension = "gif";
                                    imageFormat = ImageFormat.Gif;
                                }
                                else if (extension == "x-wmf")
                                {
                                    extension = "wmf";
                                    imageFormat = ImageFormat.Wmf;
                                }

                                if (imageFormat == null) return null;

                                string base64 = null;
                                try
                                {
                                    using (MemoryStream ms = new MemoryStream())
                                    {
                                        imageInfo.Bitmap.Save(ms, imageFormat);
                                        var ba = ms.ToArray();
                                        base64 = System.Convert.ToBase64String(ba);
                                    }
                                }
                                catch (System.Runtime.InteropServices.ExternalException)
                                { return null; }

                                ImageFormat format = imageInfo.Bitmap.RawFormat;
                                ImageCodecInfo codec = ImageCodecInfo.GetImageDecoders()
                                                            .First(c => c.FormatID == format.Guid);
                                string mimeType = codec.MimeType;

                                string imageSource =
                                        string.Format("data:{0};base64,{1}", mimeType, base64);

                                XElement img = new XElement(Xhtml.img,
                                        new XAttribute(NoNamespace.src, imageSource),
                                        imageInfo.ImgStyleAttribute,
                                        imageInfo.AltText != null ?
                                            new XAttribute(NoNamespace.alt, imageInfo.AltText) : null);
                                return img;
                            }
                        };

                        XElement htmlElement = WmlToHtmlConverter.ConvertToHtml(wDoc, settings);
                        var html = new XDocument(new XDocumentType("html", null, null, null),
                                                                                    htmlElement);
                        var htmlString = html.ToString(SaveOptions.DisableFormatting);
                        return htmlString;
                    }
                }
            }
            catch (Exception ex)
            {
                return "The file is either open, please close it or contains corrupt data";
            }
        }
        public static string ConvertDocx2Html(string fileInput, string fileOutput, int margin)
        {
            var fileInfo = new FileInfo(fileInput);
            string fullFilePath = fileInfo.FullName;
            string htmlText = string.Empty;
            try
            {
                htmlText = ParseDOCX(fileInfo, margin);
            }
            catch (OpenXmlPackageException e)
            {
                if (e.ToString().Contains("Invalid Hyperlink"))
                {
                    using (FileStream fs = new FileStream(fullFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    {
                        UriFixer.FixInvalidUri(fs, brokenUri => FixUri(brokenUri));
                    }
                    htmlText = ParseDOCX(fileInfo, margin);
                }
            }

            var writer = File.CreateText(fileOutput + ".html");
            writer.WriteLine(htmlText.ToString());
            writer.Dispose();
            return fileOutput;
        }
        public static string GetHtmlfromWord(string fileInput, int margin)
        {
            var fileInfo = new FileInfo(fileInput);
            string fullFilePath = fileInfo.FullName;
            string htmlText = string.Empty;
            try
            {
                htmlText = ParseDOCX(fileInfo, margin);
            }
            catch (OpenXmlPackageException e)
            {
                if (e.ToString().Contains("Invalid Hyperlink"))
                {
                    using (FileStream fs = new FileStream(fullFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    {
                        UriFixer.FixInvalidUri(fs, brokenUri => FixUri(brokenUri));
                    }
                    htmlText = ParseDOCX(fileInfo, margin);
                }
            }
            return htmlText;
        }
        public static string ConvertDocx2Pdf(string fileInput, string fileOutput, int margin)
        {
            //Convert docx to html
            var fileInfo = new FileInfo(fileInput);
            string fullFilePath = fileInfo.FullName;
            string htmlText = string.Empty;
            try
            {
                htmlText = ParseDOCX(fileInfo, margin);
            }
            catch (OpenXmlPackageException e)
            {
                if (e.ToString().Contains("Invalid Hyperlink"))
                {
                    using (FileStream fs = new FileStream(fullFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    {
                        UriFixer.FixInvalidUri(fs, brokenUri => FixUri(brokenUri));
                    }
                    htmlText = ParseDOCX(fileInfo, margin);
                }
            }

            var writer = File.CreateText(fileOutput + ".html");
            writer.WriteLine(htmlText.ToString());
            writer.Dispose();

            // Convert html to pdf
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                DocumentTitle = "Business Document",
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Out = fileOutput+".pdf",
                Margins = {
                        Top = 2,
                        Bottom = 2,
                        Right = 1.5,
                        Left = 1,
                        Unit = Unit.Centimeters
                    }
            },
                Objects = {
                new ObjectSettings() {
                    PagesCount = true,
                    HtmlContent = File.ReadAllText(fileOutput + ".html"),
                    WebSettings = { DefaultEncoding = "utf-8" },
                    FooterSettings = { FontSize = 9, Center = "Trang [page] / [toPage]", Line = true }
                }
                }
            };
            var converter = new BasicConverter(new PdfTools());
            converter.Convert(doc);
            return fileOutput;
        }
    }
}


