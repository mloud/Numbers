using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameUi : MonoBehaviour
{
    [SerializeField]
    public Button btnHelp;

    [SerializeField]
    public Button btnPause;

    public static GameUi Instance { get; private set; }

    private Animator Animator { get; set; }

	void Awake () 
    {
        Instance = this;
        Animator = GetComponent<Animator>();
    }
	
	

    public void ShowButtons()
    {
        Animator.SetTrigger("Show"); 
    }

    public void HideButtons()
    {
        Animator.SetTrigger("Hide"); 
    }
    

}
