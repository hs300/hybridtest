using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;

public class LoadDll : MonoBehaviour
{
    // Start is called before the first frame update
    private byte[] hotAss;
    void Start()
    {
        Assembly hotUpdateAss = null;
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
           
            
            hotUpdateAss = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "HotUpdate");
            Type type = hotUpdateAss.GetType("Hello");
            type.GetMethod("Run").Invoke(null, null);
        }
        else
        {
            // Editor环境下，HotUpdate.dll.bytes已经被自动加载，不需要加载，重复加载反而会出问题。
            var bs = StartCoroutine(LoadData());
           
        }
       
      
    }

    IEnumerator LoadData()
    {
        var uri = new System.Uri(Path.Combine(Application.streamingAssetsPath, "HotUpdate.dll.bytes"));
        Debug.Log("url " + uri.ToString());
        var req = UnityWebRequest.Get(uri);
        yield return req.SendWebRequest();
       
        hotAss = req.downloadHandler.data;
        Debug.Log(hotAss.Length);
        
        Assembly hotUpdateAss = Assembly.Load(hotAss);
        Type type = hotUpdateAss.GetType("Hello");
        type.GetMethod("Run").Invoke(null, null);
        
        
        Type testMono = hotUpdateAss.GetType("TestMono");
        gameObject.AddComponent(testMono);
    }
}
