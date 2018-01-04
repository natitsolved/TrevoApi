'use strict';

/**
 * Config constant
 */
app.constant('APP_MEDIAQUERY', {
    'desktopXL': 1200,
    'desktop': 992,
    'tablet': 768,
    'mobile': 480
});
app.constant('JS_REQUIRES', {
    //*** Scripts
    scripts: {
        'home': ['app/Controllers/homeCtrl.js', 'app/directives/countdownDirective.js'],
        'login': ['app/Controllers/loginCtrl.js'],
        'register': ['app/Controllers/registerCtrl.js'],
        'activation': ['app/Controllers/activationCtrl.js'],
        'dashboard': ['app/Controllers/dashboardCtrl.js', 'app/Controllers/leftbarCtrl.js'],
        'admincss': ['app/assets/admin/font-awesome/css/font-awesome.min.css', 'app/assets/admin/css/ionicons.min.css', 'app/assets/admin/css/AdminLTE.css', 'app/assets/admin/plugins/iCheck/square/blue.css', 'app/assets/admin/plugins/iCheck/square/blue.css', 'app/assets/admin/css/skins/_all-skins.min.css', 'app/Controllers/admin/headerCtrl.js', 'app/Controllers/admin/leftbarCtrl.js'],
        'adminjs': ['app/assets/admin/plugins/slimScroll/jquery.slimscroll.min.js', 'app/assets/admin/plugins/fastclick/fastclick.js', 'app/assets/admin/js/app.min.js', 'app/assets/admin/plugins/iCheck/icheck.min.js', 'app/assets/admin/js/demo.js', 'app/Controllers/admin/headerCtrl.js', 'app/Controllers/admin/leftbarCtrl.js'],
        'frontend': ["app/assets/css/site.css", "app/assets/css/reset.css"],
        'adminlogin': ['app/Controllers/admin/loginCtrl.js'],
        'newslist': ['app/Controllers/admin/newslistCtrl.js'],
        'categorylist': ['app/Controllers/admin/categorylistCtrl.js'],
        'footer': ["app/Controllers/footerCtrl.js"],
        'owlCaraousl': ['https://owlcarousel2.github.io/OwlCarousel2/assets/owlcarousel/assets/owl.carousel.min.css', 'https://owlcarousel2.github.io/OwlCarousel2/assets/owlcarousel/owl.carousel.js'],
        'userlist': ['app/Controllers/admin/userCtrl.js', 'app/service/userService.js'],
        'useradd': ['app/Controllers/admin/useraddCtrl.js', 'app/service/userService.js', 'app/service/userRoleService.js'],
        'cmslist': ['app/Controllers/admin/cmsCtrl.js', 'app/service/cmsService.js', 'app/Controllers/admin/cmsAddCtrl.js'],
        'patientList': ['app/Controllers/admin/patientCtrl.js', 'app/service/userService.js'],
        'doctorList': ['app/Controllers/admin/doctorCtrl.js', 'app/service/userService.js'],
        'patientadd': ['app/Controllers/admin/patientaddCtrl.js', 'app/service/userService.js', 'app/service/userRoleService.js'],
        'doctoradd': ['app/Controllers/admin/doctoreaddCtrl.js', 'app/service/userService.js', 'app/service/userRoleService.js'],
        'staffList': ['app/Controllers/admin/staffCtrl.js', 'app/service/userService.js'],
        'staffadd': ['app/Controllers/admin/staffaddCtrl.js', 'app/service/userService.js', 'app/service/userRoleService.js'],
        'faqlist': ['app/Controllers/admin/faqCtrl.js', 'app/service/faqService.js', 'app/Controllers/admin/faqAddCtrl.js'],
        'doctoravailability': ['app/Controllers/admin/doctorAvailibilityCtrl.js', 'app/service/doctorAvailibilityService.js', 'app/Controllers/admin/doctorAvailibilityAddCtrl.js'],
        'paymentDetails': ['app/Controllers/admin/paymentCtrl.js', 'app/service/paymentService.js', 'app/Controllers/admin/paymentInfoCtrl.js'],
        'questionnaire': ['app/Controllers/admin/questionnareCtrl.js', 'app/service/questionnareService.js'],
        'slider': ['app/Controllers/admin/sliderCtrl.js', 'app/service/sliderService.js'],
        'healthPackage': ['app/Controllers/admin/healthPckgCtrl.js', 'app/service/healthPckgService.js', 'app/service/staffService.js', 'app/Controllers/admin/appointmentCtrl.js'],
        'staffjs': ['app/assets/staff/js/jquery-ui.custom.min.js','app/assets/staff/js/fullcalendar.min.js','app/assets/staff/js/bootstrap-select.min.js','app/assets/staff/plugins/slimScroll/jquery.slimscroll.min.js', 'app/assets/staff/plugins/fastclick/fastclick.js', 'app/assets/staff/js/app.min.js', 'app/assets/staff/plugins/iCheck/icheck.min.js', 'app/assets/staff/js/demo.js', 'app/Controllers/staff/headerCtrl.js', 'app/Controllers/staff/leftbarCtrl.js', 'app/Controllers/staff/chatCtrl.js', 'app/assets/staff/js/calendar.js', 'app/assets/staff/js/custom.js', 'app/assets/staff/js/editors.js', 'app/assets/staff/js/forms.js', 'app/assets/staff/js/jquery.mCustomScrollbar.concat.min.js', 'app/assets/staff/js/jquery.mCustomScrollbar.js', 'app/assets/staff/js/stats.js', 'app/assets/staff/js/tables.js'],
        'staffcss': ['app/assets/staff/css/buttons.css', 'app/assets/staff/css/calendar.css', 'app/assets/staff/css/forms.css', 'app/assets/staff/css/jquery.mCustomScrollbar.css', 'app/assets/staff/css/jquery.mCustomScrollbar.min.css', 'app/assets/staff/css/stats.css', 'app/assets/staff/css/styles.css', 'app/assets/admin/font-awesome/css/font-awesome.min.css', 'app/assets/staff/css/bootstrap-theme.css', 'app/assets/staff/css/styles.css'],
        'staffDoctorList': ['app/Controllers/staff/doctorCtrl.js', 'app/service/staffService.js', 'app/Controllers/staff/clientCtrl.js', 'app/Controllers/staff/appointMentCtrl.js'],
    },

    //*** angularJS Modules

    modules: [{
        name: 'angularMoment',
        files: ['vendor/moment/angular-moment.min.js']
    },
        {
            name: 'datatables',
            files: [
                'bower_components/angular-datatables/jquery.dataTables.min.js',
                'bower_components/angular-datatables/dataTables.bootstrap.js',
                'bower_components/angular-datatables/angular-datatables.js', 'bower_components/angular-datatables/dataTables.bootstrap.css']
        },
        {
            name: 'ngMap',
            files: ['bower_components/ngmap/build/scripts/ng-map.min.js', 'http://maps.google.com/maps/api/js?libraries=placeses,visualization,drawing,geometry,places']
        },
        {
            name: 'mCart',
            files: ['app/cartFactory.js']
        },
        {
            name: 'ngCkeditor',
            files: ['bower_components/ng-ckeditor/libs/ckeditor/ckeditor.js', 'bower_components/ng-ckeditor/ng-ckeditor.js', 'bower_components/ng-ckeditor/ng-ckeditor.css']
        },
        {
            name: 'naif.base64',
            files: ['bower_components/angular-base64-upload/src/angular-base64-upload.js']
        },
        {
            name: 'cgNotify',
            files: ['bower_components/angular-notify/angular-notify.js', 'bower_components/angular-notify/angular-notify.css']
        },
        {
            name: 'ng-bootstrap-datepicker',
            files: ['bower_components/angular-bootstrap-datepicker.js', 'bower_components/angular-bootstrap-datepicker.css']
        },
        {
            name: 'timer',
            files: ['bower_components/humanize-duration/humanize-duration.js', 'bower_components/angular-timer/dist/angular-timer.min.js']
        },
        {
            name: 'ui.timepicker',
            files: ['http://recras.github.io/angular-jquery-timepicker/bower_components/jquery-timepicker-jt/jquery.timepicker.js', 'http://recras.github.io/angular-jquery-timepicker/javascripts/timepickerdirective.min.js', 'http://recras.github.io/angular-jquery-timepicker/bower_components/jquery-timepicker-jt/jquery.timepicker.css']
        },
        {
            name: '720kb.socialshare',
            files: ['https://raw.githubusercontent.com/720kb/angular-socialshare/master/dist/angular-socialshare.min.js']
        },
        {
            name: 'ngFileUpload',
            files: ['app/assets/js/ng-file-upload-shim.min.js', 'app/assets/js/ng-file-upload.min.js']
        },
         {
             name: 'ngEmoticons',
             files: ['node_modules/ng-emoticons/src/ng-emoticons.js', 'node_modules/ng-emoticons/src/ng-emoticons.css']
         },
         {
             name: 'angular.filter',
             files: ['node_modules/angular-filter/dist/angular-filter.js']
         },
        {
            name: 'summernote',
            files: ['app/assets/admin/js/summernote.js', 'app/assets/admin/js/angular-summernote.js']
        },
         //{
         //    name: 'ui.bootstrap.datetimepicker',
         //    files: ['node_modules/angular-bootstrap-datetimepicker/src/js/datetimepicker.js', 'node_modules/angular-bootstrap-datetimepicker/src/js/datetimepicker.templates.js', 'node_modules/angular-bootstrap-datetimepicker/src/css/datetimepicker.css']
         //},

    ]
});
