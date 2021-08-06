using UnityEngine;
using System.Collections;
using UnityEngine.UI; //Need this for calling UI scripts
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;


public class Manager : MonoBehaviour {

	public Transform UIPanel; //Will assign our panel to this variable so we can enable/disable it
	public Transform[] GameObjs2Disable;
	public bool isPaused; //Used to determine paused state

	void Start () {
		UIPanel.gameObject.SetActive(false); //make sure our pause menu is disabled when scene starts
		isPaused = false; //make sure isPaused is always false when our scene opens
	}

	void Update () {
		//If player presses escape and game is not paused. Pause game. If game is paused and player presses escape, unpause.
		if(Input.GetKeyDown(KeyCode.Escape) && !isPaused) {
			Pause();
		}
		else if(Input.GetKeyDown(KeyCode.Escape) && isPaused) {
			UnPause();
		}
	}

	public void Pause() {
		foreach (Transform i in GameObjs2Disable) {
			i.gameObject.SetActive(false);
		}
		isPaused = true;
		UIPanel.gameObject.SetActive(true); //turn on the pause menu
		Time.timeScale = 0f; //pause the game
	}

	public void UnPause() {
		foreach (Transform i in GameObjs2Disable) {
			i.gameObject.SetActive(true);
		}
		isPaused = false;
		UIPanel.gameObject.SetActive(false); //turn off pause menu
		Time.timeScale = 1f; //resume game
	}

	public void QuitGame() {
		Application.Quit();
	}

	public void Restart() {
		UnPause();
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}