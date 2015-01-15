using UnityEngine;
using System.Collections;


public class Window : MonoBehaviour 
{
	public string Name { get { return gameObject.name; } }

	public void Init(object param)
	{
		RectTransform rectTransform = gameObject.GetComponent<RectTransform> ();
		
		rectTransform.offsetMin = Vector2.zero;
		rectTransform.offsetMax = Vector2.zero;
		rectTransform.localScale = Vector3.one;

		OnInit (param);
	}

	public void Open()
	{
		OnOpen ();
	}

	public void Close()
	{
		OnClose ();
	}

	public void Update()
	{
		OnUpdate ();
	}

	protected virtual void OnOpen()
	{}

	protected virtual void OnClose()
	{}

	protected virtual void OnUpdate()
	{}

	protected virtual void OnInit(object param)
	{}
}
