using System.Collections;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;

public class UICellCover : QuadCell
{
    public TextMesh textTitle;
    public override string NibName
    {
        get
        {
            return "UICellCover";
        }
    }
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    public void Awake()
    {

    }
    // Use this for initialization
    public void Start()
    {

    }


    public override void UpdateItem()
    {
        base.UpdateItem();
        textTitle.text = index.ToString();
    }

}
