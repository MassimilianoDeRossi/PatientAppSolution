using PatientApp.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;


[assembly: ExportEffect(typeof(NoScrollListViewEffect), nameof(NoScrollListViewEffect))]
namespace PatientApp.iOS
{
    public class NoScrollListViewEffect : PlatformEffect
    {
        private UITableView _NativeList => Control as UITableView;

        protected override void OnAttached()
        {
            if (_NativeList != null)
            {
                _NativeList.ScrollEnabled = false;
            }
        }

        protected override void OnDetached()
        {
        }
    }
}