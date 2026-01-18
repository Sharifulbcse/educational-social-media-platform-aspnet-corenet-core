using System;
using System.Collections.Generic;

using OE.Data;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public interface IDistributionMarksServ
    {
        #region "Get Function Definitions"   
        GetDistributionMarkList GetDistributionMarkList(long institutionId, long disMADId, long classId);
        DistributionMarks GetDistributionMarkById(Int64 id);
        IEnumerable<DistributionMarks> GetDistributionMarks();
        IEnumerable<GetDisMarkSubMark> getDisMarkSubMarks(long institutionId, long classId = 0);

        #endregion "Get Function Definitions"

        #region "Insert Update Delete Function Definitions"
        string InsertDistributionMarks(InsertDistributionMark obj);
        string InsertUpdateDistributionMarks(InsertUpdateDistributionMark obj);
        void UpdateDistributionMarks(DistributionMarks distributionMarks);
        void DeleteDistributionMarks(DistributionMarks distributionMarks);
        #endregion "Insert Update Delete Function Definitions"
    }
}
