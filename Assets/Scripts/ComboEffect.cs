using UnityEngine;
using System.Collections;

public class ComboEffect : MonoBehaviour 
{
	[SerializeField]
	TextMesh comboValue;

	[SerializeField]
	TextMesh comboText;

	private enum State
	{
		Hidden,
		FadingIn,
		FadingOut
	}

	private State CurrentState { get; set; }

	[SerializeField]
	float durationOut = 2.0f;

	[SerializeField]
	float durationIn = 0.4f;

	private float Timer { get; set; }

	public void Show(int number)
	{
		CurrentState = State.FadingIn;

		Timer = durationIn;

		comboValue.text = number.ToString ();
	}

	void Update () 
	{
		var color = comboValue.color;
	
		if (CurrentState == State.FadingIn)
		{
			color.a = 1 - (Timer / durationIn);
		}
		else if (CurrentState == State.FadingOut)
		{
			color.a = Timer / durationOut;
		}
		else
		{
			color.a = 0;
		}

		comboValue.color = color;
		comboText.color = color;
	
		Timer = Mathf.Max (0, Timer - Time.deltaTime);

		if (Timer == 0)
		{
			if (CurrentState == State.FadingIn)
			{
				CurrentState = State.FadingOut;
				Timer = durationOut;
			}
		}
	}
}
