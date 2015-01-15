using UnityEngine;
using System.Collections;

public class ShapeDragComponent : DragComponent
{
	protected override GameObject CreateDragComponent()
	{
		return gameObject;
	}

	protected override void OnDragStart()
	{
		var collider = DraggedObject.GetComponent<Collider2D> ();

		collider.enabled = false;
	}

	protected override void OnDragEnd()
	{
		var collider = DraggedObject.GetComponent<Collider2D> ();
		
		collider.enabled = true;

	}
}
