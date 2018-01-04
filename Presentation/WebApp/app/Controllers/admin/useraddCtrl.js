'use strict';
/** 
 * controllers used for the login
 */
app.controller('useraddCtrl', function ($rootScope, $scope, $http,$filter, $location, myAuth, $cookieStore,$timeout,userService,userRoleService,$stateParams) {

     
    $scope.viewUser = "view";

   /* function reloadData() {
        var resetPaging = false;
        vm.dtInstance.reloadData(callback, resetPaging);
    }
    function callback(json) {
        console.log(json);
    }*/

    /*$scope.viewUser = function () {
        userService.getAllUsers($scope.loggedindetails.accesstoken).then(
            function (data) {
                //console.log(data);

                if(data.ResponseDetails.ResponseCode == 'Success') {
                    $scope.allusers = data.ResponseMessage;
                    console.log($scope.allusers);
                    $timeout(function(){

                        $scope.table=  angular.element('#userlist').DataTable({
                            "paging": true,
                            "lengthChange": false,
                            "searching": true,
                            "ordering": true,
                            "info": true,
                            "autoWidth": false
                        });
                    }, 3000, false);



                }
            },
            function (errorMessage) {
                //messageService.FlashMessage("danger", errorMessage);
            });

        $scope.userView='view';
    }
    $scope.viewUser();*/

    /*$scope.editUser = function (params) {
        $scope.item = angular.copy(params);
        $scope.item.image = '';
        $scope.item.image_url = params.image;
        $scope.userView='edit';
    }

    $scope.addCategory = function () {
        //alert(13);
        $scope.item={
            "id":'',
            "title": '',
            "image":'',
            //"is_active":0,
            "image_url":''
        };
        $scope.userView='edit';
    }

    $scope.cancelUser = function () {
        $scope.viewUser();
    }

    $scope.saveUser = function () {

        //return false;
        if($scope.item.id == '') {
            $http({
                method: "POST",
                url: $rootScope.serviceurl + "addUser",
                data: $scope.item,
                headers: {'Content-Type': 'application/json'},
            }).success(function (data) {
                console.log(data);
                $scope.viewUser();
                //$scope.allcat = data.category;
                //console.log($scope.allcat);
            });
        }else{
            $http({
                method: "POST",
                url: $rootScope.serviceurl + "updateUser",
                data: $scope.item,
                headers: {'Content-Type': 'application/json'},
            }).success(function (data) {
                console.log(data);
                $scope.viewUser();
                //$scope.allcat = data.category;
                //console.log($scope.allcat);
            });
        }

    }*/

    $scope.deleteUser = function (c_id) {
        //alert(c_id);
        if ( window.confirm("Want to delete?") ) {
            $http({
                method: "DELETE",
                url: $rootScope.serviceurl + "deleteUser/"+c_id,
                //data: {"name": $scope.item.name,"is_active": $scope.item.is_active},
                //headers: {'Content-Type': 'application/json'},
            }).success(function (data) {
                console.log(data);
                $scope.viewUser();
                //$scope.allcat = data.category;
                //console.log($scope.allcat);
            });
        }else{

        }

    }

    $scope.backFunction=function () {
        $location.path('/admin/userlist');
    }
    $scope.addUser = function () {
        //alert(13);
        userRoleService.getAllUserRole().then(
            function (data) {
                //console.log(data);

                if (data.length>0) {
                    $scope.allroleusers = data;
                    //console.log($scope.allroleusers);
                }
            },
            function (errorMessage) {
                //messageService.FlashMessage("danger", errorMessage);
                $scope.allroleusers = "";
            });
        if ($stateParams.userId == "add") {
            $scope.IsPassword=true;
            $scope.item = {
                "_id": '',
                "name": '',
                "mailid": '',
                "userrole": '',
                "password": '',
                "address": '',
                "phoneno": '',
                "isactive": true,
                "city":'',
                "dateOfBirth":'',
                "deviceId":'',
                "deviceType":'',
                "gender":'',
                "firstName":'',
                "lastName":'',
                "isDeleted":false,
                "isLoggedIn":false,
                "lastLogin":'',
                "userKey":'',
                "state":'',
                "zipCode":''
            };
        }else{
            $scope.IsPassword=false;
            $scope.user_id = $stateParams.userId;
            userService.getUser($scope.user_id).then(
                function (data) {

                    if (data!=null || data!=undefined) {
                        $scope.userdetail = data;
                        $scope.item = {
                            "_id": $scope.userdetail.UserID,
                            "name": $scope.userdetail.UserName,
                            "mailid": $scope.userdetail.Email,
                            "userrole": $scope.userdetail.RoleId,
                            "address": $scope.userdetail.Address,
                            "phoneno": $scope.userdetail.PhoneNo,
                            "isactive": $scope.userdetail.IsActive,
                            "city":$scope.userdetail.City,
                            "dateOfBirth": new Date($scope.userdetail.dateOfBirth),
                            "deviceId":$scope.userdetail.DeviceId,
                            "deviceType":$scope.userdetail.DeviceType,
                            "gender":$scope.userdetail.Gender=="F"?"Female":"Male",
                            "firstName":$scope.userdetail.FirstName,
                            "lastName":$scope.userdetail.LastName,
                            "isDeleted":$scope.userdetail.IsDeleted,
                            "isLoggedIn":$scope.userdetail.IsLoggedIn,
                            "lastLogin":$scope.userdetail.LastLogin,
                            "userKey":$scope.userdetail.UserKey,
                            "state":$scope.userdetail.State,
                            "zipCode":$scope.userdetail.ZipCode,

                        };
                    }
                },
                function (errorMessage) {
                    //messageService.FlashMessage("danger", errorMessage);

                });
        }

    }
    $scope.addUser();

    $scope.save = function() {

        //$scope.$broadcast('show-errors-check-validity');
        //alert(12);
        console.log($scope.userForm.$valid);
        debugger;
        $scope.item.dateOfBirth=new Date( $scope.item.dateOfBirth);
        if ($scope.userForm.$valid) {

            // save the user
            //console.log($scope.item);
            if($scope.item._id == '') {
                userService.userAdd($scope.item).then(
                function (data) {
                    console.log(data);

                    if(data.ResponseDetails.ResponseStatus == 10) {
                        //$scope.allcms = data.ResponseMessage;
                        $location.path('admin/userlist');

                    }else if(data.ResponseDetails.ResponseStatus == 13){
                        alert("Email Already exist.");
                    }
                },
                function (errorMessage) {
                    //messageService.FlashMessage("danger", errorMessage);
                });
            }else{

                userService.userEdit($scope.item,$scope.loggedindetails.accesstoken).then(
                    function (data) {
                        console.log(data);

                        if(data.ResponseDetails.ResponseStatus == 10) {
                            //$scope.allcms = data.ResponseMessage;
                            $location.path('admin/userlist');

                        }else if(data.ResponseDetails.ResponseStatus == 23){
                            alert("Email Already exist.");
                        }
                    },
                    function (errorMessage) {
                        //messageService.FlashMessage("danger", errorMessage);
                    });

            }
        }
    }



         
         //$scope.getLoginDetails();

         
   
});

