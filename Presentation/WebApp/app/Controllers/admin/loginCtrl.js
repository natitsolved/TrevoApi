'use strict';
/** 
 * controllers used for the login
 */
app.controller('loginCtrl', function ($rootScope, $scope, $http, $location, myAuth, $cookieStore,$base64,$state,$stateParams,$window) {


   /*myAuth.updateAdminUserinfo(myAuth.getAdminAuthorisation());
    $scope.loggedindetails = myAuth.getAdminNavlinks();
    console.log($scope.loggedindetails);
    if($scope.loggedindetails){
        $location.path('/admin/home');
    }*/
    if ($stateParams.changePassId = "change")
    {
        $scope.item = {
            "OldPassword": '',
            "NewPassword": '',
            "ConfirmPassword": '',
            "Email":''
        };
    }
    $scope.getAdminLogin = function () {
        $("#default_loader").show();
    if($scope.password !=undefined) {
        $scope.Encodedpassword = $base64.encode($scope.password);
    }
    $http({
        method: "POST",
        url: $rootScope.serviceurl+"Login",
        data: {"UserCredential":$scope.userCredential,"Password":$scope.Encodedpassword},
        headers: { 'Content-Type': 'application/json' },
       
    }).success(function(data) {
        if (data.ResponseCode == 'Success') {
            $("#default_loader").hide();
            $scope.loggedindetails = '';
            console.log(data);
            $scope.userCredential = '';
            $scope.password = '';
            myAuth.updateAdminUserinfo(data);
            $scope.loggedin = true;
            $scope.notloggedin = false;
            if (data.RoleName.toLowerCase() == 'admin') {
                $rootScope.appTitle = "Admin";
                $state.go('admin.index', {}, { reload: true });
            }
            else if (data.RoleName.toLowerCase() == 'staff')
            {
                $rootScope.appTitle = "Staff";
                $state.go('staff.index', {}, { reload: true });
            }

        } else {
            $("#default_loader").hide();
            $rootScope.showToast(data.Message, "alert alert-danger");

        }

    }).error(function (data) {
        $("#default_loader").hide();
        $rootScope.showToast( data.Message,"alert alert-danger");
        
    });

   

};

    $scope.changePassword = function ()
    {
        $("#default_loader").show();
        if ($scope.item.NewPassword.toLowerCase() != $scope.item.ConfirmPassword.toLowerCase())
        {
            $("#default_loader").hide();
            $rootScope.showToast("Confirm Password and New Password do not match.", "alert alert-danger");
           
        }
        else
        {
            $scope.loggedindetails = myAuth.getAdminAuthorisation();
            if ($scope.loggedindetails) {
                $scope.item.Email = $scope.loggedindetails.email;
            }
            $http({
                method: "POST",
                url: $rootScope.serviceurl + "ChangePassword",
                data: $scope.item,
                headers: { 'Content-Type': 'application/json' },

            }).success(function (data) {
                if (data) {
                    $("#default_loader").hide();
                    $state.go('adminlogin', {}, { reload: true });

                } else {
                    $("#default_loader").hide();

                }

            }).error(function (data) {
                $("#default_loader").hide();
                $rootScope.showToast(data.Message, "alert alert-danger");
            });
        }
    }
});

