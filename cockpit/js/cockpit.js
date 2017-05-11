			$(document).ready(function (){
                var spotFormApiUrl = 'http://obokbararumapp-api.azurewebsites.net/api/spots' //'/api/spots/';	// API URL for updating/creating/dsiplaying spot recoords
				//var spotStatusFormApiUrl = '../spotstatusget.php';		 // API URL to retrieve spot statuses (Mockup ../spotstatusget.php)
                var spotStatusFormApiUrl = 'http://obokbararumapp-api.azurewebsites.net/api/spotstates/';		 // API URL to retrieve spot statuses (Mockup ../spotstatusget.php)
				
				$('.cform').cForm();  //Apply cForm layout
                $("#SpotStatusForm").on('submit', function () {
                    console.log("klickat till retrieve");
                   /* var name = $("#name").val();
                    var email = $("#email").val();
                    var password = $("#password").val();
                    var contact = $("#contact").val();

                    var dataString = 'name1=' + name + '&email1=' + email + '&password1=' + password + '&contact1=' + contact;
                    if (name == '' || email == '' || password == '' || contact == '') {
                        alert("Please Fill All Fields");
                    }
                    else {
                        // AJAX Code To Submit Form.
                        $.ajax({
                            type: "POST",
                            url: "ajaxsubmit.php",
                            data: dataString,
                            cache: false,
                            success: function (result) {
                                alert(result);
                            }
                        });
                    }
                    return false;*/
                });
				// Handle menu scroll target (exclude top header)
				$('header a').click(function (event) {
					console.log("header a: clicked");
					event.preventDefault();
					var anchor = $(this).attr('href').slice(1),
					offset = $('a[name="' + anchor + '"]').offset();
					$('html, body').animate({
						scrollTop: offset.top - 100
					});
				});
				
				var authToken = $('#AuthToken');
				
				console.log('Setting up forms...');
				
				// Spot form -  handle submit to backedn api
				////////////////////////////////////////////////////////////////////////////////////////////////
				var spotForm = $('#SpotForm');
				
				function SpotButtonClick(event){
					// Prevent default form action
					//event.preventDefault();
					
					// Get results textarea
					var taResults = $('#SpotResults');
					
					// Get button pressed
					var button = event.data.button;
					var postURL = spotFormApiUrl;

					taResults.val('Button pressed...');
					console.log('Action:' + event.data.msg);
					console.log('Action:' + JSON.stringify(event.data));
					
					// Compile Json to send.Only include fields with content
					var jsonOut = {},
						AuthTokenVal = $.trim(authToken.val()),
						SpotIDVal = $.trim($('#SpotForm #SpotID').val()),
						SpotNameVal = $.trim($('#SpotForm #SpotName').val()),
						SensorIDVal = $.trim($('#SpotForm #SensorID').val()),
						ImageSizeVal = $.trim($('#SpotForm #ImageSize').val()),
						XPosVal = $.trim($('#SpotForm #XPos').val()),
						YPosVal = $.trim($('#SpotForm #YPos').val());
					
					if(button=='update'){
						taResults.val('Update requested...');
						//postURL += SpotIDVal;
						if(AuthTokenVal != '') { jsonOut['AuthToken'] = AuthTokenVal; }
						if(SpotIDVal != '') { jsonOut['SpotID'] = SpotIDVal; }
						if(SpotNameVal != '') { jsonOut['SpotName'] = SpotNameVal; }
						if(SensorIDVal != '') { jsonOut['SensorID'] = SensorIDVal; }
						if(ImageSizeVal != '') { jsonOut['ImageType'] = ImageSizeVal; }
						if(XPosVal != '') { jsonOut['XPos'] = XPosVal; }
						if(YPosVal != '') { jsonOut['YPos'] = YPosVal; }
					}
					else { //Create
						taResults.val('Create requested...');
						jsonOut['AuthToken'] = AuthTokenVal;
						jsonOut['SpotID'] = SpotIDVal;
						jsonOut['SpotName'] = SpotNameVal;
						jsonOut['SensorID'] = SensorIDVal;
						jsonOut['ImageType'] = ImageSizeVal;
						jsonOut['XPos'] = XPosVal;
						jsonOut['YPos'] = YPosVal;
					}
					
					//console.log(JSON.stringify(jsonOut,null,2));
					
					if(AuthTokenVal !== '' && SpotIDVal !== ''){	// If required fields are filles in
						// Show progress
						prependWithSeparator(taResults,
							'Calling backend api...\n\n' +
							'Sending json ('+ postURL +'):\n' +
							JSON.stringify(jsonOut,null,2)
						);
						
						// Call api
						$.post(postURL, jsonOut, function(data){	// API call was successfull
								// API returned document
								if(typeof data.SpotID !== undefined && data.SpotID !== ""){
									// Success: There is a variable Result
									prependWithSeparator(taResults, 'Call was successful\n\nReceived json:\n' + JSON.stringify(data,null,2));
									
									// Empty selected fields
									$('#SpotForm #SpotID').val('');
									$('#SpotForm #SpotName').val('');
									$('#SpotForm #SensorID').val('');
									$('#SpotForm #ImageSize').val('');
									$('#SpotForm #XPos').val('');
									$('#SpotForm #YPos').val('');
								}
								else {  // Fail: API call successfull but no result variable
									prependWithSeparator(taResults, 'Call failed with "' + data.Result + '"\n\nReceived json:\n' + JSON.stringify(data,null,2));
									alert('Update failed! See Result textbox.');
								}
								
							}).error(function(data){  // Fail: Backend not reached
									prependWithSeparator(taResults, 'Call failed\n\n' + JSON.stringify(data,null,2));
									alert('Call failed! See Result textbox.');
							});
					}
					else {  // Fail: Missing required fields
						prependWithSeparator(taResults,'Missing Authorization token and/or Spot ID!');
						alert('Missing Authorization token and/or Spot ID!');
					}
				}
				//$('#SpotCreate').bind('click', { 'button': 'create' }, SpotButtonClick);
				$('#SpotUpdate').bind('click', { 'button': 'update' }, SpotButtonClick);
				
				// SpotStatus form -  Test retriving Spot statuses
				////////////////////////////////////////////////////////////////////////////////////////////////
				var spotStatusForm = $('#SpotStatusForm');
				spotStatusForm.submit(function(event){
					// Prevent default form action
					event.preventDefault();
					
					// Get results textarea
					var taResults = $('#SpotStatusResults');

					taResults.val('Button pressed...');
					
					
					// Compile Json to send
					var jsonOut = {
						'TS': $.trim($('#SpotStatusForm #TS').val())
					};
					
					// Show progress
					prependWithSeparator(taResults,
						'Calling backend api...\n\n' +
						'Sending json:\n' +
						JSON.stringify(jsonOut,null,2)
					);

                    jquery.ajax(objektMedsettings, funktionsomkallasvidsuccess, funktionsomkallasviderror);


						
					// Call api
                    $.ajax({
                        type: "GET",
                        url: spotStatusFormApiUrl,
                        dataType: "jsonp",
                        success: function (data) {
                            console.log("got data from get sotstatusformapiurl", data);
                             // $.post(spotStatusFormApiUrl, jsonOut, function (data) {	//  API call was successfull
						    // API returned document
						    if(typeof data.TS !== undefined){
							    // Success: There is a variable Result
							    prependWithSeparator(taResults, 'Retrieval was successful\n\nReceived json:\n' + JSON.stringify(data,null,2));
						    }
						    else {  // Fail: API call successfull but no result variable
							    prependWithSeparator(taResults, 'Retrieval failed with "' + data.Result + '"\n\nReceived json:\n' + JSON.stringify(data,null,2));
							    alert('Retrieval failed! See Result textbox.');
						    }
							
                        },
                        error: function(data) {  // Fail: Backend not reached
						    prependWithSeparator(taResults, 'Retrieval failed with "Backend not reached"\n\n' + JSON.stringify(data,null,2));
						    alert('Retrieval failed! See Result textbox.');
                        }
                    })
				});
				
								
				// Event Hub form -  Send message to event hub
				////////////////////////////////////////////////////////////////////////////////////////////////
				var EventHubForm = $('#EventHubForm');
				EventHubForm.submit(function(event){
					// Prevent default form action
					event.preventDefault();
					
					// Get results textarea
					var taResults = $('#EventHubResults');

					taResults.val('Button pressed...');
					
					// Compile Json to send
					var jsonOut = {
						'ID': $.trim($('#EventHubForm #SensorID').val()),
						'Status': $.trim($('#EventHubForm #SensorStatus').val()),
						'TS': $.trim($('#EventHubForm #TS').val()),
						'Change': $.trim($('#EventHubForm #Change').val()),
						'Ip': $.trim($('#EventHubForm #LastIp').val())
					};
					
					// Event Hub Settings
					var	eventHubNS = $.trim($('#EventHubForm #EventHubNS').val()),
						eventQueue = $.trim($('#EventHubForm #EventQueue').val()),
						sasKeyName = $.trim($('#EventHubForm #SasKeyName').val()),
						sasKey = $.trim($('#EventHubForm #SasKey').val());
					
					if(eventHubNS !== '' && eventQueue !== '' && sasKeyName !== ''	&& sasKey !== ''){	// If required fields are filles in
						// Show progress
						prependWithSeparator(taResults,
							'Sending message to Event Hub...\n\n' +
							'Event Hub Name Space: ' + eventHubNS + "\n" +
							'Event Hub Queue path: ' + eventQueue + "\n" +
							'Event Hub Key name: ' + sasKeyName + "\n" +
							'Event Hub Key: ' + sasKey + "\n" +
							'\nMessage body:\n' +
							JSON.stringify(jsonOut,null,2)
						);
						
						// Send Event
						var ehClient = new EventHubClient(
						{
							'name': eventQueue,
							//'devicename': 'XXXXXXXX', // This is by specification partition key.
							'namespace': eventHubNS,
							'sasKey': sasKey,
							'sasKeyName': sasKeyName,
							'timeOut': 10
						});
						
						var msg = new EventData(jsonOut);
						
						ehClient.sendMessage(msg, function (messagingResult) {
								prependWithSeparator(taResults, messagingResult.result + '\n\nOutput:\n' + JSON.stringify(messagingResult,null,2));
						});
					}
					else {  // Fail: Missing required fields
						prependWithSeparator(taResults,'Missing Event Hub Settings!');
						alert('Missing Event Hub Settings!');
					}
				});
				
				
				// Function to prepend result texts to textarea
				function prependWithSeparator(elObject, tText){
					console.log('prependWithSeparator: ' + tText);
					elObject.val(
						tText + "\n\n" +
						"------------------\n" +
						elObject.val()
					);
				}
			});