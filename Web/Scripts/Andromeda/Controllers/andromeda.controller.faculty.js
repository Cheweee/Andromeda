app.controller('facultyController', function ($scope, $q, $timeout, $mdDialog, $filter, service) {
    var result = false;
    var createTimer = function () {
        $timeout(function () {
            $scope.loading = false;
        }, 500);
    };

    $scope.closeDialog = function () {
        $mdDialog.hide();
    };

    $scope.getNotFacultyDepartments = function () {
        var deferred = $q.defer();
        service.getEntities('/References/GetNotFacultyDepartments'
            , model = { Search: $scope.settings.searchDepartment }).then(function (response) {
                if (response.data.Result) {
                    var rows = response.data.Entities;
                    for (i = 0; i < $scope.entity.departments.length; i++) {
                        var index = rows.findIndex(o => o.Id === $scope.entity.departments[i].Id);
                        if (index >= 0) {
                            rows.splice(index, 1);
                        }
                    }
                    deferred.resolve(rows);
                }
            });

        return deferred.promise;
    };

    $scope.getFacultyDepartments = function () {
        service.getEntities('/References/GetFacultyDepartments'
            , model = { SearchId: $scope.entity.Id}).then(function (response) {
                if (response.data.Result) {
                    $scope.entity.departments = response.data.Entities;
                }
                $scope.loading = false;
            });
    };

    $scope.entity = {
        Id: null,
        Name: '',
        ShortName: '',
        IsFaculty: true,
        departments: []
    };

    $scope.settings = {
        searchDepartment: '',
        department: null,
        delay: 500,
        loading: false,
        message: 'Загрузка данных',
        addition: false,
        addOrEdit: service.addOrEdit,
        transformChip: service.transformChip
    };

    $scope.loadDialog = function () {
        $scope.message = 'Загрузка данных';
        if (!$scope.settings.addOrEdit) {
            $scope.loading = true;
            service.getEntityById('/References/GetFaculty', service.id)
                .then(function (args) {
                    if (args.data.Result) {
                        $scope.entity.Id = args.data.Entity.Id;
                        $scope.entity.Name = args.data.Entity.Name;
                        $scope.entity.ShortName = args.data.Entity.ShortName;
                        $scope.getFacultyDepartments();
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
        $scope.entity.departments = [];
    };

    $scope.confirm = function () {
        var entity = {
            Id: $scope.entity.Id,
            IsFaculty: true,
            Name: $scope.entity.Name,
            ShortName: $scope.entity.ShortName
        };

        $scope.message = 'Сохранение изменений';
        $scope.loading = true;
        result = true;
        var method = service.addOrEdit ? "/References/AddFaculty" : "/References/ModifyFaculty";
        service.addOrEditEntity(method, entity)
            .then(function (response) {
                if (response.data.Result) {
                    if (service.addOrEdit) {
                        $scope.entity.Id = response.data.Id;
                    }
                    if ($scope.entity.departments.length) {
                        service.changeEntities('/References/SaveFacultyDepartments', model = {
                            NewId: $scope.entity.Id,
                            Entities: $scope.entity.departments
                        });
                    }
                    $scope.closeDialog();
                }
            });
    };
});