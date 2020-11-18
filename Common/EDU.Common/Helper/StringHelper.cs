using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EDU.Common.Helper
{
    public static class StringHelper
    {
        private static Random random = new Random();

        public static string CorrectFilePath(this string s)
        {
            s = s.Replace(" ", "_");
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }

        public static string CapitalizeWords(string value)
        {
            if (value != null)
            {
                char[] array = value.ToCharArray();
                if (array.Length >= 1)
                {
                    array[0] = char.ToUpper(array[0]);
                }
                for (int i = 1; i < array.Length; i++)
                {
                    if (array[i - 1] == ' ')
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                    else
                    {
                        array[i] = char.ToLower(array[i]);
                    }
                }
                return new string(array);
            }
            return null;
        }

        public static string Repeat(this string value, int count)
        {
            return new StringBuilder(value.Length * count).Insert(0, value, count).ToString();
        }

        public static string FullTextTrim(this string s)
        {
            return s.RemoveSpecialCharacters().TrimStart(' ').TrimEnd(' ');
        }

        public static string RemoveSpecialCharacters(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_' || c == '\'')
                    sb.Append(c);
            return sb.ToString();
        }


        public static string RandomString(int len)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, len).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string ConvertToUnSign(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return "";
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }

        public static string removeSpecialCharacter(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return "";
            return Regex.Replace(s, @"[^0-9a-zA-Z ]+", string.Empty);
        }

        public static string removeSpecialCharacterForVN(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return "";
            return Regex.Replace(s, @"[^0-9a-zA-Z ýếÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ]+", string.Empty);
        }

        public static string ReplaceMultipleSpaceToOne(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return "";
            s = ("" + s).Trim();
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);
            return regex.Replace(s, " ");
        }

        public static string ConvertTo_TsQuery(this string sdata)
        {
            string data = sdata.ConvertToUnSign().removeSpecialCharacter().ReplaceMultipleSpaceToOne();
            if (string.IsNullOrEmpty(data))
                return "";
            return data.Replace(" ", "&").ToLower();
        }

        public static string ConvertTo_TsVector(this string sdata)
        {
            string data = ReplaceMultipleSpaceToOne(removeSpecialCharacter(ConvertToUnSign(sdata))).ToLower();
            if (string.IsNullOrEmpty(data))
                return "";
            var vdata = data.Split(new string[] { " " }, StringSplitOptions.None);
            if (vdata.Length == 0)
                return "";

            var distinData = vdata.Distinct().OrderBy(x => x).ToList();
            string resullt = "";
            foreach (var item in distinData)
            {
                resullt = resullt + " " + item + ":";
                for (int i = 0; i < vdata.Length; i++)
                {
                    if (vdata[i] == item)
                        resullt = resullt + ((i + 1) + ",");
                }

                resullt = resullt.Substring(0, resullt.Length - 1);
            }

            return resullt.Substring(1);
        }

        public static string SlugText(string input, string key = "-")
        {
            input = input.Trim();
            for (int i = 0x20; i < 0x30; i++)
                input = input.Replace(((char)i).ToString(), " ");
            input = input.Replace(".", key);
            input = input.Replace("*", key);
            input = input.Replace("&", key);
            input = input.Replace("#", key);
            input = input.Replace("(", key);
            input = input.Replace(")", key);
            input = input.Replace("[", key);
            input = input.Replace("]", key);
            input = input.Replace(" ", key);
            input = input.Replace(",", key);
            input = input.Replace(";", key);
            input = input.Replace(":", key);
            input = input.Replace("  ", key);
            input = input.Replace("\"", "");
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string str = input.Normalize(NormalizationForm.FormD);
            string str2 = regex.Replace(str, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');
            while (str2.IndexOf("?") >= 0)
                str2 = str2.Remove(str2.IndexOf("?"), 1);
            while (str2.Contains("--"))
                str2 = str2.Replace("--", key).ToLower();
            return str2.ToLower();
        }
    }
}
