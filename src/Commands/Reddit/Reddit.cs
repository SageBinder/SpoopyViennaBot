using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Reddit;
using RestSharp;
using RestSharp.Authenticators;

namespace SpoopyViennaBot.Commands.Reddit
{
    internal static class Reddit
    {
        internal const string BaseTriggerString = "!reddit";

        private static RedditAPI _api;
        private const string UserAgent = "SpoopyViennaBot Reddit Access by u/SaggiSponge";

        private static readonly object ApiSetNotifierObj = new object();
        private static readonly object EstablishingApiFlagLockObj = new object();
        private static readonly object ApiLockObj = new object();

        private static bool _establishingApiFlag;
        private static Task<bool> _establishApiTask;

        internal static RedditAPI GetApi()
        {
            lock(ApiLockObj)
            {
                return _api;
            }
        }

        internal static async Task<RedditAPI> EstablishApiAndGet(int timeout = 30000)
        {
            await EstablishApi(timeout);
            lock(ApiLockObj)
            {
                return _api;
            }
        }

        internal static async Task<bool> EstablishApi(int timeout = 30000)
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

        private static async Task<bool> _EstablishApi(int timeout = 30000)
        {
            Console.WriteLine($"Obtaining access token from reddit (timeout={timeout}ms)...");

            string redditUsername = File.ReadAllText("../../../src/Resources/reddit_username.txt");
            string redditPassword = File.ReadAllText("../../../src/Resources/reddit_password.txt");
            string appSecret = File.ReadAllText("../../../src/Resources/reddit_secret.txt");
            string appId = File.ReadAllText("../../../src/Resources/reddit_app_id.txt");

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

            var restResponse = await restClient.ExecutePostTaskAsync(restRequest);
            var restResponseJson = JObject.Parse(restResponse.Content);

            string accessToken;
            try
            {
                accessToken = (string)restResponseJson.GetValue("access_token");
            }
            catch(InvalidCastException)
            {
                Console.WriteLine(
                    "Error: could not obtain Reddit access token. Received response from https://www.reddit.com/api/v1/access_token:");
                Console.WriteLine(restResponse.Content);
                _api = null;
                lock(EstablishingApiFlagLockObj)
                {
                    _establishingApiFlag = false;
                }
                lock(ApiSetNotifierObj)
                {
                    Monitor.PulseAll(ApiSetNotifierObj);                    
                }
                return false;
            }

            Console.WriteLine("Reddit access token successfully obtained!");
            lock(ApiLockObj)
            {
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