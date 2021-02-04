<?php
header("Content-type: text/html; charset=utf-8"); //gb2312 utf-8

include('./simple_html_dom.php');

include('./RiddleItemInfo.php');

//http://www.cmiyu.com/

define("NAME_YUANWEN", "作品原文");
$is_find_poem = false;
$gTitleHead = "";
$gTitleEnd = "";
$gTitleTips = "";
$gTitleType = "";


$listItem = array();

// "title": "送别",
// "author": "王维",
// "year": "唐代",
// "style": "五言古诗",
// "album": "全唐诗",
/*
    百度百科 唐诗三百首
    https://baike.baidu.com/item/%E5%94%90%E8%AF%97%E4%B8%89%E7%99%BE%E9%A6%96/18677

    item:
    https://baike.baidu.com/item/%E8%B4%BC%E9%80%80%E7%A4%BA%E5%AE%98%E5%90%8F/6960505?fromtitle=%E8%B4%BC%E9%80%80%E7%A4%BA%E5%AE%98%E5%90%8F%C2%B7%E5%B9%B6%E5%BA%8F&fromid=9424895
    
    */

class ThreadParser //extends Thread
{
    public $url;
    public $id;
    public $channel;
    public $listItem;
    public  $page_total = 10; //10
    public $htmlItem;

    public function run()
    {
        echo "ThreadParse\n";
        $this->listItem =   array();
        $this->htmlItem =   new simple_html_dom();

        $this->ParserListThread($this->url, $this->id, $this->channel);
    }


    function ParserRiddleItem($url)
    {
        $info = new RiddleItemInfo();

        // get DOM from URL or file 
        //$this->htmlItem->clear();
        // $this->htmlItem->load_file($url);

        $this->htmlItem = get_html($url);

        $div_main = $this->htmlItem->find('div[class=md]', 0);
        if (!$div_main) {
            echo "ParserItem find div_main fail\n";
            return  $info;
        }
        $idx = 0;
        echo "div_main:" .   $div_main->innertext . "\n";

        $str_divmain = $div_main->innertext;
        $str_divmain = removeHtmlSpace($str_divmain);
        $str_end = "</h3>";

        $str_find = "谜面：";
        //谜面：忍者自找苦吃 （打一成语）
        $pos_start = strpos($str_divmain, $str_find);
        if ($pos_start != false) {
            $str = substr($str_divmain, $pos_start);
            $pos_end = strpos($str, $str_end);
            $str = substr($str, 0, $pos_end);
            $str = str_replace($str_find, "", $str);
            $info->head =  $str;
            //echo "gTitleHead:" .   $gTitleHead . "\n";
            //讲课还是老一套 （打一成语）
            $str_split0 = "（";
            $str_split1 = "）";

            $pos_end = strpos($info->head, $str_split0);
            $info->head = substr($info->head, 0, $pos_end);
            $info->head = str_replace($str_split0, "", $info->head);

            $info->type = substr($str, $pos_end);
            $info->type = str_replace($str_split0, "", $info->type);
            $info->type = str_replace($str_split1, "", $info->type);
        }

        $str_find = "谜底：";
        $pos_start = strpos($str_divmain, $str_find);
        if ($pos_start != false) {
            $str = substr($str_divmain, $pos_start);
            $pos_end = strpos($str, $str_end);
            $str = substr($str, 0, $pos_end);
            $str = str_replace($str_find, "", $str);
            $info->end =  $str;
            //echo "gTitleEnd:" .   $gTitleEnd . "\n";
        }


        // 小贴士：“枫林”二字里隐藏了“杀机”，余下“木”字。
        $div_main = $this->htmlItem->find('div[class=zy]', 0);
        if (!$div_main) {
            echo "ParserItem find div_main  2 fail\n";
            return  $info;
        } {
            $str = $div_main->plaintext;
            // $str = $div_main2->plaintext;
            //删除标签
            $str = strip_tags($str);
            $str = str_replace("小贴士：", "", $str);
            $str = removeHtmlSpace($str);
            $str = str_replace("\"", "'", $str);
            $str = str_replace("\\", "/", $str);
            $info->tips =  $str;
        }


        return  $info;
    }

    function ParserRiddleList($url)
    {

        $html = get_html($url);
        if (!$html) {
            echo "open html fail\n";
            return false;
        }

        $info = null;
        $div_main = $html->find('div[class=list]', 0);
        $array_a = $div_main->find('li/a');
        foreach ($array_a as $a) {
            http: //www.cmiyu.com/gxmy/37784.html
            $url_a = "http://www.cmiyu.com" . $a->href;
            echo $url_a . "\n";
            //echo $a->title . "\n";
            $info = $this->ParserRiddleItem($url_a);
            $element = array(
                'head' => urlencode($info->head), //谜面
                'end' => urlencode($info->end), //谜底 
                'tips' => urlencode($info->tips), //谜底 
                'type' => urlencode($info->type),

            );
            array_push($this->listItem, $element);
        }
    }

