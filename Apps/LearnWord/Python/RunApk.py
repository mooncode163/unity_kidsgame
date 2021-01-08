import os  
import time  
import json
from appium import webdriver   
from AppiumWebDriverCmd import AppiumWebDriverCmd
	
# com.bruce.baby:id/show_lesson_description
# com.bruce.baby:id/btn_app_back


#模拟手指点击操作  
# driver.tap([(918,413),(1026,521)], 100) 

class WordInfo(object):  
    title:None
    detail:None
    index:None 
    page:None 

class RunApk():  
    listWord:None
    driver: None
    fileJosn = "word.json"
    timeOutCount = 10

    def appium_start(self,host, port):
        bootstrap_port = str(port + 1)
        cmd = 'appium -a %s -p %s'%(host, port)
        os.system(cmd)

    #构造函数
    def Init(self): 
        host = '127.0.0.1'
        port = 4723
        # self.appium_start(host, port)

        self.LoadJson()
        desired_caps ={ 'platformName': 'Android',  
                        'platformVersion': '9.0.0',  
                        'deviceName': 'V1813BA',  
                        'noReset': True,  
                        'appPackage': 'com.bruce.baby',
                        'appActivity': 'com.bruce.baby.SplashActivity',  
                        # com.bruce.baby
                        # 'appActivity': 'com.bruce.baby.SplashActivity',  
                        # com.bruce.baby.activity.MainTabActivity
                        # 'appPackage': 'com.moonma.shapecolor',  
                        # 'appActivity': 'com.moonma.unity.MainActivity',  
                        # 'unicodeKeyboard': True,  
                        # 'resetKeyboard': True  
                        }  
      
        self.driver = webdriver.Remote('http://localhost:4723/wd/hub', desired_caps)#启动app   
        print(self.driver.current_activity) 
        # driver.start_activity('com.bruce.baby', '.activity.MainTabActivity')
        # 等主页面activity出现,30秒内
        self.driver.wait_activity(".activity.MainTabActivity", 30) 
        print(self.driver.current_activity) 
      


    def OnClickBtnNext(self):
        key = "/hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.LinearLayout/android.support.v4.view.ViewPager/android.widget.LinearLayout/android.widget.ScrollView/android.widget.LinearLayout/android.widget.LinearLayout[2]/android.widget.ImageButton[2]"
        item = self.driver.find_element_by_xpath(key)
        item.click()
        # time.sleep(1)

    def OnClickBtnBack(self):
        webcmd = AppiumWebDriverCmd(self.driver)
        item = self.driver.find_element_by_id("com.bruce.baby:id/btn_app_back")   
        item.click()
        # time.sleep(1)
        word = webcmd.FindByID("com.bruce.baby:id/word_list",True,self.timeOutCount)
        if word==None:
            self.ReStart()

    def GetWordList(self): 
        webcmd = AppiumWebDriverCmd(self.driver)
        word_list = webcmd.FindListByID("com.bruce.baby:id/word_list_item",True,self.timeOutCount)
        if word_list==None:
            self.ReStart()

        # 第 1 页
        page = webcmd.FindByID("com.bruce.baby:id/tv_page_number",True,self.timeOutCount)
        if page==None:
            self.ReStart()
        strpage = page.text
        strpage = strpage.replace("第 ","")
        strpage = strpage.replace(" 页","")
        pagevalue = int(strpage)

        i = 0
        for item in word_list:
            print(item.text)
            self.GotoWord(item,pagevalue,i)
            i=i+1

    def IsInWordList(self,word,pagevalue):
        for tmp in self.listWord:
            if tmp.title == word and tmp.page==pagevalue:
                return True
        return False

    def SaveString2File(self,str, file):
        f = open(file, 'wb')  # 若是'wb'就表示写二进制文件
        b = str.encode('utf-8',"ignore")
        f.write(b)
        f.close()

    def GetFileString(self,filePath): 
        f = open(filePath, 'rb')
        strFile = f.read().decode('utf-8',"ignore")
        f.close()
        return strFile

    def LoadJson(self):
        self.listWord = []
        if os.path.exists(self.fileJosn)==False:
            return

        strjson = self.GetFileString(self.fileJosn)
        dataRoot = json.loads(strjson)
        dataItems = dataRoot["items"]
        for item in dataItems: 
            info = WordInfo()
            info.title=item["title"]
            info.detail=item["detail"]
            info.index=item["index"]
            info.page=item["page"]
            self.listWord.append(info)

        print("json word count =",len(dataItems))
        count = 200
        for i in range(count-1):
            pagecount = self.GetPageCount(i+1)
            if pagecount<16:
                print("pagecount=",str(pagecount)," page=",str(i+1))


    def Save(self,filepath):
        dataRoot = json.loads("{}")
        dataItems = []
        for info in self.listWord:
            item= json.loads("{}")
            item["title"]=info.title
            item["detail"]=info.detail
            item["index"]=info.index
            item["page"]=info.page
            
            dataItems.append(item)

        dataRoot["items"]=dataItems
        
        json_str = json.dumps(dataRoot,ensure_ascii=False,indent=4,sort_keys = True)
        self.SaveString2File(json_str,filepath)
        return False

    def GotoWord(self,item,pagevalue,i):
        webcmd = AppiumWebDriverCmd(self.driver)
        title = item.text
        if self.IsInWordList(title,pagevalue):
            return
        # print(title)
        item.click()
        word = webcmd.FindByID("com.bruce.baby:id/show_lesson_description",True,self.timeOutCount)
        if word==None:
            self.ReStart()
 
        # time.sleep(3)
        # word = self.driver.find_element_by_id("com.bruce.baby:id/show_lesson_description")

        detail = word.text
        print(detail)
        index_list = len(self.listWord)+1
        index = (pagevalue-1)*16+(i+1)
        if index_list!=index:
            print("word error title=",title," page=",pagevalue)

        info = WordInfo()
        info.title = title
        info.detail = detail
        info.index = index
        info.page = pagevalue
        print("index=",str(index))
        if self.IsInWordList(info.title,pagevalue)==False:
            self.listWord.append(info)

        # self.Save(self.fileJosn)
        self.OnClickBtnBack()

    def GetPageCount(self,page): 
        count = 0
        for info in self.listWord:
            if page==info.page:
                count=count+1
        return count

    def Run(self): 
        
# print(driver.page_source)
# com.bruce.baby:id/word_list_item word_list
        try:
            self.Init() 
            count = 200
            for i in range(count-1):
                pagecount = self.GetPageCount(i+1)
                if pagecount<16:
                    print("pagecount=",str(pagecount)," page=",str(i+1))

                self.GetWordList()
                # self.Save(self.fileJosn)
                self.OnClickBtnNext() 
            
            
            self.Save(self.fileJosn)
                
        except Exception as e:
            print(e) #打印所有异常到屏幕
            self.ReStart()

    def ReStart(self): 
        print("ReStart")
        self.Save(self.fileJosn)
        self.driver.quit() 
        self.Run()

# 主函数的实现
if __name__ == "__main__": 
    # 入口参数：http://blog.csdn.net/intel80586/article/details/8545572
    p = RunApk()
    p.Run()