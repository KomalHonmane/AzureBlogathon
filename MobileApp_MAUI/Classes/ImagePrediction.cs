using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Microsoft.Maui.Graphics.Platform;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MauiMedia.Classes
{
    public class ImagePrediction
    {

        #region obsolete
        private const int ImageMaxSizeBytes = 4194304;
        private const int ImageMaxResolution = 1024;

        public static bool IsRunning { get; private set; }

        public static async Task<PredictionModel> ClassifyImage(Stream photoStream)
        {
            try
            {
                IsRunning = true;

                var endpoint = new CustomVisionPredictionClient(new ApiKeyServiceClientCredentials("65db0bfd444548aeb67e875d6a04324f"))
                {
                    Endpoint = "https://customimageprediction.cognitiveservices.azure.com/"
                };


                // Send image to the Custom Vision API
                var results = await endpoint.ClassifyImageAsync(Guid.Parse("8ccc7d4e-48f8-463a-a10c-599fb64644b4"), "Flowers", photoStream).ConfigureAwait(false);

                // Return the most likely prediction
                return results.Predictions?.OrderByDescending(x => x.Probability).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return new PredictionModel();
            }
            finally
            {
                IsRunning = false;
            }
        }

        #endregion

        public static async Task<byte[]> ResizePhotoStream(FileResult photo)
        {
            byte[] result = null;

            using (var stream = await photo.OpenReadAsync())
            {
                if (stream.Length > ImageMaxSizeBytes)
                {
                    var image = PlatformImage.FromStream(stream);
                    if (image != null)
                    {
                        var newImage = image.Downsize(ImageMaxResolution, true);
                        result = newImage.AsBytes();
                    }
                }
                else
                {
                    using (var binaryReader = new BinaryReader(stream))
                    {
                        result = binaryReader.ReadBytes((int)stream.Length);
                    }
                }
            }

            return result;
        }

        public static async Task<PayloadPrediction> MakePredictionRequest(byte[] imageByteArray)
        {
            try
            {
                var client = new HttpClient();
                var jsonResponse = string.Empty;
                // Request headers - replace with your valid subscription key. 
                client.DefaultRequestHeaders.Add("prediction-key", "a72c0b9983a742479281d5fb401a1588");
                // Prediction URL - replace with your valid prediction URL. 
                string url = "https://customimageprediction-prediction.cognitiveservices.azure.com/customvision/v3.0/Prediction/8ccc7d4e-48f8-463a-a10c-599fb64644b4/classify/iterations/Flowers_Iteration1/image";
                HttpResponseMessage response;
                // Request body. Try this sample with a locally stored image. 
                using (var content = new ByteArrayContent(imageByteArray))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    response = await client.PostAsync(url, content).ConfigureAwait(false);
                    jsonResponse = await response.Content.ReadAsStringAsync();
                    RootObject deserializedRootOjbect = JsonConvert.DeserializeObject<RootObject>(jsonResponse);
                    PayloadPrediction payloadPrediction = new PayloadPrediction(deserializedRootOjbect);                
                    return payloadPrediction;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }

    public class Global
    {
        public string result;
    }
    public class Prediction
    {
        public string TagId { get; set; }
        public string TagName { get; set; }
        public double Probability { get; set; }
    }
    public class PayloadPrediction
    {
        public PayloadPrediction(RootObject rootObject)
        {
            this.IsFlower = (rootObject.Predictions[0].Probability > 0.7) ? true : false;
            this.TagName = rootObject.Predictions[0].TagName;
        }
        public bool IsFlower { get; set; }
        public string TagName { get; set; }
    }
    public class RootObject
    {
        public string Id { get; set; }
        public string Project { get; set; }
        public string Iteration { get; set; }
        public DateTime Created { get; set; }
        public List<Prediction> Predictions { get; set; }
    }

}
