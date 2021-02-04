<?php
header("Content-type: text/html; charset=utf-8");

include('../../Common/Common.php');
include_once('ParseIdiomList.php');


//乐乐课堂 http://www.leleketang.com/chengyu/
class ParseIdiomSort //extends Thread
{
    public $WEB_HOME = "http://www.leleketang.com/chengyu/";
    public $ROOT_SAVE_DIR = "Data";
    public $listCategory = array();
    public $listSort = array();
    function SaveCategoryJsonList()
    {
        //save 
        $save_dir = $this->ROOT_SAVE_DIR;
        if (!is_dir($save_dir)) {
            mkdir($save_dir);
        }
        $savefilepath =  $save_dir . "/category.json";

        $count = count($this->listCategory);
        echo "count =" . $count . "\n";
        if ($count) {

            $element = array(
                'items' => ($this->listCategory),
            );
            //JSON_UNESCAPED_SLASHES json去除反斜杠 JSON_UNESCAPED_UNICODE中文不用\u格式
            $jsn = urldecode(json_encode($element, JSON_UNESCAPED_SLASHES | JSON_UNESCAPED_UNICODE));

            // "[  ]"
            //$jsn = str_replace("\"[", "[", $jsn);
            //$jsn = str_replace("]\"", "]", $jsn);

            $fp = fopen($savefilepath, "w");
            if (!$fp) {
                echo "打开文件失败<br>";
                return;
            }
            $flag = fwrite($fp, $jsn);
            if (!$flag) {
                echo "写入文件失败<br>";
            }
            fclose($fp);
        }
    }

    function GetSortJsonList($category)
    {
        //save 
        $save_dir = $this->ROOT_SAVE_DIR . "/" . $category;
        if (!is_dir($save_dir)) {
            mkdir($save_dir);
        }
        $savefilepath =  $save_dir . "/sort.json";
        return $savefilepath;
    }

    function SaveSortJsonList($category)
    {

        $savefilepath =  $this->GetSortJsonList($category);

        $count = count($this->listSort);
        echo "count =" . $count . "\n";
        if ($count) {

            $element = array(
                'items' => ($this->listSort),
            );
            //JSON_UNESCAPED_SLASHES json去除反斜杠 JSON_UNESCAPED_UNICODE中文不用\u格式
            $jsn = urldecode(json_encode($element, JSON_UNESCAPED_SLASHES | JSON_UNESCAPED_UNICODE));

            // "[  ]"
            //$jsn = str_replace("\"[", "[", $jsn);
            //$jsn = str_replace("]\"", "]", $jsn);

            $fp = fopen($savefilepath, "w");
            if (!$fp) {
                echo "打开文件失败<br>";
                return;
            }
            $flag = fwrite($fp, $jsn);
            if (!$flag) {
                echo "写入文件失败<br>";
            }
            fclose($fp);
        }
    }



    // 大类别
    function  ParseCategory()
    {
        echo "ParseCategory "   . "\n";
        // http://www.leleketang.com/chengyu/
        $html = get_html("http://www.leleketang.com/chengyu/");
        if (!$html) {
            echo "open html fail\n";
            return false;
        }

        // <div class="idiom_title clearfix" id="idiom_categories">
        $div_main = $html->find('div[class=idiom_title clearfix]', 0);
        $array_a = $div_main->find('a');
        // class="idiom_index"
        $idx = 0;
        foreach ($array_a as $a) {
            // echo "class =" . $a->class . "\n";

            if ($a->class == "idiom_index") {
                if ($idx > 1) {
                    $str  = $a->innertext;
                    //删除标签
                    $title = strip_tags($str);
                    echo "title =" . $title . "\n";

                    if ($title == "成语字数") {
                        //skip
                        continue;
                    }

                    $url = $this->WEB_HOME . $a->href;
                    echo "href =" . $url . "\n";
                    $this->ParseSort($url, $title);

                    $element = array(
                        'title' => urlencode($title), //   
                        'url' => urlencode($url),
                    );
                    array_push($this->listCategory, $element);
                }
                $idx++;
            }
        }
        $this->SaveCategoryJsonList();
    }

    public function  ParseSort($url, $category)
    {

        $filepath =  $this->GetSortJsonList($category);
        if (file_exists($filepath)) {
            return false;
        }
        echo "ParseSort "   . "\n";
        //tag_list
        $html = get_html($url);
        if (!$html) {
            echo "open html fail\n";
            return false;
        }
        $this->listSort = array();
        // <div class="idiom_title clearfix" id="idiom_categories">
        $div_main = $html->find('div[class=tag_list]', 0);
        if (!$div_main) {
            $div_main = $html->find('div[class=stag_filter_list clearfix]', 0);
        }


        $array_a = $div_main->find('a');
        $idx = 0;
        foreach ($array_a as $a) {
            if ($idx > 0) {
                $title  = $a->innertext;
                //删除标签
                $title = strip_tags($title);
                echo "title =" . $title . "\n";
                if ($title == "鸡的成语") {
                    //skip
                    continue;
                }
                if ($title == "狗的成语") {
                    //skip
                    continue;
                }
                if ($title == "猪的成语") {
                    //skip
                    continue;
                }

                if ($title == "生气的成语") {
                    //skip
                    continue;
                }
                if ($title == "泪的成语") {
                    //skip
                    continue;
                }
                if ($title == "为人正直的成语") {
                    //skip
                    continue;
                }
                if ($title == "明亮的成语") {
                    //skip
                    continue;
                }
                if ($title == "黑暗的成语") {
                    //skip
                    continue;
                }
                if ($title == "奇怪的成语") {
                    //skip
                    continue;
                }
                if ($title == "比喻的成语") {
                    //skip
                    continue;
                }
                if ($title == "读书的成语") {
                    //skip
                    continue;
                }
                if ($title == "战争的成语") {
                    //skip
                    continue;
                }
                if ($title == "影响巨大的成语") {
                    //skip
                    continue;
                }
                if ($title == "出自三国演义的成语故事") {
                    //skip
                    continue;
                }
                if ($title == "寓言故事相关的成语故事") {
                    //skip
                    continue;
                }
                if ($title == "历史故事相关的成语故事") {
                    //skip
                    continue;
                }

                if ($title == "神话故事相关的成语故事") {
                    //skip
                    continue;
                }
                if ($title == "出自水浒传的成语故事") {
                    //skip
                    continue;
                }

                $url = $this->WEB_HOME . $a->href;
                echo "href =" . $url . "\n";
                $p = new ParseIdiomList();
                $savefilepath =  $p->GetJsonList($title, $category);
                if (!file_exists($savefilepath)) {
                    $listItem = $p->ParseList($url);
                    $p->SaveJsonList($listItem, $title, $category);
                }

                $element = array(
                    'title' => urlencode($title), //   
                );
                array_push($this->listSort, $element);
            }
            $idx++;
        }

        $this->SaveSortJsonList($category);
        return true;
    }
}

$p = new ParseIdiomSort();
$p->ParseCategory();

// 
// $url = "http://www.leleketang.com/chengyu/cat12-1.shtml";
// $p->ParseSort($url, "十二生肖的成语");


echo 'done<br>';
