using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDGenWebsite.Utilities
{
    public class ApiHelper
    {
        public async Task GetOrgs() {

            string baseUrl = "https://franklin.focusschoolsoftware.com/focus/api/1.0/ims/oneroster/v1p1";
            string clientId = "4173e207-702a-4300-aaf2-30c8aa9427c2";
            string clientSecret = "f4e1a06d-cf5d-40da-b62a-0d2eae010ac2";
            //Request Token
            var restClient = new RestClient(baseUrl);
            RestRequest request = new RestRequest("token") { Method = Method.POST };
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("client_id", clientId);
            request.AddParameter("client_secret", clientSecret);
            request.AddParameter("grant_type", "client_credentials");
            var tresponse = restClient.Execute(request);
            var responseJson = tresponse.Content;
            var token = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseJson)["access_token"].ToString();

            //get the orgs
            //RestRequest orgsRequest = new RestRequest(/orgs?limit=500&offset=0)

        }
    }
}
