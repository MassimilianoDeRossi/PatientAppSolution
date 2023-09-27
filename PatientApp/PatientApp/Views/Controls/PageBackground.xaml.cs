using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PatientApp.Views.Controls
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class PageBackground : AbsoluteLayout
  {
    public string ImageName
    {
      get { return (string)GetValue(ImageNameProperty); }
      set { SetValue(ImageNameProperty, value); }
    }

    public static readonly BindableProperty ImageNameProperty =
        BindableProperty.Create("ImageName", typeof(string), typeof(PageBackground), "",
          BindingMode.TwoWay, null, (obj, oldValue, newValue) => { (obj as PageBackground).UpdateImage((string)oldValue, (string)newValue); }
          );



    public double ImageOpacity
    {
      get { return (double)GetValue(ImageOpacityProperty); }
      set { SetValue(ImageOpacityProperty, value); }
    }

    public static readonly BindableProperty ImageOpacityProperty =
        BindableProperty.Create("ImageOpacity", typeof(double), typeof(PageBackground), 1.0d,
          BindingMode.TwoWay, null, (obj, oldValue, newValue) => { (obj as PageBackground).SetOpacity((double)newValue); }
          );


    public PageBackground()
    {
      InitializeComponent();
    }

    protected void UpdateImage(string oldImageName, string newImageName)
    {
      if (oldImageName != null && newImageName != null && oldImageName == newImageName)
        return;

      ImgBackground.Source = null;
      GC.Collect();
      ImgBackground.Source = ImageSourceCache.GetImageByFile(newImageName);
    }

    protected void SetOpacity(double val)
    {
      ImgBackground.Opacity = val;
    }
  }
}
