using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class InfoWindow : Window
{
    public Action<InfoWindow> OnCloseAction;

    [SerializeField]
    private Button btnCancel;

    [SerializeField]
    private Text txtTitle;

    [SerializeField]
    private Text txtText;


    private Param Parameters { get; set; }

    public class Param
    {
        public string Title;
        public string Text;
    }


    protected override void OnInit(object param)
    {
        Parameters = param as Param;

        txtTitle.text = Parameters.Title;
        txtText.text = Parameters.Text;

        btnCancel.onClick.AddListener(OnClose);
  
    }

    private void OnClose()
    {
        
        if (OnCloseAction != null)
            OnCloseAction(this);


        App.Instance.WindowManager.CloseWindow(Name);
    }

}
