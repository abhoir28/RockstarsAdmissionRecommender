using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;
using Microsoft.Bot.Connector;
using System.Drawing;
using System.Timers;

namespace CollegeRecommendation
{
   
    [Serializable]
    public class FusionQuery : IDialog
    {
        String resultEmo = null;
        String resultTxt = null;
        double convEmo;
        double convTxt;
        public async Task StartAsync(IDialogContext context)
        {

            CameraCapture cc = new CameraCapture();
            Bitmap bmp = cc.capturePhoto();
            String path = AppDomain.CurrentDomain.BaseDirectory + "\\images\\image.jpg";
            bmp.Save(path);

            List<Scores> listColleges = await MakeRequestEmo(context);
            resultEmo = GetEmotion(listColleges);
            //List<Scores> listTextEmo = await MakeRequestText(context);

            await context.PostAsync("Did you get your desired college in the above List?");
            context.Wait(TextEmo);
        }

        

        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }

        public static async Task<List<Scores>> MakeRequestEmo(IDialogContext context)
        {
            var client = new HttpClient();

            // Request headers - replace this example key with your valid key.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "788195420288455e87bec32b9a7aaab2");

            string uri = "https://westus.api.cognitive.microsoft.com/emotion/v1.0/recognize?";
            HttpResponseMessage response;
            string responseContent;

            String path = AppDomain.CurrentDomain.BaseDirectory + "\\images\\image.jpg";
            //String path = AppDomain.CurrentDomain.BaseDirectory + "\\images\\image.jpg";
            // Request body. Try this sample with a locally stored JPEG image.


            byte[] byteData = GetImageAsByteArray(path);
            //byte[] byteData = bmp;

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(uri, content);
                responseContent = response.Content.ReadAsStringAsync().Result;
            }

            JArray array = JArray.Parse(responseContent);

            //Console.Write("Result:" + array[0]);

            List<Scores> lsc = new List<Scores>();

            Scores sc;

            JObject ScoresJobject;

