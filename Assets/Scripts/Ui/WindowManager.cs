﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class WindowManager : MonoBehaviour 
{
	private List<Window> Windows { get; set; }

    private List<Window> ClosingWindows { get; set; }
    private List<Window> WindowsToOpen { get; set; }

	private void Awake()
	{
		Windows = new List<Window>();
        ClosingWindows = new List<Window>();
        WindowsToOpen = new List<Window>();

		DontDestroyOnLoad (gameObject);
	}



   

    public Window OpenWindow(string name, object param)
	{
		var window = CreateWindow (name);

        window.OpenFinished += this.OnWindowOpen;
        window.CloseFinished += this.OnWindowClosed;

		window.Init (param);
	
		Windows.Add (window);

      
        if (ClosingWindows.Count > 0)
        {
            WindowsToOpen.Add(window);
        }
        else
        {
            window.Open();
        }

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

            ClosingWindows.Add(win);
		}
	}

	public IEnumerator ShowFadeCoroutine(bool fadeOut, float time)
	{
		var fade = (Instantiate(Resources.Load<GameObject>("Prefabs/Ui/Fade")) as GameObject).GetComponent<FadeOverlay>();
		fade.transform.SetParent(GameObject.FindObjectOfType<Canvas>().transform);
		fade.transform.localScale = Vector3.one;
		(fade.transform as RectTransform).offsetMin = Vector2.zero;
		(fade.transform as RectTransform).offsetMax = Vector2.zero;

		yield return StartCoroutine(fade.FadeCoroutine(fadeOut, time));
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

        winGo.name = name;

		return win;

	}



    private void OnWindowClosed(Window window)
    {
        ClosingWindows.Remove(window);

        Destroy(window.gameObject);

        if (WindowsToOpen.Count > 0)
        {
            WindowsToOpen[0].Open();
            WindowsToOpen.RemoveAt(0);
        }
    }

    private void OnWindowOpen(Window window)
    {
        if (WindowsToOpen.Count > 0)
        {
            WindowsToOpen[0].Open();
            WindowsToOpen.RemoveAt(0);
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        Windows.Clear();
		ClosingWindows.Clear();
		WindowsToOpen.Clear();
    }
}
