'use strict';
/**
 * controllers used for the login
 */
app.controller('questionnareCtrl', function ($rootScope, $scope, $http, $location, myAuth, $cookieStore, $timeout, questionnareService, $state, $stateParams) {


    if ($stateParams.questionId == undefined) {
        $stateParams.questionId = "add";
    }
    $scope.option = [];

    $scope.GetAllQuestionnare = function () {
        $("#default_loader").show();
        questionnareService.getAllquestionnare().then(
            function (data) {
                if (data.length > 0) {
                    $scope.allQuestions = data;
                    $timeout(function () {

                        $scope.table = angular.element('#questionnareList').DataTable({
                            "paging": true,
                            "lengthChange": false,
                            "searching": true,
                            "ordering": true,
                            "info": true,
                            "autoWidth": false
                        });
                    }, 3000, false);
                }
                $("#default_loader").hide();

            },
            function (errorMessage) {
                $("#default_loader").hide();
                $rootScope.showToast(errorMessage, "alert alert-danger");
            });

    }
    $scope.addQuestionnare = function () {
        $("#default_loader").show();
        if ($stateParams.questionId == "add") {
            $scope.item = {
                "Questions": '',
                "InputType": '',
                "NoOfInputs": '',
                "IsMultipleAnswerAllowed": '',
                "QuestionOptions": '',
                "QId":''
            };
            $("#default_loader").hide();
        } else {
            $scope.questionId = $stateParams.questionId;
            questionnareService.getQuestionDetailsById($scope.questionId).then(
                function (data) {
                    if (data != null || data != undefined) {
                        var inputType = '';
                        if (data.InputType == "tb") {
                            inputType = "0";
                        }
                        else if (data.InputType == "cb") {
                            inputType = "1";
                        }
                        else if (data.InputType = "rb") {
                            inputType = "2";
                        }
                        $scope.item = {
                            "Questions": data.Questions,
                            "InputType": inputType,
                            "NoOfInputs": data.NoOfInputs,
                            "IsMultipleAnswerAllowed": data.IsMultipleAnswerAllowed,
                            "QuestionOptions": data.QuestionOptions,
                            "QId":$scope.questionId
                        };
                        if ($scope.item.InputType == "0") {
                            $scope.isShow = false;
                        }
                        else {
                            $scope.isShow = true;
                            $scope.createInputTypes();
                            for (var i = 0; i < $scope.choices.length; i++)
                            {
                                var key = $scope.choices[i];
                                var value = $scope.item.QuestionOptions[i];
                                $scope.option.push(value)
                            }
                            console.log($scope.option);
                          
                        }
                    }
                    $("#default_loader").hide();
                },
                function (errorMessage) {
                    $("#default_loader").hide();
                    $rootScope.showToast(errorMessage, "alert alert-danger");

                });
        }

    }
    $scope.GetAllQuestionnare();
    $scope.addQuestionnare();






    $scope.onInputTypeChange = function (selectedInputType) {
        if (selectedInputType == "1" || selectedInputType == "2") {
            $scope.isShow = true;
        }
        else {
            $scope.isShow = false;
        }
    }

    $scope.createInputTypes = function () {
        $scope.choices = [];
        for (var i = 1; i <= $scope.item.NoOfInputs; i++) {
            $scope.choices.push(i);
        }
    }

    $scope.backFunction = function () {
        $location.path('/admin/questionnairelist');
    }


    $scope.save = function () {
        $("#default_loader").show();
        if ($scope.item.InputType == "2" && $scope.item.IsMultipleAnswerAllowed == "1") {
            $rootScope.showToast("Please select check box for allowing multiple selection of options.", "alert alert-danger");
            $("#default_loader").hide();
        }
        else {
            if ($scope.item.InputType == "0") {
                $scope.item.InputType = "tb";
            }
            else if ($scope.item.InputType == "1") {
                $scope.item.InputType = "cb";
                $scope.item.QuestionOptions = Object.values($scope.option);
            }
            else if ($scope.item.InputType == "2") {
                $scope.item.InputType = "rb";
                $scope.item.QuestionOptions = Object.values($scope.option);
            }
            if ($scope.questionId == "" || $scope.questionId == undefined) {
                questionnareService.insertQuestion($scope.item).then(
                         function (data) {
                             $("#default_loader").hide();
                             if (data != null || data != undefined) {
                                 $location.path("/admin/questionnairelist");
                             }
                         },
                         function (errorMessage) {
                             $("#default_loader").hide();
                             $rootScope.showToast(errorMessage, "alert alert-danger");
                         });
            }
            else {
                questionnareService.questionEdit($scope.item).then(
                         function (data) {
                             $("#default_loader").hide();
                             if (data != null || data != undefined) {
                                 $location.path("/admin/questionnairelist");
                             }
                         },
                         function (errorMessage) {
                             $("#default_loader").hide();
                             $rootScope.showToast(errorMessage, "alert alert-danger");
                         });
            }
        }
    }

    $scope.deleteQuestion = function (questionId) {
        if (window.confirm("Want to delete?")) {
            $("#default_loader").show();
            questionnareService.questionDelete(questionId).then(function (data) {
                if (data != null && data != undefined) {
                    $("#default_loader").hide();
                    $state.go("admin.questionnaire", {}, { reload: true });
                }
            }, function (errorMessage) { $("#default_loader").hide(); $rootScope.showToast(errorMessage, "alert alert-danger"); });
        }
        
    }

   
});

