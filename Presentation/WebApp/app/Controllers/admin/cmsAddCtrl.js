'use strict';
/**
 * controllers used for the login
 */
app.controller('cmsAddCtrl', function ($rootScope, $scope, $http, $location, myAuth, $cookieStore,$timeout,cmsService,$state,$stateParams) {



    $scope.viewUser = "view";

$scope.backFunction=function () {
    $state.go('admin.cmslist');
}

    $scope.addCms = function () {

        if ($stateParams.pageid == "add") {
            $scope.item = {
                "PageID": '',
                "Title": '',
                "PageContent": '',
                "Slug": '',
                "IsActive": '',
                "IsDeleted":'',
                "MetaDescription": '',
                "MetaKeyWord": ''
            };
        }else{

            $scope.pageid = $stateParams.pageid;
            cmsService.getCmsDetailsById($scope.pageid).then(
                function (data) {

                    if (data!=null || data!=undefined) {
                        $scope.userdetail = data;
                        $scope.item = {
                            "PageID": $scope.userdetail.PageID,
                            "Title": $scope.userdetail.Title,
                            "PageContent": $scope.userdetail.PageContent,
                            "Slug": $scope.userdetail.Slug,
                            "IsActive": $scope.userdetail.IsActive,
                            "IsDeleted": $scope.userdetail.IsDeleted,
                            "MetaDescription": $scope.userdetail.MetaDescription,
                            "MetaKeyWord":$scope.userdetail.MetaKeyWord,


                        };
                    }
                },
                function (errorMessage) {
                    //messageService.FlashMessage("danger", errorMessage);

                });
        }

    }
    $scope.addCms();
    $scope.saveCms = function() {
        if ($scope.cmsForm.$valid) {

            if($scope.item.PageID == '') {
                cmsService.cmsAdd($scope.item).then(
                    function (data) {
                        console.log(data);

                        if(data!=null && data!=undefined) {
                            //$scope.allcms = data.ResponseMessage;
                            $location.path('admin/cmslist');

                        }
                    },
                    function (errorMessage) {
                        alert(errorMessage);
                    });
            }else{

                cmsService.cmsEdit($scope.item).then(
                    function (data) {
                        console.log(data);

                        if(data!=null && data!=undefined) {
                            //$scope.allcms = data.ResponseMessage;
                            $location.path('admin/cmslist');

                        }
                    },
                    function (errorMessage) {
                        alert(errorMessage);
                    });

            }
        }
    }
});

