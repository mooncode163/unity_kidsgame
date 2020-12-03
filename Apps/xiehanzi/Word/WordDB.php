<?php

include_once('../Common/Sql/SqlDBUtil.php');
include_once('WordItemInfo.php');

class WordDB  extends SqlDBUtil
{
	const TABLE_NAME = 'TableItem';

	const KEY_text = 'text'; 

	const KEY_id = 'id';
	const KEY_word = 'word';
	const KEY_pinyin = 'pinyin';
	const KEY_zuci = 'zuci';
	const KEY_bushou = 'bushou';
	const KEY_bihua = 'bihua';
	const KEY_audio = 'audio';
	const KEY_gif = 'gif';
	const KEY_mean = 'mean';//含义

	 

	public $arrayCol = array(
		self::KEY_id, self::KEY_word, self::KEY_pinyin, self::KEY_zuci,
		self::KEY_bushou, self::KEY_bihua, self::KEY_audio, self::KEY_gif, self::KEY_mean
	);
	public $arrayColType;


	function __construct()
	{ }


	public function CreateDb()
	{
		$this->OpenFile("Data/OutPut/Item.db");
		$count = count($this->arrayCol);
		$this->arrayColType = array($count);
		for ($i = 0; $i < $count; $i++) {
			$this->arrayColType[$i] = self::KEY_text;
		}
		$this->CreateTableByName(self::TABLE_NAME, $this->arrayCol, $this->arrayColType);
	}
	public function ClearDB()
	{
		$this->DeleteTable(self::TABLE_NAME);
	}
	public function IsItemExist($info)
	{
		$ret = false;
		$sql = "SELECT * FROM "  . self::TABLE_NAME . " WHERE id = '" . $info->id . "'";
		//$sql = "select * from  where type='table' and name = '" . $this->table_name . "'";
		$result = $this->query($sql);
		$count = 0;
		// foreach ($result as $row) {
		// 	$count++;
		// }
		while ($res = $result->fetchArray(SQLITE3_ASSOC)) {
			$count++;
		}
		if ($count > 0) {
			$ret = true;
		}
		return $ret;
	}

	public function AddItem($info)
	{
		$count = count($this->arrayCol);
		$values = array($count);
		//id,filesave,date,addtime 

		$values[0] = $info->id; 
		$values[1] = $info->word;
		$values[2] = $info->pinyin;
		$values[3] = $info->zuci;
		$values[4] = $info->bushou;
		$values[5] = $info->bihua; 
		$values[6] = $info->audio;
		$values[7] = $info->gif;
		$values[8] = $info->mean; 
		

		$this->Insert2Table(self::TABLE_NAME, $values);
	}

	public function DeleteItem($info)
	{
		// string strsql = "DELETE FROM " + TABLE_NAME + " WHERE id = '" + info.id + "'" + " and addtime = '" + info.addtime + "'";
		$sql = "DELETE FROM " . self::TABLE_NAME . " WHERE id = '" . $info -> id . "'";
		$result = $this->query($sql);
	}

	//
	public function GetCount()
	{
		$sql = "select  *  from  " . self::TABLE_NAME;
		$result = $this->query($sql);
		$count = 0;
		while ($item = $result->fetchArray(SQLITE3_ASSOC)) {
			$count++;
		}
		return $count;
	}

	public function GetAllItem()
	{
		// Distinct 去掉重复
		//desc 降序 asc 升序 
		//string strsql = "select DISTINCT id from " + TABLE_NAME + " order by addtime desc";
		$sql = "select  *  from  " . self::TABLE_NAME;
		//$sql = "select  DISTINCT id  from  " . self::TABLE_NAME;
		$result = $this->query($sql);
		$listRet = array();
		$i = 0;
		while ($item = $result->fetchArray(SQLITE3_ASSOC)) {
			echo "read  result i=" . $i . "\n";
			// $row[$i]['user_id'] = $res['NAME']; 
			$info = $this->ReadInfo($item);
			array_push($listRet, $info);
			$i++;
		}
		return $listRet;
	}


	public function ReadInfo($item)
	{
		$info = new WordItemInfo();

		$info->id =   $item[self::KEY_id];
		$info->word =   $item[self::KEY_word];
		$info->pinyin =   $item[self::KEY_pinyin];
		$info->zuci =   $item[self::KEY_zuci];
		$info->bushou =   $item[self::KEY_bushou];
		$info->bihua =   $item[self::KEY_bihua];
		$info->audio =   $item[self::KEY_audio];
		$info->gif =   $item[self::KEY_gif];
		$info->mean =   $item[self::KEY_mean];
 

		return $info;
	}
}
