app.service(
    "cmsService",
    function( $rootScope,$http, $q ) {
        // Return public API.
        return({
            getAllcms: getAllcms,
            cmsAdd: cmsAdd,
            getCmsDetailsById:getCmsDetailsById,
            cmsDelete:cmsDelete,
            cmsEdit: cmsEdit
        });
        // ---
        // PUBLIC METHODS.
        // ---


        function getAllcms() {
            var request = $http({
                method: "GET",
                url: $rootScope.serviceurl+"GetAllCMS",
                headers: { 'Content-Type': 'application/json','pageno':'0','limit':'20' }
            });
            return( request.then( handleSuccess, handleError ) );
        }
        function getCmsDetailsById(pageId) {
            var request = $http({
                method: "GET",
                url: $rootScope.serviceurl+"GetCMSDetailsById/"+pageId
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

        function cmsAdd(item) {
            var request = $http({
                method: "POST",
                url: $rootScope.serviceurl+"InsertCMSDetails",
                data:item,
                headers: { 'Content-Type': 'application/json'}
            });
            return( request.then( handleSuccess, handleError ) );
        }

        function cmsEdit(item) {
            var request = $http({
                method: "POST",
                url: $rootScope.serviceurl+"UpdateCMSDetails",
                data:item,
                headers: { 'Content-Type': 'application/json'}
            });
            return( request.then( handleSuccess, handleError ) );
        }
        function cmsDelete(PageID) {
            var request = $http({
                method: "POST",
                url: $rootScope.serviceurl+"DeleteCMSDetails",
                data:{Id:PageID},
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