﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NumberContainer : MonoBehaviour 
{
	private List<CircleController> Numbers { get; set; }

	private Vector2 Size { get { return BCollider.size; } }

	private BoxCollider2D BCollider { get; set; }

	private void Awake()
	{
		BCollider = GetComponent<BoxCollider2D> ();

        Numbers = new List<CircleController>();
	}


	public void AddNumber(CircleController circleSrc)
	{
        var circle = circleSrc.Clone();
        circle.SetForHud();

		Numbers.Add (circle);

        circle.Visual.SetParent(gameObject.transform);
		
		float size = 0.6f;// todo

		var pos = Vector3.zero;
		var posX = size * Numbers.Count;

		float modPos = ((posX / Size.x) - Mathf.FloorToInt (posX / Size.x)) * Size.x;
		float y = Mathf.FloorToInt (posX / Size.x) * size;

		pos.x = -Size.x * 0.5f + modPos;
		pos.y -= y;


        circle.MoveTo(pos, 30);
		circle.SetScale (0.6f);
	}

	public void Clear()
	{
		//Numbers.ForEach (x => Destroy (x.gameObject));
		//Numbers.Clear ();
	
        StartCoroutine(ClearCoroutine());
    }

    private IEnumerator ClearCoroutine()
    {
        var numCopy = new List<CircleController>(Numbers);
        Numbers.Clear();

        float time = 0.3f / numCopy.Count;

        while (numCopy.Count > 0)
        {
            Destroy(numCopy[numCopy.Count - 1].gameObject);
            numCopy.RemoveAt(numCopy.Count - 1);
            yield return new WaitForSeconds(time);
        }
    }

}
