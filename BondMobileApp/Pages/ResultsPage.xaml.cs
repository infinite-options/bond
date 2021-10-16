using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace BondMobileApp.Pages
{
    public partial class ResultsPage : ContentPage
    {

        string _name;
        string _correct;
        string _wrong;

        public ResultsPage(string name, string correct, string wrong)
        {
            InitializeComponent();
            //NavigationPage.SetBackButtonTitle(this, "BondGirls");             //Trying to set back button title but this isn't working

            questionsX.Text = name;
            correctX.Text = correct;
            wrongX.Text = wrong;

        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new MainSelectonPage();              //Notice no Navigation Page so this just sets the root page
            //Navigation.PopAsync(false);                                         //Pops page to the previous page.  False ensures no motion
        }
    }
}
