using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class SoundExt : MonoBehaviour 
{
	[SerializeField]
	private string effect;


	private Button Button { get; set; }

	private Window Window { get; set; }

	private void Awake()
	{
		Button = GetComponent<Button>();

		if (Button != null)
		{
			Button.onClick.AddListener( () => OnPlay() ) ;
		}

		Window = GetComponent<Window>();

		if (Window != null)
		{
			Window.OpenStart += (Window obj) => OnPlay();
		}
		
	}

	public void OnPlay()
	{
		App.Instance.Sound.PlayEffect(effect);
	}
}
