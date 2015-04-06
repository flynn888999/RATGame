using UnityEngine;
using System.Collections;

namespace CxlRocker
{
	public class RockerInfo
	{
		public RockerInfo( Transform trans)
		{
			this.trans = trans;
			lastPos = Vector3.zero;
			screenPos = Vector3.zero;
			offset = Vector3.zero;
		}
		
		public void InitPositon()
		{
			lastPos = UICamera.currentTouch.pos;
		}
		
		public void CountScreenPosition()
		{
			screenPos = UICamera.currentCamera.WorldToScreenPoint( trans.position);
		}
		
		public void SetScreenMousePosition()
		{
			lastPos = UICamera.currentTouch.pos;
			screenPos = UICamera.currentCamera.WorldToScreenPoint( trans.position);
			trans.position = UICamera.currentCamera.ScreenToWorldPoint( new Vector3 (lastPos.x, lastPos.y, screenPos.z));
		}
		
		public void RecordOffsetByMousePosition()
		{
			lastPos = UICamera.currentTouch.pos;
			screenPos = UICamera.currentCamera.WorldToScreenPoint( trans.position);
			offset = trans.position - UICamera.currentCamera.ScreenToWorldPoint( new Vector3 (lastPos.x, lastPos.y, screenPos.z));
		}
		
		public void Move()
		{
			lastPos = UICamera.currentTouch.pos;
			trans.position = offset + UICamera.currentCamera.ScreenToWorldPoint( new Vector3 (lastPos.x, lastPos.y, screenPos.z));
		}
		
		public void Limit( float range)
		{
			if ( trans.localPosition.magnitude >= range)
			{
				trans.localPosition = trans.localPosition.normalized * range;
			}
		}
		
		public Vector2 GetNormalOffset( float range)
		{
			return new Vector2(trans.localPosition.x / range, trans.localPosition.y / range);
		}
		
		public Vector3 offset;
		public Vector3 lastPos;
		public Vector3 screenPos;
		public Transform trans;
	}
}