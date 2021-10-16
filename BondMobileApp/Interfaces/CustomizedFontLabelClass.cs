using System;
using Xamarin.Forms;

namespace BondMobileApp.Interfaces
{
    public class CustomizedFontLabelClass:Label
    {
        public CustomizedFontLabelClass()
        {
            if (Device.RuntimePlatform != Device.iOS) { Style = Application.Current.Resources["FontLabelStyle"] as Style; }
        }
    }
}
