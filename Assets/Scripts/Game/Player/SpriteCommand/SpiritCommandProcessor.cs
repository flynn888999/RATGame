using UnityEngine;
using System.Collections;

public class SpiritCommandProcessor 
{

    private SpiritObjBehaviour owner;

    public SpiritCommandBase currentCommand { get; private set; }


    public SpiritCommandProcessor(SpiritObjBehaviour fighter)
    {
        owner = fighter;
    }


    public void Execute(SpiritCommandBase cmd)
    {
        cmd.Execute();
        currentCommand = cmd;
    }


}
