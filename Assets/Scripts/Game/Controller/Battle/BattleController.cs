using UnityEngine;
using System.Collections;

/********************************
 * Author   :    Cui'XueLong
 * Date     :    2015年4月8日0:29:38
 * Version  :    V 0.1.0
 * Describe :    战斗控制
 * ******************************/

public class BattleController : ControllerBase 
{

    /// <summary>
    /// 战斗视图代理
    /// </summary>
    
    public BattleViewProxy ViewProxy { get; private set; }


    protected override void Init()
    {
        ViewProxy = new BattleViewProxy( this);
    }


    protected override void RegistMsg()
    {

    }

    public override void Destory()
    {

    }
}
