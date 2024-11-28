using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using OpenCVForUnity;

public class CvWordProcess : MonoBehaviour {

    public Texture2D ImageTexture;
    public GameObject ImageMain;
    public GameObject PostProcessPanel;

    public int threshhold;

    public void doCvWordProcess()
    {
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
        Imgproc.threshold(imgSmooth, imgEdged, threshhold, threshhold * 3, Imgproc.THRESH_BINARY_INV);

        Mat tmpMat = new Mat();
        imgEdged.copyTo(tmpMat);

        Size tempSize = new Size(2,2);
        Mat kernal = Imgproc.getStructuringElement(Imgproc.MORPH_CROSS, tempSize);
        Mat imgDilated = new Mat(ImageTexture.height, ImageTexture.width, CvType.CV_8UC4);
        Point tempPoint = new Point(-1,-1);
        Imgproc.dilate(tmpMat, imgDilated, kernal, tempPoint, 2);

        

        List<MatOfPoint> imgContoursList = new List<MatOfPoint>();
        Mat imgContoursMat = new Mat(ImageTexture.height, ImageTexture.width, CvType.CV_8UC4);
        Imgproc.findContours(imgDilated, imgContoursList, imgContoursMat, Imgproc.RETR_LIST, Imgproc.CHAIN_APPROX_NONE);

        Scalar green = new Scalar(0, 255, 0);

        foreach (MatOfPoint imgContour in imgContoursList)
        {
            OpenCVForUnity.Rect rect = Imgproc.boundingRect(imgContour);
	        if(rect.width < 10 || rect.height < 10)
            {
                continue;
            }

            Core.rectangle(imgMat, new Point(rect.x,rect.y), new Point(rect.x+rect.width,rect.y+rect.height), green);

        }


        Utils.matToTexture2D(imgMat, ImageTexture);
        PostProcessPanel.SetActive(true);
        yield return null;
    }


}
