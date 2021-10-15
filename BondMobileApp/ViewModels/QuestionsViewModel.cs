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

namespace BondMobileApp.ViewModels                                              //This is the ViewModel
{
    public class QuestionsViewModel: INotifyPropertyChanged                     //Enables Databinding.  Need to implement the Interface
    {
        public event PropertyChangedEventHandler PropertyChanged;               //Interface autocreated


        //What are the variable that you want to Databind?  ANS: The returned query list and the Results Info
        //List of variables initialized to 0 or null
        int _questions = 0;
        int _ans_correct = 0;
        int _ans_wrong = 0;
        public string Question = "";
        
        List<HenchmenClass> _options = null;

        //public List<string> QuestionsAsked = new List<string>();
        public List<int> QuestionsAsked = new List<int>();

        // Potential Alternative Answers
        public List<int> OtherOptions = new List<int>();

        // Randomized Display List of all Answers
        public List<int> Display = new List<int>();


        //Functions that trigger the PropertyChangedEventHandler

        public int questions
        {
            get => _questions; // returns the data store in _questions
            set
            {
                if (_questions == value)
                {
                    return;
                }
                else
                {
                    _questions = value;
                }

                OnPropertyChanged(nameof(questions));
            }
        }

        public int ans_correct
        {
            get => _ans_correct; // returns the data store in _ans_correct
            set
            {
                if (_ans_correct == value)
                {
                    return;
                }
                else
                {
                    _ans_correct = value;
                }

                OnPropertyChanged(nameof(ans_correct));
            }
        }

        public int ans_wrong
        {
            get => _ans_wrong; // returns the data store in _ans_wrong
            set
            {
                if (_ans_wrong == value)
                {
                    return;
                }
                else
                {
                    _ans_wrong = value;
                }

                OnPropertyChanged(nameof(ans_wrong));
            }
        }

        public List<HenchmenClass> Options
        {
            get => _options;
            set
            {
                if (_options == value)
                {
                    return;
                }
                else
                {
                    _options = value;
                }

                OnPropertyChanged(nameof(Options));
            }
        }


        //public void RadioButton_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)  // sender is of class System.Object and must be converted in the proper type
        //{
        //    Debug.WriteLine("\nRadio Button Clicked");
        //    Debug.WriteLine("sender: " + sender);
        //    Debug.WriteLine("e: " + e);

        //    var s = sender;
        //    var radioEvent = e;

        //    // converts sender to Radio Button type
        //    var radioButton = (RadioButton)sender;
        //    // Once in Radio Button type now you can see and use content
        //    string contentString = radioButton.Content.ToString();
        //    Debug.WriteLine("contentString: " + contentString);

        //    if (radioButton.IsChecked == true)
        //    {

        //        if (contentString == Options[QuestionsAsked[QuestionsAsked.Count - 1]].movie_title)
        //        {
        //            Application.Current.MainPage.DisplayAlert("Correct", "You're Good!", "OK");
        //            Debug.WriteLine("Correct Answer");
        //            ans_correct = ans_correct + 1;
        //            radioButton.IsChecked = false;
        //            SelectQuestion();

        //        }
        //        else
        //        {
        //            Application.Current.MainPage.DisplayAlert("Whoops", "Wrong Agian Mr. Bond!", "OK");
        //            Debug.WriteLine("Wrong Answer");
        //            ans_wrong = ans_wrong + 1;
        //        }
        //    }
        //}



        // Private void method that take a string corresponding to the class attribute that needs to be updated.
        // Not sure if it needs to be above or below Constructor
        void OnPropertyChanged(string variableName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(variableName));
        }



        //Program Plan:
        //1.Call the endpoint and get data
        //2.Select the Type of Question
        //3.Select the Question/Answer combination
        //4.Select 3 Other Options
        //5.Randomize the DisplayOrder


        //Constructor
        public QuestionsViewModel()                                             //Need a default constructor without arguements
        {

        }

        public QuestionsViewModel(string qtype)
        {
            Debug.WriteLine("\nQVM: Entering QuestionsViewModel.cs " + qtype);
            CallEndpoint(qtype);
        }


        async void CallEndpoint(string qtype)
        {
            Debug.WriteLine("QVM: CallEndpoint");
            var endpointObject = new Endpoints();
            Options = await endpointObject.GetData(qtype);
            //How do you get the data from _options to Options?
            // Loop through the _options and add each item to the Options binding collection

            // Verify Options has the data needed
            Debug.WriteLine("\nVerify Options content");
            for (int i = 0; i < Options.Count; i++)
            {
                Debug.WriteLine(Options[i].sidekick);
            }

            SelectQuestion();

        }

        private void SelectQuestion()
        {
            Debug.WriteLine("QVM: SelectQuestion");
            if (QuestionsAsked.Count == Options.Count)
            {
                Debug.WriteLine("QVM: QuestionPage: That's All Folks!");
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
                GetQuestion();
            }

        }

        private void GetQuestion()
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
            Debug.WriteLine("QVM: Next Question Index: " + nextQuestion);

            // Check is random number has already been used
            if (QuestionsAsked.Contains(nextQuestion) == true)
            {
                Debug.WriteLine("QVM: Question already asked!");
                GetQuestion();
            }
            else
            {
                Debug.WriteLine("QVM: Ask Question! " + nextQuestion);
                // Get other options
                GetOtherOptions(nextQuestion);

                questions = questions + 1;
                Question = "Which Film featured " + Options[nextQuestion].sidekick + "?";

                QuestionsAsked.Add(nextQuestion);
                Debug.WriteLine("QVM: After: " + QuestionsAsked.Count);
                //setLabelData();
                }
            
        }

        private void GetOtherOptions(int n)
        {
            Debug.WriteLine("QVM: Get Other Options");
            OtherOptions.Add(n);

            while (OtherOptions.Count < 4)
            {
                // generate a random number
                Random m = new Random();
                int Option = m.Next(Options.Count);
                Debug.WriteLine("Option: " + Option);

                // check if it is in the list or if it is equal to the question
                if (OtherOptions.Contains(Option) == true)
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

            // Verify that all answers are unique
            Debug.WriteLine("\nVerify Option are unique");
            for (int i = 0; i < OtherOptions.Count; i++)
            {
                Debug.WriteLine(OtherOptions[i]);
            }

            // Randomize the Order for display
            while (Display.Count < 4)
            {
                // Randomize answers for display
                Random d = new Random();
                int display = d.Next(OtherOptions.Count);
                //Debug.WriteLine("Display Order: " + display);

                // check if it is in the list or if it is equal to the question
                if (Display.Contains(display) == true)
                {
                    //Debug.WriteLine("Option already on List!");
                }
                // if unique, add it to the list
                else
                {
                    //Debug.WriteLine("Add Option!");
                    // Calls getHenchmanName with a random number in HenchmenList
                    Display.Add(display);
                    //Debug.WriteLine("Display Count: " + Display.Count);
                    //setLabelData();
                }
            }

            // Verify that all answers are unique
            Debug.WriteLine("\nVerify Display Options");
            for (int i = 0; i < Display.Count; i++)
            {
                Debug.WriteLine(Display[i]);
                Debug.WriteLine(Options[OtherOptions[Display[i]]].movie_title);
            }




        }




    }
}
