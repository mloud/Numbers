using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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


	private void Awake()
	{
		var aSources = GetComponents<AudioSource> ();

		ASource = aSources [0];
		ASourceMusic = aSources [1];
	}

	public void PlayEffect(string name)
	{
		ASource.PlayOneShot (Clips.Find (x => x.Name == name).AClip);			
		Debug.Log ("Sound.PlayEffect() " + name + " " + Time.time);
	}

	public void PlayMusic(string name)
	{
		Song song = Songs.Find (x => x.Name == name);

		ASourceMusic.clip = song.AClip;	
		ASourceMusic.volume = song.Volume;
		ASourceMusic.Play ();

		Debug.Log ("Sound.PlayMusic() " + name + " " + Time.time);
	}



	public void StopEffects()
	{
		ASource.Stop ();
		Debug.Log ("Sound.StopEffects() " + Time.time);
	}

	public void StopMusic()
	{
		StartCoroutine(StopMusicCoroutine(1.0f));
		Debug.Log ("Sound.StopMusic() " + Time.time);
	}


	private IEnumerator StopMusicCoroutine(float fadeOutTime)
	{
		float volume = ASourceMusic.volume;
		float startTime = Time.time;
		while (true)
		{
			ASourceMusic.volume = (1 - Mathf.Clamp01((Time.time - startTime) / fadeOutTime)) * volume;

			if (ASourceMusic.volume == 0)
			{
				ASourceMusic.Stop();
				break;
			}
			else
			{
				yield return 0;
			}
		}
	
	}



}
