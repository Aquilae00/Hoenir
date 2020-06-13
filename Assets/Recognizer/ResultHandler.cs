using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultHandler : MonoBehaviour
{
    Text txt;
    AndroidJavaObject context2;
    AndroidJavaClass pluginClass2;

    // Start is called before the first frame update
    void Start()
    {
        pluginClass2 = new AndroidJavaClass("com.example.ttsunityplugin.TTSPlugin");

        AndroidJavaClass unityPlayer2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        context2 = unityPlayer2.GetStatic<AndroidJavaObject>("currentActivity");
        txt = GameObject.Find("Sup").GetComponent<Text>();

    }

    void onActivityResult(string result)
    {


        txt.text = result;

        pluginClass2.CallStatic("setContext", context2);
        pluginClass2.CallStatic("sayMessage", result);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
