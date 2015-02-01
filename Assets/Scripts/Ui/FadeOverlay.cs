using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class FadeOverlay : MonoBehaviour
{
	private Image Image { get; set; }

	private float Timer { get; set; }

	private void Awake()
	{
		Image = GetComponent<Image>();
	}

	public IEnumerator FadeCoroutine(bool fadeOut, float duration)
	{
	
		float startTime = Time.time;

		while(true)
		{
			float t = Mathf.Clamp01( (Time.time - startTime) / duration); // 0..1
		
			var c = Color.black;
			c.a = fadeOut ? 1 - t : t;

			Image.color = c;


			if (t >= 1)
			{
				//Destroy(gameObject);
				break;
			}
			else
				yield return 0;
		}

	}

	private void OnDestroy()
	{
	
	}

}