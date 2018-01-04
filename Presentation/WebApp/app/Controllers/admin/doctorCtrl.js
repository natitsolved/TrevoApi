'use strict';
/**
 * controllers used for the login
 */
app.controller('doctorCtrl', function ($rootScope, $scope, $http,
                                        $location, myAuth,
                                        $cookieStore,$timeout,
                                        userService, $state)
{
   
    $scope.viewUser = "view";
    $scope.roleName="Doctor";
    
    $scope.GetAllDoctor = function () {
        $("#default_loader").show();
        userService.getAllUsersWithRole($scope.roleName).then(
            function (data) {
              
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
                $("#default_loader").hide();
            },
            function (errorMessage) {
                $("#default_loader").hide();
                $rootScope.showToast(errorMessage, "alert alert-danger");
            });



    }
    $scope.GetAllDoctor();


    $scope.deleteUser=function(c_id){
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


});

