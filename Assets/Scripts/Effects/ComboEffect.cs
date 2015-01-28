using UnityEngine;
using System.Collections;

public class ComboEffect : MonoBehaviour 
{
	[SerializeField]
	TextMesh comboValue;

    [SerializeField]
    TextMesh comboText;

    [SerializeField]
    TextMesh specialText;

    [SerializeField]
    Animator animator;


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
	float durationIn = 0.46f;

	private float Timer { get; set; }

	public void Show(int score, string special)
	{
		CurrentState = State.FadingIn;

		Timer = durationIn;

        comboValue.text = score.ToString();

        bool isSpecialtext = !string.IsNullOrEmpty(special);
       // specialText.gameObject.SetActive(isSpecialtext);
        if (isSpecialtext)
        {
            specialText.text = special;
            animator.SetTrigger("Special");
        }
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
