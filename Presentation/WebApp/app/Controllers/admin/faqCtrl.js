'use strict';
/**
 * controllers used for the login
 */
app.controller('faqCtrl', function ($rootScope, $scope, $http, $location, myAuth, $cookieStore,$timeout,faqService,$state) {



    $scope.GetAllFAQ = function () {
        $("#default_loader").show();
        faqService.getAllFaq().then(
            function (data) {
                if(data.length>0) {
                    $scope.allfaq = data;
                    $timeout(function () {

                        $scope.table = angular.element('#cmsList').DataTable({
                            "paging": true,
                            "lengthChange": false,
                            "searching": true,
                            "ordering": true,
                            "info": true,
                            "autoWidth": false
                        });
                    }, 3000, false);
                }
                $("#default_loader").hide();
            },
            function (errorMessage) {
                $("#default_loader").hide();
                $rootScope.showToast(errorMessage, "alert alert-danger");
            });
        $scope.faqView='view';
    }
    $scope.GetAllFAQ();

    $scope.editFAQ = function (params) {
        $scope.item = angular.copy(params);
        $scope.item.image = '';
        $scope.item.image_url = params.image;
        $scope.faqView='edit';
    }

    $scope.addFAQ = function () {
        //alert(13);
        $scope.item={
            "FAQID":'',
            "Question": '',
            "Answer": ''
        };
        $scope.faqView='edit';
    }


    $scope.saveFAQ = function () {

        $("#default_loader").show();
        if($scope.item.id == '') {
            $scope.item.slug = $scope.item.name.split(' ').join('-');
            faqService.faqAdd(item).then(
                function (data) {

                    /*if(data.ResponseDetails.ResponseStatus=="10") {
                     messageService.FlashMessage("success", data.ResponseMessage);
                     }
                     else
                     {
                     messageService.FlashMessage("danger", data.ResponseMessage);
                     }*/
                    if (data.ResponseDetails.ResponseCode == 'Success') {
                        $("#default_loader").hide();
                        //$scope.allcms = data.ResponseMessage;

                    }
                },
                function (errorMessage) {
                    $("#default_loader").hide();
                    $rootScope.showToast(errorMessage, "alert alert-danger");
                });
        }else{

        }

    }

    $scope.deleteFAQ = function (FAQID) {
        //alert(c_id);
        if (window.confirm("Want to delete?")) {
            $("#default_loader").show();
            faqService.faqDelete(FAQID).then(
                function (data) {
                    $("#default_loader").hide();

                    console.log(data);
                    $state.go('admin.faqlist',{},{reload:true});
                },
                function (errorMessage) {
                    $("#default_loader").hide();
                    $rootScope.showToast(errorMessage, "alert alert-danger");
                });
        }else{

        }

    }




    //$scope.getLoginDetails();



});

