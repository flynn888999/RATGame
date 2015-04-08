using UnityEngine;
using System.Collections;

/********************************
 * Author   :    Cui'XueLong
 * Date     :    2015年4月7日23:37:44
 * Version  :    V 0.1.0
 * Describe :    对象行为
 * ******************************/


public class SpiritObjBehaviour : MonoBehaviour 
{

    public int InstanceID { get; private set; }

    public ActionManager ActionMng { get; protected set; }

    public MovePositionManager MoveMng { get; protected set; }

    public SpiritCommandProcessor CommandProcessor { get; protected set; }

    public SpiritData ObjData { get; protected set; }

    public Transform mTrans { get; private set; }



	void Awake()
    {
        mTrans = transform;
    }


    void Start()
    {
        GameObject model = mTrans.FindChild("Model").gameObject;
        ActionMng = model.AddComponent<ActionManager>();
        CommandProcessor = new SpiritCommandProcessor(this);
        MoveMng = gameObject.AddComponent<MovePositionManager>();
        ObjData = new SpiritData();
    }

}

