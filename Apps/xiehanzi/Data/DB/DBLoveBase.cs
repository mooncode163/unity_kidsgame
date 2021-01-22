
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DBLoveBase : DBBase
{ 

    public  virtual void DeleteItem(DBItemInfoBase info)
    { 
    }
    public virtual bool IsItemExist(DBItemInfoBase info)
    { 
        return false;
    }
}

