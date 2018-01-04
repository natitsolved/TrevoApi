app.service(
    "faqService",
    function( $rootScope,$http, $q ) {
        // Return public API.
        return({
            getAllFaq: getAllFaq,
            faqAdd: faqAdd,
            getFAQDetailsById:getFAQDetailsById,
            faqDelete:faqDelete,
            faqEdit:faqEdit
        });
        // ---
        // PUBLIC METHODS.
        // ---


        function getAllFaq() {
            var request = $http({
                method: "GET",
                url: $rootScope.serviceurl+"GetAllFAQ",
                headers: { 'Content-Type': 'application/json','pageno':'0','limit':'20' }
            });
            return( request.then( handleSuccess, handleError ) );
        }
        function getFAQDetailsById(faqId) {
            var request = $http({
                method: "GET",
                url: $rootScope.serviceurl+"GetFAQDetailsById/"+faqId
            });
            return( request.then( handleSuccess, handleError ) );
        }
        /* function cmsAdd(item,accesstoken) {
         var request = $http({
         method: "POST",
         url: $rootScope.serviceurl+"InsertCMSDetails",
         data:item,
         headers: { 'Content-Type': 'application/json','accesstoken':accesstoken}
         });
         return( request.then( handleSuccess, handleError ) );
         }*/

        function faqAdd(item) {
            var request = $http({
                method: "POST",
                url: $rootScope.serviceurl+"InsertFAQDetails",
                data:item,
                headers: { 'Content-Type': 'application/json'}
            });
            return( request.then( handleSuccess, handleError ) );
        }

        function faqEdit(item) {
            var request = $http({
                method: "POST",
                url: $rootScope.serviceurl+"UpdateFAQDetails",
                data:item,
                headers: { 'Content-Type': 'application/json'}
            });
            return( request.then( handleSuccess, handleError ) );
        }
        function faqDelete(FAQID) {
            var request = $http({
                method: "POST",
                url: $rootScope.serviceurl+"DeleteFAQDetails",
                data:{Id:FAQID},
                headers: { 'Content-Type': 'application/json'}
            });
            return( request.then( handleSuccess, handleError ) );
        }




        // ---
        // PRIVATE METHODS.
        // ---

        function handleError( response ) {
            if (! angular.isObject( response.data ) ||! response.data.message) {
                return( $q.reject( "An unknown error occurred." ) );
            }
            // Otherwise, use expected error message.
            return( $q.reject( response.data.message ) );
        }

        function handleSuccess( response ) {
            return( response.data );


        }
    }
);