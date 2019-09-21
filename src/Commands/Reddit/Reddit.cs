using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Reddit;
using RestSharp;
using RestSharp.Authenticators;

namespace SpoopyViennaBot.Commands.Reddit
{
    internal static class Reddit
    {
        internal const string BaseTriggerString = "!reddit";

        internal const int defaultTimeout = 5000;

        private static RedditAPI _api;
        private static string lastAccessToken = ""; // Locked with ApiLockObj
        private const string UserAgent = "SpoopyViennaBot Reddit Access by u/SaggiSponge";

        private static readonly object EstablishingApiFlagLockObj = new object();
        private static readonly object ApiLockObj = new object();

        private static bool _establishingApiFlag;
        private static Task<bool> _establishApiTask;

        internal static async Task<bool> ApiIsEstablished(int timeout = defaultTimeout)
        {
            Task<IRestResponse> requestTask;

            lock(ApiLockObj)
            {
                if(_api == null)
                {
                    return false;
                }

                // Execute a GET request for api/v1/me/friends.
                // If Reddit returns HTML, it means the access token is INVALID.
                // If Reddit returns a JSON, it means the access token is VALID.
                // (This seems like a shitty way to check if the access token is valid...)
                var restClient = new RestClient("https://oauth.reddit.com")
                {
                    Timeout = timeout
                };
                var restRequest = new RestRequest("/api/v1/me/friends", Method.GET);
                restRequest.AddHeader("Authorization", $"bearer {lastAccessToken}");
                
                requestTask = restClient.ExecuteGetTaskAsync(restRequest);
            }

            try
            {
                await requestTask;
                
                // We check if the request result is a JSON by simply attempting to parse it.
                // This will throw an exception if it's not a valid JSON.
                JObject.Parse(requestTask.Result.Content);
            }
            catch(JsonReaderException e)
            {
                Console.WriteLine("\n/api/v1/me/friends GET did not return a Json. " +
                                  "That means the Reddit access token must be invalid!");
                Console.WriteLine(e.Message);
                
                return false;
            }

            return true;
        }

        internal static RedditAPI GetApi()
        {
            lock(ApiLockObj)
            {
                return _api;
            }
        }

        internal static async Task<RedditAPI> EstablishApiAndGet(int timeout = defaultTimeout)
        {
            await EstablishApi(timeout);
            lock(ApiLockObj)
            {
                return _api;
            }
        }

        internal static async Task<RedditAPI> EstablishApiIfNecessaryAndGet(int timeout = defaultTimeout)
        {
            Task<bool> apiIsEstablished;
            bool apiIsBeingEstablished;

            lock(EstablishingApiFlagLockObj)
            {
                apiIsEstablished = ApiIsEstablished(timeout);
                apiIsBeingEstablished = _establishingApiFlag;
            }

            if(!await apiIsEstablished && apiIsBeingEstablished)
            {
                await _establishApiTask;
                lock(ApiLockObj)
                {
                    return _api;
                }
            }

            if(!await apiIsEstablished)
            {
                return await EstablishApiAndGet(timeout);
            }

            lock(ApiLockObj)
            {
                return _api;
            }
        }

        internal static async Task<bool> EstablishApi(int timeout = defaultTimeout)
        {
            lock(EstablishingApiFlagLockObj)
            {
                if(!_establishingApiFlag)
                {
                    _establishingApiFlag = true;
                    _establishApiTask = _EstablishApi(timeout);
                }
            }

            return await _establishApiTask;
        }

        private static async Task<bool> _EstablishApi(int timeout = defaultTimeout)
        {
            Console.WriteLine($"\nObtaining access token from reddit (timeout={timeout}ms)... ({DateTime.UtcNow})");

            var redditUsername = File.ReadAllText("../../../src/Resources/reddit_username.txt");
            var redditPassword = File.ReadAllText("../../../src/Resources/reddit_password.txt");
            var appSecret = File.ReadAllText("../../../src/Resources/reddit_secret.txt");
            var appId = File.ReadAllText("../../../src/Resources/reddit_app_id.txt");

            var restClient = new RestClient("https://www.reddit.com")
            {
                Authenticator = new HttpBasicAuthenticator(appId, appSecret),
                Timeout = timeout
            };
            var restRequest = new RestRequest("/api/v1/access_token", Method.POST);
            restRequest.AddHeader("User-Agent", UserAgent);
            restRequest.AddParameter("grant_type", "password");
            restRequest.AddParameter("username", redditUsername);
            restRequest.AddParameter("password", redditPassword);

            IRestResponse restResponse = await restClient.ExecutePostTaskAsync(restRequest);
            
            string accessToken;
            try
            {
                var restResponseJson = JObject.Parse(restResponse.Content);
                accessToken = (string)restResponseJson.GetValue("access_token");
            }
            catch(Exception)
            {
                Console.WriteLine("Error: could not obtain Reddit access token. " +
                                  "Received response from https://www.reddit.com/api/v1/access_token:");
                Console.WriteLine(restResponse.Content);

                lock(ApiLockObj)
                {
                    lastAccessToken = "";
                    _api = null;
                }

                lock(EstablishingApiFlagLockObj)
                {
                    _establishingApiFlag = false;
                }

                return false;
            }

            Console.WriteLine("Reddit access token successfully obtained!");
            lock(ApiLockObj)
            {
                lastAccessToken = accessToken;
                _api = new RedditAPI(appId, appSecret: appSecret, accessToken: accessToken);
            }

            lock(EstablishingApiFlagLockObj)
            {
                _establishingApiFlag = false;
            }

            return true;
        }
    }
}