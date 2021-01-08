using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
public interface IGamePintuWordDelegate
{
    void OnGamePintuWordDidBack(GamePintuWord ui);
    void OnGamePintuWordDidNextLevel(GamePintuWord ui);
    void OnGamePintuWordDidGameWin(GamePintuWord ui);
    void OnGamePintuWordDidGameFail(GamePintuWord ui);
}
public class GamePintuWord : GameBase, IUIPintuBlockWordDelegate
{
    public enum ImageSource
    {
        GAME_INNER,//游戏内置图片
        SYSTEM_IMAGE_LIB,//系统图库
        NET,//web

    }
    List<UIPintuBlockWord> listItem;
    public IGamePintuWordDelegate iDelegate;

    int offsetTopbarY;
    float gameScaleX = 1f;
    float gameScaleY = 1f;
    float gamePicBgScale = 1f;
    float gameRectWidth;
    float itemWidthWorld;
    float itemHeightWorld;

    Vector2 ptDownScreen;
    Vector3 posItemWorld;
    //ItemInfo itemInfoSel;
    bool isItemHasSel;
    bool isGameFail;


    float heightTopbarWorld;
    float heightTabBarWorld;
    float itemPosZ = -1f;

    List<UIPintuBlockWord> listItemLeft;//left and top
    List<UIPintuBlockWord> listItemRight;

    GameObject objGamePicBg;
    Rect rectMain;
    int indexPlay;

    UIWordSVG uiWordSVGPrefab;
    UIWordSVG uiWordSVG;
    Color32 colorFinish = new Color32(34, 172, 56, 255);
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {

        if (uiWordSVGPrefab == null)
        {
            GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/SVG/UIWordSVG");
            if (obj != null)
            {
                uiWordSVGPrefab = obj.GetComponent<UIWordSVG>();
            }
        }


        listItemLeft = new List<UIPintuBlockWord>();
        listItemRight = new List<UIPintuBlockWord>();
        listItem = new List<UIPintuBlockWord>();


        offsetTopbarY = 160;
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        heightTopbarWorld = Common.CanvasToWorldHeight(mainCam, sizeCanvas, offsetTopbarY);


        indexPlay = 0;
    }
    // Use this for initialization
    void Start()
    {
        // ShowGameWin();
    }

    bool IsIndexInList(int[] list, int idx)
    {
        bool ret = false;
        for (int i = 0; i < list.Length; i++)
        {
            if (list[i] == idx)
            {
                ret = true;
                break;
            }
        }
        return ret;
    }

    public int GetCount()
    {
        WordItemInfo infoWord = infoGuankaItem as WordItemInfo;
        int count = Common.String2Int(infoWord.dbInfo.bihuaCount);
        return count;
    }
    public void CreateBlock()
    {
        int count = GetCount();
        Debug.Log("CreateBlock count=" + count);

        for (int i = 0; i < count; i++)
        {
            GameObject obj = new GameObject("block_" + i);
            obj.transform.parent = this.transform;
            //obj.transform.localPosition = new Vector3(0, 0, -1f);
            UIPintuBlockWord uiBlock = obj.AddComponent<UIPintuBlockWord>();
            uiBlock.iDelegate = this;
            uiBlock.index = i;
            listItem.Add(uiBlock);

        }


        //分组
        int size = count;
        int[] indexLeft = Util.main.RandomIndex(size, size / 2);
        listItemLeft.Clear();
        listItemRight.Clear();
        for (int i = 0; i < size; i++)
        {
            bool ret = IsIndexInList(indexLeft, i);
            UIPintuBlockWord ui = listItem[i];
            if (ret)
            {
                listItemLeft.Add(ui);
            }
            else
            {
                listItemRight.Add(ui);
            }
        }

    }

    public void UpdateTexture(Texture2D tex)
    {
        UpdateGameImage(tex, false);
        WordItemInfo infoWord = infoGuankaItem as WordItemInfo;
        int i = 0;
        foreach (UIPintuBlockWord ui in listItem)
        {
            string strDirRoot = CloudRes.main.rootPathGameRes +"/image/";
            string pic = strDirRoot + infoWord.id + "/" + infoWord.id + "_" + i.ToString() + ".png";
            //Texture2D texBlock = LoadTexture.LoadFromAsset(pic);
            // ui.UpdateTexture(texBlock);

            string svg = ParseWordPointInfo.main.GetWordSVG(infoWord.pointInfo, i);
            Debug.Log("svg=" + svg);
            ui.LoadWord(svg);

            Renderer rd = ui.uiSVG.gameObject.GetComponent<Renderer>();
            itemWidthWorld = rd.bounds.size.x;
            itemHeightWorld = rd.bounds.size.y;
            BoxCollider box = ui.gameObject.AddComponent<BoxCollider>();
            box.size = rd.bounds.size;
            i++;
        }

        LayOutBlock(true);
    }



