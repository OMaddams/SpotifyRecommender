﻿using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using SpotifyRecommender.App.Models;
using System.Security.Cryptography;
using System.Text;

namespace SpotifyRecommender.App
{
    public class ApiCaller
    {
        string baseAddress = "https://accounts.spotify.com/";
        string codeVerifier = string.Empty;
        string codeChallenge = string.Empty;
        AcessTokenModel? AcessTokenModel { get; set; } = null;
        public bool isAuthorized = false;
        HttpClient HttpClient { get; set; } = new HttpClient();

        //CLient id should be moved to options
        const string clientId = "355aad0c14324aedb79a54111bd18add";


        //Generates a random byte array
        static private string GenerateRandomString()
        {
            var rng = RandomNumberGenerator.Create();
            var bytes = new byte[32];
            rng.GetBytes(bytes);
            return Base64Encode(bytes);


        }

        //Creates a new SHA256 and uses it to compute hash from the codeVerifier code
        //Return a new hashed byte array encoded to Base 64
        static private string EncryptCodeVerifier(string codeVerifier)
        {
            var codeChallenge = string.Empty;
            using (var sha256 = SHA256.Create())
            {
                var challengeBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(codeVerifier));
                codeChallenge = Base64Encode(challengeBytes);
            }
            return codeChallenge;
        }

        //Converts the byte array back to a Base64 String
        static private string Base64Encode(byte[] bytes)
        {
            return Convert.ToBase64String(bytes).TrimEnd('=').Replace('+', '-').Replace("/", "-");
        }

        public async Task<Uri> RequestAuthorization()
        {

            codeVerifier = GenerateRandomString();
            codeChallenge = EncryptCodeVerifier(codeVerifier);



            var query = new Dictionary<string, string>()
            {
                ["response_type"] = "code",
                ["client_id"] = clientId,
                ["scope"] = "user-read-private user-read-email user-top-read",
                ["code_challenge_method"] = "S256",
                ["code_challenge"] = codeChallenge,
                ["redirect_uri"] = "https://localhost:5000"
            };

            string stringUri = QueryHelpers.AddQueryString("https://accounts.spotify.com/authorize?", query);
            Uri uri = new Uri(stringUri);
            var response = await HttpClient.GetAsync(uri);


            var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return uri;
        }

        public async Task RequestAccessToken(string code)
        {
            var values = new Dictionary<string, string>()
            {
                {"client_id", clientId },
                {"grant_type", "authorization_code" },
                {"code", code},
                {"redirect_uri", "https://localhost:5000" },
                {"code_verifier", codeVerifier }
            };
            //var postJson = JsonConvert.SerializeObject(values);
            //var payload = new StringContent(postJson, encoding: Encoding.UTF8, "application/x-www-form-urlencoded");


            var content = new FormUrlEncodedContent(values);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");
            HttpResponseMessage response = await HttpClient.PostAsync("https://accounts.spotify.com/api/token", content);
            string json = await response.Content.ReadAsStringAsync();

            if (json == null)
            {
                throw new JsonSerializationException();
            }

            var result = JsonConvert.DeserializeObject<AcessTokenModel>(json);
            if (result != null)
            {
                AcessTokenModel = result;
                isAuthorized = true;

            }

        }

        public async Task GetUserTopTracks()
        {
            if (AcessTokenModel != null)
            {
                HttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AcessTokenModel.AccessToken);
                var response = await HttpClient.GetAsync("https://api.spotify.com/v1/me/top/tracks");
                var responseContent = await response.Content.ReadAsStringAsync();

                var userTopTracks = JsonConvert.DeserializeObject<UserTopTracksModel>(responseContent);


            }
        }

    }
}