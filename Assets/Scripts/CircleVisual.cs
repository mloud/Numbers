using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;



public class CircleVisual : MonoBehaviour
{
   
    [SerializeField]
    private SpriteRenderer visualSpriteRenderer;

    [SerializeField]
    private Transform minusTransform;

    [SerializeField]
    private Transform renewTransform;

    [SerializeField]
    private Transform specialityTransform;

    [SerializeField]
    private Transform smallFragmentTransform;

    [SerializeField]
    private Transform outerTransform;

    [SerializeField]
    public AlphaCutOff progressBar;

    [SerializeField]
    public List<SpriteRenderer> CircleColoredFragments;

    [SerializeField]
    public ColorDef.ColorType color;


    public Vector3 Position { set { transform.position = value; } }
    public Vector3 LocalPosition { get { return transform.localPosition; } set { transform.localPosition = value; } }
    public float Radius { set { (collider2D as CircleCollider2D).radius = value; } }

    public Action<CircleVisual> OnClick;

    public Transform VisualTransform { get; set; }
   
    private List<SpriteRenderer> SpriteRenderers { get; set; }
    private List<TextMesh> TextMeshes { get; set; }
	

	private CircleCollider2D cCollider;
	
	private TextMesh TxtMesh { get; set; }

	private Animator Animator { get; set; }

	private float Timer { get; set; }

    public CircleModel Model { get; set; }

    private void Update()
    {
        OnColorChanged((int)color);
        //SetColor(color);

        bool showSpecContainer = Model.Specialities.FindAll(x => x.HasIcon).Count > 0;

        specialityTransform.gameObject.SetActive(showSpecContainer);
        smallFragmentTransform.gameObject.SetActive(!showSpecContainer);
    }

    public void OnPositionChanged(Vector3 pos)
    {
        Position = pos;
    }

    public void OnLocalPositionChanged(Vector3 locPos)
    {
        LocalPosition = locPos;
    }

    public void OnRadiusChanged(float radius)
    {
        Radius = radius;
    }

    public void OnColorChanged(int colorType)
    {
        Color c = App.Instance.ColorManager.GetColor((ColorDef.ColorType)colorType);
        foreach (var sprRen in CircleColoredFragments)
        {
            sprRen.color = c;
        }

        color = (ColorDef.ColorType)colorType;
    }

    public void AddSpeciality(Speciality spec)
    {
        if ((spec as MinusOneSpeciality) != null)
        {
            minusTransform.gameObject.SetActive(true);
        }
        else if ( (spec as ReappearSpeciality) != null)
        {
            renewTransform.gameObject.SetActive(true);
        }

        
    }

    public void RemoveSpeciality(Speciality spec)
    {
        if ((spec as MinusOneSpeciality) != null)
        {
            minusTransform.gameObject.SetActive(false);
        }
        else if ( (spec as ReappearSpeciality) != null)
        {
            renewTransform.gameObject.SetActive(false);
        }
    }

    public void SetParent(Transform parent)
    {
        transform.SetParent(parent);
    }

    public CircleVisual Clone()
    {
        return (Instantiate(this.gameObject) as GameObject).GetComponent<CircleVisual>();
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
		transform.localScale = new Vector3 (scale, scale, scale);
	}

    public void SetColor(Color color)
    {
        foreach (var sprR in CircleColoredFragments)
        {
            sprR.color = color;
        }
    }


    public IEnumerator FlipCoroutine(bool away, float time, float delay)
    {
        if (delay > 0)
            yield return new WaitForSeconds(delay);

        float startTime = Time.time;
        float flipTime = time;
        float t = 0;

        if (time == 0)
        {
            VisualTransform.localEulerAngles = new Vector3(0, away ? 90 : 0, 0);
        }
        else
        {
            while (true)
            {
                t = Mathf.Clamp((Time.time - startTime) / flipTime, 0, 1);

                float angle = away ? t * 90 : (1 - t) * 90;

                VisualTransform.localEulerAngles = new Vector3(0, angle, 0);
                if (t >= 1)
                    break;

                yield return 0;
            }
        }
    }

   private void DoOnClick()
    {

       // GetComponent<Animator>().SetTrigger("click");
       
       if (OnClick != null)
		{
			OnClick(this);
		}
	}

	private void Awake()
	{
        SpriteRenderers = new List<SpriteRenderer>(gameObject.GetComponentsInChildren<SpriteRenderer>());
        TextMeshes = new List<TextMesh>(gameObject.GetComponentsInChildren<TextMesh>());

        VisualTransform = gameObject.transform.FindChild ("Visual");
		TxtMesh = VisualTransform.GetComponentInChildren<TextMesh> ();
		Animator = GetComponent<Animator> ();


        minusTransform.gameObject.SetActive(false);
        renewTransform.gameObject.SetActive(false);
        progressBar.gameObject.SetActive(false);
	}


	private void OnMouseDown()
	{
		DoOnClick ();
	}

    public void SetProgress(float progress)
    {
        progressBar.SetCutOff(progress);
    }

    public void OnValueChanged(int value)
    {
        TxtMesh.text = value.ToString();
    }

    public void SetAlpha(float alpha)
    {
  
        SpriteRenderers.ForEach(delegate(SpriteRenderer obj)
        {
            var c = obj.color;
            c.a = alpha;
            obj.color = c;
        });

        TextMeshes.ForEach(delegate(TextMesh obj)
                                {
            var c = obj.color;
            c.a = alpha;
            obj.color = c;
        });
    }

    public void Enable(bool enable, float time)
    {
        StartCoroutine(FlipCoroutine(!enable, time, 0));
    }

    private IEnumerator ShowCoroutine(bool show, float time)
    {
        float startTime = Time.time;

        while (true)
        {
            float t = Mathf.Clamp01((Time.time - startTime) / time);
                
            if (show)
                SetAlpha(t);
            else
                SetAlpha(1 - t);

            if (t == 1)
                break;
            else
                yield return 0;
        }


        yield break;   
    }

}
