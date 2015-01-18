using UnityEngine;
using System.Collections;

public class TextMeshLink : MonoBehaviour
{
	[SerializeField]
	TextMesh mainTextMesh;

	[SerializeField]
	TextMesh receiverTextMesh;

	void Start () 
	{}
	
	void Update ()
	{
		if (mainTextMesh.text != receiverTextMesh.text)
		{
			receiverTextMesh.text = mainTextMesh.text;
		}
	}
}
