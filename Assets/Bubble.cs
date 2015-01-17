using UnityEngine;
using System.Collections;

public class Bubble : MonoBehaviour 
{
	private Transform ContainerTransform { get; set; }

	private Vector3 direction;
	private float speed;

	void Awake()
	{
		ContainerTransform = gameObject.transform.FindChild ("container");
	}

	public void Run (Vector3 pos, Vector3 dir, float speed)
	{
		this.transform.position = pos;
		this.direction = dir.normalized;
		this.speed = speed;
	}

	void Start () 
	{}

	void Update ()
	{
		transform.position += (direction * speed);
	}

	// Out of screen
	void OnBecameInvisible() 
	{
		Destroy (gameObject);
	}

}
