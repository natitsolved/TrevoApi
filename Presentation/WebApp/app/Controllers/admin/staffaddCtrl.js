'use strict';
/**
 * controllers used for the login
 */
app.controller('staffaddCtrl', function ($rootScope, $scope, $http,$filter, $location, myAuth, $cookieStore,$timeout,userService,userRoleService,$stateParams) {


    $scope.viewUser = "view";
    $scope.roleName="Staff";




    $scope.deleteUser = function (c_id) {
        //alert(c_id);
        if (window.confirm("Want to delete?")) {
            $("#default_loader").show();
            $http({
                method: "DELETE",
                url: $rootScope.serviceurl + "deleteUser/"+c_id,
                //data: {"name": $scope.item.name,"is_active": $scope.item.is_active},
                //headers: {'Content-Type': 'application/json'},
            }).success(function (data) {
                $("#default_loader").hide();
                console.log(data);
                $scope.viewUser();
                //$scope.allcat = data.category;
                //console.log($scope.allcat);
            });
        }else{

        }

    }

    $scope.backFunction=function () {
        $location.path('/admin/staffList');
    }
    $scope.addUser = function () {
        //alert(13);
        $("#default_loader").show();
        userRoleService.getAllUserRole().then(
            function (data) {
                //console.log(data);

                if (data.length>0) {
                    for (var i=0;i<data.length;i++)
                    {

                        if(data[i].Name.toLowerCase()==$scope.roleName.toLocaleLowerCase())
                        {

                            $scope.allroleusers = data[i];
                        }
                    }

                    //console.log($scope.allroleusers);
                }
            },
            function (errorMessage) {
                //messageService.FlashMessage("danger", errorMessage);
                $("#default_loader").hide();
                $scope.allroleusers = "";
                $rootScope.showToast(errorMessage, "alert alert-danger");
            });
        if ($stateParams.userId == "add") {
            $scope.IsPassword=true;
            $scope.item = {
                "UserId": '',
                "UserName": '',
                "Email": '',
                "userrole": '',
                "Password": '',
                "ConfirmPassword":'',
                "Address": '',
                "PhoneNo": '',
                "isactive": true,
                "City":'',
                "DateOfBirth":'',
                "deviceId":'',
                "deviceType":'',
                "Gender":'',
                "FirstName":'',
                "LastName":'',
                "isDeleted":false,
                "isLoggedIn":false,
                "lastLogin":'',
                "userKey":'',
                "State":'',
                "ZipCode": '',
                "picFile": '',
                "RoleType": '',
            };
            $("#default_loader").hide();
        }else{
            $scope.IsPassword=false;
            $scope.user_id = $stateParams.userId;
            userService.getUser($scope.user_id).then(
                function (data) {

                    if (data!=null || data!=undefined) {
                        $scope.userdetail = data;
                        $scope.item = {
                            "UserId": $scope.userdetail.UserID,
                            "UserName": $scope.userdetail.UserName,
                            "Email": $scope.userdetail.Email,
                            "userrole": $scope.userdetail.RoleId,
                            "Address": $scope.userdetail.Address,
                            "PhoneNo": $scope.userdetail.PhoneNo,
                            "isactive": $scope.userdetail.IsActive,
                            "City":$scope.userdetail.City,
                            "DateOfBirth": new Date($scope.userdetail.DateOfBirth),
                            "deviceId":$scope.userdetail.DeviceId,
                            "deviceType":$scope.userdetail.DeviceType,
                            "Gender":$scope.userdetail.Gender=="F"?"Female":"Male",
                            "FirstName":$scope.userdetail.FirstName,
                            "LastName":$scope.userdetail.LastName,
                            "isDeleted":$scope.userdetail.IsDeleted,
                            "isLoggedIn":$scope.userdetail.IsLoggedIn,
                            "lastLogin":$scope.userdetail.LastLogin,
                            "userKey":$scope.userdetail.UserKey,
                            "State":$scope.userdetail.State,
                            "ZipCode": $scope.userdetail.ZipCode,
                            "ConfirmPassword": '',
                            "RoleType": $scope.roleName,


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
            $("#default_loader").hide();
        }

    }
    $scope.addUser();

    $scope.save = function() {

        $("#default_loader").show();

        $scope.item.DateOfBirth= $scope.item.DateOfBirth;
        $scope.item.RoleType = $scope.roleName;
        if ($scope.item.ZipCode == "") {
            $scope.item.ZipCode = 0;
        }
        else {
            $scope.item.ZipCode = parseInt($scope.item.ZipCode);
        }
        if ($scope.userForm.$valid) {

            $scope.item.latitude='';
            $scope.item.longitude='';
            $scope.item.ImageName='';
            if($scope.item.UserId == '') {
                userService.userAdd($scope.item).then(
                    function (data) {
                        console.log(data);

                        if(data!=null && data!=undefined) {
                            //$scope.allcms = data.ResponseMessage;
                            $location.path('admin/staffList');

                        }
                    },
                    function (errorMessage) {
                        $("#default_loader").hide();
                        $rootScope.showToast(errorMessage, "alert alert-danger");
                    });
            }else{
                if ($scope.item.picFile) {
                    userService.uploadUsingUpload($scope.item).then(
                        function (data) {
                            $("#default_loader").hide();
                            $location.path('admin/staffList');
                        },
                        function (errorMessage) {
                            $("#default_loader").hide();
                            $rootScope.showToast(errorMessage, "alert alert-danger");
                        });
                }
                else {
                    userService.userEdit($scope.item).then(
                        function (data) {
                            $("#default_loader").hide();

                            $location.path('admin/staffList');
                        },
                        function (errorMessage) {
                            $("#default_loader").hide();
                            $rootScope.showToast(errorMessage, "alert alert-danger");
                        });
                }

            }
        }
    }

    $scope.setFile = function (element) {
        $scope.currentFile = element.files[0];
        $scope.isShow = true;
        var reader = new FileReader();

        reader.onload = function (event) {
            $scope.image_source = event.target.result;
            $scope.$apply()

        }
        reader.readAsDataURL(element.files[0]);
    }


    //$scope.getLoginDetails();



});


