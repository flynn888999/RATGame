using UnityEngine;
using System.Collections;

public class PlayerEntity : SpiritObjBehaviour , CxlRocker.IRocker
{

    public void OnController( bool isControl)
    {
        if (isControl)
            CxlRocker.RockerManager.rockerList[0].rockerEvent.Add(this);
        else
            CxlRocker.RockerManager.rockerList[0].rockerEvent.Remove(this);
    }

    
    public void OnRockerMove(CxlRocker.RockerData rd)
    {
        Vector3 dir = new Vector3( rd.normalOffset.x, 0f, rd.normalOffset.y).normalized;
        MoveCommand m = new MoveCommand(this, SpiritCommandType.CMD_MOVE, dir);

        CommandProcessor.Execute(m);
    }


    public void OnRockerStop(CxlRocker.RockerData rd)
    {
        Debug.Log("Move Stop!");

        IdleCommand i = new IdleCommand(this, SpiritCommandType.CMD_IDLE);
        CommandProcessor.Execute(i);
    }
}
