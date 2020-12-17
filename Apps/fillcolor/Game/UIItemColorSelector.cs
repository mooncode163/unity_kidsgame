using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public delegate void OnUIItemColorSelectorDidClickDelegate(UIItemColorSelector ui);
public class UIItemColorSelector : MonoBehaviour {
	public Image image;
	public Image imageSel;
	public OnUIItemColorSelectorDidClickDelegate callBackClick{get;set;}
	public int index;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClickItem()
	{
		 if(callBackClick!=null)
        {
            callBackClick(this);
        }
	}
	public void UpdateColor(Color cr)
	{
		image.color = cr;
	}
	public Color GetColor()
	{
		return image.color;
	}
	public void SetSelect(bool isSel)
	{
		imageSel.gameObject.SetActive(isSel);
	}

	public bool IsSelect()
	{
		return imageSel.gameObject.activeSelf;
	}
}
