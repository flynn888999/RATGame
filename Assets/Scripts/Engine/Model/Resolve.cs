using Communication.Model;
using System;
using System.Text;

namespace com.commons.network.modulecore
{

	public class Resolve
	{
		/// <summary>
		/// 收到包的解析 
        /// </summary>
		public static void resolveReceivable(object[] objs, ByteBuffer buffer, CommandInfoModel[] commands)
		{
			int i = 1;
			foreach (CommandInfoModel model in commands)
			{
                objs[i] = model.TypeId.read(buffer, model);
				i += 1;
			}
		}

		/// <summary>
		/// 解析发送出去的数据
        /// </summary>
        public static void resolveSendable(object obj, ByteBuffer buffer, CommandInfoModel command)
		{
            command.TypeId.write(buffer, obj, command);
		}

		/// <summary>
		/// 数据类型枚举
		/// </summary>
		internal class BuffType
		{
            private static ByteResolveBuffer BYTE = new ByteResolveBuffer();
            private static ShortResolveBuffer SHORT = new ShortResolveBuffer();
            private static IntResolveBuffer INT = new IntResolveBuffer();
            private static LongResolveBuffer LONG = new LongResolveBuffer();
            private static FloatResolveBuffer FLOAT = new FloatResolveBuffer();
            private static DoubleResolveBuffer DOUBLE = new DoubleResolveBuffer();
            private static StringResolveBuffer STRING = new StringResolveBuffer();
            private static BooleanResolveBuffer BOOLEAN = new BooleanResolveBuffer();
            private static OtherResolveBuffer OTHER = new OtherResolveBuffer();

            private BuffType(){}


            public static IResolveBuffer getType(string str)
            {
                IResolveBuffer type;

                if ("byte".Equals(str,System.StringComparison.CurrentCultureIgnoreCase))
                {
                    type = BuffType.BYTE;
                }
                else if ("short".Equals(str,System.StringComparison.CurrentCultureIgnoreCase))
                {
                    type = BuffType.SHORT;
                }
                else if ("int".Equals(str,System.StringComparison.CurrentCultureIgnoreCase))
                {
                    type = BuffType.INT;
                }
                else if ("long".Equals(str,System.StringComparison.CurrentCultureIgnoreCase))
                {
                    type = BuffType.LONG;
                }
                else if ("float".Equals(str,System.StringComparison.CurrentCultureIgnoreCase))
                {
                    type = BuffType.FLOAT;
                }
                else if ("double".Equals(str,System.StringComparison.CurrentCultureIgnoreCase))
                {
                    type = BuffType.DOUBLE;
                }
                else if ("string".Equals(str,System.StringComparison.CurrentCultureIgnoreCase))
                {
                    type = BuffType.STRING;
                }
                else if ("boolean".Equals(str,System.StringComparison.CurrentCultureIgnoreCase))
                {
                    type = BuffType.BOOLEAN;
                }
                else
                {
                    type = BuffType.OTHER;
                }

                return type;
            }
		}

		/// <summary>
		/// byte型解析
		/// </summary>
		private class ByteResolveBuffer : IResolveBuffer
		{
            public object read(ByteBuffer buffer, CommandInfoModel model)
			{
				return buffer.ReadByte();
			}

            public void write(ByteBuffer buffer, object value, CommandInfoModel model)
			{
				buffer.WriteByte((byte)value);
			}
		}

		/// <summary>
		/// short型解析
		/// </summary>
		private class ShortResolveBuffer : IResolveBuffer
		{

            public void write(ByteBuffer buffer, object value, CommandInfoModel model)
			{
				buffer.WriteShort((short)value);
			}

            public object read(ByteBuffer buffer, CommandInfoModel model)
			{
				return buffer.ReadShort();
			}
		}

		/// <summary>
		/// int型解析
		/// </summary>
		private class IntResolveBuffer : IResolveBuffer
		{

            public void write(ByteBuffer buffer, object value, CommandInfoModel model)
			{
				buffer.WriteInt((int) value);
			}

             public object read(ByteBuffer buffer, CommandInfoModel model)
			{
				return buffer.ReadInt();
			}
		}

		/// <summary>
		/// long型解析
		/// </summary>
		private class LongResolveBuffer : IResolveBuffer
		{
            public void write(ByteBuffer buffer, object value, CommandInfoModel model)
			{
				buffer.WriteLong((long) value);
			}

            public object read(ByteBuffer buffer, CommandInfoModel model)
			{
				return buffer.ReadLong();
			}
		}


		/// <summary>
		/// float型数据解析
		/// </summary>
		private class FloatResolveBuffer : IResolveBuffer
		{

            public void write(ByteBuffer buffer, object value, CommandInfoModel model)
			{
				buffer.WriteFloat((float) value);
			}

            public object read(ByteBuffer buffer, CommandInfoModel model)
			{
				return buffer.ReadFloat();
			}

		}


		/// <summary>
		/// double型数据解析
		/// </summary>
		private class DoubleResolveBuffer : IResolveBuffer
		{
            public void write(ByteBuffer buffer, object value, CommandInfoModel model)
			{
				buffer.WriteDouble((double) value);
			}

            public object read(ByteBuffer buffer, CommandInfoModel model)
			{
				return buffer.ReadDouble();
			}
		}

		/// <summary>
		/// 字符串数据解析
		/// </summary>
		private class StringResolveBuffer : IResolveBuffer
		{

            //这里字符串添加后缀来识别
            public void write(ByteBuffer buffer, object value, CommandInfoModel model)
			{

				string str = (string) value;
                for (int i = 0; i < str.Length; i++)
                {
                    buffer.WriteChar(str[i]);
               }

                //CHAR默认值为 '\x0000'
                buffer.WriteChar('\x0000');
			}

            public object read(ByteBuffer buffer, CommandInfoModel model)
			{
				StringBuilder tb = new StringBuilder();
                for (char c; (c = buffer.ReadChar()) != 0; )
                {
                    tb.Append(c);
                }
                string str = tb.ToString();
				return str;
			}

		}

		/// <summary>
		/// boolean型数据解析
		/// </summary>
		private class BooleanResolveBuffer : IResolveBuffer
		{

            public void write(ByteBuffer buffer, object value, CommandInfoModel model)
			{
				buffer.WriteByte((byte)((bool)value ? 1 : 0));
			}

            public object read(ByteBuffer buffer, CommandInfoModel model)
			{
				return buffer.ReadByte() == 0 ? false : true;
			}
		}


		/// <summary>
		/// 其他类型数据解析
		/// </summary>
		private class OtherResolveBuffer : IResolveBuffer
		{
            public void write(ByteBuffer buffer, object value, CommandInfoModel model)
			{
                //BufferManager.getSendable(model.Type).write(buffer, value);
			}

            public object read(ByteBuffer buffer, CommandInfoModel model)
			{
                //return BufferManager.getReceivableN(model.Type).read(buffer);
                return null;
			}

		}
	}

}