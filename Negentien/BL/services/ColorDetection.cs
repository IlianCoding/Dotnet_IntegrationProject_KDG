using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Color = NT.BL.Domain.users.Color;


namespace NT.BL.RecognitionMngr;

public class ColorDetection
{
    public Color ImageProcessing(byte[] imageBytes)
    {
        // Load the image into a Mat object
        Mat matImage = new Mat();
        CvInvoke.Imdecode(imageBytes, ImreadModes.Color, matImage);

        // Convert the image to Bgr format
        Mat image = new Mat();
        CvInvoke.CvtColor(matImage, image, ColorConversion.Bgr2Gray);

        // Load the face detection cascade
        CascadeClassifier faceCascade = new CascadeClassifier("./haarcascade_frontalface_default.xml");

        // Detect faces
        Rectangle[] faces = faceCascade.DetectMultiScale(image, 1.1, 5, Size.Empty, Size.Empty);

        // Find the largest face
        Rectangle largestFace = Rectangle.Empty;
        foreach (Rectangle face in faces)
        {
            if (largestFace == Rectangle.Empty || face.Width * face.Height > largestFace.Width * largestFace.Height)
            {
                largestFace = face;
            }
        }

        // Crop the image using the coordinates of the largest face
        if (largestFace != Rectangle.Empty)
        {
            largestFace.Offset(0, -35);
            largestFace.Height = 40;
            
            Mat croppedImage = new Mat(matImage, largestFace);
            Image<Bgr, byte> croppedBgrImage = croppedImage.ToImage<Bgr, byte>();
            
            var detectColor = DetectColor(croppedBgrImage);
            return detectColor;
        }

        return Color.None;
    }

    private Color DetectColor(Image<Bgr, byte> image)
    {
        var colorRanges = new Dictionary<string, (ScalarArray min, ScalarArray max)>
        {
            {"orange", (new ScalarArray(new MCvScalar(0, 100, 100)), new ScalarArray(new MCvScalar(10, 255, 255)))},
            {"blue", (new ScalarArray(new MCvScalar(100, 90, 90)), new ScalarArray(new MCvScalar(140, 255, 255)))},
            {"green", (new ScalarArray(new MCvScalar(50, 100, 100)), new ScalarArray(new MCvScalar(70, 255, 255)))},
            {"yellow", (new ScalarArray(new MCvScalar(20, 100, 100)), new ScalarArray(new MCvScalar(40, 255, 255)))},
            {"red", (new ScalarArray(new MCvScalar(10, 100, 100)), new ScalarArray(new MCvScalar(20, 255, 255)))},
            {"pink", (new ScalarArray(new MCvScalar(140, 100, 100)), new ScalarArray(new MCvScalar(170, 255, 255)))}
        };

        Mat hsvImage = new Mat();
        CvInvoke.CvtColor(image, hsvImage, ColorConversion.Bgr2Hsv);

        foreach (var (color, (min, max)) in colorRanges)
        {
            // Creating a mask for the current color
            Mat colorMask = new Mat();
            CvInvoke.InRange(hsvImage, min, max, colorMask);

            // Finding contours
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(colorMask, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);

            // Check if any contours were found for the current color
            if (contours.Size > 0)
            {
                // Get the area of the largest contour
                double maxArea = 0;
                for (int i = 0; i < contours.Size; i++)
                {
                    double area = CvInvoke.ContourArea(contours[i]);
                    if (area > maxArea)
                    {
                        maxArea = area;
                    }
                }

                // Determine the color based on the largest contour
                if (maxArea > 500)
                {
                    var detectedColor = color;
                    Color sendingColor;

                    switch (detectedColor)
                    {
                        case "red":
                            sendingColor = Color.Red;
                            break;
                        case "blue":
                            sendingColor = Color.Blue;
                            break;
                        case "yellow":
                            sendingColor = Color.Yellow;
                            break;
                        case "orange":
                            sendingColor = Color.Orange;
                            break;
                        case "pink":
                            sendingColor = Color.Pink;
                            break;
                        default:
                            sendingColor = Color.None;
                            break;
                    }
                    return sendingColor;
                }
            }
        }
        return Color.None;
    }
}