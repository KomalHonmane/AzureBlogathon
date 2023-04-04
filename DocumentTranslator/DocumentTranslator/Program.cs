using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DocumentTranslator
{
    internal class Program
    {
        static readonly string route = "/batches";
        static readonly string endpoint = "https://documenttranslatorservice.cognitiveservices.azure.com/translator/text/batch/v1.0";
        static readonly string key = "7c62216b69eb4be7bcee172b80eaf86c";

        static readonly string json = ("" +
            "{\"inputs\": " +
                "[{\"source\": " +
                    "{\"sourceUrl\": \"https://documentrstoragesource.blob.core.windows.net/inputdocs?sp=racwdl&st=2023-01-29T07:12:34Z&se=2023-02-03T15:12:34Z&spr=https&sv=2021-06-08&sr=c&sig=67NC8k8YwDT1bvQaSsaE9JCZZTrbTKUUuYWgh%2BJvFH8%3D\"," +
                      "\"storageSource\": \"AzureBlob\"" +
                "}," +
            "\"targets\": " +
                "[{\"targetUrl\": \"https://documentrstoragesource.blob.core.windows.net/translateddocs?sp=racwdl&st=2023-01-29T07:13:21Z&se=2023-02-03T15:13:21Z&spr=https&sv=2021-06-08&sr=c&sig=f0Pet8sOneMGYBqGjt9zW2sCzfcSdGm9J2gOdBrYQp0%3D\"," +
                   "\"storageSource\": \"AzureBlob\"," +
                    "\"language\": \"fr\"}]}]}");
        static async Task Main(string[] args)
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage();
            {
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(endpoint + route);
                request.Headers.Add("Ocp-Apim-Subscription-Key", key);
                request.Content = data;

                HttpResponseMessage response = await client.SendAsync(request);
                string result = response.Content.ReadAsStringAsync().Result;
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Operation successful with status code: {response.StatusCode}");
                }
                else
                    Console.Write($"Error occurred. Status code: {response.StatusCode}");
            }
        }


    }
}
