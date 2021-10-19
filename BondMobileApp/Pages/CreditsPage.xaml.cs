using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace BondMobileApp.Pages
{
    public partial class CreditsPage : ContentPage
    {
        public CreditsPage()
        {
            InitializeComponent();
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new MainSelectonPage();              //Notice no Navigation Page so this just sets the root page
        }
    }
}
