
using UnityEngine;
using System.Collections;
using Tacticsoft;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

//Inherit from TableViewCell instead of MonoBehavior to use the GameObject
//containing this component as a cell in a TableView
public class UISettingCellItem : UICellItemBase
{
    public UIText textTitle;
    public UIImage imageBg;
    public UIImage imageArrow;
    public UIButton btnSwitch;
    static public string[] strImageBg = { "IMAGE_CELL_BG_BLUE", "IMAGE_CELL_BG_ORINGE", "IMAGE_CELL_BG_YELLOW" };
    void Start()
    {
        this.LayOut();
        Invoke("LayOut", 0.1f);
    }
    public override void UpdateItem(List<object> list)
    {
        if (index < list.Count)
        {
            ItemInfo info = list[index] as ItemInfo;
            textTitle.text = info.title;
            tagValue = info.tag;
            ShowSwitchButton(IsShowSwitchButton(info));
            if (info.tag == (int)SettingItemTag.TAG_SETTING_BACKGROUND_MUSIC)
            {
                bool ret = Common.GetBool(AppString.STR_KEY_BACKGROUND_MUSIC);
                UpdateBtnSwitch(ret);
            }
            if (info.tag == (int)SettingItemTag.TAG_SETTING_SOUND)
            {
                bool ret = Common.GetBool(AppString.KEY_ENABLE_PLAYSOUND);
                UpdateBtnSwitch(ret);
            }
            string key = strImageBg[index % 3];
            imageBg.keyImage = key;
            imageBg.UpdateImageByKey(key);

        }
        this.LayOut();
    }
    bool IsShowSwitchButton(ItemInfo info)
    {
        bool ret = false;
        if (info.tag == (int)SettingItemTag.TAG_SETTING_BACKGROUND_MUSIC)
        {
            ret = true;
        }
        if (info.tag == (int)SettingItemTag.TAG_SETTING_SOUND)
        {
            ret = true;
        }

        return ret;
    }

    void ShowSwitchButton(bool isShow)
    {
        btnSwitch.gameObject.SetActive(isShow);
    }
    void UpdateBtnSwitch(bool isSel)
    {
        // Image img = btnSwitch.GetComponent<Image>();
        // if (isSel)
        // {
        //     img.sprite = TextureUtil.CreateSpriteFromTex(TextureCache.main.Load(IMAGE_BTN_SWITCH_SEL));
        // }
        // else
        // {
        //     img.sprite = TextureUtil.CreateSpriteFromTex(TextureCache.main.Load(IMAGE_BTN_SWITCH_UNSEL));
        // }
        btnSwitch.UpdateSwitch(isSel);
    }
    public void CellSwitchDidClick()
    {
        Debug.Log("CellSwitchDidClick");
        if (tagValue == (int)SettingItemTag.TAG_SETTING_BACKGROUND_MUSIC)
        {
            bool ret = Common.GetBool(AppString.STR_KEY_BACKGROUND_MUSIC);
            bool value = !ret;
            Common.SetBool(AppString.STR_KEY_BACKGROUND_MUSIC, value);
            UpdateBtnSwitch(value);
            if (value)
            {
                MusicBgPlay.main.PlayMusicBg();
            }
            else
            {
                MusicBgPlay.main.Stop();
            }


        }
        if (tagValue == (int)SettingItemTag.TAG_SETTING_SOUND)
        {
            bool ret = Common.GetBool(AppString.KEY_ENABLE_PLAYSOUND);
            bool value = !ret;
            Common.SetBool(AppString.KEY_ENABLE_PLAYSOUND, value);
            UpdateBtnSwitch(value);
        }

    }

    public override void LayOut()
    {
        base.LayOut();
    }
}

