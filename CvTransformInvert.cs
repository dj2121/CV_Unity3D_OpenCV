using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using OpenCVForUnity;

public class CvTransformInvert : MonoBehaviour {

    public Texture2D ImageTexture;
    public GameObject ImagePanel;
    public int rotation;

    public void doTransInvert()
    {
        Mat imgMat = new Mat(ImageTexture.height, ImageTexture.width, CvType.CV_8UC4);
        Utils.texture2DToMat(ImageTexture, imgMat);
        Point src_center = new Point(imgMat.cols() / 2.0, imgMat.rows() / 2.0);
        Mat rot_mat = Imgproc.getRotationMatrix2D(src_center, rotation, 1.0);

        
        Mat dst = new Mat(imgMat.size(), imgMat.type());
        
        Imgproc.warpAffine(imgMat, dst, rot_mat, dst.size(), Imgproc.INTER_LINEAR);
        Utils.matToTexture2D(dst, ImageTexture);
    }
}
