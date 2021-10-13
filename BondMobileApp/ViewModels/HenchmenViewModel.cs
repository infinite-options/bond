using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using BondMobileApp.EndpointDataClasses;
using BondTrivia.EndpointCalls;

namespace BondMobileApp.ViewModels
{
    // The first thing that we need to do in the view model class is to impletement the
    // INotifyPropertyChanged interface. The INotifyPropertyChange interface is going to
    // notify the xaml page about our changes and the data will be displayed and updated.

    public class HenchmenViewModel: INotifyPropertyChanged
    {
        // This is the attibute defined in the INotifyPropertyChanged interface.
        // This attribute will call the main delegate and send in a signal notifying the
        // front end of a change in the data that needs to be displayed.

        public event PropertyChangedEventHandler PropertyChanged;

        // The following attibutes are private fields of the HechmenViewModel class which can be accessed
        // by the constructor and any other methods within this class definition. However, they are not visiables or
        // access directly by objects of this class. In addition, these attributes have been initialized to empty strings.
        // In programming, a good practice is to initialize every attribute to the default value of its type such as a string is usually
        // initialize to "" or an interger to 0.

        string movieTitle = string.Empty;
        string sidekick = string.Empty;
        string sidekickActor = string.Empty;
        string villain = string.Empty;

        // The following attributes are public because they can be access by objects of this class.
        // At the same time these attributes are accessable throught the xaml file and code behind class. 

        public string DisplayMovieTitle
        {
            get => movieTitle; // returns the data store in movieTitle
            set
            {
                if (movieTitle == value) 
                {
                    return;
                }
                else
                {
                    movieTitle = value;
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
            get => sidekickActor; // returns the data store in sidekickActor
            set
            {
                if (sidekickActor == value)
                {
                    return;
                }
                else
                {
                    sidekickActor = value;
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

        public List<HenchmenClass> localHechmenList = null;

        // Private void method that take a string correspoding to the class attribute that needs to be updated.

        void OnPropertyChanged(string variableName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(variableName));
        }


        // Constructor

        public HenchmenViewModel()
        {
            GetHenchmentList();
        }

        // Private method of the HenchmenViewModel class to retrive data from the endpoint using and object fromt the endpoints class.
        // This method also set the first set of data into the display attributes.

        async void GetHenchmentList()
        {
            var endpointObject = new Endpoints();
            localHechmenList = await endpointObject.GetHenchmen();

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
