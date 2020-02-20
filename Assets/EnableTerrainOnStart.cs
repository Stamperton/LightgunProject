using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableTerrainOnStart : MonoBehaviour
{
    public GameObject[] environment;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < environment.Length; i++)
        {
            environment[i].SetActive(true);
        }
    }


}
