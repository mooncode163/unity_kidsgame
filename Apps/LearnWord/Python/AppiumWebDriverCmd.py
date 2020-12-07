# 导入selenium的浏览器驱动接口 

import sys
import os
import json
o_path = os.getcwd()  # 返回当前工作目录
sys.path.append(o_path)  # 添加自己指定的搜索路径

from appium import webdriver   

#  元素定位 https://www.cnblogs.com/cnkemi/p/9180525.html
import time 

class CmdType(object): 
    CLICK = "click"
    CLICK_SCRIPT = "click_script"
    CLICK_Action = "CLICK_Action"
    CLICK_List_ALL = "CLICK_List_ALL"
    CLICK_List_Item = "CLICK_List_Item"
    INPUT = "input" 
    INPUT_CLEAR = "input_clear" 
    ENTER = "enter"
    # 粘贴
    CTR_V = "control_v"
    

class CmdInfo(object):  
    type:None
    type2:None
    cmd:None
    value:None
    delay:None
    isWaiting:None
    item:None
    index=0





class AppiumWebDriverCmd():  
    listCmd:None
    driver: None

    #构造函数
    def __init__(self,webdv):
        self.listCmd= []
        self.driver = webdv

    def AddCmd(self,type,cmd,value="",delay=1):
        info = CmdInfo()
        info.type = type
        info.cmd = cmd
        info.value = value
        info.delay = delay
        info.isWaiting = False
        return self.AddCmdInfo(info)

    def AddCmdWait(self,type,cmd,value="",delay=1):
        info = CmdInfo()
        info.type = type
        info.cmd = cmd
        info.value = value
        info.delay = delay
        info.isWaiting = True
        return self.AddCmdInfo(info)

    def AddCmdList(self,type2,cmd,index=0,delay=1):
        info = CmdInfo()
        info.type = CmdType.CLICK_List_Item
        info.type2 = type2
        info.cmd = cmd
        info.index = index
        info.delay = delay
        info.isWaiting = False
        return self.AddCmdInfo(info)

    def AddCmd2(self,type,cmd):
        info = CmdInfo()
        info.type = type
        info.cmd = cmd
        info.value = ""
        info.delay = 1
        info.isWaiting = False

        return self.AddCmdInfo(info)

    def AddCmdInfo(self,info): 
        self.listCmd.append(info) 
        item = None
        if self.IsElementExist(info.cmd):
            item = self.driver.find_element_by_xpath(info.cmd)
            
        info.item = item

        return item

    def AddCmdById(self,type,cmd,value="",delay=1):
        info = CmdInfo()
        info.type = type
        info.cmd = cmd
        info.value = value
        info.delay = delay
        info.isWaiting = False
        return self.AddCmdInfoById(info)

    def AddCmdInfoById(self,info): 
        self.listCmd.append(info) 
        item = None
        if self.IsElementExistById(info.cmd):
            item = self.driver.find_element_by_id(info.cmd)
            
        info.item = item

        return item

    def Clear(self):
        self.listCmd.clear()

    # 让元素在可见范围 可以点击操作
    def SetItemVisible(self,item,delay=1):
        # ActionChains(self.driver).move_to_element(item).perform()
        time.sleep(delay)

    def IsElementExistById(self,element):
        flag=True
        browser=self.driver
        try:
            # browser.find_element_by_css_selector(element)
            browser.find_element_by_id(element)
            return flag 
        except:
            flag=False
            return flag

    def IsElementExist(self,element):
        flag=True
        browser=self.driver
        try:
            # browser.find_element_by_css_selector(element)
            browser.find_element_by_xpath(element)
            return flag 
        except:
            flag=False
            return flag
    def IsElementIDExist(self,element):
        flag=True
        browser=self.driver
        try:
            # browser.find_element_by_css_selector(element)
            browser.find_element_by_xpath(element)
            return flag 
        except:
            flag=False
            return flag

    def IsElementChildExist(self,parent,key):
        flag=True 
        try:
            # browser.find_element_by_css_selector(element)
            parent.find_element_by_xpath(key)
            return flag 
        except:
            flag=False
            return flag

    def Find(self,key,isWait,timeOutCount = -1):
        item = None
        count =0
        if isWait:
            if self.IsElementExist(key):
                item = self.driver.find_element_by_xpath(key)
            else:
                # waiting
                while True:
                    time.sleep(1) 
                    count=count+1
                    print("waiting key=", key," count=",str(count))
                    if timeOutCount>0 and count>timeOutCount:
                        break

                    if self.IsElementExist(key): 
                        item = self.driver.find_element_by_xpath(key)
                        break

        else: 
            item = self.driver.find_elemefind_element_by_xpath(key)  
        
        return item
        
    def FindByID(self,key,isWait=False,timeOutCount = -1):
        item = None
        count =0
        if isWait:
            if self.IsElementIDExist(key):
                item = self.driver.find_element_by_xpath(key)
            else:
                # waiting
                while True:
                    time.sleep(1) 
                    count=count+1
                    if count>3:
                        print("waiting key=", key," count=",str(count))
                    if timeOutCount>0 and count>timeOutCount:
                        break
                    if self.IsElementIDExist(key): 
                        item = self.driver.find_element_by_xpath(key)
                        break

        else: 
            item = self.driver.find_element_by_xpath(key)  
        
        return item

    def FindListByID(self,key,isWait=False,timeOutCount = -1):  
        item = None
        count =0
        if isWait:
            if self.IsElementIDExist(key):
                item = self.driver.find_element_by_xpath(key)
            else:
                # waiting
                while True:
                    time.sleep(1) 
                    count=count+1
                    if count>3:
                        print("waiting key=", key," count=",str(count))
                    if timeOutCount>0 and count>timeOutCount:
                        break
                    if self.IsElementIDExist(key): 
                        item = self.driver.find_element_by_xpath(key)
                        break

        else: 
            item = self.driver.find_element_by_xpath(key)  
        
        return item

    def FindChild(self,item,key):
        return item.find_element_by_xpath(key)

    def FindList(self,key,isWait=False):
        item = None
        if isWait:
            if self.IsElementExist(key):
                item = self.driver.find_element_by_xpath(key)
            else:
                # waiting
                while True:
                    time.sleep(1) 
                    print("waiting key=", key)
                    if self.IsElementExist(key): 
                        item = self.driver.find_element_by_xpath(key)
                        break

        else: 
            item = self.driver.find_element_by_xpath(key)  
        
        return item

    def FindListChild(self,item,key):
        return item.find_element_by_xpath(key)



    def swipe(self,x1, y1, x2, y2,t):  
        self.driver.swipe(x1, y1, x2, y2,t)
       
            


    #获得机器屏幕大小x,y
    def GetSize(self):
        x = self.driver.get_window_size()['width']
        y = self.driver.get_window_size()['height']
        # print("GetSize x=",x," y=",y)
        return (x, y)
    # swipe 需要关闭杀毒软件
    #屏幕向上滑动
    def SwipeUp(self,t=1000):
        l = self.GetSize()
        x1 = int(l[0] * 0.5)    #x坐标
        y1 = int(l[1] * 0.75)   #起始y坐标
        y2 = int(l[1] * 0.25)   #终点y坐标
        self.swipe(x1, y1, x1, y2,t)

    #屏幕向下滑动
    def SwipeDown(self,t=1000):
        l = self.GetSize()
        x1 = int(l[0] * 0.5)  #x坐标
        y1 = int(l[1] * 0.25)   #起始y坐标
        y2 = int(l[1] * 0.75)   #终点y坐标
        
        self.swipe(x1, y1, x1, y2,t)

    #屏幕向左滑动
    def SwipLeft(self,t=1000):
        l=self.GetSize()
        x1=int(l[0]*0.75)
        y1=int(l[1]*0.5)
        x2=int(l[0]*0.05)
        self.swipe(x1,y1,x2,y1,t)
    #屏幕向右滑动
    def SwipRight(self,t=1000):
        l=self.GetSize()
        x1=int(l[0]*0.05)
        y1=int(l[1]*0.5)
        x2=int(l[0]*0.75)
        self.swipe(x1,y1,x2,y1,t)


