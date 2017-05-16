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
	$statusstates = array("F","O","U");
	
	$datatojoson = array(
		"TS" => "YYYYMMDDHHmmSS",
		"SpotStates" => array(
			array(	// Nollstället
				"SpotID" => "rumBåten",
				"SensorStatus" => $statusstates[array_rand($statusstates)],
				"ImageType" => "0",
				"XPos" => "114",
				"YPos" => "228"
			),
			array(	// Kammen
				"SpotID" => "rumBygget",
				"SensorStatus" => $statusstates[array_rand($statusstates)],
				"ImageType" => "0",
				"XPos" => "334",
				"YPos" => "198"
			)
			,
			array(	// Saxen
				"SpotID" => "rumBygget",
				"SensorStatus" => $statusstates[array_rand($statusstates)],
				"ImageType" => "0",
				"XPos" => "334",
				"YPos" => "230"
			),
			array(	// Fönen
				"SpotID" => "rumBygget",
				"SensorStatus" => $statusstates[array_rand($statusstates)],
				"ImageType" => "0",
				"XPos" => "407",
				"YPos" => "146"
			),
			array(	// Pumpen
				"SpotID" => "rumBygget",
				"SensorStatus" => $statusstates[array_rand($statusstates)],
				"ImageType" => "0",
				"XPos" => "486",
				"YPos" => "233"
			),
			array(	// Lobbyn
				"SpotID" => "rumBygget",
				"SensorStatus" => $statusstates[array_rand($statusstates)],
				"ImageType" => "0",
				"XPos" => "721",
				"YPos" => "160"
			),
			array(	// Däcket
				"SpotID" => "rumBygget",
				"SensorStatus" => $statusstates[array_rand($statusstates)],
				"ImageType" => "0",
				"XPos" => "1228",
				"YPos" => "156"
			),
			array(	// Sågen
				"SpotID" => "rumBygget",
				"SensorStatus" => $statusstates[array_rand($statusstates)],
				"ImageType" => "0",
				"XPos" => "329",
				"YPos" => "562"
			),
			array(	// Gaffeln
				"SpotID" => "rumBygget",
				"SensorStatus" => $statusstates[array_rand($statusstates)],
				"ImageType" => "0",
				"XPos" => "725",
				"YPos" => "565"
			),
			array(	// Sleven
				"SpotID" => "rumBygget",
				"SensorStatus" => $statusstates[array_rand($statusstates)],
				"ImageType" => "0",
				"XPos" => "725",
				"YPos" => "615"
			),
			array(	// Plåten
				"SpotID" => "rumBygget",
				"SensorStatus" => $statusstates[array_rand($statusstates)],
				"ImageType" => "0",
				"XPos" => "808",
				"YPos" => "557"
			),
			array(	// Vispen
				"SpotID" => "rumBygget",
				"SensorStatus" => $statusstates[array_rand($statusstates)],
				"ImageType" => "0",
				"XPos" => "794",
				"YPos" => "692"
			),
			array(	// Morteln
				"SpotID" => "rumBygget",
				"SensorStatus" => $statusstates[array_rand($statusstates)],
				"ImageType" => "0",
				"XPos" => "835",
				"YPos" => "693"
			),
			array(	// Kaveln
				"SpotID" => "rumBygget",
				"SensorStatus" => $statusstates[array_rand($statusstates)],
				"ImageType" => "0",
				"XPos" => "955",
				"YPos" => "612"
			)
			,
			array(	// Kaveln
				"SpotID" => "rumBygget",
				"SensorStatus" => $statusstates[array_rand($statusstates)],
				"ImageType" => "0",
				"XPos" => "1141",
				"YPos" => "691"
			)
		)
	);
	
	// Read JSon input
	$TS = $_POST['TS'];
	if($TS == null){
		$TS = "00000000000000";
	}
	
	$datatojoson["TS"] = $TS;

	header('Content-type: application/json; charset=utf-8');
    echo json_encode($datatojoson, JSON_PRETTY_PRINT+JSON_UNESCAPED_UNICODE); 
?>