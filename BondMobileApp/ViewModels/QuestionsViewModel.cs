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
        public QuestionsViewModel(string qtype)
        {
            Debug.WriteLine("\nQVM: Entering QuestionsViewModel.cs");
            CallEndpoint(qtype);
        }


        async void CallEndpoint(string qtype)
        {
            Debug.WriteLine("QVM: CallEndpoint");
            var endpointObject = new Endpoints();
            _options = await endpointObject.GetData(qtype);
            //How do you get the data from _options to Options?
            // Loop through the _options and add each item to the Options binding collection
            foreach (HenchmenClass item in _options)
            {
                Options.Add(item);
            }
            SelectQuestion();

        }

        private void SelectQuestion()
        {
            Debug.WriteLine("QVM: SelectQuestion");
            GetQuestion();
        }

        private void GetQuestion()
        {
            Debug.WriteLine("QVM: GetQuestion");
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
                    Debug.WriteLine("QVM: Ask Question!");
                    // Calls getHenchmanName with a random number in HenchmenList
                    GetOtherOptions(nextQuestion);
                    QuestionsAsked.Add(nextQuestion);
                    Debug.WriteLine("QVM: After: " + QuestionsAsked.Count);
                    //setLabelData();
                }
            }
        }

        private void GetOtherOptions(int n)
        {
            Debug.WriteLine("QVM: Get Other Options");
        }
    }
}
