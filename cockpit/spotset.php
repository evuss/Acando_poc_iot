<?php
	//http://nitschinger.at/Handling-JSON-like-a-boss-in-PHP/
	/*
		{
			“TS”: “YYYYMMDDHHmmSS”,	// This timestamp is used by presentation in the next call.
			spots: [
					{
						"SpotID": "xxxxxxxxx",
						"SensorStatus": "O/F/U",
						“ImageSize”: “0,1,…”,
						“XPos”: “123”,
						“YPos”: “321”
					},
					{
						...
					}
			]
		}
	*/
	
    //$arr = array ('a'=>1,'b'=>2,'c'=>3,'d'=>4,'e'=>5);
	
	$datatojoson = array(
		"Result" => "success",
		"SpotID" => "rumBåten",
		"SpotName" => "Båten, obokningsbart rum",
		"SensorID" => "B4-AF-2B-AB-3B-E2",
		"ImageSize" => "0",
		"XPos" => "10",
		"YPos" => "10"
	);
	
	if($_POST['SpotID'] != null && $_POST['SpotID'] != ''){
		$datatojoson['SpotID'] = $_POST['SpotID'];
	}
	if($_POST['SpotName'] != null && $_POST['SpotName'] != ''){
		$datatojoson['SpotName'] = $_POST['SpotName'];
	}
	if($_POST['SensorID'] != null && $_POST['SensorID'] != ''){
		$datatojoson['SensorID'] = $_POST['SensorID'];
	}
	if($_POST['ImageSize'] != null && $_POST['ImageSize'] != ''){
		$datatojoson['ImageSize'] = $_POST['ImageSize'];
	}
	if($_POST['XPos'] != null && $_POST['XPos'] != ''){
		$datatojoson['XPos'] = $_POST['XPos'];
	}
	if($_POST['YPos'] != null && $_POST['YPos'] !== ''){
		$datatojoson['YPos'] = $_POST['YPos'];
	}

	header('Content-type: application/json; charset=utf-8');
    echo json_encode($datatojoson, JSON_PRETTY_PRINT+JSON_UNESCAPED_UNICODE); 
?>