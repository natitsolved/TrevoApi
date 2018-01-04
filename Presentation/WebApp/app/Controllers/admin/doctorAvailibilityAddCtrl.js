'use strict';

app.controller('doctorAvailibilityAddCtrl', function ($rootScope, $scope, $http,$filter, $location, myAuth, $cookieStore,$timeout,doctorAvailibilityService,$stateParams,$state,$window,$compile) {


    $scope.viewUser = "view";





$scope.Redirect=function (WeekDay) {
    $window.localStorage["WeekDay"]=WeekDay;

$state.go('admin.availibility',{},{reload:true});
}


$scope.BackFunctionName=function () {
    if($window.localStorage["StateParameters"]!=undefined && $window.localStorage["StateParameters"]!=null)
    {

        $state.go('admin.doctoravailability',{},{reload:true});
        $window.localStorage["StateParameters"]=null;
    }
    else {

        $state.go('admin.doctoravailabilityadd', {}, {reload: true});
    }
    $rootScope.availList=null;

}
    $scope.Save = function() {
if( $window.localStorage["WeekDay"]!=undefined && $window.localStorage["WeekDay"]!=null)
{

    $scope.WeekDay= $window.localStorage["WeekDay"];

    $window.localStorage["WeekDay"]=null;
}
if( $window.localStorage["DoctorId"] !=undefined && $window.localStorage["DoctorId"]!=null)
{

    $scope.DoctorId=$window.localStorage["DoctorId"];
    $window.localStorage["DoctorId"]=null;
}
        $scope.item={

            "WeekDay":"",
            "DoctorId":"",
            "StartTime":"",
            "EndTime":""
        };

        $scope.finalArray=[];
        $scope.startTimeArray=[];
        $scope.EndTimeArray=[];
        $scope.startTimeArray=document.querySelectorAll("[id^=StartTime]");
        $scope.EndTimeArray=document.querySelectorAll("[id^=EndTime]");
        for(var i=0;i<$scope.startTimeArray.length;i++)
        {
            $scope.item={

                "WeekDay":$scope.WeekDay,
                "DoctorId":$scope.DoctorId,
                "StartTime":$scope.startTimeArray[i].value,
                "EndTime":$scope.EndTimeArray[i].value
            }
            $scope.finalArray.push($scope.item);
        }
        doctorAvailibilityService.InsertDocAvailDetails($scope.finalArray).then(

            function (data) {

               $state.go('admin.doctoravailability',{},{reload:true});
            },
            function (errorMessage) {
                //messageService.FlashMessage("danger", errorMessage);

            }
        );

    }

    $scope.counter = 0;

    $scope.backFunction=function () {
       $state.go('admin.doctoravailability',{},{reload:true});
    };

    $scope.AppendHtml=function (inc) {
        if( $scope.appendHtml==undefined)
        {
            $scope.appendHtml='';

        }
        $scope.counter += inc;
        var myEl = angular.element( document.querySelector( '#appendHtml' ) );
        var value='StartTime_'+$scope.counter;
        var value1='EndTime_'+$scope.counter;
        myEl.append( "<p>Start Time: <input type='time' ng-model='"+value+"' id='"+value+"' > &nbsp; End Time: <input type='time' ng-model='"+value1+"' id='"+value1+"'>");
    };


    $scope.GetDoctorListWithName=function () {

        doctorAvailibilityService.GetDoctorListWithName().then(

            function (data) {

               $scope.allDoctors=data;
            },
            function (errorMessage) {
                //messageService.FlashMessage("danger", errorMessage);

            }
        );
    }

    $scope.GetDoctorListWithName();

$scope.Change=function (DoctorId) {
    $window.localStorage["DoctorId"]=DoctorId;
}





$scope.Entry=function () {
    if($stateParams.userId=="add")
    {


    }
    else
    {
        if($stateParams.userId!=undefined && $stateParams.userId!="")
        {
            if($stateParams.weekday!=undefined && $stateParams.weekday!="")
            {
                $window.localStorage["StateParameters"]=$stateParams.userId;
                doctorAvailibilityService.GetDocAvailListByDocIdAndWeek($stateParams.userId,$stateParams.weekday).then(
                    function (data) {

                        if(data.length>0)
                        {
                            $rootScope.availList=data;
                            $state.go('admin.availibility',{},{reload:true});

                        }
                    },
                    function (errorMessage) {
                        alert('Sorry!! There is some error. Please try again later');
                    }
                );
            }

        }
        else
        {
            if($rootScope.availList!=undefined && $rootScope.availList!=null) {
                $window.localStorage["WeekDay"]=$rootScope.availList[0].WeekDay;
                $window.localStorage["DoctorId"]=$rootScope.availList[0].DoctorId;
                for (var i = 0; i < $rootScope.availList.length; i++) {

                        var myEl = angular.element(document.querySelector('#editHtml'));
                        var value = 'StartTime_' + i;
                        var value1 = 'EndTime_' + i;
                    //var html="<p>Start Time: <input type='time' ng-model='" + value + "' id='" + value + "' value='"+$rootScope.availList[i].StartTime+"' disabled> &nbsp; End Time: <input type='time' ng-model='" + value1 + "' id='" + value1 + "' value='"+$rootScope.availList[i].EndTime+"' disabled> &nbsp;&nbsp;<button class='btn btn-danger ' type='button' ng-click='DeleteAvailibility("+$rootScope.availList[i].ScheduleId+");'>Delete</button>";
                      var html="<p>Start Time: <input type='time' value='"+$rootScope.availList[i].StartTime+"' disabled >";
                      html = html+" &nbsp;<span>End Time: <input type='time' value='"+$rootScope.availList[i].EndTime+"' disabled></span>";
                      html = html+"&nbsp;<span><button class='btn btn-danger ' type='button' ng-click='DeleteAvailibility("+$rootScope.availList[i].ScheduleId+");'>Delete</button></span></p>";
                      myEl.append($compile(html)($scope));
                        /*$compile(myEl)($scope);*/

                }


            }
        }

    }
}
    $scope.Entry();
    
    
    $scope.DeleteAvailibility=function (ScheduleId) {
        doctorAvailibilityService.DeleteDocAvailByScheduleId(ScheduleId).then(
            function (data) {
                $state.go('admin.doctoravailability',{},{reload:true});
            }
        );
    }

});

