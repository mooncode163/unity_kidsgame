<?php
header("Content-type: text/html; charset=utf-8");

include('ParseIdiomItem.php');
include_once('IdiomItemInfo.php');
include_once('DBIdiom.php');
include_once('../Common/FirstLetter.php');
include_once('../Common/Common.php');

//乐乐课堂 http://www.leleketang.com
//http://www.leleketang.com/chengyu/
//https://chengyu.911cha.com
class CreateIdiomDB //extends Thread
{
    const NAME_title = '成语名字';
    const NAME_pronunciation  = '成语发音';
    const NAME_translation = '成语解释';
    const NAME_album = '成语出处';
    const NAME_zhtw = '成语繁体';

    const NAME_emotional = '感情色彩';
    const NAME_usage = '成语用法';
    const NAME_structure = '成语结构';
    const NAME_year = '产生年代';
    const NAME_common_use = '常用程度';

    const NAME_near_synonym = '近义词';
    const NAME_antonym = '反义词';
    const NAME_example = '成语例句';
    const NAME_correct_pronunciation = '成语正音';


    public $url;
    public $id;
    public $channel;
    public $listItem;
    public $listSort;
    public  $page_total = 10; //10
    public $htmlItem;
    public $dbIdiom;

    public $WEB_HOME = "http://www.leleketang.com";
    public $ROOT_SAVE_DIR = "Data";
    public $sortJsonList;

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

    public function DoCreate()
    {

        $this->listItem =   array();
        $this->listSort =   array();

        $this->htmlItem =   new simple_html_dom();
        $this->sortJsonList = $this->ROOT_SAVE_DIR . "/idiom_sort_list.json";
        $this->InitDB();

        //https://chengyu.911cha.com/pinyin_a.html
        $save_dir = $this->ROOT_SAVE_DIR . "/All_Idiom";
        if (!is_dir($save_dir)) {
            mkdir($save_dir);
        }

        $fiel_exist = file_exists($this->sortJsonList);
        if (!$fiel_exist) {
            $this->PaserSort();
        }
        $this->PaserIdiom();

        $total = $this->dbIdiom->GetCount();
        echo "total idiom=" . $total . "\n";
    }


    //按字母分类
    function PaserSort()
    {
        //cy_tag_list
        $html = get_html("http://www.leleketang.com/chengyu");
        if (!$html) {
            echo "open html fail\n";
            return;
        }
        //<span class="choice">
        $div_main = $html->find('div[class=cy_tag_list]', 0);
        if (!$div_main) {
            echo "PaserSortByLetter find div_main fail\n";
            return;
        }

        $arry_a = $div_main->find('a');
        $count = count($arry_a);
        echo "arry_a count =" . $count . "\n";

        //https://chengyu.911cha.com/pinyin_b.html
        foreach ($arry_a as $a) {
            $url = $this->WEB_HOME . $a->href;
            $title =  $a->plaintext;
            if (($title != "Y") && ($title != "Z")) {
                //  continue;
            }
            $total_page = $this->GetPageCount($url);
            // echo "PaserSort url:  " . $url . " title:" . $title . "total_page=" . $total_page . "\n";
            for ($i = 0; $i < $total_page; $i++) {
                // $this->PaserIdiomList($this->GetPageUrl($this->GetChannelIdOfUrl($url), $i));
            }

            $element = array(
                'title' => urlencode($title),
                'url' => urlencode($url),
                'total' => urlencode($total_page),
                "id"  => urlencode($this->GetChannelIdOfUrl($url)),
            );
            if ($total_page > 0) {
                array_push($this->listSort, $element);
            }
        }
        $this->SaveSortJsonList();
    }

    function PaserIdiom()
    {
        $save_file = $this->sortJsonList;
        $fiel_exist = file_exists($save_file);
        if ($fiel_exist) {
            $json_string = file_get_contents($save_file);
            $root = json_decode($json_string, true);
            $data = $root['items'];
            foreach ($data as $item) {
                $url = $item['url'];
                $title =  $item['title']; 
                $total_page =  $item['total'];
                $id = $item['id'];
                echo "PaserIdiom url 1=:  " . $url . " title:" . $title . "total_page=" . $total_page . "\n";
                for ($i = 0; $i < $total_page; $i++) {
                    $this->PaserIdiomList($this->GetPageUrl($id, $i));
                }
            }
        } else {
            foreach ($this->listSort as $sort) {
                $url = $sort['url'];
                $title =  $sort['title'];
                if ($title != "D") { //error B 
                    continue;
                }
                $total_page =  $sort['total'];
                $id = $sort['id'];
                echo "PaserIdiom url 2=:  " . $url . " title:" . $title . "total_page=" . $total_page . "\n";
                for ($i = 0; $i < $total_page; $i++) {
                    $this->PaserIdiomList($this->GetPageUrl($id, $i));
                }
            }
        }
    }

