var app = angular.module("statmedclinic", [
        'ngRoute',
        'ngAnimate',
        'ngCookies',
        'ngSanitize',
        'ngTouch',
        'ui.router',
        'base64',
        'oc.lazyLoad',       
        'authFront',
        'ngToast',
        'ngFacebook',
        'angularFileUpload',
        'angularjs-dropdown-multiselect',
        'angularMoment',
    'ncy-angular-breadcrumb',
    'ngDialog',
    'ui.bootstrap.datetimepicker'


]);




app.run(['$rootScope', '$state', '$stateParams',
    function ($rootScope, $state, $stateParams, myAuth) {
        // Attach Fastclick for eliminating the 300ms delay between a physical tap and the firing of a click event on mobile browsers
      // FastClick.attach(document.body);

        // Set some reference to access them from any scope
        $rootScope.$state = $state;
        $rootScope.$stateParams = $stateParams;

        // GLOBAL APP SCOPE
        // set below basic information
        //$rootScope.sinchApplicationKey = "79de4c72-13b4-4519-99cb-1b74c5866cf8";
        $rootScope.sinchApplicationKey = "fafc801f-e590-4040-b8ff-1aacea6957e3";
        $rootScope.sinchAppSecret = "QNZBI5C6EkSHYMGQ8ah/ig==";
        $rootScope.serviceurl = "http://166.62.40.135:8089/";
          //$rootScope.serviceurl="http://localhost:49941/";
         /*$rootScope.siteurl = "http://localhost/nodejs/rentoanyapp/#/";*/
        $rootScope.siteurl = "http://166.62.40.135:8091/";
        $rootScope.app = {
            name: 'statmedclinic', // name of your project
            author: 'NITS', // author's name or company name
            //admindescription: 'LiveHelpout Admin', // brief description
            //frontdescription: 'LiveHelpout', // brief description
            description:'statmedclinic',
            keywords:'statmedclinic',
            version: '1.0', // current version
            year: ((new Date()).getFullYear()), // automatic current year (for copyright information)
            isMobile: (function () {// true if the browser is a mobile device
                var check = false;
                if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
                    check = true;
                };
                return check;
            })()
        };

    }]);

angular.module('statmedclinic').run(['$http',function($http){
   $http.defaults.headers.common.responsetype = 'json';
}])

angular.module('statmedclinic').filter('tel', function () {
    return function (tel) {
        if (!tel) { return ''; }

        var value = tel.toString().trim().replace(/^\+/, '');

        if (value.match(/[^0-9]/)) {
            return tel;
        }

        var country, city, number;

        switch (value.length) {
            case 10: // +1PPP####### -> C (PPP) ###-####
                country = 1;
                city = value.slice(0, 3);
                number = value.slice(3);
                break;

            case 11: // +CPPP####### -> CCC (PP) ###-####
                country = value[0];
                city = value.slice(1, 4);
                number = value.slice(4);
                break;

            case 12: // +CCCPP####### -> CCC (PP) ###-####
                country = value.slice(0, 3);
                city = value.slice(3, 5);
                number = value.slice(5);
                break;

            default:
                return tel;
        }

        if (country == 1) {
            country = "";
        }

        number = number.slice(0, 3) + '-' + number.slice(3);

        return (country + " (" + city + ") " + number).trim();
    };
});


// translate config
/*app.config(['$translateProvider',
    function ($translateProvider) {

        // prefix and suffix information  is required to specify a pattern
        // You can simply use the static-files loader with this pattern:
        $translateProvider.useStaticFilesLoader({
            prefix: 'app/assets/i18n/',
            suffix: '.json'
        });

        // Since you've now registered more then one translation table, angular-translate has to know which one to use.
        // This is where preferredLanguage(langKey) comes in.
        $translateProvider.preferredLanguage('en');

        // Store the language in the local storage
        $translateProvider.useLocalStorage();

    }]);*/

// Angular-Loading-Bar
// configuration
//app.config(['cfpLoadingBarProvider',
//    function (cfpLoadingBarProvider) {
//        cfpLoadingBarProvider.includeBar = true;
//        cfpLoadingBarProvider.includeSpinner = true;

//    }]);

//configuring toast messages
app.config(['ngToastProvider',
    function (ngToast) {
        ngToast.configure({
            verticalPosition: 'top',
            horizontalPosition: 'center'
        });

    }]);
