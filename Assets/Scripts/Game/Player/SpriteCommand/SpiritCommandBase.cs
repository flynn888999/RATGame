using UnityEngine;
using System.Collections;


public enum SpiritCommandType
{
    CMD_DIE = 0,    //  死亡
    CMD_MOVE,       //  移动
    CMD_IDLE,       //  空闲
}


public abstract class SpiritCommandBase 
{

    public SpiritCommandType cmdType { get; private set; }

    public SpiritObjBehaviour cmdReceiver { get; private set; }


    public SpiritCommandBase(SpiritObjBehaviour obj, SpiritCommandType type)
    {
        cmdReceiver = obj;
        cmdType = type;
    }


    public abstract void Execute();
}



//  空闲指令

public class IdleCommand : SpiritCommandBase
{
    public IdleCommand(SpiritObjBehaviour obj, SpiritCommandType type)
        : base( obj, type)
    {
    }

    public override void Execute()
    {
        cmdReceiver.MoveMng.StopMove();
        cmdReceiver.ActionMng.DoAction(ActionType.ACTION_IDLE);
    }
}


//  移动指令

public class MoveCommand : SpiritCommandBase
{

    Vector3 dir;

    public MoveCommand(SpiritObjBehaviour obj, SpiritCommandType type, Vector3 dir)
        : base( obj, type)
    {
        this.dir = dir;
    }

    public override void Execute()
    {
        cmdReceiver.MoveMng.Dir(dir);
        cmdReceiver.ActionMng.DoAction(ActionType.ACTION_RUN);
    }
}