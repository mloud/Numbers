using UnityEngine;
using System.Collections;

public class ColorManager : MonoBehaviour 
{
    private ColorDef ColorDef { get; set; }

    private void Awake()
    {
   
        // find color def 
        ColorDef = Resources.Load<GameObject>("Prefabs/ColorDef").GetComponent<ColorDef>();
    }

    public Color GetColor(ColorDef.ColorType colorType)
    {
        return ColorDef.Colors.Find(x => x.Type == colorType).Color;
    }
}
