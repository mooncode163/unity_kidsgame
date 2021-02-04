<?php
header("Content-type: text/html; charset=utf-8");
include_once('CrazyImageDB.php');
include_once('CrazyImageItemInfo.php');
include_once('../Common/Download.php');
include_once('../Common/Common.php');
include_once('../Common/Html/simple_html_dom.php');

//疯狂猜图答案_疯狂猜图所有答案_疯狂猜图品牌__72G疯狂猜图专区
//http://fkct.72g.com/

function get_html($url)
{
    $html = new simple_html_dom();

    // // 从url中加载  
    // $html->load_file('http://www.jb51.net');  

    // // 从字符串中加载  
    // $html->load('<html><body>从字符串中加载html文档演示</body></html>');  

    //从文件中加载  
    $html->load_file($url);

    return $html;
}

// 疯狂猜图  http://www.caichengyu.com/fkct/
class ParseCrazyImage //extends Thread
{

    public $url;
    public $id;
    public $channel;
    public $listItem = array();
    public $listSort = array();
    public  $page_total = 10; //10 
    public $ROOT_SAVE_DIR = "Data";
    public $WEB_HOME = "http://www.caichengyu.com";
    public $IMAGE_DIR = "CrazyImage";
    public $PIC_DIR = "Pic";
    public function DoPaser()
    {

        $save_dir = $this->ROOT_SAVE_DIR;
        if (!is_dir($save_dir)) {
            mkdir($save_dir);
        }

        $save_dir = $this->ROOT_SAVE_DIR . "/" . $this->IMAGE_DIR;
        if (!is_dir($save_dir)) {
            mkdir($save_dir);
        }

        $dir = $this->ROOT_SAVE_DIR . "/" . $this->IMAGE_DIR . "/" . $this->PIC_DIR;
        if (!is_dir($dir)) {
            mkdir($dir);
        }

        $this->PaserSortList();

        $this->SaveSortJson($save_dir);
    }


