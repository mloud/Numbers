using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[ExecuteInEditMode]
public class Circle : MonoBehaviour
{
    public enum ScoreType
    {
        Copper, 
        Silver,
        Gold
    }

    public enum TapBehaviour
    {
        None,
        Minus
    }

	[System.Serializable]
	public class AttributeConfig
	{
		public SpecialAttribute Attribute;
		public Sprite Visual;
	}

	public enum SpecialAttribute
	{
		None,
		Joker
	}

    [SerializeField]
	private List<AttributeConfig> attributesConfig;

    [SerializeField]
    private SpriteRenderer visualSpriteRenderer;

    [SerializeField]
    private Transform minusTransform;


	public delegate void ClickDelegate(Circle cirlce);

	public ClickDelegate OnClick;

	[SerializeField]
	private int value;

	public int Value { get { return value; } set { this.value = value; TxtMesh.text = value.ToString();} } 

	public float Radius { get { return (collider2D as CircleCollider2D).radius; } } 

	private CircleCollider2D cCollider;
	
	private TextMesh TxtMesh { get; set; }

	private Animator Animator { get; set; }

	private float Timer { get; set; }

	private SpecialAttribute attribute;

    private TapBehaviour tapBehaviour;

	private Transform VisualTransform { get; set; }

    public TapBehaviour TapBehav
    {
        get { return tapBehaviour; }

        set 
        {
            tapBehaviour = value;
            minusTransform.gameObject.SetActive(value == TapBehaviour.Minus);
        }
    }

	public SpecialAttribute Attribute 
	{ 
		get { return attribute; }

		set
		{
			if (value == SpecialAttribute.Joker)
			{
				TxtMesh.renderer.enabled = false;
			}
			else if (value == SpecialAttribute.None)
			{
				TxtMesh.renderer.enabled = true;
			}

			visualSpriteRenderer.sprite = attributesConfig.Find(x=>x.Attribute == value).Visual;
			attribute = value;
		}
	}

    public Circle Clone()
    {
        return (Instantiate(gameObject) as GameObject).GetComponent<Circle>();
    }

    public void SetForHud()
    {
        Destroy (GetComponent<CircleCollider2D> ()); // no touches anymore
        Destroy (GetComponent<Animator> ()); // no anims
    
        minusTransform.gameObject.SetActive(false);
    }

	public void Run(TapBehaviour behaviour)
	{
		Timer = Time.time;
	
        TapBehav = behaviour;
    }

	public void SetScale(float scale)
	{
		//VisualTransform.localScale = new Vector3 (scale, scale, scale);
		transform.localScale = new Vector3 (scale, scale, scale);
	}

	public IEnumerator MoveToCoroutine(Vector3 locPos, float speed)
	{
		Vector3 dirN = (locPos - transform.localPosition).normalized;
		Vector3 startLocPos = transform.localPosition;
		float totDistanceSqr = (locPos - transform.localPosition).sqrMagnitude;

		float t = 0;

		while (true)
		{
			transform.localPosition = transform.localPosition + dirN * speed * Time.deltaTime;
			t = (transform.localPosition - startLocPos).sqrMagnitude / totDistanceSqr;
			t = Mathf.Clamp01(t);
		
			if (t < 1)
				yield return 0;

			else
				break;
		}

		transform.localPosition = locPos;
	}

	public IEnumerator ChangeValueTo(int value, SpecialAttribute attribute, TapBehaviour tapBehav)
	{
		float startTime = Time.time;
		float flipTime = 0.3f;

		float t = 0;

		while (true)
		{
			t = Mathf.Clamp((Time.time - startTime) / flipTime, 0, 1);

			transform.localEulerAngles = new Vector3(0, t * 90, 0);

			if (t >= 1)
				break;

			yield return 0;
		}
	
		// set new value
		Value = value;

		Attribute = attribute;

        TapBehav = tapBehav;

		startTime = Time.time;
		t = 0;
		while (true)
		{
			t = Mathf.Clamp((Time.time - startTime) / flipTime, 0, 1);
			
			transform.localEulerAngles = new Vector3(0, ((1 - t) * 90), 0);
			
			if (t >= 1)
				break;
			
			yield return 0;
		}
	}

	private void DoOnClick()
	{
		//Animator.SetTrigger ("click");
	
		if (OnClick != null)
		{
			OnClick(this);
		}
	}

	private void Awake()
	{
		TxtMesh = GetComponent<TextMesh> ();
		Animator = GetComponent<Animator> ();
		VisualTransform = gameObject.transform.FindChild ("Visual");
	}

	private void Start ()
	{}

	private void Update () 
	{}

	private void OnMouseDown()
	{
		DoOnClick ();
	}

}
