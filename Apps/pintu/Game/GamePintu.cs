using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
public interface IGamePintuDelegate
{
    void OnGamePintuDidBack(GamePintu ui);
    void OnGamePintuDidNextLevel(GamePintu ui);
    void OnGamePintuDidGameWin(GamePintu ui);
}
public class GamePintu : GameBase, IUIPintuBlockDelegate
{
    public enum ImageSource
    {
        GAME_INNER,//游戏内置图片
        SYSTEM_IMAGE_LIB,//系统图库
        NET,//web

    }
    List<UIPintuBlock> listItem;
    public int row;
    public int col;
    public IGamePintuDelegate iDelegate;

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


    float heightTopbarWorld;
    float itemPosZ = -1f;

    List<UIPintuBlock> listItemLeft;//left and top
    List<UIPintuBlock> listItemRight;

    GameObject objGamePic;
    Rect rectMain;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {

        listItemLeft = new List<UIPintuBlock>();
        listItemRight = new List<UIPintuBlock>();
        listItem = new List<UIPintuBlock>();


        offsetTopbarY = 160;
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        heightTopbarWorld = Common.CanvasToWorldHeight(mainCam, sizeCanvas, offsetTopbarY);



    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
    public void CreateBlock()
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                GameObject obj = new GameObject("block_row_" + i + "_col_" + j);
                obj.transform.parent = this.transform;
                //obj.transform.localPosition = new Vector3(0, 0, -1f);
                UIPintuBlock uiBlock = obj.AddComponent<UIPintuBlock>();
                uiBlock.iDelegate = this;
                uiBlock.row = row;
                uiBlock.col = col;
                uiBlock.indexRow = i;
                uiBlock.indexCol = j;
                uiBlock.sideTypeTop = UIPintuBlock.SIDE_TYPE_NORMAL;
                uiBlock.sideTypeBottom = UIPintuBlock.SIDE_TYPE_NORMAL;
                uiBlock.sideTypeLeft = UIPintuBlock.SIDE_TYPE_NORMAL;
                uiBlock.sideTypeRight = UIPintuBlock.SIDE_TYPE_NORMAL;
                // obj.SetActive(false);
                // if ((i == 0) && (j == 0))
                // {
                //     obj.SetActive(true);
                // }
                listItem.Add(uiBlock);
            }
        }


        //分组
        int size = row * col;
        int[] indexLeft = Util.main.RandomIndex(size, size / 2);
        listItemLeft.Clear();
        listItemRight.Clear();
        for (int i = 0; i < size; i++)
        {
            bool ret = IsIndexInList(indexLeft, i);
            UIPintuBlock ui = listItem[i];
            if (ret)
            {
                listItemLeft.Add(ui);
            }
            else
            {
                listItemRight.Add(ui);
            }
        }

        UpdateBlockSideType();
    }

    public void UpdateTexture(Texture2D tex)
    {
        UpdateGameImage(tex, false);

        foreach (UIPintuBlock ui in listItem)
        {
            //
            ui.centerWidth = rectMain.size.x / ui.col;
            ui.centerHeight = rectMain.size.y / ui.row;
            ui.UpdateTexture(tex);
            Renderer rd = ui.gameObject.GetComponent<Renderer>();
            itemWidthWorld = rd.bounds.size.x;
            itemHeightWorld = rd.bounds.size.y;
            BoxCollider box = ui.gameObject.AddComponent<BoxCollider>();
            box.size = rd.bounds.size;
            //ui.transform.localPosition = GetItemPositionNormal(ui.indexRow, ui.indexCol, false);
        }

        LayOutBlock(true);
    }
    //随机类型
    int GetRandomSideType()
    {
        int idx = Random.Range(0, 2);
        int ret = UIPintuBlock.SIDE_TYPE_IN;
        if (idx == 0)
        {
            ret = UIPintuBlock.SIDE_TYPE_OUT;
        }
        ret = UIPintuBlock.SIDE_TYPE_IN;
        return ret;
    }

    //反向类型
    int GetReverseSideType(int type)
    {
        int ret = UIPintuBlock.SIDE_TYPE_NORMAL;
        if (type == UIPintuBlock.SIDE_TYPE_IN)
        {
            ret = UIPintuBlock.SIDE_TYPE_OUT;
        }
        else if (type == UIPintuBlock.SIDE_TYPE_OUT)
        {
            ret = UIPintuBlock.SIDE_TYPE_IN;
        }
        return ret;
    }
    //设置四边的凹凸类型
    void UpdateBlockSideType()
    {

        foreach (UIPintuBlock ui in listItem)
        {

            UIPintuBlock uiLeft = GetItem(ui.indexRow, ui.indexCol - 1);
            UIPintuBlock uiRight = GetItem(ui.indexRow, ui.indexCol + 1);
            UIPintuBlock uiTop = GetItem(ui.indexRow + 1, ui.indexCol);
            UIPintuBlock uiBottom = GetItem(ui.indexRow - 1, ui.indexCol);

            if (uiLeft == null)
            {
                //最左边
                ui.sideTypeLeft = UIPintuBlock.SIDE_TYPE_NORMAL;
                ui.sideTypeRight = GetRandomSideType();
            }
            else
            {
                // Debug.Log("now ui.row=" + ui.row + " ui.col=" + ui.col + " uiLeft.row=" + uiLeft.row + " uiLeft.col=" + uiLeft.col + "uiLeft.sideTypeRight=" + uiLeft.sideTypeRight);
                ui.sideTypeLeft = GetReverseSideType(uiLeft.sideTypeRight);

                if (uiRight != null)
                {
                    ui.sideTypeRight = GetRandomSideType();
                }

            }


            if (uiBottom == null)
            {
                //最底部
                ui.sideTypeBottom = UIPintuBlock.SIDE_TYPE_NORMAL;
                ui.sideTypeTop = GetRandomSideType();
            }
            else
            {
                ui.sideTypeBottom = GetReverseSideType(uiBottom.sideTypeTop);
                if (uiTop != null)
                {
                    ui.sideTypeTop = GetRandomSideType();
                }
            }


        }

    }

    UIPintuBlock GetItem(int r, int c)
    {
        UIPintuBlock item = null;
        foreach (UIPintuBlock ui in listItem)
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
        if (!isOption)
        {
            ShowItemAnimate();
        }
    }

    public void LayOutGamePic()
    {
        float x = 0, y = 0, z = 0, w = 0, h = 0, w_rect = 0, h_rect = 0;
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        heightTopbarWorld = Common.CanvasToWorldHeight(mainCam, sizeCanvas, offsetTopbarY);

        Vector2 sizeWorld = Vector2.zero;//Common.ScreenToWorldSize(mainCam, new Vector2(pixsel_w, pixsel_h));
        RectTransform rcTran = AppSceneBase.main.GetRectMainWorld();
        sizeWorld.x = rcTran.rect.width;
        sizeWorld.y = rcTran.rect.height - heightTopbarWorld;
        if (!Device.isLandscape)
        {
            sizeWorld.x = rcTran.rect.width * 0.6f;
            sizeWorld.y = rcTran.rect.height - heightTopbarWorld;
        }
        if (objGamePic != null)
        {

            SpriteRenderer spRd = objGamePic.GetComponent<SpriteRenderer>();
            Sprite sp = spRd.sprite;
            Texture2D tex = sp.texture;
            float pixsel_per_unit = 100;
            float tex_world_w = tex.width / pixsel_per_unit;
            float tex_world_h = tex.height / pixsel_per_unit;

            float pos_y = 0;
            if (Device.isLandscape)
            {
                w_rect = sizeWorld.x;
                h_rect = sizeWorld.y - heightTopbarWorld - Device.offsetBottomWithAdBannerWorld;//  GameManager.main.heightAdWorld;
                pos_y = -heightTopbarWorld / 2 + Device.offsetBottomWithAdBannerWorld / 2;
            }
            else
            {
                w_rect = sizeWorld.x;
                h_rect = sizeWorld.y;
                pos_y = 0;
            }
            float scale = Common.GetBestFitScale(tex_world_w, tex_world_h, w_rect, h_rect);
            gameScaleX = scale;
            gameScaleY = scale;
            //gamePicBgScale = scale;
            z = 0;
            objGamePic.transform.localScale = new Vector3(scale, scale, 1f);
            //rcTran.sizeDelta = new Vector2(tex_world_w, tex_world_h);

            objGamePic.transform.localPosition = new Vector3(0, pos_y, z);


            w = spRd.bounds.size.x;
            h = spRd.bounds.size.y;
            x = -w / 2;
            y = pos_y - h / 2;
            Debug.Log("game pic:gamePicBgScale=" + gamePicBgScale + " gameScaleX=" + gameScaleX + " x=" + x + " y=" + y + " w=" + w + " h=" + h);
            rectMain = new Rect(x, y, w, h);

            gameRectWidth = Common.WorldToScreenWidth(mainCam, w);

        }


    }

    public void LayOutBlock(bool isOption)
    {

        foreach (UIPintuBlock ui in listItem)
        {
            ui.enableTouch = !isOption;
        }
        if (objGamePic != null)
        {
            objGamePic.SetActive(!isOption);
        }
        //更新位置
        foreach (UIPintuBlock ui in listItem)
        {
            ui.transform.localPosition = GetItemPositionNormal(ui.indexRow, ui.indexCol, isOption);
            ui.centerWidth = rectMain.size.x / ui.col;
            ui.centerHeight = rectMain.size.y / ui.row;
            float scale = 1f;
            if (isOption)
            {
                scale = 0.9f;
                //  ui.centerWidth *= 0.9f;
            }
            ui.transform.localScale = new Vector3(scale, scale, 1f);
            ui.Draw();
        }

    }
    public void ShowGameImage(bool isOption)
    {
    }

    //置顶
    void SetZorderTopMost(UIPintuBlock ui)
    {
        foreach (UIPintuBlock uilist in listItem)
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
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        w = rectMain.size.x / col;
        h = rectMain.size.y / row;
        float oftx = rectMain.x;
        float ofty = rectMain.y;
        if (isOption)
        {


            // x = itemWidthWorld * c - oft;
            // oft = itemHeightWorld * r / 2 - itemHeightWorld / 2;
            // if (!Device.isLandscape)
            // {
            //     oft -= itemHeightWorld * r / 2;
            //     oft += Common.CanvasToWorldHeight(mainCam, sizeCanvas, offsetTopbarY);
            // }
            // y = itemHeightWorld * r - oft;
            Vector2 sizeWorld = Common.GetWorldSize(mainCam);

            if (Device.isLandscape)
            {
                oftx = -sizeWorld.x / 4 - rectMain.width / 2;
                ofty = rectMain.y;
            }
            else
            {
                oftx = rectMain.x;
                // ofty = sizeWorld.y / 4 - rectMain.height / 2;
            }
        }

        x = oftx + w * c + w / 2;
        y = ofty + h * r + h / 2;


        //itemPosZ = z;
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
            objGamePic = new GameObject("game_pic");
            AppSceneBase.main.AddObjToMainWorld(objGamePic);


            SpriteRenderer spRd = objGamePic.AddComponent<SpriteRenderer>();

            spRd.material = new Material(Shader.Find("Custom/Grey"));


            Debug.Log("tex,w:" + tex.width + " h:" + tex.height);
            Sprite sprite = TextureUtil.CreateSpriteFromTex(tex);
            sprite.name = "game";
            spRd.sprite = sprite;
            float pixsel_w = gameRectWidth;//GAME_ITEM_SCREEN_WIDTH * AppCommon.scaleBase;
            float pixsel_h = pixsel_w;

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

            Debug.Log("rcLeft=" + rcLeft + " row=" + layoutLeft.row + " col=" + layoutLeft.col);

            List<int> listRandom = new List<int>();
            int count = layoutLeft.row * layoutLeft.col;
            for (int i = 0; i < count; i++)
            {
                listRandom.Add(i);
            }
            shuffleArray(listRandom);



            foreach (UIPintuBlock ui in listItemLeft)
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



            List<int> listRandom = new List<int>();
            int count = layoutRight.row * layoutRight.col;
            for (int i = 0; i < count; i++)
            {
                listRandom.Add(i);
            }
            shuffleArray(listRandom);



            foreach (UIPintuBlock ui in listItemRight)
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
        Renderer rd = obj.GetComponent<Renderer>();
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
        foreach (UIPintuBlock ui in listItem)
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
        string title = Language.main.GetString(AppString.STR_UIVIEWALERT_TITLE_GAME_FINISH);
        string msg = Language.main.GetString(AppString.STR_UIVIEWALERT_MSG_GAME_FINISH);
        string yes = Language.main.GetString(AppString.STR_UIVIEWALERT_YES_GAME_FINISH);
        string no = Language.main.GetString(AppString.STR_UIVIEWALERT_NO_GAME_FINISH);
        ViewAlertManager.main.ShowFull(title, msg, yes, no, true, UIGameBase.STR_KEYNAME_VIEWALERT_GAME_FINISH, OnUIViewAlertFinished);
        if (iDelegate != null)
        {
            iDelegate.OnGamePintuDidGameWin(this);
        }
        SetGameItemStatus(infoGuankaItem, GAME_STATUS_FINISH);
    }

    public void OnUIPintuBlockTouchDown(UIPintuBlock ui, PointerEventData eventData)
    {

        isItemHasSel = false;
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
    public void OnUIPintuBlockTouchMove(UIPintuBlock ui, PointerEventData eventData)
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

        Bounds bd = ui.gameObject.GetComponent<Renderer>().bounds;

        Vector2 posSel = ui.gameObject.transform.localPosition;

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
            //选中项拼图正确
            ui.SetItemLock(true);
            Vector3 posTarget = GetItemPositionNormal(ui.indexRow, ui.indexCol, false);

            //将选中item清除置顶
            posTarget.z = itemPosZ;
            ui.gameObject.transform.localPosition = posTarget;
            Vector3 posMoveTo = posTarget;
            //iTween.MoveTo(itemInfoSel.obj, posMoveTo, 0.4f);
            PlayAudioBlockFinish();
        }
        LimitBlockPosition(ui);
        CheckGameWin();

    }
    public void OnUIPintuBlockTouchUp(UIPintuBlock ui, PointerEventData eventData)
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
    void LimitBlockPosition(UIPintuBlock ui)
    {
        //limit pos
        Vector3 pos = ui.gameObject.transform.position;
        Bounds bd = ui.gameObject.GetComponent<Renderer>().bounds;
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
                iDelegate.OnGamePintuDidNextLevel(this);
            }

        }
        else
        {
            if (iDelegate != null)
            {
                iDelegate.OnGamePintuDidBack(this);
            }
            // OnClickBtnBack();
        }
    }
}
