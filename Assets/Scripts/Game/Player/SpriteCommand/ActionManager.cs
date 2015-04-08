using UnityEngine;
using System.Collections;

/********************************
 * Author   :    Cui'XueLong
 * Date     :    2015年4月7日23:44:17
 * Version  :    V 0.1.0
 * Describe :    动作管理
 * ******************************/


public enum ActionType
{
    ACTION_IDLE = 0,
    ACTION_RUN = 1,
}


public class ActionManager : MonoBehaviour 
{

    public Animator activityAnmator { get; private set; }

    public ActionType currentType { get; private set; }


    void Awake()
    {
        activityAnmator = GetComponent<Animator>();
    }
	

    public void DoAction(ActionType type)
    {
        SetActionParameter(type);
    }


    private void SetActionParameter( ActionType type)
    {
        activityAnmator.SetInteger( "State", (int)type);
    }

}
