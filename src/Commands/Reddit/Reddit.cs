﻿using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Reddit;
using RestSharp;
using RestSharp.Authenticators;
using SpoopyViennaBot.Utils;

namespace SpoopyViennaBot.Commands.Reddit
{
    internal static class Reddit
    {
        internal const int DefaultTimeout = 5000;

        internal static RedditAPI Api { get; private set; }

        private const string UserAgent = "SpoopyViennaBot Reddit Access by u/SaggiSponge";

        private static bool _establishingApiFlag;
        private static Task<bool> _establishApiTask;

        internal static bool ApiIsEstablished()
        {
            if(Api == null)
            {
                return false;
            }

            try
            {
                // To determine whether or not the access token is valid, try to access AskReddit.
                // The framework will throw an exception if it can't access it
                Api.Subreddit("AskReddit").About();

                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine($"_api.Subreddit(\"AskReddit\").About(); threw an exception! ({DateTime.UtcNow})");
                Console.WriteLine(e.Message);
                
                return false;
            }
        }

        internal static async Task<RedditAPI> EstablishApiAndGet(int timeout = DefaultTimeout)
        {
            await EstablishApi(timeout).ConfigureAwait(false);
            return Api;
        }

        internal static async Task<RedditAPI> EstablishApiIfNecessaryAndGet(int timeout = DefaultTimeout) =>
            ApiIsEstablished() ? Api : await EstablishApiAndGet(timeout).ConfigureAwait(false);

        internal static Task<bool> EstablishApi(int timeout = DefaultTimeout)
        {
            if(_establishingApiFlag) return _establishApiTask;

            _establishApiTask = _EstablishApi(timeout);
            return _establishApiTask;
        }

        private static async Task<bool> _EstablishApi(int timeout = DefaultTimeout)
        {
            _establishingApiFlag = true;
            Console.WriteLine($"\nObtaining access token from reddit (timeout={timeout}ms)... ({DateTime.UtcNow})");

            var redditUsername = await Resources.ReadAllTextAsync("reddit_username.txt").ConfigureAwait(false);
            var redditPassword = await Resources.ReadAllTextAsync("reddit_password.txt").ConfigureAwait(false);
            var appSecret = await Resources.ReadAllTextAsync("reddit_secret.txt").ConfigureAwait(false);
            var appId = await Resources.ReadAllTextAsync("reddit_app_id.txt").ConfigureAwait(false);

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

            IRestResponse restResponse = await restClient.ExecutePostTaskAsync(restRequest).ConfigureAwait(false);

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

                Api = null;
                _establishingApiFlag = false;

                return false;
            }

            Console.WriteLine("Reddit access token successfully obtained!");

            Api = new RedditAPI(appId, appSecret: appSecret, accessToken: accessToken);
            _establishingApiFlag = false;

            return true;
        }
    }
}