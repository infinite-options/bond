using System;
using System.Collections.Generic;   // Needed for List functionality
using System.Diagnostics;           // Need for Debug command.  Changes the color of output compared to Console.WriteLine
using System.Net.Http;              // Needed for HttpClient
using System.Threading.Tasks;       // Allows data to be returned in an async function
using Newtonsoft.Json;              // Needed for JsonConvert
using static BondMobileApp.Pages.MovieQuestionPage;   // Has the Class that uses the data returned from the endpoint


namespace BondTrivia.EndpointCalls
{
    public class Endpoints
    {

        string baseURL;
        string suffix;
        string endpoint;



        // Constructor
        public Endpoints()
        {

        }

        // Create a Generic Endpoint call


        // Endpoint Step 3: Perform the Task of calling the Endpoint
        public async Task<List<Henchmen>> GetHenchmen()
        {
            Debug.WriteLine("\nInside GetHenchmen Endpoint Call");
            baseURL = "https://iznfqs92n3.execute-api.us-west-1.amazonaws.com/dev/";
            suffix = "api/v2/sidekicks";
            endpoint = baseURL + suffix;
            Debug.WriteLine("Endpoint URL: " + endpoint);

            // Set the result of the upcoming call to null
            List<Henchmen> result = null;
            // HttpClient requires using System.Net.Http which defines what dot attributes it has
            var client = new HttpClient();
            // Endpoint call definition.  Requires Newtonsoft.Json;  endpointCall variable has dot attributes
            //var endpointCall = await client.GetAsync("https://iznfqs92n3.execute-api.us-west-1.amazonaws.com/dev/api/v2/sidekicks");
            var endpointCall = await client.GetAsync(endpoint);

            // Check if endpoint call was successful
            if (endpointCall.IsSuccessStatusCode)
            {
                // This is the actual endpoint call and data is stored in variable
                var endpointContentString = await endpointCall.Content.ReadAsStringAsync();
                // Print statement to see what data was received.  Especially valuable with breakpoints
                Debug.WriteLine("From Endpoint Call: " + endpointContentString);
                // Put the results in result for return.  Put data into list.  List of Objects each of Type Henchmen
                result = JsonConvert.DeserializeObject<List<Henchmen>>(endpointContentString);
            }

            Debug.WriteLine("End GetHenchmen Endpoint Call");

            // if endpoint call is successful return result OR return null as initialized
            return result;


        }

    }
}
