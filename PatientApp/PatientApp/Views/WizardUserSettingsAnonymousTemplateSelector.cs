using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace PatientApp.Views
{
    public class WizardUserSettingsAnonymousTemplateSelector : WizardUserSettingsBaseTemplateSelector
    {       
        public WizardUserSettingsAnonymousTemplateSelector()
        {
            Templates.Add(new DataTemplate(typeof(WizardWelcomeAnonymousTemplate)));
            Templates.Add(new DataTemplate(typeof(WizardUserProfileTemplate)));
            Templates.Add(new DataTemplate(typeof(WizardPinSiteCareTemplate)));
        }
     
    }
}

