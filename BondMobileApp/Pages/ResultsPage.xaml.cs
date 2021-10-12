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

            questionsX.Text = name;
            correctX.Text = correct;
            wrongX.Text = wrong;

        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new MainSelectonPage();
        }
    }
}
