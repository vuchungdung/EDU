using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace EDU.Common.Helper
{
    public static class LabelHelper
    {
        public static string CreateBarcode<T>(List<T> list, string pathTemplate, int rows = 1)
        {
            try
            {
                string result = "<html><head><meta charset='utf-8'/></head><body style='margin:0;padding:0'>";
                Type entityType = typeof(T);
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);
                foreach (T item in list)
                {
                    string st = File.ReadAllText(pathTemplate);
                    for (int i = 0; i < rows; ++i)
                    {
                        foreach (PropertyDescriptor prop in properties)
                        {
                            st = st.Replace("%" + prop.Name + "%", Convert.ToString(prop.GetValue(item)));
                        }
                        result += st;
                    }
                }
                result += "<script src='../../../assets/js/JsBarcode.all.min.js'></script><script>JsBarcode('.barcode').init();</script></body></html>";
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static string CreateBarcode(List<string> list, string pathTemplate)
        {
            try
            {
                string result = "<html><head><meta charset='utf-8'/></head><body style='margin:0;padding:0'>";
                foreach (var item in list)
                {
                    string st = File.ReadAllText(pathTemplate);
                    st = st.Replace("%" + "accession_number" + "%", Convert.ToString(item));
                    result += st;
                }
                result += $"<script src='../../../assets/js/JsBarcode.all.min.js'></script><script>JsBarcode('.barcode').init();</script></body></html>";
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public static string Keystruct(string k1, string k2)
        {
            return k1.CompareTo(k2) >= 0 ? k1 + "," + k2 : k2 + "," + k1;
        }

        public static string JsonCompressionLZString(string jsondata)
        {
            try
            {
                return LZString.LzString.CompressToEncodedUriComponent(jsondata);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static string JsonDecompressionLZString(string json)
        {
            try
            {
                if (string.IsNullOrEmpty(json)) return json;
                else return LZString.LzString.DecompressFromEncodedUriComponent(json);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static string ConvertCurrencyToTextVietNamese(decimal total, bool not_currency = false)
        {
            try
            {
                string rs = "";
                total = Math.Round(total, 0);
                string[] ch = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
                string[] rch = { "linh", "mốt", "", "", "", "lăm" };
                string[] u = { "", "mươi", "trăm", "nghìn", "", "", "triệu", "", "", "tỷ", "", "", "nghìn", "", "", "triệu" };
                string nstr = total.ToString();
                int[] n = new int[nstr.Length];
                int len = n.Length;
                for (int i = 0; i < len; i++)
                {
                    n[len - 1 - i] = Convert.ToInt32(nstr.Substring(i, 1));
                }
                for (int i = len - 1; i >= 0; i--)
                {
                    if (i % 3 == 2)
                    {
                        if (n[i] == 0 && n[i - 1] == 0 && n[i - 2] == 0) continue;
                    }
                    else if (i % 3 == 1)
                    {
                        if (n[i] == 0)
                        {
                            if (n[i - 1] == 0) { continue; }
                            else
                            {
                                rs += " " + rch[n[i]]; continue;
                            }
                        }
                        if (n[i] == 1)
                        {
                            rs += " mười"; continue;
                        }
                    }
                    else if (i != len - 1)
                    {
                        if (n[i] == 0)
                        {
                            if (i + 2 <= len - 1 && n[i + 2] == 0 && n[i + 1] == 0) continue;
                            rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);
                            continue;
                        }
                        if (n[i] == 1)
                        {
                            rs += " " + ((n[i + 1] == 1 || n[i + 1] == 0) ? ch[n[i]] : rch[n[i]]);
                            rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);
                            continue;
                        }
                        if (n[i] == 5)
                        {
                            if (n[i + 1] != 0)
                            {
                                rs += " " + rch[n[i]];
                                rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);
                                continue;
                            }
                        }
                    }
                    rs += (rs == "" ? " " : " ") + ch[n[i]];
                    rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);
                }
                if (!not_currency)
                {
                    if (rs[rs.Length - 1] != ' ')
                        rs += " đồng.";
                    else
                        rs += "đồng.";
                }
                if (rs.Length > 2)
                {
                    string rs1 = rs.Substring(0, 2);
                    rs1 = rs1.ToUpper();
                    rs = rs.Substring(2);
                    rs = rs1 + rs;
                }
                return rs.Trim().Replace(",", "");
            }
            catch
            {
                return "Số bạn nhập vào quá lớn";
            }
        }

        public static string RandomText(int lent)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var stringChars = new char[lent];
            var random = new Random();
            for (int i = 0; i < stringChars.Length; i++)
                stringChars[i] = chars[random.Next(chars.Length)];
            var finalString = new String(stringChars);
            return finalString;
        }

        public static string GetAgeFromText(string age)
        {
            if (age == "00:00:00")
                return "0 tháng";
            var arr = age.Split(' ');
            var length = arr.Length;
            int year = 0, month = 0, day = 0;
            for (int i = 0; i < arr.Length - 1; i += 2)
            {
                var text = arr[i + 1];
                if (text.Contains("year"))
                    year = Convert.ToInt32(arr[i]);
                if (text.Contains("mon"))
                    month = Convert.ToInt32(arr[i]);
                if (text.Contains("day"))
                    day = Convert.ToInt32(arr[i]);
            }
            if (year == 0 && month == 0)
                return day + " ngày";
            return year > 6 ? arr[length - 1].ToString() : year * 12 + month + " tháng";
        }

        public static string ObjNotificationMobile(Guid patient_id, string dataNotify, string prefix = "")
        {
            string data = "{patient_id:'" + patient_id + "', data_notification:\"" + dataNotify;
            data += (prefix != "" ? "&" + prefix : "") + "\"}";
            return data;
        }

        public static string ObjNotificationMobile2(Guid patient_id, string dataNotify, string prefix = "")
        {
            string data = "{patient_id:'" + patient_id + "', data_notification:'" + dataNotify;
            data += (prefix != "" ? "&" + prefix : "") + "'}";
            return data;
        } 

        public static string SizeSuffix(long value, int decimalPlaces = 0)
        {
            string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
            if (value < 0)
            {
                throw new ArgumentException("Bytes should not be negative", "value");
            }
            var mag = (int)Math.Max(0, Math.Log(value, 1024));
            var adjustedSize = Math.Round(value / Math.Pow(1024, mag), decimalPlaces);
            return string.Format("{0} {1}", adjustedSize, SizeSuffixes[mag]);
        }
    }
}
