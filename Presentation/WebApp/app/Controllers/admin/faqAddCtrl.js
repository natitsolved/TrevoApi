'use strict';
/**
 * controllers used for the login
 */
app.controller('faqAddCtrl', function ($rootScope, $scope, $http, $location, myAuth, $cookieStore,$timeout,faqService,$state,$stateParams) {



    $scope.viewUser = "view";

    $scope.backFunction=function () {
        $state.go('admin.faqlist',{},{reload:true});
    }

    $scope.addFAq = function () {

        if ($stateParams.faqId == "add") {
            $scope.item = {
                "FAQID": '',
                "Question": '',
                "Answer": ''
            };
        }else{

            $scope.faqId = $stateParams.faqId;
            faqService.getFAQDetailsById($scope.faqId).then(
                function (data) {

                    if (data!=null || data!=undefined) {
                        $scope.userdetail = data;
                        $scope.item = {
                            "FAQID": $scope.userdetail.FAQID,
                            "Question": $scope.userdetail.Question,
                            "Answer": $scope.userdetail.Answer,



                        };
                    }
                },
                function (errorMessage) {
                    //messageService.FlashMessage("danger", errorMessage);

                });
        }

    }
    $scope.addFAq();
    $scope.saveFAQ = function() {
        if ($scope.faqForm.$valid) {

            if($scope.item.FAQID == '') {
                faqService.faqAdd($scope.item).then(
                    function (data) {
                        console.log(data);

                        if(data!=null && data!=undefined) {
                            //$scope.allcms = data.ResponseMessage;
                            $location.path('admin/faqlist');

                        }
                    },
                    function (errorMessage) {
                        alert(errorMessage);
                    });
            }else{

                faqService.faqEdit($scope.item).then(
                    function (data) {
                        console.log(data);

                        if(data!=null && data!=undefined) {
                            //$scope.allcms = data.ResponseMessage;
                            $location.path('admin/faqlist');

                        }
                    },
                    function (errorMessage) {
                        alert(errorMessage);
                    });

            }
        }
    }
});

