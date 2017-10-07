using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.FormFlow;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using AdaptiveCards;
using System.Text;

namespace CollegeRecommendation
{
    //Classes used by Text Emotion
    public class Rootobject1
    {
        public Document_Tone document_tone { get; set; }
    }

    public class Document_Tone
    {
        public Tone_Categories[] tone_categories { get; set; }
    }

    public class Tone_Categories
    {
        public Tone[] tones { get; set; }
        public string category_id { get; set; }
        public string category_name { get; set; }
    }

    public class Tone
    {
        public float score { get; set; }
        public string tone_id { get; set; }
        public string tone_name { get; set; }
    }

    public class Score
    {
        public string anger { get; set; }
        public string disgust { get; set; }
        public string fear { get; set; }
        public string happiness { get; set; }
        public string sadness { get; set; }

    }
    [Serializable]
    public class TextEmotion : IDialog
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Please provide a text input:");
            context.Wait(TextEmo);
        }

        public async Task TextEmo(IDialogContext context, IAwaitable<object> result)
        {
            var reply1 = await result as Activity;

            string baseURL;
            string username;
            string password;
            string ResponseVal;

            //Essential connections strings for connecting to IBM Tone Analyzer
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

            var reply = context.MakeMessage();
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments = GetApiCardsAttachments(lsc);
            await context.PostAsync(reply);
            String resultTxt = GetTxtEmo(lsc);
            await context.PostAsync(resultTxt);
            context.Done<object>(null);
        }

        private IList<Attachment> GetApiCardsAttachments(List<Score> listEmotions)
        {
            List<Attachment> attachment = new List<Attachment>();

            foreach (Score c in listEmotions)
            {

                var resAttach = GetEmotionCard(
                    c.sadness,
                    c.happiness,
                    c.anger,
                    c.disgust,
                    c.fear
                    );

                attachment.Add(resAttach);


            }

            return attachment;
        }

        private static Attachment GetEmotionCard(string sad, string happy, string angry, string disgust, string fear)
        {
            AdaptiveCard card = new AdaptiveCard()
            {
                Body = new List<CardElement>()
                {
                    new Container()
                    {
                        Speak = "<s>Hello!</s><s>Are you looking for a flight or a hotel?</s>",
                        Items = new List<CardElement>()
                        {
                            new ColumnSet()
                            {
                                Columns = new List<Column>()
                                {
                                    new Column()
                                    {
                                        Size = ColumnSize.Auto,
                                        Items = new List<CardElement>()
                                        {
                                            new AdaptiveCards.Image()
                                            {
                                                Url = "https://s3-ap-southeast-1.amazonaws.com/botframework21/images/sad.jpg",
                                                Size = ImageSize.Medium,
                                                Style = ImageStyle.Person
                                            }
                                        }
                                    },
                                    new Column()
                                    {
                                        Size = ColumnSize.Stretch,
                                        Items = new List<CardElement>()
                                        {
                                            new TextBlock()
                                            {
                                                Text = "Sad",
                                                Weight = TextWeight.Bolder,
                                                IsSubtle = true
                                            },
                                            new TextBlock()
                                            {

                                                Text = sad,
                                                Wrap = true
                                            }
                                        }
                                    }
                                }
                            },
                            new ColumnSet()
                            {
                                Columns = new List<Column>()
                                {
                                    new Column()
                                    {
                                        Size = ColumnSize.Auto,
                                        Items = new List<CardElement>()
                                        {
                                            new AdaptiveCards.Image()
                                            {
                                                Url = "https://s3-ap-southeast-1.amazonaws.com/botframework21/images/happy.jpg",
                                                Size = ImageSize.Medium,
                                                Style = ImageStyle.Person
                                            }
                                        }
                                    },
                                    new Column()
                                    {
                                        Size = ColumnSize.Stretch,
                                        Items = new List<CardElement>()
                                        {
                                            new TextBlock()
                                            {
                                                Text =  "happy",
                                                Weight = TextWeight.Bolder,
                                                IsSubtle = true
                                            },
                                            new TextBlock()
                                            {
                                                Text = happy,
                                                Wrap = true
                                            }
                                        }
                                    }
                                }
                            },
                             new ColumnSet()
                            {
                                Columns = new List<Column>()
                                {
                                    new Column()
                                    {
                                        Size = ColumnSize.Auto,
                                        Items = new List<CardElement>()
                                        {
                                            new AdaptiveCards.Image()
                                            {
                                                Url = "https://s3-ap-southeast-1.amazonaws.com/botframework21/images/angry.jpg",
                                                Size = ImageSize.Medium,
                                                Style = ImageStyle.Person
                                            }
                                        }
                                    },
                                    new Column()
                                    {
                                        Size = ColumnSize.Stretch,
                                        Items = new List<CardElement>()
                                        {
                                            new TextBlock()
                                            {
                                                Text =  "Anger",
                                                Weight = TextWeight.Bolder,
                                                IsSubtle = true
                                            },
                                            new TextBlock()
                                            {
                                                Text = angry,
                                                Wrap = true
                                            }
                                        }
                                    }
                                }
                            },
                             new ColumnSet()
                            {
                                Columns = new List<Column>()
                                {
                                    new Column()
                                    {
                                        Size = ColumnSize.Auto,
                                        Items = new List<CardElement>()
                                        {
                                            new AdaptiveCards.Image()
                                            {
                                                Url = "https://s3-ap-southeast-1.amazonaws.com/botframework21/images/disgusted-smiley-face.jpg",
                                                Size = ImageSize.Medium,
                                                Style = ImageStyle.Person
                                            }
                                        }
                                    },
                                    new Column()
                                    {
                                        Size = ColumnSize.Stretch,
                                        Items = new List<CardElement>()
                                        {
                                            new TextBlock()
                                            {
                                                Text =  "Disgust",
                                                Weight = TextWeight.Bolder,
                                                IsSubtle = true
                                            },
                                            new TextBlock()
                                            {
                                                Text = disgust,
                                                Wrap = true
                                            }
                                        }
                                    }
                                }
                            },
                             new ColumnSet()
                            {
                                Columns = new List<Column>()
                                {
                                    new Column()
                                    {
                                        Size = ColumnSize.Auto,
                                        Items = new List<CardElement>()
                                        {
                                            new AdaptiveCards.Image()
                                            {
                                                Url = "https://s3-ap-southeast-1.amazonaws.com/botframework21/images/fear.jpg",
                                                Size = ImageSize.Medium,
                                                Style = ImageStyle.Person
                                            }
                                        }
                                    },
                                    new Column()
                                    {
                                        Size = ColumnSize.Stretch,
                                        Items = new List<CardElement>()
                                        {
                                            new TextBlock()
                                            {
                                                Text =  "Fear",
                                                Weight = TextWeight.Bolder,
                                                IsSubtle = true
                                            },
                                            new TextBlock()
                                            {
                                                Text = fear,
                                                Wrap = true
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };


            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };

            return attachment;
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
            return TextDictionary[tempo];
            //string abc = dictionary[temp];/*dictionary.TryGetValue(temp);*/
            //return abc;
        }
    
    }
}

internal class ToneAnalyzerTools
{
    public static string JsonPrettify(string json)
    {
        return Newtonsoft.Json.Linq.JObject.Parse(json).ToString();
    }

}