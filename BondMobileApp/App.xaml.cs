using System;
using BondMobileApp.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BondMobileApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new MainPage();
            MainPage = new MainSelectonPage();
            //MainPage = new QuestionPage("villain");
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
