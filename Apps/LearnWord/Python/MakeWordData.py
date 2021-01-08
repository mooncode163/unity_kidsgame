# 导入selenium的浏览器驱动接口

import time
from selenium.webdriver.common.by import By
from selenium.webdriver.chrome.options import Options
from selenium.webdriver.common.keys import Keys
from selenium import webdriver
from selenium.webdriver import ActionChains
# pip3 install Pillow
from PIL import Image

import sys
import shutil
import os
import json
import requests
import math
# https://hanziwriter.org/docs.html
# 要想调用键盘按键操作需要引入keys包

# 导入chrome选项


# pip3 install pywin32

# sys.path.append('../common')
TYPE_WORD = "type_word"


class WordInfo(object):
    title: None
    detail: None
    index: None
    page: None


class MakeWordData:
    driver: None
    dirRoot: None
    urlCreatePlaceId: None
    osApp: None
    fileJosn = "word.json"
    listWord: None

    def SaveString2File(self, str, file):
        f = open(file, 'wb')  # 若是'wb'就表示写二进制文件
        b = str
        if type(str).__name__ == "str":
            b = str.encode('utf-8', "ignore")
        f.write(b)
        f.close()

    def GetFileString(self, filePath):
        f = open(filePath, 'rb')
        strFile = f.read().decode('utf-8', "ignore")
        f.close()
        return strFile

    def SetCmdPath(self, str):
        dir = FileUtil.GetLastDirofDir(str)
        dir = FileUtil.GetLastDirofDir(dir)
        dir = FileUtil.GetLastDirofDir(dir)
        self.dirRoot = dir
        print("dir = ", dir)

    def cur_file_dir(self):
        # 获取脚本路径
        path = sys.path[0]
        # 判断为脚本文件还是py2exe编译后的文件，如果是脚本文件，则返回的是脚本的目录，如果是py2exe编译后的文件，则返回的是编译后的文件路径
        if os.path.isdir(path):
            return path
        elif os.path.isfile(path):
            return os.path.dirname(path)

    # 让元素在可见范围 可以点击操作
    def SetItemVisible(self, item, delay=1):
        ActionChains(self.driver).move_to_element(item).perform()
        time.sleep(delay)

    def GetRootDirWord(self, word):
        dir = self.cur_file_dir()+"/image/"+word
        dir = "../../../../../../ResourceData/LearnWord/LearnWord/GameRes/image/"+word
        if os.path.exists(dir) == False:
            os.mkdir(dir)
        return dir

    def GetRootDirWordBihuaShow(self, word):
        dir = self.cur_file_dir()+"/image_bihua_show/"+word
        dir = "../../../../../../ResourceData/LearnWord/LearnWord/GameRes/image_bihua_show/"+word
        if os.path.exists(dir) == False:
            os.mkdir(dir)
        return dir

    def GetRootDirWordJson(self, word):
        dir = self.cur_file_dir()+"/image/"+word
        dir = "../../../../../../ResourceData/LearnWord/LearnWord/GameRes/guanka/word"
        # dir = 'word'
        if os.path.exists(dir) == False:
            os.mkdir(dir)
        return dir


    def GetRootDirTmp(self):
        dir = self.cur_file_dir()+"/Tmp"
        return dir

    def GetCountStroke(self, word):
        # type =""
        # self.driver.get("file:///F:/sourcecode/unity/product/kidsgame/kidsgameUnityNow/Assets/Script/Apps/LearnWord/web_wordgame/index.html?type="+type+"&word="+word+"&index=-1")
        # time.sleep(2)
        # count = self.driver.execute_script('return GetCountStroke()')
        dataRoot = self.LoadJsonStroke(word)
        count = len(dataRoot["strokes"])
        pt = self.GetStrokePoint(dataRoot, 0, 0)
        x = pt[0]
        return count

    def GetCountPoint(self, word, index):
        # type ="animate"
        # self.driver.get("file:///F:/sourcecode/unity/product/kidsgame/kidsgameUnityNow/Assets/Script/Apps/LearnWord/web_wordgame/index.html?type="+type+"&word="+word+"&end_index_point=-1")
        # time.sleep(2)
        # count = self.driver.execute_script('return GetCountPoint()')
        dataRoot = self.LoadJsonStroke(word)
        count = len(dataRoot["medians"][index])

        return count

    # 制作笔画图片
    def SaveWordStroke(self):
        idx = 0
        for info in self.listWord:
            word = info.title
            count_stroke = self.GetCountStroke(word)

            count = count_stroke
            dir = self.GetRootDirWord(word)
            if os.path.exists(dir) == False:
                os.mkdir(dir)

            path_last = dir+"/"+word+"_"+str(count-1)+".png"
            idx += 1
            if os.path.exists(path_last) == True:
                continue
            print("word = ", word, " idx=", idx)

            for i in range(count+1):
                type = ""

                # file:///F:/sourcecode/unity/product/kidsgame/kidsgameUnityNow/Assets/Script/Apps/LearnWord/web_wordgame/index.html?type=&word=他&index=0"
                self.driver.get(
                    "file:///F:/sourcecode/unity/product/kidsgame/kidsgameUnityNow/Assets/Script/Apps/LearnWord/web_wordgame/index.html?type="+type+"&word="+word+"&index="+str(i-1))
                time.sleep(2)
                # self.driver.save_screenshot('capture.png')    #全屏截图
                html = self.driver.page_source
                # print(html)
                key = "//div/*[name()='svg']"
                item = self.driver.find_element(By.XPATH, key)
                # self.ConverImag("./MakeWordData.png")
                if i == 0:
                    path = dir+"/"+word+".png"
                else:
                    path = dir+"/"+word+"_"+str(i-1)+".png"

                pathnew = path

                if os.path.exists(path) == False:
                    self.SetItemVisible(item)
                    item.screenshot(path)  # 元素截图
                    time.sleep(1)
                    self.ConverImag(path, pathnew)

    # 制作笔画动画图片
    def SaveWordAnimate(self):
        idx = 0
        for info in self.listWord:
            word = info.title
            count_stroke = self.GetCountStroke(word)

            dir = self.GetRootDirWord(word)
            if os.path.exists(dir) == False:
                os.mkdir(dir)

            dirAnimate = dir+"/Animate"
            if os.path.exists(dirAnimate) == False:
                os.mkdir(dirAnimate)

            idx += 1
            count_point = self.GetCountPoint(word, count_stroke-1)
            path_last = dirAnimate+"/" + str(count_stroke-1)+"/"+str(count_point-1)+".png"
            # if os.path.exists(path_last)==True:
            #     continue

            print("word = ", word, " idx=", idx)

            for i in range(count_stroke):
                count_point = self.GetCountPoint(word, i)
                # print("count_point = ",str(count_point))
                type = "animate"

                for j in range(count_point):
                    end_index_point = j+2
                    if end_index_point>=count_point:
                        continue
                    
                # file:///F:/sourcecode/unity/product/kidsgame/kidsgameUnityNow/Assets/Script/Apps/LearnWord/web_wordgame/index.html?type=&word=他&index=0"
                    self.driver.get("file:///F:/sourcecode/unity/product/kidsgame/kidsgameUnityNow/Assets/Script/Apps/LearnWord/web_wordgame/index.html?type=" +
                                    type+"&word="+word+"&index="+str(i)+"&end_index_point="+str(end_index_point))
                    time.sleep(2)
                # self.driver.save_screenshot('capture.png')    #全屏截图
                    html = self.driver.page_source
                # print(html) 
                    key = "//div/*[name()='svg']"
                    item = None 
                        
                    while True:
                        try:
                            item = self.driver.find_element(By.XPATH, key)
                        except Exception as e:
                            print(e)  # 打印所有异常到屏幕

                        if item == None:
                            time.sleep(1)
                        else:
                            break

                  
                    if item == None:
                        continue

                    dir = dirAnimate+"/"+str(i)
                    if os.path.exists(dir) == False:
                        os.mkdir(dir)

                    path = dir+"/"+str(j)+".png"

                    pathnew = path

                    if os.path.exists(path) == False:
                        self.SetItemVisible(item)
                        item.screenshot(path)  # 元素截图
                        time.sleep(1)
                        self.ConverImag(path, pathnew)

    def GetStrokePoint(self, jsonData, indexStroke, index):
        list = jsonData["medians"][indexStroke][index]
        x = 0
        y = 0
        for k in range(len(list)):
            v = list[k]
            if k == 0:
                x = v

            if k == 1:
                y = v

        # print(x)
        # print(y)
        return (x, y)

    # y=kx+b
    def GetLineK(self, x1, y1, x2, y2):
        return (y2-y1)/(x2-x1)
    # y=kx+b

    def GetLineB(self, x1, y1, x2, y2):
        k = self.GetLineK(x1, y1, x2, y2)
        return y1-k*x1

    def GetLineStepX(self, x1, x2, r):
        step = (x2-x1)/8
        if x2 > x1:
            step = r
        else:
            step = -r
        return step

    def GetLineStepXCount(self, x1, x2, r):
        step = self.GetLineStepX(x1, x2, r)
        count = round(abs(x2-x1)/step)
        return int(count)

    def GetLineX(self, x1, x2, r, index):
        step = self.GetLineStepX(x1, x2, r)
        return x1+step*index

    def RenderPoint(self, x, y, word, index):
        print("RenderPoint point  x= ", str(x), " y=", str(y))
        type = "circle"
        self.driver.get("file:///F:/sourcecode/unity/product/kidsgame/kidsgameUnityNow/Assets/Script/Apps/LearnWord/web_wordgame/index.html?type=" +
                        type+"&word="+word+"&index="+str(index)+"&x="+str(x)+"&y="+str(y))
        time.sleep(2)

        key = "//div/*[name()='svg']"
        item = None
        try:
            item = self.driver.find_element(By.XPATH, key)

        except Exception as e:
            print(e)  # 打印所有异常到屏幕

        if item == None:
            return False

        dir = self.GetRootDirTmp()
        if os.path.exists(dir) == False:
            os.mkdir(dir)

        path = dir+"/tmp.png"

        pathnew = path

        self.SetItemVisible(item)
        item.screenshot(path)  # 元素截图
        time.sleep(1)
        self.ConverImag(path, pathnew)
        return path

    def SaveSVG2Image(self, filepath):
        key = "//div/*[name()='svg']"
        item = None
        try:
            item = self.driver.find_element(By.XPATH, key)
        except Exception as e:
            print(e)  # 打印所有异常到屏幕
        if item == None:
            return

        if os.path.exists(filepath) == False:
            self.SetItemVisible(item)
            item.screenshot(filepath)  # 元素截图
            time.sleep(1)
            self.ConverImag(filepath, filepath)

    def SVGPoint2ImagePoint(self, word, indexStroke, index, ptSvg):
        x = ptSvg[0]
        y = ptSvg[1]
        path = self.RenderPoint(x, y, word, indexStroke)
        pt = self.GetPointInImage(path)
        return pt

