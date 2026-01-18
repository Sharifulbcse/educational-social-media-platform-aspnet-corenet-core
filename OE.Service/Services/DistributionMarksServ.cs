
using System;
using System.Linq;
using System.Collections.Generic;

using OE.Data;
using OE.Repo;
using OE.Service.ServiceModels;
using OE.Service.CustomEntitiesServ;
namespace OE.Service
{
    public class DistributionMarksServ : CommonServ, IDistributionMarksServ
    {
        #region "Variables"
        private readonly IClassesRepo<Classes> _classesRepo;
        private readonly IDistributionMarksRepo<DistributionMarks> _distributionMarksRepo;
        private readonly IMarkTypesRepo<MarkTypes> _markTypesRepo;
        private readonly IOE_InstitutionsRepo<OE_Institutions> _oeInstitutionsRepo;
        private readonly ISubjectsRepo<Subjects> _subjectsRepo;
        #endregion "Variables"

        #region "Constructor"
        public DistributionMarksServ(
            IClassesRepo<Classes> classesRepo,
            IDistributionMarksRepo<DistributionMarks> distributionMarksRepo,
            IOE_InstitutionsRepo<OE_Institutions> oeInstitutionsRepo,
            ISubjectsRepo<Subjects> subjectsRepo, 
            IMarkTypesRepo<MarkTypes> markTypesRepo
            )
        {
            _distributionMarksRepo = distributionMarksRepo;
            _oeInstitutionsRepo = oeInstitutionsRepo;
            _subjectsRepo = subjectsRepo;
            _markTypesRepo = markTypesRepo;
            _classesRepo = classesRepo;
        }
        #endregion "Constructor"

        #region "Get Methods"
        public GetDistributionMarkList GetDistributionMarkList(long institutionId, long disMADId, long classId)
        {
            var result = (dynamic)null;
            var subjectList = (dynamic)null;
            var classs = (dynamic)null;
            var institution = _oeInstitutionsRepo.Get(institutionId);
            var distributionMark = _distributionMarksRepo.GetAll();
            var subject = _subjectsRepo.GetAll();
            var markType = _markTypesRepo.GetAll();
            if (classId != 0)
            {
                classs = _classesRepo.Get(classId);

                var query = from disM in distributionMark
                            where disM.ClassId == classId && disM.DistributionMarkActionDateId == disMADId
                            join s in subject on disM.SubjectId equals s.Id
                            join mt in markType on disM.MarkTypeId equals mt.Id
                            select new { disM, s, mt };

                result = new List<C_DistributionMarks>();
                foreach (var item in query)
                {
                    var temp = new C_DistributionMarks()
                    {
                        Id = item.disM.Id,
                        SubjectId = item.disM.SubjectId,
                        InstitutionId = item.disM.InstitutionId,
                        DistributionMarkActionDateId = item.disM.DistributionMarkActionDateId,
                        MarkTypeId = item.disM.MarkTypeId,
                        ClassId = item.disM.ClassId,
                        BreakDownInP = item.disM.BreakDownInP,
                        SubjectName = item.s.Name,
                        MarkTypeName = item.mt.Name
                    };
                    result.Add(temp);
                }
               
                var querySubject = from s in subject
                                   where s.ClassId == classId
                                   select s;
                
                subjectList = new List<C_Subjects>();
                
                foreach (var item in querySubject)                
                {
                    var temp = new C_Subjects()
                    {
                        Id = item.Id,
                        Name = item.Name
                    };
                    subjectList.Add(temp);
                }
            }           
            var markTypeList = new List<C_MarkTypes>();
            foreach (var item in markType)
            {
                var temp = new C_MarkTypes()
                {
                    Id = item.Id,
                    Name = item.Name
                };
                markTypeList.Add(temp);
            }           
            var model = new GetDistributionMarkList()
            {
                DistributionMark = result,
                Subject = subjectList,               
                MarkType = markTypeList,                
                InstitutionName = institution != null ? institution.Name : "",
                ClassName = classs != null ? classs.Name : ""
            };
            return model;
        }

        public IEnumerable<DistributionMarks> GetDistributionMarks()
        {
            var queryAll = _distributionMarksRepo.GetAll();
            var returnQuery = from e in queryAll
                              select e;
            return returnQuery;
        }

        public DistributionMarks GetDistributionMarkById(Int64 id)
        {
            var queryAll = _distributionMarksRepo.GetAll();
            var returnQuery = (from e in queryAll
                               where e.Id == id
                               select e).SingleOrDefault();
            return returnQuery;
        }

