'use strict';
/** 
 * controllers used for the login
 */
app.controller('settingsCtrl', function ($rootScope, $scope, $http, $location, myAuth, $cookieStore,$timeout) {



    $scope.editSetting = function () {

        $http({
            method: "GET",
            url: $rootScope.serviceurl + "getSiteSetting",
            //data: {"email":$scope.email,"password":$scope.password},
            //headers: {'Content-Type': 'application/json'},
        }).success(function (data) {
            $scope.item = data.site;
            $scope.categoryView='edit';
        });



    }
    $scope.editSetting();


    $scope.saveSetting = function () {

        //return false;
        //console.log($scope.item);
        //return false;

            $http({
                method: "POST",
                url: $rootScope.serviceurl + "updateSiteSettings",
                data: {"new_promo_days": $scope.item.new_promo_days,"last_day_promo": $scope.item.last_day_promo,"hot_percentage": $scope.item.hot_percentage,"id": $scope.item.id,"special_promo_title":$scope.item.special_promo_title},
                headers: {'Content-Type': 'application/json'},
            }).success(function (data) {
                console.log(data);
                $scope.editSetting();

                //$scope.allcat = data.category;
                //console.log($scope.allcat);
            });


    }


         //$scope.getLoginDetails();

         
   
});

