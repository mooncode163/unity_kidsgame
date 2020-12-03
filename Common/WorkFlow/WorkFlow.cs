using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkFlow : MonoBehaviour
{
    int indexItem;
    List<object> listItem;
    public void AddWork(WorkItem item)
    {
        if (listItem == null)
        {
            listItem = new List<object>();
        }
        listItem.Add(item);
    }

    public void Run()
    {

    }

    public void OnWorkStart()
    {

    }
    public void OnWorkFinish()
    {

    }
    public void OnWorkProcess()
    {

    }
}
