
using System.Collections.Generic;
using Microsoft.AspNetCore.Http; //[NOTE: for file]
using OE.Data;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public interface IStudentDetailsServ
    {
        #region "Get Function Definitions"
        IEnumerable<StudentDetails> GetStudentDetails();
        StudentDetails GetStudentDetailsById(long id);
        #endregion "Get Function Definitions"

        #region "Insert Update Delete Function Definitions"
        void InsertStudentDetails(StudentDetails studentDetails, IFormFile file, string webRoot, long? regIemTypeId);
        void UpdateStudentDetails(StudentDetails studentDetails, IFormFile file, string webRoot, long? regIemTypeId);

        void DeleteStudentDetails(StudentDetails studentDetails);
        void DeleteStaticFile(StudentDetails students, string rootPath, long? regItemTypeId);

        #endregion "Insert Update Delete Function Definitions"        
    }
}
