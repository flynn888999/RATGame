using UnityEngine;
using System.Collections;

public class MovePositionManager : MonoBehaviour 
{

    SpiritObjBehaviour owner;

    private Vector3 selfDir;
    private Quaternion toLookAt;
    private Vector3 toPosition;

    private float minMoveRange = 0.1f;

    void Awake()
    {
        owner = GetComponent<SpiritObjBehaviour>();
        StopExecute();
    }


    void Update()
    {
        owner.mTrans.localRotation = Quaternion.Slerp(owner.mTrans.localRotation, toLookAt, Time.deltaTime * 10f);
        owner.mTrans.position += GetNormalMoveRange() * owner.mTrans.forward;

        //TODO
        //Debug.DrawLine(owner.mTrans.position, owner.mTrans.position + selfDir, Color.green);
        //Debug.DrawLine(owner.mTrans.position, toPosition, Color.red);
    }


    public void Dir(Vector3 dir)
    {
        StartExecute();
        SetSelfDir();
        toLookAt = Quaternion.LookRotation(dir);
        MoveTo(owner.mTrans.position + selfDir * minMoveRange);
    }


    public void MoveTo( Vector3 position)
    {
        toPosition = position;
        //Debug.Log("To Position " + toPosition.ToString());
    }


    public void StopMove()
    {
        toPosition = owner.mTrans.position;
        StopExecute();
    }


    private void OnMoveFinish()
    {
        StopExecute();
    }


    private void StartExecute()
    {
        enabled = true;
    }

    private void StopExecute()
    {
        enabled = false;
    }

    private void SetSelfDir()
    {
        selfDir.x = owner.mTrans.position.x;
        selfDir.y = 0f;
        selfDir.z = owner.mTrans.position.z;
        selfDir.Normalize();
    }

    private float GetNormalMoveRange()
    {
        return owner.ObjData.Speed * Time.deltaTime;
    }

    private bool CheckDis( Vector3 f, Vector3 t, float dis)
    {
        Vector3 temp = f - t;
        temp.y = 0f;
        return temp.sqrMagnitude <= dis * dis;
    }
}
