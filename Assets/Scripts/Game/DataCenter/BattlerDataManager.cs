using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/********************************
 * Author   :    Cui'XueLong
 * Date     :    2015年4月8日0:36:53
 * Version  :    V 0.1.0
 * Describe :    战斗数据中心
 * ******************************/

public class BattlerDataManager 
{

    private BattlerDataManager() 
    {
        AllSpirit = new Dictionary<int, SpiritObjBehaviour>();
    }


    private static BattlerDataManager _instance;

    public static BattlerDataManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new BattlerDataManager();
            return _instance;
        }
    }


    public Dictionary<int, SpiritObjBehaviour> AllSpirit { get; private set; }
   
    
    public void AddSpirit( SpiritObjBehaviour spiritObj)
    {
        AllSpirit.Add(spiritObj.InstanceID, spiritObj);
    }

   
    public void Clean()
    {
        AllSpirit.Clear();
    }
}
