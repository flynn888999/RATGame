using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
#pragma warning disable 0414


namespace Communication.Network
{
    public class GameSocket
    {
        public enum SocketStatus
        {
            Success = 0,                          //成功
            Timeout = 1,                          //超时
            SocketNull = 2,                      //套接字为空
            SocketUnconnect = 3,                 //套接字未连接
            ConnectUnsuccessUnknow = 4,         //连接失败未知错误
            ConnectConnecedError = 5,           //重复连接错误
            SendUnsuccessUnknow = 6,            //发送失败未知错误
            ReceiveUnsuccessUnknow = 7,         //收消息未知错误
            DisconnectUnsuccessUnknow = 8,      //断开连接未知错误
        }

        public delegate void CallBack_Connect(bool success, SocketStatus status, string exception);
        public delegate void CallBack_Send(bool success, SocketStatus status, string exception);
        public delegate void CallBack_Receive(bool success, SocketStatus status, string exception, OrderData byte_Message, string str_Message);
        public delegate void CallBack_Disconnect(bool success, SocketStatus status, string exception);

        /// <summary>
        /// 连接回调
        /// </summary>
        public CallBack_Connect      callBack_Connect;
        /// <summary>
        /// 发送回调
        /// </summary>
        public CallBack_Send         callBack_Send;
        /// <summary>
        /// 获取消息回调
        /// </summary>
        public CallBack_Receive      callBack_Receive;
        /// <summary>
        /// 断开连接回调
        /// </summary>
        public CallBack_Disconnect   callBack_Disconnect;
        /// <summary>
        /// 
        /// </summary>
        private SocketStatus socketStatus = 0;

        private Socket clientSocket;
        private string addressIP;
        private int port;
        private int receiveDataLength;
        private byte[] receiveDataAll;
        private int curDataLength;    

        #region   建立socket 连接
        /// <summary>
        /// socket 连接
        /// </summary>
        public void Socket_Connect(string ip,int port,CallBack_Connect callbackConnect,CallBack_Receive receive)
        {
            socketStatus=SocketStatus.Success;
            this.callBack_Connect = callbackConnect;
            this.callBack_Receive = receive;

            if(clientSocket!=null&&clientSocket.Connected)
            {
                UnityEngine.Debug.Log("#GameSocket " + clientSocket.Connected + " " + ip + " " + port);
                //重复连接错误
                this.callBack_Connect(false, SocketStatus.ConnectConnecedError, "");
            }
            else if(clientSocket==null||!clientSocket.Connected)
            {

                this.addressIP = ip;
                this.port = port;
                IPAddress ipAddress = IPAddress.Parse(ip);
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);

                UnityEngine.Debug.Log("#GameSocket "  + ip + " " + port);

                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //与目标终端连接
                IAsyncResult asyncResult = clientSocket.BeginConnect(ipEndPoint,new AsyncCallback(connectCallback),clientSocket);


                WriteDot(asyncResult);
            }

        }
 
        /// <summary>
        /// 超时检测
        /// </summary>
        internal bool WriteDot(IAsyncResult asyncResult)
        {
            return asyncResult.AsyncWaitHandle.WaitOne(5000, true);
        }
        #endregion


        #region   连接回调
        /// <summary>
        /// 连接回调 TODO:这里的try其实只用包裹SynReceive，以后优化
        /// </summary>
        private void connectCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                client.EndConnect(ar);

