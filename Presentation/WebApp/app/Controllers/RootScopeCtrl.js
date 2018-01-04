app.controller("RootScopeCtrl", function ($scope, myAuth, $location, ngToast, $rootScope, ngDialog, $http, $state, $window) {
    $scope.loggedindetails = myAuth.getAdminAuthorisation();

    if ($scope.loggedindetails) {
        if ($scope.loggedindetails.roleName.toLowerCase() == 'admin') {
            $location.path("/admin/home");
        }
        else if ($scope.loggedindetails.roleName.toLowerCase() == 'staff') {
            $location.path("/staff/home");
        }
    }
    else {
        $location.path("/admin/adminlogin");
    }
    var products = [{
        key: "Televisions",
        items: [
            { text: "Login", price: "$1200", src: "images/products/7.png" },
            { text: "Register", price: "$1450", src: "images/products/5.png" },

        ]
    }

    ];
    $scope.data1 = products;
    $scope.menuVisible = false;
    $scope.swipeValue = true;

    $scope.showMenu = function () {
        $scope.menuVisible = !$scope.menuVisible;
    };

    $scope.checkList = function () {
        //alert(12);
    };

    $scope.toolbarItems = [{
        location: "before",
        widget: "button",
        options: {
            icon: "menu",
            onClick: $scope.showMenu
        }
    }, {
        location: "center",
        template: "title"
    }];

    $rootScope.showToast = function (message, className) {
        ngToast.create({
            className: className,
            content: message,
            dismissButton: true,
            timeout: 2000,
            dismissOnTimeout: true
        });

    }


    $rootScope.dismissToast = function () {
        ngToast.dismiss();
    }

    socket = new io.connect('http://166.62.40.135:8080');
    //socket.connect();
    console.log(socket);
    if ($scope.loggedindetails) {
        socket.emit('new_contact_from_patient', 'e3e177a2-b615-408c-9dd5-2f6948ffa482');
    }


    socket.on('contact_recieved_by_staff', function (data) {


        if ($scope.loggedindetails)
        { }
        else
        {
            $scope.loggedindetails = myAuth.getAdminAuthorisation();
        }
        if ($scope.loggedindetails.roleName.toLowerCase() == 'staff') {
            localStorage.setItem("patientKey", JSON.stringify(data));
            //localStorage.setItem("recipientId", JSON.stringify('test@gmail.com'));
            localStorage.setItem("recipientId", JSON.stringify('staff3@test.com'));
            $scope.clickToOpen();
            //$scope.insertPatientContactDetails(data);


        }
        //audio.pause();
    });
    $rootScope.ngDialog = ngDialog;
    $scope.clickToOpen = function () {
        ngDialog.open({ template: 'templateId' });
    };
    $scope.getQuestionAnsBasedOnUser = function (userKey) {
        $http({
            method: "POST",
            url: $rootScope.serviceurl + "GetQuestionAnsBasedOnUser",
            headers: { 'Content-Type': 'application/json' },
            data: { Id: userKey },
        }).success(function (data) {

            console.log(data);
            $rootScope.patientAnswerDetails = data;
            $rootScope.isCameThroughRequest = true;
            $state.go('staff.chat', {}, {reload:true});

        });
    }

    $scope.insertPatientContactDetails = function (userKey) {

        $http({
            method: "POST",
            url: $rootScope.serviceurl + "InsertPatientContactDetails",
            headers: { 'Content-Type': 'application/json' },
            data: { PatientKey: userKey },
        }).success(function (data) {

            $scope.getUserDetails(userKey);

        });
    }
    $rootScope.requestAccepted = function () {

        socket.emit('request_accepted_by_staff', $scope.loggedindetails.email);
       
        ngDialog.close();
        //$state.go('staff.chat');
        $scope.updatePatientContactDetails();
    }

    $scope.getUserDetails = function (userKey) {
        $http({
            method: "POST",
            url: $rootScope.serviceurl + "GetUserDetailsById",
            headers: { 'Content-Type': 'application/json' },
            data: { Id: userKey },
        }).success(function (data) {
            console.log(data);
            $rootScope.dataDynamic = data.FirstName.toUpperCase();
            notifyMe($rootScope.dataDynamic);
            $rootScope.imgSrc = data.ImagePath;
            localStorage.setItem("recipientId", JSON.stringify(data.Email));
            var audio = new Audio('http://localhost:60873/app/assets/staff/audio/notification.mp3');
            audio.play();
            $scope.clickToOpen();

        });
    }
    $scope.updatePatientContactDetails = function () {
        if ($window.localStorage["patientKey"] != undefined) {
            var patientKey = JSON.parse($window.localStorage["patientKey"]);
            $http({
                method: "POST",
                url: $rootScope.serviceurl + "UpdatePatientContactDetails",
                headers: { 'Content-Type': 'application/json' },
                data: { PatientKey: patientKey, StaffKey: $scope.loggedindetails.id },
            }).success(function (data) {
                console.log(data);
                ngDialog.close();
                $scope.getQuestionAnsBasedOnUser(patientKey);
                //$state.go('staff.chat');
            });
        }

    }


    function notifyMe(patientName) {
        if (Notification.permission !== "granted")
            Notification.requestPermission();
        else {
            var notification = new Notification('New Request From Patient', {
                icon: 'http://localhost:60873/app/assets/Images/logo.png',
                body: "New Request from patient" + patientName,
            });

            notification.onclick = function () {
                window.open("http://166.62.40.135:8091/#/staff/home");
            };

        }

    }

    
    if ($window.localStorage["appTitle"] != undefined && $window.localStorage["appTitle"] != null) {
        $rootScope.appTitle = $window.localStorage["appTitle"];

    }
    else {
        $rootScope.appTitle = "StatMedClinic";
    }
    
   
    //$rootScope.getAllPatients = function () {
    //    $http({
    //        method: "GET",
    //        url: $rootScope.serviceurl + "GetAllUserWithRoleAndImage/" + 'patient',
    //    }).success(function (data) {
    //        $rootScope.recentPatientList = data;

    //    });
    //}




});
