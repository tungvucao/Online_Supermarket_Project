using System.Text;
using System.Text.RegularExpressions;

namespace Online_Supermarket_Project.Helpper
{
    public static class Utilities
    {
        public static bool ValidEmail(string email)
        {
            if (email.Trim().EndsWith(".")) return false;
            try
            {
                var add = new System.Net.Mail.MailAddress(email);
                return add.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static int PAGE_SIZE = 10;
        public static void CreateIfMissing(string path)
        {
            bool folderExists = Directory.Exists(path);
            if (!folderExists)
            {
                Directory.CreateDirectory(path);
            }
        }

        public static string ToTitleCase(string str)
        {
            string result = str;
            if (!string.IsNullOrEmpty(str))
            {
                var words = str.Split(' ');
                for (int i = 0; i < words.Length; i++)
                {
                    var s = words[i];
                    if (s.Length > 0)
                    {
                        words[i] = s[0].ToString().ToUpper() + s.Substring(1);
                    }
                }
                result = string.Join(" ", words);
            }
            return result;
        }

        public static bool ToInteger(string str)
        {
            Regex regex = new Regex(@"^[0-9]+$");

            try
            {
                if (String.IsNullOrWhiteSpace(str)) return false;
                if (!regex.IsMatch(str)) return false;
                return true;
            }
            catch 
            {

            }
            return false;
        }

        public static string GetRandomKey(int length = 5)
        {
            string pattern = @"0123456789zxcvbnmasdfghjklqưertyuiop[]{}:~!@#$%^&*()+";
            Random rd = new Random();
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0;i < length;i++)
            {
                stringBuilder.Append(pattern[rd.Next(0, pattern.Length)]);
            }

            return stringBuilder.ToString();
        }

        public static string ToUrlFriendly2(this string url)
        {
            var result = url.ToLower();
            result = Regex.Replace(result, @"[áàảãạâấầẩẫậăắằẳẵặ]", "a");
            result = Regex.Replace(result, @"[éèẻẽẹêếềểễệ]", "e");
            result = Regex.Replace(result, @"[óòỏõọôốồổỗộơớờởỡợ]", "o");
            result = Regex.Replace(result, @"[úùủũụưúừửữự]", "u");
            result = Regex.Replace(result, @"[íìỉĩị]", "i");
            result = Regex.Replace(result, @"[ýỳỷỹỵ]", "y");
            result = Regex.Replace(result, @"[đ]", "d");
            result = Regex.Replace(result.Trim(), @"[^a-z0-9-]", "").Trim();
            result = Regex.Replace(result.Trim(), @"\s+", "-").Trim();
            result = Regex.Replace(result.Trim(), @"\s", "-").Trim();
            
            while (true) { 
                if (result.IndexOf("--") != -1) {
                    url = url.Replace("--", "-");
                }
                else { break; }

            }
            return result;
        }

        public static string ToUrlFriendly(string url)
        {
            url = url.ToLower();
            url = Regex.Replace(url, @"[áàảãạâấầẩẫậăắằẳẵặ]", "a");
            url = Regex.Replace(url, @"[éèẻẽẹêếềểễệ]", "e");
            url = Regex.Replace(url, @"[óòỏõọôốồổỗộơớờởỡợ]", "o");
            url = Regex.Replace(url, @"[úùủũụưúừửữự]", "u");
            url = Regex.Replace(url, @"[íìỉĩị]", "i");
            url = Regex.Replace(url, @"[ýỳỷỹỵ]", "y");
            url = Regex.Replace(url, @"[đ]", "d");

            url = Regex.Replace(url.Trim(), @"[^0-9a-zA-Z-\s]", "").Trim();
            url = Regex.Replace(url.Trim(), @"\s+", "-");
            url = Regex.Replace(url, @"\s", "-");

            while (true)
            {
                if (url.IndexOf("--") != -1)
                {
                    url = url.Replace("--", "-");
                }
                else { break; }

            }
            return url;

        }

        public static async Task<string> UploadFile(Microsoft.AspNetCore.Http.IFormFile file, string sDirectory, string newname)
        {
            try
            {
                if (newname == null) newname = file.FileName;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", sDirectory);
                CreateIfMissing(path);
                string pathFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", sDirectory, newname);

                var supportedTypes = new[] { "jpg", "jpeg", "png" };
                var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
                if (!supportedTypes.Contains(fileExt.ToLower()))
                {
                    return null;
                }
                else
                {
                    using (var stream = new FileStream(pathFile, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    return newname;
                }
            }
            catch(Exception e) {
                return null;
            }
        }
    }
}
