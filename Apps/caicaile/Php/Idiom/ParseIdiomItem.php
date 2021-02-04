<?php
header("Content-type: text/html; charset=utf-8");

include('../Common/Html/simple_html_dom.php');
include('IdiomItemInfo.php');
// include('../Common/Common.php');


define("NAME_TITLE", "词目");
define("NAME_PINYIN", "发音");
define("NAME_TRANSLATION", "成语解释");
define("NAME_ALBUM", "成语出处");
define("NAME_ID", "成语简拼");

class ParseIdiomItem
{
    function ParseIdiomItemInfoByType($html, $type)
    {
        $str_ret = "";
        $div_main = $html->find('div[class=mcon bt noi f14]', 0);
        if (!$div_main) {
            echo "ParseIdiomItemInfo find div_main fail\n";
            return;
        }
        $str = $div_main->innertext;
        //echo "div_main =\n" . $str;

        $arry_span = $div_main->find('span[class=tt]');
        $is_find_item = false;
        foreach ($arry_span as $span) {
            $str =  $span->innertext;
            if ($str == $type) {
                $str =  $span->parent->innertext;
                //删除相关标签和内容
                $str = strip_html_tags(array('span', 'a'), $str);
                echo "ParseIdiomItemInfo  " . $str . "\n";
                $str_ret = $str;
                break;
            }
        }

        $str_ret = str_replace("911cha.com", "", $str_ret);
        return $str_ret;
    }

    //poem
    public  function ParseIdiomItemInfo($url)
    {
        $infoRet = new IdiomItemInfo();

        // $gUrl = $url;

        // get DOM from URL or file
        $html = get_html($url);
        if (!$html) {
            echo "open html fail\n";
            return false;
        }

        //<div class="gray mt f16">jù hǔ jìn láng</div> 
        $array_div = $html->find('div');
        foreach ($array_div as $div) {

            $ret = strstr($div->class, 'gray mt f16');
            if ($div->class == "gray mt f16") {
                //  echo "div class =" . $div->class . "\n";
                $infoRet->pronunciation  = $div->innertext;

                // echo "div parent =" . $div->parent->innertext . "\n";
                $str = strip_html_tags(array('div'), $div->parent->innertext);

                //删除标签
                $str = strip_tags($str);

                $infoRet->title = removeHtmlSpace($str);
                //$infoRet->id = $infoRet->title;
            }
        }

        $infoRet->translation = $this->ParseIdiomItemInfoByType($html, constant("NAME_TRANSLATION"));
        $infoRet->album = $this->ParseIdiomItemInfoByType($html, constant("NAME_ALBUM"));
        $infoRet->id = $this->ParseIdiomItemInfoByType($html, constant("NAME_ID"));


        return $infoRet;
    }
    function SaveIdiom($info, $save_file)
    {
        $element = array(
            'title' => urlencode($info->title),
            'pinyin' => urlencode($info->pronunciation),
            'album' => urlencode($info->album),
            // 'url' => urlencode($gUrl),
            'translation' => urlencode($info->translation),
        );
        $jsn = urldecode(json_encode($element, JSON_UNESCAPED_SLASHES | JSON_UNESCAPED_UNICODE));

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
}




/*
https://chengyu.911cha.com/YmJz.html
    */



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
//




$url = "https://chengyu.911cha.com/YmJz.html";
//ParserChengyuItem($url, "chengyu_item.json");
//ParserChengyuItem("1.html", "chengyu_item.json");


echo 'done<br>';