                if(client.Connected==false)
                {
                   UnityEngine.Debug.Log("[连接失败]====================ConnectServer");
                   socketStatus = SocketStatus.ConnectUnsuccessUnknow;
                   return;
                }
                else
                {
                    UnityEngine.Debug.Log("[连接成功,开始接受消息]====================ConnectServer");

                    //回调函数
                    if (callBack_Connect != null)
                    {
                        UnityEngine.Debug.Log("[回调不为空，执行]====================ConnectServer");
                        callBack_Connect(client.Connected, socketStatus, "" + addressIP + "  " + port);
                    }

                    //连接成功,开始接受消息
                    SynReceive();
                }
 
            }catch(Exception e)
            {
                if(callBack_Connect!=null)
                {
                    UnityEngine.Debug.Log("[连接失败,关闭连接]====================ConnectServer" + e);
                    callBack_Connect(false, SocketStatus.ConnectUnsuccessUnknow, e.ToString());

                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                    clientSocket = null;
                }
            }
        }
        #endregion



        #region   接收数据
        private void SynReceive()
        {
            while (true)
            {
                if (clientSocket != null && clientSocket.Connected)
                {           
                        int receLen = clientSocket.Available;
                        if (receLen == 0) continue;

                        byte[] tBytes = new byte[receLen];
                        int length = clientSocket.Receive(tBytes, 0, receLen, SocketFlags.None);

                        //套接字取得数据
                        if (length == 0) continue;

                        receiveNewBytes(tBytes);
                }
                else
                {
                    UnityEngine.Debug.Log("========================= socket is shutdown...");
                    break;
                    
                }
            }
        }

        //byte缓冲数组，所有数据都要从socket缓冲区里拿出来，缓存在这里。
        private byte[] buffer=null;
        private bool hasHeads=false;

        #endregion


        #region   分包操作
        /// <summary>
        /// 接受到数据处理函数
        /// </summary>
        /// <param name="bytes"></param>
        private void receiveNewBytes(byte[] bytes)
        {
            byte[] tBytes;

            if (buffer == null)
            {
                tBytes = bytes;
            }
            else
            {
                tBytes = new byte[buffer.Length + bytes.Length];
                buffer.CopyTo(tBytes, 0);
                bytes.CopyTo(tBytes, buffer.Length);

            }

            UnityEngine.Debug.Log("[tBytes.Length====]：" + tBytes.Length);

            if (tBytes.Length > 4)
            {
                if (!hasHeads)
                {
                    byte[] heads = new byte[4];

                    Array.Copy(tBytes, 0, heads, 0, 4);

                    //从大端转为小端编码
                    Array.Reverse(heads);

                    //获取整个包长
                   curDataLength = System.BitConverter.ToInt16(heads, 0);

                   UnityEngine.Debug.Log("[curDataLength为====]：" + curDataLength);
       
                   receiveDataAll = new byte[curDataLength];
                   heads = null;
                }
                if (tBytes.Length >= curDataLength)
                {

                    int curLength = curDataLength;

                    Array.Copy(tBytes, 0, receiveDataAll, 0, curDataLength);

                    ReceiveMessageOver();
                    hasHeads = false;
                    buffer = null;

                    if (tBytes.Length == curDataLength)
                    {
                        return;
                    }

                    byte[] t_buffer = new byte[tBytes.Length - curLength];

                    Array.Copy(tBytes, curLength, t_buffer, 0, t_buffer.Length);
					receiveNewBytes(t_buffer);
				}
                else
                {
                    hasHeads = true;
                    buffer = tBytes;
                }
            }
            else
            {
                buffer = tBytes;
            }
            tBytes = null;
        }

        #region 完整截取到一个包的数据
        /// <summary>
        /// 完整截取到一个包的数据
        /// </summary>
        private void ReceiveMessageOver()
        {

            if(callBack_Receive!=null)
            {
                UnityEngine.Debug.Log("receiveDataAll==" + receiveDataAll.Length);


                for (int i = 0; i < receiveDataAll.Length; i++)
                {
                    UnityEngine.Debug.Log("receiveDataAll------------"+receiveDataAll[i]);
               }


                //使用新的byte类
                ByteBuffer mByteBuffer = new ByteBuffer(receiveDataAll);

                int mlen = mByteBuffer.ReadInt();
                UnityEngine.Debug.Log("mlen==" + mlen);

                OrderData orderData = new OrderData();
                orderData.Module = mByteBuffer.ReadShort();
                UnityEngine.Debug.Log("Module==" + orderData.Module);

                orderData.Cmd = mByteBuffer.ReadShort();
                UnityEngine.Debug.Log("Cmd==" + orderData.Cmd); 

                orderData.Data = mByteBuffer.cloneLeftData();
                mByteBuffer = null;

                callBack_Receive(true, socketStatus, "", orderData, System.Text.Encoding.Unicode.GetString(receiveDataAll));

            }
            receiveDataAll = null;
            receiveDataLength = 0;
            curDataLength = 0;
        }
        #endregion


 
        #endregion

        public void SynSendMessage(byte[] sendBuffer, CallBack_Send callbackSend)
        {

            this.callBack_Send = callbackSend;
            if (clientSocket == null)
            {
                UnityEngine.Debug.Log("SynSendMessage================================1");
                socketStatus = SocketStatus.SocketNull;
                callBack_Send(false, socketStatus, "");
            }
            else if (!clientSocket.Connected)
            {
                UnityEngine.Debug.Log("SynSendMessage================================2");
                socketStatus = SocketStatus.SocketUnconnect;
                callBack_Send(false, socketStatus, "");
            }
            else
            {
                UnityEngine.Debug.Log("SynSendMessage================================3");
                clientSocket.Send(sendBuffer, 0, sendBuffer.Length, SocketFlags.None);
                callBack_Send(true, socketStatus, "");
            }
        }

        public void Quit()
        {
            if(clientSocket!=null&&clientSocket.Connected)
            {
                callBack_Connect = null;
                callBack_Send = null;
                callBack_Receive = null;
                clientSocket.Disconnect(false);
            }
        }

        public string AddressIP
        { 
            get
            {
                return addressIP;
            }
        }

        public int Port
        {
            get
            {
                return port;
            }
        }

        public bool IsConnected()
        {
            if(clientSocket==null)
            {
                return false;
            }
            return clientSocket.Connected;
        }

    }
}
