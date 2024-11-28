using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using OpenCVForUnity;

public class CvThresholdProcess : MonoBehaviour {

    public GameObject SliderPanel;
    public Slider ThreshSlider;
    public Texture2D SourceImage;
    public Texture2D EdgedImage;

    int ThreshLocalValue;
    int ThreshSliderValue;

    void start()
    {
        ThreshLocalValue = (int)ThreshSlider.value;
    }

	void Update () {

        ThreshSliderValue = (int)ThreshSlider.value;
        if (SliderPanel.activeInHierarchy)
        {
            
            if(ThreshLocalValue != ThreshSliderValue)
            {
                ThreshLocalValue = ThreshSliderValue;
                StartCoroutine(ShowEdges());
            }

        }

	}


    IEnumerator ShowEdges()
    {
        Mat imgMat = new Mat(SourceImage.height, SourceImage.width, CvType.CV_8UC4);
        Utils.texture2DToMat(SourceImage, imgMat);

        Mat imgGray = new Mat(SourceImage.height, SourceImage.width, CvType.CV_8UC4);
        Imgproc.cvtColor(imgMat, imgGray, Imgproc.COLOR_BGR2GRAY);
        Size GaussianKernalSize = new Size(5, 5);

        Mat imgSmooth = new Mat(SourceImage.height, SourceImage.width, CvType.CV_8UC4);
        Imgproc.GaussianBlur(imgGray, imgSmooth, GaussianKernalSize, 0);

        Mat imgEdged = new Mat(SourceImage.height, SourceImage.width, CvType.CV_8UC4);
        Imgproc.Canny(imgSmooth, imgEdged, ThreshLocalValue, ThreshLocalValue * 2);

        Mat tmpMat = new Mat();

        imgEdged.copyTo(tmpMat);

        List<MatOfPoint> imgContoursList = new List<MatOfPoint>();
        Mat imgContoursMat = new Mat(SourceImage.height, SourceImage.width, CvType.CV_8UC4);
        Imgproc.findContours(tmpMat, imgContoursList, imgContoursMat, Imgproc.RETR_EXTERNAL, Imgproc.CHAIN_APPROX_SIMPLE);



        double ConPer;
        MatOfPoint2f appPer = new MatOfPoint2f();
        List<MatOfPoint> resultContour = new List<MatOfPoint>();
        bool closed = true;

        foreach (MatOfPoint imgContour in imgContoursList)
        {
            MatOfPoint2f newPoint = new MatOfPoint2f(imgContour.toArray());
            ConPer = Imgproc.arcLength(newPoint, closed);
            Imgproc.approxPolyDP(newPoint, appPer, ConPer * 0.02, closed);

            MatOfPoint points = new MatOfPoint(appPer.toArray());

            if (appPer.toArray().Length >= 4)
            {
                resultContour.Add(points);
            }

        }

        Scalar green = new Scalar(0, 255, 0);
        Imgproc.drawContours(imgMat, resultContour, -1, green, 5);
        Utils.matToTexture2D(imgMat, EdgedImage);
        yield return null;
    }


}
