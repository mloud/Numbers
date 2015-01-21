using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PatternToTutorial : MonoBehaviour 
{
    [System.Serializable]
    public class Link
    {
        public string PatternName;
        public GameObject TutorialPrefab;
    }

    public List<Link> Links;

}
