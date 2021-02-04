<?php
// header("Content-type: text/html; charset=utf-8");


include_once('../../Common/Common.php');
include_once('../IdiomItemInfo.php');
include_once('ParseIdiomItem.php');

class ParseIdiomList
{
    public $ROOT_SAVE_DIR = "Data";
    public $WEB_HOME = "http://www.leleketang.com/chengyu/";
    function GetPageCount($url)
    {
        $html = get_html($url);
        $ret = 0;
        if (!$html) {
            echo "GetPageCount open html fail\n";
            return 0;
        }
        //<div class="gray mt f16">jù hǔ jìn láng</div> 
        $div_main = $html->find('div[class=list_anchor]', 0);

        //total
        // <span class="p_current">1 / 7</span>
        $span_total = $div_main->find('span[class=p_current]', 0);
        if ($span_total != null) {
            $str  = $span_total->innertext;
            $pos = stripos($str, "/") + 1;
            $str = substr($str, $pos);
            echo "span_total =" . $str . "\n";
            $ret = (int) $str;
        }

        return $ret;
    }
    //http://www.leleketang.com/chengyu/list39-1.shtml
    public  function ParseList($url)
    {
        $total_page = $this->GetPageCount($url);
        echo "total_page =" . $total_page . "\n";
        $pos = stripos($url, "-") + 1;
        $url_head = substr($url, 0, $pos);
        $listItem = array();
        for ($i = 1; $i <= $total_page; $i++) {
            $this->ParseonePageList($url_head . (string) $i . ".shtml", $listItem);
        }
        return $listItem;
    }

    //http://www.leleketang.com/chengyu/list39-1.shtml
    public  function ParseonePageList($url, &$listItem)
    {
        echo "ParseonePageList url= " . $url . "\n";
        $html = get_html($url);
        if (!$html) {
            echo "open html fail\n";
            return false;
        }

        //<div class="gray mt f16">jù hǔ jìn láng</div> 
        $div_main = $html->find('div[class=list_anchor]', 0);
        $array_a = $div_main->find('span/a');
        foreach ($array_a as $a) {
            $str  = $a->title;
            echo "title =" . $str . "\n";
            $url = $this->WEB_HOME . $a->href;
            echo "href =" . $url . "\n";
            $p = new ParseIdiomItem();
            $info = $p->ParseIdiomItemInfo($url);
            array_push($listItem, $info);
        }
        // $this->SaveJsonList($listItem);

    }



    function GetJsonList($title, $category)
    {
        //save 
        $save_dir = $this->ROOT_SAVE_DIR;
        if (!is_dir($save_dir)) {
            mkdir($save_dir);
        }

        $save_dir = $this->ROOT_SAVE_DIR . "/" . $category;
        if (!is_dir($save_dir)) {
            mkdir($save_dir);
        }
        $savefilepath =  $save_dir . "/" . $title . ".json";
        return $savefilepath;
    }
    function SaveJsonList($list, $title, $category)
    {

        $savefilepath =  $this->GetJsonList($title, $category);

        // public $url;

        // public $usage; //成语用法
        // public $common_use;  //常用程度
        // public $emotional;  //感情色彩
        // public $structure; //成语结构：
        // public $near_synonym; //近义词 near-synonym
        // public $antonym; //反义词：Antonym:
        // public $example; //成语例句：
        // public $correct_pronunciation;  //成语正音：别，不能读作“biè”

        $listJson = array();
        foreach ($list as $info) {
            $element = array(
                'title' => urlencode($info->title), //
                'id' => urlencode($info->id), // 
                'album' => urlencode($info->album), // 
                'translation' => urlencode($info->translation), // 
                'pronunciation' => urlencode($info->pronunciation), // 
                // 'year' => urlencode($info->year), //  

            );
            array_push($listJson, $element);
        }

        //save  
        $ret = file_exists($savefilepath);
        if ($ret) {
            // return;
        }

        $count = count($listJson);
        echo "count =" . $count . "\n";
        if ($count) {

            $element = array(
                'items' => ($listJson),
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
}




// $url = "http://www.leleketang.com/chengyu/list39-1.shtml";
// // http://www.leleketang.com/chengyu/list39-2.shtml
// $p = new ParseIdiomList();
// $p->ParseList($url);

echo 'done<br>';
