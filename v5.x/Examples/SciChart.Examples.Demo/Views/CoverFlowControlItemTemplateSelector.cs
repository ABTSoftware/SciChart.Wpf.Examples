

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using SciChart.Charting.Common.Helpers;

namespace SciChart.Examples.Demo.Views
{
    public class ItemTemplateKey
    {
        public string Key { get; set; }
        public DataTemplate DataTemplate { get; set; }
    }

    public class CoverFlowControlItemTemplateSelector : DataTemplateSelector
    {
        private ObservableCollection<ItemTemplateKey> _templateKeys;

        public CoverFlowControlItemTemplateSelector()
        {
            TemplateKeys = new ObservableCollection<ItemTemplateKey>();
        }

        public ObservableCollection<ItemTemplateKey> TemplateKeys
        {
            get { return _templateKeys; }
            set
            {
                if (_templateKeys != null) _templateKeys.CollectionChanged -= OnCollectionChanged;
                _templateKeys = value;
                if (_templateKeys != null) _templateKeys.CollectionChanged += OnCollectionChanged;

                UpdateControlTemplate();
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateControlTemplate();
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {            
            string value = item as string;

            if (value != null)
            {
                var templateKey = TemplateKeys != null ? TemplateKeys.FirstOrDefault(t => t.Key == value) : null;
                if (templateKey != null)
                {
                    return templateKey.DataTemplate;
                }
                return base.SelectTemplate(item, container);
            }
            else
                return base.SelectTemplate(item, container);
        }
    }
}
