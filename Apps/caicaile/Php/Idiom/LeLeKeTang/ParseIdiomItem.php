<?php
// header("Content-type: text/html; charset=utf-8");


include_once('../../Common/Common.php');
include_once('../IdiomItemInfo.php');
include_once('../../Common/FirstLetter.php');

// http://www.leleketang.com/chengyu/18437.shtml
class ParseIdiomItem
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

    public  function ParseIdiomItemInfo($url)
    {
        $infoRet = new IdiomItemInfo();
        $html = get_html($url);
        if (!$html) {
            echo "open html fail\n";
            return false;
        }

        //<div class="gray mt f16">jù hǔ jìn láng</div> 
        $array_div = $html->find('div[class=idiom_explain_detail]');
        foreach ($array_div as $div) {
            $span = $div->find('span', 0);
            $strspan  = $span->innertext;
            // echo "span =" . $strspan . "\n";

            $div_content = $div->find('div[class=idiom_detail_content]', 0);

            $str  = $div_content->innertext;
            //删除标签
            $str = strip_tags($str);
            $str = removeHtmlSpace($str);
            $str = str_replace("&nbsp", "-", $str);
            // echo "div_content =" . $str . "\n";

            if (strstr($strspan, self::NAME_title) != false) {
                $infoRet->title = $str;
            }
            if (strstr($strspan, self::NAME_album) != false) {
                $infoRet->album = $str;
            }

            if (strstr($strspan, self::NAME_translation) != false) {
                $infoRet->translation = $str;
            }

            if (strstr($strspan, self::NAME_pronunciation) != false) {
                $infoRet->pronunciation = $str;
            }

            if (strstr($strspan, self::NAME_year) != false) {
                $infoRet->year = $str;
            }

            if (strstr($strspan, self::NAME_usage) != false) {
                $infoRet->usage = $str;
            }
            if (strstr($strspan, self::NAME_common_use) != false) {
                $infoRet->common_use = $str;
            }
            if (strstr($strspan, self::NAME_emotional) != false) {
                $infoRet->emotional = $str;
            }

            if (strstr($strspan, self::NAME_structure) != false) {
                $infoRet->structure = $str;
            }

            if (strstr($strspan, self::NAME_near_synonym) != false) {
                $infoRet->near_synonym = $str;
            }

            if (strstr($strspan, self::NAME_example) != false) {
                $infoRet->example = $str;
            }
            if (strstr($strspan, self::NAME_correct_pronunciation) != false) {
                $infoRet->correct_pronunciation = $str;
            }
            if (strstr($strspan, self::NAME_antonym) != false) {
                $infoRet->antonym = $str;
            }

            $infoRet->id = GetPinyin($infoRet->title);
            // echo "infoRet->id=" . $infoRet->id . "  title=" . $infoRet->title . "\n";
        }

        return $infoRet;
    }
}




$url = "http://www.leleketang.com/chengyu/18437.shtml";
$p = new ParseIdiomItem();
$p->ParseIdiomItemInfo($url);

echo 'done<br>';
