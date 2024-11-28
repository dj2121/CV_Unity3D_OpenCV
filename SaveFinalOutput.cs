using System.Collections;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SFB;

public class SaveFinalOutput : MonoBehaviour {

    public Text OutputText;
    public string Title = "";
    public string Directory = "";
    public string FileName = "";
    public string Extension = "";

    private byte[] _textBytes;

    public void doSaveResult()
    {
        _textBytes = System.Text.Encoding.UTF8.GetBytes(OutputText.text);
        var path = StandaloneFileBrowser.SaveFilePanel(Title, Directory, FileName, Extension);
        if (!string.IsNullOrEmpty(path))
        {
            File.WriteAllBytes(path, _textBytes);
        }
    }



}