    function GetPageUrl($url, $channel, $idx)
    {
        //http://www.cmiyu.com/qtmy/my352.html
        $ret = $url . "my" . $channel . $idx . ".html";
        return $ret;
    }
    public function ParserListThread($url, $id, $channel)
    {

        $file_list = "Riddle/item_" . $id . ".json";
        $ret = file_exists($file_list);
        if ($ret) {
            return;
        }
        array_splice($this->listItem, 0, count($this->listItem));

        for ($i = 0; $i < $this->page_total; $i++) {
            $url_page = $this->GetPageUrl($url, $channel, $i + 1);
            echo "page: i=" . $i . "  " . $url_page . "\n";
            $this->ParserRiddleList($url_page);
        }
        if (count($this->listItem)) {

            $element = array(
                'items' => ($this->listItem),
            );
            //$jsn = urldecode(json_encode($this->listItem));
            $jsn = urldecode(json_encode($element));
            $fp = fopen($file_list, "w");
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




function BlankString($str)
{
    if (!is_string($str)) {
        return false; //判断是否是字符串类型
    }
    // if (empty($str)) { //判断是否已定义字符串
    //     echo "BlankString empty ";
    //     return true;
    // }
    if ($str == "") {
        echo "BlankString null ";
        return true;
    }

    return false;
}

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

/*
删除HTML标签和标签的内容。

使用方法：strip_html_tags($tags,$str)

$tags：需要删除的标签（数组格式）
$str：需要处理的字符串；
*/
function strip_html_tags($tags, $str)
{
    $html = array();
    foreach ($tags as $tag) {
        $html[] = '/<' . $tag . '.*?>[\s|\S]*?<\/' . $tag . '>/';
        $html[] = '/<' . $tag . '.*?>/';
    }
    $data = preg_replace($html, '', $str);
    return $data;
}
function removeHtmlSpace($str)
{
    $ret = str_replace("&nbsp;", "", $str);
    $ret = str_replace(" ", "", $ret);
    return  $ret;
}


function ParserSortId($url)
{
    //<li class="sy2"><a href="my202.html">2</a></li>
    $html = get_html($url);
    if (!$html) {
        echo "open html fail\n";
        return false;
    }
    $li_main = $html->find('li[class=sy2]', 0);
    $a = $li_main->find('a', 0);
    $ret = str_replace("my", "", $a->href);
    $ret = str_replace("2.html", "", $ret);
    return $ret;
}



function ParserSortList($url, $save_file)
{
    global $page_total;
    global $listItem;

    $html = get_html($url);
    if (!$html) {
        echo "open html fail\n";
        return false;
    }

    $listSort = array();

    $fiel_exist = file_exists($save_file);
    if ($fiel_exist) {
        $json_string = file_get_contents($save_file);
        $data = json_decode($json_string, true);
        foreach ($data as $item) {
            $info = new RiddleItemInfo();
            $info->url = $item['url'];
            $info->title = $item['title'];
            if (isset($item['id'])) {
                $info->id = $item['id'];
            }
            $info->channel = $item['channel'];
            if ($info->id) {
                array_push($listSort, $info);
            }
        }
    } else {
        $div_main = $html->find('div[class=miyuheader]', 0);
        $array_a = $div_main->find('a');
        foreach ($array_a as $a) {
            $info = new RiddleItemInfo();
            http: //www.cmiyu.com/gxmy/37784.html
            $info->url = "http://www.cmiyu.com" . $a->href;
            $info->title = $a->plaintext;

            // /gxmy/ 
            $info->id =  $a->href;
            $info->id = str_replace("/", "", $info->id);

            $info->channel = ParserSortId($info->url);


            //echo $url_page . "\n";
            // innertext – 返回innerHTML
            //outertext – 返回outerHTML
            //plaintext – 返回html标签中的文本


            array_push($listSort, $info);
        }
    }


    if (count($listSort) && (!$fiel_exist)) {
        $listJson = array();
        foreach ($listSort as $info) {
            $element = array(
                'url' => urlencode($info->url), //
                'title' => urlencode($info->title), //
                'id' => urlencode($info->id), // 
                'channel' => urlencode($info->channel), // 
            );
            array_push($listJson, $element);
        }



        $jsn = urldecode(json_encode($listJson));

        $fp = fopen($save_file, "w");
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


    foreach ($listSort as $info) {
        ParserList($info->id, $info->channel);
    }
}
function ParserList($id, $channel)
{
    //"http://www.cmiyu.com/zmmy/";
    $url = "http://www.cmiyu.com/" . $id . "/";
    $thread = new ThreadParser();
    $thread->url = $url;
    $thread->id = $id;
    $thread->channel = $channel;
    // $thread->start() && $thread->join();
    $thread->run();
}


$url = "http://www.cmiyu.com/cymy/39070.html";
//ParserRiddleItem($url);

/*
ng 列表：

"url": "http://www.cmiyu.com/etmy/",
        "title": "儿童谜语",
        "id": "tid}"、

         "url": "http://www.cmiyu.com/wpmy/",
        "title": "物品谜语",
        "id": "29"


        symy
        28

        dwmy
        25

        zmmy
        13

        rmmy
        22

        dm
        21

        dgmy
        24

        // zwmy
        // 33

        ysmy
        32

        // cwmy
        // 23

         
*/
//  { 
//     $id = "etmy";
//     $channel = "tid}";
//     ParserList($id, $channel);
// }

{
    $id = "wpmy";
    $channel = "29";
    // ParserList($id, $channel);
}
/*
 {
    $url_a = "http://www.cmiyu.com/symy/";
    $id = "28";
    ParserList($url_a, $id);
}

{
    $url_a = "http://www.cmiyu.com/zmmy/";
    $id = "13";
    ParserList($url_a, $id);
}


*/

$url = "http://www.cmiyu.com/";
$dir = "Riddle";
if (!is_dir($dir)) {
    mkdir("Riddle");
}

$filepath = "Riddle/riddle_sort.json";
ParserSortList($url, $filepath);

echo 'done<br>';
