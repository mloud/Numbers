using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class CircleController : MonoBehaviour
{
    public CircleModel Model { get; private set; }
    public CircleVisual Visual { get; private set; }

    public Action<CircleController> OnClick;


    private float Timer { get; set; }


    public void AddSpeciality(Speciality spec)
    {
        Model.AddSpeciality(spec);
        Visual.AddSpeciality(spec);
    }

    public void RemoveSpeciality(Speciality spec)
    {
        Model.RemoveSpeciality(spec);
        Visual.RemoveSpeciality(spec);
    }


    public CircleController Clone()
    {
        CircleController copy = (GameObject.Instantiate(this.gameObject) as GameObject).GetComponent<CircleController>();
        //copy.Model = this.Model.Clone();
        //copy.Visual = this.Visual.Clone();

        return copy;
    }

    public void SetForHud()
    {
        Visual.SetForHud();
    }

    public void Run()
    {
        Timer = Time.time;
    }

    public void SetScale(float scale)
    {
        Visual.SetScale(scale);
    }

    public void SetPosition(Vector3 pos)
    {
        Model.Position = pos;
    }

    public void SetLocalPosition(Vector3 locPos)
    {
        Model.LocalPosition = locPos;
    }


    public void MoveTo(Vector3 locPos, float speed)
    {
        StartCoroutine(MoveToCoroutine(locPos, speed));
    }

    public IEnumerator MoveToCoroutine(Vector3 locPos, float speed)
    {
        Vector3 dirN = (locPos - Visual.LocalPosition).normalized;
        Vector3 startLocPos = Visual.LocalPosition;
        float totDistanceSqr = (locPos - Visual.LocalPosition).sqrMagnitude;

        float t = 0;

        while (true)
        {
            Visual.LocalPosition = Visual.LocalPosition + dirN * speed * Time.deltaTime;
           
            t = (Visual.LocalPosition - startLocPos).sqrMagnitude / totDistanceSqr;
            t = Mathf.Clamp01(t);

            if (t < 1)
                yield return 0;

            else
                break;
        }
        Visual.LocalPosition = locPos;
        //Model.LocalPosition = locPos;
    }

    public void Flip(bool away, float time)
    {
        Visual.StartCoroutine(Visual.FlipCoroutine(away, time));
    }

    public void SetValue(int value)
    {
        Model.Value = value;
    }

    public void ChangeValueTo(int value)
    {
        StartCoroutine(ChangeValueToCoroutine(value));
    }

    public void ShowProgressBar(bool show)
    {
        Visual.progressBar.gameObject.SetActive(show);
    }

    public void SetProgress(float progress)
    {
        Visual.SetProgress(progress);
    }
    

    private IEnumerator ChangeValueToCoroutine(int value)
    {
        yield return Visual.StartCoroutine(Visual.FlipCoroutine(true, 0.3f));

        Model.Value = value;

        yield return Visual.StartCoroutine(Visual.FlipCoroutine(false, 0.3f));
    }


    private void Awake()
    {
        Init();
    }

    private void Start()
    { }

    private void Update()
    {
        Model.Update();
    }


    private void Init()
    {
        Visual = GetComponent<CircleVisual>();
        Model = GetComponent<CircleModel>();

        // Radius
        Model.Radius = 0.4f;

        Model.PositionChanged += Visual.OnPositionChanged;
        Model.LocalPositionChanged += Visual.OnLocalPositionChanged;
        Model.RadiusChanged += Visual.OnRadiusChanged;
        Model.ValueChanged += Visual.OnValueChanged;

        // Clicks
        Visual.OnClick = OnClickInternal;
    }


    private void SetAlpha(float alpha)
    {
        Visual.SetAlpha(alpha);
    }

    public void Enable(bool enable, float time)
    {
        Visual.StartCoroutine(Visual.FlipCoroutine(!enable, time));
    }


    private void OnClickInternal(CircleVisual circleVisual)
    {
        if (OnClick != null)
            OnClick(this);
    }

}
