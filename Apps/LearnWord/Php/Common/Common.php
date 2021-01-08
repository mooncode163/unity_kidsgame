<?php
function FileIsExist($filepath)
{
    $ret = file_exists($filepath);
    return $ret;
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
        // echo "BlankString null ";
        return true;
    }

    return false;
}

function RemoveHtmlSpace($str)
{
    $ret = str_replace("&nbsp;", "", $str);
    $ret = str_replace(" ", "", $ret);
    return  $ret;
}
