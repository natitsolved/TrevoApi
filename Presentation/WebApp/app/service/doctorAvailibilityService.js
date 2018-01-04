app.service(
    "doctorAvailibilityService",
    function( $rootScope,$http, $q) {
        // Return public API.
        return({

            GetDoctorAvalList:GetDoctorAvalList,
            InsertDocAvailDetails:InsertDocAvailDetails,
            GetDoctorListWithName:GetDoctorListWithName,
            DeleteDocAvailByDocId:DeleteDocAvailByDocId,
            GetDocAvailListByDocId:GetDocAvailListByDocId,
            GetDocAvailListByDocIdAndWeek:GetDocAvailListByDocIdAndWeek,
            DeleteDocAvailByScheduleId:DeleteDocAvailByScheduleId,

        });


        function GetDoctorAvalList() {
            var request = $http({
                method: "GET",
                url: $rootScope.serviceurl+"GetDoctorAvalList",
                headers: { 'Content-Type': 'application/json','offset':'0','limit':'20' }
            });
            return( request.then( handleSuccess, handleError ) );
        }




        function InsertDocAvailDetails(item) {
            var request = $http({
                method: "POST",
                url: $rootScope.serviceurl+"InsertDocAvailDetails",
                data:item,
                headers: { 'Content-Type': 'application/json'}
            });
            return( request.then( handleSuccess, handleError ) );
        }


        function GetDoctorListWithName() {
            var request = $http({
                method: "GET",
                url: $rootScope.serviceurl+"GetDoctorListWithName",
                headers: { 'Content-Type': 'application/json'}
            });
            return( request.then( handleSuccess, handleError ) );
        }

        function DeleteDocAvailByDocId(DoctorId,ScheduleId) {
            var request = $http({
                method: "POST",
                url: $rootScope.serviceurl+"DeleteDocAvailByDocId",
                data:{Id:DoctorId,ScheduleId:ScheduleId},
                headers: { 'Content-Type': 'application/json'}
            });
            return( request.then( handleSuccess, handleError ) );
        }


        function GetDocAvailListByDocId(doctorId) {
            var request = $http({
                method: "GET",
                url: $rootScope.serviceurl+"GetDocAvailListByDocId/"+doctorId,
                headers: { 'Content-Type': 'application/json'}
            });
            return( request.then( handleSuccess, handleError ) );
        }

        function GetDocAvailListByDocIdAndWeek(doctorId,weekDay) {
            var request = $http({
                method: "GET",
                url: $rootScope.serviceurl+"GetDocAvailListByDocIdAndWeek/"+doctorId+"/"+weekDay,
                headers: { 'Content-Type': 'application/json'}
            });
            return( request.then( handleSuccess, handleError ) );
        }

        function DeleteDocAvailByScheduleId(ScheduleId) {
            var request = $http({
                method: "POST",
                url: $rootScope.serviceurl+"DeleteDocAvailByScheduleId",
                data:{Id:ScheduleId},
                headers: { 'Content-Type': 'application/json'}
            });
            return( request.then( handleSuccess, handleError ) );
        }


        function handleError( response ) {
            if (! angular.isObject( response.data ) ||! response.data.message) {
                return( $q.reject( response.data.Message ) );
            }
            // Otherwise, use expected error message.
            return( $q.reject( response.data.message ) );
        }

        function handleSuccess( response ) {
            return( response.data );


        }
    }
);