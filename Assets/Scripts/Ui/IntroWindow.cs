using UnityEngine;
using System.Collections;

public class IntroWindow : Window 
{
    private Param Parameters { get; set; }

    public class Param
    {}

    protected override void OnInit(object param)
    {
        Parameters = param as Param;
    }
}
