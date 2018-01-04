app.service(
    "staffService",
    function ($rootScope, $http, $q) {
        // Return public API.
        return ({
            getAllUsersWithRole: getAllUsersWithRole,
            getAllUsersWithRoleAndUserId:getAllUsersWithRoleAndUserId,
            getAllClientListByStaffId: getAllClientListByStaffId,
            getUser: getUser,
            getDoctorDetails: getDoctorDetails,
            getEmojiData: getEmojiData,
            getAllApointMents: getAllApointMents,
            deleteAppointMentById: deleteAppointMentById,
            insertappointmentDetails: insertappointmentDetails,
            updateAppointmentDetails: updateAppointmentDetails,
            getAppointmentById: getAppointmentById
        });
        // ---
        // PUBLIC METHODS.
        // ---




        function getAllUsersWithRole(roleName) {
            var request = $http({
                method: "GET",
                url: $rootScope.serviceurl + "GetAllUserWithRole/" + roleName,
                headers: { 'Content-Type': 'application/json', 'offset': '0', 'limit': '20' }
            });
            return (request.then(handleSuccess, handleError));
        }
        function getAllUsersWithRoleAndUserId(roleName) {
            var request = $http({
                method: "GET",
                url: $rootScope.serviceurl + "GetUserListWithIdByRole/" + roleName,
                headers: { 'Content-Type': 'application/json', 'offset': '0', 'limit': '20' }
            });
            return (request.then(handleSuccess, handleError));
        }

        function getUser(user_id) {
            var request = $http({
                method: "POST",
                url: $rootScope.serviceurl + "GetUserWithRole/" + user_id,
                headers: { 'Content-Type': 'application/json' }
            });
            return (request.then(handleSuccess, handleError));
        }
        

        function getAllClientListByStaffId(staffKey) {
            var request = $http({
                method: "GET",
                url: $rootScope.serviceurl + "GetPatientDetailsByStaffId/" + staffKey,
                headers: { 'Content-Type': 'application/json', 'offset': '0', 'limit': '20' }
            });
            return (request.then(handleSuccess, handleError));
        }
        function getDoctorDetails(user_id) {
            var request = $http({
                method: "GET",
                url: $rootScope.serviceurl + "GetDoctorDetailsByKey/" + user_id,
                headers: { 'Content-Type': 'application/json' }
            });
            return (request.then(handleSuccess, handleError));
        }
        function getEmojiData() {
            var request = $http({
                method: "GET",
                dataType:"json",
                url: "app/assets/staff/emojis.json",
                headers: { 'Content-Type': 'application/json' }
            });
            return (request.then(handleSuccess, handleError));
        }
        function getAllApointMents() {
            var request = $http({
                method: "GET",
                dataType: "json",
                url: $rootScope.serviceurl + "GetAllAppointMentDetails",
                headers: { 'Content-Type': 'application/json' }
            });
            return (request.then(handleSuccess, handleError));
        }


        function deleteAppointMentById(id) {
            var request = $http({
                method: "POST",
                url: $rootScope.serviceurl + "DeleteAppointMentDetails",
                headers: { 'Content-Type': 'application/json' },
                data: { Id: id }
            });
            return (request.then(handleSuccess, handleError));
        }
        function getAppointmentById(id) {
            var request = $http({
                method: "POST",
                url: $rootScope.serviceurl + "GetappointmentDetailsById",
                headers: { 'Content-Type': 'application/json' },
                data: { Id: id }
            });
            return (request.then(handleSuccess, handleError));
        }
        function insertappointmentDetails(item) {
            var request = $http({
                method: "POST",
                dataType: "json",
                url: $rootScope.serviceurl + "InsertAppointMentDetails",
                headers: { 'Content-Type': 'application/json' },
                data: item
            });
            return (request.then(handleSuccess, handleError));
        }

        function updateAppointmentDetails(item) {
            var request = $http({
                method: "POST",
                dataType: "json",
                url: $rootScope.serviceurl + "UpdateAppointmentDetails",
                headers: { 'Content-Type': 'application/json' },
                data: item
            });
            return (request.then(handleSuccess, handleError));
        }
        

        //        }
        //    });
        //    return json;
        //})();
       

        // ---
        // PRIVATE METHODS.
        // ---

        function handleError(response) {
            if (!angular.isObject(response.data) || !response.data.message) {
                return ($q.reject(response.data));
            }
            // Otherwise, use expected error message.
            return ($q.reject(response.data.message));
        }

        function handleSuccess(response) {
            return (response.data);


        }
    }
);