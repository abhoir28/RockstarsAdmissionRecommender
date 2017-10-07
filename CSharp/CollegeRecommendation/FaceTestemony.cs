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
    }
}