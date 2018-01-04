'use strict';
/**
 * controllers used for the login
 */
app.controller('patientCtrl', function ($rootScope, $scope, $http,
                                     $location, myAuth,
                                     $cookieStore,$timeout,
                                     userService,$state,ngToast)
{

    

    $scope.viewUser = "view";
    $scope.roleName="Patient";

    $scope.GetAllPatient = function () {
        $("#default_loader").show();
        userService.getAllUsersWithRole($scope.roleName).then(
            function (data) {
                $("#default_loader").hide();
                if(data.length>0) {
                    $scope.userList = data;
                    $timeout(function () {

                        $scope.table = angular.element('#userList').DataTable({
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
            },
            function (errorMessage) {
                $("#default_loader").hide();
                $rootScope.showToast(errorMessage, "alert alert-danger");
            });

        /*userService.getAllUsers().then(function(result) {
         $scope.userList=result.ResponseMessage;
         });*/

    }
    $scope.GetAllPatient();

    
    $scope.deleteUser = function (c_id) {
     
        if (window.confirm("Want to delete?")) {
            $("#default_loader").show();
            $http({
                method: "POST",
                url: $rootScope.serviceurl + "DeleteUserById",
                data:{Id:c_id},
                headers: {'Content-Type': 'application/json'},
            }).success(function (data) {
                $("#default_loader").hide();
               /* console.log(data)*/;
                $state.go($state.current, {}, {reload: true});
                /*alert(data);*/

                //$scope.allcat = data.category;
                //console.log($scope.allcat);
            });
        }else{

        }

    }

    $scope.editUser=function(u_id){
        $location.path("/");
    }


 /*   ngToast.create('a toast message...');
*/

});

