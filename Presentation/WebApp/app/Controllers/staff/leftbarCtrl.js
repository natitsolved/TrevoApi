'use strict';
/** 
 * controllers used for the login
 */
app.controller('leftbarCtrl', function ($rootScope, $scope, $http, $location, myAuth, $cookieStore, $stateParams) {
    if ($stateParams.changePassId = "change") {
        $scope.item = {
            "OldPassword": '',
            "NewPassword": '',
            "ConfirmPassword": '',
            "Email": ''
        };
    }

    $scope.changePassword = function () {
        $("#default_loader").show();
        if ($scope.item.NewPassword.toLowerCase() != $scope.item.ConfirmPassword.toLowerCase()) {
            $("#default_loader").hide();
            $rootScope.showToast("Confirm Password and New Password do not match.", "alert alert-danger");

        }
        else {
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

