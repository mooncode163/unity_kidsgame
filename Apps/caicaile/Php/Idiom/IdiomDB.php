<?php

include_once('../Common/Sql/SqlDBUtil.php');
include_once('IdiomItemInfo.php');

class IdiomDB  extends SqlDBUtil
{
	const TABLE_NAME = 'TableIdiom';

	const KEY_text = 'text';


	const KEY_id = 'id';
	const KEY_title = 'title';
	const KEY_album = 'album';
	const KEY_translation = 'translation';
	const KEY_pinyin = 'pronunciation';
	const KEY_year = 'year';

	const KEY_usage = 'usage';
	const KEY_common_use = 'common_use';

	const KEY_emotional = 'emotional';
	const KEY_structure = 'structure';
	const KEY_near_synonym = 'near_synonym';  //近义词
	const KEY_antonym = 'antonym'; //反义词  
	const KEY_example = 'example';
	const KEY_correct_pronunciation = 'correct_pronunciation'; //成语正音：别，不能读作“biè”


	public $arrayCol = array(
		self::KEY_id, self::KEY_title, self::KEY_pinyin, self::KEY_album,
		self::KEY_translation, self::KEY_year, self::KEY_usage,
		self::KEY_common_use, self::KEY_emotional, self::KEY_structure,
		self::KEY_near_synonym, self::KEY_antonym, self::KEY_example, self::KEY_correct_pronunciation
	);
	public $arrayColType;


	function __construct()
	{ }


	public function CreateDb()
	{
		$this->OpenFile("Idiom.db");
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
		//values[0] = "性";//ng
		$values[1] = $info->title;
		$values[2] = $info->pronunciation;
		$values[3] = $info->album;
		$values[4] = $info->translation;

		$values[5] = $info->year;
		$values[6] = $info->usage;
		$values[7] = $info->common_use;
		$values[8] = $info->emotional;

		$values[9] = $info->structure;
		$values[10] = $info->near_synonym;
		$values[11] = $info->antonym;
		$values[12] = $info->example;
		$values[13] = $info->correct_pronunciation;

		$this->Insert2Table(self::TABLE_NAME, $values);
	}

	public function DeleteItem($info)
	{
		// string strsql = "DELETE FROM " + TABLE_NAME + " WHERE id = '" + info.id + "'" + " and addtime = '" + info.addtime + "'";
		$sql = "DELETE FROM " . self::TABLE_NAME . " WHERE id = '" . $info . id . "'";
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
		$info = new IdiomItemInfo();

		$info->id =   $item[self::KEY_id];
		$info->title =   $item[self::KEY_title];
		$info->pronunciation =   $item[self::KEY_pinyin];
		$info->album =   $item[self::KEY_album];
		$info->translation =   $item[self::KEY_translation];

		$info->year =   $item[self::KEY_year];
		$info->usage =   $item[self::KEY_usage];
		$info->common_use =   $item[self::KEY_common_use];
		$info->emotional =   $item[self::KEY_emotional];
		$info->structure =   $item[self::KEY_structure];

		$info->near_synonym =   $item[self::KEY_near_synonym];
		$info->antonym =   $item[self::KEY_antonym];
		$info->example =   $item[self::KEY_example];
		$info->correct_pronunciation =   $item[self::KEY_correct_pronunciation];

		echo "id=" . $info->id . "\n";
		echo "title=" . $info->title . "\n";
		return $info;
	}
}
