using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace AFA.Web.Helpers
{
    public class CheckboxList : IEnumerable<CheckboxItem>
    {
        public CheckboxList(IEnumerable items)
            : this(items, null /* selectedValues */)
        {
        }

        public CheckboxList(IEnumerable items, IEnumerable selectedValues)
            : this(items, null /* dataValuefield */, null /* dataTextField */, selectedValues)
        {
        }

        public CheckboxList(IEnumerable items, string dataValueField, string dataTextField)
            : this(items, dataValueField, dataTextField, null /* selectedValues */)
        {
        }

        public CheckboxList(IEnumerable items, string dataValueField, string dataTextField, IEnumerable selectedValues)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            Items = items;
            DataValueField = dataValueField;
            DataTextField = dataTextField;
            SelectedValues = selectedValues;
        }

        public string DataTextField
        {
            get;
            private set;
        }

        public string DataValueField
        {
            get;
            private set;
        }

        public IEnumerable Items
        {
            get;
            private set;
        }

        public IEnumerable SelectedValues
        {
            get;
            private set;
        }

        public virtual IEnumerator<CheckboxItem> GetEnumerator()
        {
            return GetListItems().GetEnumerator();
        }

        internal IList<CheckboxItem> GetListItems()
        {
            return (!String.IsNullOrEmpty(DataValueField)) ?
                GetListItemsWithValueField() :
                GetListItemsWithoutValueField();
        }

        private IList<CheckboxItem> GetListItemsWithValueField()
        {
            HashSet<string> selectedValues = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (SelectedValues != null)
            {
                selectedValues.UnionWith(from object value in SelectedValues select Convert.ToString(value, CultureInfo.CurrentCulture));
            }

            var listItems = from object item in Items
                            let value = Eval(item, DataValueField)
                            select new CheckboxItem
                            {
                                Id = value,
                                Name = Eval(item, DataTextField),
                                Checked = selectedValues.Contains(value)
                            };
            return listItems.ToList();
        }

        private IList<CheckboxItem> GetListItemsWithoutValueField()
        {
            HashSet<object> selectedValues = new HashSet<object>();
            if (SelectedValues != null)
            {
                selectedValues.UnionWith(SelectedValues.Cast<object>());
            }

            var listItems = from object item in Items
                            select new CheckboxItem
                            {
                                Name = Eval(item, DataTextField),
                                Checked = selectedValues.Contains(item)
                            };
            return listItems.ToList();
        }

        private static string Eval(object container, string expression)
        {
            object value = container;
            if (!String.IsNullOrEmpty(expression))
            {
                value = DataBinder.Eval(container, expression);
            }
            return Convert.ToString(value, CultureInfo.CurrentCulture);
        }

        #region IEnumerable Members
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}