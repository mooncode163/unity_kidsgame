<?php
//获取整条字符串所有汉字拼音首字母
function GetPinyin($zh)
{
    $ret = "";
    $s1 = iconv("UTF-8", "GBK//IGNORE", $zh);
    $s2 = iconv("GBK", "UTF-8", $s1);
    if ($s2 == $zh) {
        $zh = $s1;
    }
    for ($i = 0; $i < strlen($zh); $i++) {
        $s1 = substr($zh, $i, 1);
        $p = ord($s1);
        if ($p > 160) {
            $s2 = substr($zh, $i++, 2);
            $ret .= getfirstchar($s2);
        } else {
            $ret .= $s1;
        }
    }
    return $ret;
}

//获取单个汉字拼音首字母。注意:此处不要纠结。汉字拼音是没有以U和V开头的
/**
 * 取汉字的第一个字的首字母
 * @param string $str
 * @return string|null
 */
function getFirstChar($str)
{
    if (empty($str)) {
        return '';
    }

    $fir = $fchar = ord($str[0]);
    if ($fchar >= ord('A') && $fchar <= ord('z')) {
        return strtoupper($str[0]);
    }
    $s1 = $str;
    $s2 = $s1;

    //@moon 导致HTML解析error
    // $s1 = @iconv('UTF-8', 'gb2312//IGNORE', $str);
    // $s2 = @iconv('gb2312', 'UTF-8', $s1);
    //@moon 

    $s = $s2 == $str ? $s1 : $str;
    if (!isset($s[0]) || !isset($s[1])) {
        return '';
    }

    $asc = ord($s[0]) * 256 + ord($s[1]) - 65536;

    if (is_numeric($str)) {
        return $str;
    }

    if (($asc >= -20319 && $asc <= -20284) || $fir == 'A') {
        return 'A';
    }
    if (($asc >= -20283 && $asc <= -19776) || $fir == 'B') {
        return 'B';
    }
    if (($asc >= -19775 && $asc <= -19219) || $fir == 'C') {
        return 'C';
    }
    if (($asc >= -19218 && $asc <= -18711) || $fir == 'D') {
        return 'D';
    }
    if (($asc >= -18710 && $asc <= -18527) || $fir == 'E') {
        return 'E';
    }
    if (($asc >= -18526 && $asc <= -18240) || $fir == 'F') {
        return 'F';
    }
    if (($asc >= -18239 && $asc <= -17923) || $fir == 'G') {
        return 'G';
    }
    if (($asc >= -17922 && $asc <= -17418) || $fir == 'H') {
        return 'H';
    }
    if (($asc >= -17417 && $asc <= -16475) || $fir == 'J') {
        return 'J';
    }
    if (($asc >= -16474 && $asc <= -16213) || $fir == 'K') {
        return 'K';
    }
    if (($asc >= -16212 && $asc <= -15641) || $fir == 'L') {
        return 'L';
    }
    if (($asc >= -15640 && $asc <= -15166) || $fir == 'M') {
        return 'M';
    }
    if (($asc >= -15165 && $asc <= -14923) || $fir == 'N') {
        return 'N';
    }
    if (($asc >= -14922 && $asc <= -14915) || $fir == 'O') {
        return 'O';
    }
    if (($asc >= -14914 && $asc <= -14631) || $fir == 'P') {
        return 'P';
    }
    if (($asc >= -14630 && $asc <= -14150) || $fir == 'Q') {
        return 'Q';
    }
    if (($asc >= -14149 && $asc <= -14091) || $fir == 'R') {
        return 'R';
    }
    if (($asc >= -14090 && $asc <= -13319) || $fir == 'S') {
        return 'S';
    }
    if (($asc >= -13318 && $asc <= -12839) || $fir == 'T') {
        return 'T';
    }
    if (($asc >= -12838 && $asc <= -12557) || $fir == 'W') {
        return 'W';
    }
    if (($asc >= -12556 && $asc <= -11848) || $fir == 'X') {
        return 'X';
    }
    if (($asc >= -11847 && $asc <= -11056) || $fir == 'Y') {
        return 'Y';
    }
    if (($asc >= -11055 && $asc <= -10247) || $fir == 'Z') {
        return 'Z';
    }

    return '';
}

// echo GetPinyin('测试活动');