    UIPintuBlockWord GetItem(int r, int c)
    {
        UIPintuBlockWord item = null;
        foreach (UIPintuBlockWord ui in listItem)
        {
            if ((ui.indexRow == r) && (ui.indexCol == c))
            {
                item = ui;
                break;
            }
        }
        return item;
    }

    public void LayOut(bool isOption)
    {
        LayOutGamePic();
        LayOutBlock(isOption);
        ShowItemAnimate();
    }

    public void LayOutGamePic()
    {
        float x = 0, y = 0, z = 0, w = 0, h = 0, w_rect = 0, h_rect = 0;
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        heightTopbarWorld = Common.CanvasToWorldHeight(mainCam, sizeCanvas, offsetTopbarY);
        heightTabBarWorld = Common.CanvasToWorldHeight(mainCam, sizeCanvas, offsetTopbarY);

        Vector2 sizeWorld = Vector2.zero;//Common.ScreenToWorldSize(mainCam, new Vector2(pixsel_w, pixsel_h));
        RectTransform rcTran = AppSceneBase.main.GetRectMainWorld();
        sizeWorld.x = rcTran.rect.width;
        sizeWorld.y = rcTran.rect.height - heightTopbarWorld;
        if (!Device.isLandscape)
        {
            sizeWorld.x = rcTran.rect.width * 0.6f;
            sizeWorld.y = rcTran.rect.height - heightTopbarWorld;
        }
        if (uiWordSVG != null)
        {

            // SpriteRenderer spRd = objGamePic.GetComponent<SpriteRenderer>();
            // Sprite sp = spRd.sprite; 
            Texture2D tex = uiWordSVG.GetTexture();
            if (tex != null)
            {

                float pixsel_per_unit = 100;
                float tex_world_w = tex.width / pixsel_per_unit;
                float tex_world_h = tex.height / pixsel_per_unit;

                float pos_y = 0;
                if (Device.isLandscape)
                {
                    w_rect = sizeWorld.x;
                    // h_rect = sizeWorld.y - heightTopbarWorld - Device.offsetBottomWithAdBannerWorld - heightTabBarWorld;//  GameManager.main.heightAdWorld;
                    // pos_y = -heightTopbarWorld / 2 + Device.offsetBottomWithAdBannerWorld / 2 + heightTabBarWorld / 2;
                    h_rect = sizeWorld.y - heightTopbarWorld - heightTabBarWorld;//  GameManager.main.heightAdWorld;
                    pos_y = -heightTopbarWorld / 2 + heightTabBarWorld / 2;
                }
                else
                {
                    w_rect = sizeWorld.x;
                    h_rect = sizeWorld.y - heightTopbarWorld - Device.offsetBottomWithAdBannerWorld - heightTabBarWorld;//  GameManager.main.heightAdWorld;
                    pos_y = -heightTopbarWorld / 2 + Device.offsetBottomWithAdBannerWorld / 2 + heightTabBarWorld / 2;

                }
                float scale = Common.GetBestFitScale(tex_world_w, tex_world_h, w_rect, h_rect);
                gameScaleX = scale;
                gameScaleY = scale;
                //gamePicBgScale = scale;
                z = -1;
                uiWordSVG.transform.localScale = new Vector3(scale, scale, 1f);
                //rcTran.sizeDelta = new Vector2(tex_world_w, tex_world_h);

                uiWordSVG.transform.localPosition = new Vector3(0, pos_y, z);

                if (objGamePicBg != null)
                {
                    objGamePicBg.transform.localScale = new Vector3(scale, scale, 1f);

                    objGamePicBg.transform.localPosition = new Vector3(0, pos_y, z + 1);

                }
                Vector2 bd = uiWordSVG.GetBoundWord();

                w = bd.x;
                h = bd.y;
                x = -w / 2;
                y = pos_y - h / 2;
                Debug.Log("game pic:gamePicBgScale=" + gamePicBgScale + " gameScaleX=" + gameScaleX + " x=" + x + " y=" + y + " w=" + w + " h=" + h);
                rectMain = new Rect(x, y, w, h);

                gameRectWidth = Common.WorldToScreenWidth(mainCam, w);

            }
        }


    }

