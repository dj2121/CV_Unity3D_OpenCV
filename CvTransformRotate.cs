using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using OpenCVForUnity;

public class CvTransformRotate : MonoBehaviour {

    public Texture2D ImageTexture;
    public GameObject ImagePanel;
    public int rotation;

    public void doTransRotate()
    {
        Mat imgMat = new Mat(ImageTexture.height, ImageTexture.width, CvType.CV_8UC4);
        Utils.texture2DToMat(ImageTexture, imgMat);
        float ratio = 1.3f;
        float multiplierx = 2.85f / ratio;
        float multipliery = 2.0f / ratio;
        Point src_center = new Point(imgMat.width() / multiplierx, imgMat.height() / multipliery);

        Mat rot_mat = Imgproc.getRotationMatrix2D(src_center, rotation, 1.0);

       

        Size newSize = new Size(imgMat.width() * ratio, imgMat.height() * ratio); 
        Mat dst = new Mat();

        Imgproc.warpAffine(imgMat, dst, rot_mat, newSize, Imgproc.INTER_LINEAR);

        Imgproc.resize(dst, dst, imgMat.size());

        Utils.matToTexture2D(dst, ImageTexture);
    }
}
