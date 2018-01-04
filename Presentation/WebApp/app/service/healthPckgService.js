app.service(
    "healthPckgService",
    function ($rootScope, $http, $q, Upload) {
        // Return public API.
        return ({
            getAllHealthPackages: getAllHealthPackages,
            uploadImage: uploadImage,
            getHealthPckgById: getHealthPckgById,
            deleteHealthPckg: deleteHealthPckg,
            insertHealthPackage: insertHealthPackage,
            updateHealthPackage: updateHealthPackage
        });
        // ---
        // PUBLIC METHODS.
        // ---


        function getAllHealthPackages() {
            var request = $http({
                method: "GET",
                url: $rootScope.serviceurl + "GetAllHealthPackage",
                headers: { 'Content-Type': 'application/json', 'pageno': '0', 'limit': '20' }
            });
            return (request.then(handleSuccess, handleError));
        }
        function getHealthPckgById(pckgId) {
            var request = $http({
                method: "POST",
                url: $rootScope.serviceurl + "GetHealthPackageDetailsById",
                data: { Id: pckgId },
                headers: { 'Content-Type': 'application/json' }
            });
            return (request.then(handleSuccess, handleError));
        }

        function insertHealthPackage(item) {
            var request = $http({
                method: "POST",
                url: $rootScope.serviceurl + "InsertHealthPackageDetails",
                data: item,
                headers: { 'Content-Type': 'application/json' }
            });
            return (request.then(handleSuccess, handleError));
        }

        function updateHealthPackage(item) {
            var request = $http({
                method: "POST",
                url: $rootScope.serviceurl + "UpdateHealthPackageDetails",
                data: item,
                headers: { 'Content-Type': 'application/json' }
            });
            return (request.then(handleSuccess, handleError));
        }
        function deleteHealthPckg(pckgId) {
            var request = $http({
                url: $rootScope.serviceurl + "DeleteHealthPackageDetails",
                data: { Id: pckgId },
                headers: { 'Content-Type': 'application/json' }
            });
            return (request.then(handleSuccess, handleError));
        }

        function uploadImage(item) {
            return $q(function (handleSuccess, handleError) {
                Upload.upload({
                    method: 'POST',
                    url: $rootScope.serviceurl + "UploadPackageImage",
                    data: item,
                }).success(function (response) {

                    //console.log(response.ack);

                    handleSuccess(response);
                }).error(function (err) {
                    handleError(err.Message);
                });

            });
        };


        // ---
        // PRIVATE METHODS.
        // ---

        function handleError(response) {
            if (!angular.isObject(response.data) || !response.data.message) {
                return ($q.reject(response));
            }
            // Otherwise, use expected error message.
            return ($q.reject(response.data.message));
        }

        function handleSuccess(response) {
            return (response.data);


        }
    }
);