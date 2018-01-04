'use strict';
/** 
 * controllers used for the login
 */
app.controller('headerCtrl', function ($rootScope, $scope, $http, $location, myAuth, $cookieStore, $state, $window) {

  
        $scope.loggedindetails = myAuth.getAdminAuthorisation();
        
       
         $scope.userLogout=function(){
                $http({
                    method: "POST",
                   url: $rootScope.serviceurl + "user/logout",
                    data: {'userid': $scope.loggedindetails.id},
                        headers: {'Content-Type': 'application/json','accesstoken':$scope.loggedindetails.accesstoken},
                }).success(function(data) {
                    myAuth.resetAdminUserinfo();
                    $scope.loggedindetails = '';                   
                    $scope.loggedin = false;
                    $scope.notloggedin = true;
                    $location.path("/adminlogin/signin");

                });

               
         };
    $scope.SignOut = function() {
        myAuth.resetAdminUserinfo();
        $window.localStorage["appTitle"] = "StatMedClinic";
        $state.go('adminlogin', {}, { reload: true });
    };
        
    $scope.goToChangePassword = function ()
    {
        $state.go('admin.changePassword', { changePassId: "change" });
    }
   
});

