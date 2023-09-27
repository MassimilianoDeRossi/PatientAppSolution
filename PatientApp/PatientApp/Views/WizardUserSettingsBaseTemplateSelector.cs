using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace PatientApp.Views
{
    public class WizardUserSettingsBaseTemplateSelector : DataTemplateSelector
    {
        protected readonly List<DataTemplate> Templates = new List<DataTemplate>();

        public int ItemsCount
        {
            get
            {
                return Templates != null ? Templates.Count : 0;
            }
        }

        public WizardUserSettingsBaseTemplateSelector()
        {
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is int)
            {
                var index = (int)item;
                if (index >= 0 && index < Templates.Count)
                    return Templates[index];   
            }
            return null;
        }
    }
}