//app.directive('timepicker', [

//  function() {
//      var link;
//      link = function(scope, element, attr, ngModel) {
//          console.log(scope[attr.ngModel]);
//          element = $(element);
//          element.datetimepicker({
//              format: 'YYYY-MM-DD HH:mm:ss',
//              defaultDate: scope[attr.ngModel]
//          });
//          element.on('dp.change', function(event) {
//              scope.$apply(function() {
//                  ngModel.$setViewValue(event.date._d);
//              });
//          });
//      };

//      return {
//          restrict: 'A',
//          link: link,
//          require: 'ngModel'
//      };
//  }
//])


app.config(['$stateProvider', '$urlRouterProvider', '$controllerProvider', '$compileProvider', '$filterProvider', '$provide', '$ocLazyLoadProvider', 'JS_REQUIRES','$locationProvider',
    function ($stateProvider, $urlRouterProvider, $controllerProvider, $compileProvider, $filterProvider, $provide, $ocLazyLoadProvider, jsRequires,$locationProvider) {

    app.controller = $controllerProvider.register;
     app.directive = $compileProvider.directive;
     app.filter = $filterProvider.register;
     app.factory = $provide.factory;
     app.service = $provide.service;
     app.constant = $provide.constant;
     app.value = $provide.value;

        // LAZY MODULES
        $ocLazyLoadProvider.config({
            debug: false,
            events: true,
            modules: jsRequires.modules
        });

///Remove Hash from URL//////
 //$locationProvider.html5Mode(true).hashPrefix('!');

    // APPLICATION ROUTES
    // -----------------------------------
    // For any unmatched url, redirect to /app/dashboard
        $urlRouterProvider.otherwise("/adminlogin");
        //$urlRouterProvider.otherwise("/admin/home");
    //
    // Set up the states

        $stateProvider
        //Login state
            .state('frontend', {
                url: '',
                templateUrl: 'app/views/app.html',
                abstract :true,
                resolve: loadSequence('footer','mCart'),

            })
            .state('frontend.index', {
                url: '/',
                resolve: loadSequence('home'),
                templateUrl: 'app/views/home.html',
                title: 'Home'
            })
            .state('frontend.login', {
                url: '/login?returnUrl',
                resolve: loadSequence('login'),
                templateUrl: 'app/views/login.html',
                title: 'Login'
            })
            .state('frontend.register', {
                url: '/register/:email',
                resolve: loadSequence('register'),
                templateUrl: 'app/views/register.html',
                title: 'Register'
            })
            .state('frontend.dashboard', {
                url: '/dashboard',
                resolve: loadSequence('dashboard'),
                templateUrl: 'app/views/dashboard.html',
                title: 'Dashboard'
            })
            .state('admin', {
                url: '/admin',
                templateUrl: 'app/views/admin/adminbase.html',
                abstract :true,
                resolve: loadSequence('admincss','adminjs')
            })
            .state('admin.index', {
                url: '/home',
                templateUrl: 'app/views/admin/home.html',
                title: 'Home',
                ncyBreadcrumb: {
                    label: 'Home page'
                }
            })
            .state('admin.changePassword', {
                url: '/changePassword',
                templateUrl: 'app/views/admin/changePassword.html',
                title: 'Change Password',
                ncyBreadcrumb: {
                    label: 'Change Password'
                }
            })
            .state('admin.categorylist', {
                url: '/categorylist',
                templateUrl: 'app/views/admin/categorylist.html',
                title: 'Category List',
                resolve: loadSequence('categorylist','naif.base64')
            })
            .state('admin.newslist', {
                url: '/newslist',
                templateUrl: 'app/views/admin/newslist.html',
                title: 'News List',
                resolve: loadSequence('newslist','ngCkeditor','naif.base64')
            })
            .state('admin.iconlist', {
                url: '/iconlist',
                templateUrl: 'app/views/admin/iconlist.html',
                title: 'Icon List',
                resolve: loadSequence('iconlist','naif.base64')
            })
            .state('admin.settings', {
                url: '/settings',
                templateUrl: 'app/views/admin/settings.html',
                title: 'Site Settings',
                resolve: loadSequence('settings')
            })

            .state('adminlogin', {
                url: '/adminlogin',
                templateUrl: 'app/views/admin/login.html',
                resolve: loadSequence('adminlogin',"admincss","adminjs"),
                title: 'Login'
            })
            .state('admin.userlist', {
                url: '/userlist',
                templateUrl: 'app/views/admin/user.html',
                title: 'User',
                resolve: loadSequence('userlist'),
                ncyBreadcrumb: {
                    parent: 'admin.index',
                    label: 'User List'
                }
            })
            .state('admin.patientList', {
                url: '/patientList',
                templateUrl: 'app/views/admin/patient.html',
                title: 'Patient',
                resolve: loadSequence('patientList','ngFileUpload'),
                ncyBreadcrumb: {
                    parent: 'admin.index',
                    label: 'Patient List'
                }
            })
            .state('admin.doctorList', {
                url: '/doctorList',
                templateUrl: 'app/views/admin/doctor.html',
                title: 'Doctor',
                resolve: loadSequence('doctorList','ngFileUpload'),
                ncyBreadcrumb: {
                    parent: 'admin.index',
                    label: 'Doctor List'
                }
            })
            .state('admin.staffList', {
                url: '/staffList',
                templateUrl: 'app/views/admin/staff.html',
                title: 'Staff',
                resolve: loadSequence('staffList','ngFileUpload'),
                ncyBreadcrumb: {
                    parent: 'admin.index',
                    label: 'Staff List'
                }
            })

            .state('admin.useradd', {
                url: '/usermanage/:userId',
                templateUrl: 'app/views/admin/useradd.html',
                title: 'Manage User',
                resolve: loadSequence('useradd','ngFileUpload'),
                ncyBreadcrumb: {
                    parent: 'admin.userlist',
                    label: 'Manage User'
                }
            })
            .state('admin.patientadd', {
                url: '/patientmanage/:userId',
                templateUrl: 'app/views/admin/patientadd.html',
                title: 'Manage User',
                resolve: loadSequence('patientadd','ngFileUpload'),
                ncyBreadcrumb: {
                    parent: 'admin.patientList',
                    label: 'Manage Patient'
                }
            })
            .state('admin.doctoradd', {
                url: '/doctormanage/:userId',
                templateUrl: 'app/views/admin/doctoradd.html',
                title: 'Manage User',
                resolve: loadSequence('doctoradd','ngFileUpload'),
                ncyBreadcrumb: {
                    parent: 'admin.doctorList',
                    label: 'Manage Doctor'
                }
            })
            .state('admin.staffadd', {
                url: '/staffmanage/:userId',
                templateUrl: 'app/views/admin/staffadd.html',
                title: 'Manage Staff',
                resolve: loadSequence('staffadd','ngFileUpload'),
                ncyBreadcrumb: {
                    parent: 'admin.staffList',
                    label: 'Manage Staff'
                }
            })

            .state('admin.cmslist', {
                url: '/cmslist',
                templateUrl: 'app/views/admin/cms.html',
                title: 'CMS',
                resolve: loadSequence('cmslist','ngCkeditor'),
                ncyBreadcrumb: {
                    parent: 'admin.index',
                    label: 'CMS List'
                }
            })

            .state('admin.cmsadd', {
                url: '/cmsmanage/:pageid',
                templateUrl: 'app/views/admin/cmsadd.html',
                title: 'CMS',
                resolve: loadSequence('cmslist','summernote'),
                ncyBreadcrumb: {
                    parent: 'admin.cmslist',
                    label: 'CMS List'
                }
            })

            .state('admin.faqlist', {
                url: '/faqlist',
                templateUrl: 'app/views/admin/faq.html',
                title: 'FAQ',
                resolve: loadSequence('faqlist'),
                ncyBreadcrumb: {
                    parent: 'admin.index',
                    label: 'FAQ List'
                }
            })

            .state('admin.faqadd', {
                url: '/faqmanage/:faqId',
                templateUrl: 'app/views/admin/faqadd.html',
                title: 'FAQ',
                resolve: loadSequence('faqlist'),
                ncyBreadcrumb: {
                    parent: 'admin.faqlist',
                    label: 'FAQ List'
                }
            })
            .state('admin.doctoravailability', {
                url: '/doctoravailability',
                templateUrl: 'app/views/admin/doctoraval.html',
                title: 'Doctor Availability',
                resolve: loadSequence('doctoravailability'),
                ncyBreadcrumb: {
                    parent: 'admin.index',
                    label: 'Doctor Availability'
                }
            })
            .state('admin.availibility', {
                url: '/availibility',
                templateUrl: 'app/views/admin/availibility.html',
                title: 'Doctor Availability',
                resolve: loadSequence('doctoravailability'),
                ncyBreadcrumb: {
                    parent: 'admin.doctoravailabilityadd',
                    label: 'Doctor Availability'
                }
            })

            .state('admin.doctoravailabilityadd', {
                url: '/availabilitymanage/:userId/:weekday:scheduleid',
                templateUrl: 'app/views/admin/doctoravalAdd.html',
                title: 'Add Availibility',
                resolve: loadSequence('doctoravailability'),
                ncyBreadcrumb: {
                    parent: 'admin.doctoravailability',
                    label: 'Availability'
                }
            })

        .state('admin.questionnaire', {
            url: '/questionnairelist',
            templateUrl: 'app/views/admin/questionnare.html',
            title: 'Questionnare',
            resolve: loadSequence('questionnaire'),
            ncyBreadcrumb: {
                parent: 'admin.index',
                label: 'Questionarre List'
            }
        })
        .state('admin.questionnaireadd', {
            url: '/questionnaremanage/:questionId',
            templateUrl: 'app/views/admin/questionadd.html',
            title: 'Manage Questions',
            resolve: loadSequence('questionnaire'),
            ncyBreadcrumb: {
                parent: 'admin.questionnaire',
                label: 'Manage Questions'
            }
        })
         .state('admin.sliderList', {
             url: '/sliderList',
             templateUrl: 'app/views/admin/slider.html',
             title: 'Slider',
             resolve: loadSequence('slider', 'ngFileUpload'),
             ncyBreadcrumb: {
                 parent: 'admin.index',
                 label: 'Slider List'
             }
         })
         .state('admin.slidereadd', {
             url: '/sliderManage/:ImageID',
             templateUrl: 'app/views/admin/slideradd.html',
             title: 'Manage Slider Images',
             resolve: loadSequence('slider', 'ngFileUpload'),
             ncyBreadcrumb: {
                 parent: 'admin.sliderList',
                 label: 'Manage Slider Image'
             }
         })
             .state('admin.healthPackageList', {
                 url: '/healthPackageList',
                 templateUrl: 'app/views/admin/healthPackage.html',
                 title: 'Health Subscription Package',
                 resolve: loadSequence('healthPackage', 'ngFileUpload'),
                 ncyBreadcrumb: {
                     parent: 'admin.index',
                     label: 'Health Subscription Package List'
                 }
             })
             .state('admin.healthPckgAdd', {
                 url: '/healthPckgManage/:PackageID',
                 templateUrl: 'app/views/admin/healthPckgAdd.html',
                 title: 'Manage Health Packages',
                 resolve: loadSequence('healthPackage', 'ngFileUpload'),
                 ncyBreadcrumb: {
                     parent: 'admin.healthPackageList',
                     label: 'Manage Health Packages'
                 }
             })
             .state('admin.appointmentList', {
                 url: '/appointmentList',
                 templateUrl: 'app/views/admin/appointmentList.html',
                 title: 'AppointMent',
                 resolve: loadSequence('healthPackage'),
                 ncyBreadcrumb: {
                     parent: 'admin.index',
                     label: 'AppointMent List'
                 }
             })
            .state('admin.manageAppointMent', {
                url: '/manageAppointMent/:appointmentId',
                templateUrl: 'app/views/admin/appointmentManage.html',
                title: 'Manage Appointment',
                resolve: loadSequence('healthPackage'),
                ncyBreadcrumb: {
                    parent: 'admin.appointmentList',
                    label: 'Appointment Details'
                }
            })
             .state('admin.paymentList', {
                 url: '/paymentList',
                 templateUrl: 'app/views/admin/paymentList.html',
                 title: 'Payment',
                 resolve: loadSequence('paymentDetails'),
                 ncyBreadcrumb: {
                     parent: 'admin.index',
                     label: 'Payment List'
                 }
             })
            .state('admin.userPaymentInfo', {
                url: '/userPaymentInfo',
                templateUrl: 'app/views/admin/paymentInfo.html',
                params: {
                    'userId': null,
                },
                title: 'Payment Details',
                resolve: loadSequence('paymentDetails'),
                ncyBreadcrumb: {
                    parent: 'admin.index',
                    label: 'Payment List'
                }
            })
         .state('staff', {
             url: '/staff',
             templateUrl: 'app/views/staff/staffbase.html',
             abstract: true,
             resolve: loadSequence('staffcss', 'staffjs')
         })
        .state('staff.index', {
            url: '/home',
            templateUrl: 'app/views/staff/home.html',
            title: 'Home',
            ncyBreadcrumb: {
                label: 'Home page'
            }
        })
        .state('staff.changePassword', {
            url: '/changePassword',
            templateUrl: 'app/views/staff/changePassword.html',
            title: 'Change Password',
            ncyBreadcrumb: {
                label: 'Change Password'
            }
        })
         .state('staff.chat', {
             url: '/chat',
             templateUrl: 'app/views/staff/chat.html',
             title: 'Chat',
             resolve: loadSequence('ngEmoticons', 'angular.filter', 'staffDoctorList'),
             ncyBreadcrumb: {
                 label: 'Chat'
             }
         })

         .state('staff.doctorList', {
             url: '/doctorList',
             templateUrl: 'app/views/staff/doctorList.html',
             title: 'Doctor',
             resolve: loadSequence('staffDoctorList'),
             ncyBreadcrumb: {
                 parent: 'staff.index',
                 label: 'Doctor List'
             }
         })
         .state('staff.clientList', {
             url: '/clientList',
             templateUrl: 'app/views/staff/clientList.html',
             title: 'Doctor',
             resolve: loadSequence('staffDoctorList'),
             ncyBreadcrumb: {
                 parent: 'staff.index',
                 label: 'Doctor List'
             }
         })

            .state('staff.docDetails', {
                url: '/docDetailView/:docId',
                templateUrl: 'app/views/staff/docDetails.html',
                title: 'Doctor',
                resolve: loadSequence('staffDoctorList'),
                ncyBreadcrumb: {
                    parent: 'staff.doctorList',
                    label: 'Doctor Details'
                }
            })
         .state('staff.clientDetails', {
             url: '/staffDetailView/:staffId',
             templateUrl: 'app/views/staff/staffDetails.html',
             title: 'Patient',
             resolve: loadSequence('staffDoctorList'),
             ncyBreadcrumb: {
                 parent: 'staff.clientList',
                 label: 'Patient Details'
             }
         })
        .state('staff.appointMentList', {
            url: '/appointMentList',
            templateUrl: 'app/views/staff/appointMentList.html',
            title: 'AppointMent',
            resolve: loadSequence('staffDoctorList'),
            ncyBreadcrumb: {
                parent: 'staff.index',
                label: 'AppointMent List'
            }
        })
       .state('staff.manageAppointMent', {
           url: '/manageAppointMent/:appointmentId',
           templateUrl: 'app/views/staff/appointmentManage.html',
           title: 'Manage Appointment',
           resolve: loadSequence('staffDoctorList'),
           ncyBreadcrumb: {
               parent: 'staff.appointMentList',
               label: 'Appointment Details'
           }
       })
         
        ;



























        function loadSequence() {
            var _args = arguments;
            return {
                deps: ['$ocLazyLoad', '$q',
                    function ($ocLL, $q) {
                        var promise = $q.when(1);
                        for (var i = 0, len = _args.length; i < len; i++) {
                            promise = promiseThen(_args[i]);
                        }
                        return promise;

                        function promiseThen(_arg) {
                            if (typeof _arg == 'function')
                                return promise.then(_arg);
                            else
                                return promise.then(function () {
                                    var nowLoad = requiredData(_arg);
                                    if (!nowLoad)
                                        return $.error('Route resolve: Bad resource name [' + _arg + ']');
                                    return $ocLL.load(nowLoad);
                                });
                        }

                        function requiredData(name) {
                            if (jsRequires.modules)
                                for (var m in jsRequires.modules)
                                    if (jsRequires.modules[m].name && jsRequires.modules[m].name === name)
                                        return jsRequires.modules[m];
                            return jsRequires.scripts && jsRequires.scripts[name];
                        }
                    }]
            };
        }

  }]);
