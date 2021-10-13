using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http;

using Xamarin.Forms;
using System.Diagnostics;
using System.Threading.Tasks;
using BondTrivia.EndpointCalls;  // Needed to be able to call Functions in Endpoints class 
using System.Collections.ObjectModel;  // Needed for Observable Collection
using System.ComponentModel;  // Needed for INotifyPropertyChanged
using System.Linq;
using BondMobileApp.EndpointDataClasses;
using BondMobileApp.ViewModels;

namespace BondMobileApp.Pages
{
    public partial class QuestionPage : ContentPage
    {
        // Global variables

        
        public int index = 0;
        public int questions = 0;
        public int ans_correct = 0;
        public int ans_wrong = 0;

        // Endpoint Step 2: Initialize a list of a specific public class defined below
        public List<HenchmenClass> HenchmenList = new List<HenchmenClass>();

        //public List<string> QuestionsAsked = new List<string>();
        public List<int> QuestionsAsked = new List<int>();

        // Potential Alternative Answers
        public List<int> Options = new List<int>();

        // Randomized Display List of all Answers
        public List<int> Display = new List<int>();




        public ObservableCollection<HenchmenClass> HenchmenDetails = new ObservableCollection<HenchmenClass>();
        public ObservableCollection<displaytext> bindingSource = new ObservableCollection<displaytext>();



        public HenchmenViewModel HenchmenModel = new HenchmenViewModel();



        // Constructor
        public QuestionPage(string qtype)
        {
            InitializeComponent();
            Debug.WriteLine("\nStep 5: In QuestionPage Step 1");
            BindingContext = HenchmenModel;
            Debug.WriteLine("Step 6: In QuestionPage Step 2");
            LocalHenchmen();
        }


        // Endpoint Step 1: Define a public class with the specific attributes that will be received from the endpoint
        // Get Data Mapping from : https://jsonutils.com/
        //public class Henchmen
        //{
        //    public string movie_uid { get; set; }
        //    public string movie_title { get; set; }
        //    public int movie_order { get; set; }
        //    public int movie_year { get; set; }
        //    public int? book_order { get; set; }
        //    public int? book_year { get; set; }
        //    public string bond_actor { get; set; }
        //    public string director { get; set; }
        //    public string car { get; set; }
        //    public string gun { get; set; }
        //    public string villain_uid { get; set; }
        //    public string villain_movie_uid { get; set; }
        //    public string villain { get; set; }
        //    public string villain_actor { get; set; }
        //    public string objective { get; set; }
        //    public string outcome { get; set; }
        //    public string fate { get; set; }
        //    public string sidekick_uid { get; set; }
        //    public string sidekick_movie_uid { get; set; }
        //    public string sidekick { get; set; }
        //    public string sidekick_actor { get; set; }
        //}


        public class displaytext : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged = delegate { };
            public string Answer { get; set; }
            public string updateAnswer
            {
                set
                {
                    Answer = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Answer"));
                }

            }
        }



        void setLabelData()
        {
            var displayTextObject = new displaytext();
            //displayTextObject.Answer = henchmenName.Text;
            //bindingSource.Add(displayTextObject);
            //bindingCollection.ItemsSource = bindingSource;
        }





        // LocalHenchmen                                              Calls the Endpoint and gets data in a list
        //   getQuestion                                              Checks if 10 questions asked OR if all questions have been asked (if < 10), gets an index of the question to ask  
        //     getHenchmanName ==> Has Question format                Generates Questions and Stores answer, gets 3 other potential answers
        //     setLabelData (something to do with databinding)











        






        // Endpoint Step 3: Perform the Task of calling the Endpoint
        // Gets the data and stores it in a global variable
        async void LocalHenchmen()
        {
            Debug.WriteLine("Step 7: Inside QuestionPage LocalHenchmen");
            //var endpointobject = new Endpoints();
            ////var localresult = await endpointobject.GetHenchmen();  //GetHenchmen returns the result but is not storing it
            //HenchmenList = await endpointobject.GetHenchmen();  //GetHenchmen returns the result and stores it


            // Calling Endpoint
            HenchmenList = await HenchmenModel.getHenchmentList();
            Debug.WriteLine("Step 13: Finished QuestionPage LocalHenchmen");

            

            // Puts in a single Henchmen into the binding collection
            //HenchmenDetails.Add(HenchmenList[0]);
            //bindingCollection.ItemsSource = HenchmenDetails;

            // Loop through the HenchmenList and add each item to the bind collection
            foreach (HenchmenClass item in HenchmenList)
            {
                HenchmenDetails.Add(item);
            }

            //bindingCollection.ItemsSource = HenchmenDetails;











            // Call getQuestion to get the Question, Correct Answer and Choices from the data in HenchmenList
            // Call getQuestion here to ensure it runs after data is in the HenchmenList
            getQuestion();
        }



        // Need a Function to get the Question, Correct Answer and Choices from the data in HenchmenList
        // Need a Function that gets a random number of an item not previously asked + 3 other answers and displays them

