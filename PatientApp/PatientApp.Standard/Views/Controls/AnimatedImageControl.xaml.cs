using System;
using System.Threading;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;

namespace PatientApp.Views.Controls
{
    /// <summary>
    /// User control with animation capabilities 
    /// Display a loop sequence of provided images
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AnimatedImageControl : Grid
    {
        private Timer _timer;
        private int _currentImageIndex, _prevImageIndex;
        private List<Image> _images = new List<Image>();

        public string ImagesNamePrefix
        {
            get { return (string)GetValue(ImagesNamePrefixProperty); }
            set { SetValue(ImagesNamePrefixProperty, value); }
        }

        public static readonly BindableProperty ImagesNamePrefixProperty =
            BindableProperty.Create("ImagesNamePrefix", typeof(string), typeof(AnimatedImageControl), "",
              BindingMode.OneWay, null, (obj, oldValue, newValue) => { (obj as AnimatedImageControl).UpdateControls(); }
              );

        public int ImagesCount
        {
            get { return (int)GetValue(ImagesCountProperty); }
            set { SetValue(ImagesCountProperty, value); }
        }

        public static readonly BindableProperty ImagesCountProperty =
            BindableProperty.Create("ImagesCount", typeof(int), typeof(AnimatedImageControl), 0,
              BindingMode.OneWay, null, (obj, oldValue, newValue) => { (obj as AnimatedImageControl).UpdateControls(); }
              );

        public int FrameDuration
        {
            get { return (int)GetValue(FrameDurationProperty); }
            set { SetValue(FrameDurationProperty, value); }
        }

        public static readonly BindableProperty FrameDurationProperty =
            BindableProperty.Create("FrameDuration", typeof(int), typeof(AnimatedImageControl), 1000,
              BindingMode.OneWay, null, (obj, oldValue, newValue) => { (obj as AnimatedImageControl).UpdateControls(); }
              );

        public AnimatedImageControl()
        {
            InitializeComponent();
            UpdateControls();
        }

        protected void UpdateControls()
        {
            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }

            _images.Clear();

            if (ImagesCount <= 0)
                return;

            for (int i = 1; i <= ImagesCount; i++)
            {
                var image = new Image()
                {
                    Aspect = Aspect.AspectFit,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Opacity = 0,
                };

                try
                {
                    string fileName = string.Concat(this.ImagesNamePrefix, i);
                    image.Source = ImageSource.FromFile(fileName) as FileImageSource;
                    _images.Add(image);
                    this.Children.Add(image);
                }
                catch
                {
                }
            }

            _currentImageIndex = _prevImageIndex = 0;
            _timer = new Timer(TimerCallback, null, 0, FrameDuration);
        }

        bool _working = false;
        private void TimerCallback(object state)
        {
            if (_working)
                return;

            _working = true;

            try
            {
                if (ImagesCount > 0)
                {
                    if (_currentImageIndex == ImagesCount)
                        _currentImageIndex = 0;

                    if (_currentImageIndex >= 0)
                    {
                        _images[_currentImageIndex].Opacity = 1;
                    }


                    if (_prevImageIndex >= 0)
                    {
                        _images[_prevImageIndex].Opacity = 0;
                    }

                    _prevImageIndex = _currentImageIndex;
                    _currentImageIndex++;
                }
            }
            catch (Exception ex)
            {

            }

            _working = false;

        }

    }
}
