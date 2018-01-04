app.service(
    "sliderService",
    function ($rootScope, $http, $q, Upload) {
        // Return public API.
        return ({
            getAllSliderImages: getAllSliderImages,
            uploadImage: uploadImage,
            getImageDetailsById: getImageDetailsById,
            imageDelete: imageDelete
        });
        // ---
        // PUBLIC METHODS.
        // ---


        function getAllSliderImages() {
            var request = $http({
                method: "GET",
                url: $rootScope.serviceurl + "GetAllSliderImages",
                headers: { 'Content-Type': 'application/json', 'pageno': '0', 'limit': '20' }
            });
            return (request.then(handleSuccess, handleError));
        }
        function getImageDetailsById(imageId) {
            var request = $http({
                method: "GET",
                url: $rootScope.serviceurl + "GetSliderImageById/" + imageId
            });
            return (request.then(handleSuccess, handleError));
        }

        function insertQuestion(item) {
            var request = $http({
                method: "POST",
                url: $rootScope.serviceurl + "InsertQuestionDetails",
                data: item,
                headers: { 'Content-Type': 'application/json' }
            });
            return (request.then(handleSuccess, handleError));
        }

        function questionEdit(item) {
            var request = $http({
                method: "POST",
                url: $rootScope.serviceurl + "UpdateQuestionDetails",
                data: item,
                headers: { 'Content-Type': 'application/json' }
            });
            return (request.then(handleSuccess, handleError));
        }
        function imageDelete(imageId) {
            var request = $http({
                method: "POST",
                url: $rootScope.serviceurl + "DeleteSliderImage",
                data: { Id: imageId },
                headers: { 'Content-Type': 'application/json' }
            });
            return (request.then(handleSuccess, handleError));
        }

        function uploadImage(item) {
            return $q(function (handleSuccess, handleError) {
                Upload.upload({
                    method: 'POST',
                    url: $rootScope.serviceurl + "UploadImageSlider",
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