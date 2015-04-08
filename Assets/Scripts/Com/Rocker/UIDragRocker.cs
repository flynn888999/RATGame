using UnityEngine;
using System.Collections;

using UIRocker = CxlRocker.RockerManager;

namespace CxlRocker
{
	public class UIDragRocker : MonoBehaviour {


		private Transform mTrans;
		private UIRocker rocker;

		void Awake()
		{
			mTrans = transform;
		}
		
		void Start () {
		
			rocker = NGUITools.FindInParents<UIRocker>( mTrans);

			if ( rocker == null)
			{
				Debug.Log("UIDragRocker => rocker == null!");
				return;
			}

			if ( mTrans.GetComponent<BoxCollider>() == null)
				mTrans.gameObject.AddComponent<BoxCollider>().isTrigger = true;
		}

		bool Activate()
		{
			return rocker != null && rocker.enabled;
		}
		
		void OnPress( bool isPress)
		{
			if ( Activate())
			{
				rocker.OnChildPress( mTrans,isPress);
			}
		}

		void OnDrag( Vector2 delta)
		{
            if (delta == Vector2.zero)
                return;
            //Debug.Log("距离 " + delta.ToString());
			if ( Activate())
			{
				rocker.OnChildDrag(mTrans);
			}
			else
			{
				rocker.OnChildPress( mTrans,false);
			}
		}
	}
}
