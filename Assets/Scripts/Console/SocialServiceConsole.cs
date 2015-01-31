using System.Collections.Generic;
using UnityEngine;


public class SocialServiceConsole : MonoBehaviour
{
	public bool Enabled { get { return show; } }

	bool show;
	bool collapse;
	
	const int margin = 20;
	const int marginTop = 50;
	
	Rect windowRect = new Rect(margin, marginTop, Screen.width - (margin * 2), Screen.height - (margin + marginTop));
	Rect titleBarRect = new Rect(0, 0, 10000, 20);

	private Vector2 scrollPosition;

	public void Show(bool show)
	{
		this.show = show; 
	}


	void OnGUI ()
	{
		if (!show) {
			return;
		}
		
		windowRect = GUILayout.Window(123456, windowRect, ConsoleWindow, "Social service");
	}
	
	void ConsoleWindow (int windowID)
	{
		scrollPosition = GUILayout.BeginScrollView(scrollPosition);

		if (GUILayout.Button("Log In")) 
		{
			App.Instance.SocialService.Login(null);
		}

		if (GUILayout.Button("Log Out")) 
		{
			App.Instance.SocialService.SignOut();
		}

		if (GUILayout.Button("Show Leaderboards")) 
		{
			App.Instance.SocialService.ShowLeaderBoard();
		}

		if (GUILayout.Button("Show Achievements")) 
		{
			App.Instance.SocialService.ShowAchievements();
		}



		GUILayout.EndScrollView();


		// Allow the window to be dragged by its title bar.
		GUI.DragWindow(titleBarRect);
	}
}