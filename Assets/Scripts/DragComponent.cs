using UnityEngine;
using System.Collections;

public class DragComponent : MonoBehaviour
{
	protected GameObject DraggedObject { get; set; }


	protected virtual GameObject CreateDragComponent()
	{
		return gameObject;
	}

	protected virtual void OnDragStart()
	{}

	protected virtual void OnDragEnd()
	{}


	void Update()
	{
		if ( DraggedObject != null)
		{
			var pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			pos.z = DraggedObject.transform.position.z;

			DraggedObject.transform.position = pos;
		}
	}

	void OnMouseDown()
	{
		DraggedObject = CreateDragComponent ();

		OnDragStart ();
	}

	void OnMouseUp()
	{
		OnDragEnd ();
		DraggedObject = null;
	}

	void OnMouseDrag()
	{
	}
}
