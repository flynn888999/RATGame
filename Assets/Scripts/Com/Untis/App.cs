using UnityEngine;
using System.Collections;
using System.Collections.Generic;




public class App 
{

    public readonly static App Instance = new App();


    AsyncOperation asyncOperation;

    public System.Action<string> OnEnterNextScene;

    public System.Action OnLeaveCurrentScene;


    public void OpenSence(string nextScene)
    {
        GameStarter.Instance.StartCoroutine(FadeScene(nextScene)); 
    }


    /// <summary>
    /// 根据不同地图来传送
    /// </summary>
    /// <param name="nextScene"></param>
    /// <returns></returns>
    IEnumerator FadeScene(string nextScene)
    {

        yield return new WaitForEndOfFrame();

        Application.LoadLevel(nextScene);

        yield return new WaitForEndOfFrame();

        NoticeEnterNextScene(nextScene);
        ////1.完成后开始加载下一个场景 
        //asyncOperation = Application.LoadLevelAsync(nextScene);

        //NoticeLeaveCurrentScene();

        ////2.这里设置为当下一个场景加载完毕后不会进行跳转(unity4.x新增的API) 
        //asyncOperation.allowSceneActivation = false; 

        ////3.定义循环等待异步操作完成 
        //while (!asyncOperation.isDone && asyncOperation.progress < 0.9f) 
        //{
        //    yield return null; 
        //}

        //asyncOperation.allowSceneActivation = true;
    } 


    private void NoticeLeaveCurrentScene()
    {
        if (OnLeaveCurrentScene != null)
            OnLeaveCurrentScene();
    }


    private void NoticeEnterNextScene( string scene)
    {
        //yield return new WaitForEndOfFrame();
        //new GameObject("asdf");
        if (OnEnterNextScene != null)
            OnEnterNextScene(scene);
    }

    //public void setGo()
    //{
    //    //4.等待服务端命令，设置allowSceneActivation为true，开始跳转 
    //    Debug.Log("loading Complete!"); 

    //    asyncOperation.allowSceneActivation = true; 
    //}
} 
