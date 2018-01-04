'use strict';
/** 
 * controllers used for the login
 */
app.controller('userCtrl', function ($rootScope, $scope, $http, $location, myAuth, $cookieStore,$timeout,userService) {

    myAuth.updateAdminUserinfo(myAuth.getAdminAuthorisation());
    $scope.loggedindetails = myAuth.getAdminNavlinks();
    //console.log($scope.loggedindetails);
    if(!$scope.loggedindetails){
        $location.path('/adminlogin/signin');
    }

    /*myApp.service('filteredListService', function () {



        this.paged = function (valLists, pageSize) {
            retVal = [];
            for (var i = 0; i < valLists.length; i++) {
                if (i % pageSize === 0) {
                    retVal[Math.floor(i / pageSize)] = [valLists[i]];
                } else {
                    retVal[Math.floor(i / pageSize)].push(valLists[i]);
                }
            }
            return retVal;
        };

    });*/

//Inject Custom Service Created by us and Global service $filter. This is one way of specifying dependency Injection
   /* var TableCtrl = myApp.controller('TableCtrl', function ($scope, $filter, filteredListService) {

        $scope.pageSize = 4;
        $scope.allItems =  $scope.allusers;
        $scope.reverse = false;

        $scope.resetAll = function () {
            $scope.filteredList = $scope.allItems;
            $scope.newEmpId = '';
            $scope.newName = '';
            $scope.newEmail = '';
            $scope.searchText = '';
            $scope.currentPage = 0;
            $scope.Header = ['', '', ''];
        }

        $scope.add = function () {
            $scope.allItems.push({
                EmpId: $scope.newEmpId,
                name: $scope.newName,
                Email: $scope.newEmail
            });
            $scope.resetAll();
        }

        $scope.search = function () {
            $scope.filteredList = filteredListService.searched($scope.allItems, $scope.searchText);

            if ($scope.searchText == '') {
                $scope.filteredList = $scope.allItems;
            }
            $scope.pagination();
        }

        // Calculate Total Number of Pages based on Search Result
        $scope.pagination = function () {
            $scope.ItemsByPage = filteredListService.paged($scope.filteredList, $scope.pageSize);
        };

        $scope.setPage = function () {
            $scope.currentPage = this.n;
        };

        $scope.firstPage = function () {
            $scope.currentPage = 0;
        };

        $scope.lastPage = function () {
            $scope.currentPage = $scope.ItemsByPage.length - 1;
        };

        $scope.range = function (input, total) {
            var ret = [];
            if (!total) {
                total = input;
                input = 0;
            }
            for (var i = input; i < total; i++) {
                if (i != 0 && i != total - 1) {
                    ret.push(i);
                }
            }
            return ret;
        };

        $scope.sort = function (sortBy) {
            $scope.resetAll();

            $scope.columnToOrder = sortBy;

            //$Filter - Standard Service
            $scope.filteredList = $filter('orderBy')($scope.filteredList, $scope.columnToOrder, $scope.reverse);

            if ($scope.reverse)iconName = 'glyphicon glyphicon-chevron-up';
            else iconName = 'glyphicon glyphicon-chevron-down';

            if (sortBy === 'EmpId') {
                $scope.Header[0] = iconName;
            } else if (sortBy === 'name') {
                $scope.Header[1] = iconName;
            } else {
                $scope.Header[2] = iconName;
            }

            $scope.reverse = !$scope.reverse;

            $scope.pagination();
        };

        //By Default sort ny Name
        $scope.sort('name');

    });*/






    $scope.viewUser = function () {
        /*userService.getAllUsers($scope.loggedindetails.accesstoken).then(
            function (data) {
                console.log(data);

                if(data.ResponseDetails.ResponseCode == 'Success') {
                    $scope.allusers = data.ResponseMessage;
                    console.log($scope.allusers);

                }
            },
            function (errorMessage) {
                //messageService.FlashMessage("danger", errorMessage);
            });*/

        $scope.userView='view';
    }
    $scope.viewUser();

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



         
         //$scope.getLoginDetails();

         
   
});

