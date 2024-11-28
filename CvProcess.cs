using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using OpenCVForUnity;

public class CvProcess : MonoBehaviour {

    public Texture2D ImageTexture;
    public Texture2D FinalImage;
    public GameObject ImageMain;
    public int threshhold;


	 public void doProcess() {
         StartCoroutine(CvProcessing());
	}


     IEnumerator CvProcessing()
     {
         Mat imgMat = new Mat(ImageTexture.height, ImageTexture.width, CvType.CV_8UC4);
         Utils.texture2DToMat(ImageTexture, imgMat);

         Mat imgGray = new Mat(ImageTexture.height, ImageTexture.width, CvType.CV_8UC4);
         Imgproc.cvtColor(imgMat, imgGray, Imgproc.COLOR_BGR2GRAY);
         Size GaussianKernalSize = new Size(5, 5);

         Mat imgSmooth = new Mat(ImageTexture.height, ImageTexture.width, CvType.CV_8UC4);
         Imgproc.GaussianBlur(imgGray, imgSmooth, GaussianKernalSize, 0);

         Mat imgEdged = new Mat(ImageTexture.height, ImageTexture.width, CvType.CV_8UC4);
         Imgproc.Canny(imgSmooth, imgEdged, threshhold, threshhold*3);

         Mat tmpMat = new Mat();

         imgEdged.copyTo(tmpMat);
         
         List<MatOfPoint> imgContoursList = new List<MatOfPoint>();
         Mat imgContoursMat = new Mat(ImageTexture.height, ImageTexture.width, CvType.CV_8UC4);
         Imgproc.findContours(tmpMat, imgContoursList, imgContoursMat, Imgproc.RETR_LIST, Imgproc.CHAIN_APPROX_NONE);

         

         double ConPer;
         MatOfPoint2f appPer = new MatOfPoint2f();
         List<MatOfPoint> resultContour = new List<MatOfPoint>();
         bool closed = true;
         bool pfound = false;

         foreach (MatOfPoint imgContour in imgContoursList)
         {
             MatOfPoint2f newPoint = new MatOfPoint2f(imgContour.toArray());
             ConPer = Imgproc.arcLength(newPoint, closed);
             Imgproc.approxPolyDP(newPoint, appPer, ConPer * 0.02, closed);

             MatOfPoint points = new MatOfPoint(appPer.toArray());

             if (appPer.toArray().Length == 4)
             {
                 resultContour.Add(points);
                 pfound = true;
             }

         }

         if(pfound)
         {
             //Scalar green = new Scalar(0, 255, 0);
             //Imgproc.drawContours(imgMat, resultContour, -1, green, 5);

             double[] temp_double;
             List<Point> source = new List<Point>();
             temp_double = appPer.get(0, 0);
             Point p1 = new Point(temp_double[0], temp_double[1]);

             temp_double = appPer.get(1, 0);
             Point p2 = new Point(temp_double[0], temp_double[1]);

             temp_double = appPer.get(2, 0);
             Point p3 = new Point(temp_double[0], temp_double[1]);

             temp_double = appPer.get(3, 0);
             Point p4 = new Point(temp_double[0], temp_double[1]);


             source.Insert(0, p1);
             source.Insert(1, p2);
             source.Insert(2, p3);
             source.Insert(3, p4);

             Mat startM = Converters.vector_Point2f_to_Mat(source);
             Mat resultMat = warp(imgMat, startM);


             Utils.matToTexture2D(resultMat, FinalImage);
             UnityEngine.Rect rec = new UnityEngine.Rect(0, 0, FinalImage.width, FinalImage.height);
             ImageMain.GetComponent<Image>().sprite = Sprite.Create(FinalImage, rec, new Vector2(0.5f, 0.5f));
         }
         


         yield return null;
     }

    




     public Mat warp(Mat inputMat, Mat startM)
     {
         int resultWidth = ImageTexture.width;
         int resultHeight = ImageTexture.height;

         Mat outputMat = new Mat(resultWidth, resultHeight, CvType.CV_8UC4);



         Point ocvPOut1 = new Point(0, 0);
         Point ocvPOut2 = new Point(0, resultHeight);
         Point ocvPOut3 = new Point(resultWidth, resultHeight);
         Point ocvPOut4 = new Point(resultWidth, 0);
         List<Point> dest = new List<Point>();
         dest.Insert(0, ocvPOut1);
         dest.Insert(1, ocvPOut2);
         dest.Insert(2, ocvPOut3);
         dest.Insert(3, ocvPOut4);
         Mat endM = Converters.vector_Point2f_to_Mat(dest);

         Mat perspectiveTransform = Imgproc.getPerspectiveTransform(startM, endM);

         Imgproc.warpPerspective(inputMat,
                                 outputMat,
                                 perspectiveTransform,
                                 new Size(resultWidth, resultHeight),
                                 Imgproc.INTER_CUBIC);

         return outputMat;
     }

}
