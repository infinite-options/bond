﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http;

using Xamarin.Forms;
using System.Diagnostics;
using System.Threading.Tasks;
using BondTrivia.EndpointCalls;         // Needed to be able to call Functions in Endpoints class 
using System.Collections.ObjectModel;   // Needed for Observable Collection
using System.ComponentModel;            // Needed for INotifyPropertyChanged
using System.Linq;
using BondMobileApp.EndpointDataClasses;
using BondMobileApp.ViewModels;

namespace BondMobileApp.Pages
{
    public partial class MovieQuestionPage : ContentPage
    {
        // Global variables

        public int questions = 0;
        public int ans_correct = 0;
        public int ans_wrong = 0;
        public string Question = "";
        public int typeQuestion;

        public List<MovieClass> Options { get; set; }                        // List of Endpoint Data
        public List<int> QuestionsAsked = new List<int>();                      // List of Questions Asked
        public List<int> OtherOptions = new List<int>();                        // Potential Alternative Answers
        public List<int> Display = new List<int>();                             // Randomized Display List of all Answers


        // Program Strategy
        // 1. Call Endpoint
        // 2. Get Question
        //  2a.  Add Question to Questions List
        //  2b.  Select Other Options
        //  2c.  Randomize Question
        //  2d.  Select Question Type
        //  2e.  Render Question
        // 3. Get Response Answer



        // Constructor
        public MovieQuestionPage(string qtype)
        {
            Debug.WriteLine("\nEntering Question Page Code Behind " + qtype);
            InitializeComponent();
            CallEndpoint(qtype);

        }




        async void CallEndpoint(string qtype)
        {
            Debug.WriteLine("MQP: CallEndpoint");
            var endpointObject = new Endpoints();
            Options = await endpointObject.GetMovieData(qtype);


            // Verify Options has the data needed
            //Debug.WriteLine("\nVerify Options content");
            //for (int i = 0; i < Options.Count; i++)
            //{
            //    Debug.WriteLine(Options[i].villain);
            //}


            // 2. Get Question
            GetQuestion();

        }

        private void GetScores()
        {
            NumQuestions.Text = questions.ToString();
            NumCorrect.Text = ans_correct.ToString();
            NumWrong.Text = ans_wrong.ToString();
        }


