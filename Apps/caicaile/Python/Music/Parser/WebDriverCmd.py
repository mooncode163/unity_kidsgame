# 导入selenium的浏览器驱动接口 

import sys
import os
import json
o_path = os.getcwd()  # 返回当前工作目录
sys.path.append(o_path)  # 添加自己指定的搜索路径
 
import time
from selenium.webdriver.common.by import By
from selenium.webdriver.chrome.options import Options
from selenium.webdriver.common.keys import Keys
from selenium import webdriver 
from selenium.webdriver import ActionChains 

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




class WebDriverCmd():  
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
            item = self.driver.find_element(By.XPATH, info.cmd)
            
        info.item = item

        return item
    
    def Clear(self):
        self.listCmd.clear()

    def IsElementExist(self,element):
        flag=True
        browser=self.driver
        try:
            # browser.find_element_by_css_selector(element)
            browser.find_element(By.XPATH, element)
            return flag 
        except:
            flag=False
            return flag

# 组合查找 https://blog.csdn.net/qq_32189701/article/details/100176577
# find_element_by_xpath("//input[@class=‘s_ipt’ and @name=‘wd’]")
    def Run(self,isClear):
        for info in self.listCmd:
            if info.isWaiting:
                if self.IsElementExist(info.cmd):
                    item = self.driver.find_element(By.XPATH, info.cmd)
                else:
                    # waiting
                    while True:
                        time.sleep(1) 
                        print("waiting info.cmd=", info.cmd)
                        if self.IsElementExist(info.cmd): 
                            item = self.driver.find_element(By.XPATH, info.cmd)
                            break

            else:
                item = info.item
                if item == None:
                    item = self.driver.find_element(By.XPATH, info.cmd) 
                    info.item = item

            
            if info.type == CmdType.CLICK:
                # self.driver.execute_script("arguments[0].click();", item)
                item.click()
            if info.type == CmdType.CLICK_SCRIPT:
                # 有些item.click() 会报InvalidArgumentException: Message: invalid argument
                self.driver.execute_script("arguments[0].click();", item) 
                
            if info.type == CmdType.CLICK_Action:
                # 有些item.click() 无响应 用这个鼠标模拟点击 
                action= ActionChains(self.driver)
                action.click(item).perform()

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

  

 
 