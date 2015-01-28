using System;
using System.Collections.Generic;
using UnityEngine;

public class ColorDef : MonoBehaviour
{
    public enum ColorType
    {
        SlotLight,
        SlotDark,
        PogRed,
        PogWhite,
        PogPink,
        PogGreen
    }

   
    [System.Serializable]
    public class ColorRec
    {
        public ColorType Type;
        public Color Color;
    }

    public List<ColorRec> Colors;
}