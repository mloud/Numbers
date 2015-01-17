using UnityEngine;

[ExecuteInEditMode]
public class AlphaCutOff : MonoBehaviour
{
	[SerializeField]
	public float CutOff = 0.5f;

	private void Update () 
	{ 
		CutOff = Mathf.Clamp (CutOff, 0.01f, 1f);
		renderer.sharedMaterial.SetFloat("_Cutoff", CutOff); 
	}
}