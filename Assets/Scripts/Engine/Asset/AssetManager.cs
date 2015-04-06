using UnityEngine;
using System.Collections;

/********************************
 * Author   :    Cui'XueLong
 * Date     :    2015年4月6日11:08:42
 * Version  :    V 0.1.0
 * Describe :    资源管理
 * ******************************/

public class AssetManager 
{
    
    /// <summary>
    /// 加载资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>

    public static T LoadResources<T>( string path) where T : Object
    {
        return Resources.Load<T>( path);
    }


}
