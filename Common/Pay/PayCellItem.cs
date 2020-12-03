using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PayCellItem : MonoBehaviour
{
    public GameObject objContent;
    public Image imageItem;
    public Text textItem;
    public int index;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Hide(bool isHide)
    {
        objContent.gameObject.SetActive(!isHide);

    }
    public void UpdateInfo(ItemInfo info)
    {
        textItem.text = info.title;
        Texture2D tex = LoadTexture.LoadFromResource(info.pic);

        imageItem.sprite = TextureUtil.CreateSpriteFromTex(tex);

        RectTransform rctran = imageItem.GetComponent<RectTransform>();
        float w = imageItem.sprite.texture.width;//rectTransform.rect.width;
        float h = imageItem.sprite.texture.height;//rectTransform.rect.height;
                                                  // print("imageItem size:w=" + w + " h=" + h);
        rctran.sizeDelta = new Vector2(w, h);

        RectTransform rctranText = textItem.GetComponent<RectTransform>();
        RectTransform rctranContent = objContent.GetComponent<RectTransform>();
        float oft_y = rctranText.rect.height;
        float scalex = rctranContent.rect.width / w;
        float scaley = (rctranContent.rect.height - oft_y) / h;
        float scale = Mathf.Min(scalex, scaley);
        imageItem.transform.localScale = new Vector3(scale, scale, 1.0f);
        float x = 0;
        float y = oft_y / 2;

        rctran.anchoredPosition = new Vector2(x, y);

    }
    public void OnItemClick()
    {

    }
}
