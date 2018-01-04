'use strict';
/**
 * controllers used for the login
 */
app.controller('healthPckgCtrl', function ($rootScope, $scope, $http, $location, myAuth, $cookieStore, $timeout, healthPckgService, $state, $stateParams) {


    if ($stateParams.PackageID == undefined) {
        $stateParams.PackageID = "add";
    }
    $scope.option = {};

    $scope.GetAllHealthPackages = function () {
        $("#default_loader").show();
        healthPckgService.getAllHealthPackages().then(
            function (data) {
                if (data.length > 0) {
                    $scope.healthPckgList = data;
                    $timeout(function () {

                        $scope.table = angular.element('#healthPckgList').DataTable({
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
    $scope.addHealthPckg = function () {
        $("#default_loader").show();
        if ($stateParams.PackageID == "add") {
            $scope.item = {
                "PackageType": '',
                "Description": '',
                "Price": '',
                "Validity": '',
                "IsActive": '',
                "SelectedWay": '',
                "PackageID": ''
            };
            $("#default_loader").hide();
        } else {
            $scope.PackageID = $stateParams.PackageID;
            healthPckgService.getHealthPckgById($scope.PackageID).then(
                function (data) {
                    if (data != null || data != undefined) {

                        $scope.item = {
                            "PackageType": data.PackageType,
                            "Description": data.Description,
                            "Price": data.Price,
                            "Validity": data.Validity,
                            "IsActive": data.IsActive,
                            "SelectedWay": data.SelectedWay,
                            "PackageID": data.PackageID

                        };
                        if ($scope.item.IsActive == 1) {
                            $scope.item.IsActive = true;
                        }
                        else {
                            $scope.item.IsActive = false;
                        }
                        $scope.image_source = data.ImagePath;
                        $scope.isShow = true;
                    }
                },
                function (errorMessage) {
                    $("#default_loader").hide();
                    $rootScope.showToast(errorMessage, "alert alert-danger");

                });
            $("#default_loader").hide();
        }

    }
    $scope.GetAllHealthPackages();
    $scope.addHealthPckg();


    $scope.setFile = function (element) {
        $scope.currentFile = element.files[0];
        $scope.isShow = true;
        var reader = new FileReader();

        reader.onload = function (event) {
            $scope.image_source = event.target.result;
            $scope.$apply()

        }
        reader.readAsDataURL(element.files[0]);
    }
    $scope.deletePckg = function (pckgId) {
        if (window.confirm("Want to delete?")) {
            $("#default_loader").show();
            healthPckgService.deleteHealthPckg(pckgId).then(
                   function (data) {
                       if (data != null || data != undefined) {
                           $state.go('admin.healthPackageList', {}, { reload: true });
                       }
                   },
                   function (errorMessage) {
                       $("#default_loader").hide();
                       $rootScope.showToast(errorMessage, "alert alert-danger");

                   });
        }

    }

    $scope.save = function () {
        $("#default_loader").show();

        if ($scope.item.SelectedWay.toString() == "") {
            $("#default_loader").hide(); $rootScope.showToast("Please select the validity type.", "alert alert-danger");
        }
        else {
            $scope.item.SelectedWay = parseInt($scope.item.SelectedWay);
            $scope.item.Validity = parseFloat($scope.item.Validity);
            if ($scope.item.IsActive) {
                $scope.item.IsActive = 1;
            }
            else {
                $scope.item.IsActive = 0;
            }
            if ($scope.PackageID != "" && $scope.PackageID != undefined) {

                healthPckgService.updateHealthPackage($scope.item).then(function (data) {
                    if ($scope.picFile) {
                        $scope.picItem = {
                            "picFile": $scope.picFile,
                            "PackageID": $scope.PackageID
                        }
                        healthPckgService.uploadImage($scope.picItem).then(function (data) {
                            $("#default_loader").hide();
                            $state.go('admin.healthPackageList', {}, { reload: true });
                        },
                       function (errorMessage) {
                           $("#default_loader").hide();
                           $rootScope.showToast(errorMessage, "alert alert-danger");
                       })
                    }
                    else {
                        $("#default_loader").hide();
                        $state.go('admin.healthPackageList', {}, { reload: true });
                    }
                },
                       function (errorMessage) {
                           $("#default_loader").hide();
                           $rootScope.showToast(errorMessage, "alert alert-danger");
                       });
            }
            else {
                healthPckgService.insertHealthPackage($scope.item).then(
                       function (data) {
                           debugger;
                           if ($scope.picFile) {
                               $scope.picItem = {
                                   "picFile": $scope.picFile,
                                   "PackageID": data.Message
                               }
                               healthPckgService.uploadImage($scope.picItem).then(function (data) {
                                   $("#default_loader").hide();
                                   $state.go('admin.healthPackageList', {}, { reload: true });
                               }, function (errorMessage) {
                                   $("#default_loader").hide();
                                   $rootScope.showToast(errorMessage, "alert alert-danger");
                               })
                           }
                           else { $("#default_loader").hide(); $state.go('admin.healthPackageList', {}, { reload: true }); }
                       },
                       function (errorMessage) {

                           $("#default_loader").hide();
                           $rootScope.showToast(errorMessage.data.Message, "alert alert-danger");
                       });
            }
        }


    }


    $scope.backFunction = function () {
        $state.go('admin.healthPackageList', {}, { reload: true });
    }

});

