'use strict';
/** 
 * controllers used for the login
 */
app.controller('headerCtrl', function ($rootScope, $scope, $http, $location, myAuth, $cookieStore, $state, ngDialog, $window) {
    $scope.loggedindetails = myAuth.getAdminAuthorisation();


    $scope.userLogout = function () {
        $http({
            method: "POST",
            url: $rootScope.serviceurl + "Logout",
            data: { 'Id': $scope.loggedindetails.id },
            headers: { 'Content-Type': 'application/json' },
        }).success(function (data) {
            myAuth.resetAdminUserinfo();
            $scope.loggedindetails = '';
            $scope.loggedin = false;
            $scope.notloggedin = true;
            $location.path("/adminlogin/signin");

        });


    };
    $scope.SignOut = function () {
        myAuth.resetAdminUserinfo();
        $window.localStorage["appTitle"] = "StatMedClinic";
        $state.go('adminlogin', {}, { reload: true });
    };

    $scope.goToChangePassword = function () {
        $state.go('staff.changePassword', { changePassId: "change" });
    }
    SQL.PersistentDatabase = function (name, success, failure) {
        var adapter, data = window.localStorage.getItem(name), result = null;
        if (data !== null) { // Fetch existing database from localStorage
            result = [];
            for (var i = 0, size = data.length; i < size; i++)
                result.push(data.charCodeAt(i));
            result = new Uint8Array(result);
        }
        try {
            adapter = new SQL.Database(result);


            adapter.run('SELECT name FROM sqlite_master');


            adapter.save = function () {
                var result = adapter.export(), strings = [], chunksize = 0xffff;
                for (var i = 0; i * chunksize < result.length; i++)
                    strings.push(String.fromCharCode.apply(null, result.subarray(i * chunksize, (i + 1) * chunksize)));
                window.localStorage.setItem(name, strings.join(''));
            };
        } catch (e) {
            // Callback executed for when life happens
            if (failure !== undefined) failure(e);
            return null;
        }
        // Callback executed when the database is initially created
        if ((result === null) && (success !== undefined)) success(adapter);

        return adapter;
    }
    $rootScope.initDatabase = function () {
        $rootScope.db = new SQL.PersistentDatabase('myDatabase',
 function (sender) { // Initial creation of database if not found
     sender.run('CREATE TABLE if not exists ChatTable (Id INTEGER  PRIMARY KEY , message TEXT NOT NULL, sender TEXT NOT NULL, reciever TEXT NOT NULL, date TEXT NULL,IsRead INTEGER NOT NULL)');
     //sender.run('INSERT INTO friends VALUES (NULL, ?)', ['Gerhard Stander']);
     sender.save();
 },
 function (e) { // Initialization of existing database failed
     alert(e);
 }
);

        var res = "SELECT name FROM sqlite_master WHERE name='ChatTable'"
        var isExist = $rootScope.db.exec(res);
        $rootScope.recieverChatList = [];
        $rootScope.senderChatList = [];
        if (isExist.length > 0) {
            var tableData = $rootScope.db.exec("SELECT * FROM ChatTable  GROUP BY sender Order By date desc Limit 1;");
            if (tableData.length > 0) {
                $rootScope.recentPatientList = [];
                for (var i = 0; i < tableData[0].values.length; i++) {
                    if (tableData[0].values[i][2] == $scope.loggedindetails.email) {
                        var data = {
                            Email: tableData[0].values[i][3],
                            Date: tableData[0].values[i][4],
                            Message: tableData[0].values[i][1],
                            IsRead: tableData[0].values[i][5],
                            Id: tableData[0].values[i][0]
                        }

                    }
                    else {
                        var data = {
                            Email: tableData[0].values[i][2],
                            Date: tableData[0].values[i][4],
                            Message: tableData[0].values[i][1],
                            IsRead: tableData[0].values[i][5],
                            Id: tableData[0].values[i][0]
                        }
                    }
                    $rootScope.recentPatientList.push(data);
                    //if ($scope.recipientId) {
                    //    if (tableData[0].values[i][2] == $scope.loggedindetails.email) {
                    //        $rootScope.senderChatList.push(tableData[0].values[i][1]);
                    //    }
                    //    else {
                    //        $rootScope.recieverChatList.push(tableData[0].values[i][1]);
                    //    }
                    //}
                }

            }

        }
    }

    $rootScope.initDatabase();

    $rootScope.makeReipient = function (email, id) {
        $window.localStorage["recipientId"] = JSON.stringify(email);
        $window.localStorage["recipientName"] = email;
        $rootScope.recipientId = JSON.parse($window.localStorage["recipientId"]);
        if (email == $rootScope.selectedEmail && $rootScope.isCameThroughRequest==true)
        { }
        else
        {
            $rootScope.isCameThroughRequest = false;
            $rootScope.selectedEmail = email;
            var sqlStr = "Select *  From  ChatTable where sender= '" + $scope.loggedindetails.email + "' and reciever='" + email + "';";
            var results = $rootScope.db.exec(sqlStr);
            $rootScope.senderChatList = [];
            $rootScope.recieverChatList = [];
            if (results.length > 0) {
                for (var i = 0; i < results[0].values.length; i++) {
                    var data = {
                        message: results[0].values[i][1],
                        time: results[0].values[i][4]
                    }
                    $rootScope.senderChatList.push(data);
                }
            }
            sqlStr = "Select *  From  ChatTable where sender= '" + email + "' and reciever='" + $scope.loggedindetails.email + "';";
            results = $rootScope.db.exec(sqlStr);
            if (results.length > 0) {
                for (var i = 0; i < results[0].values.length; i++) {


                    var data = {
                        message: results[0].values[i][1],
                        time: results[0].values[i][4]
                    }
                    $rootScope.recieverChatList.push(data);

                }
            }
        }
        

        sqlStr = "Select * from ChatTable where Id=" + id + ";";
        results = $rootScope.db.exec(sqlStr);
        if (results.length > 0 && results[0].values[0][5] == 0) {
            sqlStr = "Update ChatTable set IsRead=1 where Id=" + id + ";";
            $rootScope.db.exec(sqlStr);
            $rootScope.db.save();

            for (var i = 0; i < $rootScope.recentPatientList.length; i++) {
                if ($rootScope.recentPatientList[i].Id == id) {
                    $rootScope.recentPatientList[i].IsRead = 1;
                    if ($rootScope.$root.$$phase != '$apply' && $rootScope.$root.$$phase != '$digest') {
                        $rootScope.$apply();
                    }
                    break;
                }
            }
        }

        $state.go('staff.chat');
    }


});

