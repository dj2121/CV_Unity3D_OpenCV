using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using OpenCVForUnity;

public class CvSkewProcess : MonoBehaviour {

    public Texture2D ImageTexture;
    public GameObject ImagePanel;


    public void doSkewProcess()
    {
        StartCoroutine(CvSkewProcessing());
    }


    IEnumerator CvSkewProcessing()
    {
        Mat imgMat = new Mat(ImageTexture.height, ImageTexture.width, CvType.CV_8UC4);
        Utils.texture2DToMat(ImageTexture, imgMat);

        Mat imgGray = new Mat(ImageTexture.height, ImageTexture.width, CvType.CV_8UC1);
        Imgproc.cvtColor(imgMat, imgGray, Imgproc.COLOR_BGR2GRAY);

        double angle = 0.0;
        angle = compute_skew1(imgGray);

        angle = angle * 180 / Mathf.PI;
        imgMat = deskew(imgMat, angle);

       Utils.matToTexture2D(imgMat, ImageTexture);

        yield return null;
    }

    Mat deskew(Mat src, double angle)
    {
        Point center = new Point(src.width() / 2, src.height() / 2);
        Mat rotImage = Imgproc.getRotationMatrix2D(center, angle, 1.0);
        Size size = new Size(src.width(), src.height());
        Imgproc.warpAffine(src, src, rotImage, size, Imgproc.INTER_LINEAR + Imgproc.CV_WARP_FILL_OUTLIERS);
        return src;
    }


    public double compute_skew1(Mat src){

    Size size = src.size();
    double minLineSize = src.width() / 2.0;
    double maxLineGap = src.height() / 5;

    Core.bitwise_not(src, src);

    Mat lines = new Mat();
    double angle = 0.0;
        Imgproc.HoughLinesP(src, lines, 1, Mathf.PI / 180, 100, minLineSize, maxLineGap);

        Mat disp_lines = new Mat(size, CvType.CV_8UC1, new Scalar(0, 0, 0));
        int nb_lines = lines.cols();
        for (int i = 0; i < nb_lines; i++) {
            double[] vec = lines.get(0, i);
            double x1 = vec[0], 
                   y1 = vec[1],
                   x2 = vec[2],
                   y2 = vec[3];
            Point start = new Point(x1, y1);
            Point end = new Point(x2, y2);
            Core.line(disp_lines, start, end, new Scalar(255,0,0));
            angle += Mathf.Atan2((float)(y2 - y1), (float)(x2 - x1));
        }
    angle /= nb_lines;
    return angle;
    }


}
