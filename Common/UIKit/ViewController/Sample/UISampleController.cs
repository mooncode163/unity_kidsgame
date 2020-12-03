using System.Collections;
using System.Collections.Generic; 
using UnityEngine; 
using UnityEngine.UI;
 
public class UISampleController : UIView
{
    /// <summary>
    /// Unity's Awake method.
    /// </summary>
    public void Awake()
    {
        base.Awake();
        LoadPrefab();
    }

    /// <summary>
    /// Unity's Start method.
    /// </summary>
    public void Start()
    {
        base.Start();

        LayOut();
        OnUIDidFinish();
    }
    void LoadPrefab()
    {
       
    }
    public override void LayOut()
    {
        base.LayOut();
    }


}
