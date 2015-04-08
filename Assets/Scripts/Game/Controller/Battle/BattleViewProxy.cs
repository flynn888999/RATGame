using UnityEngine;
using System.Collections;

public class BattleViewProxy  
{

    /// <summary>
    /// 战斗控制
    /// </summary>

    BattleController _controller;

    /// <summary>
    /// battle 视图代理
    /// </summary>

    private BattleWin _BattleInterface;


    public BattleViewProxy( BattleController controller)
    {
        _controller = controller;
    }


    public BattleController Regist(BattleWin win)
    {
        _BattleInterface = win;
        return _controller;
    }


    public void SetController( PlayerEntity playerEntity)
    {
        playerEntity.OnController(true);
    }

}
