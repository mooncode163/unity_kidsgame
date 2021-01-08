using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMoreAppCellItem : UICellItemBase
{
    public RawImage imageItem;
    public Text textTitle;
    public Text textDetail;
    public UIViewLoading viewLoading;
    // Use this for initialization
    private string strAppUrl;
    bool isHideChild;

    public override void UpdateItem(List<object> list)
    {
        ItemInfo info = list[index] as ItemInfo;
        textTitle.text = info.title;
        textDetail.text = info.description;
        strAppUrl = info.url;

        textTitle.gameObject.SetActive(false);
        textDetail.gameObject.SetActive(false);

        StartParsePic(info.pic);

    }
    public override bool IsLock()
    {
        return false;//imageBgLock.gameObject.activeSelf;
    }


    void StartParsePic(string pic)
    {
        HttpRequest http = new HttpRequest(OnHttpRequestFinished);
        http.Get(pic);
        viewLoading.Show(true);
        if (http.isReadFromCatch)
        {
            viewLoading.Show(false);
        }
    }


    void OnHttpRequestFinished(HttpRequest req, bool isSuccess, byte[] data)
    {
        Debug.Log("MoreAppParser OnHttpRequestFinished:isSuccess=" + isSuccess);
        //  return;
        if (isSuccess)
        {
            if (!MoreViewController.main.isActive)
            {
                return;
            }
            Texture2D tex = LoadTexture.LoadFromData(data);
            TextureUtil.UpdateRawImageTexture(imageItem, tex, true);
            if (!req.isReadFromCatch)
            {
                //imageItem.GetComponent<Animation>().Play();
            }

            RectTransform rctran = imageItem.GetComponent<RectTransform>();
            float w = imageItem.texture.width;//rectTransform.rect.width;
            float h = imageItem.texture.height;//rectTransform.rect.height;

            RectTransform rctranCellItem = this.gameObject.GetComponent<RectTransform>();
            float scale = Common.GetBestFitScale(w, h, rctranCellItem.rect.width, rctranCellItem.rect.height);
            imageItem.transform.localScale = new Vector3(scale, scale, 1.0f);

            viewLoading.Show(false);

            AnimateCommon.main.RunFlipH(imageItem.gameObject,imageItem.gameObject,0.15f);
        }
        else
        {

        }
    }
}
