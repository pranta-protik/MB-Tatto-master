using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ScreenshotHandler : MonoBehaviour
{
    private static ScreenshotHandler _instance;

    private new Camera _camera;
    private bool _takeScreenshotOnNextFrame;

    private void Awake()
    {
        _instance = this;
        _camera = Camera.main;
    }

    private void OnPostRender()
    {
        if (_takeScreenshotOnNextFrame)
        {
            _takeScreenshotOnNextFrame = false;
            RenderTexture renderTexture = _camera.targetTexture;

            Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            renderResult.ReadPixels(rect, 0, 0);
            renderResult.Apply();

            try
            {
                if (!Directory.Exists($"{Application.persistentDataPath}/Snapshots"))
                {
                    Directory.CreateDirectory($"{Application.persistentDataPath}/Snapshots");
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Cannot create Directory");
            }
            
            byte[] byteArray = renderResult.EncodeToPNG();
            string snapshotName =
                $"{Application.persistentDataPath}/Snapshots/Snapshot_{renderTexture.width}x{renderTexture.height}_{System.DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png";

            new System.Threading.Thread(() =>
            {
                System.Threading.Thread.Sleep(100);
                File.WriteAllBytes(snapshotName, byteArray);
            }).Start();

            // byte[] savedSnapshot = null;
            
            // new System.Threading.Thread(() =>
            // {
            //     System.Threading.Thread.Sleep(100);
            //     savedSnapshot = File.ReadAllBytes(snapshotName);
            // }).Start();
            
            // Texture2D loadedTexture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            // loadedTexture.LoadImage(savedSnapshot);
            
            UiManager.Instance.instaPostPage.transform.GetChild(1).GetChild(0).GetComponent<RawImage>().texture = renderResult;
            
            RenderTexture.ReleaseTemporary(renderTexture);
            _camera.targetTexture = null;
        }
    }

    private void TakeScreenshot(int width, int height)
    {
        _camera.targetTexture = RenderTexture.GetTemporary(width, height, 16);
        _takeScreenshotOnNextFrame = true;
    }

    public static void TakeScreenshot_Static(int width, int height)
    {
        _instance.TakeScreenshot(width, height);
    }
}