# 组合查找 https://blog.csdn.net/qq_32189701/article/details/100176577
# find_element_by_xpath("//input[@class=‘s_ipt’ and @name=‘wd’]")
    def Run(self,isClear):
        for info in self.listCmd:
            if info.isWaiting:
                if self.IsElementExist(info.cmd):
                    item = self.driver.find_element_by_xpath(info.cmd)
                else:
                    # waiting
                    while True:
                        time.sleep(1) 
                        print("waiting info.cmd=", info.cmd)
                        if self.IsElementExist(info.cmd): 
                            item = self.driver.find_element_by_xpath(info.cmd)
                            break

            else:
                item = info.item
                if item == None:
                    item = self.driver.find_element_by_xpath(info.cmd) 
                    info.item = item

            
            if info.type == CmdType.CLICK:
                # self.driver.execute_script("arguments[0].click();", item)
                item.click()
            if info.type == CmdType.CLICK_SCRIPT:
                # 有些item.click() 会报InvalidArgumentException: Message: invalid argument
                self.driver.execute_script("arguments[0].click();", item) 
                
            # if info.type == CmdType.CLICK_Action:
            #     # 有些item.click() 无响应 用这个鼠标模拟点击 
            #     action= ActionChains(self.driver)
            #     action.click(item).perform()

            if info.type == CmdType.INPUT:
                item.clear()
                item.send_keys(info.value)
                # item.clear()
                # item.send_keys(info.value)
                # item.text = info.value

            if info.type == CmdType.INPUT_CLEAR:
                item.clear()

            if info.type == CmdType.ENTER:
                item.send_keys(Keys.ENTER)
            if info.type == CmdType.CTR_V:
                item.send_keys(Keys.CONTROL,"v")
                 
            if info.type == CmdType.CLICK_List_ALL:
                list =  self.driver.find_elements(By.XPATH, info.cmd)
                for item in list:
                    item.click()

            if info.type == CmdType.CLICK_List_Item:
                list =  self.driver.find_elements(By.XPATH, info.cmd)
                item = list[info.index]
                if info.type2 == CmdType.CLICK:
                    item.click()
                if info.type2 == CmdType.CLICK_SCRIPT:
                    self.driver.execute_script("arguments[0].click();", item)
                if info.type2 == CmdType.CLICK_Action:
                    action= ActionChains(self.driver)
                    action.click(item).perform()

            time.sleep(info.delay)

        if isClear:
            self.Clear()

  

 
 