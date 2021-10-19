using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;

namespace BondMobileApp.Pages
{
    public partial class MainSelectonPage : ContentPage
    {
        // Global variables

        public string qtype;    // stores the type of question being asked



        // Constructor

        public MainSelectonPage()
        {
            InitializeComponent();
        }

        //void Movie_Questions(System.Object sender, System.EventArgs e)
        //{
        //    Debug.WriteLine("Movie Button Pressed");
        //    Application.Current.MainPage = new NavigationPage(new MovieQuestionPage());
        //}

        void Movie_Questions(System.Object sender, System.EventArgs e)
        {
            qtype = "movies";
            Application.Current.MainPage = new NavigationPage(new MovieQuestionPage(qtype));
        }

        void BondGirl_Questions(System.Object sender, System.EventArgs e)
        {
            Debug.WriteLine("Bond Girl Button Pressed");
            qtype = "girls";
            Application.Current.MainPage = new NavigationPage(new BondGirlsQuestionPage(qtype));
        }

        void Villain_Questions(System.Object sender, System.EventArgs e)
        {
            Debug.WriteLine("Villain Button Pressed");
            qtype = "villains";
            Application.Current.MainPage = new NavigationPage(new VillainsQuestionPage(qtype));
        }

        void Henchmen_Questions(System.Object sender, System.EventArgs e)
        {
            Debug.WriteLine("Henchmen Button Pressed");
            qtype = "sidekicks";
            Application.Current.MainPage = new NavigationPage(new QuestionPage(qtype));
        }

        void Plot_Questions(System.Object sender, System.EventArgs e)
        {
            Debug.WriteLine("Plot Button Pressed");
            qtype = "plot";
            Application.Current.MainPage = new NavigationPage(new QuestionPage(qtype));
        }

        void Line_Questions(System.Object sender, System.EventArgs e)
        {
            Debug.WriteLine("Lines Button Pressed");
            qtype = "line";
            Application.Current.MainPage = new NavigationPage(new ResultsPage("3","2","1"));
        }

        void Song_Questions(System.Object sender, System.EventArgs e)
        {
            Debug.WriteLine("Songs Button Pressed");
            qtype = "songs";
            Application.Current.MainPage = new NavigationPage(new ResultsPage("3", "2", "1"));
        }

        void Credits(System.Object sender, System.EventArgs e)
        {
            Debug.WriteLine("Credits Button Pressed");
            qtype = "credits";
            Application.Current.MainPage = new NavigationPage(new CreditsPage());
        }
    }
}
