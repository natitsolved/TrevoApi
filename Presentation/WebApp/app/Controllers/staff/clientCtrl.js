'use strict';
/**
 * controllers used for the login
 */
app.controller('clientCtrl', function ($rootScope, $scope, $http,
                                        $location, myAuth,
                                        $cookieStore, $timeout,
                                        staffService, $state, $stateParams) {

    $scope.viewUser = "view";
    $scope.loggedinDetails = myAuth.getAdminAuthorisation();

    $scope.GetAllClients = function () {
        $("#default_loader").show();
        staffService.getAllClientListByStaffId($scope.loggedinDetails.id).then(
            function (data) {

                if (data.length > 0) {
                    $scope.clientList = data;
                    $timeout(function () {

                        $scope.table = angular.element('#clientList').DataTable({
                            "paging": true,
                            "lengthChange": false,
                            "searching": true,
                            "ordering": true,
                            "info": true,
                            "autoWidth": false,
                            "retrieve": true,
                            columnDefs: [{ targets: '_all', visible: true }
                            ]
                        });
                    }, 5000, false);

                }
                $("#default_loader").hide();
            },
            function (errorMessage) {
                $("#default_loader").hide();
                $rootScope.showToast(errorMessage, "alert alert-danger");
            });



    }
    $scope.GetAllClients();

    $scope.addUser = function () {
        $("#default_loader").show();
        $scope.UserKey = $stateParams.staffId;

        staffService.getUser($scope.UserKey).then(
                function (data) {
                    if (data != null || data != undefined) {
                        $scope.userdetail = data;
                        $scope.item = {
                            "UserName": $scope.userdetail.UserName,
                            "Email": $scope.userdetail.Email,
                            "Address": $scope.userdetail.Address,
                            "PhoneNo": $scope.userdetail.PhoneNo,
                            "City": $scope.userdetail.City,
                            "DateOfBirth": new Date($scope.userdetail.DateOfBirth),
                            "Gender": $scope.userdetail.Gender == "F" ? "Female" : "Male",
                            "FirstName": $scope.userdetail.FirstName,
                            "LastName": $scope.userdetail.LastName,
                            "State": $scope.userdetail.State,
                            "ZipCode": $scope.userdetail.ZipCode,
                        };
                        if ($scope.userdetail.ImagePath != null && $scope.userdetail.ImagePath != '') {
                            $scope.isShow = true;
                            $scope.image_source = $scope.userdetail.ImagePath;

                        }
                        else {
                            $scope.isShow = false;
                        }

                    }
                    $("#default_loader").hide();
                },
                function (errorMessage) {
                    $("#default_loader").hide();
                    $rootScope.showToast(errorMessage, "alert alert-danger");

                });


    }
    $scope.addUser();

   
    $scope.backFunction=function()
    {
        $state.go('staff.clientList', {}, { reload: true });
    }


});

