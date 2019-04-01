using System;

namespace ExtendedCase.Dal.Data
{
    public class ExtendedCaseFormStateItem 
    {
        public int Id { get; set; }
        public int ExtendedCaseDataId { get; set; }
        public string TabId { get; set; }
        public string SectionId { get; set; }
        public int SectionIndex { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        public bool Equals(ExtendedCaseFormStateItem item)
        {
            if (item == null)
                return false;
            
            return item.TabId.Equals(TabId, StringComparison.OrdinalIgnoreCase) &&
                   item.SectionId.Equals(SectionId, StringComparison.OrdinalIgnoreCase) &&
                   item.SectionIndex == SectionIndex &&
                   item.Key.Equals(Key, StringComparison.OrdinalIgnoreCase);
        }
    }
}