app.service(
    "paymentService",
    function ($rootScope, $http, $q) {
        // Return public API.
        return ({
            getAllPaymentList: getAllPaymentList,
            deletePaymentById: deletePaymentById,
            getPaymentInfoByUserId: getPaymentInfoByUserId
        });
        // ---
        // PUBLIC METHODS.
        // ---


        function getAllPaymentList() {
            var request = $http({
                method: "GET",
                url: $rootScope.serviceurl + "GetAllPaymentDetails",
                headers: { 'Content-Type': 'application/json', 'pageno': '0', 'limit': '20' }
            });
            return (request.then(handleSuccess, handleError));
        }
       
        

        function deletePaymentById(paymentId) {
            var request = $http({
                method: "POST",
                url: $rootScope.serviceurl + "DeletePaymentDetails",
                data: { Id: paymentId },
                headers: { 'Content-Type': 'application/json' }
            });
            return (request.then(handleSuccess, handleError));
        }
        function getPaymentInfoByUserId(userId) {
            var request = $http({
                method: "POST",
                url: $rootScope.serviceurl + "GetPaymentDetailsByUserId",
                data: { Id: userId },
                headers: { 'Content-Type': 'application/json' }
            });
            return (request.then(handleSuccess, handleError));
        }




        // ---
        // PRIVATE METHODS.
        // ---

        function handleError(response) {
            if (!angular.isObject(response.data) || !response.data.message) {
                return ($q.reject("An unknown error occurred."));
            }
            // Otherwise, use expected error message.
            return ($q.reject(response.data.message));
        }

        function handleSuccess(response) {
            return (response.data);


        }
    }
);