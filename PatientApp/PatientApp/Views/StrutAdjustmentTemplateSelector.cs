using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace PatientApp.Views
{
    public class StrutAdjustmentTemplateSelector : DataTemplateSelector
    {
        private readonly List<DataTemplate> Templates;

        public int ItemsCount
        {
            get
            {
                return Templates != null ? Templates.Count : 0;
            }
        }

        public StrutAdjustmentTemplateSelector()
        {
            Templates = new List<DataTemplate>();
            Templates.Add(new DataTemplate(typeof(WizardStrutAdjustmentTemplate)));
            Templates.Add(new DataTemplate(typeof(WizardStrutAdjustmentTemplate)));
            Templates.Add(new DataTemplate(typeof(WizardStrutAdjustmentTemplate)));
            Templates.Add(new DataTemplate(typeof(WizardStrutAdjustmentTemplate)));
            Templates.Add(new DataTemplate(typeof(WizardStrutAdjustmentTemplate)));
            Templates.Add(new DataTemplate(typeof(WizardStrutAdjustmentTemplate)));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item != null)
            {
                try
                {
                    var index = (int)item;
                    if (index > Templates.Count - 1)
                        index = Templates.Count - 1;

                    return Templates[index];
                }
                catch (Exception ex)
                {
                    App.Current.MainPage.DisplayAlert("ERROR", ex.Message, "OK");
                }

            }
            return null;
        }
    }
}

