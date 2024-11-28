using System.Collections;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SFB;


public class SaveImage : MonoBehaviour {

    public Texture2D FinalImage;
    public string Title = "";
    public string Directory = "";
    public string FileName = "";
    public string Extension = "";

    private byte[] _textureBytes;

	public void doSaveImage()
    {
        _textureBytes = FinalImage.EncodeToPNG();
        var path = StandaloneFileBrowser.SaveFilePanel(Title, Directory, FileName, Extension);
        if (!string.IsNullOrEmpty(path))
        {
            File.WriteAllBytes(path, _textureBytes);
        }
    }
}
