using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Moonma.IAP;

public class IAPCommon : MonoBehaviour
{

    public static IAPCommon main;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (main == null)
        {
            main = this;
        }

        //IAP 初始化
        IAP iap = IAP.main;

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
