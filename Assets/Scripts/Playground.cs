using UnityEngine;
using System.Collections;

public class Playground : MonoBehaviour 
{
	public Vector2 Size { get { return bCollider.size; } }
	public Vector3 Position { get { return transform.position + new Vector3(bCollider.center.x, bCollider.center.y, 0); } }

	private BoxCollider2D bCollider;

	void Awake () 
	{
		bCollider = GetComponent<BoxCollider2D> ();
	}



}
