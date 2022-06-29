using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ScreenshotHandler : MonoBehaviour
{
    [SerializeField] private int _snapshotWidth;
    [SerializeField] private int _snapshotHeight;
    
    private static ScreenshotHandler _instance;
    private Camera _camera;
    private Texture2D _snapshotTexture;
    
    private void Awake()
    {
        _instance = this;
        _camera = Camera.main;
        _snapshotTexture = new Texture2D(_snapshotWidth, _snapshotHeight, TextureFormat.ARGB32, false);
    }

    private IEnumerator UpdateSnapshotTexture()
    {
        _camera.targetTexture = RenderTexture.GetTemporary(_snapshotWidth, _snapshotHeight, 16);
        
        yield return new WaitForEndOfFrame();
        
        RenderTexture renderTexture = _camera.targetTexture;
        RenderTexture.active = renderTexture;
        
        _snapshotTexture.ReadPixels(new Rect(0, 0, _snapshotTexture.width, _snapshotTexture.height), 0, 0);
        
        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(renderTexture);
        _camera.targetTexture = null;
        
        _snapshotTexture.Apply();

        try
        {
            if (!Directory.Exists($"{Application.persistentDataPath}/Snapshots"))
            {
                Directory.CreateDirectory($"{Application.persistentDataPath}/Snapshots");
            }
        }
        catch (IOException e)
        {
            Console.WriteLine("Cannot create directory");
        }

        PlayerPrefs.SetInt("SnapshotsTaken", PlayerPrefs.GetInt("SnapshotsTaken", 0) + 1);
        string snapshotNo = PlayerPrefs.GetInt("SnapshotsTaken", 0).ToString();

        byte[] byteArray = _snapshotTexture.EncodeToPNG();
        string snapshotName = $"{Application.persistentDataPath}/Snapshots/{snapshotNo}.png";

        new System.Threading.Thread(() =>
        {
            System.Threading.Thread.Sleep(100);
            File.WriteAllBytes(snapshotName, byteArray);
        }).Start();

        UiManager.Instance.instagramPostPage.transform.GetChild(1).GetChild(1).GetComponent<RawImage>().texture = _snapshotTexture;
    }

    private void TakeScreenshot()
    {
        StartCoroutine(UpdateSnapshotTexture());
    }

    public static void TakeScreenshot_Static()
    {
        _instance.TakeScreenshot();
    }
}