        private void GetQuestion()
        {
            // Check if all Questions have been asked
            Debug.WriteLine("\nMQP: GetQuestion");
            if (QuestionsAsked.Count == Options.Count || QuestionsAsked.Count >= 10)
            {
                Debug.WriteLine("MQP: QuestionPage: That's All Folks!");
                // To start same set of questions again:
                //Application.Current.MainPage = new NavigationPage(new MovieQuestionPage());
                // To Return to Main Page:
                //Application.Current.MainPage = new MainPage();
                // To got to Results Page:
                //Application.Current.MainPage = new ResultsPage();
                Application.Current.MainPage = new NavigationPage(new ResultsPage(questions.ToString(), ans_correct.ToString(), ans_wrong.ToString()));
            }

            else
            {
                // Print which questions have been asked already
                // Debug.WriteLine("Questions asked so far:");
                //for (int i = 0; i < QuestionsAsked.Count; i++)
                //{
                //    Debug.WriteLine(QuestionsAsked[i]);
                //}

                //  2a.  Add Question to Questions List
                // Generate Random number for next Question
                Random n = new Random();
                int nextQuestion = n.Next(Options.Count);
                Debug.WriteLine("MQP: Next Question Index: " + nextQuestion);

                // Check is random number has already been used
                if (QuestionsAsked.Contains(nextQuestion) == true)
                {
                    Debug.WriteLine("MQP: Question already asked!");
                    GetQuestion();
                }
                else
                {
                    Debug.WriteLine("MQP: Ask Question! " + nextQuestion);
                    QuestionsAsked.Add(nextQuestion);

                    //  2b.  Select Other Options
                    GetOtherOptions(nextQuestion);                              // Returns DisplayOptions with list or random integers

                    questions = questions + 1;
                    GetScores();

                    //  2d.  Select Question Type
                    //  2e.  Render Question
                    Random q = new Random();
                    typeQuestion = q.Next(6);

                    switch (typeQuestion)
                    {
                        case 1:                                                 // Actor
                            Question = "Who played James Bond in the moview " + Options[nextQuestion].movie_title + "?";
                            Option0.Content = Options[Display[0]].bond_actor;
                            Option1.Content = Options[Display[1]].bond_actor;
                            Option2.Content = Options[Display[2]].bond_actor;
                            Option3.Content = Options[Display[3]].bond_actor;
                            break;
                        case 2:                                                 // Year
                            Question = "What year was " + Options[nextQuestion].movie_title + " released?";
                            Option0.Content = Options[Display[0]].movie_year;
                            Option1.Content = Options[Display[1]].movie_year;
                            Option2.Content = Options[Display[2]].movie_year;
                            Option3.Content = Options[Display[3]].movie_year;
                            break;
                        case 3:                                                 // Director
                            Question = "Who directed " + Options[nextQuestion].movie_title + "?";
                            Option0.Content = Options[Display[0]].director;
                            Option1.Content = Options[Display[1]].director;
                            Option2.Content = Options[Display[2]].director;
                            Option3.Content = Options[Display[3]].director;
                            break;
                        case 4:                                                 // M
                            Question = "Who played 'M' in " + Options[nextQuestion].movie_title + "?";
                            Option0.Content = Options[Display[0]].M;
                            Option1.Content = Options[Display[1]].M;
                            Option2.Content = Options[Display[2]].M;
                            Option3.Content = Options[Display[3]].M;
                            break;
                        case 5:                                                  // Q
                            Question = "Who played 'M' in " + Options[nextQuestion].movie_title + "?";
                            Option0.Content = Options[Display[0]].Q;
                            Option1.Content = Options[Display[1]].Q;
                            Option2.Content = Options[Display[2]].Q;
                            Option3.Content = Options[Display[3]].Q;
                            break;
                        default:                                                 // MoneyPenny
                            Question = "Who played 'M' in "  + Options[nextQuestion].movie_title + "?";
                            Option0.Content = Options[Display[0]].Moneypenny;
                            Option1.Content = Options[Display[1]].Moneypenny;
                            Option2.Content = Options[Display[2]].Moneypenny;
                            Option3.Content = Options[Display[3]].Moneypenny;
                            break;
                    }

                    DisplayQuestion.Text = Question;

                    //QuestionsAsked.Add(nextQuestion);                           // Not sure why this is so far down in the logic.  Moving Up.  Delete if tests pass
                    Debug.WriteLine("MQP: After: " + QuestionsAsked.Count);
                    //setLabelData();
                }

            }
        }