        public IEnumerable<GetDisMarkSubMark> getDisMarkSubMarks(long institutionId, long classId = 0)
        {
            var distributionMark = _distributionMarksRepo.GetAll().ToList();
            var subjects = _subjectsRepo.GetAll().ToList();
            var markType = _markTypesRepo.GetAll().ToList();
            var getClasses = _classesRepo.GetAll().ToList();
            var jointQuery = (dynamic)null;
            if (classId != 0)
            {
                jointQuery = from dm in distributionMark
                             where dm.InstitutionId == institutionId
                             join sub in subjects
                            on dm.SubjectId equals sub.Id
                             join mt in markType
                             on dm.MarkTypeId equals mt.Id
                             join c in getClasses
                             on dm.ClassId equals c.Id
                             where dm.ClassId == classId
                             select new { dm, sub, mt, c };
            }
            else
            {
                jointQuery = from dm in distributionMark
                             where dm.InstitutionId == institutionId
                             join sub in subjects
                            on dm.SubjectId equals sub.Id
                             join mt in markType
                             on dm.MarkTypeId equals mt.Id
                             join c in getClasses
                             on dm.ClassId equals c.Id
                             select new { dm, sub, mt, c };
            }
           

            var queryResult = new List<GetDisMarkSubMark>();
            foreach (var item in jointQuery)
            {
                var obj = new GetDisMarkSubMark()
                {
                    DistributionMarks = item.dm,
                    Subjects = item.sub,
                    MarkTypes = item.mt,
                    Classes = item.c                   

                };
                queryResult.Add(obj);
            }
            return queryResult;
        }
        #endregion "Get Methods"

        #region "Insert Update Delete Methods"
        
        public string InsertDistributionMarks(InsertDistributionMark obj)
        {
            
            string returnResult = (dynamic)null;
            try
            {
                if (obj != null)
                {
                    //[Note: insert 'DistributionMarks' table]
                    if (obj.DistributionMarks != null)
                    {
                        var d = new DistributionMarks()
                        {
                            InstitutionId = obj.DistributionMarks.InstitutionId,
                            SubjectId = obj.DistributionMarks.SubjectId,
                            MarkTypeId = obj.DistributionMarks.MarkTypeId,
                            ClassId = obj.DistributionMarks.ClassId,

                            BreakDownInP = obj.DistributionMarks.BreakDownInP,
                           
                            IsActive = obj.DistributionMarks.IsActive,
                            InsId = obj.DistributionMarks.InsId,
                            AddedBy = obj.DistributionMarks.AddedBy,
                            AddedDate = obj.DistributionMarks.AddedDate
                        };

                        _distributionMarksRepo.Insert(d);
                        returnResult = "Saved";

                    }
                }
            }
            catch(Exception ex)
            {
                returnResult = "ERROR102:DistributionMarksServ/InsertDistributionMarks - " + ex.Message; 
            }
           
            return returnResult;

        }

        public string InsertUpdateDistributionMarks(InsertUpdateDistributionMark obj)
        {
            string returnResult = (dynamic)null;
            try
            {
                if (obj != null)
                {
                    //[Note: insert 'DistributionMarks' table]
                    if (obj._DistributionMarks != null)
                    {
                        foreach (var item in obj._DistributionMarks)
                        {
                            var distributionMark = _distributionMarksRepo.GetAll().Where(d => d.SubjectId == item.SubjectId && d.MarkTypeId == item.MarkTypeId && d.ClassId == item.ClassId && d.DistributionMarkActionDateId == item.DistributionMarkActionDateId).SingleOrDefault();
                            if (distributionMark != null)
                            {
                                distributionMark.SubjectId = item.SubjectId;
                                distributionMark.MarkTypeId = item.MarkTypeId;
                                distributionMark.ClassId = item.ClassId;
                                distributionMark.DistributionMarkActionDateId = item.DistributionMarkActionDateId;
                                distributionMark.BreakDownInP = item.BreakDownInP;
                                distributionMark.ModifiedBy = item.ModifiedBy;
                                distributionMark.ModifiedDate = Convert.ToDateTime(CommDate_ConvertToUtcDate((DateTime)item.ModifiedDate));
                                _distributionMarksRepo.Update(distributionMark);
                                returnResult = "Updated";
                            }
                            else
                            {
                                var d = new DistributionMarks()
                                {
                                    InstitutionId = item.InstitutionId,
                                    SubjectId = item.SubjectId,
                                    ClassId = item.ClassId,
                                    MarkTypeId = item.MarkTypeId,
                                    DistributionMarkActionDateId = item.DistributionMarkActionDateId,
                                    BreakDownInP = item.BreakDownInP,
                                    IsActive = item.IsActive,
                                    AddedBy = item.AddedBy,
                                    AddedDate = Convert.ToDateTime(CommDate_ConvertToUtcDate((DateTime)item.AddedDate))
                                };
                                _distributionMarksRepo.Insert(d);
                                returnResult = "Saved";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                returnResult = "ERROR102:DistributionMarksServ/InsertUpdateDistributionMarks - " + ex.Message;
            }
            return returnResult;
        }

        public void UpdateDistributionMarks(DistributionMarks distributionMarks)
        {
             _distributionMarksRepo.Update(distributionMarks);
           
        }
        public void DeleteDistributionMarks(DistributionMarks distributionMarks)
        {
             _distributionMarksRepo.Delete(distributionMarks);
           
        }
        #endregion "Insert Update Delete Methods"
    }
}
