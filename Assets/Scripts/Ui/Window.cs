using UnityEngine;
using System.Collections;
using System;


public class Window : MonoBehaviour 
{
    public Action<Window> CloseFinished;
    public Action<Window> OpenFinished;


    private const string EVENT_OPEN_FINISHED = "openFinished";
    private const string EVENT_CLOSE_FINISHED = "closeFinished";


	public string Name { get { return gameObject.name; } }

    private Animator Animator { get; set; }


    private void Awake()
    {
        Animator = GetComponent<Animator>();
        
        if (Animator != null)
            Animator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }
	
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
        if (Animator != null)
            Animator.SetTrigger("Open");

		OnOpen ();


        if (Animator == null)
            OnEvent(EVENT_OPEN_FINISHED);
    }

	public void Close()
	{
        if (Animator != null)
            Animator.SetTrigger("Close");

		OnClose ();

        if (Animator == null)
            OnEvent(EVENT_CLOSE_FINISHED);
	}

	public void Update()
	{
		OnUpdate ();
	}

    public void OnEvent(string eventName)
    {
        if (eventName == EVENT_CLOSE_FINISHED)
        {
            if (CloseFinished != null)
                CloseFinished(this);
        }
        else if (eventName == EVENT_OPEN_FINISHED)
        {
            if (OpenFinished != null)
                OpenFinished(this);
        }
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
