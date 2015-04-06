using UnityEngine;
using System.Collections;


namespace CxlRocker
{
	public enum RockerType
	{
		Fixed,
		Region,
	}

	public class RockerManager : MonoBehaviour {

		private static readonly string backGroundName = "backGround";
		private static readonly string iconName = "icon";
		public static BetterList<RockerManager> rockerList = new BetterList<RockerManager>();


		private Transform mTrans;
		private UIWidget mWidget;
		private RockerType mType = RockerType.Region;


		private int regionWidth = 300;
		private int regionHeight = 300;
		
		private RockerInfo rockerBackGround;
		private RockerInfo rockerIcon;

		private float offsetMaxLenth = 100f;
		
		private bool mIsStartMove = false;

		public BetterList<IRocker> rockerEvent = new BetterList<IRocker>();

		private RockerData eventData;
		#region Attribute
		public int RegionWidth
		{
			get{ return regionWidth;}
			set
			{ 
				if ( regionWidth != value)
				{
					regionWidth = value;
					mWidget.width = regionWidth;
				}
			}
		}

		public int RegionHeight
		{
			get{ return regionHeight;}
			set
			{
				if ( regionHeight != value)
				{
					regionHeight = value;
					mWidget.height = regionHeight;
				}
			}
		}

		public RockerType RType
		{
			get{ return mType;}
			set
			{
				if ( mType != value)
				{
					ChangeType( mType);
				}
			}
		}
		#endregion


		void Awake()
		{
			mTrans = transform;
			rockerList.Add( this);
		}

		void Start()
		{
			eventData = new RockerData();

			rockerBackGround = new RockerInfo (mTrans.Find(backGroundName));
			rockerIcon = new RockerInfo( rockerBackGround.trans.GetChild(0));

			if ( mType == RockerType.Region)
			{
				mWidget = mTrans.GetComponent<UIWidget>();

				mWidget.SetDimensions( regionWidth, regionHeight);
				NGUITools.AddWidgetCollider( mTrans.gameObject);
			}
		}

		public void OnChildPress( Transform trans, bool isPress)
		{
			if ( isPress)
			{
				//	如果按压到背景框按钮应立即显示到点击位置
				if ( trans.name.Equals(backGroundName))
				{
					rockerIcon.SetScreenMousePosition();
					rockerIcon.offset = Vector3.zero;

					//	立即派发移动事件
					eventData.normalOffset = rockerIcon.GetNormalOffset( offsetMaxLenth);
					DistributedMoveEvent();

					mIsStartMove = true;
				}
				else if ( trans.name.Equals(iconName))
				{
					mIsStartMove = true;
					rockerIcon.RecordOffsetByMousePosition();
				}
			}
			else
			{
				mIsStartMove = false;
				rockerIcon.trans.localPosition = Vector2.zero;

				//	立即派发停止事件
				eventData.normalOffset = Vector2.zero;
				DistributedStopEvent();
			}
		}

		public void OnChildDrag( Transform trans)
		{
			if ( mIsStartMove)
			{
				rockerIcon.Move();
				rockerIcon.Limit(offsetMaxLenth);

				eventData.normalOffset = rockerIcon.GetNormalOffset( offsetMaxLenth);
				DistributedMoveEvent();
			}
		}

		private void OnPress( bool isPress)
		{
			if ( isPress)
			{
				//	移动整个摇杆到指定区域
				rockerBackGround.SetScreenMousePosition();

				//	同时派发摇杆按下事件
				OnChildPress( rockerIcon.trans, true);
			}
			else
			{
				OnChildPress( rockerIcon.trans, false); 

				rockerBackGround.trans.localPosition = Vector3.zero;
			}
		}

		private void OnDrag()
		{
			OnChildDrag( rockerIcon.trans);
		}

		private void DistributedMoveEvent()
		{
			if ( rockerEvent.size == 0) return;

			int length = rockerEvent.size;
			for (int i = 0; i < length; i++) {
				rockerEvent[i].OnRockerMove( eventData);
			}
		}

		private void DistributedStopEvent()
		{
			if ( rockerEvent.size == 0) return;

			int length = rockerEvent.size;
			for (int i = 0; i < length; i++) {
				rockerEvent[i].OnRockerStop( eventData);
			}
		}

		private void ResetRegionSize( int w, int h)
		{
			regionWidth = w;
			regionHeight = h;
			mWidget.SetDimensions( regionWidth, regionHeight);
		}

		private void ChangeType( RockerType type)
		{
			mType = type;
			switch (mType) 
			{
				case RockerType.Fixed:
					{
						mWidget.GetComponent<BoxCollider>().enabled = false;
					}break;
				case RockerType.Region:
					{
						mWidget.GetComponent<BoxCollider>().enabled = true;
					}break;
				default:
						break;
			}
		}
	}
}

