using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour 
{
    [SerializeField]
    private float Duration = 1.5f;

    private Text Text { get; set; }

	void Awake () 
    {
        Text = GetComponent<Text>();

        Core.Dbg.Assert(Text != null, "ScoreCounter.Awake() no Text component found");
    }

    public void Set(int num)
    {
        StartCoroutine(SetCoroutine(num, Duration));
    }

    private IEnumerator SetCoroutine(int num, float duration)
    {
        float startTime = Time.time;

        float t = 0;



        while (true)
        {
            t = Mathf.Clamp01((Time.time - startTime) / duration);

            Text.text = ((int)(num * t)).ToString();

            if (t >= 1)
                break;
            else
                yield return 0;
        }
    }


}
