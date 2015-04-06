using UnityEngine;
using System.Collections;

/********************************
 * Author   :    Cui'XueLong
 * Date     :    2015年4月6日0:14:00
 * Version  :    V 0.1.0
 * Describe :    登录控制
 * ******************************/

public class LoginController : ControllerBase 
{

    /// <summary>
    /// 消息代理
    /// </summary>
    public LoginMsg MsgProxy { get; private set; }

    /// <summary>
    /// 视图代理
    /// </summary>
    public LoginViewProxy ViewProxy { get; private set; }


    protected override void Init()
    {
        MsgProxy = new LoginMsg();
        ViewProxy = new LoginViewProxy(this);
    }


    protected override void RegistMsg()
    {
        MsgProxy.Init();
    }


    public override void Destory()
    {

    }

}