            foreach (JObject obj in array.Children<JObject>())
            {
                foreach (JProperty singleProp in obj.Properties())
                {
                    string name = singleProp.Name;
                    string value = singleProp.Value.ToString();

                    string data = value;

                    if (name == "scores")
                    {
                        ScoresJobject = JObject.Parse(data);

                        sc = new Scores();

                        sc.anger = (string)ScoresJobject["anger"];
                        sc.contempt = (string)ScoresJobject["contempt"];
                        sc.disgust = (string)ScoresJobject["disgust"];
                        sc.fear = (string)ScoresJobject["fear"];
                        sc.happiness = (string)ScoresJobject["happiness"];
                        sc.neutral = (string)ScoresJobject["neutral"];
                        sc.sadness = (string)ScoresJobject["sadness"];
                        sc.surprise = (string)ScoresJobject["surprise"];

                        lsc.Add(sc);

                    }


                }
            }
            return lsc;
        }

        public String GetEmotion(List<Scores> listEmotions)
        {
            //List<String> value = new List<String>();
            String value = null;
            foreach (Scores c in listEmotions)
            {

                String resAttach = GetHighestEmo(
                    c.anger,
                    c.contempt,
                    c.disgust,
                    c.fear,
                    c.happiness,
                    c.neutral,
                    c.sadness,
                    c.surprise
                    );
                //value.Add(resAttach);
                value = resAttach;
            }
            //string ListToStr = string.Join(",", value.ToArray());
            return value;
        }

        public String GetHighestEmo(string angry, string contempt, string disgust, string fear, string happy, string neutral, string sad, string surprise)
        {
            double a = Convert.ToDouble(angry);
            double b = Convert.ToDouble(contempt);
            double c = Convert.ToDouble(disgust);
            double d = Convert.ToDouble(fear);
            double e = Convert.ToDouble(happy);
            double f = Convert.ToDouble(neutral);
            double g = Convert.ToDouble(sad);
            double h = Convert.ToDouble(surprise);

            double[] arr = new double[] { a, b, c, d, e, f, g, h };
            double temp = 0;

            Dictionary<double, string> EmoDictionary = new Dictionary<double, string>();

            EmoDictionary.Add(a, "Angry");
            EmoDictionary.Add(b, "Contempt");
            EmoDictionary.Add(c, "Disgust");
            EmoDictionary.Add(d, "Fear");
            EmoDictionary.Add(e, "Happy");
            EmoDictionary.Add(f, "Neutral");
            EmoDictionary.Add(g, "Sad");
            EmoDictionary.Add(h, "Surprise");


            //if (a > b && a > c && a > d && a > e && a > f && a > g && a > h)
            for (int i = 0; i <= 7; i++)
            {
                if (temp < arr[i])
                {
                    temp = arr[i];
                }

            }
            convEmo = temp * 0.2;
            return EmoDictionary[temp];
            //string abc = dictionary[temp];/*dictionary.TryGetValue(temp);*/
            //return abc;
        }


        public async Task TextEmo(IDialogContext context, IAwaitable<object> result)
        {
            var reply1 = await result as Activity;

            //List<Score> listColleges = await MakeRequestTextEmotions();
            string baseURL;
            string username;
            string password;
            string ResponseVal;

            baseURL = "https://gateway.watsonplatform.net/tone-analyzer/api/v3/tone?version=2016-05-19&sentences=false";
            username = "37fc928f-109a-4ed8-9bde-90caa8f3dbe7";
            password = "wPL1NkiF3K4m";

            var intext = reply1.Text;
            // Get the data to be analyzed for tone
            string postData = "{\"text\": \"" + intext + "\"}";

            // Create the web request
            var request = (HttpWebRequest)WebRequest.Create(baseURL);

            // Configure the BlueMix credentials
            string auth = string.Format("{0}:{1}", username, password);
            string auth64 = Convert.ToBase64String(Encoding.ASCII.GetBytes(auth));
            string credentials = string.Format("{0} {1}", "Basic", auth64);

            // Set the web request parameters
            request.Headers[HttpRequestHeader.Authorization] = credentials;
            request.Method = "POST";
            request.Accept = "application/json";
            request.ContentType = "application/json";

            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            // Set the ContentLength property of the WebRequest
            request.ContentLength = byteArray.Length;

            // Get the request stream
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);

            // Get the response
            WebResponse response = request.GetResponse();
            // Display the status
            // Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the service
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access
            StreamReader reader = new StreamReader(dataStream);
            // Read and format the content
            string responseFromServer = reader.ReadToEnd();
            responseFromServer = ToneAnalyzerTools.JsonPrettify(responseFromServer);


            // Dynamically assign the JSON to objects
            JObject DocumentTone = JObject.Parse(responseFromServer);
            JArray ToneCategories = (JArray)DocumentTone["document_tone"]["tone_categories"];

            // Loop through the categories returned in the JSON
            dynamic categories = ToneCategories;

            List<Score> lsc = new List<Score>();

            Score sc;

            foreach (dynamic category in categories)
            {
                // Random troubleshooting code; did this in the beginning to check values
                Console.WriteLine(category.category_id);
                Console.WriteLine(category.category_name);

                if (category.category_id == "emotion_tone")
                {
                    int i = 0;

                    sc = new Score();
                    foreach (dynamic tone in category.tones)
                    {
                        int iwe = 0;

                        switch ((string)tone.tone_id)
                        {
                            case "anger":
                                sc.anger = tone.score;
                                break;
                            case "disgust":
                                sc.disgust = tone.score;
                                break;
                            case "fear":
                                sc.fear = tone.score;
                                break;
                            case "joy":
                                sc.happiness = tone.score;
                                break;
                            case "sadness":
                                sc.sadness = tone.score;
                                break;
                        }



                    }

                    lsc.Add(sc);
                }

            }

            //return lsc;

            //var reply = context.MakeMessage();

            //reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            resultTxt = GetTxtEmo(lsc);
            String resultFusion = FusionEmoTxt();
            await context.PostAsync(resultFusion);
            context.Done<object>(null);
        }

        private String GetTxtEmo(List<Score> listTextEmotions)
        {
            //List<Attachment> attachment = new List<Attachment>();
            String txtValue = null;
            foreach (Score c in listTextEmotions)
            {

                String resAttach = GetEmotionText(
                    c.sadness,
                    c.happiness,
                    c.anger,
                    c.disgust,
                    c.fear
                    );

                txtValue = resAttach;
            }
            return txtValue;
        }

        public String GetEmotionText(string sadness, string happiness, string anger, string disgust, string fear)
        {
            double s = Convert.ToDouble(sadness);
            double h = Convert.ToDouble(happiness);
            double a = Convert.ToDouble(anger);
            double d = Convert.ToDouble(disgust);
            double f = Convert.ToDouble(fear);

            double[] arr = new double[] { s, h, a, d, f };
            double tempo = 0;

            Dictionary<double, string> TextDictionary = new Dictionary<double, string>();

            TextDictionary.Add(s, "Sadness");
            TextDictionary.Add(h, "Happiness");
            TextDictionary.Add(a, "Anger");
            TextDictionary.Add(d, "Disgust");
            TextDictionary.Add(f, "Fear");


            //if (a > b && a > c && a > d && a > e && a > f && a > g && a > h)
            for (int i = 0; i <= 4; i++)
            {
                if (tempo < arr[i])
                {
                    tempo = arr[i];
                }

            }
            convTxt = tempo * 0.8;
            return TextDictionary[tempo];
            //string abc = dictionary[temp];/*dictionary.TryGetValue(temp);*/
            //return abc;
        }

        public String FusionEmoTxt()
        {
            Dictionary<double, string> FinalEmoDict = new Dictionary<double, string>();
            double temporary = 0;
            double[] arr = new double[] { convEmo, convTxt };
            FinalEmoDict.Add(convEmo, resultEmo);
            FinalEmoDict.Add(convTxt, resultTxt);


            for (int i = 0; i <= 1; i++)
            {
                if (temporary < arr[i])
                {
                    temporary = arr[i];
                }

            }
            return FinalEmoDict[temporary];
        }
    }
    internal class ToneAnalyzerTools
    {
        public static string JsonPrettify(string json)
        {
            return Newtonsoft.Json.Linq.JObject.Parse(json).ToString();
        }

    }
}