    public function SaveSortJson($save_dir)
    {
        //save sort
        $savefilepath = $save_dir . "/crazy_image_sort.json";
        $ret = file_exists($savefilepath);
        if ($ret) {
            // return;
        }

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


    public function SaveImageListJson($save_dir, $id)
    {
        //save sort
        $savefilepath = $save_dir . "/image_list_" . $id . ".json";
        $ret = file_exists($savefilepath);
        if ($ret) {
            // return;
        }

        // $element = array(
        //     'title' => "e",
        // );
        // array_push($this->listItem, $element);

        $count = count($this->listItem);
        echo "SaveImageListJson count =" . $count . "\n";
        if ($count) {

            $element = array(
                'items' => ($this->listItem),
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
                echo "写入文件失败:" . $savefilepath . "\n" . " jsn=" . $jsn . "\n";
            }
            fclose($fp);
        }
    }

    //http://fkct.72g.com/#c13  id = c13
    function GetChannelIdFromUrl($url)
    {
        $pos = strpos($url, "#");
        $str = substr($url, $pos + 1);
        return $str;
    }

    //http://www.caichengyu.com/fkct/dianyingdianshi/list_75_1.html
    function PaserImageList($url, $title)
    {
        $id_channel = $this->GetChannelIdFromUrl($url);
        echo "id_channel=" . $id_channel . "\n";
        $html = get_html($url);
        if (!$html) {
            echo "PaserSortList html fail\n";
            return;
        }
        $div_main = null;
        //zttj fl
        $array_div = $html->find('div[class=zttj fl]');
        foreach ($array_div as $div) {
            $arry_a = $div->find('a');

            foreach ($arry_a as $a) {
                $id = $a->id;
                if ($id) {

                    if ($id == $id_channel) {
                        $div_main = $div;
                        echo "PaserImageList find div_main id=" . $id . " url=" . $url . "\n";
                        break;
                    }
                }
            }

            if ($div_main) {
                break;
            }
        }

        // $div_main = $html->find('div[class=conn]', 0);
        if (!$div_main) {
            echo "PaserImageList find div_main fail\n";
            return;
        }


        $idx = 0;
        $arry_img = $div_main->find('ul/li/a/img');
        foreach ($arry_img as $img) {
            $url_img = $img->xsrc;
            $answer =  $img->alt;
            if ($answer) {
                $save_dir = $this->ROOT_SAVE_DIR . "/" . $this->IMAGE_DIR . "/" . $this->PIC_DIR . "/" . $title;
                if (!is_dir($save_dir)) {
                    mkdir($save_dir);
                }
                $savepic =  $save_dir . "/" . $answer . ".png";
                if (!file_exists($savepic)) {
                    //if ($id_channel == "c6") 
                    {
                        echo "img=" . $url_img . " answer=" . $answer . "\n";
                    }
                    Download::Main()->DownloadFile($url_img, $savepic);
                } else {
                    // echo "file_exists img=" . $url_img . " answer=" . $answer . "\n";
                }

                if ($answer == "西铁城") {
                    /// continue;
                }
                $element = array(
                    'title' => $answer,
                );
                //
                array_push($this->listItem, $element);
                $count = count($this->listItem);
                echo "array_push answer=" . $answer  . " count=" . $count . "  idx=" .  $idx . "\n";
                $idx++;
                // if ($count < 101)
                //  {
                //     array_push($this->listItem, $element);
                // } else { }
            } else {

                echo "no anser img=" . $url_img . " answer=" . $answer . "\n";
            }
        }
    }

    function GetPageUrl($url_sort, $page)
    {
        //http://www.caichengyu.com/fkct/dianyingdianshi/list_75_2.html
        $ret = $url_sort . "list_75_" . $page . ".html";
        return $ret;
    }

    //http://www.caichengyu.com/ktccy/
    function PaserSortList()
    {
        $url = "http://fkct.72g.com/";
        echo "PaserSortList  " .  "\n";
        $html = get_html($url);
        if (!$html) {
            echo "PaserSortList html fail\n";
            return;
        }
        $div_main = $html->find('div[class=lanmu]', 0);
        if (!$div_main) {
            echo "PaserSortList find div_main fail\n";
            return;
        }
        $arry_a = $div_main->find('a');
        foreach ($arry_a as $a) {
            $url_a =  $a->href;
            $title =  $a->plaintext;
            $id_channel = $this->GetChannelIdFromUrl($url_a);

            // if (($id_channel == "c5") || ($id_channel == "c6") || ($id_channel == "c7")) {
            //     continue;
            // }

            if ($id_channel != "c5") {
                continue;
            }

            if ($title == "回到顶部") {
                continue;
            }
            $element = array(
                'url' => $url_a, //谜面
                'title' => $title,
            );

            $this->listItem = array();
            echo "PaserSortList url_a:  " . $url_a . " title=" . $title . "\n";
            $this->PaserImageList($url_a, $title);
            //save
            $save_dir = $this->ROOT_SAVE_DIR . "/" . $this->IMAGE_DIR;

            $this->SaveImageListJson($save_dir, $id_channel);

            array_push($this->listSort, $element);
        }
    }
}


//https://www.runoob.com/sqlite/sqlite-php.html
/*
$db = new CrazyImageDB();
$db->CreateDb(); {
    $info = new CrazyImageItemInfo();
    $info->id = "a";
    $info->title = "性";
    $db->AddItem($info);
} {
    $info = new CrazyImageItemInfo();
    $info->id = "b";
    $info->title = "中国";
    $db->AddItem($info);
}

$db->GetAllItem();
*/
//建表
// $table_name = "COMPANY";
// $sql = <<<EOF
// CREATE TABLE COMPANY
// (ID INT PRIMARY KEY     NOT NULL,
// NAME           TEXT    NOT NULL,
// AGE            INT     NOT NULL,
// ADDRESS        CHAR(50),
// SALARY         REAL);
// EOF;
// if (!$db->IsExitTable($table_name)) {
//     $db->CreateTable($sql);
// }




// $sql = <<<EOF
// INSERT INTO COMPANY (ID,NAME,AGE,ADDRESS,SALARY)
// VALUES (1, 'Paul', 32, 'California', 20000.00 );

// INSERT INTO COMPANY (ID,NAME,AGE,ADDRESS,SALARY)
// VALUES (2, 'Allen', 25, 'Texas', 15000.00 );

// INSERT INTO COMPANY (ID,NAME,AGE,ADDRESS,SALARY)
// VALUES (3, 'Teddy', 23, 'Norway', 20000.00 );

// INSERT INTO COMPANY (ID,NAME,AGE,ADDRESS,SALARY)
// VALUES (4, 'Mark', 25, 'Rich-Mond ', 65000.00 );
// EOF;

// //$db->Execute($sql);

// $info = new CrazyImageItemInfo();
// $db->Read($info);

//$db->CloseDB();

$parser = new ParseCrazyImage();
$parser->DoPaser();

echo 'done<br>';
