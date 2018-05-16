app.controller('roleController', function ($scope, $q, $timeout, $mdDialog, $filter, service) {
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
    
    $scope.searchRight = '';
    $scope.selectedRole = null;

    $scope.getNotRoleRights = function () {
        var deferred = $q.defer();
        service.getEntities('/Administration/GetNotRoleRights'
            , model = { Search: $scope.settings.searchRight }).then(function (response) {
                if (response.data.Result) {
                    var rows = response.data.Entities;
                    for (i = 0; i < $scope.entity.rights.length; i++) {
                        var index = rows.findIndex(o => o.Id === $scope.entity.rights[i].Id);
                        if (index >= 0) {
                            rows.splice(index, 1);
                        }
                    }
                    deferred.resolve(rows);
                }
            });

        return deferred.promise;
    };

    $scope.getRoleRights = function () {
        service.getEntities('/Administration/GetRoleRights'
            , model = { SearchId: $scope.entity.Id }).then(function (response) {
                if (response.data.Result) {
                    $scope.entity.rights = response.data.Entities;
                }
                $scope.loading = false;
            });
    };

    $scope.entity = {
        Id: null,
        Name: '',
        CanTeach: false,
        rights: []
    };

    $scope.settings = {
        searchRight: '',
        right: null,
        delay: 500,
        loading: false,
        message: 'Загрузка данных',
        addition: false,
        addOrEdit: service.addOrEdit
    };

    $scope.loadDialog = function () {
        $scope.message = 'Загрузка данных';
        if (!$scope.settings.addOrEdit) {
            $scope.loading = true;
            service.getEntityById('/Administration/GetRole', service.id)
                .then(function (response) {
                    if (response.data.Result) {
                        $scope.entity.Id = response.data.Entity.Id;
                        $scope.entity.Name = response.data.Entity.Name;
                        $scope.entity.CanTeach = response.data.Entity.CanTeach;
                        $scope.getRoleRights();
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
        $scope.entity.CanTeach = false;
        $scope.entity.rights = [];
    };

    $scope.confirm = function () {
        var entity = {
            Id: $scope.entity.Id,
            Name: $scope.entity.Name,
            CanTeach: $scope.entity.CanTeach
        };

        $scope.message = 'Сохранение изменений';
        $scope.loading = true;
        result = true;
        var method = service.addOrEdit ? "/Administration/AddRole" : "/Administration/ModifyRole";
        service.addOrEditEntity(method, entity)
            .then(function (response) {
                if (response.data.Result) {
                    if (service.addOrEdit) {
                        $scope.entity.Id = response.data.Id;
                    }
                    if ($scope.entity.Id) {
                        if ($scope.entity.rights.length) {
                            service.changeEntities('/Administration/SaveRoleRights', model = {
                                NewId: $scope.entity.Id,
                                Entities: $scope.entity.rights
                            });
                        }
                    }
                    $scope.closeDialog();
                }
            });
    };
});