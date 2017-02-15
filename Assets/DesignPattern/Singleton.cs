/* Author : woodie(野汪)
 * Update Date:
 * */
using UnityEngine;

namespace FirePanda.DesignPattern
{
	public class Singleton<T> where T : Singleton<T>, new()
	{
		private static T mInst = null;
		
		protected virtual void Init()
		{
		}

		protected virtual void OnDestoryInst()
		{
		}
		
		public static T Inst
		{
			get
			{
				if (mInst != null)
					return mInst;
				
				//找不到,自行建立
				mInst = new T();
				mInst.Init();
	
				return mInst;
			}
		}

		public static bool IsExist
		{
			get
			{
				return (mInst != null);
			}
		}

		public static void DestroyInst()
		{
			if (mInst == null)
				return;

			mInst.OnDestoryInst();

			mInst = null;
		}
	}
}

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
	private static T mInst = null;

	protected virtual void Init()
	{
	}

	protected virtual void OnDestroyInst()
	{
	}

	public virtual string ResName
	{
		get
		{
			return string.Empty;
		}
	}

	public static T Inst
	{
		get
		{
			if (mInst != null)
				return mInst;

			mInst = FindObjectOfType(typeof(T)) as T;//找有沒有已經被產生的物件實體
			if (mInst != null)
				return mInst;

			//找不到,自行產生GameObject
			GameObject obj = new GameObject("TempName");
			mInst = obj.AddComponent<T>();
			obj.name = typeof(T).ToString();
			mInst.Init();

			return mInst;
		}
	}

	public static bool IsExist
	{
		get
		{
			return (mInst != null);
		}
	}

	public static void DestroyInst()
	{
		if (mInst == null)
			return;

		GameObject.Destroy(mInst.gameObject);
		
		mInst.OnDestroyInst();

		mInst = null;
	}

}