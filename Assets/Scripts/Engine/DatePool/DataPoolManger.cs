using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Communication;
using Communication.Model;
using Communication.Network;
using System.Text;
using System.Net;

/********************************
 * Author		：	djl
 * Date			：	2014/7/1
 * Version		：	V 0.1.0	
 * 消息中心
 *******************************/
public class DataPoolManger
{

    private DataPoolManger() { }

    public static readonly DataPoolManger Instance = new DataPoolManger();

    /// <summary>
    /// 唯一的阻塞的消息池
    /// </summary>
    public static Queue<OrderData> MessageDataPool = new Queue<OrderData>();

    private readonly System.Object lockThis = new System.Object();

    /// <summary>
    /// 命令号与方法的对应
    /// </summary>
    public Dictionary<int, Action<Dictionary<string, object>>> CmdMap = new Dictionary<int, Action<Dictionary<string, object>>>();

    /// <summary>
    ///  模块号 : (命令号：对应解析)
    /// </summary>
    public Dictionary<int, Dictionary<int, CommandModel>> modelMap = new Dictionary<int, Dictionary<int, CommandModel>>();


    /// <summary>
    /// 每次从消息池里取出一个消息
    /// </summary>
    public void DoUpdate()
    {
        while (MessageDataPool.Count > 0)
        {
            Debug.Log("有消息" + MessageDataPool.Count);
            OrderData data = new OrderData();
            lock (lockThis)
            {
                data = MessageDataPool.Dequeue();
                UpdateView(data);
            }
        }
    }


    /// <summary>
    /// 通过命令号接收数据，直接执行方法
    /// </summary>
    /// <param name="type"></param>
    public void UpdateView(OrderData obj)
    {

        if (!modelMap.ContainsKey(obj.Module))
        {
            Debug.LogError("have no this Module,Module is " + obj.Module);
            return;
        }

        //拆包
        Debug.Log("Receive Server Message ==> module "+obj.Module+"  cmd "+obj.Cmd);

        Dictionary<string, object> mdic = DataTransition(obj.Module, new ByteBuffer(obj.Data), obj.Cmd);

        //错误码显示
        //short status = (short) mdic["s"];

        if (CmdMap.ContainsKey(obj.Cmd + obj.Module * 10000))
        {
            Debug.Log("Receive  module " + obj.Module + "  cmd " + obj.Cmd );
            Action<Dictionary<string, object>> fun = CmdMap[obj.Cmd + obj.Module * 10000];
            fun(mdic);
        }

        else
        {
            Debug.LogError("Not Found!!!   module:" + obj.Module + "  cmd:" + obj.Cmd + "  status:" + mdic["s"]);
        }
       // if (status != 0)
        {
           // MessageManger.tipWin.ShowSingleButtonWin("错误码提示", ServerError.ErrorList[status]);
        }
    }



    public void SendMessage(int module, int cmd, object[] objs = null)
    {
        Debug.Log("Send Message To Server  ======> module " + module + "  cmd " + cmd);
        if (objs == null)
        {
            SendMessageTo((short)module, (short)cmd);

        }
        else
        {
            SendMessageTo((short)module, (short)cmd, getBuffer((short)module, (short)cmd, objs));
        }
    }

    private ByteBuffer getBuffer(short module, short cmd, object[] objs)
    {
        if (!modelMap.ContainsKey(module))
        {
            Debug.LogError("module is " + module);
        }
        Dictionary<int, CommandModel> modelList = modelMap[module];

        //TODO：此处不应该是固定的2048，待优化//
        ByteBuffer rBuffer = new ByteBuffer(new byte[2048]);
        modelList[(short)cmd].writeBuffer(rBuffer, objs);

        return rBuffer;
    }


    /// <summary>
    /// 转为大端编码，然后发送
    /// 通过命令号发送数据，直接执行方法
    /// </summary>
    private void SendMessageTo(short module, short cmd, ByteBuffer buffer = null)
    {
        int size = (buffer == null) ? 0 : buffer.GetWriteIndex();
        byte[] head = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(size + 4));

        //Debug.LogError("head size ==------------------------------------------" + head.Length);

        //Debug.LogError("size ==------------------------------------------" + size);

        byte[] newByte = new byte[size + 8];
        byte[] moduleByte = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(module));
        byte[] cmdByte = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(cmd));

       // Debug.LogError("moduleByte.Length ==------------------------------------------" + moduleByte.Length);
       // Debug.LogError("cmdByte.Length ==------------------------------------------" + cmdByte.Length);

        Array.Copy(head, 0, newByte, 0, 4);
        Array.Copy(moduleByte, 0, newByte, 4, 2);
        Array.Copy(cmdByte, 0, newByte, 6, 2);
        Array.Copy(buffer.buf, 0, newByte, 8, size);

        //向服务端异步发送这个字节数组//  
        GameSocketManager.Instance.sendToServer(newByte);
    }


    /// <summary>
    /// 添加阻塞的消息包，由通信线程调用
    /// </summary>
    public void AddMessage(OrderData BufferBytes)
    {
        lock (lockThis)
        {
            OrderData newOr = BufferBytes;
            MessageDataPool.Enqueue(newOr);
        }
    }

    /// <summary>
    /// 添加GC命令号 对应 执行方法
    /// CG命令不注册
    /// </summary>
    /// <param name="cmd">命令号</param>
    /// <param name="type">模块号</param>
    /// <param name="action"></param>
    public void AddCmdMapInfo(int cmd, int module, Action<Dictionary<string, object>> action)
    {
        Instance.CmdMap.Add(cmd + module * 10000, action);
    }

    /// <summary>
    /// 添加 模块号 : (命令号+对应解析)
    /// </summary>
    public void AddModleInfo(int module, Dictionary<int, CommandModel> cmdInfo)
    {
        modelMap.Add(module, cmdInfo);
    }

    /// <summary>
    /// 解析数据
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public Dictionary<string, object> DataTransition(short module, ByteBuffer value, short cmd)
    {
        Dictionary<string, object> obj = new Dictionary<string, object>();
        if (value == null || value.GetCapacity() == 0)
        {
            return obj;
        }
       // short status = value.ReadShort();
        //Debug.Log("-------------------status is-----------" + status);

        Dictionary<string, object> list = null; 
        //if (status == 0)
        {
            Dictionary<int, CommandModel> modelList = modelMap[module];

            if(!modelList.ContainsKey((int)cmd))
            {
                return obj;
            }
            CommandModel typeArr = modelList[(int)cmd];
        
            if (typeArr != null)
            {
                list = typeArr.readBuffer(value);
            }

        }
        value = null;
        //obj.Add("s", status);
        obj.Add("data", list);
        return obj;
    }

}

