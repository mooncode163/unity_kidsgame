using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//图片帧动画
public class ActionImage : ActionBase
{
    public int count = 20;
    int count_step = 0;
    List<string> listPic;

    float deltaTime = 0;
    float animationTimeDelay = 0;
    public void AddPic(string pic)
    {
        listPic.Add(pic);
    }

    void UpdateImage()
    {
        if (index < listPic.Count)
        {

            string pic = listPic[index];
            // Debug.Log("UpdateImage:index=" + index + " listPic.Count=" + listPic.Count + " pic=" + pic);
            Texture2D tex = TextureCache.main.Load(pic);
            if (tex == null)
            {
                Debug.Log("UpdateImage:index=" + index + " tex=null");
            }
            SpriteRenderer rd = this.gameObject.GetComponent<SpriteRenderer>();
            //sprite
            if ((rd != null) && (tex != null))
            {
                //  Debug.Log("UpdateImage:index=" + index + " update sprite");
                rd.sprite = TextureUtil.CreateSpriteFromTex(tex);
            }

            if ((rd == null) && (tex != null))
            {
                Image image = this.gameObject.GetComponent<Image>();
                if (image != null)
                {
                    //image
                    image.sprite = TextureUtil.CreateSpriteFromTex(tex);
                }
                else
                {
                    //rawimage
                    RawImage rawimage = this.gameObject.GetComponent<RawImage>();
                    rawimage.texture = tex;
                }
                RectTransform rctran = this.gameObject.GetComponent<RectTransform>();
                rctran.sizeDelta = new Vector2(tex.width, tex.height);
            }

        }
    }
    public override void InitAction()
    {

        listPic = new List<string>();
        index = 0;
        isUpdateByPercent = false;
        percentage = 0;
        deltaTime = 0;
        Debug.Log("ActionImage:InitAction index=" + index);
    }
    public override void UpdateAction()
    {
        if (listPic.Count == 0)
        {
            return;
        }
        animationTimeDelay = duration / listPic.Count;
        deltaTime += Time.deltaTime;
        // Debug.Log(animationDeltaTime);
        if (deltaTime >= animationTimeDelay)
        {
            deltaTime = 0;
            UpdateImage();
            index++;
            // Debug.Log("UpdateImage:index++=" + index);
            if (index >= listPic.Count)
            {
                OnFinish();
                index = 0;
            }
        }


    }

    public override void OnActionComplete()
    {
        if (target != null)
        {
            target.gameObject.SetActive(true);
        }

    }
}
