app.controller('departmentController', function ($scope, $q, $timeout, $mdDialog, $filter, service) {
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

    $scope.faculties = [];
    $scope.searchFaculty = '';
    $scope.selectedFaculty = null;

    $scope.getFaculties = function () {
        var deferred = $q.defer();
        service.getEntities('/References/GetDepartmentFaculties'
            , model = { Search: $scope.searchFaculty }).then(function (response) {
                if (response.data.Result) {
                    var rows = response.data.Entities;
                    $scope.faculties = rows;
                    deferred.resolve(rows);
                }
            });

        return deferred.promise;
    };

    $scope.entity = {
        Id: null,
        Name: '',
        ShortName: '',
        FacultyName: '',
        FacultyId: null,
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
            service.getEntityById('/References/GetDepartment', service.id)
                .then(function (args) {
                    if (args.data.Result) {
                        $scope.entity.Id = args.data.Entity.Id;
                        $scope.entity.Code = args.data.Entity.Code;
                        $scope.entity.Name = args.data.Entity.Name;
                        $scope.entity.ShortName = args.data.Entity.ShortName;
                        $scope.entity.FacultyId = args.data.Entity.FacultyId;
                        $scope.getFaculties().then(function () {
                            var rows = $scope.faculties;
                            for (i = 0; i < rows.length; i++) {
                                if ($scope.entity !== null) {
                                    if ($scope.entity.FacultyId === rows[i].Id) {
                                        $scope.selectedFaculty = rows[i];
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
        $scope.entity.FacultyName = '';
        $scope.entity.FacultyId = null;
        $scope.selectedFaculty = null;
        $scope.entity.Code = 0;
    };

    $scope.confirm = function () {
        var facultyId = null;
        if ($scope.selectedFaculty) {
            facultyId = $scope.selectedFaculty.Id;
        }
        var entity = {
            Id: $scope.entity.Id,
            Name: $scope.entity.Name,
            ShortName: $scope.entity.ShortName,
            Code: $scope.entity.Code,
            FacultyId: facultyId
        };

        $scope.message = 'Сохранение изменений';
        $scope.loading = true;
        result = true;
        var method = service.addOrEdit ? "/References/AddDepartment" : "/References/ModifyDepartment";
        service.addOrEditEntity(method, entity)
            .then(function (response) {
                if (response.data.Result) {
                    $scope.closeDialog();
                }
            });
    };
});