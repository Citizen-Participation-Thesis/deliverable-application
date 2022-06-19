using System;
using System.Collections.Generic;

namespace Models
{
    public interface IAlternativeData
    {
        public event Action AlternativeSet;
        
        public List<string> GetAlternatives();
        public int GetCurrentAlternativeIndex();
        
        public void SetAlternative(int alternativeIndex);
    }
}