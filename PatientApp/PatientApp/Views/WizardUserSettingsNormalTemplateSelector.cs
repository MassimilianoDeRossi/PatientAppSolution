using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace PatientApp.Views
{
    public class WizardUserSettingsNormalTemplateSelector : WizardUserSettingsBaseTemplateSelector
    {       
        public WizardUserSettingsNormalTemplateSelector()
        {
            Templates.Add(new DataTemplate(typeof(WizardWelcomeLoggedTemplate)));
            Templates.Add(new DataTemplate(typeof(WizardUserProfileTemplate)));
            Templates.Add(new DataTemplate(typeof(WizardPinSiteCareTemplate)));
            Templates.Add(new DataTemplate(typeof(WizardPersonalGoalTemplate)));
            Templates.Add(new DataTemplate(typeof(WizardInsightMessagesTemplate)));
        }     
    }
}

