using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NumberContainer : MonoBehaviour 
{
	private List<Circle> Numbers { get; set; }

	private Vector2 Size { get { return BCollider.size; } }

	private BoxCollider2D BCollider { get; set; }

	private void Awake()
	{
		BCollider = GetComponent<BoxCollider2D> ();

		Numbers = new List<Circle> ();
	}


	public void AddNumber(Circle circleSrc)
	{
        var circle = circleSrc.Clone();
        circle.SetForHud();

		Numbers.Add (circle);

		circle.transform.SetParent (gameObject.transform);

		float size = 0.6f;// todo

		var pos = Vector3.zero;
		var posX = size * Numbers.Count;

		float modPos = ((posX / Size.x) - Mathf.FloorToInt (posX / Size.x)) * Size.x;
		float y = Mathf.FloorToInt (posX / Size.x) * size;

		pos.x = -Size.x * 0.5f + modPos;
		pos.y -= y;

		//circle.transform.localPosition = pos;
		circle.StartCoroutine(circle.MoveToCoroutine(pos, 30));

		circle.SetScale (0.7f);
	}

	public void Clear()
	{
		Numbers.ForEach (x => Destroy (x.gameObject));
		Numbers.Clear ();
	}
}
