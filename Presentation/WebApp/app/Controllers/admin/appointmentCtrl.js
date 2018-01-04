'use strict';
/**
 * controllers used for the login
 */
app.controller('appointmentCtrl', function ($rootScope, $scope, $http,
                                        $location, myAuth,
                                        $cookieStore, $timeout,
                                        staffService, $state, $stateParams, $filter) {




    $scope.GetAllAppointMents = function () {
        $("#default_loader").show();
        staffService.getAllApointMents().then(
            function (data) {

                if (data.length > 0) {
                    $scope.appointmentList = data;
                    $timeout(function () {

                        $scope.table = angular.element('#appointmentList').DataTable({
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
    $scope.GetAllAppointMents();

    $scope.loadDDls = function () {
        staffService.getAllUsersWithRoleAndUserId('doctor').then(
                   function (data) {
                       if (data != null || data != undefined) {
                           $scope.doctorList = data;

                           staffService.getAllUsersWithRoleAndUserId('patient').then(
                   function (data) {
                       if (data != null || data != undefined) {
                           $scope.patientList = data;

                       }
                   },
                   function (errorMessage) {
                       $("#default_loader").hide();
                       $rootScope.showToast(errorMessage, "alert alert-danger");

                   });

                       }
                       $("#default_loader").hide();
                   },
                   function (errorMessage) {
                       $("#default_loader").hide();
                       $rootScope.showToast(errorMessage, "alert alert-danger");

                   });
    }

    $scope.addUser = function () {
        $("#default_loader").show();
        $scope.loadDDls();
        $scope.item = {
            "DoctorID": '',
            "PatientID": '',
            "AppointmentDate": '',
            "AppointmentID": '',
            "ScheduledApptmntTime":''
        };
        if ($stateParams.appointmentId == undefined || $stateParams.appointmentId == 'add') {
            $scope.item = {
                "DoctorID": '',
                "PatientID": '',
                "AppointmentDate": '',
                "ScheduledApptmntTime": ''
            };
        }
        else {
            $scope.item.AppointmentID = $stateParams.appointmentId;
            staffService.getAppointmentById($scope.item.AppointmentID).then(
                    function (data) {
                        if (data != null || data != undefined) {
                            $scope.item.DoctorID = data.DoctorID;
                            $scope.item.PatientID = data.PatientID;
                            $scope.item.AppointmentDate = data.AppointmentDate;
                            $scope.item.ScheduledApptmntTime = data.ApptmntTime;

                        }
                        $("#default_loader").hide();
                    },
                    function (errorMessage) {
                        $("#default_loader").hide();
                        $rootScope.showToast(errorMessage, "alert alert-danger");

                    });
        }



    }
    $scope.addUser();


    $scope.backFunction = function () {
        $state.go('admin.appointmentList', {}, { reload: true });
    }

    $scope.save = function () {
        if ($scope.item.AppointmentDate && $scope.item.DoctorID && $scope.item.PatientID) {
            if ($scope.item.AppointmentID) {
                $scope.item.AppointMentStatus = "UPDATED";
                staffService.updateAppointmentDetails($scope.item).then(
         function (data) {
             if (data != null || data != undefined) {
                 $state.go('admin.appointmentList', {}, { reload: true });

             }
         },
         function (errorMessage) {
             $("#default_loader").hide();
             $rootScope.showToast(errorMessage, "alert alert-danger");

         });
            }

            else {
                staffService.insertappointmentDetails($scope.item).then(
         function (data) {
             if (data != null || data != undefined) {
                 $state.go('admin.appointmentList', {}, { reload: true });

             }
         },
         function (errorMessage) {
             $("#default_loader").hide();
             $rootScope.showToast(errorMessage, "alert alert-danger");

         });
            }
        }
        else {
            $rootScope.showToast("Doctor , Patient and AppointMent Date are required.", "alert alert-danger");
        }
    }

    $scope.deleteInfo = function (id) {
        var result = confirm("Are you sure you want to delete?");
        if (result)
        {
            staffService.deleteAppointMentById(id).then(
             function (data) {
                 if (data != null || data != undefined) {
                     $state.go('admin.appointmentList', {}, { reload: true });

                 }
             },
             function (errorMessage) {
                 $("#default_loader").hide();
                 $rootScope.showToast(errorMessage, "alert alert-danger");

             });
        }
        
        
    }


    $scope.changeformat = function () {
        $scope.item.AppointmentDate = $filter('date')($scope.data.date, 'yyyy-MM-dd HH:mm');
    }

   



});

