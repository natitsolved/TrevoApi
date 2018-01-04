angular
.module("authFront", ["ngCookies"])
.factory("myAuth", function ($http, $cookies, $cookieStore, $window, $location) {
  var factobj = {};

        /*********************************Admin Authorisation ***********************************/
  factobj.adminuserinfo = { loginstatus: false, id: "", username: "", accesstoken: "", isadmin: "", roleName: "", email: "", sinchTicket: "", imagePath :""};

        factobj.updateAdminUserinfo = function (obj) {           
            if(obj)
            {
                factobj.adminuserinfo = { loginstatus: true, username: obj.UserName, accesstoken: obj.accesstoken, isadmin: true, id: obj.UserKey, roleName: obj.RoleName, email: obj.Email, sinchTicket: obj.loginObject, imagePath: obj.ImagePath };
                localStorage.setItem("admin", JSON.stringify(factobj.adminuserinfo));
                return true;
            }
  
        };
        factobj.resetAdminUserinfo = function () {
            factobj.adminuserinfo = { loginstatus: false, id: "", username: "", accesstoken: "", isadmin: "", roleName: "", email: "", sinchTicket: "", imagePath :""};
            $window.localStorage["admin"]=null;
        };

        factobj.getAdminAuthorisation = function () {
            if ($window.localStorage["admin"] != undefined) {
                var obj = JSON.parse($window.localStorage["admin"]);
                if (obj)
                    return obj;
                else {
                    $location.path('/adminlogin/signin');
                    return null;
                }
            }
            else {
                $location.path('/adminlogin/signin');
                return null;
            }
        };

        factobj.getAdminNavlinks = function () {          
            var adminlogin = factobj.adminuserinfo.loginstatus;
            if (!adminlogin) {
                return false;
            } else {
                return factobj.adminuserinfo;
            }
        };

        factobj.isAdminLoggedIn = function () {
            var adminlogin = factobj.adminuserinfo.loginstatus;
            if (!adminlogin||adminlogin==null) {
                $location.path("/adminlogin/signin");
            } else {
                return true;
            }
        };

    return factobj;
    
})
