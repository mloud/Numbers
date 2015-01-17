using UnityEngine;

[ExecuteInEditMode]
public class AlphaCutOff : MonoBehaviour
{
	[SerializeField]
	private float CutOff = 0.5f;

	public void SetCutOff(float value)
	{
		CutOff = Mathf.Clamp (value, 0.001f, 1f);
	}

	private void Update () 
	{ 
		renderer.sharedMaterial.SetFloat("_Cutoff", CutOff); 
	}
}