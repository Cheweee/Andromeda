app.controller('areaOfTrainingController', function ($scope, $q, $timeout, $mdDialog, $filter, service) {
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

    $scope.levelsOfHE = [];
    $scope.searchLevelOfHE = '';
    $scope.selectedLevelOfHE = '';

    $scope.getLevelsOfHE = function () {
        var deferred = $q.defer();
        service.getEntities('/CirriculumDevelopment/GetAreaOfTrainingLevelsOfHigherEducation'
            , model = { Search: $scope.searchLevelOfHE }).then(function (response) {
                if (response.data.Result) {
                    $scope.levelsOfHE = response.data.Entities;
                    deferred.resolve($scope.levelsOfHE);
                }
            });

        return deferred.promise;
    };

    $scope.departments = [];
    $scope.searchDepartment = '';
    $scope.selectedDepartment = null;

    $scope.getDepartments = function () {
        var deferred = $q.defer();
        service.getEntities('/CirriculumDevelopment/GetAreaOfTrainingDepartments'
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
        addOrEdit: service.addOrEdit,
        closeDialog: function () {
            service.closeDialog();
        }
    };

    $scope.loadDialog = function () {
        $scope.message = 'Загрузка данных';
        if (!$scope.settings.addOrEdit) {
            $scope.loading = true;
            service.getEntityById('/CirriculumDevelopment/GetAreaOfTraining', service.id)
                .then(function (response) {
                    if (response.data.Result) {
                        $scope.entity.Id = response.data.Entity.Id;
                        $scope.entity.Code = response.data.Entity.Code;
                        $scope.entity.Name = response.data.Entity.Name;
                        $scope.entity.ShortName = response.data.Entity.ShortName;
                        $scope.entity.DepartmentId = response.data.Entity.DepartmentId;
                        $scope.entity.Directionaly = response.data.Entity.Directionaly;
                        $scope.entity.LevelOfHigherEducationName = response.data.Entity.LevelOfHigherEducationName;
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
                        $scope.getLevelsOfHE().then(function () {
                            var rows = $scope.levelsOfHE;
                            for (i = 0; i < rows.length; i++) {
                                if ($scope.entity !== null) {
                                    if ($scope.entity.LevelOfHigherEducationName === rows[i].Name) {
                                        $scope.selectedLevelOfHE = rows[i];
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