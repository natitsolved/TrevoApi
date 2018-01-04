'use strict';
/**
 * controllers used for the login
 */
app.controller('paymentInfoCtrl', function ($rootScope, $scope, $http, paymentService, $state, $stateParams, $timeout) {




    $scope.getPaymentInfo = function () {
        $("#default_loader").show();
        if ($stateParams.userId)
        {
            paymentService.getPaymentInfoByUserId($stateParams.userId).then(
                    function (data) {

                        if (data.length > 0) {
                            $scope.paymentInfoList = data;
                            $timeout(function () {

                                $scope.table = angular.element('#paymentInfoList').DataTable({
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
    



    }
    $scope.getPaymentInfo();

   

   


    $scope.backFunction = function () {
        $state.go('admin.patientList', {}, { reload: true });
    }

   




});

