using System;
using System.Timers;
using UnityEngine;


public class GameSoundSystem : MonoBehaviour
{
	[SerializeField]
	private float MinPitch = 0.95f;

	[SerializeField]
	private float MaxPitch = 1.1f;


	private GameContext Context { get; set; }

	public void Init(GameContext context)
	{
		Context = context;
	}

	public void Update()
	{
		float pitch = MinPitch + (MaxPitch - MinPitch) * Context.Controller.GetProgress();
		App.Instance.Sound.SetMusicPitch(pitch);
	}


	public void Leave()
	{
		App.Instance.Sound.SetMusicPitch(1.0f);
	}
}