# file:///F:/sourcecode/unity/product/kidsgame/kidsgameUnityNow/Assets/Script/Apps/LearnWord/web_wordgame/index2.html?type=circle&word=他&index=0&x=300&y=793
# 制作
    def MakeNewMedian(self):
        idx = 0
        for info in self.listWord:
            word = info.title
            # word = "他"
            dir = self.GetDirWordMedian()
            filepath = dir+"/"+word+".json" 
           
            idx += 1
            if os.path.exists(filepath) == True:
                continue
            print("word = ", word, " idx=", idx)

            count_stroke = self.GetCountStroke(word)
            dataRoot = self.LoadJsonStroke(word)
            # print("count_stroke = ",str(count_stroke))
            jsonRootMedian = dataRoot["medians"]
            for i in range(count_stroke):
                jsonStrokePoints = jsonRootMedian[i]
                count_point = self.GetCountPoint(word, i)
                # print("count_point = ",str(count_point))
                type = "circle"
                newStart = None
                newEnd = None
                r = 8
                # 开始
                pt1 = self.GetStrokePoint(dataRoot, i, 0)
                pt2 = self.GetStrokePoint(dataRoot, i, 1)
                # print("start point1  x= ",pt1[0]," y=",pt1[1])
                # print("start point2  x= ",pt2[0]," y=",pt2[1])

                count = self.GetLineStepXCount(pt1[0], pt2[0], r)
                x1 = pt1[0]
                x2 = pt2[0]
                y1 = pt1[1]
                y2 = pt2[1]

                if x1 != x2:
                    k = self.GetLineK(x1, y1, x2, y2)
                    b = self.GetLineB(x1, y1, x2, y2)

                for j in range(count):
                    if x1 != x2:
                        x = self.GetLineX(pt1[0], pt2[0], r, j)
                        y = k*x+b
                    else:
                        x = x1
                        y = y1+(y2-y1)*j/count

                    path = self.RenderPoint(x, y, word, i)
                    isin = self.IsPointInImage(path)
                    if isin:
                        # print("find new start point in image x= ",x," y=",y)
                        newStart = (x, y)
                        break

                # 结束
                pt1 = self.GetStrokePoint(dataRoot, i, count_point-1)
                pt2 = self.GetStrokePoint(dataRoot, i, count_point-2)

                # print("end point1  x= ",pt1[0]," y=",pt1[1])
                # print("end point2  x= ",pt2[0]," y=",pt2[1])

                count = self.GetLineStepXCount(pt1[0], pt2[0], r)
                x1 = pt1[0]
                x2 = pt2[0]
                y1 = pt1[1]
                y2 = pt2[1]

                if x1 != x2:
                    k = self.GetLineK(x1, y1, x2, y2)
                    b = self.GetLineB(x1, y1, x2, y2)

                for j in range(count):
                    if x1 != x2:
                        x = self.GetLineX(pt1[0], pt2[0], r, j)
                        y = k*x+b
                    else:
                        x = x1
                        y = y1+(y2-y1)*j/count

                    path = self.RenderPoint(x, y, word, i)
                    isin = self.IsPointInImage(path)
                    if isin:
                        # print("find new end point in image x= ",x," y=",y)
                        newEnd = (x, y)
                        break

                if newStart != None and newEnd != None:
                    # save
                    for k in range(count_point):
                        jsonXY = jsonStrokePoints[k]
                        pt = self.GetStrokePoint(dataRoot, i, k)
                        if k == 0:
                            pt = newStart
                        if k == count_point-1:
                            pt = newEnd

                        jsonXY[0] = pt[0]
                        jsonXY[1] = pt[1]

            self.SaveNewMedian(filepath, jsonRootMedian)

    def SaveNewMedian(self, filepath, jsonData):
        dataRoot = json.loads("{}")
        dataRoot["medians"] = jsonData
        json_str = json.dumps(dataRoot, ensure_ascii=False,
                              indent=4, sort_keys=True)
        self.SaveString2File(json_str, filepath)

    # 制作笔画示意图
    def SaveWordLineShow(self):
        idx = 0
        for info in self.listWord:
            word = info.title
            count_stroke = self.GetCountStroke(word)
            count = count_stroke
            dir = self.GetRootDirWordBihuaShow(word) 
            filepath = dir+"/"+word+"_LineShow.png"
            idx += 1
            if os.path.exists(filepath) == True:
                continue
            print("word = ", word, " idx=", idx)

            type = "stroke_lineshow"
            # file:///F:/sourcecode/unity/product/kidsgame/kidsgameUnityNow/Assets/Script/Apps/LearnWord/web_wordgame/index.html?type=stroke_lineshow&word=他"
            self.driver.get(
                "file:///F:/sourcecode/unity/product/kidsgame/kidsgameUnityNow/Assets/Script/Apps/LearnWord/web_wordgame/index.html?type="+type+"&word="+word)
            time.sleep(2)
            self.SaveSVG2Image(filepath)

    # 制作笔画描绘的点坐标

    def SaveWordStrokePoint(self):
        for info in self.listWord:
            word = info.title
            count_stroke = self.GetCountStroke(word)
            print("count_stroke = ", str(count_stroke))
            count = count_stroke

    def DownloadJsonStroke(self, url):
        r = requests.get(url)
        return r.content

    def LoadJsonStroke(self, word):
        dir = self.GetRootDirWordJson(word)
        if os.path.exists(dir) == False:
            os.mkdir(dir)
        self.fileJosn = dir+"/"+word+".json"
        if os.path.exists(self.fileJosn) == False:
            # https://cdn.jsdelivr.net/npm/hanzi-writer-data@2.0/%E4%BB%96.json
            url = "https://cdn.jsdelivr.net/npm/hanzi-writer-data@2.0/"+word+".json"
            strjson = self.DownloadJsonStroke(url)
            # print(strjson)
            self.SaveString2File(strjson, self.fileJosn)

        else:
            strjson = self.GetFileString(self.fileJosn)

        dataRoot = json.loads(strjson)
        # strokes
        return dataRoot

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

