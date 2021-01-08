import os
import time
import json
from appium import webdriver
from AppiumWebDriverCmd import AppiumWebDriverCmd
from AppiumWebDriverCmd import CmdType
 

# com.bruce.baby:id/show_lesson_description
# com.bruce.baby:id/btn_app_back


# 模拟手指点击操作
# driver.tap([(918,413),(1026,521)], 100)

class WordInfo(object):
    title: None
    detail: None
    index: None
    page: None
   


class ParseBookWord():
    listWord: None
    listBook: None
    bookType: None
    bookName: None
    driver: None
    fileJosn = "bookword.json"
    timeOutCount = 10

    def appium_start(self, host, port):
        bootstrap_port = str(port + 1)
        cmd = 'appium -a %s -p %s' % (host, port)
        os.system(cmd)

    # 构造函数
    def Init(self):
        host = '127.0.0.1'
        port = 4723
        # self.appium_start(host, port)

        # self.LoadJson()
        desired_caps = {'platformName': 'Android',
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

        self.driver = webdriver.Remote(
            'http://localhost:4723/wd/hub', desired_caps)  # 启动app
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
        word = webcmd.FindByID(
            "com.bruce.baby:id/word_list", True, self.timeOutCount)
        if word == None:
            self.ReStart()

    def GotoTabBook(self,book):
        webcmd = AppiumWebDriverCmd(self.driver)
        key = "//*[@elementId='acbcbdb0-ef6c-481c-bf93-b8c36fb1b8e4' and @class='android.widget.ImageView']"
        # key = "//*[@class='android.widget.ImageView']"
        
        key ="/hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.LinearLayout/android.widget.LinearLayout/android.widget.LinearLayout/android.widget.LinearLayout[2]/android.widget.ImageView"
        webcmd.AddCmd(CmdType.CLICK,key)
        webcmd.Run(True)

        # 选择教材版本
        key = "/hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.LinearLayout/android.support.v4.view.ViewPager/android.widget.LinearLayout/android.widget.LinearLayout/android.widget.Button"
        key = 'com.bruce.baby:id/ib_show_version'
        webcmd.AddCmdById(CmdType.CLICK,key) 
        webcmd.Run(True)
        # com.bruce.baby:id/tv_rjb
        # com.bruce.baby:id/tv_sjb
        # com.bruce.baby:id/tv_hjb
        # com.bruce.baby:id/tv_bsdb
        key = 'com.bruce.baby:id/'+book
        
        webcmd.AddCmdById(CmdType.CLICK,key)

        webcmd.Run(True)

    def GetBookList(self):
        name = ""
        lastname = ""
        count = 0
        self.listBook = []
        while True:
            name = self.GetBookListInternal()
            if name!=lastname:
                lastname = name
            else:
                
                count=count+1
                if count>=3:
                    # end
                    print("GetBookList end len=",len(self.listBook)) 
                    # save book list
                    self.MoveList(True,5)  
                    print("GetBookList MoveList topest") 
                    break

            self.MoveList(False)  


        for i in range(len(self.listBook)): 
            self.bookName = self.listBook[i].title
            self.jsonFileWordList = "OutPut/"+ self.bookType+"_"+self.bookName+".json"
            if not os.path.exists(self.jsonFileWordList):
                self.GotoBook(self.bookName)
                time.sleep(2)

        
        filepath ="OutPut/"+ self.bookType+".json"
        self.SaveBookList(filepath)

         
    def IsInBookList(self,name):
        for info in self.listBook:
            if info.title == name:
                return True

        return False

    # com.bruce.baby:id/grade_image
    def GetBookListInternal(self):
        webcmd = AppiumWebDriverCmd(self.driver)  
        key =  "//*[@resource-id='com.bruce.baby:id/grade_image']"

        list = self.driver.find_elements_by_xpath(key)
        print("book len = ",len(list))
        for item in list:
            print(item.text)

        key =  "//*[@resource-id='com.bruce.baby:id/grade_name']"
        list = self.driver.find_elements_by_xpath(key)
        print("book len = ",len(list))
        name = ""
        for item in list:
            name = item.text
            if not self.IsInBookList(name):
                info = WordInfo()
                info.title = name
                self.listBook.append(info)
                print(item.text)

        return name
         
#resource-id com.bruce.baby:id/tv_course_title
# com.bruce.baby:id/word_list_item
# back 	com.bruce.baby:id/leftButton
    def GetLastBookWordTitle(self, page):
        webcmd = AppiumWebDriverCmd(self.driver)  
        key =  "//*[@resource-id='com.bruce.baby:id/tv_course_title']"
        list = self.driver.find_elements_by_xpath(key)
        count = len(list)
        print("GetLastBookWordTitle len = ",len(list))
        for item in list:
            print(item.text)
        
        if count>0:
            return list[count-1].text


    def GetWordList(self):
        print("GetWordList enter") 
        name = ""
        lastname = ""
        count = 0
        self.listWord = []
        while True:
            name = self.GetWordListInternal()
            if name!=lastname:
                lastname = name
            else:
                
                count=count+1
                if count>=3:
                    # end
                    print("GetWordList end len=",len(self.listWord))   
                    self.SaveWordList(self.jsonFileWordList)

                    self.OnClickBtnBack()  
                    break

            self.MoveList(False)  

 

    def GetWordListInternal(self):
        webcmd = AppiumWebDriverCmd(self.driver)
        key =  "//*[@resource-id='com.bruce.baby:id/tv_course_title']"
        list = self.driver.find_elements_by_xpath(key) 
        print("word len = ",len(list))
        idx =0
        strret = ""
        strnum = "0123456789"
        for item in list:
            
            strret = item.text
            title = strret
            for num in strnum:
                title = title.replace(num,"")

            print(title) 
            # cell 
            key =  "/hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.LinearLayout[2]/android.widget.LinearLayout[2]/android.support.v4.view.ViewPager/android.widget.LinearLayout/android.widget.ListView/android.widget.LinearLayout["+str(idx+1)+"]"
            item = self.driver.find_element_by_xpath(key) 
            key =  ".//*[@resource-id='com.bruce.baby:id/word_list_item']"
            list = item.find_elements_by_xpath(key) 
            # print("sub word len = ",len(list)) 
            detail = ""
            for item in list:
                # print(item.text)  
                detail=detail+item.text

            print(detail)
            
            if not self.IsInWordList(title):
                info = WordInfo()
                info.title = title
                info.detail = detail
                self.listWord.append(info)


            idx=idx+1 


        
        return strret

    def GotoBook(self, bookname):
        print("GotoBook bookname=",bookname) 
        webcmd = AppiumWebDriverCmd(self.driver)  
        itemSel = None
        while True:
            key =  "//*[@resource-id='com.bruce.baby:id/grade_name']"
            list = self.driver.find_elements_by_xpath(key)  
            isFind = False
            for item in list:
                name = item.text 
                if name==bookname:
                    isFind = True
                    itemSel = item
                    time.sleep(1)
                    break
            
            if isFind:
                break

            self.MoveList(False) 
            
    
        if isFind:
            itemSel.click()

        # key =  "//*[@resource-id='com.bruce.baby:id/grade_image']"
        # list = self.driver.find_elements_by_xpath(key)
        # print("book len = ",len(list))
        # idx =0
        # for item in list:
        #     print(item.text)
        #     if idx==index:
        #         self.bookName = item.text
        #         item.click()
        #         break

        #     idx=idx+1

        #
        time.sleep(2)
 
        self.TryGetWordList()  


    def TryGetWordList(self):
        try:
           self.GetWordList() 
        except Exception as e:
            print(e)  # 打印所有异常到屏幕 
            print("GetWordList error ") 
            self.SaveWordList(self.jsonFileWordList)
            # self.TryGetWordList() 



    def IsInWordList(self, title):
        for info in self.listWord:
            if info.title == title:
                return True
        return False

    def SaveString2File(self, str, file):
        f = open(file, 'wb')  # 若是'wb'就表示写二进制文件
        b = str.encode('utf-8', "ignore")
        f.write(b)
        f.close()

    def GetFileString(self, filePath):
        f = open(filePath, 'rb')
        strFile = f.read().decode('utf-8', "ignore")
        f.close()
        return strFile

    def LoadJson(self):
        self.listWord = []
        if os.path.exists(self.fileJosn) == False:
            return

        strjson = self.GetFileString(self.fileJosn)
        dataRoot = json.loads(strjson)
        dataItems = dataRoot["items"]
        for item in dataItems:
            info = WordInfo()
            info.title = item["title"]
            info.detail = item["detail"]
            info.index = item["index"]
            info.page = item["page"]
            self.listWord.append(info)

        print("json word count =", len(dataItems))
        count = 200
 
    def SaveBookList(self, filepath):
        dataRoot = json.loads("{}")
        dataItems = []
        for info in self.listBook:
            item = json.loads("{}")
            item["title"] = info.title 
            dataItems.append(item)

        dataRoot["items"] = dataItems

        json_str = json.dumps(dataRoot, ensure_ascii=False,
                              indent=4, sort_keys=True)
        self.SaveString2File(json_str, filepath)
        return False

    def SaveWordList(self, filepath):
        dataRoot = json.loads("{}")
        dataItems = []
        for info in self.listWord:
            item = json.loads("{}")
            item["title"] = info.title
            item["detail"] = info.detail 
            dataItems.append(item)

        dataRoot["items"] = dataItems

        json_str = json.dumps(dataRoot, ensure_ascii=False,
                              indent=4, sort_keys=True)
        self.SaveString2File(json_str, filepath)
        return False
 
 
 

    def MoveList(self,isUp,count=1):
        webcmd = AppiumWebDriverCmd(self.driver)
        for i in range(count):
            if isUp:
                webcmd.SwipeDown()
            else:
                webcmd.SwipeUp()

        isEnd = False
        time.sleep(1)
        return isEnd


    def IsMoveListBottom(self):
        return False

    def Run(self):
        # com.bruce.baby:id/tv_rjb
        # com.bruce.baby:id/tv_sjb
        # com.bruce.baby:id/tv_hjb
        # com.bruce.baby:id/tv_bsdb
        list = ["tv_rjb","tv_sjb","tv_hjb","tv_bsdb"]
        for book in list:
            self.RunBook(book)

    def RunBook(self,book):
        self.bookType = book
        filepath ="OutPut/"+ self.bookType+".json"
        if os.path.exists(filepath):
            return

        # print(driver.page_source)
        # com.bruce.baby:id/word_list_item word_list
        try:
            self.Init()
            count = 1
            self.GotoTabBook(book)
            self.GetBookList()
            

            # self.GetWordList()
                # self.Save(self.fileJosn)
                # self.OnClickBtnNext()

            # self.Save(self.fileJosn)

        except Exception as e:
            print(e)  # 打印所有异常到屏幕
            print("error need ReStart")
            self.ReStart()

    def ReStart(self):
        print("ReStart")
        # self.Save(self.fileJosn)
        self.driver.quit()
        self.Run()


# 主函数的实现
if __name__ == "__main__":
    # 入口参数：http://blog.csdn.net/intel80586/article/details/8545572
    p = ParseBookWord()
    p.Run()
