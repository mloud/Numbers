using UnityEngine;
using System.Collections;

public class RootConsole: MonoBehaviour 
{

	private void OnGUI ()
	{
		GUILayout.BeginHorizontal();

		if (GUILayout.Button("Console",  GUILayout.Width(100), GUILayout.Height(25))) 
		{
			App.Instance.Console.Show(!App.Instance.Console.Enabled);
		}

        if (GUILayout.Button("GooglePlay", GUILayout.Width(100), GUILayout.Height(25)))
        {
            App.Instance.SocialConsole.Show(!App.Instance.SocialConsole.Enabled);
        }

        if (GUILayout.Button("DownloadResources", GUILayout.Width(100), GUILayout.Height(25)))
        {
            App.Instance.DownloadResources();
        }


		GUILayout.EndHorizontal();

	}

}