    function SaveSortJsonList()
    {
        //save 
        $savefilepath = $this->sortJsonList;
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

    //http://www.leleketang.com/chengyu/list501-1.shtml
    function GetPageUrl($id, $page)
    {
        return "http://www.leleketang.com/chengyu/list" . $id . "-"  . ($page + 1) . ".shtml";
    }

    function GetChannelIdOfUrl($url)
    {
        $ret = "";
        $strfind = 'list';
        $pos = strpos($url, $strfind);
        if ($pos) {
            $str = substr($url, $pos + strlen($strfind));
            $strfind = "-";
            $pos = strpos($str, $strfind);
            $ret = substr($str, 0, $pos - strlen($strfind) + 1);
        } else {
            echo "GetChannelIdOfUrl url:  " . $url . " pos=" . $pos . "\n";
        }

        return $ret;
    }

    function GetPageCount($url)
    {
        $html = get_html($url);
        $ret = 0;
        if (!$html) {
            echo "GetPageCount open html fail\n";
            return 0;
        }
        $div_main = $html->find('span[class=p_current]', 0);
        if (!$div_main) {
            echo "GetPageCount find div_main fail\n";
            return 0;
        }
        //2 / 24
        $str = $div_main->plaintext;
        $pos = strpos($str, "/");
        if ($pos != false) {
            $str = substr($str, $pos + 1);
            $ret = (int) $str;
        }

        return $ret;
    }

    //https://chengyu.911cha.com/pinyin_a.html
    function PaserIdiomList($url)
    {
        echo "PaserIdiomList url:  " . $url . "\n";
        $html = get_html($url);
        if (!$html) {
            echo "PaserIdiomList open html fail\n";
            return;
        }
        //table : id table1
        $div_main = $html->find('div[class=list_anchor]', 0);
        if (!$div_main) {
            echo "PaserIdiomList find div_main fail url=" . $url . "\n";
            return;
        }
        //  echo "div_main =\n" .  $div_main . innertext;

        $arry_a = $div_main->find('a');
        $count = count($arry_a);
        echo "arry_a count =" . $count . "\n";


        foreach ($arry_a as $a) {
            if ($a->class) {
                continue;
            }
            $url = $this->WEB_HOME . "/chengyu/" . $a->href;
            // echo "arry_a url:  " . $url  . "\n";
            $info = $this->ParseIdiomItemInfo($url);

            $title =  $a->plaintext;
            $element = array(
                'url' => $url, //谜面
                'title' => $title,
                'pinyin' => $info->pronunciation,
                'translation' => $info->translation,
                'album' => $info->album,
            );
            array_push($this->listItem, $element);
            $this->SaveOneIdiom($info);
        }
    }

    function ParseIdiomItemInfoByType($html, $type)
    {
        $str_ret = "";
        $array_div = $html->find('div[class=idiom_explain_detail]');
        if (!$array_div) {
            echo "ParseIdiomItemInfo find div_main fail\n";
            return;
        }

        $is_find_item = false;
        foreach ($array_div as $div) {
            $span = $div->find('span', 0);
            if ($span) {
                $str =  $span->plaintext;
                if (strstr($str, $type) != false) {
                    $div_child = $div->find('div', 0);
                    $str_ret =  $div_child->plaintext;
                    break;
                }
            }
        }
        // $str_ret =  RemoveHtmlSpace($str_ret);

        //网页空格
        $str_ret = str_replace("	", "", $str_ret);

        if ($type != self::NAME_pronunciation) {
            //普通空格
            $str_ret = str_replace(" ", "", $str_ret);
            $str_ret = RemoveHtmlSpace($str_ret);
        } else {
            //读音去除开头和结尾的2个空格
            $str_ret = str_replace("  ", "", $str_ret);
        }

        //'为DB 关键字 转换为’
        $str_ret = str_replace("'", "\"", $str_ret);

        return $str_ret;
    }

    public  function ParseIdiomItemInfo($url)
    {
        $infoRet = new IdiomItemInfo();
        // get DOM from URL or file
        $html =  $this->get_html($url);
        if (!$html) {
            echo "open html fail\n";
            return false;
        }
        echo "ParseIdiomItemInfo start url=" . $url . "\n";
        $infoRet->title = $this->ParseIdiomItemInfoByType($html, self::NAME_title); // 
        echo "ParseIdiomItemInfo end title=" . $infoRet->title . "\n";
        $infoRet->id = GetPinyin($infoRet->title); // $this->ParseIdiomItemInfoByType($html, self::NAME_id);
        echo "infoRet->id=" . $infoRet->id . "  title=" . $infoRet->title . "\n";
        $infoRet->album = $this->ParseIdiomItemInfoByType($html, self::NAME_album);
        $infoRet->translation = $this->ParseIdiomItemInfoByType($html, self::NAME_translation);
        $infoRet->pronunciation = $this->ParseIdiomItemInfoByType($html, self::NAME_pronunciation);


        $infoRet->year = $this->ParseIdiomItemInfoByType($html, self::NAME_year);
        $infoRet->usage = $this->ParseIdiomItemInfoByType($html, self::NAME_usage);
        $infoRet->common_use = $this->ParseIdiomItemInfoByType($html, self::NAME_common_use);
        $infoRet->emotional = $this->ParseIdiomItemInfoByType($html, self::NAME_emotional);
        $infoRet->structure = $this->ParseIdiomItemInfoByType($html, self::NAME_structure);


        $infoRet->near_synonym = $this->ParseIdiomItemInfoByType($html, self::NAME_near_synonym);
        $infoRet->antonym = $this->ParseIdiomItemInfoByType($html, self::NAME_antonym);
        $infoRet->example = $this->ParseIdiomItemInfoByType($html, self::NAME_example);
        $infoRet->correct_pronunciation = $this->ParseIdiomItemInfoByType($html, self::NAME_correct_pronunciation);

        return $infoRet;
    }

    public function InitDB()
    {
        $this->dbIdiom = new IdiomDB();
        $this->dbIdiom->CreateDb();
    }
    function  SaveOneIdiom($info)
    {
        if (!$this->dbIdiom->IsItemExist($info)) {
            $this->dbIdiom->AddItem($info);
        }

        // $save_dir = $this->ROOT_SAVE_DIR . "/All_Idiom";
        // if (!is_dir($save_dir)) {
        //     mkdir($save_dir);
        // }
        // //save
        // $savefilepath = $save_dir . "/" . $info->title . ".json";
        // $ret = file_exists($savefilepath);
        // if ($ret) {
        //     // return;
        // } {

        //     $element = array(
        //         'title' => $info->title,
        //         'id' => $info->id,
        //         'pinyin' => $info->pronunciation,
        //         'translation' => $info->translation,
        //         'album' => $info->album,
        //     );
        //     //JSON_UNESCAPED_SLASHES json去除反斜杠 JSON_UNESCAPED_UNICODE中文不用\u格式
        //     $jsn = urldecode(json_encode($element, JSON_UNESCAPED_SLASHES | JSON_UNESCAPED_UNICODE));

        //     // "[  ]"
        //     //$jsn = str_replace("\"[", "[", $jsn);
        //     //$jsn = str_replace("]\"", "]", $jsn);

        //     $fp = fopen($savefilepath, "w");
        //     if (!$fp) {
        //         echo "打开文件失败<br>";
        //         return;
        //     }
        //     $flag = fwrite($fp, $jsn);
        //     if (!$flag) {
        //         echo "写入文件失败<br>";
        //     }
        //     fclose($fp);
        // }
    }
}

$p = new CreateIdiomDB();
$p->DoCreate();

//http://www.leleketang.com/chengyu/8489.shtml
//http://www.leleketang.com/chengyu/7530.shtml
//http://www.leleketang.com/chengyu/11871.shtml
// $p->InitDB();
// $info = $p->ParseIdiomItemInfo("http://www.leleketang.com/chengyu/11871.shtml");
// $info = $p->ParseIdiomItemInfo("http://www.leleketang.com/chengyu/7530.shtml");
// $p->SaveOneIdiom($info);

// echo "GetPinyin=" . GetPinyin("中华") . "\n";
echo 'done<br>';
