using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
	public TextMeshProUGUI Text;
	public TextMeshProUGUI Score;

	public float GmaeOverShowTime = 5.0f;

	public float ReturnToMenuTime = Mathf.Infinity;

	public AliveTime time;

	private void Start()
	{
		Text.gameObject.SetActive(false);
		Score.gameObject.SetActive(false);
	}

	public void StartGameOver() {
		if (ReturnToMenuTime == Mathf.Infinity)
		{
			Text.gameObject.SetActive(true);
			Score.gameObject.SetActive(true);
			ReturnToMenuTime = Time.time + GmaeOverShowTime;
			PlayerPrefs.SetInt("Highscore", time.GetAliveTime());
			Score.text = "Score: " + FindObjectOfType<AliveTime>().GetAliveTime() + " Seconds";
		}
	}

	private void Update()
	{
		if(Time.time >= ReturnToMenuTime)
		{
			SceneManager.LoadScene(0, LoadSceneMode.Single);
		}
	}
}
