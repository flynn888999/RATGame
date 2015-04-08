using UnityEngine;
using System.Collections;

public class BattleWin : UIBase 
{

    private BattleController _controller;
	

    void Awake()
    {
        _controller = ControllerManager.Get<BattleController>().ViewProxy.Regist(this); 
    }

}
