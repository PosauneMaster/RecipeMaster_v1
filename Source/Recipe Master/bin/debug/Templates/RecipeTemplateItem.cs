using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BendSheets
{
    [Serializable]
    public class RecipeTemplateItem : IEquatable<RecipeTemplateItem>
    {
        public string Name
        { get; set; }

        public RecipeTemplateItemType ItemType
        { get; set; }

        public string CellStart
        { get; set; }

        public string CellEnd
        { get; set; }

        public RecipeTemplateItem() { }

        public RecipeTemplateItem(string name, RecipeTemplateItemType itemType, string cellStart, string cellEnd)
        {
            Name = name;
            ItemType = itemType;
            CellStart = cellStart;
            CellEnd = cellEnd;
        }

        #region IEquatable<RecipeTemplateItem> Members

        public bool Equals(RecipeTemplateItem other)
        {
            return Name.Equals(other.Name) && ItemType.Equals(other.ItemType) && CellStart.Equals(other.CellStart) && CellEnd.Equals(other.CellEnd);
        }

        #endregion
    }

    public enum RecipeTemplateItemType
    {
        Cell,
        Range
    }
}
