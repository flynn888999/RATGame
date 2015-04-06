using System;
using UnityEngine;

namespace Communication
{
    public class OrderData
    {
        /// <summary>
        /// 模块
        /// </summary>
        public short Module;

        /// <summary>
        /// 命令
        /// </summary>
        public short Cmd;

        private byte[] data;

        /// <summary>
        /// 设置/获取数据
        /// </summary>
        public byte[] Data
        {
            get { return data; }
            set
            {
                data = value;
            }
        }

        internal OrderData Clone()
        {
            OrderData newData = new OrderData();
            newData.Module = Module;
            newData.Cmd = Cmd;
            newData.data = data;
            return newData;
        }
    }
}