    public void LayOutBlock(bool isOption)
    {

        foreach (UIPintuBlockWord ui in listItem)
        {
            ui.enableTouch = !isOption;
        }
        if (uiWordSVG != null)
        {
            uiWordSVG.gameObject.SetActive(!isOption);
        }
        //更新位置
        foreach (UIPintuBlockWord ui in listItem)
        {
            ui.transform.localPosition = GetItemPositionNormal(ui.indexRow, ui.indexCol, isOption);
            ui.posNormal = ui.transform.localPosition;

            float scale = 1f;
            if (isOption)
            {
                scale = 0.9f;
                //  ui.centerWidth *= 0.9f;
            }
            scale = gameScaleX;
            ui.transform.localScale = new Vector3(scale, scale, 1f);
            ui.uiSVG.Draw();
        }

    }
    public void ShowGameImage(bool isOption)
    {
    }

    //置顶
    void SetZorderTopMost(UIPintuBlockWord ui)
    {
        foreach (UIPintuBlockWord uilist in listItem)
        {
            Vector3 pos = uilist.transform.localPosition;
            pos.z = itemPosZ;
            if (ui == uilist)
            {
                pos.z = itemPosZ - 1;
            }
            uilist.transform.localPosition = pos;
        }
    }
    Vector3 GetItemPositionNormal(int r, int c, bool isOption)
    {
        float x = 0, y = 0, z = 0, w = 0, h = 0;
        UIViewSVG ui = uiWordSVG.GetItemSVG(0);
        x = ui.transform.position.x;
        y = ui.transform.position.y;
        return new Vector3(x, y, itemPosZ);
    }


    void UpdateGameImage(Texture2D tex, bool isOption)
    {

        float z = 0;
        float x = 0, y = 0, w = 0, h = 0;
        //gameRectWidth = Mathf.Min(Screen.width, Screen.height) - (int)((offsetTopbarY + 128) * AppCommon.scaleBase * 2);
        //gameRectWidth = Mathf.Min(Screen.width, Screen.height - (offsetTopbarY + 128) * AppCommon.scaleBase);

        // if (objGamePic == null)
        {


            uiWordSVG = (UIWordSVG)GameObject.Instantiate(uiWordSVGPrefab);
            UIViewController.ClonePrefabRectTransform(uiWordSVGPrefab.gameObject, uiWordSVG.gameObject);
            // uiWordSVG.transform.SetParent(this.transform, false);
            // objGamePic = new GameObject("game_pic"); 
            AppSceneBase.main.AddObjToMainWorld(uiWordSVG.gameObject);
            uiWordSVG.SetRenderMode(UIViewSVG.RenderMode.WORLD);
            uiWordSVG.colorWord = Color.gray;
            uiWordSVG.type = UIWordSVG.Type.ALL_STROKE;
            uiWordSVG.LoadWord(infoGuankaItem as WordItemInfo);

            //obj.transform.localPosition = new Vector3(0, 0, -1f);
            // UIPintuBlockWord uiBlock = objGamePic.AddComponent<UIPintuBlockWord>();
            // Material mat = new Material(Shader.Find("Custom/Word"));
            // mat.SetColor("_Color", Color.gray);
            // uiBlock.UpdateMaterial(mat);

            // uiBlock.UpdateTexture(tex);

            // WordItemInfo infoWord = infoGuankaItem as WordItemInfo;
            // string svg = ParseWordPointInfo.main.GetWordSVG(infoWord.pointInfo, 0);
            // uiBlock.LoadWord(svg);
            // // 实际显示颜色
            // uiBlock.SetColor(colorWord);



            // spRd.material = new Material(Shader.Find("Custom/Grey"));


            Debug.Log("tex,w:" + tex.width + " h:" + tex.height);
            // Sprite sprite = TextureUtil.CreateSpriteFromTex(tex);
            // sprite.name = "game";
            // spRd.sprite = sprite;

            LayOutGamePic();


        }





    }