#  判断image是否存在圆点
    def IsPointInImage(self, filepath):
        img = Image.open(filepath)
        img = img.convert("RGBA")  # 转换获取信息
        pixdata = img.load()
        for y in range(img.size[1]):
            for x in range(img.size[0]):
                # 红色的点
                if pixdata[x, y][0] == 255 or pixdata[x, y][1] == 255 or pixdata[x, y][2] == 255:
                    return True

        return False

    def GetPointInImage(self, filepath):
        img = Image.open(filepath)
        img = img.convert("RGBA")  # 转换获取信息
        pixdata = img.load()
        for y in range(img.size[1]):
            for x in range(img.size[0]):
                # 红色的点
                if pixdata[x, y][0] == 255 or pixdata[x, y][1] == 255 or pixdata[x, y][2] == 255:
                    return (x, y)

        return (0, 0)

    def ConverImag(self, filepath, filepathnew):
        img = Image.open(filepath)
        img = img.convert("RGBA")  # 转换获取信息
        pixdata = img.load()

        for y in range(img.size[1]):
            for x in range(img.size[0]):
                if pixdata[x, y][0] == 255 and pixdata[x, y][1] == 255 and pixdata[x, y][2] == 255:
                    pixdata[x, y] = (0, 0, 0, 0)

        img.save(filepathnew)

    def Init(self):
        # 创建chrome浏览器驱动，无头模式（超爽）
        chrome_options = Options()
        chrome_options.add_argument('--headless')
        chrome_options.add_argument("--no-sandbox")

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
        self.LoadJson()

    def GetDirWordMedian(self):
        dir = "../../../../../../ResourceData/LearnWord/LearnWord/GameRes/guanka/word_median"
        # dir = "word_median"
        if os.path.exists(dir) == False:
            os.mkdir(dir)
        return dir

# 下载json
    def SaveWordJson(self):
        idx = 0
        for info in self.listWord:
            word = info.title
            # dataRoot = self.LoadJsonStroke(word)
            dir = "F:\\sourcecode\\unity\\product\\kidsgame\\ResourceData\\pintu\\Word\\GameRes\\image\\"+word
            src = dir+"\\"+word+"_median.json"
            
            dir = self.GetDirWordMedian() 
            dst = dir+"\\"+word+".json"
            if os.path.exists(src):
                shutil.copyfile(src,dst)
            # else:
            #     # print(src)
            
         


# 主函数的实现
if __name__ == "__main__":
    # 入口参数：http://blog.csdn.net/intel80586/article/details/8545572
    p = MakeWordData()
    p.Init()
    # p.SaveWordJson()
    # p.SaveWordStroke()
    # p.SaveWordAnimate()
    # p.SaveWordLineShow()
    # p.SaveWordStrokePoint()
    p.MakeNewMedian()
