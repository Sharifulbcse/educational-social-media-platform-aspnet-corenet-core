
using System;
using System.Collections.Generic;

using OE.Data;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public interface IInsCategoriesServ
    {
        #region "Function Definitions"
        IEnumerable<InsCategories> GetInsCategory();
        InsCategories GetInsCategoryById(Int64 id);
        IEnumerable<GetInsCategoryAndCountries> GetInsCategoryAndCountries();
        #endregion "Function Definitions"

        #region "Insert_Update_Delete Function Definitions"
        void InsertInsCategory(InsCategories insCategory);
        void UpdateInsCategory(InsCategories insCategory);
        void DeleteInsCategory(InsCategories insCategory);
        #endregion "Insert_Update_Delete Function Definitions"

        #region "Dropdown Function Definitions"
        IEnumerable<dropdown_InsCategories> Dropdown_InsCategory();
        #endregion "Dropdown Function Definitions"
    }
}