    public void UpdateGameImageBg(Texture2D tex)
    {

        float z = 0;
        float x = 0, y = 0, w = 0, h = 0;

        // if (objGamePic == null)
        {
            objGamePicBg = new GameObject("game_pic_bg");
            AppSceneBase.main.AddObjToMainWorld(objGamePicBg);
            //obj.transform.localPosition = new Vector3(0, 0, -1f);
            MeshTexture uiBg = objGamePicBg.AddComponent<MeshTexture>();
            uiBg.UpdateTexture(tex);


            Debug.Log("tex,w:" + tex.width + " h:" + tex.height);
            // Sprite sprite = TextureUtil.CreateSpriteFromTex(tex);
            // sprite.name = "game";
            // spRd.sprite = sprite;

            LayOutGamePic();


        }





    }

    //随机重排数组
    void shuffleArray(List<int> array)
    {
        if (array == null)
        {
            return;
        }

        if (array.Count == 0)
        {
            return;
        }

        for (int i = array.Count - 1; i >= 0; i--)
        {

            if (i != 0)
            {
                int k_random = UnityEngine.Random.Range(0, i) % i;//arc4random()%(i);

                //从剩余数据中随机选取一个
                //int item_random = array[k_random];

                //将随机选取的元素与当前位置元素互换 
                var temp = array[i];
                array[i] = array[k_random];
                array[k_random] = temp;
            }



        }
    }

    public void ShowItemAnimate()
    {

        LayOutBlock(false);
        float x, y, w, h;
        float dur_moveto = 0.8f;
        //left or top
        {
            if (Device.isLandscape)
            {
                //left
                w = Common.GetCameraWorldSizeWidth(mainCam) - Common.ScreenToWorldWidth(mainCam, gameRectWidth) / 2;
                x = -Common.GetCameraWorldSizeWidth(mainCam);
                h = mainCam.orthographicSize * 2 - Device.offsetBottomWithAdBannerWorld - heightTopbarWorld;
                y = -mainCam.orthographicSize;
                y += Device.offsetBottomWithAdBannerWorld;
            }
            else
            {
                //top
                w = Common.GetCameraWorldSizeWidth(mainCam) * 2;
                x = -Common.GetCameraWorldSizeWidth(mainCam);
                y = Common.ScreenToWorldHeight(mainCam, gameRectWidth) / 2;
                h = (mainCam.orthographicSize - heightTopbarWorld) - y;

            }

            Rect rcLeft = new Rect(x, y, w, h);
            LayOut layoutLeft = new LayOut();
            layoutLeft.rect = rcLeft;


            layoutLeft.row = (int)(rcLeft.height / itemHeightWorld);
            if (layoutLeft.row <= 0)
            {
                layoutLeft.row = 1;
            }
            layoutLeft.col = listItemLeft.Count / layoutLeft.row;
            if (listItemLeft.Count % layoutLeft.row != 0)
            {
                layoutLeft.col++;
            }

            Debug.Log("layoutarea rcLeft=" + rcLeft + " row=" + layoutLeft.row + " col=" + layoutLeft.col + " count= " + listItemLeft.Count);

            List<int> listRandom = new List<int>();
            int count = layoutLeft.row * layoutLeft.col;
            for (int i = 0; i < count; i++)
            {
                listRandom.Add(i);
            }
            shuffleArray(listRandom);



            foreach (UIPintuBlockWord ui in listItemLeft)
            {
                layoutLeft.AddItem(ui.gameObject);
            }
            for (int i = 0; i < listItemLeft.Count; i++)
            {
                int idx = listRandom[i];
                int row = idx / layoutLeft.col;
                int col = idx - row * layoutLeft.col;

                GameObject obj = listItemLeft[i].gameObject;
                //RectTransform rctran = obj.transform as RectTransform;
                //rctran.anchoredPosition = pos;
                Vector2 pos = LimitePos(obj, layoutLeft.GetItemPostion(row, col), layoutLeft);

                Vector3 posMoveTo = pos;
                posMoveTo.z = obj.transform.position.z;
                //Debug.Log("i="+i+" posMoveTo="+posMoveTo);
                // iTween.MoveTo(obj, posMoveTo, dur_moveto);
                obj.transform.DOMove(posMoveTo, dur_moveto);
            }


        }
        //right or bottom
        {
            if (Device.isLandscape)
            {
                //right
                w = Common.GetCameraWorldSizeWidth(mainCam) - Common.ScreenToWorldWidth(mainCam, gameRectWidth) / 2;
                x = Common.GetCameraWorldSizeWidth(mainCam) - w;
                h = mainCam.orthographicSize * 2 - Device.offsetBottomWithAdBannerWorld - heightTopbarWorld;
                y = -mainCam.orthographicSize;
                y += Device.offsetBottomWithAdBannerWorld;
            }
            else
            {
                //bottom
                w = Common.GetCameraWorldSizeWidth(mainCam) * 2;
                x = -Common.GetCameraWorldSizeWidth(mainCam);
                y = -mainCam.orthographicSize + Device.offsetBottomWithAdBannerWorld;
                h = (-Common.ScreenToWorldHeight(mainCam, gameRectWidth) / 2) - y;
            }
            Rect rcRight = new Rect(x, y, w, h);
            LayOut layoutRight = new LayOut();
            layoutRight.rect = rcRight;

            layoutRight.row = (int)(rcRight.height / itemHeightWorld);
            if (layoutRight.row <= 0)
            {
                layoutRight.row = 1;
            }
            layoutRight.col = listItemRight.Count / layoutRight.row;
            if (listItemRight.Count % layoutRight.row != 0)
            {
                layoutRight.col++;
            }

            Debug.Log(" layoutarea rcRight=" + rcRight + " row=" + layoutRight.row + " col=" + layoutRight.col + " count= " + listItemRight.Count);

            List<int> listRandom = new List<int>();
            int count = layoutRight.row * layoutRight.col;
            for (int i = 0; i < count; i++)
            {
                listRandom.Add(i);
            }
            shuffleArray(listRandom);



            foreach (UIPintuBlockWord ui in listItemRight)
            {
                layoutRight.AddItem(ui.gameObject);
            }
            for (int i = 0; i < listItemRight.Count; i++)
            {
                int idx = listRandom[i];
                int row = idx / layoutRight.col;
                int col = idx - row * layoutRight.col;
                GameObject obj = listItemRight[i].gameObject;
                //RectTransform rctran = obj.transform as RectTransform;
                //rctran.anchoredPosition = pos;
                Vector2 pos = LimitePos(obj, layoutRight.GetItemPostion(row, col), layoutRight);
                Vector3 posMoveTo = pos;
                posMoveTo.z = obj.transform.position.z;
                // iTween.MoveTo(obj, posMoveTo, dur_moveto);
                obj.transform.DOMove(posMoveTo, dur_moveto);
            }

        }

    }

