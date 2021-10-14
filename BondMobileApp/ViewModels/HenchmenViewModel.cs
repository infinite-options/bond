using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using BondMobileApp.EndpointDataClasses;
using BondTrivia.EndpointCalls;

namespace BondMobileApp.ViewModels                                          //This is the ViewModel
{
    // The first thing that we need to do in the view model class is to impletement the
    // INotifyPropertyChanged interface. The INotifyPropertyChange interface is going to
    // notify the xaml page about our changes and the data will be displayed and updated.

    public class HenchmenViewModel: INotifyPropertyChanged                  //Enable Databinding.  Implement an interface on our View Model that Xamarin will register for events
    {
        // This is the attibute defined in the INotifyPropertyChanged interface.
        // This attribute will call the main delegate and send in a signal notifying the
        // front end of a change in the data that needs to be displayed.

        public event PropertyChangedEventHandler PropertyChanged;           // Interface implemented

        // The following attibutes are private fields of the HechmenViewModel class which can be accessed
        // by the constructor and any other methods within this class definition. However, they are not viriables or
        // access directly by objects of this class. In addition, these attributes have been initialized to empty strings.
        // In programming, a good practice is to initialize every attribute to the default value of its type such as a string is usually
        // initialize to "" or an interger to 0.

        //string movieTitle = string.Empty;
        //string sidekick = string.Empty;
        //string sidekickActor = string.Empty;
        //string villain = string.Empty;



        //These are the properties that we want to do DataBinding to.  You need a public variable for each item you want to bind and you need to initialize it
        string movie_uid = string.Empty;
        string movie_title = string.Empty;
        int movie_order = 0;
        int movie_year = 0;
        int? book_order = 0;
        int? book_year = 0;
        string bond_actor  = string.Empty;
        string director  = string.Empty;
        string car  = string.Empty;
        string gun  = string.Empty;
        string villain_uid  = string.Empty;
        string villain_movie_uid  = string.Empty;
        string villain  = string.Empty;
        string villain_actor  = string.Empty;
        string objective  = string.Empty;
        string outcome  = string.Empty;
        string fate  = string.Empty;
        string sidekick_uid  = string.Empty;
        string sidekick_movie_uid  = string.Empty;
        string sidekick  = string.Empty;
        string sidekick_actor  = string.Empty;


        List<HenchmenClass> _options = null;
        List<HenchmenClass> localHechmenList = null;




        // For each variable that you want to enable databinding, you need a get/set definition that calls OnPropertyChanged

        // The following attributes are public because they can be access by objects of this class.
        // At the same time these attributes are accessable throught the xaml file and code behind class. 

        public string DisplayMovieTitle
        {
            get => movie_title; // returns the data store in movieTitle
            set
            {
                if (movie_title == value) 
                {
                    return;
                }
                else
                {
                    movie_title = value;
                }

                OnPropertyChanged(nameof(DisplayMovieTitle));
            }
        }

        public string DisplaySidekick
        {
            get => sidekick; // returns the data store in sidekick
            set
            {
                if (sidekick == value)
                {
                    return;
                }
                else
                {
                    sidekick = value;
                }

                OnPropertyChanged(nameof(DisplaySidekick));
            }
        }

        public string DisplaySidekickActor
        {
            get => sidekick_actor; // returns the data store in sidekickActor
            set
            {
                if (sidekick_actor == value)
                {
                    return;
                }
                else
                {
                    sidekick_actor = value;
                }

                OnPropertyChanged(nameof(DisplaySidekickActor));
            }
        }

        public string DisplayVillain
        {
            get => villain; // returns the data store in villain
            set
            {
                if (villain == value)
                {
                    return;
                }
                else
                {
                    villain = value;
                }

                OnPropertyChanged(nameof(DisplayVillain));
            }
        }




        
        public List<HenchmenClass> Options
        {
            get => _options;
            set
            {
                if(_options == value)
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






        // Constructor
        public HenchmenViewModel()
        {
            Debug.WriteLine("\nEntering HenchmenViewModel.cs");
            GetHenchmentList();                                     //Includes endpoint call
        }




        // Private method of the HenchmenViewModel class to retrive data from the endpoint using and object fromt the endpoints class.
        // This method also set the first set of data into the display attributes.

        async void GetHenchmentList()
        {
            var endpointObject = new Endpoints();                                                   //This is defined in Endpoints.cs
            Debug.WriteLine("HenchmenViewModel.cs: Before Endpoint Call");
            localHechmenList = await endpointObject.GetHenchmen();                                  //This calls GetHenchmen in Endpoints
            Debug.WriteLine("\nHenchmenViewModel.cs: After Endpoint Call" + localHechmenList);

            //Original code that would appear on the MovieQuestionPage
            DisplayMovieTitle = localHechmenList[0].movie_title;
            DisplaySidekick = localHechmenList[0].sidekick;
            DisplaySidekickActor = localHechmenList[0].sidekick_actor;
            DisplayVillain = localHechmenList[0].villain;

        }

        // Public method to iterate over henchmen list throught any object from this class. 

        public void IterateOverHenchmenList(int i)
        {
            if(i < localHechmenList.Count)
            {
                DisplayMovieTitle = localHechmenList[i].movie_title;
                DisplaySidekick = localHechmenList[i].sidekick;
                DisplaySidekickActor = localHechmenList[i].sidekick_actor;
                DisplayVillain = localHechmenList[i].villain;
            }
        }
    }
}
