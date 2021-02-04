using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIWordImageText : UIWordContentBase
{
    public const int GUANKA_GROUP_ITEM_NUM = 5;
    public const string STR_UNKNOWN_WORD = "__";
    public const float TIME_ANIMATE_ERROR = 2.0f;
    public const int COUNT_ANIMATE_ERROR = 10;
    public Image imageBg;

    public RawImage imagePic;
    public Image imagePicBoard;
    public Text textTitle;

    public GameObject objText;
    public Text textLine0;
    public Text textLine1;
    public Text textXieHouYu;

    int[] listIndexAnswer;

    public List<AnswerInfo> listAnswerInfo;//
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        textXieHouYu.gameObject.SetActive(false);
        CaiCaiLeItemInfo info = GameLevelParse.main.GetItemInfo();
        if (info.gameType == GameRes.GAME_TYPE_TEXT)
        {
            objText.gameObject.SetActive(true);
            imagePic.gameObject.SetActive(false);
        }
        else
        {
            objText.gameObject.SetActive(false);
            imagePic.gameObject.SetActive(true);
        }

        textLine0.color = GameRes.main.colorGameText;
    }
    // Use this for initialization
    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {

    }

    public override void LayOut()
    {
        base.LayOut();
        //imagePicBoard
        {
            RectTransform rctran = imagePicBoard.GetComponent<RectTransform>();
            float oft = 16;
            rctran.offsetMin = new Vector2(oft, oft);
            rctran.offsetMax = new Vector2(-oft, -oft);
        }
        if (Common.appKeyName == GameRes.GAME_IDIOM)
        {
            LayOutScale ls = imagePic.GetComponent<LayOutScale>();
            if (ls != null)
            {
                ls.ratio = 0.5f;
                ls.LayOut();
            }
        }

    }
    public override void UpdateGuankaLevel(int level)
    {
        UpdateItem();
    }

    public void UpdateItem()
    {
        CaiCaiLeItemInfo info = GameLevelParse.main.GetItemInfo();
        GameLevelParse.main.ParseItem(info);

        if (info.gameType == GameRes.GAME_TYPE_TEXT)
        {

            if (Common.appKeyName == GameRes.GAME_POEM)
            {

                PoemContentInfo infopoem0 = info.listPoemContent[0];
                string strPoem = infopoem0.content;
                //过虑标点符号
                List<int> listIndexGuanka = GameLevelParse.main.IndexListNotPunctuation(strPoem);
                //GUANKA_GROUP_ITEM_NUM
                int[] fillWordNum = { 1, 1, 2, 2, 3 };
                int idxfill = LevelManager.main.gameLevel % GUANKA_GROUP_ITEM_NUM;
                //
                listIndexAnswer = Util.main.RandomIndex(listIndexGuanka.Count, fillWordNum[idxfill]);
                ListSorter.EbullitionSort(listIndexAnswer);

                listAnswerInfo = new List<AnswerInfo>();
                GameAnswer.main.strWordAnswer = "";
                for (int i = 0; i < listIndexAnswer.Length; i++)
                {
                    int idx = listIndexGuanka[listIndexAnswer[i]];
                    string word_answer = strPoem.Substring(idx, 1);
                    GameAnswer.main.strWordAnswer += word_answer;

                    AnswerInfo infoanswer = new AnswerInfo();
                    infoanswer.word = word_answer;
                    infoanswer.index = idx;
                    infoanswer.isFinish = false;
                    infoanswer.isFillWord = false;
                    listAnswerInfo.Add(infoanswer);
                    Debug.Log("listAnswerInfo add " + word_answer);
                }

            }

            if (Common.appKeyName == GameRes.GAME_RIDDLE)
            {

                listAnswerInfo = new List<AnswerInfo>();
                GameAnswer.main.strWordAnswer = info.end;
                Debug.Log("GAME_RIDDLE info.end="+info.end);
                for (int i = 0; i < GameAnswer.main.strWordAnswer.Length; i++)
                {
                    int idx = i;
                    string word_answer = GameAnswer.main.strWordAnswer.Substring(idx, 1);
                    AnswerInfo infoanswer = new AnswerInfo();
                    infoanswer.word = word_answer;
                    infoanswer.index = idx;
                    infoanswer.isFinish = false;
                    infoanswer.isFillWord = false;
                    listAnswerInfo.Add(infoanswer);
                    Debug.Log("listAnswerInfo add " + word_answer);
                }
            }
        }
        else
        {
            TextureUtil.UpdateRawImageTexture(imagePic, info.pic, true);
        }
    }


    public int GetFirstUnFillAnswer()
    {
        int index = 0;
        foreach (AnswerInfo info in listAnswerInfo)
        {
            if (info.isFillWord == false)
            {
                break;
            }
            index++;
        }
        return index;
    }

    public int GetFirstUnFinishAnswer()
    {
        int index = 0;
        foreach (AnswerInfo info in listAnswerInfo)
        {
            if (info.isFinish == false)
            {
                break;
            }
            index++;
        }
        return index;
    }

    public override bool CheckAllFill()
    {
        bool isAllFill = true;
        foreach (AnswerInfo info in listAnswerInfo)
        {
            if (!info.isFillWord)
            {
                isAllFill = false;
            }
        }
        return isAllFill;
    }
    public override bool CheckAllAnswerFinish()
    {
        bool ret = true;
        Debug.Log("CheckAllAnswerFinish len=" + listAnswerInfo.Count);
        foreach (AnswerInfo info in listAnswerInfo)
        {
            if (info.isFinish == false)
            {
                ret = false;
                break;
            }

        }
        return ret;
    }

    public void UpdateGameWordString(string str)
    {
        textLine0.text = str;
    }
    public override void UpdateWord()
    {
        CaiCaiLeItemInfo info = GameLevelParse.main.GetItemInfo();

        if (info.gameType == GameRes.GAME_TYPE_TEXT)
        {
            textLine0.text = GetDisplayText(false, false, 0, "");
            // PoemContentInfo infopoem1 = info.listPoemContent[1];  
            // textLine1.text = infopoem1.content;
        }
        else
        {
            if ((!Common.BlankString(info.head)) && (!Common.BlankString(info.end)))
            {
                objText.gameObject.SetActive(false);
                textXieHouYu.gameObject.SetActive(true);
                textXieHouYu.text = GetDisplayText(false, false, 0, "");
            }
        }

    }

    public string GetDisplayText(bool isAnswr, bool isSucces, int indexAnswer, string word)
    {
        CaiCaiLeItemInfo info = GameLevelParse.main.GetItemInfo();
        if ((!Common.BlankString(info.head)) && (!Common.BlankString(info.end)))
        {
            if (Common.appKeyName == GameRes.GAME_RIDDLE)
            {
                return info.head + "\n" + "(" + info.type + ")";
            }

            return info.head;
        }

        PoemContentInfo infopoem0 = info.listPoemContent[0];
        string strPoem = infopoem0.content;

        //过虑标点符号
        List<int> listIndexGuanka = GameLevelParse.main.IndexListNotPunctuation(strPoem);
        List<string> listStr = GetSplitStringByAnswerIndex(listIndexAnswer, listIndexGuanka);
        string strShow = "";
        for (int i = 0; i < listStr.Count; i++)
        {
            string tmp = listStr[i];
            if (i == (listStr.Count - 1))
            {
                strShow += tmp;
            }
            else
            {
                if (isAnswr)
                {
                    AnswerInfo infoanswer = listAnswerInfo[i];

                    if ((infoanswer.isFinish))
                    {
                        strShow += (tmp + infoanswer.word);
                    }
                    else
                    {

                        if ((indexAnswer == i) && (indexAnswer < listAnswerInfo.Count))
                        {
                            AnswerInfo infotmp = listAnswerInfo[indexAnswer];
                            infotmp.isFillWord = true;
                            Debug.Log("GetDisplayText isFillWord="+ infotmp.isFillWord+"i=" + i);
                            string word_show = "";
                            if (isSucces)
                            {
                                word_show = infotmp.word;
                            }
                            else
                            {
                                word_show = word;
                            }

                            //<color=#00ffffff>TestTest</color>
                            Color color = new Color32(255, 0, 0, 255);
                            word_show = "<color=#" + ColorUtility.ToHtmlStringRGBA(color) + ">" + word_show + "</color>";
                            strShow += (tmp + word_show);
                            infotmp.wordFill = word_show;
                        }
                        else
                        {
                            if (infoanswer.isFillWord)
                            {
                                strShow += (tmp + infoanswer.wordFill);
                            }
                            else
                            {
                                strShow += (tmp + STR_UNKNOWN_WORD);
                            }


                        }

                    }

                }

                else
                {
                    strShow += (tmp + STR_UNKNOWN_WORD);
                }
            }

        }
        return strShow;
    }


    List<string> GetSplitStringByAnswerIndex(int[] listIndex, List<int> listIndexGuanka)
    {
        CaiCaiLeItemInfo info = GameLevelParse.main.GetItemInfo();
        PoemContentInfo infopoem0 = info.listPoemContent[0];
        string strPoem = infopoem0.content;


        List<string> listStr = new List<string>();
        for (int i = 0; i < listIndex.Length; i++)
        {
            int idx = listIndexGuanka[listIndex[i]];
            int idx_pre = 0;
            if (i > 0)
            {
                idx_pre = listIndexGuanka[listIndex[i - 1]] + 1;
            }
            int len = idx - idx_pre;
            string str = strPoem.Substring(idx_pre, len);
            listStr.Add(str);
            //最后一个
            if (i == listIndex.Length - 1)
            {
                len = (strPoem.Length - 1) - idx;
                if (len > 0)
                {
                    str = strPoem.Substring(idx + 1, len);
                    listStr.Add(str);
                }
            }

        }
        return listStr;
    }

    public void ClearWord()
    {
    }
    public void SetWordColor(Color color)
    {
        textTitle.color = color;
    }

    public void SetFontSize(int size)
    {
        textTitle.fontSize = size;
    }

    public override void OnTips()
    {
        int index = GetFirstUnFinishAnswer();
        AnswerInfo info = listAnswerInfo[index];
        UpdateGameWordString(GetDisplayText(true, true, index, ""));
        info.isFinish = true;
    }

    public override void OnAddWord(string word)
    {
        bool isInAnswerList = false;
        int index = 0;
        foreach (AnswerInfo info in listAnswerInfo)
        {
            if (info.word == word)
            {
                //回答正确
                Debug.Log("GetDisplayText ok index =" + index);
                UpdateGameWordString(GetDisplayText(true, true, index, ""));
                info.isFinish = true;
                isInAnswerList = true;
                break;

            }
            index++;
        }

        if (!isInAnswerList)
        {
            //回答错误
            index = GetFirstUnFillAnswer();
            Debug.Log("GetDisplayText error index=" + index);

            UpdateGameWordString(GetDisplayText(true, false, index, word));
        }
    }

    public void OnClickItem()
    {

    }
}
