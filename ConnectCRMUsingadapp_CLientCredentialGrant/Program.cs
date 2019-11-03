﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;//make sure version is 3.13.5.907 otherwise authority url got changed

namespace ConnectCRMUsingadapp_CLientCredentialGrant
{
    class Program
    {
        private static readonly string API_URL = "https://vgrade2.api.crm8.dynamics.com/api/data/v9.1/";
        private static readonly string CLIENT_ID = "f3a73937-1f45-43f5-b4cc-a0fa3ea53078";
        private static readonly string CLIENT_SECRET = "xr549Q=Mr?j8aWcnuNSoEhy]aSiMGik/";
        static void Main(string[] args)
        {
            var execute = Task.Run(async () => await Auth());
            Task.WaitAll(execute);
        }

        public static async Task Auth()
        {
            AuthenticationParameters ap = AuthenticationParameters.CreateFromResourceUrlAsync(
            new Uri(API_URL)).Result;
            AuthenticationContext authContext = new AuthenticationContext(ap.Authority);
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