    Vector2 LimitePos(GameObject obj, Vector2 pos, LayOut ly)
    {
        Vector2 posNew = pos;
        UIPintuBlockWord ui = obj.GetComponent<UIPintuBlockWord>();
        Renderer rd = ui.uiSVG.GetComponent<Renderer>();
        float y_bottom = ly.rect.y;
        float y = pos.y;
        if ((pos.y - rd.bounds.size.y / 2) < y_bottom)
        {
            y = y_bottom + rd.bounds.size.y / 2;
        }
        posNew.y = y;
        return posNew;
    }
    void PlayAudioBlockFinish()
    {
        AudioPlay.main.PlayFile(AppRes.AUDIO_PINTU_BLOCK_FINISH);
    }
    void CheckGameWin()
    {
        bool isAllItemLock = true;
        Debug.Log("CheckGameWin");
        foreach (UIPintuBlockWord ui in listItem)
        {
            bool isLock = ui.IsItemLock();
            if (!isLock)
            {
                isAllItemLock = false;
            }

        }

        if (isAllItemLock)
        {
            //show game win
            //gameEndParticle.Play();
            Invoke("ShowGameWin", 0.1f);

        }

    }

    void ShowGameWin()
    {
        // return;
        Debug.Log("ShowGameWin");
        if (iDelegate != null)
        {
            iDelegate.OnGamePintuWordDidGameWin(this);
        }
        SetGameItemStatus(infoGuankaItem, GAME_STATUS_FINISH);
    }

