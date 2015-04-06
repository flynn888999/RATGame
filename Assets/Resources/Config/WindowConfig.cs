using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/********************************
 * Author   :    Cui'XueLong
 * Date     :    2015年4月6日10:53:53
 * Version  :    V 0.1.0
 * Describe :    映射UI预设路径,预设脚本名与预设同名
 * ******************************/


public sealed class WindowConfig 
{

    public static Dictionary<System.Type, string> WindowBind = new Dictionary<System.Type, string>
    {
        //  脚本 (绑定)=> 预制
        {typeof(LoginWin), "Window/Login/LoginWidget"},
    };


}
