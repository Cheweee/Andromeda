app.controller('workingCirriculumController', function ($scope, $q, $timeout, $mdDialog, $filter, service) {
    var result = false;

    $scope.closeDialog = function () {
        $mdDialog.hide();
    };

    $scope.academicDisciplines = service.createQuery('Code', '/CirriculumDevelopment/GetWorkingCirriculumAcademicDisciplines');

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
        addOrEdit: true,
        needToAddAreaOfTraining: false,
        createTimer: function () {
            return $timeout(function () {
                // loading
                $scope.settings.loading = false;
            }, 500);
        }
    };

    $scope.loadDialog = function () {
        $scope.message = 'Загрузка данных';
        var id = $cookies.get('entityId');
        if (id) {
            $scope.settings.addOrEdit = false;
            $scope.loading = true;
            service.getEntityById('/CirriculumDevelopment/GetAreaOfTraining', id)
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
                            $scope.settings.createTimer();
                        });
                        $scope.getAreasOfTraining().then(function () {
                            var rows = $scope.areasOfTrainings;
                            for (i = 0; i < rows.length; i++) {
                                if ($scope.entity !== null) {
                                    if ($scope.entity.AreaOfTrainingId === rows[i].Id) {
                                        $scope.selectedAreaOfTraining = rows[i];
                                    }
                                }
                            }
                            $scope.settings.createTimer();
                        });
                        $scope.getTypesOfEd().then(function () {
                            var rows = $scope.typesOfEd;
                            for (i = 0; i < rows.length; i++) {
                                if ($scope.entity !== null) {
                                    if ($scope.entity.TypeOfEducationName === rows[i]) {
                                        $scope.selectedTypeOfEd = rows[i];
                                    }
                                }
                            }
                            $scope.settings.createTimer();
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

    $scope.openUploadDialog = function (event) {
        $scope.settings.loading = true;
        $mdDialog.show({
            clickOutsideToClose: false,
            controller: 'uploadController',
            controllerAs: 'ctrl',
            focusOnOpen: true,
            targetEvent: event,
            templateUrl: 'uploadForm.html',
            fullscreen: $scope.fullscreen
        }).then(function () {
            $scope.entity = service.entity;
            $scope.selectedTypeOfEd = service.entity.TypeOfEducationName;
            $scope.getDepartments().then(function () {
                var rows = $scope.departments;
                for (i = 0; i < rows.length; i++) {
                    if ($scope.entity !== null) {
                        if ($scope.entity.DepartmentId === rows[i].Id) {
                            $scope.selectedDepartment = rows[i];
                        }
                    }
                }
            });
            $scope.getAreasOfTraining().then(function () {
                var rows = $scope.areasOfTrainings;
                for (i = 0; i < rows.length; i++) {
                    if ($scope.entity !== null) {
                        if ($scope.entity.AreaOfTrainingId === rows[i].Id) {
                            $scope.selectedAreaOfTraining = rows[i];
                        }
                    }
                }

                if (!$scope.selectedAreaOfTraining) {
                    $scope.settings.needToAddAreaOfTraining = true;
                    $scope.selectedAreaOfTraining = service.entity.AreaOfTraining;
                }
            });
            $scope.getTypesOfEd().then(function () {
                var rows = $scope.typesOfEd;
                for (i = 0; i < rows.length; i++) {
                    if ($scope.entity !== null) {
                        if ($scope.entity.TypeOfEducationName === rows[i]) {
                            $scope.selectedTypeOfEd = rows[i];
                        }
                    }
                }
            });

            $scope.academicDisciplines.entities = service.entity.AcademicDisciplines;

            $scope.settings.createTimer();
        });
    };
});