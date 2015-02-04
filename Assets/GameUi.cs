using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameUi : MonoBehaviour 
{
    public static GameUi Instance { get; private set; }

    [SerializeField]
    public Button btnPause;

    [SerializeField]
    public Button btnHelp;

    void Awake()
    {
        Instance = this;
    }

	void Update () 
    {}
}
