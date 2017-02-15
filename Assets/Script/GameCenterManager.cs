using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;

public class GameCenterManager : MonoSingleton<GameCenterManager> {

	[DllImport("__Internal")]
	private static extern void GameCenterSave(string message);

	[DllImport("__Internal")]
	private static extern void GameCenterLoad();

	[DllImport("__Internal")]
	private static extern void GameCenterDelete();

	public System.Action<Player> LoadEndAct {
		get;
		set;
	}

	public System.Action<string> Message {
		get;
		set;
	}

	public void SaveProgress(Player data) {
		string msg = Player.DataEncode(data);
		GameCenterSave(msg);
	}

	public void LoadProgress() {
		GameCenterLoad();
	}

	public void DeleteProgress() {
		GameCenterDelete();
	}

	public void OnErrorMessage(string errorCode) {
		int kind = Convert.ToInt32(errorCode);
		switch(kind) {
			case 3:
				if(Message != null)
					Message("Delete succeed.");
				break;
			case 2:
				Debug.Log("Load succeed.");
				//用不到
				break;
			case 1:
				if(Message != null)
					Message("Save succeed.");
				break;
			case -1:
				if(Message != null)
					Message("No auth.");
				break;
			case -2:
				if(Message != null)
					Message("Save error.");
				break;
			case -3:
				if(Message != null)
					Message("Fetch error.");
				break;
			case -4:
				if(Message != null)
					Message("No data.");
				break;
			case -5:
				if(Message != null)
					Message("Load error.");
				break;
			case -6:
				if(Message != null)
					Message("Encode error.");
				break;
			case -7:
				if(Message != null)
					Message("Decode error.");
				break;
			case -8:
				if(Message != null)
					Message("Delete error.");
				break;
			default:
				if(Message != null)
					Message("Unknow error.");
				break;
		}
	}

	public void OnLoadData(string dataString) {
		Player data = Player.DataDecode(dataString);
		if(LoadEndAct != null)
			LoadEndAct(data);
	}
}