        //  2b.  Select Other Options
        private void GetOtherOptions(int n)
        {
            Debug.WriteLine("MQP: Get Other Options");
            OtherOptions.Clear();
            OtherOptions.Add(n);

            while (OtherOptions.Count < 4)
            {
                // generate a random number
                Random m = new Random();
                int Option = m.Next(Options.Count);
                Debug.WriteLine("Option: " + Option);

                //// check if it is in the list or if it is equal to the question
                //if (OtherOptions.Contains(Option) == true ||                            //Check if Options is already on the Options List
                //    Options[Option].bond_girl == Options[n].bond_girl ||                //Check if Option villain = Question villain
                //    Options[Option].bond_girl_actress == Options[n].bond_girl_actress ||    //Check if Option actor = Question actor
                //    Options[Option].movie_title == Options[n].movie_title ||            //Check if Option movie = Question movie
                //    CheckOtherOptions(Option) == true)                                  //Check if Option values match any previous Option values
                //{
                //    //Debug.WriteLine("Option already on List!");
                //}


                switch (typeQuestion)
                {
                    case 1:                                                 // Actor
                        if (OtherOptions.Contains(Option) == true || Options[Option].bond_actor == Options[n].bond_actor) ;
                        else OtherOptions.Add(Option);
                        break;
                    case 2:                                                 // Year
                        if (OtherOptions.Contains(Option) == true || Options[Option].bond_actor == Options[n].movie_year) ;
                        else OtherOptions.Add(Option);
                        break;
                    case 3:                                                 // Director
                        if (OtherOptions.Contains(Option) == true || Options[Option].bond_actor == Options[n].director) ;
                        else OtherOptions.Add(Option);
                        break;
                    case 4:                                                 // M
                        if (OtherOptions.Contains(Option) == true || Options[Option].bond_actor == Options[n].M) ;
                        else OtherOptions.Add(Option);
                        break;
                    case 5:                                                  // Q
                        if (OtherOptions.Contains(Option) == true || Options[Option].bond_actor == Options[n].Q) ;
                        else OtherOptions.Add(Option);
                        break;
                    default:                                                 // MoneyPenny
                        if (OtherOptions.Contains(Option) == true || Options[Option].bond_actor == Options[n].Moneypenny) ;
                        else OtherOptions.Add(Option);
                            break;
                }






                // if unique, add it to the list
                //else
                //{
                //    //Debug.WriteLine("Add Option!");
                //    // Calls getHenchmanName with a random number in HenchmenList
                //    OtherOptions.Add(Option);
                //    //Debug.WriteLine("Option Count: " + Options.Count);
                //    //setLabelData();
                //}
            }





            //// For Debug Purposes: Verify that all answers are unique
            Debug.WriteLine("\nVerify Option are unique");
            for (int i = 0; i < OtherOptions.Count; i++)
            {
                Debug.WriteLine(OtherOptions[i]);
            }



            // 2c. Randomize the Order for display - New Approach
            Display.Clear();
            while (Display.Count < 4)
            {
                // Randomize answers for display
                //Debug.WriteLine("OtherOptions Count: " + OtherOptions.Count);

                Random d = new Random();
                int display = d.Next(OtherOptions.Count);
                Display.Add(OtherOptions[display]);
                //Debug.WriteLine("Display Count: " + Display.Count);
                OtherOptions.RemoveAt(display);
            }



            //// For Debug Purposes: Verify that all answers are unique
            //Debug.WriteLine("\nVerify Display Options");
            //for (int i = 0; i < Display.Count; i++)
            //{
            //    Debug.WriteLine(Display[i]);
            //    Debug.WriteLine(Options[Display[i]].movie_title);
            //}

        }


        //private bool CheckOtherOptions(int selection)
        //{
        //    //Debug.WriteLine("In Check Other Options " + selection + " OtherOptions Count: " + OtherOptions.Count);
        //    for (int i = 0; i < OtherOptions.Count; i++)
        //    {
        //        //Debug.WriteLine(i + ": Evaluate " + OtherOptions[i] + " Against " + selection);
        //        //Debug.WriteLine(Options[selection].villain + " is equal to " + Options[OtherOptions[i]].villain);

        //        //if (Options[selection].bond_girl == Options[OtherOptions[i]].bond_girl || Options[selection].bond_girl_actress == Options[OtherOptions[i]].bond_girl_actress || Options[selection].movie_title == Options[OtherOptions[i]].movie_title)
        //        //{
        //        //    return true;
        //        //}


        //        switch (typeQuestion)
        //        {
        //            case 1:                                                 // Actor
        //                if (Options[selection].bond_actor == Options[OtherOptions[i]].bond_actor) return true;
        //                else return false;
        //            case 2:                                                 // Year
        //                if (Options[selection].movie_year == Options[OtherOptions[i]].movie_year) return true;
        //                else return false;
        //            case 3:                                                 // Director
        //                if (Options[selection].director == Options[OtherOptions[i]].director) return true;
        //                else return false;
        //            case 4:                                                 // M
        //                if (Options[selection].M == Options[OtherOptions[i]].M) return true;
        //                else return false;
        //            case 5:                                                  // Q
        //                if (Options[selection].Q == Options[OtherOptions[i]].Q) return true;
        //                else return false;
        //            default:                                                 // MoneyPenny
        //                if (Options[selection].Moneypenny == Options[OtherOptions[i]].Moneypenny) return true;
        //                else return false;
        //        }

        //    }
        //    //return false;
        //}


