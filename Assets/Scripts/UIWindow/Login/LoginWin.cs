using UnityEngine;
using System.Collections;

/********************************
 * Author   :    Cui'XueLong
 * Date     :    2015年4月6日11:20:17
 * Version  :    V 0.1.0
 * Describe :    登录窗口
 * ******************************/

public class LoginWin : UIBase 
{

    private LoginController _controller;

    private Transform mTrans;

    private UIInput input_Account;
    private UIInput input_Password;


    void Awake()
    {
        _controller = ControllerManager.Get<LoginController>().ViewProxy.Regist(this);
        mTrans = transform;
        Debug.Log(" LoginWidget Init !");
    }
    

    void Start()
    {
        input_Account = mTrans.Find("Contents/Input_Account").GetComponent<UIInput>();
        input_Password = mTrans.Find("Contents/Input_Password").GetComponent<UIInput>();

        UIEventListener.Get(mTrans.Find("Contents/Button_Login").gameObject).onClick = OnLoginClick;
    }


    private void OnLoginClick( GameObject obj)
    {
        if ( string.IsNullOrEmpty (input_Account.value))
        {
            Debug.LogError("账号不能为空!");
            return;
        }

        if ( string.IsNullOrEmpty (input_Password.value))
        {
            Debug.LogError("密码不能为空");
            return;
        }

        _controller.MsgProxy.RequestLogin(input_Account.value, input_Password.value);
    }

}
