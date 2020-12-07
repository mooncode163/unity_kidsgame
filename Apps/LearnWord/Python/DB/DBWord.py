
from DB.Sql import Sql
import re


class DBWord():
    sql: None
    TABLE_NAME: ""

    def OpenDB(self, dbfile):
        self.sql = Sql()
        self.sql.Open(dbfile)
        self.TABLE_NAME = "table_word"

        self.item_col = []
        self.item_coltype = []

        self.KEY_id = "id"
        self.KEY_title = "title"
        self.KEY_imagetitle = "imagetitle"
        self.KEY_pinyin = "pinyin"
        self.KEY_zuci = "zuci"
        self.KEY_bushou = "bushou"
        self.KEY_bihuaCount = "bihuaCount"
        self.KEY_bihuaName = "bihuaName"
        self.KEY_bihuaOrder = "bihuaOrder"
        self.KEY_audio = "audio"
        self.KEY_gif = "gif"
        self.KEY_antonym = "antonym"
        self.KEY_homoionym = "homoionym"
        self.KEY_wubi= "wubi"
        self.KEY_meanBasic = "meanBasic"
        self.KEY_meanDetail = "meanDetail"
        self.KEY_date = "date"
        self.KEY_fanti= "fanti"
 
        self.KEY_addtime = "addtime"
        # self.arrayPunctuation = ["。", "？", "！", "，", "、", "；", "：" ]

        self.item_col.append(self.KEY_id)
        self.item_col.append(self.KEY_title)
        self.item_col.append(self.KEY_imagetitle)
        self.item_col.append(self.KEY_pinyin)
        self.item_col.append(self.KEY_zuci)
        self.item_col.append(self.KEY_bushou)
        self.item_col.append(self.KEY_bihuaCount)
        self.item_col.append(self.KEY_bihuaName)
        self.item_col.append(self.KEY_bihuaOrder)
        self.item_col.append(self.KEY_audio)
        self.item_col.append(self.KEY_gif)
        self.item_col.append(self.KEY_meanBasic)
        self.item_col.append(self.KEY_meanDetail)
        self.item_col.append(self.KEY_antonym)
        self.item_col.append(self.KEY_homoionym)
        self.item_col.append(self.KEY_wubi)
        self.item_col.append(self.KEY_fanti)

        for i in range(len(self.item_col)):
            self.item_coltype.append("TEXT")

         # 注意 CREATE TABLE 这种语句不分大小写 PRIMARY KEY
        # sql_create = '''
        # CREATE TABLE IF NOT EXISTS `TablePoem`  (
        #     `title`    TEXT  ,
        #     `year`    TEXT  ,
        #     `author`    TEXT  ,
        #     `content`    TEXT  ,
        #     `content_pinyin`    TEXT  ,
        #     `translation`    TEXT  ,
        #     `authorDetail`    TEXT  ,
        #     `appreciation`    TEXT
        # )
        # '''

        # self.sql.Execute(sql_create)
        self.sql.CreateTable(self.TABLE_NAME, self.item_col, self.item_coltype)


    def IsBlankString(self,string):
        if string==None:
            return True

        if len(string)==0:
            return True

        return False

    def SetVaule(self,values,content):
        if self.IsBlankString(content):
            values.append("unknown")
        else:
            str = content
            # values.append(re.escape(content))
            # str = str.replace(",",".")
            # str = str.replace("，",".")
            values.append(str)

    def AddItem(self,info):
        if self.IsItemExist(info.id)==True:
            return

        values=[] 
        self.SetVaule(values,info.id)
        self.SetVaule(values,info.title)
        self.SetVaule(values,info.imagetitle)
        self.SetVaule(values,info.pinyin)
        self.SetVaule(values,info.zuci)
        self.SetVaule(values,info.bushou)
        self.SetVaule(values,info.bihuaCount)
        self.SetVaule(values,info.bihuaName)
        self.SetVaule(values,info.bihuaOrder)
        self.SetVaule(values,info.audio)
        self.SetVaule(values,info.gif) 
        self.SetVaule(values,info.meanBasic)  
        self.SetVaule(values,info.meanDetail) 
        self.SetVaule(values,info.antonym) 
        self.SetVaule(values,info.homoionym) 
        self.SetVaule(values,info.wubi) 
        self.SetVaule(values,info.fanti) 
        
 

        # print("AddItem content_pinyin=",values[4])
        # INSERT INTO TablePoem  VALUES('a','c','b','d','e','f')
# INSERT INTO TablePoem ('title', 'author', 'content','content_pinyin','translation','appreciation') VALUES('a','unknown','b','unknown','unknown','unknown')
       
        self.sql.Insert(self.TABLE_NAME,values)


    
    def IsItemExist(self,id):
        ret = False
        strsql = "SELECT * FROM " + self.TABLE_NAME + " WHERE id = '" + id + "'"
        cursor = self.sql.Execute(strsql)
        rows=cursor.fetchall()
        if len(rows)>0:
            ret = True
        
        # print("IsItemExist  ret=",ret)
        return ret


    def GetIndexOfCol(self,strcol):
        for i,value in enumerate(self.item_col):
            if value == strcol:
                return i
        
        return 0


    def FortmatContent(self,content):
        strtmp = content
        # 去除 (难着 一作：犹著)
        for i in range(10):
            idx0 = strtmp.find("(")
            idx1 = strtmp.find(")")
            if idx0<idx1:
                strfind = strtmp[idx0:idx1+1]
                strtmp = strtmp.replace(strfind,"")
        
        for i in range(10):
            idx0 = strtmp.find("（")
            idx1 = strtmp.find("）")
            if idx0<idx1:
                strfind = strtmp[idx0:idx1+1]
                strtmp = strtmp.replace(strfind,"")

        for i in range(10):
            idx0 = strtmp.find("[")
            idx1 = strtmp.find("]")
            if idx0<idx1:
                strfind = strtmp[idx0:idx1+1]
                strtmp = strtmp.replace(strfind,"")
                      
        for i in range(10):
            idx0 = strtmp.find("【")
            idx1 = strtmp.find("】")
            if idx0<idx1:
                strfind = strtmp[idx0:idx1+1]
                strtmp = strtmp.replace(strfind,"")

        return strtmp

    

    def SplitContent(self,content):
        strtmp = content
        strsplit = "-"
        for s in self.arrayPunctuation:
            strtmp = strtmp.replace(s,strsplit)
        
        liststr = strtmp.split(strsplit)
        # for s in liststr:
        #     print(s)
        return liststr

            
    def GetPoemContent(self,title):
        strsql = "select * from " + self.TABLE_NAME + " where title = '" + title + "'";
        cursor = self.sql.Execute(strsql) 
        rows=cursor.fetchall()
        for r in rows:
            listRow = list(r) 
            content = listRow[self.GetIndexOfCol(self.KEY_content)]
            return self.SplitContent(content) 

        return None


    def GetAllItem(self):
        strsql = "select * from " + self.TABLE_NAME
        cursor = self.sql.Execute(strsql) 
        rows=cursor.fetchall()
        for r in rows:
            listRow = list(r)
            title = listRow[0]
            # content = listRow[self.GetIndexOfCol(self.KEY_content)]
            # self.SplitContent(content)
            # print(title) 
            break

  
 
 
