app.controller('workingCirriculumController', function ($scope, $q, $timeout, $mdDialog, $filter, service) {
    var result = false;
    var createTimer = function () {
        $timeout(function () {
            // loading
            $scope.loading = false;
        }, 500);
    };

    $scope.closeDialog = function () {
        $mdDialog.hide();
    };

    $scope.typesOfEd = [];
    $scope.searchTypeOfEd = '';
    $scope.selectedTypeOfEd = '';

    $scope.getTypesOfEd = function () {
        var deferred = $q.defer();
        service.getEntities('/CirriculumDevelopment/GetWorkingCirriculumTypesOfEducation'
            , model = { Search: $scope.searchTypeOfEd }).then(function (response) {
                if (response.data.Result) {
                    $scope.typesOfEd = response.data.Entities;
                    deferred.resolve($scope.typesOfEd);
                }
            });

        return deferred.promise;
    };

    $scope.areasOfTrainings = [];
    $scope.searchAreaOfTraining = '';
    $scope.selectedAreaOfTraining = null;

    $scope.getAreasOfTraining = function () {
        var deferred = $q.defer();
        service.getEntities('/CirriculumDevelopment/GetWorkingCirriculumAreaOfTrainings'
            , model = { Search: $scope.searchAreaOfTraining }).then(function (response) {
                if (response.data.Result) {
                    $scope.areasOfTrainings = response.data.Entities;
                    deferred.resolve($scope.areasOfTrainings);
                }
            });

        return deferred.promise;
    };

    $scope.departments = [];
    $scope.searchDepartment = '';
    $scope.selectedDepartment = null;

    $scope.getDepartments = function () {
        var deferred = $q.defer();
        service.getEntities('/CirriculumDevelopment/GetWorkingCirriculumDepartments'
            , model = { Search: $scope.searchDepartment }).then(function (response) {
                if (response.data.Result) {
                    $scope.departments = response.data.Entities;
                    deferred.resolve($scope.departments);
                }
            });

        return deferred.promise;
    };

    $scope.entity = {
        Id: null,
        FacultyId: null,
        Name: '',
        ShortName: '',
        FacultyName: '',
        Directionaly: '',
        Code: 0
    };

    $scope.settings = {
        delay: 500,
        loading: false,
        message: 'Загрузка данных',
        addition: false,
        addOrEdit: null,
        closeDialog: function () {
            service.closeDialog();
        }
    };

    $scope.loadDialog = function () {
        $scope.message = 'Загрузка данных';
        $scope.settings.addOrEdit = $cookies.get('addOrEdit');
        if (!$scope.settings.addOrEdit) {
            var wcId = $cookies.get('entityId');
            $scope.loading = true;
            service.getEntityById('/CirriculumDevelopment/GetAreaOfTraining', wcId)
                .then(function (response) {
                    if (response.data.Result) {
                        $scope.entity.Id = response.data.Entity.Id;
                        $scope.entity.StartTraining = response.data.Entity.StartTraining;
                        $scope.entity.TrainingPeriod = response.data.Entity.TrainingPeriod;
                        $scope.entity.EducationalStandart = response.data.Entity.EducationalStandart;
                        $scope.entity.AreaOfTrainingId = response.data.Entity.AreaOfTrainingId;
                        $scope.entity.TypeOfEducationName = response.data.Entity.TypeOfEducationName;
                        $scope.entity.DepartmentId = response.data.Entity.DepartmentId;
                        $scope.getDepartments().then(function () {
                            var rows = $scope.departments;
                            for (i = 0; i < rows.length; i++) {
                                if ($scope.entity !== null) {
                                    if ($scope.entity.DepartmentId === rows[i].Id) {
                                        $scope.selectedDepartment = rows[i];
                                    }
                                }
                            }
                            createTimer();
                        });
                        $scope.getAreasOfTraining().then(function () {
                            var rows = $scope.areasOfTrainings;
                            for (i = 0; i < rows.length; i++) {
                                if ($scope.entity !== null) {
                                    if ($scope.entity.AreaOfTrainingId === rows[i].Id) {
                                        $scope.selectedTypeOfEd = rows[i];
                                    }
                                }
                            }
                            createTimer();
                        });
                        $scope.getTypesOfEd().then(function () {
                            var rows = $scope.typesOfEd;
                            for (i = 0; i < rows.length; i++) {
                                if ($scope.entity !== null) {
                                    if ($scope.entity.TypeOfEducationName === rows[i].Name) {
                                        $scope.selectedAreaOfTraining = rows[i];
                                    }
                                }
                            }
                            createTimer();
                        });
                    }
                });
        }
    };

    $scope.clearFields = function () {
        if (!$scope.settings.addOrEdit) {
            $scope.loadDialog();
            return;
        }
        $scope.entity.Name = '';
        $scope.entity.ShortName = '';
        $scope.entity.DepartmentName = '';
        $scope.entity.DepartmentId = null;
        $scope.selectedDepartment = null;
        $scope.entity.Code = 0;
    };

    $scope.confirm = function () {
        var departmentId = null;
        if ($scope.selectedDepartment) {
            departmentId = $scope.selectedDepartment.Id;
        }
        var levelOfHEName = '';
        if ($scope.selectedLevelOfHE) {
            levelOfHEName = $scope.selectedLevelOfHE.Name;
        }
        var entity = {
            Id: $scope.entity.Id,
            Name: $scope.entity.Name,
            ShortName: $scope.entity.ShortName,
            Code: $scope.entity.Code,
            Directionaly: $scope.entity.Directionaly,
            LevelOfHigherEducationName: levelOfHEName,
            DepartmentId: departmentId
        };

        $scope.message = 'Сохранение изменений';
        $scope.loading = true;
        result = true;
        var method = service.addOrEdit ? "/CirriculumDevelopment/AddAreaOfTraining" : "/CirriculumDevelopment/ModifyAreaOfTraining";
        service.addOrEditEntity(method, entity)
            .then(function (response) {
                if (response.data.Result) {
                    $scope.closeDialog();
                }
            });
    };
});