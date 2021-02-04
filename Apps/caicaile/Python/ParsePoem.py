from bs4 import BeautifulSoup

from urllib.request import urlopen
import sqlite3 
import json

from DB.DBPoem import DBPoem

from PoemInfo import PoemInfo 

import ssl
# 跳过ssl证书
ssl._create_default_https_context = ssl._create_unverified_context

#古诗网 http://www.cquctt.cn/ 
WEB_HOME_URL = "http://www.cquctt.cn"

class ParsePoem():
    title =""
    author =""
    listSort = []
    dbPoem = None
    
    def LoadJson(self,filepath):  
        data = None
        with open(filepath) as json_file:
            data = json.load(json_file)
            return data


    def Run(self):
        url = WEB_HOME_URL
        html = self.GetHtml(url)
        self.ParseSort(html)
        for d in self.listSort:
            name = d["title"]
            url = d["url"]
            page = self.GetTotalPage(url)
            print("page=",page)
            listPoem = []
            for i in range(page):
                url_page = self.GetUrlPage(url,i)
                print("url_page=",url_page)
                listtmp = self.ParsePoemList(url_page)
                listPoem+=listtmp

            jsonroot = dict (items=listPoem)
            self.SaveJson("OutPut/"+name+".json",jsonroot) 


        # self.CreateDB()

