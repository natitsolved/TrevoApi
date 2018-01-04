'use strict';
/**
 * controllers used for the login
 */
app.controller('paymentCtrl', function ($rootScope, $scope, $http, $location, myAuth, $cookieStore, $timeout, paymentService, $state) {



    $scope.GetAllPaymentList = function () {
        $("#default_loader").show();
        paymentService.getAllPaymentList().then(
            function (data) {
                if (data.length > 0) {
                    $scope.paymentList = data;
                    $timeout(function () {

                        $scope.table = angular.element('#paymentList').DataTable({
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
       
    }
    $scope.GetAllPaymentList();

    

    $scope.deletePaymentInfo = function (paymentId) {
        //alert(c_id);
        if (window.confirm("Want to delete/")) {
            $("#default_loader").show();
            paymentService.deletePaymentById(paymentId).then(
                function (data) {
                    $("#default_loader").hide();

                    console.log(data);
                    $state.go('admin.paymentList', {}, { reload: true });
                },
                function (errorMessage) {
                    $("#default_loader").hide();
                    $rootScope.showToast(errorMessage, "alert alert-danger");
                });
        } else {

        }

    }




   



});

