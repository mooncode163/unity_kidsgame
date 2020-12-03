using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Moonma.Share; 

public class ShareCommon : MonoBehaviour
{

    public static ShareCommon main;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (main == null)
        {
            main = this;
        }

    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

 

}
