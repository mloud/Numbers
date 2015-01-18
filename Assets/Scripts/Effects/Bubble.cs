using UnityEngine;
using System.Collections;
using System;

public class Bubble : MonoBehaviour 
{
	[SerializeField]
	SpriteRenderer bubbleRenderer;

	[SerializeField]
	Transform containerTransform;

	public Action<Bubble> OnClick;

	public BonusBase Bonus { get; private set; }

	private Vector3 direction;
	private float speed;

	private enum State
	{
		Flying,
		Consuming
	}

	private State CurrentState { get; set; }


	public void Run (Vector3 pos, Vector3 dir, float speed, BonusBase bonus)
	{
		this.transform.position = pos;
		this.direction = dir.normalized;
		this.speed = speed;

		Bonus = bonus;
		bonus.transform.SetParent (containerTransform);
		bonus.transform.localPosition = Vector3.zero;
		bonus.transform.localScale = Vector3.one;

		CurrentState = State.Flying;
	}

	void Start () 
	{}

	void Update ()
	{
		switch(CurrentState)
		{
		case State.Flying:
			UpdateFlyingState();
			break;

		case State.Consuming:
			UpdateConsumingState();
			break;
		
		}
	}

	private void UpdateFlyingState()
	{
		transform.position += (direction * speed);
	}

	private void UpdateConsumingState()
	{
		transform.position += (direction * 6 * speed);
	}


	// Out of screen
	void OnBecameInvisible() 
	{
		//Destroy (gameObject);
	}

	private void DoOnClick()
	{
		bubbleRenderer.enabled = false;
		//Animator.SetTrigger ("click");


		CurrentState = State.Consuming;

		if (OnClick != null)
		{
			OnClick(this);
		}
	}

	
	private void OnMouseDown()
	{
		DoOnClick ();
	}
}
