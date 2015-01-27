using UnityEngine;
using System.Collections;

public class Slot : MonoBehaviour
{
    private SpriteRenderer SpriteRenderer { get; set; }


	void Awake () 
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	public void SetColor(Color color)
    {
        SpriteRenderer.color = color;
    }

}
