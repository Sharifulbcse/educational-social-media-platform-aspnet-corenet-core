using System;
using System.IO;
using System.Text;
using System.Drawing; //[NOTE: for 'Image']
using System.Security.Cryptography; //[NOTE: Ramdome value generator]

using Microsoft.AspNetCore.Http;//[NOTE: for 'IFormFile']
using Microsoft.Extensions.Caching.Memory; //[NOTE: for delete cache]

using MimeKit;  //[NOTE: for email]
using MailKit.Net.Smtp; //[NOTE: for email]

namespace OE.Service
{
    public class CommonServ: ICommonServ
    {
        
        #region "Image Functions"       
        public bool Comm_ImageFormat(string imgName, IFormFile file, string webRootPath, string dbImgPath, int imgHeight, int imgWidth, string imgExt)
        {
            try
            {
                //[NOTE: Convert FileType into ImageType]
                Image img = Image.FromStream(file.OpenReadStream(), true, true);

                //[NOTE: Create Directory]
                string fullPath = Path.Combine(webRootPath, dbImgPath);
                Directory.CreateDirectory(fullPath);

                //[NOTE: Creating empty canvas]
                var draw_NewImage = new Bitmap(imgHeight, imgWidth);

                //[NOTE: Drawing image inside empty canvas]
                using (var g = Graphics.FromImage(draw_NewImage))
                {
                    g.DrawImage(img, 0, 0, imgHeight, imgWidth);                   
                    draw_NewImage.Save(fullPath + imgName+ imgExt);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


        #endregion "Image Functions"

        #region "File Functions"
        public bool Comm_FileSave(Int64 Id, IFormFile file, string savePath, string dp, string extention)
        {
            try
            {
                //[NOTE: Create Directory]
                string fullPath = Path.Combine(savePath, dp);
                Directory.CreateDirectory(fullPath);

                var saveFullPath = fullPath + Id + "." + extention;

                //[NOTE: upload file]
                using (var stream = new FileStream(saveFullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DelFileFromLocation(string path)
        {
            if ((File.Exists(path)))
            {
                File.Delete(path);
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion "File Functions"

        #region "Paragraph Functions"        
        public string Comm_ParagraphFormatForView(string msg)
        {
            try
            {
                string msgFormat = msg.Replace("[nl]", "<br/>");
                msgFormat = msgFormat.Replace("[sp]", " ");
                return msgFormat;

            }
            catch (Exception)
            {
                return "";
            }
        }

        public string Comm_ParagraphFormatForStore(string msg)
        {
            try
            {
                //[NOTE: Need to create function which are now in html pages]
                return null;

                //text = document.getElementById('txtEditWcShortDescriptionValue').value;
                //text = text.replace(/ / g, "[sp]");
                //text = text.replace(/\n / g, "[nl]");
                //document.getElementById('txtEditWcShortDescription').value = text;

            }
            catch (Exception)
            {
                return "";
            }
        }
        #endregion "Format Functions"

        
        #region "RandomValue Generator Functions" 
        public string Comm_GetUniqueKey(int maxSize)
        {
            char[] chars = new char[62];
            chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[1];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[maxSize];
                crypto.GetNonZeroBytes(data);
            }
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }


        #endregion "RandomValue Generator Functions" 

        #region "Email Functions"
        public bool Comm_SendMail(string senderName, string senderMail, string senderPassword, string receiverName, string receiverMail, string subject, string msgBody, string smtpServer, int smtpPort, bool ssl)
        {
            try
            {
                //Instantiate mimmessage
                var msg = new MimeMessage();                
                msg.From.Add(new MailboxAddress(senderName, senderMail));               
                msg.To.Add(new MailboxAddress(receiverName, receiverMail));                
                msg.Subject = subject;              
                msg.Body = new TextPart("plain")
                {
                    Text = msgBody
                };
                //[NOTE:Configure smtp and Send Mail]
                using (var client = new SmtpClient())
                {
                    client.Connect(smtpServer, smtpPort, ssl);
                    client.Authenticate(senderMail, senderPassword);
                    client.Send(msg);
                    client.Disconnect(true);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion "Email Functions"

        #region "Date Time Functions"
        public int CommDate_CurrentYear()
        {
            var systemYear = DateTime.Now.Year;
            return systemYear;
        }
        public Nullable<DateTime> CommDate_ConvertToLocalDate(DateTime dateValue)
        {
            if (dateValue!=null){
                DateTime tempDate = DateTime.Parse(dateValue.ToString("u"));
                dateValue = tempDate.ToLocalTime();
            }
            return dateValue;            
        }
        public Nullable<DateTime> CommDate_ConvertToUtcDate(DateTime dateValue)
        {
            if (dateValue != null)
            {
                DateTime tempDate = DateTime.Parse( dateValue.ToString("u"));
                dateValue = tempDate.ToUniversalTime();
            }
            return dateValue;            
        }
        #endregion "Date Time Functions"

        #region "Encrypt and Decrypt Functions"
        public string CommString_EncryptString(string text)
        {
            var key = Encoding.UTF8.GetBytes("E546C8DF278CD5931069B522E695D4F2");

            using (var aesAlg = Aes.Create())
            {
                using (var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
                {
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }

                        var iv = aesAlg.IV;

                        var decryptedContent = msEncrypt.ToArray();

                        var result = new byte[iv.Length + decryptedContent.Length];

                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

                        return Convert.ToBase64String(result);
                    }
                }
            }
        }
        public string CommString_DecryptString(string cipherText)
        {
            var key = Encoding.UTF8.GetBytes("E546C8DF278CD5931069B522E695D4F2");
            var fullCipher = Convert.FromBase64String(cipherText);

            var iv = new byte[16];
            var cipher = new byte[fullCipher.Length - iv.Length];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, fullCipher.Length - iv.Length);
                      

            using (var aesAlg = Aes.Create())
            {
                using (var decryptor = aesAlg.CreateDecryptor(key, iv))
                {
                    string result;
                    using (var msDecrypt = new MemoryStream(cipher))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                    return result;
                }
            }
        }
        #endregion"Encrypt and Decrypt Functions"
    }
}
