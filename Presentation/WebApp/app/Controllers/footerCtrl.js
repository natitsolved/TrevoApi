'use strict';
/** 
 * controllers used for the register
 */
app.controller('footerCtrl', function ($rootScope, $scope, $http, $location) {

   $scope.getSocialLinks = function(){
       $http({
           method: "GET",
           url: $rootScope.serviceurl+"getSiteSetting",
           headers: {'Content-Type': 'application/json'},
       }).success(function(data) {
           if(data.site) {
               $scope.sociallinks = data.site;
           }
       })
   }
    $scope.getSocialLinks();
});

