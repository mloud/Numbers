using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class WindowManager : MonoBehaviour 
{
	private List<Window> Windows { get; set; }

	private void Awake()
	{
		Windows = new List<Window>();

		DontDestroyOnLoad (gameObject);
	}

	public Window OpenWindow(string name, object param)
	{
		var window = CreateWindow (name);

		window.Init (param);
	
		Windows.Add (window);
	
		return window;
	}

	public void CloseWindow(string name)
	{
		if (IsOpen(name))
		{
			int index = Windows.FindIndex(x=>x.Name == name);

			var win = Windows[index];

			Windows.RemoveAt(index);

			win.Close();

			Destroy (win.gameObject);
		}
	}

	public bool IsOpen(string name)
	{
		return Windows.Find (x => x.Name == name) != null;
	}

	private Window CreateWindow(string name)
	{
		var winGo = Instantiate (Resources.Load ("Prefabs/Ui/" + name)) as GameObject;

		var win = winGo.GetComponent<Window> ();

		if (win == null)
		{
			win = winGo.AddComponent<Window>();
		}

		winGo.transform.SetParent (GameObject.FindObjectOfType<Canvas> ().transform);

		return win;

	}


    private void OnLevelWasLoaded(int level)
    {
        Windows.Clear();
    }
}
