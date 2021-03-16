
using UnityEngine;
using System.Collections;
using Tacticsoft;
using UnityEngine.UI;
using System.Collections.Generic;

//Inherit from TableViewCell instead of MonoBehavior to use the GameObject
//containing this component as a cell in a TableView
public class UINetImageCellItem : UICellItemBase
{
    public const int TAG_IMAGE_SORT = 0;
    public const int TAG_IMAGE_LIST = 1;
    //public Image imageBg;
    public UIImage imagePic;
    public RawImage rawImagePic;
    public UIText textTitle;
    public UIViewLoading viewLoading;

    Texture2D texPic;

    void OnDestroy()
    {
        Debug.Log("OnDestroy:UINetImageCellItem");
    }
    public override void UpdateItem(List<object> list)
    {
        // return;
        if (index < list.Count)
        {
            ImageItemInfo info = list[index] as ImageItemInfo;


            if (tagValue == TAG_IMAGE_SORT)
            {
                rawImagePic.gameObject.SetActive(false);
                imagePic.gameObject.SetActive(true);

                textTitle.gameObject.SetActive(true);
                textTitle.text = info.title;
                Vector4 border = AppRes.borderCellSettingBg;
                // TextureUtil.UpdateImageTexture(imagePic, UISettingCellItem.strImageBg[index % 3], false, border);
                imagePic.UpdateImageByKey(UISettingCellItem.strImageBg[index % 3]);
                //TextureUtil.UpdateRawImageTexture(rawImagePic, UISettingCellItem.strImageBg[index % 3], true);

                LayOut();
            }
            else
            {
                imagePic.gameObject.SetActive(false);

                rawImagePic.gameObject.SetActive(false);
                textTitle.gameObject.SetActive(false);
                StartParsePic(info.pic);
            }


        }
    }
    public override void LayOut()
    {
        if (tagValue == TAG_IMAGE_LIST)
        {
            RectTransform rctran = rawImagePic.GetComponent<RectTransform>();
            float ratio = 0.9f;
            float scale = Common.GetBestFitScale(rctran.rect.width, rctran.rect.height, width, height) * ratio;
            rawImagePic.transform.localScale = new Vector3(scale, scale, 1.0f);
        }
    }

    void StartParsePic(string pic)
    {
        if (Common.BlankString(pic))
        {
            return;
        }
        HttpRequest http = new HttpRequest(OnHttpRequestFinished);
        Debug.Log("StartParsePic pic=" + pic);
        http.Get(pic);
        viewLoading.Show(true);
        if (http.isReadFromCatch)
        {
            viewLoading.Show(false);
        }
    }

    void ActionFinish(object obj, int w, int h)
    {
        byte[] data = obj as byte[];
        if (data == null)
        {
            return;
        }

        texPic = LoadTexture.LoadFromRGBData(data, w, h);
        if (texPic != null)
        {
            Loom.QueueOnMainThread(() =>
            {
                UpdateItemTexture(texPic);
            });
        }
    }

    void UpdateItemTexture(Texture2D tex)
    {
        if (!NetImageListViewController.main.isActive)
        {
            //return;
        }
        // TextureUtil.UpdateImageTexture(imagePic, tex, true); 
        TextureUtil.UpdateRawImageTexture(rawImagePic, tex, true);

        LayOut();

        rawImagePic.gameObject.SetActive(true);
    }


    IEnumerator OnUpdateItemImageEnumerator(byte[] data)
    {
        float time = 0;
        //yield return null;
        yield return new WaitForSeconds(time);
        UpdateItemTexture(LoadTexture.LoadFromData(data));
    }

    void OnHttpRequestFinished(HttpRequest req, bool isSuccess, byte[] data)
    {
        Debug.Log("MoreAppParser OnHttpRequestFinished:isSuccess=" + isSuccess);
        viewLoading.Show(false);
        if (!NetImageListViewController.main.isActive)
        {
            //   return;
        }
        if (isSuccess)
        {
            // if (Common.isAndroid)
            // {
            //     TextureThread.main.LoadTexThread(data, ActionFinish);
            // }
            // else
            {
                // StartCoroutine(OnUpdateItemImageEnumerator(data)); 
                UpdateItemTexture(LoadTexture.LoadFromData(data));
            }

        }
        else
        {

        }
    }
}

