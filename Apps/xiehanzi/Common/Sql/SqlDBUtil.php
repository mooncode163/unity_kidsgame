<?php
// class SQLiteDB extends SQLite3 {
// 	function __construct(){
// 		try {
// 			$this->open(dirname(__FILE__).'/../data/sqlite_ecloud.db');
// 		}catch (Exception $e){
// 			die($e->getMessage());
// 		}
// 	}
// }

class SqlDBUtil extends SQLite3
{
	function __construct()
	{ }
	private static $db;
	private static function instance()
	{
		if (!self::$db) {
			self::$db = new SQLiteDB();
		}
	}
	public function OpenFile($filepath)
	{
		try {
			$this->open($filepath);
		} catch (Exception $e) {
			die($e->getMessage());
		}
	}
	public function CloseDB()
	{
		$this->close();
	}

	/**
	 * 创建表
	 * @param string $sql
	 */
	public function CreateTable($sql)
	{
		// $result = @self::$db->query($sql);
		// if ($result) {
		// 	return true;
		// }
		$ret = $this->exec($sql);
		if (!$ret) {
			echo $this->lastErrorMsg();
		} else {
			echo "Table created successfully\n";
			return true;
		}

		return false;
	}
	public function CreateTableByName($name, $col, $colType) //创建表
	{
		if ($this->IsExsitTable($name)) {
			echo "Table Exsit  name= " . $name . "\n";
			return;
		}
		$sql = "CREATE TABLE " . $name . " (" . $col[0] . " " . $colType[0];
		for ($i = 1; $i < count($col); $i++) {
			$sql .=   ", " . $col[$i] . " " . $colType[$i];
		}
		$sql .= ")";
		$ret = $this->exec($sql);
		if (!$ret) {
			echo $this->lastErrorMsg();
		} else {
			echo "Table created successfully\n";
		}
	}

	public function IsExsitTable($name) //创建表
	{
		$ret = false;
		$sql = "select * from sqlite_master where type='table' and name = '" . $name . "'";
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
	public function IsItemExistBase($info)
	{
		return false;
	}

	/**
	 * 执行增删改操作
	 * @param string $sql
	 */
	public function Execute($sql)
	{
		$result =  $this->exec($sql);
		if ($result) {
			return true;
		}
		return false;
	}
	public function ReadBase($sql)
	{
		$result = $this->query($sql);
		foreach ($result as $row) {
			echo $row[0] . "\n";
		}
	}

	//在表中插入数据
	public function Insert2Table($tableName, $values)

	{
		$sql = "INSERT INTO " . $tableName .  " VALUES('" . $values[0];
		for ($i = 1; $i < count($values); $i++) {
			$sql .= "','" . $values[$i];
		}

		$sql .= "')";
		echo "Insert2Table sql=" . $sql . "\n";
		$result = $this->query($sql);
	}

	public function DeleteTable($tableName) //删除表
	{
		$sql = "DELETE FROM " . $tableName;
		$result =  $this->exec($sql);
	}

	// /**
	//  * 获取记录条数
	//  * @param string $sql
	//  * @return int
	//  */
	// public static function count($sql)
	// {
	// 	self::instance();
	// 	$result = @self::$db->querySingle($sql);
	// 	return $result ? $result : 0;
	// }

	// /**
	//  * 查询单个字段
	//  * @param string $sql
	//  * @return void|string
	//  */
	// public static function querySingle($sql)
	// {
	// 	self::instance();
	// 	$result = @self::$db->querySingle($sql);
	// 	return $result ? $result : '';
	// }

	// /**
	//  * 查询单条记录
	//  * @param string $sql
	//  * @return array
	//  */
	// public static function queryRow($sql)
	// {
	// 	self::instance();
	// 	$result = @self::$db->querySingle($sql, true);
	// 	return $result;
	// }

	// /**
	//  * 查询多条记录
	//  * @param string $sql
	//  * @return array
	//  */
	// public static function queryList($sql)
	// {
	// 	self::instance();
	// 	$result = array();
	// 	$ret = @self::$db->query($sql);
	// 	if (!$ret) {
	// 		return $result;
	// 	}
	// 	while ($row = $ret->fetchArray(SQLITE3_ASSOC)) {
	// 		array_push($result, $row);
	// 	}
	// 	return $result;
	// }


}
