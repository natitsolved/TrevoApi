'use strict';
/**
 * controllers used for the login
 */
app.controller('sliderCtrl', function ($rootScope, $scope, $http, $location, myAuth, $cookieStore, $timeout, sliderService, $state, $stateParams) {


    if ($stateParams.ImageID == undefined) {
        $stateParams.ImageID = "add";
    }
    $scope.option = {};

    $scope.GetAllSiderImages = function () {
        $("#default_loader").show();
        sliderService.getAllSliderImages().then(
            function (data) {
                if (data.length > 0) {
                    $scope.sliderList = data;
                    $timeout(function () {

                        $scope.table = angular.element('#sliderList').DataTable({
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
    $scope.addSliderImage = function () {
        $("#default_loader").show();
        if ($stateParams.ImageID == "add") {
            $scope.item = {
                "ImageName": '',
                "ImageID": '',
                "picFile": '',
                "Path": ''
            };
            $("#default_loader").hide();
        } else {
            $scope.ImageID = $stateParams.ImageID;
            sliderService.getImageDetailsById($scope.ImageID).then(
                function (data) {
                    if (data != null || data != undefined) {
                        $scope.item = {
                            "ImageName": data.ImageName,
                            "Path": data.Path,
                            "ImageID": $scope.ImageID
                        };
                        $scope.image_source = $scope.item.Path;
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
    $scope.GetAllSiderImages();
    $scope.addSliderImage();


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
    $scope.deleteImage = function (ImageID) {
        if (window.confirm("Want to delete?")) {
            $("#default_loader").show();
            sliderService.imageDelete(ImageID).then(
                   function (data) {
                       if (data != null || data != undefined) {
                           $state.go('admin.sliderList', {}, { reload: true });
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
        if ($scope.item.picFile == "" || $scope.item.picFile == undefined) {
            $("#default_loader").hide();
            alert("Please upload a file first.");
        }
        else {
            sliderService.uploadImage($scope.item.picFile).then(
                       function (data) {
                           $("#default_loader").hide();
                           $location.path('/admin/sliderList');
                       },
                       function (errorMessage) {
                           $("#default_loader").hide();
                           $rootScope.showToast(errorMessage, "alert alert-danger");
                       });
        }
    }


    $scope.backFunction = function () {
        $location.path('/admin/sliderList');
    }

});

