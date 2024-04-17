﻿using Microsoft.AspNetCore.WebUtilities;
using System.Security.Cryptography;
using System.Text;

namespace SpotifyRecommender.App
{
    public class ApiCaller
    {
        string baseAddress = "https://accounts.spotify.com/";
        HttpClient HttpClient { get; set; } = new HttpClient();

        //CLient id should be moved to options
        const string clientId = "355aad0c14324aedb79a54111bd18add";


        //Generates a random string containing only the possible characters 
        static private string generateRandomString(int length)
        {
            const string possibleCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string values = RandomNumberGenerator.GetString(possibleCharacters, length);
            return values;
        }

        //Creates a new SHA256 and uses it to compute hash from the codeVerifier code
        //Return a new hashed byte array
        static private byte[] EncryptCodeVerifier(string codeVerifier)
        {
            var crypt = SHA256.Create();
            string hash = string.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(codeVerifier));
            return crypto;
        }

        //Converts the hashed byte array back to a Base64 String
        static private string Base64Encode(byte[] hash)
        {
            return System.Convert.ToBase64String(hash);
        }

        public async Task<string> RequestAuthorization()
        {

            string codeVerifier = generateRandomString(69);
            byte[] hashed = EncryptCodeVerifier(codeVerifier);
            string codeChallenge = Base64Encode(hashed);


            var query = new Dictionary<string, string>()
            {
                ["response_type"] = "code",
                ["client_id"] = clientId,
                ["scope"] = "user-read-private user-read-email",
                ["code_challenge_method"] = "S256",
                ["code_challenge"] = codeChallenge,
                ["redirect_uri"] = "/home"
            };

            var uri = QueryHelpers.AddQueryString(baseAddress, query);

            var response = await HttpClient.GetAsync(uri);


            var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return responseBody;
        }
    }
}