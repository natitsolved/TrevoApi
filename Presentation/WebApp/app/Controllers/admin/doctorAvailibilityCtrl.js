'use strict';
/**
 * controllers used for the login
 */
app.controller('doctorAvailibilityCtrl', function ($rootScope, $scope, $http,
                                     $location, myAuth,
                                     $cookieStore,$timeout,
                                                   doctorAvailibilityService,$state)
{

    $scope.viewUser = "view";


    $scope.GetAllUser=function(){
        doctorAvailibilityService.GetDoctorAvalList().then(
            function (data) {

                if(data.length>0) {
                    $scope.alldoctor = data;
                    $timeout(function () {

                        $scope.table = angular.element('#doctorList').DataTable({
                            "paging": true,
                            "lengthChange": false,
                            "searching": true,
                            "ordering": true,
                            "info": true,
                            "autoWidth": false
                        });
                    }, 3000, false);
                }
            },
            function (errorMessage) {
                //messageService.FlashMessage("danger", errorMessage);
            });



    }
    $scope.GetAllUser();


$scope.deleteSchedule=function (DoctorId,ScheduleId) {
    doctorAvailibilityService.DeleteDocAvailByDocId(DoctorId,ScheduleId).then(
        function (data) {

            $state.go('admin.doctoravailability',{},{reload:true});
        },
        function (errorMessage) {
            //messageService.FlashMessage("danger", errorMessage);
        }
    );

}




});

