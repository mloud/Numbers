using UnityEngine;
using System.Collections;

public class Slot : MonoBehaviour
{
    private SpriteRenderer SpriteRenderer { get; set; }

    public CircleController Circle { get; set; }

    public bool CanPlaceCircle { get { return Circle == null && SpecialSlot == null; } }

    public SpecialSlot SpecialSlot { get; set; }

	void Awake () 
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
	}

    public void OnCircleRemove(CircleController circle)
    {
        Circle = null;
    }

	public void SetColor(Color color)
    {
        SpriteRenderer.color = color;
    }

}
