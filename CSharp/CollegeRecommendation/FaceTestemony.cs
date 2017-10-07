using System;
using System.Collections.Generic;

using System.Threading.Tasks;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

using System.IO;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using AdaptiveCards;

namespace CollegeRecommendation
{
    public class Rootobject
    {
        public Rootobject()
        {
        }

        public Class1[] Property1 { get; set; }
    }

    public class Class1
    {
        public Facerectangle faceRectangle { get; set; }
        public Scores scores { get; set; }
    }

    public class Facerectangle
    {
        public int height { get; set; }
        public int left { get; set; }
        public int top { get; set; }
        public int width { get; set; }
    }

    public class Scores
    {
        public string anger { get; set; }
        public string contempt { get; set; }
        public string disgust { get; set; }
        public string fear { get; set; }
        public string happiness { get; set; }
        public string neutral { get; set; }
        public string sadness { get; set; }
        public string surprise { get; set; }
    }
    [Serializable]
    public class FaceTestemony : IDialog
    {
        
        public async Task StartAsync(IDialogContext context)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "\\images\\image.jpg";
            CameraCapture cc = new CameraCapture();
            Bitmap bmp = cc.capturePhoto();
            bmp.Save(path);

            var replyMessage = context.MakeMessage();
            Attachment attachment = null;
            attachment = ImageCapture();
            replyMessage.Attachments = new List<Attachment> { attachment };
            await context.PostAsync(replyMessage);

            List<Scores> listColleges = await MakeRequest(context);
            var reply = context.MakeMessage();
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments = GetApiCardsAttachments(listColleges);
            await context.PostAsync(reply);
            String resultEmo = GetEmotion(listColleges);
            await context.PostAsync(resultEmo);
            context.Done<object>(null);

        }

        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }

        private static async Task<List<Scores>> MakeRequest(IDialogContext context)
        {
            var client = new HttpClient();

            // Request headers - replace this example key with your valid key.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "788195420288455e87bec32b9a7aaab2");

            string uri = "https://westus.api.cognitive.microsoft.com/emotion/v1.0/recognize?";
            HttpResponseMessage response;
            string responseContent;

            String path = AppDomain.CurrentDomain.BaseDirectory + "images\\image.jpg";
            // Request body. Try this sample with a locally stored JPEG image.
            byte[] byteData = GetImageAsByteArray(path);

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
                        sc.happiness = (string)ScoresJobject["happiness"];
                        sc.fear = (string)ScoresJobject["fear"];
                        sc.neutral = (string)ScoresJobject["neutral"];
                        sc.sadness = (string)ScoresJobject["sadness"];
                        sc.surprise = (string)ScoresJobject["surpise"];


                        lsc.Add(sc);

                    }


                }
            }


            return lsc;


        }

        private IList<Attachment> GetApiCardsAttachments(List<Scores> listEmotions)
        {
            List<Attachment> attachment = new List<Attachment>();

            foreach (Scores c in listEmotions)
            {

                var resAttach = GetEmotionCard(
                    c.anger,
                    c.contempt,
                    c.disgust,
                    c.fear,
                    c.happiness,
                    c.neutral,
                    c.sadness,
                    c.surprise
                    );

                attachment.Add(resAttach);


            }

            return attachment;
        }

        private static Attachment GetEmotionCard(string angry, string contempt, string disgust, string fear, string happy, string neutral, string sad, string surprise)
        {
            AdaptiveCard card = new AdaptiveCard()
            {
                Body = new List<CardElement>()
                {
                    new AdaptiveCards.Container()
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
                                                Text = "Angry",
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
                                                Url = "https://s3-ap-southeast-1.amazonaws.com/botframework21/images/contmp.jpg",
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
                                                Text =  "Contempt",
                                                Weight = TextWeight.Bolder,
                                                IsSubtle = true
                                            },
                                            new TextBlock()
                                            {
                                                Text = contempt,
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
                                                Text = "Fear",
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
                                                Text =  "Happy",
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
                                                Url = "https://s3-ap-southeast-1.amazonaws.com/botframework21/images/neutral.jpg",
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
                                                Text =  "Neutral",
                                                Weight = TextWeight.Bolder,
                                                IsSubtle = true
                                            },
                                            new TextBlock()
                                            {
                                                Text = neutral,
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
                                                Url = "https://s3-ap-southeast-1.amazonaws.com/botframework21/images/surprised.jpg",
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
                                                Text =  "Surprise",
                                                Weight = TextWeight.Bolder,
                                                IsSubtle = true
                                            },
                                            new TextBlock()
                                            {
                                                Text = surprise,
                                                Wrap = true
                                            }
                                        }
                                    }
                                }
                            },


                        }
                    }
                },


            };


            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };

            return attachment;
        }

        public static Attachment ImageCapture()
        {
            String imagePath = AppDomain.CurrentDomain.BaseDirectory + "\\images\\image.jpg";
            //String imagePath = "https://s3-ap-southeast-1.amazonaws.com/emotiondata17/songod.jpg";

            var imageData = Convert.ToBase64String(File.ReadAllBytes(imagePath));
            return new Attachment
            {
                Name = "image.jpg",
                ContentType = "image/jpeg",
                ContentUrl = $"data:image/png;base64,{imageData}"
            };

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
            return EmoDictionary[temp];
            //string abc = dictionary[temp];/*dictionary.TryGetValue(temp);*/
            //return abc;
        }
    }
}