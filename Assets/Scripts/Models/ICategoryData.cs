using System;
using System.Collections.Generic;

namespace Models
{
    public interface ICategoryData : IAlternativeData
    {
        public event Action CategorySet;

        public List<string> GetCategories();
        public int GetCurrentCategoryIndex();
        
        public void SetCategory(int categoryIndex);
    }
}