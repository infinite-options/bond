using System;
using System.Collections.Generic;
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
            var endpointobject = new Endpoints();
            var endpointData = await endpointobject.GetHenchmen();
            TestOption0 = endpointData[0].movie_title;
            TestOption1 = endpointData[0].sidekick;
            TestOption2 = endpointData[0].sidekick_actor;
            TestOption3 = endpointData[0].villain;
            return endpointData;
        }
    }




}
