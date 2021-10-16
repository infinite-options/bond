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
    public partial class BondGirlsQuestionPage : ContentPage
    {
        // Global variables

        public int questions = 0;
        public int ans_correct = 0;
        public int ans_wrong = 0;
        public string Question = "";
        public int typeQuestion;

        public List<BondGirlClass> Options { get; set; }                        // List of Endpoint Data
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
        public BondGirlsQuestionPage(string qtype)
        {
            Debug.WriteLine("\nEntering Question Page Code Behind " + qtype);
            InitializeComponent();
            CallEndpoint(qtype);

        }




        async void CallEndpoint(string qtype)
        {
            Debug.WriteLine("BGQP: CallEndpoint");
            var endpointObject = new Endpoints();
            Options = await endpointObject.GetBondGirlData(qtype);


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
            Debug.WriteLine("\nBGQP: GetQuestion");
            if (QuestionsAsked.Count == Options.Count || QuestionsAsked.Count >= 10)
            {
                Debug.WriteLine("BGQP: QuestionPage: That's All Folks!");
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
                Debug.WriteLine("BGQP: Next Question Index: " + nextQuestion);

                // Check is random number has already been used
                if (QuestionsAsked.Contains(nextQuestion) == true)
                {
                    Debug.WriteLine("BGQP: Question already asked!");
                    GetQuestion();
                }
                else
                {
                    Debug.WriteLine("BGQP: Ask Question! " + nextQuestion);
                    QuestionsAsked.Add(nextQuestion);

                    //  2b.  Select Other Options
                    GetOtherOptions(nextQuestion);                              // Returns DisplayOptions with list or random integers

                    questions = questions + 1;
                    GetScores();

                    //  2d.  Select Question Type
                    //  2e.  Render Question
                    Random q = new Random();
                    typeQuestion = q.Next(3);

                    switch (typeQuestion)
                    {
                        case 1:                                                 // Actor
                            Question = "Who played " + Options[nextQuestion].bond_girl_actress + " in the movie " + Options[nextQuestion].movie_title + "?";
                            Option0.Content = Options[Display[0]].bond_girl_actress;
                            Option1.Content = Options[Display[1]].bond_girl_actress;
                            Option2.Content = Options[Display[2]].bond_girl_actress;
                            Option3.Content = Options[Display[3]].bond_girl_actress;
                            break;
                        case 2:                                                 // Role
                            Question = "What role did " + Options[nextQuestion].bond_girl_actress + " play in the movie " + Options[nextQuestion].movie_title + "?";
                            Option0.Content = Options[Display[0]].bond_girl;
                            Option1.Content = Options[Display[1]].bond_girl;
                            Option2.Content = Options[Display[2]].bond_girl;
                            Option3.Content = Options[Display[3]].bond_girl;
                            break;
                        default:                                                 // Movie
                            Question = "Which Film featured " + Options[nextQuestion].bond_girl + "?";
                            Option0.Content = Options[Display[0]].movie_title;
                            Option1.Content = Options[Display[1]].movie_title;
                            Option2.Content = Options[Display[2]].movie_title;
                            Option3.Content = Options[Display[3]].movie_title;
                            break;
                    }

                    DisplayQuestion.Text = Question;

                    //QuestionsAsked.Add(nextQuestion);                           // Not sure why this is so far down in the logic.  Moving Up.  Delete if tests pass
                    Debug.WriteLine("BGQP: After: " + QuestionsAsked.Count);
                    //setLabelData();
                }

            }
        }

        //  2b.  Select Other Options
        private void GetOtherOptions(int n)
        {
            Debug.WriteLine("BGQP: Get Other Options");
            OtherOptions.Clear();
            OtherOptions.Add(n);

            while (OtherOptions.Count < 4)
            {
                // generate a random number
                Random m = new Random();
                int Option = m.Next(Options.Count);
                Debug.WriteLine("Option: " + Option);

                // check if it is in the list or if it is equal to the question
                if (OtherOptions.Contains(Option) == true ||                        //Check if Options is already on the Options List
                    Options[Option].bond_girl == Options[n].bond_girl ||                //Check if Option villain = Question villain
                    Options[Option].bond_girl_actress == Options[n].bond_girl_actress ||    //Check if Option actor = Question actor
                    Options[Option].movie_title == Options[n].movie_title ||        //Check if Option movie = Question movie
                    CheckOtherOptions(Option) == true)                              //Check if Option values match any previous Option values
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


        private bool CheckOtherOptions(int selection)
        {
            //Debug.WriteLine("In Check Other Options " + selection + " OtherOptions Count: " + OtherOptions.Count);
            for (int i = 0; i < OtherOptions.Count; i++)
            {
                //Debug.WriteLine(i + ": Evaluate " + OtherOptions[i] + " Against " + selection);
                //Debug.WriteLine(Options[selection].villain + " is equal to " + Options[OtherOptions[i]].villain);
                if (Options[selection].bond_girl == Options[OtherOptions[i]].bond_girl || Options[selection].bond_girl_actress == Options[OtherOptions[i]].bond_girl_actress || Options[selection].movie_title == Options[OtherOptions[i]].movie_title)
                {
                    return true;
                }
            }
            return false;
        }


        private bool CheckAnswer(string selection)
        {
            //Debug.WriteLine("In Check Answer");
            for (int i = 0; i < Options.Count; i++)
            {
                //Debug.WriteLine(i + ": " + Options[i]);
                if (Options[i].movie_title == selection)
                {
                    if (Options[i].bond_girl == Options[QuestionsAsked[QuestionsAsked.Count - 1]].bond_girl)
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
                if (typeQuestion == 1 && contentString == Options[QuestionsAsked[QuestionsAsked.Count - 1]].bond_girl_actress)
                {
                    Application.Current.MainPage.DisplayAlert("Correct", "You're Good!", "OK");
                    Debug.WriteLine("Correct Answer");
                    ans_correct = ans_correct + 1;
                    radioButton.IsChecked = false;
                    GetQuestion();
                }

                //Role
                else if (typeQuestion == 2 && contentString == Options[QuestionsAsked[QuestionsAsked.Count - 1]].bond_girl)
                {
                    Application.Current.MainPage.DisplayAlert("Correct", "You're Good!", "OK");
                    Debug.WriteLine("Correct Answer");
                    ans_correct = ans_correct + 1;
                    radioButton.IsChecked = false;
                    GetQuestion();
                }

                //Movie
                else if (typeQuestion == 0 && (contentString == Options[QuestionsAsked[QuestionsAsked.Count - 1]].movie_title || CheckAnswer(contentString) == true))
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
    //{
    //    // Global variables

    //    public int questions = 0;
    //    public int ans_correct = 0;
    //    public int ans_wrong = 0;
    //    public string Question = "";

    //    //public List<string> QuestionsAsked = new List<string>();
    //    public List<int> QuestionsAsked = new List<int>();

    //    // Potential Alternative Answers
    //    public List<int> OtherOptions = new List<int>();

    //    // Randomized Display List of all Answers
    //    public List<int> Display = new List<int>();

    //    // List of Endpoint Data
    //    public List<BondGirlClass> Options { get; set; }




    //    //Program Plan:
    //    //1.Call the endpoint and get data
    //    //2.Select the Type of Question
    //    //3.Select the Question/Answer combination
    //    //4.Select 3 Other Options
    //    //5.Randomize the DisplayOrder



    //    // Constructor
    //    public BondGirlsQuestionPage(string qtype)
    //    {
    //        Debug.WriteLine("\nEntering Bond Girl Question Page Code Behind " + qtype);
    //        InitializeComponent();
    //        CallEndpoint(qtype);

    //    }




    //    async void CallEndpoint(string qtype)
    //    {
    //        Debug.WriteLine("BGQP: CallEndpoint");
    //        var endpointObject = new Endpoints();
    //        Options = await endpointObject.GetBondGirlData(qtype);                      // Puts data directly into Options

    //        // Verify Options has the data needed
    //        Debug.WriteLine("\nVerify Options content");
    //        for (int i = 0; i < Options.Count; i++)
    //        {
    //            Debug.WriteLine(Options[i].bond_girl);
    //        }

    //        GetQuestion();

    //    }


    //    private void GetQuestion()
    //    {

    //        Debug.WriteLine("\nBGQP: GetQuestion");
    //        if (QuestionsAsked.Count == Options.Count || QuestionsAsked.Count >= 10)        // Limit to number of questions in db or 10
    //        {
    //            Debug.WriteLine("BGQP: QuestionPage: That's All Folks!");
    //            // To start same set of questions again:
    //            //Application.Current.MainPage = new NavigationPage(new MovieQuestionPage());
    //            // To Return to Main Page:
    //            //Application.Current.MainPage = new MainPage();
    //            // To got to Results Page:
    //            //Application.Current.MainPage = new ResultsPage();
    //            Application.Current.MainPage = new NavigationPage(new ResultsPage(questions.ToString(), ans_correct.ToString(), ans_wrong.ToString()));
    //        }

    //        else
    //        {

    //            // Print which questions have been asked already
    //            // Debug.WriteLine("Questions asked so far:");
    //            for (int i = 0; i < QuestionsAsked.Count; i++)
    //            {
    //                Debug.WriteLine(QuestionsAsked[i]);
    //            }

    //            // Generate Random number for next Question
    //            Random n = new Random();
    //            int nextQuestion = n.Next(Options.Count);
    //            Debug.WriteLine("BGQP: Next Question Index: " + nextQuestion);

    //            // Check is random number has already been used
    //            if (QuestionsAsked.Contains(nextQuestion) == true)
    //            {
    //                Debug.WriteLine("BGQP: Question already asked!");
    //                GetQuestion();
    //            }
    //            else
    //            {
    //                Debug.WriteLine("BGQP: Ask Question! " + nextQuestion);
    //                // Get other options
    //                GetOtherOptions(nextQuestion);

    //                questions = questions + 1;
    //                Question = "Which Film featured " + Options[nextQuestion].bond_girl + "?";
    //                DisplayQuestion.Text = Question;

    //                QuestionsAsked.Add(nextQuestion);
    //                Debug.WriteLine("BGQP: After: " + QuestionsAsked.Count);
    //                //setLabelData();
    //            }
    //        }
    //    }

    //    private void GetOtherOptions(int n)
    //    {
    //        Debug.WriteLine("BGQP: Get Other Options");
    //        OtherOptions.Clear();
    //        OtherOptions.Add(n);

    //        while (OtherOptions.Count < 4)
    //        {
    //            // generate a random number
    //            Random m = new Random();
    //            int Option = m.Next(Options.Count);
    //            Debug.WriteLine("Option: " + Option);

    //            // check if it is in the list or if it is equal to the question
    //            if (OtherOptions.Contains(Option) == true)
    //            {
    //                //Debug.WriteLine("Option already on List!");
    //            }
    //            // if unique, add it to the list
    //            else
    //            {
    //                //Debug.WriteLine("Add Option!");
    //                // Calls getHenchmanName with a random number in HenchmenList
    //                OtherOptions.Add(Option);
    //                //Debug.WriteLine("Option Count: " + Options.Count);
    //                //setLabelData();
    //            }
    //        }

    //        // Verify that all answers are unique
    //        Debug.WriteLine("\nVerify Option are unique");
    //        for (int i = 0; i < OtherOptions.Count; i++)
    //        {
    //            Debug.WriteLine(OtherOptions[i]);
    //        }

    //        // Randomize the Order for display
    //        Display.Clear();
    //        while (Display.Count < 4)
    //        {
    //            // Randomize answers for display
    //            Random d = new Random();
    //            int display = d.Next(OtherOptions.Count);
    //            //Debug.WriteLine("Display Order: " + display);

    //            // check if it is in the list or if it is equal to the question
    //            if (Display.Contains(display) == true)
    //            {
    //                //Debug.WriteLine("Option already on List!");
    //            }
    //            // if unique, add it to the list
    //            else
    //            {
    //                //Debug.WriteLine("Add Option!");
    //                // Calls getHenchmanName with a random number in HenchmenList
    //                Display.Add(display);
    //                //Debug.WriteLine("Display Count: " + Display.Count);
    //                //setLabelData();
    //            }
    //        }



    //        // Verify that all answers are unique
    //        Debug.WriteLine("\nVerify Display Options");
    //        for (int i = 0; i < Display.Count; i++)
    //        {
    //            Debug.WriteLine(Display[i]);
    //            Debug.WriteLine(Options[OtherOptions[Display[i]]].movie_title);
    //        }


    //        Option0.Content = Options[OtherOptions[Display[0]]].movie_title;
    //        Option1.Content = Options[OtherOptions[Display[1]]].movie_title;
    //        Option2.Content = Options[OtherOptions[Display[2]]].movie_title;
    //        Option3.Content = Options[OtherOptions[Display[3]]].movie_title;


    //    }


    //    public void RadioButton_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)  // sender is of class System.Object and must be converted in the proper type
    //    {
    //        Debug.WriteLine("\nRadio Button Clicked");
    //        Debug.WriteLine("sender: " + sender);
    //        Debug.WriteLine("e: " + e);

    //        var s = sender;
    //        var radioEvent = e;

    //        // converts sender to Radio Button type
    //        var radioButton = (RadioButton)sender;
    //        // Once in Radio Button type now you can see and use content
    //        string contentString = radioButton.Content.ToString();
    //        Debug.WriteLine("contentString: " + contentString);

    //        if (radioButton.IsChecked == true)
    //        {

    //            if (contentString == Options[QuestionsAsked[QuestionsAsked.Count - 1]].movie_title)
    //            {
    //                Application.Current.MainPage.DisplayAlert("Correct", "You're Good!", "OK");
    //                Debug.WriteLine("Correct Answer");
    //                ans_correct = ans_correct + 1;
    //                radioButton.IsChecked = false;
    //                GetQuestion();

    //            }
    //            else
    //            {
    //                Application.Current.MainPage.DisplayAlert("Whoops", "Wrong Agian Mr. Bond!", "OK");
    //                Debug.WriteLine("Wrong Answer");
    //                ans_wrong = ans_wrong + 1;
    //            }

    //            NumQuestions.Text = questions.ToString();
    //            NumCorrect.Text = ans_correct.ToString();
    //            NumWrong.Text = ans_wrong.ToString();
    //        }
    //    }

    //    void Button_Clicked(System.Object sender, System.EventArgs e)
    //    {
    //        Navigation.PushAsync(new ResultsPage(questions.ToString(), ans_correct.ToString(), ans_wrong.ToString()) , false);

    //    }
    //}
}
