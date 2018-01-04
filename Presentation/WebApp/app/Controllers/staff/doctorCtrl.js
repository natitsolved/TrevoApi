'use strict';
/**
 * controllers used for the login
 */
app.controller('doctorCtrl', function ($rootScope, $scope, $http,
                                        $location, myAuth,
                                        $cookieStore, $timeout,
                                        staffService, $state, $stateParams) {

    $scope.viewUser = "view";
    $scope.roleName = "Doctor";

    $scope.GetAllDoctor = function () {
        $("#default_loader").show();
        staffService.getAllUsersWithRole($scope.roleName).then(
            function (data) {

                if (data.length > 0) {
                    $scope.doctorList = data;
                    $timeout(function () {

                        $scope.table = angular.element('#doctorList').DataTable({
                            "paging": true,
                            "lengthChange": false,
                            "searching": true,
                            "ordering": true,
                            "info": true,
                            "autoWidth": false,
                            "retrieve": true,
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
    $scope.GetAllDoctor();



    $scope.addUser = function () {
        $("#default_loader").show();
        $scope.docId = $stateParams.docId;
        staffService.getUser($scope.docId).then(
                function (data) {
                    if (data != null || data != undefined) {
                        $scope.userdetail = data;
                        staffService.getDoctorDetails(data.UserKey).then(

                            function (data) {
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
                                    "Description": data == null ? '' : data.Description,
                                    "Speciality": data == null ? '' : data.Speciality,
                                };
                                if ($scope.userdetail.ImagePath != null && $scope.userdetail.ImagePath != '') {
                                    $scope.isShow = true;
                                    $scope.image_source = $scope.userdetail.ImagePath;

                                }
                                else {
                                    $scope.isShow = false;
                                }
                            },
                            function (errorMessage) {
                                $("#default_loader").hide();
                                $rootScope.showToast(errorMessage, "alert alert-danger");

                            });

                    }
                    $("#default_loader").hide();
                },
                function (errorMessage) {
                    $("#default_loader").hide();
                    $rootScope.showToast(errorMessage, "alert alert-danger");

                });
     

    }
    $scope.addUser();

    $scope.backFunction = function ()
    {
        $state.go('staff.doctorList', {}, { reload: true });
    }
   
});

