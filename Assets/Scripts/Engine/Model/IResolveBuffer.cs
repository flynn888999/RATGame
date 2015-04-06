
namespace Communication.Model
{
	/// <summary>
	/// 解析类型接口
	/// </summary>
	public interface IResolveBuffer
	{

		/// <summary>
		/// 读 </summary>
		/// <param name="buffer">
		/// @return </param>
        object read(ByteBuffer buffer, CommandInfoModel model);

		/// <summary>
		/// 写 </summary>
		/// <param name="buffer"> </param>
		/// <param name="value"> </param>
        void write(ByteBuffer buffer, object value, CommandInfoModel model);
	}

}