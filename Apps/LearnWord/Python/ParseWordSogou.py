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
class ParseWordSogou():
    title =""
    author =""
    listSort = []
    dbWord = None
    dbWordZuci = None
    driver: None 

    def InitWebDriver(self): 
        # 创建chrome浏览器驱动，无头模式（超爽）
        chrome_options = Options()
        chrome_options.add_argument('--headless')
        ua = "Mozilla/5.0 (Linux; U; Android 3.0; en-us; Xoom Build/HRI39) AppleWebKit/534.13 (KHTML, like Gecko) Version/4.0 Safari/534.13"
        # chrome_options.add_argument("user-agent="+ua)
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
        chrome_options.add_argument('--headless')
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


# https://hanyu.sogou.com/result?query=%E4%BB%8A
    def ParseWordInfo(self,word,imagetitle):
        if self.dbWord.IsItemExist(word)==True:
            return

        info = WordInfo()
        info.imagetitle = imagetitle 
        info.title = word
        info.id = info.title


        if "词"==word:
            info.pinyin = "cí"
            info.audio ="https://fanyiapp.cdn.bcebos.com/zhdict/mp3/ci2.mp3"
            info.bushou = "讠"
            info.bihuaCount = "7"
            info.fanti = "詞"
            info.wubi = "YNGK"
            info.bihuaOrder = ""
            info.bihuaName = "点 横折提 横折钩 横 竖 横折 横 "
            info.meanBasic = "1.（～儿）说话或诗歌、文章、戏剧中的语句：戏～。义正～严。～不达意。他问得我没～儿回答。2.一种韵文形式，由五言诗、七言诗和民间歌谣发展而成，起于唐代，盛于宋代。原是配乐歌唱的一种诗体，句的长短随着歌调而改变，因此又叫做长短句。有小令和慢词两种，一般分上下两阕。3.语言里最小的、可以自由运用的单位。"
            info.meanDetail = "(形声。从言,司声。本义:言词。按,“辞”、“词”在“言词”这个意义上是同义词。但在较古的时代,一般只说“辞”,不说“词”。汉代以后逐渐以“词”代“辞”)同本义 [one's words;what one say]词,意内而言外也。——《说文》词色甚强。——《世说新语·轻诋》听妇前致词:三男邺城戍。—— 杜甫《石壕吏》门者故不入,则甘言媚词,作妇人状,袖金以私之。——宗臣《报刘一丈书》又如:名词;动词;形容词;词色(声色);词锋(犀利的文笔,好像刀剑的锋芒);词不达意诗文中的词语 [sentences in speeches,poems,writings or operas;speech;statement]纵豆蔻词工,青楼梦好,难赋深情。——姜夔《扬州慢》又如:词科(文词科场;科举考场);词场(文坛、文苑、文辞荟萃的地方;文词科试的场所);词翰(词章);词章(文辞的通称。后用以专称诗赋文章而言状纸;诉讼次日,一乘轿子抬到县门口,正值知县坐早堂,就喊了冤,知县叫补进词来。——《儒林外史》又如:词状(状词;诉状);词因(原因,情由。多指供词,讼状所陈述的内容);词讼(同辞讼。诉讼)文体名,诗歌的一种 [ci—classical poetry conforming to a definite pattern]。一种韵文形式,由五言诗、七言诗或民间歌谣发展而成,起于唐代,盛于宋代。原是配乐歌唱的一种诗体,句的长短随歌调而改变,因此又叫长短句。有小令和慢词两种,一般分上下两阕"
            info.zuci = "锐词;热词;诗词;"
            self.dbWord.AddItem(info)
 
            return

        webcmd = WebDriverCmd(self.driver)
        # word = "他"
        url = "https://hanyu.sogou.com/result?query="+word
        self.driver.get(url)
        # print(url)


        item = webcmd.Find("//a[@class='voice-play text-default no-underline']",True) 
        span = webcmd.FindChild(item,".//span")  
        # [ jīn ]
        strtmp = span.text
        strtmp = strtmp.replace("[ ","")
        strtmp = strtmp.replace(" ]","")
        info.pinyin = strtmp
        print("pinyin=",info.pinyin)

        key = ".//i"
        if webcmd.IsElementChildExist(item,key):
            i = webcmd.FindChild(item,key)
            info.audio = i.get_attribute('data-src')  
            print(info.audio) 
        
