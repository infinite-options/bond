using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace BondMobileApp.Pages
{
    public partial class MainSelectonPage : ContentPage
    {
        public MainSelectonPage()
        {
            InitializeComponent();
        }

        void Movie_Questions(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new MovieQuestionPage());
        }
    }
}
