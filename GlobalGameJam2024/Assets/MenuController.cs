using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
	public TextMeshProUGUI ScoreText;

	public void StartGame() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene(1, LoadSceneMode.Single);
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public void Start()
	{
		int score = PlayerPrefs.GetInt("Highscore", 0);
		ScoreText.text = score + " Seconds";
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}
}
