using UnityEngine;
using System.Collections;

public abstract class ControllerBase 
{

    public ControllerBase()
    {
        Init();
        
        RegistMsg();
    }

    protected abstract void Init();

    //public abstract void Start();

    protected abstract void RegistMsg();


    public abstract void Destory();

}
