using Xamarin.Forms;

namespace PatientApp
{
    /// <summary>
    /// Class used to underline button text. Implementations must be managed on different platform.
    /// </summary>
    public class UnderlineEffect : RoutingEffect
    {
        public const string EffectNamespace = "Example";

        public UnderlineEffect() : base($"{EffectNamespace}.{nameof(UnderlineEffect)}")
        {
        }
    }
}