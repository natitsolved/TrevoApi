'use strict';
/** 
 * controllers used for the register
 */

app.controller('chatCtrl', function ($rootScope, $scope, $http, $location, $stateParams, myAuth, $window, staffService) {

    // $scope.listOfEmoticons = [":smile:", "<3", ":(", ";)", ":=D", "8=)", ":=O", "(:|", ":=|", ":-*", ":P", ":^)"];


    var sinchClient = new SinchClient({
        applicationKey: $rootScope.sinchApplicationKey,
        applicationSecret: $rootScope.sinchAppSecret,
        capabilities: { messaging: true },
        startActiveConnection: true,
        //Note: For additional loging, please uncomment the three rows below
        onLogMessage: function (message) {
            console.log(message);
        }
    });

   






    var messageClient = sinchClient.getMessageClient();
    var sessionName = 'sinchSession-' + sinchClient.applicationKey;
    var audio = new Audio('http://166.62.40.135:8091/app/assets/staff/audio/incoming_message.mp3');
    $scope.init = function () {
        $scope.getEmoj();


        //if ($window.localStorage["senderChatContent"] != null && $window.localStorage["senderChatContent"] != undefined) {
        //    $scope.senderChatList = JSON.parse($window.localStorage["senderChatContent"]);
        //}
        //else {
        //    $scope.senderChatList = [];
        //}
        //if ($window.localStorage["recieverChatContent"] != null && $window.localStorage["recieverChatContent"] != undefined) {
        //    $scope.recieverChatList = JSON.parse($window.localStorage["recieverChatContent"]);
        //}
        //else {
        //    $scope.recieverChatList = [];
        //}

        $scope.loggedindetails = myAuth.getAdminAuthorisation();



        if ($window.localStorage["recipientId"] != undefined) {
            $rootScope.recipientId = JSON.parse($window.localStorage["recipientId"])
        }
        /*** Check for valid session. NOTE: Deactivated by default to allow multiple browser-tabs with different users. Remove "false &&" to activate session loading! ***/

        var sessionObj = JSON.parse(localStorage[sessionName] || '{}');
        if (false && sessionObj.userId) { //Remove "false &&"" to actually check start from a previous session!
            sinchClient.start(sessionObj)
                .then(function () {

                    localStorage[sessionName] = JSON.stringify(sinchClient.getSession());
                })
                .fail(function () {
                    //alert('fail');
                });
        }
        else {
            //alert('fail');
        }
        $scope.startSinch();


    }

    $scope.getEmoj = function () {
        staffService.getEmojiData().then(
             function (data) {
                 if (data) {
                     $scope.smileyEmojis = [];
                     $scope.aniFlowerEmojis = [];
                     for (var i = 0; i < data.emojisArray.length; i++) {
                         if (data.emojisArray[i].group == 'smiley') {
                             $scope.smileyEmojis.push(data.emojisArray[i]);
                         }
                         else if (data.emojisArray[i].group == 'aniFlower') {
                             $scope.aniFlowerEmojis.push(data.emojisArray[i]);
                         }
                     }

                 }

             },
             function (errorMessage) {
                 $("#default_loader").hide();
                 $rootScope.showToast("Sorry!!! There is some error.", "alert alert-danger");
             });
    }




    $scope.startSinch = function () {
        if (!$rootScope.isStarted) {
            $("#default_loader").show();
            var signUpObj = {};
            signUpObj.username = $scope.loggedindetails.email;
            signUpObj.password = "123456";
            sinchClient.newUser(signUpObj, function (ticket) {
                //On success, start the client
                sinchClient.start(ticket, function () {
                    $("#default_loader").hide();
                    // alert('user created and sich started.');
                    localStorage[sessionName] = JSON.stringify(sinchClient.getSession());
                    $rootScope.isStarted = true;
                }).fail(function (data) {
                    $("#default_loader").hide();
                    alert(data);
                });
            }).fail(function () {
                //  alert('user already exists.');
                var signInObj = {};
                signInObj.username = $scope.loggedindetails.email;
                signInObj.password = "123456";
                sinchClient.start(signInObj, function () {
                    localStorage[sessionName] = JSON.stringify(sinchClient.getSession());
                    $rootScope.isStarted = true;
                    $("#default_loader").hide();
                }).fail(function (data) {
                    $("#default_loader").hide();
                    alert(data);
                });



            });
        }

    }

    $scope.init();



    /*** Create user and start sinch for that user and save session in localStorage ***/
    $scope.sendMessage = function () {
        if ($scope.messageToBeSent != '') {
            $scope.isDisable = true;
            messageClient = sinchClient.getMessageClient();
            var sinchMessage = messageClient.newMessage($rootScope.recipientId, $scope.messageToBeSent);
            messageClient.send(sinchMessage).fail(handleError);
            // $scope.senderChatList = [];

            //$window.localStorage["senderChatContent"] = JSON.stringify($scope.senderChatList);
            $scope.getCurrentTime("sender");
            var data1 = {
                message: $scope.messageToBeSent,
                time: $scope.selfTime
            }
            $rootScope.senderChatList.push(data1);
            $scope.isDisable = false;
            var sqlstr = "";
            sqlstr = "INSERT INTO ChatTable VALUES (Null,'" + $scope.messageToBeSent + "','" + $scope.loggedindetails.email + "','" + $rootScope.recipientId + "','" + $scope.selfTime.toString() + "',1);";
            $rootScope.db.exec(sqlstr);
            $rootScope.db.save();
            var rowId = $rootScope.db.exec(" select Max (Id) from ChatTable");
            if ($rootScope.recentPatientList == undefined) {
                $rootScope.recentPatientList = [];
            }
            if ($window.localStorage["recipientName"] != undefined) {
                if ($window.localStorage["recipientName"] != $rootScope.recipientId) {
                    var data = {
                        Email: $rootScope.recipientId,
                        Date: $scope.selfTime,
                        Message: $scope.messageToBeSent,
                        IsRead: 1,
                        Id: rowId[0].values[0][0]
                    }
                    $window.localStorage["recipientName"] = $rootScope.recipientId;
                    $rootScope.recentPatientList.push(data);
                }
            }
            else {
                var data = {
                    Email: $rootScope.recipientId,
                    Date: $scope.selfTime,
                    Message: $scope.messageToBeSent,
                    IsRead: 1,
                    Id: rowId[0].values[0][0]
                }
                $window.localStorage["recipientName"] = $rootScope.recipientId;
                $rootScope.recentPatientList.push(data);
            }

            $scope.messageToBeSent = '';
            if ($scope.$root.$$phase != '$apply' && $scope.$root.$$phase != '$digest') {
                $scope.$apply();
            }
        }

    }

    $scope.sendMessageByEnter = function (event) {
        if (event.keyCode == 13) {
            if ($scope.messageToBeSent != '') {
                messageClient = sinchClient.getMessageClient();
                var sinchMessage = messageClient.newMessage($rootScope.recipientId, $scope.messageToBeSent);
                messageClient.send(sinchMessage).fail(handleError);
                // $scope.senderChatList = [];

                // $window.localStorage["senderChatContent"] = JSON.stringify($scope.senderChatList);
                $scope.getCurrentTime("sender");
                var data1 = {
                    message: $scope.messageToBeSent,
                    time: $scope.selfTime
                }
                $scope.senderChatList.push(data1);
                var sqlstr = "";
                sqlstr = "INSERT INTO ChatTable VALUES (Null,'" + $scope.messageToBeSent + "','" + $scope.loggedindetails.email + "','" + $rootScope.recipientId + "','" + $scope.selfTime.toString() + "',1);";
                $rootScope.db.run(sqlstr);
                $rootScope.db.save();
                var rowId = $rootScope.db.exec(" select Max (Id) from ChatTable");
                if ($rootScope.recentPatientList == undefined) {
                    $rootScope.recentPatientList = [];
                }
                if ($window.localStorage["recipientName"] != undefined) {

                    if ($window.localStorage["recipientName"] != $rootScope.recipientId) {
                        var data = {
                            Email: $rootScope.recipientId,
                            Date: $scope.selfTime,
                            Message: $scope.messageToBeSent,
                            IsRead: 1,
                            Id: rowId[0].values[0][0]
                        }
                        $window.localStorage["recipientName"] = $rootScope.recipientId;
                        $rootScope.recentPatientList.push(data);
                    }

                }
                else {
                    var data = {
                        Email: $rootScope.recipientId,
                        Date: $scope.selfTime,
                        Message: $scope.messageToBeSent,
                        IsRead: 1,
                        Id: rowId[0].values[0][0]
                    }
                    $window.localStorage["recipientName"] = $rootScope.recipientId;
                    $rootScope.recentPatientList.push(data);
                }
                $scope.messageToBeSent = '';
                if ($scope.$root.$$phase != '$apply' && $scope.$root.$$phase != '$digest') {
                    $scope.$apply();
                }
            }
        }
    }






    var eventListener = {
        onIncomingMessage: function (message) {
            if (message.senderId.toLowerCase() == $scope.loggedindetails.email.toLowerCase())
            { }
            else
            {
                audio.play();
                $scope.getCurrentTime("reciever");
                // $scope.recieverChatList = [];
                var isInsert = false;
                var isRead = 0;
                var recipeintName = '';
                if ($rootScope.recentPatientList == undefined) {
                    $rootScope.recentPatientList = [];
                }
                for (var i = 0; i < message.recipientIds.length; i++) {
                    if ($window.localStorage["recipientName"] != undefined) {
                        if ($window.localStorage["recipientName"] != message.recipientIds[i] && message.recipientIds[i] != $scope.loggedindetails.email) {
                            $window.localStorage["recipientName"] = message.recipientIds[i];
                            recipeintName = message.recipientIds[i];
                            if (message.recipientIds[i] == $scope.recipientId) {
                                var data1 = {
                                    message: message.textBody,
                                    time: $scope.otherTime
                                };
                                $rootScope.recieverChatList.push(data1);
                            }
                            if ($rootScope.recentPatientList != undefined) {
                                for (var i = 0; i < $rootScope.recentPatientList.length; i++) {
                                    if (message.recipientIds[i] != $rootScope.recentPatientList[i].Email && message.recipientIds[i] != $scope.loggedindetails.email) {
                                        isInsert = true;
                                        break;
                                    }
                                    else if (message.senderId != $scope.loggedindetails.email) {
                                        isInsert = true;
                                        break;
                                    }
                                }
                            }
                            else {
                                isInsert = true;
                            }
                        }
                        else {
                            if (message.recipientIds[i] == $rootScope.recipientId) {
                                var data1 = {
                                    message: message.textBody,
                                    time: $scope.otherTime
                                };
                                $rootScope.recieverChatList.push(data1);
                                recipeintName = message.recipientIds[i];
                            }
                        }


                    }
                    else {
                        if (message.recipientIds[i] != $scope.loggedindetails.email) {
                            recipeintName = message.recipientIds[i];
                            var isRead = 0;
                            $window.localStorage["recipientName"] = message.recipientIds[i];
                            if (message.recipientIds[i] == $rootScope.recipientId) {
                                var data1 = {
                                    message: message.textBody,
                                    time: $scope.otherTime
                                };
                                $rootScope.recieverChatList.push(data1);
                            }
                            isInsert = true;


                        }


                    }
                }
                var sqlstr = "";
                //if ($rootScope.isCameThroughRequest != undefined && $rootScope.isCameThroughRequest == true)
                //{
                //    sqlstr = "INSERT INTO ChatTable VALUES (Null,'" + message.textBody + "','" + recipeintName + "','" + $scope.loggedindetails.email + "','" + $scope.otherTime.toString() + "',0,1);";
                //}
                //else {
                //    sqlstr = "INSERT INTO ChatTable VALUES (Null,'" + message.textBody + "','" + recipeintName + "','" + $scope.loggedindetails.email + "','" + $scope.otherTime.toString() + "',0,0);";
                //    $rootScope.isCameThroughRequest = false;
                //}
                sqlstr = "INSERT INTO ChatTable VALUES (Null,'" + message.textBody + "','" + recipeintName + "','" + $scope.loggedindetails.email + "','" + $scope.otherTime.toString() + "',0);";
                if ($rootScope.selectedEmail != recipeintName)
                {
                    $rootScope.isCameThroughRequest = false;
                }
                $rootScope.selectedEmail = recipeintName;
                $rootScope.db.run(sqlstr);
                $rootScope.db.save();
                var rowId = $rootScope.db.exec(" select Max (Id) from ChatTable");
                if (isInsert) {
                    var data = {
                        Email: recipeintName,
                        Date: $scope.otherTime,
                        Message: message.textBody,
                        IsRead: isRead,
                        Id: rowId[0].values[0][0]
                    }
                    $rootScope.recentPatientList.push(data);
                }
                if ($scope.$root.$$phase != '$apply' && $scope.$root.$$phase != '$digest') {
                    $scope.$apply();
                }
                // $window.localStorage["recieverChatContent"] = JSON.stringify($scope.recieverChatList);
                audio.pause();
            }

        }
    }

    messageClient.addEventListener(eventListener);


    /*** Handle delivery receipts ***/

    var eventListenerDelivery = {
        onMessageDelivered: function (messageDeliveryInfo) {
            // alert('message delivered.');
        }
    }

    messageClient.addEventListener(eventListenerDelivery);
    var handleError = function (error) {
        alert(error);
    }


    $scope.getCurrentTime = function (user) {
        var today = new Date();
        var time = today.getHours() + ":" + today.getMinutes();
        if (user == "sender") {
            $scope.selfTime = time;

        }
        else {
            $scope.otherTime = time;

        }
    }


    $scope.toggleShow = function () {
        $scope.isShowEmoticon = !$scope.isShowEmoticon;
    }
    $scope.addEmojitToMsg = function (that) {

        if ($scope.messageToBeSent == undefined) {
            $scope.messageToBeSent = "";
        }
        //$scope.messageToBeSent = $scope.messageToBeSent + " " + that.emoji.class;
        $scope.messageToBeSent = $scope.messageToBeSent + " " + "<i class='"+that+"'></i>";
    }







});

