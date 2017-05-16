<?php
	//http://nitschinger.at/Handling-JSON-like-a-boss-in-PHP/
	/*
		{
			“TS”: “YYYYMMDDHHmmSS”,	// This timestamp is used by presentation in the next call.
			spots: [
					{
						"SpotID": "xxxxxxxxx",
						"SensorStatus": "O/F/U/B",
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
	$statusstates = array("F","O","U","B");
	
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
				"ImageType" => "1",
				"XPos" => "334",
				"YPos" => "230"
			),
			array(	// Fönen
				"SpotID" => "rumBygget",
				"SensorStatus" => $statusstates[array_rand($statusstates)],
				"ImageType" => "1",
				"XPos" => "407",
				"YPos" => "146"
			),
			array(	// Pumpen
				"SpotID" => "rumBygget",
				"SensorStatus" => $statusstates[array_rand($statusstates)],
				"ImageType" => "2",
				"XPos" => "486",
				"YPos" => "233"
			),
			array(	// Lobbyn
				"SpotID" => "rumBygget",
				"SensorStatus" => $statusstates[array_rand($statusstates)],
				"ImageType" => "2",
				"XPos" => "721",
				"YPos" => "160"
			),
			/*array(	// Däcket
				"SpotID" => "rumBygget",
				"SensorStatus" => $statusstates[array_rand($statusstates)],
				"ImageType" => "3",
				"XPos" => "1228",
				"YPos" => "156"
			),
			array(	// Sågen
				"SpotID" => "rumBygget",
				"SensorStatus" => $statusstates[array_rand($statusstates)],
				"ImageType" => "3",
				"XPos" => "329",
				"YPos" => "562"
			),*/
			array(	// Gaffeln
				"SpotID" => "rumBygget",
				"SensorStatus" => $statusstates[array_rand($statusstates)],
				"ImageType" => "4",
				"XPos" => "725",
				"YPos" => "565"
			),
			array(	// Sleven
				"SpotID" => "rumBygget",
				"SensorStatus" => $statusstates[array_rand($statusstates)],
				"ImageType" => "4",
				"XPos" => "725",
				"YPos" => "615"
			),
			array(	// Plåten
				"SpotID" => "rumBygget",
				"SensorStatus" => $statusstates[array_rand($statusstates)],
				"ImageType" => "1",
				"XPos" => "808",
				"YPos" => "557"
			),
			array(	// Vispen
				"SpotID" => "rumBygget",
				"SensorStatus" => $statusstates[array_rand($statusstates)],
				"ImageType" => "1",
				"XPos" => "794",
				"YPos" => "692"
			),
			array(	// Morteln
				"SpotID" => "rumBygget",
				"SensorStatus" => $statusstates[array_rand($statusstates)],
				"ImageType" => "1",
				"XPos" => "835",
				"YPos" => "693"
			),
			array(	// Kaveln
				"SpotID" => "rumBygget",
				"SensorStatus" => $statusstates[array_rand($statusstates)],
				"ImageType" => "1",
				"XPos" => "955",
				"YPos" => "612"
			)
			,
			array(	// rumSpritsen
				"SpotID" => "rumSpritsen",
				"SensorStatus" => $statusstates[array_rand($statusstates)],
				"ImageType" => "4",
				"XPos" => "1177",
				"YPos" => "696"
			)
			,
			array(
				"SpotID" => "plats249",
				"SensorStatus" => $statusstates[array_rand($statusstates)],
				"ImageType" => "0",
				"XPos" => "1396",
				"YPos" => "195"
			)
			,
			array(
				"SpotID" => "plats250",
				"SensorStatus" => $statusstates[array_rand($statusstates)],
				"ImageType" => "0",
				"XPos" => "1368",
				"YPos" => "195"
			)
			,
			array(
				"SpotID" => "plats251",
				"SensorStatus" => $statusstates[array_rand($statusstates)],
				"ImageType" => "0",
				"XPos" => "1340",
				"YPos" => "195"
			)
			,
			array(
				"SpotID" => "plats252",
				"SensorStatus" => $statusstates[array_rand($statusstates)],
				"ImageType" => "0",
				"XPos" => "1340",
				"YPos" => "215"
			)
			,
			array(
				"SpotID" => "plats253",
				"SensorStatus" => $statusstates[array_rand($statusstates)],
				"ImageType" => "0",
				"XPos" => "1368",
				"YPos" => "215"
			)
			,
			array(
				"SpotID" => "plats254",
				"SensorStatus" => $statusstates[array_rand($statusstates)],
				"ImageType" => "0",
				"XPos" => "1396",
				"YPos" => "215"
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
	header("Cache-Control: no-store, no-cache, must-revalidate, max-age=0");
	header("Cache-Control: post-check=0, pre-check=0", false);
	header("Pragma: no-cache");
    echo json_encode($datatojoson, JSON_PRETTY_PRINT+JSON_UNESCAPED_UNICODE); 
?>