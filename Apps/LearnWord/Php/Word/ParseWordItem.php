<?php
include_once('../Common/HtmlUtil.php');
include_once('WordItemInfo.php');
include_once('WordDB.php');
// include('../Common/Common.php');


/**
 * utf8字符转换成Unicode字符
 * @param [type] $utf8_str Utf-8字符
 * @return [type]      Unicode字符
 */
function utf8_str_to_unicode($utf8_str)
{
    $unicode = 0;
    $unicode = (ord($utf8_str[0]) & 0x1F) << 12;
    $unicode |= (ord($utf8_str[1]) & 0x3F) << 6;
    $unicode |= (ord($utf8_str[2]) & 0x3F);
    return dechex($unicode);
}


/**
 * Unicode字符转换成utf8字符
 * @param [type] $unicode_str Unicode字符
 * @return [type]       Utf-8字符
 */
function unicode_to_utf8($unicode_str)
{
    $utf8_str = '';
    $code = intval(hexdec($unicode_str));
    //这里注意转换出来的code一定得是整形，这样才会正确的按位操作
    $ord_1 = decbin(0xe0 | ($code >> 12));
    $ord_2 = decbin(0x80 | (($code >> 6) & 0x3f));
    $ord_3 = decbin(0x80 | ($code & 0x3f));
    $utf8_str = chr(bindec($ord_1)) . chr(bindec($ord_2)) . chr(bindec($ord_3));
    return $utf8_str;
}

class ParseWordItem
{

    public $dbItem;
    //poem
    public  function ParseWordItemInfo($word)
    {

        $url = "https://hanyu.baidu.com/s?wd=" . $word . "&ptype=zici";
        $infoRet = new WordItemInfo();
        $infoRet->word = $word;
        $infoRet->id = '\u'.utf8_str_to_unicode($word);

        // get DOM from URL or file
        $html = HtmlUtil::Main()->get_html($url);
        if (!$html) {
            echo "open html fail\n";
            return false;
        }

        $div_pinyin = $html->find('div[id=pinyin]', 0);
        $b = $div_pinyin->find('b', 0);
        $infoRet->pinyin  = $b->innertext;
        $a = $div_pinyin->find('a', 0);
        $infoRet->audio  = $a->url;


        $li = $html->find('li[id=radical]', 0);
        $span = $li->find('span', 0);
        $infoRet->bushou  = $span->innertext;

        //五笔
        $li = $html->find('li[id=wubi]', 0);
        if ($li != null) {
            $span = $li->find('span', 0);
            // $infoRet->id  = $span->innertext;
        }


        $li = $html->find('li[id=stroke_count]', 0);
        $span = $li->find('span', 0);
        $infoRet->bihua  = $span->innertext;


        //word_bishun gif
        $img = $html->find('img[id=word_bishun]', 0);
        echo $img;
        $key = "data-gif";
        $infoRet->gif = $img->$key;

        //组词
        $div_zuci = $html->find('div[id=zuci-wrapper]', 0);
        $array_a = $div_zuci->find('a');
        foreach ($array_a as $a) {
            $zuci  = $a->innertext;
            $infoRet->zuci = $zuci;
            break;
        }

        // 解释
        $div = $html->find('div[id=basicmean-wrapper]', 0);
        $p = $div->find('div/dl/dd/p', 0);
        $infoRet->mean  = HtmlUtil::Main()->strip_tags($p->innertext);


        $this->SaveWordToDB($infoRet);
        $this->SaveWord($infoRet, "Data/OutPut/" . $infoRet->word . ".json");
        return $infoRet;
    }

    function SaveWordToDB($info)
    {
        if ($this->dbItem == null) {
            $this->InitDB();
        }
        if (!$this->dbItem->IsItemExist($info)) {
            $this->dbItem->AddItem($info);
        }
    }


    public function InitDB()
    {
        $this->dbItem = new WordDB();
        $this->dbItem->CreateDb();
    }


    function SaveWord($info, $save_file)
    {
        $element = array(
            'id' => urlencode($info->id),
            'word' => urlencode($info->word),
            'pinyin' => urlencode($info->pinyin),
            'zuci' => urlencode($info->zuci),
            'bushou' => urlencode($info->bushou),
            'bihua' => urlencode($info->bihua),
            'audio' => urlencode($info->audio),
            'gif' => urlencode($info->gif),
            'mean' => urlencode($info->mean),
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




// $word = "舵";
// $p = new ParseWordItem();
// $info = $p->ParseWordItemInfo($word);


echo 'done<br>';
