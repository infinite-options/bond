using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using BondMobileApp.EndpointDataClasses;
using BondTrivia.EndpointCalls;

namespace BondMobileApp.ViewModels
{
    public class HenchmenViewModel
    {

        public string TestOption0 { set; get; }
        public string TestOption1 { set; get; }
        public string TestOption2 { set; get; }
        public string TestOption3 { set; get; }




        // Constructor

        public HenchmenViewModel()
        {
            getHenchmentList();
        }

        public async Task<List<HenchmenClass>> getHenchmentList()
        {
            Debug.WriteLine("\nStep 1: Enter HenchmenViewModel");
            // Defines new endpoint object
            var endpointobject = new Endpoints();
            Debug.WriteLine("Step 2: Endpointobject: " + endpointobject);

            // Calls endpoint GetHenchmen
            var endpointData = await endpointobject.GetHenchmen();
            Debug.WriteLine("EndpointData: " + endpointData);

            // Now all data from endpoint is in endpointData

            TestOption0 = endpointData[0].movie_title;
            TestOption1 = endpointData[0].sidekick;
            TestOption2 = endpointData[0].sidekick_actor;
            TestOption3 = endpointData[0].villain;
            Debug.WriteLine("Exit HenchmenViewModel");
            //Debug.WriteLine("Endpoint URL: " + endpoint);
            return endpointData;
        }
    }




}
