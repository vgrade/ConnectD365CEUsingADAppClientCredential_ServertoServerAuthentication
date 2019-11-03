using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConnectCRMUsingadapp_ClientCredentialGrant
{
    //Change urls and appid accoridng to yours
    class Program
    {
        private static readonly string API_URL = "https://vgrade2.api.crm8.dynamics.com/api/data/v9.1/";
        private static readonly string CLIENT_ID = "f3a73937-1f45-43f5-b4cc-a0fa3ea53078";
        private static readonly string CLIENT_SECRET = "<yoursecret>";
        static void Main(string[] args)
        {
            var execute = Task.Run(async () => await Auth());
            Task.WaitAll(execute);
        }

        public static async Task Auth()
        {
            // use below lines for old version (3.13.5)  of adal library
            //    AuthenticationParameters ap = AuthenticationParameters.CreateFromResourceUrlAsync(
            //    new Uri(API_URL)).Result;

            //Use below lines for latest version of adal library
            AuthenticationParameters ap = AuthenticationParameters.CreateFromUrlAsync(
           new Uri(API_URL)).Result; //for latest version of adal
            
            // use below lines for old version (3.13.5)  of adal library
            //AuthenticationContext authContext = new AuthenticationContext(ap.Authority);

            //Use below lines for latest version of adal library
            AuthenticationContext authContext = new AuthenticationContext("https://login.microsoftonline.com/<yourtenantid>");
            var clinetCredential = new ClientCredential(CLIENT_ID, CLIENT_SECRET);
            var token = authContext.AcquireTokenAsync(ap.Resource, clinetCredential).Result.AccessToken;
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.Timeout = new TimeSpan(0, 2, 0);
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await httpClient.GetAsync(API_URL + "/contacts?$top=1");
            }
        }
    }
}

