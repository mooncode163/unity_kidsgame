from selenium import webdriver
from selenium.webdriver import ActionChains
from selenium.webdriver.common.keys import Keys
from selenium.webdriver.chrome.options import Options
from selenium.webdriver.common.by import By
from urllib.request import urlopen

from urllib import request
from urllib.parse import quote
import string

import sqlite3 
import json
import time

from DB.DBWord import DBWord

from WordInfo import WordInfo 
from WebDriverCmd import WebDriverCmd 

import ssl
# 跳过ssl证书
ssl._create_default_https_context = ssl._create_unverified_context

#古诗网 http://www.cquctt.cn/ 
WEB_HOME_URL = "http://www.cquctt.cn"
# https://hanyu.baidu.com/s?wd=%E6%88%91&ptype=zici
class ParseWord():
    title =""
    author =""
    listSort = []
    dbWord = None
    dbWordZuci = None
    driver: None 

    def InitWebDriver(self): 
        # 创建chrome浏览器驱动，无头模式（超爽）
        chrome_options = Options()
        # chrome_options.add_argument('--headless')
        ua = "Mozilla/5.0 (Linux; U; Android 3.0; en-us; Xoom Build/HRI39) AppleWebKit/534.13 (KHTML, like Gecko) Version/4.0 Safari/534.13"
        chrome_options.add_argument("user-agent="+ua)
        self.driver = webdriver.Chrome(chrome_options=chrome_options)
        # 全屏
        self.driver.maximize_window()
        # 具体大小
        # driver.set_window_size(width, height)

        # self.GoHome()
        # self.Login()
        # time.sleep(2)
        # GoAppgallery(driver)

        #     # 快照显示已经成功登录
        # print(driver.save_screenshot('jietu.png'))
        # driver.quit()

        chrome_options = Options()
        # chrome_options.add_argument('--headless')
        # chrome_options.add_argument("user-agent="+ua)
        self.driver2 = webdriver.Chrome(chrome_options=chrome_options)
        # 全屏
        self.driver2.maximize_window()


    def Quit(self,delay):
        time.sleep(delay) 
        self.driver.quit()
        time.sleep(1)
    def LoadJson(self,filepath):  
        data = None
        with open(filepath, 'rb') as json_file:
            data = json.load(json_file)
            return data
 
    def Init(self):
        self.InitWebDriver()
        self.CreateDB()

    def CreateDB(self):
        self.dbWord = DBWord()
        self.dbWord.OpenDB("../../../../../../ResourceData/LearnWord/LearnWord/GameRes/Word.db")
 
        self.dbWordZuci = DBWord() 
        self.dbWordZuci.OpenDB("../../../../../../ResourceData/LearnWord/LearnWord/GameRes/WordZuci.db")
    # def Run(self):
    #     url = WEB_HOME_URL
    #     html = self.GetHtml(url)
    #     self.ParseSort(html)
    #     for d in self.listSort:
    #         name = d["title"]
    #         url = d["url"]
    #         page = self.GetTotalPage(url)
    #         print("page=",page)
    #         listPoem = []
    #         for i in range(page):
    #             url_page = self.GetUrlPage(url,i)
    #             print("url_page=",url_page)
    #             listtmp = self.ParseWordList(url_page)
    #             listPoem+=listtmp

    #         jsonroot = dict (items=listPoem)
    #         self.SaveJson("OutPut/"+name+".json",jsonroot) 
 
     
    def GetHtml(self,url):
        s = quote(url,safe=string.printable)
        response = request.urlopen(s)
        # return  urlopen(url).read().decode('utf-8',"ignore")
        # python3 出现'ascii' codec can't encode characters
        # https://blog.csdn.net/weixin_30819163/article/details/95509156
        return  response.read() 

    def ParseWordList(self):
        jsonRoot = self.LoadJson("word.json")
        jsonItems = jsonRoot["items"]
        idx = 0
        for item in jsonItems: 
            word = item["title"] 
            imagetitle = item["detail"]
            print(" word =",word," idx=",idx)
            self.ParseWordInfo(word,imagetitle)
            idx += 1
 

        # jsonroot = dict (items=self.listSort)
        # self.SaveJson("OutPut/sort.json",jsonroot)
  

    def RemoveAllHtmlTag(self,tag):
        # 去除所有标签符号
        # [s.extract() for s in tag("")]
        # print(tag.text)
        idx = 0
 

    def GetLiById(self,webcmd,key): 
        li = webcmd.Find("//li[@id='"+key+"']")
        if li is None:
            return ""
        span = webcmd.FindChild(li,".//span")  
        return span.text


    def ParseWordInfo(self,word,imagetitle):
        if self.dbWord.IsItemExist(word)==True:
            return

        webcmd = WebDriverCmd(self.driver)
        url = "https://hanyu.baidu.com/s?wd="+word+"&ptype=zici"
        self.driver.get(url)
        # print(url)
        info = WordInfo()
        info.imagetitle = imagetitle 
        info.title = word

        item = webcmd.Find("//div[@id='pinyin']",True) 
        a = webcmd.FindChild(item,".//a")  
        info.audio = a.get_attribute('url')  
        # print(info.audio) 
        b = webcmd.FindChild(item,".//b")   
        info.pinyin = b.get_attribute("innerText")
        
        # info.pinyin = "wǒ"
        # print(item.get_attribute("innerHTML"))
        print("pinyin=",info.pinyin) 
 
        info.bushou = self.GetLiById(webcmd,"radical")
        info.fanti = self.GetLiById(webcmd,"traditional") 
        # info.id = self.GetLiById(soup,"wubi")
        info.id = info.title
        info.bihuaCount = self.GetLiById(webcmd,"stroke_count")
 
