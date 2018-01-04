'use strict';
/** 
 * controllers used for the login
 */
app.controller('categorylistCtrl', function ($rootScope, $scope, $http, $location, myAuth, $cookieStore,$timeout) {

    $scope.viewCategory = function () {
        $http({
            method: "GET",
            url: $rootScope.serviceurl + "categories",
            //data: {"email":$scope.email,"password":$scope.password},
            //headers: {'Content-Type': 'application/json'},
        }).success(function (data) {

            $scope.allcat = data.category;

            $timeout(function(){

                $scope.table=  angular.element('#categoryList').DataTable({
                    "paging": true,
                    "lengthChange": false,
                    "searching": true,
                    "ordering": true,
                    "info": true,
                    "autoWidth": false
                });
            }, 3000, false);
            //console.log($scope.allcat);


        });
        $scope.categoryView='view';
    }

    $scope.viewCategory();

    $scope.editCategory = function (params) {
        $scope.item = params;
        $scope.categoryView='edit';
    }

    $scope.addCategory = function () {
        //alert(13);
        $scope.item={
            "name": '',
            "id": '',
            "seq":'',
            icon:'',
            "is_active":0
        };
        $scope.categoryView='edit';
    }

    $scope.cancelCat = function () {
        $scope.viewCategory();
    }

    $scope.saveCategory = function () {

        //return false;
        if($scope.item.id == '') {
            $http({
                method: "POST",
                url: $rootScope.serviceurl + "categories",
                data: {"name": $scope.item.name, "parent_id": "0", "is_active": $scope.item.is_active,seq:$scope.item.seq,icon:$scope.item.icon},
                headers: {'Content-Type': 'application/json'},
            }).success(function (data) {
                console.log(data);
                $scope.viewCategory();
                //$scope.allcat = data.category;
                //console.log($scope.allcat);
            });
        }else{
            $http({
                method: "PUT",
                url: $rootScope.serviceurl + "categories/"+$scope.item.id,
                data: {"name": $scope.item.name,"is_active": $scope.item.is_active,seq:$scope.item.seq,icon:$scope.item.icon},
                headers: {'Content-Type': 'application/json'},
            }).success(function (data) {
                console.log(data);
                $scope.viewCategory();
                //$scope.allcat = data.category;
                //console.log($scope.allcat);
            });
        }

    }

    $scope.deleteCategory = function (c_id) {
        //alert(c_id);
        if ( window.confirm("Want to delete?") ) {
            $http({
                method: "DELETE",
                url: $rootScope.serviceurl + "categories/"+c_id,
                //data: {"name": $scope.item.name,"is_active": $scope.item.is_active},
                //headers: {'Content-Type': 'application/json'},
            }).success(function (data) {
                console.log(data);
                $scope.viewCategory();
                //$scope.allcat = data.category;
                //console.log($scope.allcat);
            });
        }else{

        }

    }



         
         //$scope.getLoginDetails();

         
   
});

