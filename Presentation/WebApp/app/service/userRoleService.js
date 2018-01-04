app.service(
    "userRoleService",
    function( $rootScope,$http, $q ) {
        // Return public API.
        return({
            getAllUserRole: getAllUserRole,
        });
        // ---
        // PUBLIC METHODS.
        // ---

        // User Login

        function getAllUserRole() {
            var request = $http({
                method: "GET",
                url: $rootScope.serviceurl+"GetAllRoles?offset=0&limit=20",
                headers: { 'Content-Type': 'application/json', 'offset': '0', 'limit': '20' },
                beforeSend: function () {
                    $("#loading").show();
                },
            });
            return( request.then( handleSuccess, handleError ) );
        }
        // User Login


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