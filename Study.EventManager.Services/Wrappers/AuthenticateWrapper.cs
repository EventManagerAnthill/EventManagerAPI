using Newtonsoft.Json;
using RestSharp;
using Study.EventManager.Services.Models.APIModels;
using Study.EventManager.Services.Wrappers.Contracts;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Study.EventManager.Services.Wrappers
{
    public class AuthenticateWrapper : IAuthenticateWrapper
    {
        private RestClient _client;
        private readonly string _facebookURL;
        private readonly string _accesstokenUrl;
        private readonly string _userDataUrl;
        public AuthenticateWrapper()
        {
            _facebookURL = "https://graph.facebook.com";
            _accesstokenUrl = "/me?access_token=";
            _userDataUrl = "?fields=id,name,first_name,last_name,email&access_token=";

            _client = new RestClient(_facebookURL);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
        }

        public async Task<FacebookTokenAPIModel> GetFacebookToken(string idToken)
        {
            var request = new RestRequest($"{_accesstokenUrl}{idToken}", Method.GET, DataFormat.Json);
            request.AddHeader("Accept", "application/json");

            try
            {
                IRestResponse response = await _client.ExecuteAsync(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var answer = JsonConvert.DeserializeObject<FacebookTokenAPIModel>(response.Content);
                    return answer;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<FacebookUserAPIModel> GetFacebookUserData(string fbUserId, string idToken)
        {
            var request = new RestRequest($"/{fbUserId}/{_userDataUrl}{idToken}", Method.GET, DataFormat.Json);
            request.AddHeader("Accept", "application/json");

            try
            {
                IRestResponse response = await _client.ExecuteAsync(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var answer = JsonConvert.DeserializeObject<FacebookUserAPIModel>(response.Content);
                    return answer;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }           
        }
    }
}
