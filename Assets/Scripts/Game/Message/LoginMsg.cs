using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Communication.Network;

public class LoginMsg
{
    public LoginMsg() { }


    private LoginController controller;

    public LoginController Controller
    {
        get 
        {
            if (controller == null)
                controller = ControllerManager.Get<LoginController>();
            return controller;
        }
    }


    public void Init()
    {
        //注册模块
        DataPoolManger.Instance.AddModleInfo(ControllerModel.LGMODEL, LoginProxy.DATA_MODE());
        DataPoolManger.Instance.AddModleInfo(ControllerModel.PLAYERMODEL, PlayerProxy.DATA_MODE());

        //注册命令（CG命令不注册）
        DataPoolManger.Instance.CmdMap.Add(LoginProxy.GC_LoginBack + ControllerModel.LGMODEL * 10000, OnReplayLogin);
    }



    /// <summary>
    /// 发送登陆命令
    /// </summary>
    /// <param name="count"></param>
    /// <param name="password"></param>

    public void RequestLogin(string count, string password)
    {
        Debug.Log("发起登录 的请求命令");

        object[] objs = { count, password };
        DataPoolManger.Instance.SendMessage(ControllerModel.LGMODEL, LoginProxy.CG_Login, objs);
    }



    /// <summary>
    /// 收到登录,跳转到下一个场景
    /// </summary>
    /// <param name="obj"></param>

    private void OnReplayLogin(Dictionary<string, object> obj) 
    {
        //  将玩家数据存储进数据中心
        Dictionary<string, object> newObj = (Dictionary<string, object>)obj["data"];
        Debug.Log("收到登录信息" + (int)newObj["result"]);

        //  跳转直接进入战斗场景
        GameFSM.Instance.ChangeState(FSM.GameStateType.FighterEnterState);
    }




    //发送创建角色命令
    public void OnCreateRole(string name)
    {
        Debug.Log("发起创建角色命令 的请求命令");
        //国家 头像名称 性别
        object[] objs = { name, (byte)1, (byte)1, (byte)1 };
        DataPoolManger.Instance.SendMessage(ControllerModel.PLAYERMODEL,  PlayerProxy.CG_CREATE, objs);
    }
}
