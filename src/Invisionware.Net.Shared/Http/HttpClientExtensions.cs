﻿using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Invisionware.Net.Http
{
    public static class HttpClientExtensions
    {
        public static Task<Stream> GetFileAsync(this HttpClient client, Uri requestUri)
        {
            var result = client.GetAsync(requestUri).ContinueWith(
                requestTask =>
                {
                    var response = requestTask.Result;

                    response.EnsureSuccessStatusCode();

                    return response.Content.ReadAsStreamAsync();
                });

            return result.Unwrap();
        }

        public static Task<string> GetFileAsBase64Async(this HttpClient client, Uri requestUri)
        {
            var result = client.GetFileAsync(requestUri).ContinueWith(
                requestTask =>
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        requestTask.Result.CopyTo(memoryStream);
                        var byteArray = memoryStream.ToArray();

                        return Convert.ToBase64String(byteArray);
                    }
                });

            return result;
        }
    }
}