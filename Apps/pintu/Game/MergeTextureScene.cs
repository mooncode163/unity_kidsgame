using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MergeTextureScene : MonoBehaviour
{
    public Text textTitle;
    UIGamePintu gamePintu;
    Texture2D texGameBg;
    Texture2D texGamePic;

    int indexMerge;
    int indexPlace;
    int totalPlace;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        indexMerge = 0;
        indexPlace = 0;
        gamePintu = new UIGamePintu();
        texGameBg = UIGamePintu.texBgGamePic;

        totalPlace = LevelManager.main.placeTotal;
        LevelManager.main.placeLevel = 0;
        LevelManager.main.ParseGuanka();
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void MergeItem(ItemInfo info)
    {
        Texture2D tex = LoadTexture.LoadFromAsset(info.pic);
        if (tex.format == TextureFormat.RGB24)
        {
            Debug.Log("MergeItem:24bit pic no need to merge :" + info.pic);
            return;
        }
        texGamePic = PintuUtil.MergeTextureGPU(texGameBg, tex);

        byte[] bytes = texGamePic.EncodeToPNG();
        string filepath = Application.streamingAssetsPath + "/" + info.pic;
        System.IO.File.WriteAllBytes(filepath, bytes);
    }
    public void OnClickBtnMerge()
    {
        DoMerge();
    }


    void DoMerge()
    {
        // Debug.Log("DoFillColor start");

        int idx = 0;
        // foreach (ItemInfo info in listGuanka)
        if (indexMerge < GameLevelParse.main.listGuanka.Count)
        {
            textTitle.text = "place= " + indexPlace + " ,indexMerge= " + indexMerge;
            ItemInfo info = GameLevelParse.main.listGuanka[indexMerge] as ItemInfo;
            long tickmerge = Common.GetCurrentTimeMs();
            MergeItem(info);
            tickmerge = Common.GetCurrentTimeMs() - tickmerge;
            Debug.Log("place= " + indexPlace + " ,indexMerge= " + indexMerge + " tickmerge = " + tickmerge + " ms");
            indexMerge++;

            Invoke("DoMerge", 0.5f);
            //Debug.Log("Invoke DoFillColor end");
        }
        else
        {

            GotoNextPlace();
        }

        // Debug.Log("DoFillColor end");
    }


    void GotoNextPlace()
    {
        indexMerge = 0;
        LevelManager.main.placeLevel++;
        indexPlace = LevelManager.main.placeLevel;
        if (LevelManager.main.placeLevel >= totalPlace)
        {
            textTitle.text = "All  Image Merge has finished!";
            Debug.Log(textTitle.text);
            return;
        }
        LevelManager.main.ParseGuanka();

        DoMerge();

    }
}
