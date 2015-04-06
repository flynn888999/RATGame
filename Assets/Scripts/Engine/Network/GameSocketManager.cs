
using System;
using UnityEngine;

/********************************
 * Author		：	djl
 * Date			：	2014/7/1
 * Version		：	V 0.1.0	
 * 通信管理
 *******************************/
namespace Communication.Network
{

    public class GameSocketManager
    {
        private GameSocketManager(){}

        public static readonly GameSocketManager Instance = new GameSocketManager();
        protected GameSocket socket;
        public bool IsConnnected
        {
            get
            {
                return socket.IsConnected();
            }
        }


        public void Quit()
        {
            if (socket != null)
                socket.Quit();
        }

        public void ConnetServer(string ip, int port)
        {

            //如果是 开启登录服线程，则直接开; 如果开启游戏服线程，则要先清除登录服线程，重用socket
            Quit();

            socket = new GameSocket();
            socket.Socket_Connect(ip, port, CallBack_Connect, CallBack_Receive);
        }


        private void CallBack_Connect(bool success, GameSocket.SocketStatus status, string exception)
        {
            Debug.Log("GameSocketManager " + IsConnnected.ToString() + socket.AddressIP + socket.Port + ((bool)(socket == null)));

        }

        private void CallBack_Receive(bool success, GameSocket.SocketStatus status, string exception, OrderData byteMessage, string strMessage)
        {
            if (GameSocket.SocketStatus.Success == status)
            {
                Debug.Log("===============================  receive data");
                DataPoolManger.Instance.AddMessage(byteMessage);

            }
        }

        internal void sendToServer(byte[] bytes)
        {
             socket.SynSendMessage(bytes, callBackSend);
        }

        void callBackSend(bool success, GameSocket.SocketStatus status, string exception)
        {
            Debug.Log("Send to server successful");
        }

    }
}