# info = PoemInfo()
# info.title = "test"
# zm = Student('zhangming', 20, [69, 88, 100])
# print(zm.get_name())  
         
    def CreateDB(self):
        self.dbPoem = DBPoem()
        self.dbPoem.OpenDB("OutPut/poem.db")

        jsonRoot = self.LoadJson("OutPut/sort.json")
        jsonItems = jsonRoot["items"]
        for item in jsonItems: 
            title = item["title"] 
            self.ParseJsonPoemList(title,False)
                # break


        self.dbPoem.GetAllItem()
 
    def MakeGuanka(self):
        if self.dbPoem is None:
            self.dbPoem = DBPoem()
            self.dbPoem.OpenDB("OutPut/poem.db")

        jsonRoot = self.LoadJson("OutPut/sort.json")
        jsonItems = jsonRoot["items"]
        for item in jsonItems: 
            title = item["title"] 
            self.ParseJsonPoemList(title,True) 

    
    def ParseJsonPoemList(self,name,isMakeGuanka): 
        jsonRoot = self.LoadJson("OutPut/"+name+".json")
        jsonItems = jsonRoot["items"]
        for item in jsonItems:  
            url = item["url"] 
            print(url)
            title = item["title"]
            if isMakeGuanka==True:
                listContent = self.dbPoem.GetPoemContent(title)
                listDict = []
                for s in listContent:
                    # d = dict (title=s)
                    tmp = self.RemoveSpace(s)
                    if(len(tmp)):
                        listDict.append(tmp)
                
                # dict_content = dict (content=listDict)
                item["content"]=listDict

            else:
                if self.dbPoem.IsItemExist(title)==False:
                    self.ParsePoemContent(url)
        
        if isMakeGuanka==True:
            filePath = "OutPut_new/"+name+".json"
            self.SaveJson(filePath,jsonRoot)

    def GetHtml(self,url):
        return  urlopen(url).read().decode('utf-8')

    def ParseSort(self,html):
    # 本质上是一个tag类型,生成一个tag实例对象，调用tag的方法
        soup = BeautifulSoup(html, "lxml")
        ul = soup.find("ul",class_="zhuanj_ul zm1 mt_10")
        # div = soup.find("ul")
        # print(type(div))    # <class 'bs4.element.Tag'>
        # # string打印标签下的直接子元素，隔行显示不能打印
        # print(div.string)
        # # contents打印标签下的所有元素，返回一个列表
        # print(ul.contents)
        # # children打印标签下的所有元素，返回一个迭代器
        # print(div.children) 
        list_li = ul.find_all("li")
        for li in list_li:
            a = li.find("a",class_="f_12")
            # print(a.get('href'))
            url = WEB_HOME_URL+a["href"]
            print(url)
            img = li.find("img",class_="dis_b")
            title = img["alt"]
            # info = PoemInfo()
            # info.url = url
            # info.title ="title"
            d = dict (title=title,url= url)
            self.listSort.append(d)
            
            # ParsePoemList(url)
            # break

        jsonroot = dict (items=self.listSort)
        self.SaveJson("OutPut/sort.json",jsonroot)

    def GetUrlPage(self,url,index):
        # /gushi/0/0/0/70/0/28/
        url_tmp = url
        url_head =""
        last = url_tmp[-1:]
        if last=="/":
            url_tmp = url_tmp[0:(len(url_tmp)-1)]

        # /gushi/0/0/0/70/0/28
        idx = url_tmp.rfind("/")
        if idx>=0:
            url_head = url_tmp[0:idx+1] 
        return url_head+str(index+1)+"/"

    def GetTotalPage(self,url):
        page = 0
        html = self.GetHtml(url)
        soup = BeautifulSoup(html, "lxml")
        div = soup.find("div",class_="page")
        list_a = div.find_all("a")
        haslast = False
        for a in list_a:
            if a.get_text()=="末页":
                haslast = True
                href = a["href"]
                # /gushi/0/0/0/70/0/28/ 
                last = href[-1:]
                if last=="/":
                    href = href[0:(len(href)-1)]
                idx = href.rfind("/")
                if idx>=0:
                    strtmp = href[idx+1:]
                    page = int(strtmp)
        
        if haslast==False:
            page = len(list_a)-1
        return page

    def ParsePoemList(self,url):
        listPoem = []
        html = self.GetHtml(url)
        soup = BeautifulSoup(html, "lxml")
        div = soup.find("div",class_="left")
        ul = div.find("ul")
        # print(ul)
        list_li = ul.find_all("li")
        for li in list_li:
            # img
            img = li.find("img") 
            if img is not None:
                src = img["src"] 
                url = WEB_HOME_URL+src
                # print("img:"+url) 

            # title
            a = li.find("strong").find("a")
            # print("title:")
            title = a.get_text()
            # print(a.contents)
            url_poem = WEB_HOME_URL+a["href"]
            # print(url_poem)  
            # info = self.ParsePoemContent(url_poem)
            
            # 作者
            author = ""
            a = li.find("span").find("a")
            if a is not None:
                url = WEB_HOME_URL+a["href"]
                # print("author:")
                author = a.get_text()
                # print(a.contents)
                # print(url)  

            # d = dict (title=title,url= url_poem,author=author,content=info.content)
            d = dict (title=title,url= url_poem,author=author)
            listPoem.append(d)

        return listPoem



    def RemoveSpace(self,content): 
        strtmp = content
        strtmp = strtmp.replace(" ", "")
        strtmp = strtmp.replace(" ", "")
        strtmp = strtmp.replace("\n", "")
        strtmp = strtmp.replace("\r", "")
        strtmp = strtmp.replace("\r\n", "")
        strtmp = strtmp.replace("'", "")
        # strtmp = strtmp.replace("[", "")
        # strtmp = strtmp.replace("]", "")
        strtmp = strtmp.replace(',<br/>,','')
        return strtmp

    def FormatTitle(self,content): 
        strtmp = content.replace("['", "")
        strtmp = strtmp.replace("']", "") 
        return strtmp

 

    def ParsePoemContent(self,url):
        info = PoemInfo()
        html = self.GetHtml(url)
        soup = BeautifulSoup(html, "lxml")
        div_main = soup.find("div",class_="left")
        div_article = div_main.find("div",class_="article")
        h2 = div_article.find("h2")
        info.title = self.FormatTitle(str(h2.contents))
        a = div_article.find("a")
        info.author = a.get_text()

        list_em = div_article.find_all("em")
        for em in list_em:
            a = em.find("a")
            # 作者
            # url = WEB_HOME_URL+a["href"]
            # print(url)

            # 朝代
            if a is not None:
                year = a.get_text()
                print(year)
                info.year = year

        # content
        info.content =self.GetOrignContent(div_article)
        info.content_pinyin =self.GetPinyin(div_article)
        info.translation= self.GetFanyi(div_article)
        info.appreciation=self.GetShangxi(div_article)
        info.authorDetail=self.GetAuthor(div_article)
        self.dbPoem.AddItem(info)
        return info
            

    # 原文
    def GetOrignContent(self,div):
        strtmp = ""
        dl = div.find("dl",class_="content")
        dd = dl.find("dd") 
        strtmp = dd.get_text() 
        strtmp = self.RemoveSpace(strtmp)
        # strtmp = self.RemoveSpace(str(dd.contents)) 
        # dd = dd.extract()
        print(strtmp)
        return strtmp

    # 拼音
    def GetPinyin(self,div):
        strtmp = ""
        dd = div.find("dd",id="dd")
        # 去除foot标签
        [s.extract() for s in dd("font")] 

        # strtmp = str(dd.contents)
        strtmp = dd.get_text()

        strtmp = self.RemoveSpace(strtmp)  
        print(strtmp)
        return strtmp


    # 翻译
    def GetFanyi(self,div): 
        strtmp = ""
        list_dd = div.find_all("dd")
        for dd in list_dd:
            h4 = dd.find("h4")
            if h4 is not None:
                str = h4.get_text()
                if '注释' in str:
                    # 去除标签
                    [s.extract() for s in dd("h4")]
                    [s.extract() for s in dd("a")]
                    strtmp = dd.get_text()
                    strtmp = self.RemoveSpace(strtmp)  
                    print(strtmp)  
        
        return strtmp
    

    # 赏析
    def GetShangxi(self,div):
        strtmp = ""
        list_dd = div.find_all("dd")
        for dd in list_dd:
            h4 = dd.find("h4")
            if h4 is not None:
                str = h4.get_text()
                if '赏析' in str:
                    # 去除标签
                    [s.extract() for s in dd("h4")]
                    [s.extract() for s in dd("a")]
                    strtmp = dd.get_text()
                    strtmp = self.RemoveSpace(strtmp)  
                    print(strtmp) 
        return strtmp

    # 作者介绍
    def GetAuthor(self,div):
        dd = div.find("dd",class_="zz")
        # 去除标签
        [s.extract() for s in dd("span")] 
        [s.extract() for s in dd("a")] 
        
        # strtmp = str(dd.contents)
        strtmp = dd.get_text()

        strtmp = self.RemoveSpace(strtmp)  
        print(strtmp)
        return strtmp

    def SaveJson(self,filePath,dataRoot):  
        # 保存json
        with open(filePath, 'w') as f:
            json.dump(dataRoot, f, ensure_ascii=False,indent=4,sort_keys = True)

    
 
parse = ParsePoem()
# parse.Run()
parse.CreateDB()
parse.MakeGuanka()



     

