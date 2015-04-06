using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ControllerManager  
{

    private static Dictionary<string, ControllerBase> moduleMap = new Dictionary<string, ControllerBase>();


    public static void Regist( ControllerBase module)
    {
        string name = module.ToString();
        moduleMap[name] = module;
    }


    //public static void Start()
    //{
    //    foreach (var element in moduleMap.Values)
    //    {
    //        element.Start();
    //    }
    //}


    public static void Destory()
    {
        foreach (var element in moduleMap.Values)
        {
            element.Destory();
        }
    }


    public static T Get<T> () where T : ControllerBase
    {
        return moduleMap[typeof(T).Name] as T;
    }
	
}
