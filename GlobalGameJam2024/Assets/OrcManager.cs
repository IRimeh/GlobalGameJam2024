using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcManager : MonoBehaviour
{

    List<Orc> orcList;
    // Start is called before the first frame update
    void Start()
    {
        orcList = new List<Orc>(FindObjectsOfType<Orc>());
    }

    
}
