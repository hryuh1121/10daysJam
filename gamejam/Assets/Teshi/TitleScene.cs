using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public class TitleScene : MonoBehaviour
{
    public string txt;
#if UNITY_STANDALONE_WIN
    [DllImport("user32.dll", EntryPoint="FindWindow", CharSet=CharSet.Unicode)]
    static extern System.IntPtr FindWindow(string className, string windowName);
    [DllImport("user32.dll", EntryPoint="SetWindowText", CharSet=CharSet.Unicode)]
    static extern bool SetWindowText(System.IntPtr hwnd, string txt);
    void Start() {
      System.IntPtr hwnd = FindWindow(null, Application.productName);
      SetWindowText(hwnd, txt);
    }
#endif


    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }   

    void ChangeScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}