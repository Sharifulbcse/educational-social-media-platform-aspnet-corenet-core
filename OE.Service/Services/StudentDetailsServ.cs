using System.Linq;
using System.IO;
using System.Collections.Generic;

using Microsoft.AspNetCore.Http; //[NOTE: for file]

using OE.Data;
using OE.Repo;

namespace OE.Service
{
    public class StudentDetailsServ : CommonServ, IStudentDetailsServ
    {
        #region "Variables"
        private readonly IStudentDetailsRepo<StudentDetails> _studentDetailsRepo;
        #endregion "Variables"

        #region "Constructor"
        public StudentDetailsServ(IStudentDetailsRepo<StudentDetails> studentDetailsRepo)
        {
            _studentDetailsRepo = studentDetailsRepo;
        }
        #endregion "Constructor"

        #region "Get Methods"
        public IEnumerable<StudentDetails> GetStudentDetails()
        {
            var queryAll = _studentDetailsRepo.GetAll();
            var query = from e in queryAll
                        select e;
            return query;
        }
        public StudentDetails GetStudentDetailsById(long id)
        {
            var queryAll = _studentDetailsRepo.GetAll();
            var query = (from e in queryAll
                         where e.Id == id
                         select e).SingleOrDefault();
            return query;
        }
        #endregion "Get Methods"

        #region "Insert Update Delete Methods"
        public void InsertStudentDetails(StudentDetails studentDetails, IFormFile file, string webRoot, long? regIemTypeId)
        {
            _studentDetailsRepo.Insert(studentDetails);
            var fetchLastRecord = _studentDetailsRepo.GetAll().Last();
            var msg = (dynamic)null;
            if (file != null && regIemTypeId == 1)
            {
                string dbPath = "ClientDictionary/StudentDetails/FilePathValue/";
                string ext = Path.GetExtension(file.FileName);
                ext = ext.Remove(0, 1);
                if (Comm_FileSave(fetchLastRecord.Id, file, webRoot, dbPath, ext))
                {
                    msg = "File successfully saved.";
                    fetchLastRecord.FilePathValue = dbPath + fetchLastRecord.Id + "." + ext;
                }
                else { msg = "Error occured while saving file."; }
            }
            if (file != null && regIemTypeId == 2)
            {
                string dbPath = "ClientDictionary/StudentDetails/ImagePathValue/";
                string ext = ".png";
                if (Comm_ImageFormat(studentDetails.Id.ToString(), file, webRoot, dbPath, 300, 200, ext))
                {
                    msg = "File successfully saved.";
                }
                else { msg = "Error occured while saving file."; }
                fetchLastRecord.ImagePathValue = dbPath + fetchLastRecord.Id + ext;
            }
            _studentDetailsRepo.Update(fetchLastRecord);
        }

        public void UpdateStudentDetails(StudentDetails studentDetails, IFormFile file, string webRoot, long? regIemTypeId)
        {
            var msg = (dynamic)null;
            if (file != null && regIemTypeId == 1)
            {
                string dbPath = "ClientDictionary/StudentDetails/FilePathValue/";
                string ext = Path.GetExtension(file.FileName);
                ext = ext.Remove(0, 1);
                var previousFile = Path.Combine(webRoot, studentDetails.FilePathValue == null ? "" : studentDetails.FilePathValue);
                if (File.Exists(previousFile))
                {
                    File.Delete(previousFile);
                }
                if (Comm_FileSave(studentDetails.Id, file, webRoot, dbPath, ext))
                {
                    msg = "File successfully saved.";
                    studentDetails.FilePathValue = dbPath + studentDetails.Id + "." + ext;
                }
                else { msg = "Error occured while saving file."; }
            }
            if (file != null && regIemTypeId == 2)
            {
                string dbPath = "ClientDictionary/StudentDetails/ImagePathValue/";
                string ext = ".png";

                if (Comm_ImageFormat(studentDetails.Id.ToString(), file, webRoot, dbPath, 600, 400, ext))
                {
                    msg = "File successfully saved.";
                }
                else { msg = "Error occured while saving file."; }
                studentDetails.ImagePathValue = dbPath + studentDetails.Id + ext;
            }
            _studentDetailsRepo.Update(studentDetails);
        }
        public void DeleteStudentDetails(StudentDetails studentDetails)
        {
            _studentDetailsRepo.Delete(studentDetails);
        }
        public void DeleteStaticFile(StudentDetails students, string rootPath, long? regItemTypeId)
        {
            var msg = (dynamic)null;
            if (regItemTypeId == 1)
            {
                if (DelFileFromLocation(Path.Combine(rootPath, students.FilePathValue)))
                {
                    msg = "File deleted.";
                    students.FilePathValue = (dynamic)null;
                    _studentDetailsRepo.Update(students);
                }
                else
                    msg = "Error Occured.";
            }
            if (regItemTypeId == 2)
            {
                if (DelFileFromLocation(Path.Combine(rootPath, students.ImagePathValue)))
                {
                    msg = "File deleted.";
                    students.ImagePathValue = (dynamic)null;
                    _studentDetailsRepo.Update(students);
                }
                else
                    msg = "Error Occured.";
            }
        }

        #endregion "Insert Update Delete Methods"        
    }
}
