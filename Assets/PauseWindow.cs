using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseWindow : Window
{
    [SerializeField]
    public Button BtnMenu;

    [SerializeField]
    public Button BtnRestart;

    [SerializeField]
    public Button BtnContinue;

    private Param Parameters { get; set; }

    public class Param
    {
       
    }

    protected override void OnInit(object param)
    {
        Parameters = param as Param;
    }
}