# <div class="word_stroke_order c-line-clamp1"><span>笔顺 ：</span>
#                                         &nbsp;
#                                         &nbsp;
#                                         &nbsp;
#                                         &nbsp;
#                                         &nbsp;
#                                         &nbsp;
#                                         &nbsp;
#                                         &nbsp;
#                                         &nbsp;
#                                         &nbsp;
#                                         &nbsp;
#                                     </div>
# <div class="word_stroke_order" id="stroke"><span>名称 ：</span>
#                                         点、
#                                         横、
#                                         撇、
#                                         横折、
#                                         竖、
#                                         竖、
#                                         横、
#                                         横、
#                                         竖提、
#                                         撇、
#                                         竖弯钩、
#                                     </div>
 
# 笔顺 ：                   
# 名称 ： 点、 提、 撇、 竖、 点、 横、 横、 横、 竖、 横、
        key = "//div[@class='word_stroke_order c-line-clamp1']"  
        item = webcmd.Find(key)
        # print(item.get_attribute("innerHTML"))
        stritem = item.text
        strhead = "笔顺 ： "
        idx = stritem.find(strhead)
        if idx >=0:
            idx+=len(strhead)
            stritem = stritem[idx:]
        print(stritem)
        info.bihuaOrder = stritem
        print("丿")

        key = "//div[@id='stroke']" 
        item = webcmd.Find(key)
        # print(item.get_attribute("innerHTML"))
        stritem = item.text
        strhead = "名称 ： "
        idx = stritem.find(strhead)
        if idx >=0:
            idx+=len(strhead)
            stritem = stritem[idx:]
        print(stritem)
        info.bihuaName = stritem

        
        key = "//img[@id='word_bishun']"

        if webcmd.IsElementExist(key):
            img = webcmd.Find(key)
        # https://hanyu-word-gif.cdn.bcebos.com/ba0a041b62c4347f0ba3f4e086316baa8.gif
            info.gif = img.get_attribute('data-gif') 
        print(info.gif)


        # div_mean = webcmd.Find("//div[@id='basicmean-wrapper']")   
        # if div_mean is None:
        div = webcmd.Find("//div[@id='base-mean']")   
        
        # div = webcmd.FindChild(div_mean,".//div[contains(@class,'tab-content')]")  
        if div is None:
            print("not find tab-content srow")
        else:
            idx = 0
            list_p = webcmd.FindListChild(div,".//p")  
            for p in list_p: 
                # p = p.extract()
                # tmp = [s.extract() for s in p('span')]
                # print(tmp)
                # [s.extract() for s in p('span')]
            
                # 去除所有标签符号 
                self.RemoveAllHtmlTag(p) 

                if idx==(len(list_p)-1):
                    info.mean+=p.text
                else:
                    info.mean+=p.text+"\n"
            
                idx=idx+1 

            

            # print(info.mean)

        div = webcmd.Find("//div[@id='zuci-wrapper']")       
        list_a = webcmd.FindListChild(div,".//a") 
        idx = 0
        for a in list_a:
            strtmp = a.text
            # print(str," idx=",idx," len=",len(list_a))
            if idx==(len(list_a)-1):
                continue
             
            ret = self.ParseMultiWordInfo(strtmp)
            print("zuci:str=",strtmp," ret=",str(ret))
             
            if idx==(len(list_a)-2):
                info.zuci+=strtmp
            else:
                info.zuci+=strtmp+";"

            idx=idx+1

        print(info.zuci)  
        self.dbWord.AddItem(info)
        return info
            

    def ParseMultiWordInfo(self,word):
        if self.dbWordZuci.IsItemExist(word)==True:
            return False
        webcmd = WebDriverCmd(self.driver2)

        # url = "https://hanyu.baidu.com/s?wd="+word+"&ptype=zici"
        url =  "https://hanyu.baidu.com/s?wd="+word+"&cf=zuci&ptype=term"
        self.driver2.get(url)
        time.sleep(2)
        # print(url)
        info = WordInfo()  

        info.title = word
        info.id = word

        key = "//div[@id='pinyin']"
        if webcmd.IsElementExist(key)==False:
            # https://hanyu.baidu.com/s?wd=%E9%BA%8B%E9%B9%BF&cf=zuci&ptype=term
            return False
        item = webcmd.Find(key)  

        try: 
            a = webcmd.FindChild(item,".//a")  
            b = webcmd.FindChild(item,".//b")   
        except Exception as e:
            print(e)  # 打印所有异常到屏幕
            return False

        info.audio = a.get_attribute('url')  
        # print(info.audio)       
        # info.pinyin = b.text
        info.pinyin = b.get_attribute("innerText")
        # print(info.pinyin) 
            
        # [ yī tóng ]
        info.pinyin = info.pinyin.replace("[ ","")
        info.pinyin = info.pinyin.replace(" ]","")
        # print(info.pinyin) 
 
      
 
        div = None
        div_mean = None
        try: 
            div = webcmd.Find("//div[@id='base-mean']")  
        except Exception as e:
            print(e)  # 打印所有异常到屏幕
            print(url)
            return False

        
        if div is None:
            print("not find tab-content ParseMultiWordInfo")
            print(url)
        else:
            idx = 0 
            list_p = webcmd.FindListChild(div,".//p") 
            for p in list_p: 
                # p = p.extract()
                # tmp = [s.extract() for s in p('span')]
                # print(tmp)
                # [s.extract() for s in p('span')]
            
                # 去除所有标签符号 
                self.RemoveAllHtmlTag(p) 

                if idx==(len(list_p)-1):
                    info.mean+=p.text
                else:
                    info.mean+=p.text+"\n"
            
                idx=idx+1 

            
            info.mean = info.mean.replace(" ","")
            # print(info.mean)

        
        self.dbWordZuci.AddItem(info)
 
        return True


    def SaveJson(self,filePath,dataRoot):  
        # 保存json
        with open(filePath, 'w') as f:
            json.dump(dataRoot, f, ensure_ascii=False,indent=4,sort_keys = True)

    
 
parse = ParseWord()
parse.Init()
# parse.Run()
# parse.CreateDB()
# parse.MakeGuanka() 
parse.ParseWordList() 


     

