using UnityEngine;
using System.Collections;
using System;


public class Window : MonoBehaviour 
{

    public enum CloseAnim { Close_ToBottom, Close_ToTop }
    public enum OpenAnim { Open_FromBottom, Open_FromTop }


    public OpenAnim OpenAnimType = OpenAnim.Open_FromBottom;
    public CloseAnim CloseAnimType = CloseAnim.Close_ToTop;


	public Action<Window> OpenStart;
    public Action<Window> CloseFinished;
    public Action<Window> OpenFinished;


    private const string EVENT_OPEN_FINISHED = "openFinished";
    private const string EVENT_CLOSE_FINISHED = "closeFinished";


	public string Name { get { return gameObject.name; } }

    private Animator Animator { get; set; }


    protected virtual void Awake()
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
            Animator.SetTrigger(OpenAnimType.ToString());

		if ( OpenStart != null)
			OpenStart(this);

		OnOpen ();


        if (Animator == null)
            OnEvent(EVENT_OPEN_FINISHED);
    }

	public void Close()
	{
        if (Animator != null)
            Animator.SetTrigger(CloseAnimType.ToString());

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
