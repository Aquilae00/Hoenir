using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class STT : MonoBehaviour
{
    Button button;
    string objectName;
    GameObject dialog;
    AndroidJavaObject context;
    AndroidJavaClass pluginClass;

    // Start is called before the first frame update
    void Start()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
            dialog = new GameObject();
        }
        objectName = "Sup";
        button = GameObject.Find("Button").GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);

    }

    void TaskOnClick()
    {
        pluginClass = new AndroidJavaClass("com.example.sttunityplugin.Plugin");

        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        //Set Context
        pluginClass.CallStatic("setContext", context);

        //Set objectName for text
        pluginClass.CallStatic("setObjectName", objectName);

        //Start Listening
        context.Call("runOnUiThread", new AndroidJavaRunnable(listen));
        
    }

    void listen()
    {
        pluginClass.CallStatic("startListening");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
