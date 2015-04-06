using UnityEngine;
using System.Collections;

/********************************
 * Author   :    Cui'XueLong
 * Date     :    2015年4月6日0:08:23
 * Version  :    V 0.1.0
 * Describe :    登录模块视图代理
 * ******************************/

public class LoginViewProxy 
{

    private LoginController _controller;

    private LoginWin _loginInterface;

    
    public LoginViewProxy( LoginController module)
    {
        this._controller = module;
    }


    public LoginController Regist(LoginWin s)
    {
        _loginInterface = s;
        return _controller;
    }


}
