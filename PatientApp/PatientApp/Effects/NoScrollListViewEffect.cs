using Xamarin.Forms;

namespace PatientApp
{
    /// <summary>
    /// Class used to remove scroll on listview. Implementations must be managed on different platform.
    /// </summary>
    public class NoScrollListViewEffect : RoutingEffect
    {
        public const string EffectNamespace = "Example";

        public NoScrollListViewEffect() : base($"{EffectNamespace}.{nameof(NoScrollListViewEffect)}")
        {
        }
    }
}
