using System;
using Microsoft.AspNetCore.Http;//[NOTE: for 'IFormFile']
namespace OE.Service
{
    public interface ICommonServ
    {
        #region "Date Time Function Definitions"
        int CommDate_CurrentYear();
        Nullable<DateTime> CommDate_ConvertToLocalDate(DateTime dateValue);
        Nullable<DateTime> CommDate_ConvertToUtcDate(DateTime dateValue);
        #endregion "Date Time Function Definitions"

        #region "Image Function Definitions"
        bool Comm_ImageFormat(string imgName, IFormFile file, string webRootPath, string dbImgPath, int imgHeight, int imgWidth, string imgExt);
        #endregion "Image Function Definitions"

        #region "File Function Definitions"
        bool Comm_FileSave(Int64 Id, IFormFile file, string savePath, string dp, string extention);

        bool DelFileFromLocation(string path);
        #endregion "File Function Definitions"

        #region "Paragraph Function Definitions"        
        string Comm_ParagraphFormatForView(string msg);

        string Comm_ParagraphFormatForStore(string msg);
        #endregion "Format Function Definitions"

        #region "RandomValue Generator Function Definitions" 
        string Comm_GetUniqueKey(int maxSize);

        #endregion "RandomValue Generator Function Definitions" 

        #region "Email Function Definitions"
        bool Comm_SendMail(string senderName, string senderMail, string senderPassword, string receiverName, string receiverMail, string subject, string msgBody, string smtpServer, int smtpPort, bool ssl);
        #endregion "Email Function Definitions"

        #region "Encrypt and Decrypt Function Definitions"
        string CommString_EncryptString(string text);
        string CommString_DecryptString(string text);
        #endregion "Encrypt and Decrypt Function Definitions"
    }
}
