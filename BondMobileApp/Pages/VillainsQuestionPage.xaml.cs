using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using BondMobileApp.EndpointDataClasses;
using BondMobileApp.Pages;
using BondTrivia.EndpointCalls;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace BondMobileApp.Pages
{
    public partial class VillainsQuestionPage : ContentPage
    {
        // Global variables

        public int questions = 0;
        public int ans_correct = 0;
        public int ans_wrong = 0;
        public string Question = "";
        public int typeQuestion;

        //public List<string> QuestionsAsked = new List<string>();
        public List<int> QuestionsAsked = new List<int>();

        // Potential Alternative Answers
        public List<int> OtherOptions = new List<int>();

        // Randomized Display List of all Answers
        public List<int> Display = new List<int>();
        


        // List of Endpoint Data
        public List<VillainsClass> Options { get; set; }






        // 1. Call Endpoint
        // 3. Get Question
        //  3a.  Select Question Type
        //  3b.  Add Question to Questions List
        //  3c.  Select Other Options
        //  3d.  Randomize Question
        //  3e.  Render Question




        // Constructor
        public VillainsQuestionPage(string qtype)
        {
            Debug.WriteLine("\nEntering Question Page Code Behind " + qtype);
            InitializeComponent();
            CallEndpoint(qtype);

        }




        async void CallEndpoint(string qtype)
        {
            Debug.WriteLine("VQP: CallEndpoint");
            var endpointObject = new Endpoints();
            Options = await endpointObject.GetVillainData(qtype);


            // Verify Options has the data needed
            Debug.WriteLine("\nVerify Options content");
            for (int i = 0; i < Options.Count; i++)
            {
                Debug.WriteLine(Options[i].villain);
            }

            GetQuestion();

        }

        private void GetScores()
        {
            NumQuestions.Text = questions.ToString();
            NumCorrect.Text = ans_correct.ToString();
            NumWrong.Text = ans_wrong.ToString();
        }

        //private void SelectQuestion()
        //{
        //    Debug.WriteLine("VQP: SelectQuestion");
        //    if (QuestionsAsked.Count == Options.Count || QuestionsAsked.Count >= 10)
        //    {
        //        Debug.WriteLine("VQP: QuestionPage: That's All Folks!");
        //        // To start same set of questions again:
        //        //Application.Current.MainPage = new NavigationPage(new MovieQuestionPage());
        //        // To Return to Main Page:
        //        //Application.Current.MainPage = new MainPage();
        //        // To got to Results Page:
        //        //Application.Current.MainPage = new ResultsPage();
        //        Application.Current.MainPage = new NavigationPage(new ResultsPage(questions.ToString(), ans_correct.ToString(), ans_wrong.ToString()));
        //    }

        //    else
        //    {
        //        Random q = new Random();
        //        int typeQuestion = q.Next(3);
        //        switch (typeQuestion)
        //        {
        //            case 1:
        //                typeAnswer = "villain";
        //                QuestionPrefix = "What role does ";
        //                QuestionBody = "play in the movie ";
        //                break;
        //            case 2:
        //                typeAnswer = "villain_actor";
        //                QuestionPrefix = "Who plays ";
        //                QuestionBody = "in the movie ";
        //                break;
        //            default:
        //                typeAnswer = "movie_title";
        //                QuestionPrefix = "Which Film featured ";
        //                QuestionBody = "";
        //                break;
        //        }
        //        GetQuestion();
        //    }

        //}

        private void GetQuestion()
        {
            // Check if all Questions have been asked
            Debug.WriteLine("VQP: GetQuestion");
            if (QuestionsAsked.Count == Options.Count || QuestionsAsked.Count >= 10)
            {
                Debug.WriteLine("VQP: QuestionPage: That's All Folks!");
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
                for (int i = 0; i < QuestionsAsked.Count; i++)
                {
                    Debug.WriteLine(QuestionsAsked[i]);
                }

                // Generate Random number for next Question
                Random n = new Random();
                int nextQuestion = n.Next(Options.Count);
                Debug.WriteLine("VQP: Next Question Index: " + nextQuestion);

                // Check is random number has already been used
                if (QuestionsAsked.Contains(nextQuestion) == true)
                {
                    Debug.WriteLine("VQP: Question already asked!");
                    GetQuestion();
                }
                else
                {
                    Debug.WriteLine("VQP: Ask Question! " + nextQuestion);
                    // Get other options
                    GetOtherOptions(nextQuestion);                              // Returns DisplayOptions with list or random integers

                    questions = questions + 1;
                    GetScores();

                    //Generate Qquestions
                    //Question = "Which Film featured " + Options[nextQuestion].villain + "?";

                    Random q = new Random();
                    typeQuestion = q.Next(3);

                    switch (typeQuestion)
                    {
                        case 1:
                            Question = "Who played " + Options[nextQuestion].villain + " in the movie " + Options[nextQuestion].movie_title + "?";
                            Option0.Content = Options[Display[0]].villain_actor;
                            Option1.Content = Options[Display[1]].villain_actor;
                            Option2.Content = Options[Display[2]].villain_actor;
                            Option3.Content = Options[Display[3]].villain_actor;
                            break;
                        case 2:
                            Question = "What role did " + Options[nextQuestion].villain_actor + " play in the movie " + Options[nextQuestion].movie_title + "?";
                            Option0.Content = Options[Display[0]].villain;
                            Option1.Content = Options[Display[1]].villain;
                            Option2.Content = Options[Display[2]].villain;
                            Option3.Content = Options[Display[3]].villain;
                            break;
                        default:
                            Question = "Which Film featured " + Options[nextQuestion].villain + "?";
                            Option0.Content = Options[Display[0]].movie_title;
                            Option1.Content = Options[Display[1]].movie_title;
                            Option2.Content = Options[Display[2]].movie_title;
                            Option3.Content = Options[Display[3]].movie_title;
                            break;
                    }

                    DisplayQuestion.Text = Question;

                    QuestionsAsked.Add(nextQuestion);
                    Debug.WriteLine("VQP: After: " + QuestionsAsked.Count);
                    //setLabelData();
                }

            }
        }

            //  3b.  Select Other Options
            private void GetOtherOptions(int n)
            {
                Debug.WriteLine("VQP: Get Other Options");
                OtherOptions.Clear();
                OtherOptions.Add(n);

                while (OtherOptions.Count < 4)
                {
                    // generate a random number
                    Random m = new Random();
                    int Option = m.Next(Options.Count);
                    Debug.WriteLine("Option: " + Option);

                    // check if it is in the list or if it is equal to the question
                    if (OtherOptions.Contains(Option) == true ||
                        Options[Option].villain == Options[n].villain ||
                        Options[Option].villain_actor == Options[n].villain_actor ||
                        Options[Option].movie_title == Options[n].movie_title)
                    {
                        //Debug.WriteLine("Option already on List!");
                    }
                    // if unique, add it to the list
                    else
                    {
                        //Debug.WriteLine("Add Option!");
                        // Calls getHenchmanName with a random number in HenchmenList
                        OtherOptions.Add(Option);
                        //Debug.WriteLine("Option Count: " + Options.Count);
                        //setLabelData();
                    }
                }
            

                


                //// For Debug Purposes: Verify that all answers are unique
                //Debug.WriteLine("\nVerify Option are unique");
                //for (int i = 0; i < OtherOptions.Count; i++)
                //{
                //    Debug.WriteLine(OtherOptions[i]);
                //}



                // Randomize the Order for display - New Approach
                Display.Clear();
                while (Display.Count < 4)
                {
                    // Randomize answers for display
                    Debug.WriteLine("OtherOptions Count: " + OtherOptions.Count);

                    Random d = new Random();
                    int display = d.Next(OtherOptions.Count);
                    Display.Add(OtherOptions[display]);
                    Debug.WriteLine("Display Count: " + Display.Count);
                    OtherOptions.RemoveAt(display);
                }



                //// For Debug Purposes: Verify that all answers are unique
                //Debug.WriteLine("\nVerify Display Options");
                //for (int i = 0; i < Display.Count; i++)
                //{
                //    Debug.WriteLine(Display[i]);
                //    Debug.WriteLine(Options[Display[i]].movie_title);
                //}



                //// Render Questions to Front End
                //Option0.Content = Options[Display[0]].movie_title;
                //Option1.Content = Options[Display[1]].movie_title;
                //Option2.Content = Options[Display[2]].movie_title;
                //Option3.Content = Options[Display[3]].movie_title;


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
                if (typeQuestion == 1 && contentString == Options[QuestionsAsked[QuestionsAsked.Count - 1]].villain_actor)
                {
                    Application.Current.MainPage.DisplayAlert("Correct", "You're Good!", "OK");
                    Debug.WriteLine("Correct Answer");
                    ans_correct = ans_correct + 1;
                    radioButton.IsChecked = false;
                    GetQuestion();


                }
                else if (typeQuestion == 2 && contentString == Options[QuestionsAsked[QuestionsAsked.Count - 1]].villain)
                {
                    Application.Current.MainPage.DisplayAlert("Correct", "You're Good!", "OK");
                    Debug.WriteLine("Correct Answer");
                    ans_correct = ans_correct + 1;
                    radioButton.IsChecked = false;
                    GetQuestion();

                }
                else if (typeQuestion == 0 && contentString == Options[QuestionsAsked[QuestionsAsked.Count - 1]].movie_title)
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