    public void OnUIPintuBlockWordTouchDown(UIPintuBlockWord ui, PointerEventData eventData)
    {

        isItemHasSel = false;
        isGameFail = false;
        Vector2 pos = Common.GetInputPosition();
        ptDownScreen = pos;

        Vector3 posword = mainCam.ScreenToWorldPoint(pos);
        posItemWorld = ui.gameObject.transform.localPosition;
        // itemInfoSel = info;
        isItemHasSel = true;
        //  Debug.Log("itemInfoSel:row:" + itemInfoSel.row + " col:" + itemInfoSel.col);
        SetZorderTopMost(ui);

        if (GetGameItemStatus(infoGuankaItem) == GAME_STATUS_UN_START)
        {
            SetGameItemStatus(infoGuankaItem, GAME_STATUS_PLAY);
        }
    }
    public void OnUIPintuBlockWordTouchMove(UIPintuBlockWord ui, PointerEventData eventData)
    {
        if (!isItemHasSel)
        {
            return;
        }

        float x, y, w, h;
        Vector2 pos = Common.GetInputPosition();

        Vector2 ptStep = pos - ptDownScreen;
        Vector2 ptStepWorld = Common.ScreenToWorldSize(mainCam, ptStep);
        Vector3 posStepWorld = new Vector3(ptStepWorld.x, ptStepWorld.y, 0);
        Vector3 posword = posItemWorld + posStepWorld;

        //将选中item暂时置顶
        //posword.z = itemPosZ - 1;


        Vector3 posItemSelNormal = GetItemPositionNormal(ui.indexRow, ui.indexCol, false);

        Bounds bd = ui.uiSVG.gameObject.GetComponent<Renderer>().bounds;

        Vector2 posSel = ui.gameObject.transform.position;//localPosition

        float center_step = Common.ScreenToWorldWidth(mainCam, 40 * AppCommon.scaleBase);
        w = center_step;
        h = w;
        x = posSel.x - w / 2;
        y = posSel.y - h / 2;
        Rect rc = new Rect(x, y, w, h);
        Debug.Log("rc=" + rc + " posnormal=" + posItemSelNormal);
        //if (bd.Contains(posItemSelNormal))
        if (rc.Contains(new Vector2(posItemSelNormal.x, posItemSelNormal.y)))
        {
            if (indexPlay == ui.index)
            {
                //选中项拼图正确
                ui.SetItemLock(true);
                Vector3 posTarget = GetItemPositionNormal(ui.indexRow, ui.indexCol, false);

                //将选中item清除置顶
                posTarget.z = itemPosZ;
                ui.gameObject.transform.localPosition = posTarget;
                Vector3 posMoveTo = posTarget;
                //iTween.MoveTo(itemInfoSel.obj, posMoveTo, 0.4f);
                PlayAudioBlockFinish();
                ui.gameObject.SetActive(false);
                // 34 172 56 Color colorFinish =
                uiWordSVG.UpdateStrokeColor(indexPlay, colorFinish);
                indexPlay++;
            }
            else
            {
                //笔画顺序错误  
                ui.gameObject.transform.localPosition = ui.posNormal;
                if (!isGameFail)
                {
                    if (iDelegate != null)
                    {
                        iDelegate.OnGamePintuWordDidGameFail(this);
                    }
                    isGameFail = true;
                }
                return;
            }

        }
        LimitBlockPosition(ui);
        CheckGameWin();

    }
    public void OnUIPintuBlockWordTouchUp(UIPintuBlockWord ui, PointerEventData eventData)
    {
        if (!isItemHasSel)
        {
            return;
        }

        //将选中item清除置顶
        // Vector3 pos = ui.gameObject.transform.position;
        // //pos.z = itemPosZ;
        // itemInfoSel.obj.transform.position = pos;
        LimitBlockPosition(ui);

    }

    //防止超出边界和广告区域
    void LimitBlockPosition(UIPintuBlockWord ui)
    {
        //limit pos
        Vector3 pos = ui.gameObject.transform.position;
        Bounds bd = ui.uiSVG.gameObject.GetComponent<Renderer>().bounds;
        float y_top_limit = 0, y_bottom_limit = 0;
        y_top_limit = mainCam.orthographicSize - heightTopbarWorld;
        y_bottom_limit = -mainCam.orthographicSize + Device.offsetBottomWithAdBannerWorld;
        if ((pos.y + bd.size.y / 2) > y_top_limit)
        {
            pos.y = y_top_limit - bd.size.y / 2;
        }
        if ((pos.y - bd.size.y / 2) < y_bottom_limit)
        {
            pos.y = y_bottom_limit + bd.size.y / 2;
        }
        ui.gameObject.transform.position = pos;
    }

    void OnUIViewAlertFinished(UIViewAlert alert, bool isYes)
    {

        if (isYes)
        {
            if (iDelegate != null)
            {
                iDelegate.OnGamePintuWordDidNextLevel(this);
            }

        }
        else
        {
            if (iDelegate != null)
            {
                iDelegate.OnGamePintuWordDidBack(this);
            }
            // OnClickBtnBack();
        }
    }
}