#    <div class="details"><span>部首：人</span> <span>笔画：4</span> <span>繁体：今</span> <span>五笔：WYNB</span></div>
        div = webcmd.Find("//div[@class='details']")
        list_span = webcmd.FindListChild(div,".//span")
        
        span = list_span[0]
        text = span.text
        text = text.replace("部首：","")
        info.bushou = text
        # print(text)

        span = list_span[1]
        text = span.text
        text = text.replace("笔画：","")
        if "六"==word:
            text = "4"
        info.bihuaCount = text
        # print(text)


        span = list_span[2]
        text = span.text
        text = text.replace("繁体：","")
        info.fanti = text
        # print(text)

        span = list_span[3]
        text = span.text
        text = text.replace("五笔：","")
        info.wubi = text
        # print(text)

        key = "//div[@class='bishun']"
        div = webcmd.Find(key)
        list_span = webcmd.FindListChild(div,".//span")
        # print("list_span=",len(list_span))
        idx = 0
        
        text = list_span[0].text
        text = text.replace("笔顺：","")
        info.bihuaOrder = text

        # for span in list_span:
        #     text = span.text
        #     print(text)
        #     if idx>0:
        #         if idx % 2 == 0:
        #             info.bihuaOrder+=text+"、"

        #     idx+=1

        print(info.bihuaOrder)

        key = "//div[@class='designation']"
        if webcmd.IsElementExist(key):
            div = webcmd.Find(key)
            list_span = webcmd.FindListChild(div,".//span")
            # print("list_span=",len(list_span))
            idx = 0
            for span in list_span:
                text = span.text
                # print("span = ",text)
                if idx>0:
                    info.bihuaName+=text+"  "

                idx+=1
        else:
            info.bihuaName = info.bihuaOrder
        
        # print(info.bihuaName)


        key = "//div[@id='shiyiDiv']"
        div = webcmd.Find(key)
        li = webcmd.FindChild(div,".//li")
        info.meanBasic = li.text
        # print(info.mean)


        key = "//li[@id='shiyidetailDiv']"
        if webcmd.IsElementExist(key):
            li = webcmd.Find(key) 
            info.meanDetail = li.text
            # print(info.meanDetail)    

        key = "//div[@id='fyc']"
        if webcmd.IsElementExist(key):
            div = webcmd.Find(key) 
            list = webcmd.FindListChild(div,".//li/a")
            for a in list:
                text = a.text  
                info.antonym+=text+";"

        # key = "//div[@id='fyc']"
        # if webcmd.IsElementExist(key):
        #     div = webcmd.Find(key) 
        #     list = webcmd.FindListChild(div,".//li/a")
        #     for a in list:
        #         text = a.text  
        #         info.homoionym+=text+";"
 

        key = "//div[@id='zuci']"
        if webcmd.IsElementExist(key):
            div = webcmd.Find(key) 
            list = webcmd.FindListChild(div,".//li")
            for li in list:
                a = webcmd.FindChild(li,".//a")
                text = a.text 
                # print(text)
                info.zuci+=text+";"
        
        # print(info.zuci)
 
   
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
                    info.meanBasic+=p.text
                else:
                    info.meanBasic+=p.text+"\n"
            
                idx=idx+1 

            
            info.meanBasic = info.meanBasic.replace(" ","")
            # print(info.mean)

        
        self.dbWordZuci.AddItem(info)
 
        return True


    def SaveJson(self,filePath,dataRoot):  
        # 保存json
        with open(filePath, 'w') as f:
            json.dump(dataRoot, f, ensure_ascii=False,indent=4,sort_keys = True)

    
 
parse = ParseWordSogou()
parse.Init()
# parse.Run()
# parse.CreateDB()
# parse.MakeGuanka() 
parse.ParseWordList() 


     

