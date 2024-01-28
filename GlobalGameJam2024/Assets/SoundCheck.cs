using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.UIElements;

public class SoundCheck : MonoBehaviour
{
    bool showMenu = false;

    FMOD.Studio.VCA musicVCA;
    FMOD.Studio.VCA ambientVCA;
    FMOD.Studio.VCA sfxVCA;

    public float musicVolume, ambientVolume, sfxVolume;

    private void Start()
    {
        //FMOD.Studio.VCA musicVCA = FMODUnity.RuntimeManager.GetVCA("vca:/ Music");
        musicVCA = FMODUnity.RuntimeManager.GetVCA("vca:/Music");
        ambientVCA = FMODUnity.RuntimeManager.GetVCA("vca:/Ambient");
        sfxVCA = FMODUnity.RuntimeManager.GetVCA("vca:/SFX");

        musicVCA.getVolume(out musicVolume);Debug.Log("musicVolume" + musicVolume);
        ambientVCA.getVolume(out ambientVolume); Debug.Log("ambientVolume" + ambientVolume);
        sfxVCA.getVolume(out  sfxVolume); Debug.Log("sfxVolume" + sfxVolume);

        FMODUnity.RuntimeManager.PlayOneShot("event:/BGM/Ambient");
        StartCoroutine(nameof(DelayedStartMusic));
    }

     //Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Step();
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            showMenu = !showMenu;
        }
        
    }

    void Step()
    {
        

        try
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/PeonStep", transform.position);
        }
        catch (EventNotFoundException)
        {
            Debug.LogWarning("[FMOD] Event not found: " + "event:/SFX/PeonStep");
        }
    }

    IEnumerator DelayedStartMusic()
    {
            yield return new WaitForSeconds(60f);
            FMODUnity.RuntimeManager.PlayOneShot("event:/BGM/BGM");
    }

    private void OnGUI()
    {
        if (!showMenu)
        {
            if(GUI.Button(new Rect(5, 5, 64, 16),"Menu"))
            {
                showMenu = true;
            }

        }
        else
        {
            if (GUI.Button(new Rect(5, 5, 64, 16), "Menu"))
            {
                showMenu = false;
            }

            Rect rect = new Rect(5,  5+24, 64, 24);
            GUI.Label(rect, "Music");
            rect.y += 24; GUI.Label(rect, "Ambient");
            rect.y += 24; GUI.Label(rect, "SFX");

            rect = new Rect(5 + 64, 5 + 24, 128, 24);
            musicVolume = (GUI.HorizontalSlider(rect, musicVolume, -60, 5));
            rect.y += 24;
            ambientVolume = (GUI.HorizontalSlider(rect, ambientVolume, -60, 5));
            rect.y += 24;
            sfxVolume = (GUI.HorizontalSlider(rect, sfxVolume, -60, 5));

            musicVCA.setVolume(Mathf.Pow(10.0f, musicVolume / 20f));
            ambientVCA.setVolume(Mathf.Pow(10.0f, ambientVolume / 20f));
            sfxVCA.setVolume(Mathf.Pow(10.0f, sfxVolume / 20f));

        }
    }
}
