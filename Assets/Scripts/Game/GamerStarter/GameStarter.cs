using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Communication.Network;

public class GameStarter : MonoBehaviour
{
    //加载资源前文字提示对象
    private GameObject preLoadLabel;

    public static GameStarter Instance;
    public static string filepath;
    public static bool isLogin = false;
    public static bool isCreate = false;
    public static bool loginSuccess = false;
    public static bool isUpdateSuccess = false;


    public static int num;
    


	void Awake ()
	{
        Instance = this;

		GameObject.DontDestroyOnLoad(this);

        //  注册所有模块控制
        RegistAllController();

        //  基础组件
        gameObject.AddComponent<Engine.FPS>();
        gameObject.AddComponent<GameFSM>();

        //  网络连接
        //GameSocketManager.Instance.ConnetServer("125.94.71.201", 10020);
        GameSocketManager.Instance.ConnetServer("127.0.0.1", 8080);

        //  显示登录界面
        UIManager.ShowWidget<LoginWin>();
	}



	void  Update()
	{
		DataPoolManger.Instance.DoUpdate ();   
	}


    /// <summary>
    /// 退出游戏函数
    /// </summary>
    void OnApplicationQuit()
    {
        GameSocketManager.Instance.Quit();
        ControllerManager.Destory();
    }


    /// <summary>
    /// 初始化控制器
    /// </summary>

    private void RegistAllController()
    {
        ControllerManager.Regist(new LoginController());
    }
}