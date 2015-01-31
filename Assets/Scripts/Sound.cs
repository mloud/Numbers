using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Sound : MonoBehaviour
{
	[System.Serializable]
	public class Clip
	{
		public string Name;
		public AudioClip AClip;
	}


	[System.Serializable]
	public class Song
	{
		public string Name;
		public AudioClip AClip;
		public float Volume;
	}

	
	[SerializeField]
	private List<Clip> Clips;

	[SerializeField]
	private List<Song> Songs;


	private AudioSource ASource { get; set; }

	private AudioSource ASourceMusic { get; set; }


    private float MaxMusicVolume { get; set; }

	private void Awake()
	{
		var aSources = GetComponents<AudioSource> ();

		ASource = aSources [0];
		ASourceMusic = aSources [1];

        MaxMusicVolume = ASourceMusic.volume;
    }

	public void PlayEffect(string name)
	{
		ASource.PlayOneShot (Clips.Find (x => x.Name == name).AClip);			
		Core.Dbg.Log ("Sound.PlayEffect() " + name + " " + Time.time);
	}

	public void PlayMusic(string name)
	{
		Song song = Songs.Find (x => x.Name == name);

		ASourceMusic.clip = song.AClip;	
		ASourceMusic.volume = song.Volume;
		ASourceMusic.Play ();

		Core.Dbg.Log ("Sound.PlayMusic() " + name + " " + Time.time);
	}



	public void StopEffects()
	{
		ASource.Stop ();
		Core.Dbg.Log ("Sound.StopEffects() " + Time.time);
	}

    public void PauseMusic()
    {
        StartCoroutine(FadeMusicCoroutine(true, 1.0f, delegate { ASourceMusic.Pause(); }));
        Core.Dbg.Log("Sound.Pause() " + Time.time);
    }

    public void ResumeMusic()
    {
        StartCoroutine(FadeMusicCoroutine(false, 1.0f, delegate { ASourceMusic.Play(); }));
        Core.Dbg.Log("Sound.Resume() " + Time.time);
    }

	public void StopMusic()
	{
        StartCoroutine(FadeMusicCoroutine(true, 1.0f, delegate { ASourceMusic.Stop(); }));
		Core.Dbg.Log ("Sound.StopMusic() " + Time.time);
	}

   private IEnumerator FadeMusicCoroutine(bool fadeOut, float time, Action action)
	{
		
		float startTime = Time.realtimeSinceStartup;
        float volume = ASourceMusic.volume;

        if (!fadeOut)
        {
            action();
        }

		while (true)
		{
            float t = Mathf.Clamp01((Time.realtimeSinceStartup - startTime) / time);


            if (fadeOut)
            {
                ASourceMusic.volume = (1 - t) * volume;
                 if (ASourceMusic.volume == 0)
                 {
                    action();
                    break;
                 }
            }
            else
            {
                 ASourceMusic.volume = t;
                 if (ASourceMusic.volume >= MaxMusicVolume)
                 {
                     ASourceMusic.volume = MaxMusicVolume;
                     break;
                 }
            }
            
		    yield return 0;
		}
	
	}



}
