app.service(
    "questionnareService",
    function ($rootScope, $http, $q) {
        // Return public API.
        return ({
            getAllquestionnare: getAllquestionnare,
            insertQuestion: insertQuestion,
            getQuestionDetailsById: getQuestionDetailsById,
            questionDelete: questionDelete,
            questionEdit: questionEdit
        });
        // ---
        // PUBLIC METHODS.
        // ---


        function getAllquestionnare() {
            var request = $http({
                method: "GET",
                url: $rootScope.serviceurl + "GetAllQuestions",
                headers: { 'Content-Type': 'application/json', 'pageno': '0', 'limit': '20' }
            });
            return (request.then(handleSuccess, handleError));
        }
        function getQuestionDetailsById(questionId) {
            var request = $http({
                method: "GET",
                url: $rootScope.serviceurl + "GetQuestionDetailsById/" + questionId
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
        function questionDelete(questionId) {
            var request = $http({
                method: "POST",
                url: $rootScope.serviceurl + "DeleteQuestionDetails",
                data: { Id: questionId },
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