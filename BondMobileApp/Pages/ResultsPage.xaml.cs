using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace BondMobileApp.Pages
{
    public partial class ResultsPage : ContentPage
    {


        public ResultsPage(string questions, string correct, string wrong)
        {
            InitializeComponent();
            //NavigationPage.SetBackButtonTitle(this, "BondGirls");             //Trying to set back button title but this isn't working

            NumQuestions.Text = questions;
            NumCorrect.Text = correct;
            NumWrong.Text = wrong;

        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new MainSelectonPage();              //Notice no Navigation Page so this just sets the root page
            //Navigation.PopAsync(false);                                         //Pops page to the previous page.  False ensures no motion
        }
    }
}
