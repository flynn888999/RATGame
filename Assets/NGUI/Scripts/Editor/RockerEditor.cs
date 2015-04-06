using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace CxlRocker
{
	public class RockerEditor : EditorWindow {

		UIAtlas selectAtlas = null;

		string backGroundSprite = "";
		string iconSprite = "";

		private Transform selectTrans;

		[MenuItem("NGUI/Rocker")]
		static void CreateRocker()
		{
			Rect rt = new Rect(0,0,450,180);
			RockerEditor window = (RockerEditor)EditorWindow.GetWindowWithRect(typeof(RockerEditor), rt, true, "Rocker");

			Transform[] trans = Selection.GetTransforms( SelectionMode.TopLevel);
			if ( trans != null && trans.Length > 0)
			{
				window.selectTrans = trans[0];
			}
		}

		void OnInspextorUpdate()
		{
			this.Repaint();
		}

		void OnSelectAtlas (Object ob)
		{
			selectAtlas = ob as UIAtlas;
			Debug.Log(selectAtlas);
			OnInspextorUpdate();
		}

		void OnSelectSprite1(string spriteName)
		{
			backGroundSprite = spriteName;
			OnInspextorUpdate();
		}

		void OnSelectSprite2(string spriteName)
		{
			iconSprite = spriteName;
			OnInspextorUpdate();
		}

		void OnSelectionChange()
		{
			foreach(Transform t in Selection.transforms)
			{
				selectTrans = t;
				OnInspextorUpdate();
				break;
			}
		}

		void OnGUI()
		{
			GUILayout.BeginHorizontal();

			if (NGUIEditorTools.DrawPrefixButton("Atlas"))
				ComponentSelector.Show<UIAtlas>(OnSelectAtlas);

			GUILayout.EndHorizontal();


			GUILayout.BeginHorizontal();
			GUILayout.Label("backGround", GUILayout.MinWidth(120));
			if ( selectAtlas != null)
			{
				GUILayout.Label(backGroundSprite, GUILayout.MinWidth(120));
				NGUIEditorTools.DrawAdvancedSpriteField(selectAtlas, selectAtlas.name, OnSelectSprite1, false);
			}
			GUILayout.EndHorizontal();


			GUILayout.BeginHorizontal();
			GUILayout.Label("icon", GUILayout.MinWidth(120));
			if ( selectAtlas != null)
			{
				GUILayout.Label(iconSprite, GUILayout.MinWidth(120));
				NGUIEditorTools.DrawAdvancedSpriteField(selectAtlas, selectAtlas.name, OnSelectSprite2, false);
			}
			GUILayout.EndHorizontal();

			GUILayout.Space(20f);

			if ( selectTrans != null)
			{
				GUILayout.Label(string.Format("Parent: 添加Rocker到 {0}层次下!",selectTrans.name));
			}
			else
				GUILayout.Label( "Parent: 没有选择父节点!");

			GUILayout.Space(20f);

			if ( selectAtlas == null || string.IsNullOrEmpty(iconSprite) || string.IsNullOrEmpty(backGroundSprite))
			{
				GUILayout.Label( "你需要先选择图集和纹理!然后才能点击创建按钮!");
			}
			else if ( selectTrans == null)
			{
				GUILayout.Label( "你需要选择一个父节点!");
			}
			else
			{
				if (GUILayout.Button("Create"))
				{
					Create();
					Close();
				}
			}
		}

		void Create()
		{
			int depth = 15;

			GameObject rocker = CreateWidget("Rocker", 300, 300, depth++, selectTrans.gameObject);

			GameObject go = CreateSprite("backGround", backGroundSprite, depth++, rocker);

			CreateSprite("icon", iconSprite, depth++, go);

			rocker.GetComponent<UIWidget>().MakePixelPerfect();
		}

		GameObject CreateWidget(string name, int width, int height , int depth, GameObject parent)
		{
			GameObject rocker = CreateEmptyGameObject(name, parent);

			rocker.AddComponent<RockerManager>();

			UIWidget widget = rocker.AddComponent<UIWidget>();
			NGUITools.AddWidgetCollider(rocker);

			widget.depth = depth;
			widget.SetDimensions(width, height);
			widget.autoResizeBoxCollider = true;

			return rocker;
		}

		GameObject CreateSprite(string name, string uiName, int depth, GameObject parent)
		{
			GameObject child = CreateEmptyGameObject(name, parent);


	//		UISprite sp = NGUITools.AddSprite( child, selectAtlas, uiName);
			UISprite sp = child.AddComponent<UISprite>();
			NGUITools.AddWidgetCollider(child);

			sp.depth = depth;
			sp.atlas = selectAtlas;
			sp.spriteName = uiName;
			sp.MakePixelPerfect();
			sp.autoResizeBoxCollider = true;

			child.AddComponent<UIDragRocker>();

			return child;
		}

	    GameObject CreateEmptyGameObject( string name, GameObject parent)
		{
			GameObject ob = new GameObject(name);

			ob.layer = parent.layer;

			Transform trans = ob.transform;
			trans.parent = parent.transform;
			trans.localPosition = Vector3.zero;
			trans.localScale = Vector3.zero;
			trans.rotation = Quaternion.identity;

			return ob;
		}
	}
}
