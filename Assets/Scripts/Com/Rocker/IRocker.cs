using UnityEngine;
using System.Collections;


namespace CxlRocker
{
	public class RockerData
	{
		public string rockerName;
		public Vector2 normalOffset;
	}

	public interface IRocker  {

		void OnRockerMove(RockerData rd);

		void OnRockerStop(RockerData rd);
	}
}