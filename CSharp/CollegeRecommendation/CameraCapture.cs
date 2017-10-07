using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace CollegeRecommendation
{
    public class CameraCapture
    {
        Capture capture;

        public Bitmap capturePhoto()
        {

            if (capture == null)
            {

                capture = new Capture();

            }

            //capture.QueryFrame();
            Image<Bgr, Byte> img = capture.QueryFrame().ToImage<Bgr, Byte>();
            Bitmap bmp = img.Bitmap;

            //MemoryStream stream = new MemoryStream();
            //bmp.Save(stream, bmp.RawFormat);
            //byte[] result = stream.ToArray();


            //  bmp.Save(@"F:\44.jpg");

            if (capture != null)
            {

                capture.Dispose();

            }

            return bmp;

        }
    }
}