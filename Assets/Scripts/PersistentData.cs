using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PersistentObject : MonoBehaviour
{
	public object Data { get; private set; }

	public static PersistentObject Create(object data)
	{
		var go = new GameObject("PersistentObj_" + data.ToString());
		var pObj = go.AddComponent<PersistentObject> ();
		pObj.Data = data;

		return pObj;
	}

	private void Awake()
	{
		DontDestroyOnLoad (gameObject);
	}
}


public class PersistentData : MonoBehaviour
{
	private List<PersistentObject> List { get; set; }

	void Awake()
	{
		DontDestroyOnLoad (gameObject);
		List = new List<PersistentObject> ();
	}

	public void Push(object data)
	{
		var pObj = PersistentObject.Create (data);

		List.Add (pObj);
	}

	public T Pop<T>() where T : class
	{
		var pObj = List.Find (x => x.Data.GetType () == typeof(T));

		List.Remove (pObj);

		var data = pObj.Data;

		Destroy (pObj.gameObject);

		return data as T;
	}

}