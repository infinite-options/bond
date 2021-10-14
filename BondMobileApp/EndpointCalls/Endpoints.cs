using System;
using System.Collections.Generic;   // Needed for List functionality
using System.Diagnostics;           // Need for Debug command.  Changes the color of output compared to Console.WriteLine
using System.Net.Http;              // Needed for HttpClient
using System.Threading.Tasks;       // Allows data to be returned in an async function
using Newtonsoft.Json;              // Needed for JsonConvert
using BondMobileApp.EndpointDataClasses;



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
        public async Task<List<HenchmenClass>> GetHenchmen()
        {
            Debug.WriteLine("\nEndpoints.cs: Inside GetHenchmen Endpoint Call");
            baseURL = "https://iznfqs92n3.execute-api.us-west-1.amazonaws.com/dev/";
            suffix = "api/v2/sidekicks";
            endpoint = baseURL + suffix;
            Debug.WriteLine("Endpoints.cs: Endpoint URL: " + endpoint);

            // Set the result of the upcoming call to null
            List<HenchmenClass> result = null;
            Debug.WriteLine("Endpoints.cs: 1 " + result);
            var client = new HttpClient();  // HttpClient requires using System.Net.Http which defines what dot attributes it has
            Debug.WriteLine("Endpoints.cs: 2 " + client);
            var endpointCall = await client.GetAsync(endpoint);  // Endpoint call definition.  Requires Newtonsoft.Json;  endpointCall variable has dot attributes
            Debug.WriteLine("Endpoints.cs: endpoint called");
            Debug.WriteLine("Endpoints.cs: After endpoint call Status Code " + endpointCall.IsSuccessStatusCode);

            if (endpointCall.IsSuccessStatusCode)  // Check if endpoint call was successful
            {
                var endpointContentString = await endpointCall.Content.ReadAsStringAsync();  // Converts data into a String and stores it
                //Debug.WriteLine("Endpoint.cs: From Endpoint Call: " + endpointContentString);  // Print statement to see what data was received.  Especially valuable with breakpoints
                result = JsonConvert.DeserializeObject<List<HenchmenClass>>(endpointContentString);  // Put the results in result for return.  Put data into list.  List of Objects each of Type Henchmen
            }

            Debug.WriteLine("Endpoint.cs: End GetHenchmen Endpoint Call");
            Debug.WriteLine("Print result before return statement: " + result);

            return result;  // if endpoint call is successful return result OR return null as initialized
        }

    }
}
