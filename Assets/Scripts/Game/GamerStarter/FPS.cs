using UnityEngine;

namespace Engine
{
    ///<summary>
    ///FPS类
    ///</summary>
    public class FPS : MonoBehaviour
    {

        private float updateInterval = 1.0f;
        private float lastInterval;
        private int frames = 0;
        private float fps;
        private float ms;


        //默认帧频设置函数
        public static void SetDefualtFrameRate()
        {
            Application.targetFrameRate = 45;
        }

        //死亡帧频设置函数
        public static void SetIdleFrameRate()
        {
            Application.targetFrameRate = 1;
        }



        void Start()
        {
            SetDefualtFrameRate();

            lastInterval = Time.realtimeSinceStartup;
            frames = 0;
            GameObject.DontDestroyOnLoad(this);
        }

        void Update()
        {
            ++frames;
            var timeNow = Time.realtimeSinceStartup;
            if (timeNow > lastInterval + updateInterval)
            {
                fps = frames / (timeNow - lastInterval);
                ms = 1000.0f / Mathf.Max(fps, 0.00001f);
                frames = 0;
                lastInterval = timeNow;
            }
        }

        void OnGUI()
        {
            if (fps < 15)
            {
                GUI.color = Color.red;
            }
            else if (fps < 25)
            {
                GUI.color = Color.yellow;
            }
            else
            {
                GUI.color = Color.white;
            }
            GUI.Label(new Rect(200, 0, 200, 20), ms.ToString("f1") + "ms " + fps.ToString("f2") + "FPS");
            GUI.color = Color.white;
        }
    }
}