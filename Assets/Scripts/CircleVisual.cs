using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[ExecuteInEditMode]
public class CircleVisual : MonoBehaviour
{
    public enum ScoreType
    {
        Copper, 
        Silver,
        Gold
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

    public List<Speciality> Specialities { get; private set; }


    [SerializeField]
	private List<AttributeConfig> attributesConfig;

    [SerializeField]
    private SpriteRenderer visualSpriteRenderer;

    [SerializeField]
    private Transform minusTransform;

    [SerializeField]
    private Transform renewTransform;
        
	public delegate void ClickDelegate(CircleVisual cirlce);

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

	private Transform VisualTransform { get; set; }

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

    public void AddSpeciality(Speciality spec)
    {
        Specialities.Add(spec);

        // TODO to view
        if ((spec as MinusOneSpeciality) != null)
        {
            minusTransform.gameObject.SetActive(true);
        }
        else if ( (spec as ReappearSpeciality) != null)
        {
            renewTransform.gameObject.SetActive(true);
        }
    }

    public void Removepeciality(Speciality spec)
    {
        Specialities.Remove(spec);

        if ((spec as MinusOneSpeciality) != null)
        {
            minusTransform.gameObject.SetActive(false);
        }
        else if ( (spec as ReappearSpeciality) != null)
        {
            renewTransform.gameObject.SetActive(false);
        }
    }



    public CircleVisual Clone()
    {
        return (Instantiate(gameObject) as GameObject).GetComponent<CircleVisual>();
    }

    public void SetForHud()
    {
        Destroy (GetComponent<CircleCollider2D> ()); // no touches anymore
        Destroy (GetComponent<Animator> ()); // no anims
    
        minusTransform.gameObject.SetActive(false);
    }

	public void Run()
	{
		Timer = Time.time;
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

	public IEnumerator ChangeValueTo(int value, SpecialAttribute attribute)
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
        Specialities = new List<Speciality>();

		TxtMesh = GetComponent<TextMesh> ();
		Animator = GetComponent<Animator> ();
		VisualTransform = gameObject.transform.FindChild ("Visual");
	}

	private void Start ()
	{}

	private void Update () 
	{
        Specialities.ForEach(x=>x.UpdateMe());
    }

	private void OnMouseDown()
	{
		DoOnClick ();
	}

}
