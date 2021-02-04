<?php

include_once('../Common/Sql/SqlDBUtil.php');
class CrazyImageDB  extends SqlDBUtil
{
	const TABLE_NAME = 'TableImage';

	const KEY_text = 'text';
	const KEY_id = 'id';

	const KEY_title = 'title';

	public $arrayCol = array(self::KEY_id, self::KEY_title);
	public $arrayColType;


	function __construct()
	{ }


	public function CreateDb()
	{
		$this->OpenFile("items.db");
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

		/* 
        values[1] = info.intro;
        values[2] = info.album;
        Debug.Log("translation=" + info.translation);
        //values[3] = "成千上万匹马在奔跑腾跃。形容群众性的活动声势浩大或场面热烈。";
        //values[3] = "成千上万匹马在奔跑腾跃。形容群众性";//ng
        // values[3] = "性";//ng
        // values[3] = "性";//ng

        values[3] = "u6027";//\u6027

        values[4] = info.author;
        values[5] = info.year;
        values[6] = info.style;
       
        values[8] = info.appreciation;
        values[9] = info.head;
        values[10] = info.end;
        values[11] = info.tips;
        */
		$this->Insert2Table(self::TABLE_NAME, $values);
	}

	public function DeleteItem($info)
	{
		// string strsql = "DELETE FROM " + TABLE_NAME + " WHERE id = '" + info.id + "'" + " and addtime = '" + info.addtime + "'";
		$sql = "DELETE FROM " . self::TABLE_NAME . " WHERE id = '" . $info . id . "'";
		$result = $this->query($sql);
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
		$info = new CrazyImageItemInfo();
		$info->id =   $item[self::KEY_id];
		$info->title =   $item[self::KEY_title];
		echo "id=" . $info->id . "\n";
		echo "title=" . $info->title . "\n";
		return $info;
	}
}
