using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

using OE.Data;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public interface IStudentsServ
    {

        #region "Get Function Definitions"

        string CheckValidUserLoginIdAndRoll(string userLoginId, long institutionId, string oldUserLoginId, long admittedYear, long admittedPYear, long admittedClassId, long ddlClassId, long rollNo, long oldRollNo);
        Students GetStudentsById(long id, long instituteId, long userId);
        StudentPromotions GetStudentPromotionsById(long Id);
        GetStudentDetails GetStudentDetails(long institutionId, long studentId);
        GetStudentLicenses GetStudentLicenses(int year, long institutionId, int ddlLicenseId, long classId);
        GetStudentList GetStudentList(long institutionId, int year, long classId);

        IEnumerable<StudentLicensesValidation> StudentLicensesValidation(List<StudentLicensesValidation> obj);
        IEnumerable<Students> GetStudents();       
        IEnumerable<GetStudentPromotions> GetStudentPromotions(long institutionId, long fromYear, long classId);
        IEnumerable<GetStudentPromotions> PromotedStudents(long institutionId, int year);
        #endregion "Get Function Definitions"

        #region "Insert update and delete Function Definitions"        
        string InsertStudents(InsertStudents insertStudents, IFormFile file, string webRootPath);
        void InsertStudentPromotions(InsertStudentPromotions obj);
        InsertUpdateStudentLicenses InsertUpdateStudentLicenses(InsertUpdateStudentLicenses obj);

        void UpdateStudents(UpdateStudents students, IFormFile file, string webRootPath);
        void UpdateStudentPromotions(UpdateStudentPromotions obj);

        void DeleteStudents(Students students);
        void DeleteStaticFile(Students students, string rootPath);
        void DeleteStudentPromotions(DeleteStudentPromotions obj);
        #endregion "Insert update and delete Function Definitions"

        #region "Dropdown Function Definitions"
        IEnumerable<dropdown_Students> Dropdown_Students(long institutionId);
        #endregion "Dropdown Function Definitions"


    }
}
