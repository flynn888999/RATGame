using UnityEngine;
using System.Collections;

/********************************
 * Author   :    Cui'XueLong
 * Date     :    2015年4月3日0:21:55
 * Version  :    V 0.1.0
 * Describe :    战斗开始
 * ******************************/


namespace FSM
{
    public class FighterEnterState : GameState
    {
        public override void Enter()
        {
            App.Instance.OnEnterNextScene += OnEnterBattle;
            App.Instance.OpenSence("Battle");
        }

        public override void Leave()
        {
            App.Instance.OnEnterNextScene -= OnEnterBattle;
        }

        public override void Update()
        {

        }


        private void OnEnterBattle( string sceneName)
        {
            GameObject scene = GameObject.Instantiate(AssetManager.LoadResources<GameObject>("SceneModel/Scene_1"));
            GameObject player = GameObject.Instantiate(AssetManager.LoadResources<GameObject>("PersonModel/Model_Ez"));
            player.transform.parent = GameObject.Find("Origin").transform;
            player.transform.localPosition = Vector2.zero;

            Debug.Log(scene.name);
        }
    }
}

