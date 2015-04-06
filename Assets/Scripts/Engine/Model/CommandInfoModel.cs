using com.commons.network.modulecore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Communication.Model
{
    public class CommandInfoModel
    {
        private String type;

        private IResolveBuffer typeId;

        private int size;

        private String name;

        /**
         * 传递LIST的时候适用
         */
        private CommandInfoModel[] commandList;

        public CommandInfoModel(String name, String type, int size)
        {
            this.name = name;
            this.type = type;
            this.typeId = Resolve.BuffType.getType(type);
            this.size = size;
        }

        public CommandInfoModel(String name, String type, int size, CommandInfoModel[] commandList)
        {
            this.name = name;
            this.type = type;
            this.typeId = Resolve.BuffType.getType(type);
            this.size = size;
            this.commandList = commandList;
        }

        public String getType()
        {
            return type;
        }

        public void setType(String type)
        {
            this.type = type;
        }

        public int getSize()
        {
            return size;
        }

        public void setSize(int size)
        {
            this.size = size;
        }

        public String getName()
        {
            return name;
        }

        public void setName(String name)
        {
            this.name = name;
        }

        public IResolveBuffer TypeId
        {
            get { return typeId; }
            set { typeId=value; }
        }

        public void setTypeId(IResolveBuffer typeId)
        {
            this.typeId = typeId;
        }

        public CommandInfoModel[] getCommandList()
        {
            return commandList;
        }

        public void setCommandList(CommandInfoModel[] commandList)
        {
            this.commandList = commandList;
        }


        /// <summary>
        /// 解包
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        internal object readBuffer(ByteBuffer bytes)
        {
            object obj = null;

            if (commandList != null && commandList.Length > 0)
            {
                int length = commandList.Length;
                short listLen = bytes.ReadShort();

                Dictionary<string, object> list = new Dictionary<string, object>();
                for (int j = 0; j < listLen; j++)
                {

                    Dictionary<string, object> list1 = new Dictionary<string, object>();
                    for (int i = 0; i < length; i++)
                    {

                        list1.Add(commandList[i].name, commandList[i].readBuffer(bytes));
                    }

                    list.Add(j.ToString(), list1);
                }
                return list;
            }
            else
            {                
 			    obj = TypeId.read(bytes, this);
            }

            return obj;
        }

        
        /// <summary>
        /// 封包
        /// </summary>
        internal void writeBuffer(ByteBuffer bytes, object obj)
        {
            if (commandList != null && commandList.Length > 0)
            {
                int length = commandList.Length;
                object[] list = (object[])obj;
                short len = short.Parse(list.Length.ToString());
                bytes.WriteShort(len);
                object[] data = null;
                for (int j = 0; j < len; j++)
                {
					data = new object[]{list[j]};
                    for (int i = 0; i < length; i++)
                    {
                        commandList[i].writeBuffer(bytes, data[i]);
                    }
                }
            }

            //当循环封包结束后，投入此处真正的封包
            else
            {
                try
                {
                    Debug.Log("11111111111111111111111111111111111111111111");
                    typeId.write(bytes, obj, this);
                }
                catch
                {
                    Debug.Log("CommandInfoModel"+"=====writeBuffer"+typeId.GetType().ToString()+"===="+ obj.GetType().ToString());
                }
                
            }
        }
    }
}
