'use strict';
/** 
 * controllers used for the login
 */
app.controller('userCtrl', function ($rootScope, $scope, $http,
                                     $location, myAuth,
                                     $cookieStore,$timeout,
                                     userService)
{
   
$scope.viewUser = "view";

    $scope.GetAllUser=function(){
        userService.getAllUsers().then(
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
                            "autoWidth": false
                        });
                    }, 3000, false);
                }
            },
            function (errorMessage) {
                //messageService.FlashMessage("danger", errorMessage);
            });


    }
	$scope.GetAllUser();



    //function deleteUser (c_id) {
    $scope.deleteUser=function(c_id){

        if ( window.confirm("Want to delete?") ) {
            $http({
                method: "DELETE",
                url: $rootScope.serviceurl + "deleteUser/"+c_id,
                //data: {"name": $scope.item.name,"is_active": $scope.item.is_active},
                //headers: {'Content-Type': 'application/json'},
            }).success(function (data) {
                console.log(data);
                $scope.GetAllUser();
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