        void getQuestion()
        {
            Debug.WriteLine("\n Step 14: Inside getQuestion");
            //Debug.WriteLine(QuestionsAsked.Count);
            //Debug.WriteLine(HenchmenList.Count);

            if (QuestionsAsked.Count == HenchmenList.Count)
            {
                Debug.WriteLine("That's All Folks!");
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
                int nextQuestion = n.Next(HenchmenList.Count);
                Debug.WriteLine("Step 15: Next Question Index: " + nextQuestion);

                // Check is random number has already been used
                if (QuestionsAsked.Contains(nextQuestion) == true)
                {
                    Debug.WriteLine("Question already asked!");
                    getQuestion();
                }
                else
                {
                    Debug.WriteLine("Step 16: Ask Question!");
                    // Calls getHenchmanName with a random number in HenchmenList
                    getHenchmanName(nextQuestion);
                    QuestionsAsked.Add(nextQuestion);
                    Debug.WriteLine("Step 26: After: " + QuestionsAsked.Count);
                    setLabelData();
                }
            }
        }



        void getHenchmanName(int n)
        {
            Debug.WriteLine("Step 17: Inside getHenchmenName " + n);
            Debug.WriteLine("Step 18: Total Number of Henchmen " + HenchmenList.Count);
            // Check if data was returned
            if (HenchmenList.Count != 0 && n < HenchmenList.Count)
            {
                questions = questions + 1;
                dynamicHenchmenName.Text = "Which Film featured " + HenchmenList[n].sidekick + "?";
                // Put data in appropriate variables
                //henchmenName.Text = HenchmenList[n].sidekick;
                //henchmenMovie.Text = HenchmenList[n].movie_title;
                //Answer.Content = HenchmenList[n].movie_title;
                questionsX.Text = questions.ToString();
                ans_correctX.Text = ans_correct.ToString();
                ans_wrongX.Text = ans_wrong.ToString();

                //Debug.WriteLine("From Get_Henchmen: {0}, {1}", henchmenName.Text, henchmenMovie.Text);
                //Debug.WriteLine("From Get_Henchmen: " + henchmenName.Text + ", " + henchmenMovie.Text);
                Debug.WriteLine("Step 19: From Get_Henchmen: " + HenchmenList[n].sidekick + ", " + HenchmenList[n].movie_title);




                // define Options as a List -- done as a global variable although I'm not sure it has to be global
                // Put answer in list of questions and stop after 4 total options are avialable
                List<int> Options = new List<int>() { n };

                while (Options.Count < 4)
                {
                    // generate a random number
                    Random m = new Random();
                    int Option = m.Next(HenchmenList.Count);
                    Debug.WriteLine("Step 20: Option: " + Option);

                    // check if it is in the list or if it is equal to the question
                    if (Options.Contains(Option) == true)
                    {
                        //Debug.WriteLine("Option already on List!");
                    }
                    // if unique, add it to the list
                    else
                    {
                        //Debug.WriteLine("Add Option!");
                        // Calls getHenchmanName with a random number in HenchmenList
                        Options.Add(Option);
                        //Debug.WriteLine("Option Count: " + Options.Count);
                        setLabelData();
                    }
                }

                // Verify that all answers are unique
                Debug.WriteLine("\nStep 21: Verify Option are unique");
                for (int i = 0; i < Options.Count; i++)
                {
                    Debug.WriteLine(Options[i]);
                }


                //Option1.Content = HenchmenList[Options[1]].movie_title;
                //Option2.Content = HenchmenList[Options[2]].movie_title;
                //Option3.Content = HenchmenList[Options[3]].movie_title;
                //Answer2.Content = HenchmenList[Options[0]].movie_title;



                // Put answer in randomized list for display
                // Define new Display List
                List<int> Display = new List<int>();

                while (Display.Count < 4)
                {
                    // Randomize answers for display
                    Random d = new Random();
                    int display = d.Next(Options.Count);
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
                        setLabelData();
                    }
                }

                // Verify that all answers are unique
                Debug.WriteLine("\n Step 23: Verify Display Option are unique");
                for (int i = 0; i < Display.Count; i++)
                {
                    Debug.WriteLine(Display[i]);
                }



                Option0.Content = HenchmenList[Options[Display[0]]].movie_title;
                Option1.Content = HenchmenList[Options[Display[1]]].movie_title;
                Option2.Content = HenchmenList[Options[Display[2]]].movie_title;
                Option3.Content = HenchmenList[Options[Display[3]]].movie_title;




            }
            Debug.WriteLine("Step 25: Finished getHenchmenName");

        }










        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            //Application.Current.MainPage = new MainPage();
            Application.Current.MainPage = new NavigationPage(new ResultsPage(questions.ToString(), ans_correct.ToString(), ans_wrong.ToString()));
        }


        void RadioButton_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)  // sender is of class System.Object and must be converted in the proper type
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

                if (contentString == HenchmenList[QuestionsAsked[QuestionsAsked.Count - 1]].movie_title)
                {
                    DisplayAlert("Correct", "You're Good!", "OK");
                    Debug.WriteLine("Correct Answer");
                    ans_correct = ans_correct + 1;
                    radioButton.IsChecked = false;
                    getQuestion();

                }
                else
                {
                    DisplayAlert("Whoops", "Wrong Agian Mr. Bond!", "OK");
                    Debug.WriteLine("Wrong Answer");
                    ans_wrong = ans_wrong + 1;
                }
            }
        }




    }
}