        private bool CheckAnswer(string selection)
        {
            //Debug.WriteLine("In Check Answer");
            for (int i = 0; i < Options.Count; i++)
            {
                //Debug.WriteLine(i + ": " + Options[i]);
                if (Options[i].movie_title == selection)
                {
                    if (Options[i].director == Options[QuestionsAsked[QuestionsAsked.Count - 1]].director)
                    {
                        return true;
                    }
                }
            }
            return false;
        }



        public void RadioButton_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)  // sender is of class System.Object and must be converted in the proper type
        {
            Debug.WriteLine("\nRadio Button Clicked");
            Debug.WriteLine("sender: " + sender);
            Debug.WriteLine("e: " + e);

            var s = sender;
            var radioEvent = e;

            // converts sender to Radio Button type
            var radioButton = (RadioButton)sender;
            // Once in Radio Button type now you can see and use content
            string contentString = radioButton.Content.ToString();
            Debug.WriteLine("contentString: " + contentString);

            if (radioButton.IsChecked == true)
            {
                Debug.WriteLine("typeQuestion: " + typeQuestion);
                //Actor
                if (typeQuestion == 1 && contentString == Options[QuestionsAsked[QuestionsAsked.Count - 1]].bond_actor)
                {
                    Application.Current.MainPage.DisplayAlert("Correct", "You're Good!", "OK");
                    Debug.WriteLine("Correct Answer");
                    ans_correct = ans_correct + 1;
                    radioButton.IsChecked = false;
                    GetQuestion();
                }

                //Year
                else if (typeQuestion == 2 && contentString == Options[QuestionsAsked[QuestionsAsked.Count - 1]].movie_year)
                {
                    Application.Current.MainPage.DisplayAlert("Correct", "You're Good!", "OK");
                    Debug.WriteLine("Correct Answer");
                    ans_correct = ans_correct + 1;
                    radioButton.IsChecked = false;
                    GetQuestion();
                }

                //Director
                else if (typeQuestion == 0 && (contentString == Options[QuestionsAsked[QuestionsAsked.Count - 1]].director || CheckAnswer(contentString) == true))
                {
                    Application.Current.MainPage.DisplayAlert("Correct", "You're Good!", "OK");
                    Debug.WriteLine("Correct Answer");
                    ans_correct = ans_correct + 1;
                    radioButton.IsChecked = false;
                    GetQuestion();
                }

                //M
                if (typeQuestion == 1 && contentString == Options[QuestionsAsked[QuestionsAsked.Count - 1]].M)
                {
                    Application.Current.MainPage.DisplayAlert("Correct", "You're Good!", "OK");
                    Debug.WriteLine("Correct Answer");
                    ans_correct = ans_correct + 1;
                    radioButton.IsChecked = false;
                    GetQuestion();
                }

                //Q
                else if (typeQuestion == 2 && contentString == Options[QuestionsAsked[QuestionsAsked.Count - 1]].Q)
                {
                    Application.Current.MainPage.DisplayAlert("Correct", "You're Good!", "OK");
                    Debug.WriteLine("Correct Answer");
                    ans_correct = ans_correct + 1;
                    radioButton.IsChecked = false;
                    GetQuestion();
                }

                //MoneyPenny
                else if (typeQuestion == 0 && contentString == Options[QuestionsAsked[QuestionsAsked.Count - 1]].Moneypenny)
                {
                    Application.Current.MainPage.DisplayAlert("Correct", "You're Good!", "OK");
                    Debug.WriteLine("Correct Answer");
                    ans_correct = ans_correct + 1;
                    radioButton.IsChecked = false;
                    GetQuestion();
                }

                else
                {
                    Application.Current.MainPage.DisplayAlert("Whoops", "Wrong Agian Mr. Bond!", "OK");
                    Debug.WriteLine("Wrong Answer");
                    ans_wrong = ans_wrong + 1;
                }

                //NumQuestions.Text = questions.ToString();
                //NumCorrect.Text = ans_correct.ToString();
                //NumWrong.Text = ans_wrong.ToString();
            }
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new ResultsPage(questions.ToString(), ans_correct.ToString(), ans_wrong.ToString()));

        }
    }
}
