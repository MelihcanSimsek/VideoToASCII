using System.Text;
using OpenCvSharp;

namespace VideoToASCII
{
    internal class Program
    {
        public static char getPixelForGrayValue(byte grayValue)
        {
            //char[] asciiChars = "@&/%?!*+;:,.  ".ToCharArray();
            char[] asciiChars = "    .,:;+*!/%&$@".ToCharArray();
            int index = (grayValue * asciiChars.Length) / 256;
            return asciiChars[index];
        }

        static void Main(string[] args)
        {
            string videoPath = @"your-video-file-path";  
            VideoCapture videoCapture = new VideoCapture(videoPath);

            if (!videoCapture.IsOpened())
            {
                Console.WriteLine("Error: Unable to open video file.");
                return;
            }

            double fps = videoCapture.Fps;
            double frameDurationMs =(1000 / fps);
            int height = 200;
            int width = 200;
            List<StringBuilder> lineList = new List<StringBuilder>();

            while (true)
            {
                StringBuilder line = new StringBuilder();
                Mat frame = new Mat();
                if (!videoCapture.Read(frame)) break;
                var resizedFrame = frame.Resize(new OpenCvSharp.Size(width, height));
                Cv2.CvtColor(resizedFrame, resizedFrame, ColorConversionCodes.BGR2GRAY);
                for (int x = 0; x < resizedFrame.Rows; x++)
                {
                    for (int y = 0; y < resizedFrame.Cols; y++)
                    {
                        char pixelValue = getPixelForGrayValue(resizedFrame.At<byte>(x, y)); //2^8
                        line.Append(pixelValue);
                    }
                    line.Append("\n");
                }
                lineList.Add(line);
            }

            foreach (var line in lineList)
            {
                Console.Clear();
                Console.WriteLine(line);
                Thread.Sleep((int)frameDurationMs);
            }

        }
    }
}
