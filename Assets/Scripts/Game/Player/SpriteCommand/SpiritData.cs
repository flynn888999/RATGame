using UnityEngine;
using System.Collections;

/********************************
 * Author   :    Cui'XueLong
 * Date     :    2015年4月8日23:49:29
 * Version  :    V 0.1.0
 * Describe :    
 * ******************************/

public class SpiritData 
{

    /// <summary>
    /// 基础移动速度
    /// </summary>

    public float baseSpeed { get; protected set; }


    /// <summary>
    /// 速度
    /// </summary>

    public float Speed { get; set; }


    public SpiritData()
    {
        baseSpeed = 2f;
        Speed = 2f;
    }

}
