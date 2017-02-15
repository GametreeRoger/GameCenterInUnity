using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TestiCloudUI : MonoBehaviour {
	[SerializeField]
	private Text Name;
	[SerializeField]
	private Text Money;
	[SerializeField]
	private Text Message;
	
	private Player mPlayer;

	private bool mAuthenticating = false;

	public bool Authenticating {
		get {
			return mAuthenticating;
		}
	}

	public bool Authenticated {
		get {
			return Social.Active.localUser.authenticated;
		}
	}
	
	// Use this for initialization
	void Start () {
		mPlayer = new Player("Roger", 100);

		GameCenterManager.Inst.LoadEndAct = OnLoadEnd;
		GameCenterManager.Inst.Message = OnMessage;
		UpdatePlayer();
	}

	public void OnAddMoney() {
		mPlayer.AddMoney();
		UpdatePlayer();
	}

	public void OnLogin() {
		Authenticate();
	}

	public void OnSave() {
		GameCenterManager.Inst.SaveProgress(mPlayer);
	}

	public void OnLoad() {
		GameCenterManager.Inst.LoadProgress();
	}

	public void OnDelete() {
		GameCenterManager.Inst.DeleteProgress();
	}

	public void OnLoadEnd(Player data) {
		mPlayer = data;
		OnMessage("Load succeed.");
		UpdatePlayer();
	}

	public void OnMessage(string message) {
		Message.text = message;
	}

	private void UpdatePlayer() {
		if(mPlayer == null)
			return;
		
		mPlayer.PrintPlayer();
		Name.text = mPlayer.Name;
		Money.text = mPlayer.Money;
	}

	private void Authenticate() {
		if(Authenticated || mAuthenticating) {
			Debug.LogWarning("Ignoring repeated call to Authenticate().");
			return;
		}

		// Sign in to Google Play Games
		mAuthenticating = true;
		Social.localUser.Authenticate((bool success) => {
			mAuthenticating = false;
			if(success) {
				// if we signed in successfully, load data from cloud
				Debug.Log("Login successful!");
				OnMessage("Login successful!");
			} else {
				// no need to show error message (error messages are shown automatically
				// by plugin)
				Debug.LogWarning("Failed to sign in.");
				OnMessage("Failed to sign in.");
			}
		});
	}
}
