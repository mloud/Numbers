using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class CircleModel : MonoBehaviour
{

    public Action<Vector3> PositionChanged;
    public Action<Vector3> LocalPositionChanged;
    public Action<float> RadiusChanged;
    public Action<int> ValueChanged;

    public List<Speciality> Specialities { get; private set; }

    public int Value { get { return value; } set { this.value = value; if (ValueChanged != null) ValueChanged(value);  } }

    public float Radius { get { return radius; } set { radius = value; if (RadiusChanged != null) RadiusChanged(value); } }

    public Vector3 Position { get { return position; } set { position = value; if (PositionChanged != null) PositionChanged(position);  } }

    public Vector3 LocalPosition { get { return locPosition; }  set { locPosition = value;  if (LocalPositionChanged != null) LocalPositionChanged(position); } } 

	private int value;

    private Vector3 position;

    private Vector3 locPosition;

    private float radius;


    public void Awake()
    {
        Specialities = new List<Speciality>();
    }

    public void AddSpeciality(Speciality spec)
    {
        Specialities.Add(spec);
    }

    public void RemoveSpeciality(Speciality spec)
    {
        Specialities.Remove(spec);
    }


    public CircleModel Clone()
    {

        CircleModel model = (GameObject.Instantiate(this.gameObject) as GameObject).GetComponent<CircleModel>();

        model.position = this.position;
        model.locPosition = this.locPosition;
        model.Value = this.Value;
        model.Radius = this.Radius;

        return model;
    }


	public void Update () 
	{
        Specialities.ForEach(x=>x.UpdateMe());
    }

}
