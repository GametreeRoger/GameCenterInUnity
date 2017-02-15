using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Player {
	public string mName;
	public int mMoney;

	public string Name {
		get{ return string.Format("Name:{0}", mName); }
	}

	public string Money {
		get {return string.Format("Money:{0}", mMoney); }
	}

	public void AddMoney() {
		mMoney += 100;
	}

	public Player(string name, int money) {
		mName = name;
		mMoney = money;
	}

	public static string DataEncode(Player data) {
		string json = JsonUtility.ToJson(data);
		byte[] jsonbyte = PlayerToBytes(json);
		return Convert.ToBase64String(jsonbyte);
	}

	public static Player DataDecode(string dataString) {
		byte[] base64byte = Convert.FromBase64String(dataString);
		string json = PlayerFromBytes(base64byte);
		return JsonUtility.FromJson<Player>(json);
	}

	public void PrintPlayer() {
		Debug.Log(string.Format("Name:{0}, Money:{1}", mName, mMoney));
	}

	private static byte[] PlayerToBytes(string data) {
		return System.Text.Encoding.Default.GetBytes(data);
	}

	private static string PlayerFromBytes(byte[] b) {
		return System.Text.Encoding.Default.GetString(b);
	}
}
