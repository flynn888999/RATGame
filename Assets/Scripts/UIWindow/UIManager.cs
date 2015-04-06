using UnityEngine;
using System.Collections;
using System;

public sealed class UIManager : MonoBehaviour 
{

    public GameObject WidgetParent;

    public static UIManager Instance { get; private set; }


    void Awake()
    {
        Instance = this;
    }

    
    public void OnSceneChange()
    {

    }


    public static T ShowWidget<T> () where T : UIBase
    {
        string path;

        if ( WindowConfig.WindowBind.TryGetValue( typeof(T), out path))
        {
            GameObject obj = AssetManager.LoadResources<GameObject>(path);
            GameObject child = NGUITools.AddChild(Instance.WidgetParent, obj);
            return child.AddComponent<T>();
        }

        throw new System.Exception(string.Format("[{0}] 没有绑定在 [WindowBind] 中!", typeof(T).Name));
    }


    public void ShowDialog( string path)
    {

    }

}
