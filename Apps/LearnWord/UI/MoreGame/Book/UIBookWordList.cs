using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 教材网： http://www.dzkbw.com/books/rjb/yuwen/
public class UIBookWordList : UITableViewControllerBase
{
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    public void Awake()
    {
        base.Awake(); 
        ParseWordList();
        heightCell = 512;
        oneCellNum = 1;


    }
    public void Start()
    {
        base.Start();

        UpdateTable(false,false);
        LayOut();
        OnUIDidFinish();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OnClickBtnBack();
        }
    }



    public void ParseWordList()
    {
        int count = 0;
        ItemInfo infoBook = BookWordViewController.main.infoBook;
        ItemInfo infoSort = BookParse.main.listSort[BookParse.main.indexSort] as ItemInfo;
        BookParse.main.ParseBookWordList(infoSort.id, infoBook.title);
        listItem = BookParse.main.listWord;
        // tableView.ReloadData();
    }
    public override void LayOut()
    {
        base.LayOut();

    }

    public override void OnCellItemDidClick(UICellItemBase item)
    {
        if (item.IsLock())
        {
            return;
        }
        // LevelManager.main.placeLevel = item.index;
      


    }

    public void OnClickBtnBack()
    {
        if (this.controller.naviController != null)
        {
            this.controller.naviController.Pop();
        }
    }

}
