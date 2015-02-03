using UnityEngine;

//[ExecuteInEditMode]
public class AlphaCutOff : MonoBehaviour
{
	[SerializeField]
	private float CutOff = 0.5f;

    [SerializeField]
    private Color color = Color.white;

	public void SetCutOff(float value)
	{
		CutOff = Mathf.Clamp (value, 0.001f, 1f);
	}

    public void SetColor(Color color)
    {
        renderer.sharedMaterial.SetColor("_Color", color);
        renderer.sharedMaterial.SetColor("_SpecColor", color);
        renderer.sharedMaterial.SetColor("_Emission", color);
    }

    public void Awake()
    {
        
    }

    private void Update()
    {
        renderer.sharedMaterial.SetFloat("_Cutoff", CutOff);
    }
}