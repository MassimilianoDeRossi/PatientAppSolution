using System;
using PatientApp.Services;
using Xamarin.Forms;

namespace PatientApp.Views
{
    public class BaseContentPage : ContentPage
    {

        public BaseContentPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            int top = 0;
            switch (Device.RuntimePlatform)
            {
                case Device.iOS: top = 20; break;
                case Device.Android: top = 0; break;                
            }
            
            this.Padding = new Thickness(0, top, 0, 0);
            Xamarin.Forms.PlatformConfiguration.iOSSpecific.Page.SetUseSafeArea(this, true);            

            this.BindingContextChanged += BaseContentPage_BindingContextChanged;            
        }

        protected override void OnAppearing()
        {
            MessagingCenter.Send<BaseContentPage>(this, Messaging.Messages.VIEW_APPEARING_MESSAGE);
            this.BackgroundColor = (Color)Application.Current.Resources["DefaultBackgroundColor"];
            base.OnAppearing();
        }        

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Send<BaseContentPage>(this, Messaging.Messages.VIEW_DISAPPEARING_MESSAGE);
            BindingContextChanged -= BaseContentPage_BindingContextChanged;
            GC.Collect();
        }
        

        private void BaseContentPage_BindingContextChanged(object sender, EventArgs e)
        {
            MessagingCenter.Send<BaseContentPage>(this, Messaging.Messages.VIEW_INIT_MESSAGE);
        }

        /// <summary>
        /// Catch android back button pressed, disable default behavior and delegate management
        /// to the NavigationController 
        /// </summary>
        /// <returns></returns>
        protected override bool OnBackButtonPressed()
        {
            if (Navigation.NavigationStack.Count <= 1)
            {
                // Default exit app behavior
                return false;
            }
            else
            {
                MessagingCenter.Send<BaseContentPage>(this, Messaging.Messages.ANDROID_BACKBUTTON_PRESSED);
                return true;
            }
        }

    }
}
