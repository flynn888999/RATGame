using UnityEngine;
using System.Collections;

/********************************
 * Author   :    Cui'XueLong
 * Date     :    2015年4月3日0:11:09
 * Version  :    V 0.1.0
 * Describe :    游戏状态机
 * ******************************/

namespace FSM
{

    public enum GameStateType
    {
        GlobalState,
        GameStartState,
        FighterEnterState,
        FighterOverState,
        FightingState,
    }


    public abstract class GameState
    {

        public abstract void Enter();

        public abstract void Leave();

        public abstract void Update();

    }

}

