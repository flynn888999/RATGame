using System;
using System.Collections.Generic;

namespace Communication.Model
{
    public class CommandModel
    {
       private int opcode;
        public int Opcode
        {
            get { return opcode; }
            set { opcode = value; }
        }
    
        private CommandType type;
        public CommandType Type
        {
            get {return type; }
            set {type = value; }
        }

        private CommandInfoModel[] commandInfoModel;

        public CommandModel(){ }

        public CommandModel(int opcode, String type)
        {
            this.opcode = opcode;
            if ("client".Equals(type,StringComparison.CurrentCultureIgnoreCase))
            {
                this.type = CommandType.CLIENT_TYPE;
            }
            else
            {
                this.type = CommandType.SERVER_TYPE;
            }
        }


        public CommandInfoModel[] getCommandInfoModel()
        {
            return commandInfoModel;
        }

        public void setCommandInfoModel(CommandInfoModel[] commandInfoModel)
        {
            this.commandInfoModel = commandInfoModel;
        }

        public Dictionary<string, object> readBuffer(ByteBuffer bytes)
        {
            int length = commandInfoModel.Length;
            Dictionary<string, object> list = new Dictionary<string, object>();
            for (int i = 0; i < length; i++)
            {
                list.Add(commandInfoModel[i].getName(), commandInfoModel[i].readBuffer(bytes));
            }
            return list;
        }

        public void writeBuffer(ByteBuffer bytes, object[] objs)
        {
            int length = commandInfoModel.Length;

            //以前用了小端转大端，但服务端是java，就不转了//
            for (int i = 0; i < length; i++)
            {
                
                if (commandInfoModel[i].getType() == "int")
                {
                   // UnityEngine.Debug.LogError("int");
                    int buff = (int)objs[i];
                   // int newBuff = System.Net.IPAddress.HostToNetworkOrder(buff);
                    commandInfoModel[i].writeBuffer(bytes, buff);
                }
                else if (commandInfoModel[i].getType() == "short")
                {
                    UnityEngine.Debug.LogError("short");
                    short buff = (short)objs[i];
                    //short newBuff = System.Net.IPAddress.HostToNetworkOrder(buff);
                    commandInfoModel[i].writeBuffer(bytes, buff);
                }
                else if (commandInfoModel[i].getType() == "long")
                {
                    UnityEngine.Debug.LogError("long");
                    long buff = (long)objs[i];
                   // long newBuff = System.Net.IPAddress.HostToNetworkOrder(buff);
                    commandInfoModel[i].writeBuffer(bytes, buff);
                }
                else if (commandInfoModel[i].getType() == "float")
                {

                    UnityEngine.Debug.LogError("float");
                    commandInfoModel[i].writeBuffer(bytes, objs[i]);
                }
                else
                {
                    UnityEngine.Debug.LogError("其他类型");
                    commandInfoModel[i].writeBuffer(bytes, objs[i]);
                }
            }
        }

    }
}
