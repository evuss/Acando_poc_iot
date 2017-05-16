<?php
header('Content-type: application/json; charset=utf-8');
echo("{
  'TS': '20160622141334',
  'Result': 'Api reached',
  'SpotStates': [
    {
      'SpotID': 'testRumBåten',
      'SensorStatus': 'F',
      'ImageType': '0',
      'XPos': '100',
      'YPos': '100'
    },
    {
      'SpotID': 'testRumKammen',
      'SensorStatus': 'O',
      'ImageType': '0',
      'XPos': '300',
      'YPos': '100'
    },
    {
      'SpotID': 'testRumSaxen',
      'SensorStatus': 'U',
      'ImageType': '0',
      'XPos': '500',
      'YPos': '100'
    },
    {
      'SpotID': 'rumDäcket',
      'SensorStatus': 'B',
      'ImageType': '0',
      'XPos': '700',
      'YPos': '100'
    },
    {
      'SpotID': 'testRumDäcket',
      'SensorStatus': 'F',
      'ImageType': '1',
      'XPos': '1228',
      'YPos': '156'
    },
    {
      'SpotID': 'testRumLobbyn',
      'SensorStatus': 'O',
      'ImageType': '1',
      'XPos': '721',
      'YPos': '160'
    },
    {
      'SpotID': 'testRumPumpen',
      'SensorStatus': 'U',
      'ImageType': '1',
      'XPos': '486',
      'YPos': '233'
    },
    {
      'SpotID': 'testRumFönen',
      'SensorStatus': 'B',
      'ImageType': '1',
      'XPos': '407',
      'YPos': '146'
    }
  ]
}");
?>