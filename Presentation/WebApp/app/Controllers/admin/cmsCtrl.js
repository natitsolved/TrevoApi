'use strict';
/**
 * controllers used for the login
 */
app.controller('cmsCtrl', function ($rootScope, $scope, $http, $location, myAuth, $cookieStore,$timeout,cmsService,$state) {



    $scope.GetAllCms = function () {
        $("#default_loader").show();
        cmsService.getAllcms().then(
            function (data) {
                if(data.length>0) {
                    $scope.allcms = data;
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
        $scope.cmsView='view';
    }
    $scope.GetAllCms();

    $scope.editCms = function (params) {
      
     $scope.item = angular.copy(params);
     $scope.item.image = '';
     $scope.item.image_url = params.image;
     $scope.cmsView='edit';
     }

     $scope.addCms = function () {
     //alert(13);
     $scope.item={
     "PageID":'',
     "Title": '',
     "PageContent": '',
     "MetaDescription":'',
     "MetaKeyWord":'',
     "Slug":'',
     "IsActive":'',
         "IsDeleted":''
     };
     $scope.cmsView='edit';
     }

     $scope.cancelCms = function () {
     }

     $scope.saveCms = function () {
         $("#default_loader").show();

     if($scope.item.id == '') {
         $scope.item.slug = $scope.item.name.split(' ').join('-');
         cmsService.cmsAdd(item).then(
             function (data) {

                 /*if(data.ResponseDetails.ResponseStatus=="10") {
                  messageService.FlashMessage("success", data.ResponseMessage);
                  }
                  else
                  {
                  messageService.FlashMessage("danger", data.ResponseMessage);
                  }*/
                 if(data.ResponseDetails.ResponseCode == 'Success') {
                     //$scope.allcms = data.ResponseMessage;
                     $("#default_loader").hide();

                 }
             },
             function (errorMessage) {
                 $("#default_loader").hide();
                 $rootScope.showToast(errorMessage, "alert alert-danger");
             });
     }else{

     }

     }

    $scope.deleteCms = function (PageID) {
        //alert(c_id);
        if (window.confirm("Want to delete?")) {
            $("#default_loader").show();
            cmsService.cmsDelete(PageID).then(
                function (data) {

                    console.log(data);
                    $("#default_loader").hide();
                  $state.go('admin.cmslist',{},{reload:true});
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

