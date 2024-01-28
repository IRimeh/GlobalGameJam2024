using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hole : AbstractInteractableObject
{
    public static List<Hole> Instances = new List<Hole>();

    public int HolesTilLGameOver = 8;

	private void OnDestroy()
	{
        Instances.Remove(this);
	}

	private void Awake()
	{
        Instances.Add(this);
	}

	public ParticleSystem Splash;

    private List<Orc> orcsCleaning = new List<Orc>();
    public bool isCleaned = false;

    public float TimeToFix = 10.0f;
    private float _timeFixed = 0.0f;

    public GameObject ObjToDestroyWhenClean;
    public GameObject HammerPrefab;

    public override bool IsWorkable(Orc orc)
    {
        return !isCleaned;
    }

    public override void OnStopTask(Orc orc)
    {
        if (orc.isHoldingObj)
        {
            orc.StopHoldingObject(out GameObject holdObj);
            Destroy(holdObj);
        }

        if (orcsCleaning.Contains(orc))
            orcsCleaning.Remove(orc);
    }

    public override IEnumerator Task(Orc orc)
    {
        if (orc.isHoldingObj)
        {
            orc.StopHoldingObject(out GameObject holdObj);
            Destroy(holdObj);
        }
        GameObject broom = Instantiate(HammerPrefab);
        orc.HoldObject(broom);
        orc.agent.destination = transform.position + (orc.transform.position - transform.position).normalized;

        float distance = (transform.position - orc.transform.position).magnitude;
        while (distance > 2)
        {
            if (this == null)
                yield break;
            distance = (transform.position - orc.transform.position).magnitude;
            yield return new WaitForSeconds(0.1f);
        }

        orcsCleaning.Add(orc);

        while (!isCleaned)
        {
            if (this == null)
                yield break;
            yield return new WaitForSeconds(0.1f);
        }

        orc.StopTask();
    }

    private void Update()
    {
        _timeFixed += Time.deltaTime * orcsCleaning.Count;
        if (_timeFixed > TimeToFix)
        {
            isCleaned = true;
            for (int i = orcsCleaning.Count - 1; i >= 0; i--)
            {
                orcsCleaning[i].StopTask();
            }
            StopAllCoroutines();
            Destroy(ObjToDestroyWhenClean);
        }

        if (Instances.Count >= HolesTilLGameOver) {
            FindObjectOfType<GameOverScript>().StartGameOver();
        }
    }
}
