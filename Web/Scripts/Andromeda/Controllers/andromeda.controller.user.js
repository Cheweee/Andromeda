app.controller('userController', function ($scope, $q, $timeout, $cookies, $mdDialog, $filter, service) {
    $scope.entity = {
        Id: null,
        AcademicTitleId: null,
        Name: '',
        Patronimyc: '',
        LastName: '',
        Login: '',
        Password: '',
        AcademicDegrees: []
    };

    $scope.settings = {
        searchAcademicDegree: '',
        academicTitles: [],
        searchAcademicTitle: '',
        selectedAcademicTitle: null,
        delay: 500,
        loading: false,
        message: 'Загрузка данных',
        addition: false,
        transformChip: service.transformChip,
        addOrEdit: $cookies.get('addOrEdit'),
        createTimer: function () {
            $timeout(function () {
                // loading
                $scope.settings.loading = false;
            }, this.delay);
        }
    };

    $scope.roles = service.createQuery('Name', '/Administration/GetUserRoles');

    $scope.loadDialog = function () {
        $scope.message = 'Загрузка данных';
        var id = $cookies.get('entityId');
        if (id) {
            $scope.settings.loading = true;
            service.getEntityById('/Administration/GetUser', id)
                .then(function (response) {
                    if (response.data.Result) {
                        $scope.entity.Id = response.data.Entity.Id;
                        $scope.entity.Name = response.data.Entity.Name;
                        $scope.entity.LastName = response.data.Entity.LastName;
                        $scope.entity.Patronimyc = response.data.Entity.Patronimyc;
                        $scope.entity.Login = response.data.Entity.Login;
                        $scope.entity.Password = response.data.Entity.Password;
                        $scope.entity.AcademicTitleId = response.data.Entity.AcademicTitleId;
                        $scope.getUserAcademicDegrees();
                        $scope.getAcademicTitles().then(function () {
                            var rows = $scope.settings.academicTitles;
                            for (i = 0; i < rows.length; i++) {
                                if ($scope.entity !== null) {
                                    if ($scope.entity.AcademicTitleId === rows[i].Id) {
                                        $scope.settings.selectedAcademicTitle = rows[i];
                                    }
                                }
                            }
                            $scope.settings.createTimer();
                        });
                        $scope.roles.getEntities(id);
                        $scope.settings.loading = false;
                    }
                });
        }
    };

    $scope.clearFields = function () {
        $scope.entity.Name = '';
        $scope.entity.Login = '';
        $scope.entity.Password = '';
        $scope.entity.LastName = '';
        $scope.entity.Patronimyc = '';
        $scope.entity.AcademicDegrees = [];
        $scope.entity.AcademicTitleId = null;
        $scope.selectedAcademicTitle = null;
        if (!$scope.settings.addOrEdit) {
            $scope.loadDialog();
            return;
        }
    };

    $scope.confirm = function () {
        var academicTitleId = null;
        if ($scope.settings.selectedAcademicTitle) {
            academicTitleId = $scope.settings.selectedAcademicTitle.Id;
        }
        var entity = {
            Id: $scope.entity.Id,
            Name: $scope.entity.Name,
            LastName: $scope.entity.LastName,
            Patronimyc: $scope.entity.Patronimyc,
            Login: $scope.entity.Login,
            Password: $scope.entity.Password,
            AcademicTitleId: academicTitleId
        };

        $scope.message = 'Сохранение изменений';
        $scope.settings.loading = true;
        result = true;
        var method = '/Administration/SaveUser';
        service.addOrEditEntity(method, entity)
            .then(function (response) {
                if (response.data.Result) {
                    entity.Id = response.data.Id;
                    for (var i = 0; i < $scope.roles.entities.length; i++) {
                        $scope.roles.entities[i].UserId = entity.Id;
                    }

                    service.changeEntities('/Administration/SaveUserAcademicDegrees', model = {
                        NewId: $scope.entity.Id,
                        Entities: $scope.entity.AcademicDegrees ? $scope.entity.AcademicDegrees : []
                    })
                        .then(function (response) {
                            if ($scope.roles.entities.length) {
                                service.postMethod('/Administration/SaveUserRolesState', model = {
                                    Entities: $scope.roles.entities
                                })
                                    .then(function (response) {
                                        $scope.settings.createTimer();
                                    });
                            }
                            else {
                                $scope.settings.createTimer();
                            }
                        });
                }
            });
    };

    $scope.getNotUserAcademicDegrees = function () {
        var deferred = $q.defer();
        service.getEntities('/Administration/GetNotUserAcademicDegrees'
            , model = { Search: $scope.settings.searchAcademicDegree }).then(function (response) {
                if (response.data.Result) {
                    var rows = response.data.Entities;
                    for (i = 0; i < $scope.entity.AcademicDegrees.length; i++) {
                        var index = rows.findIndex(o => o.Id === $scope.entity.AcademicDegrees[i].Id);
                        if (index >= 0) {
                            rows.splice(index, 1);
                        }
                    }
                    deferred.resolve(rows);
                }
            });

        return deferred.promise;
    };

    $scope.getUserAcademicDegrees = function () {
        service.getEntities('/Administration/GetUserAcademicDegrees'
            , model = { SearchId: $scope.entity.Id }).then(function (response) {
                if (response.data.Result) {
                    $scope.entity.AcademicDegrees = response.data.Entities;
                }
                $scope.loading = false;
            });
    };

    $scope.getAcademicTitles = function () {
        var deferred = $q.defer();
        service.getEntities('/Administration/GetAcademicTitles'
            , model = { Search: $scope.settings.searchAcademicTitle }).then(function (response) {
                if (response.data.Result) {
                    $scope.settings.academicTitles = response.data.Entities;
                    deferred.resolve($scope.settings.academicTitles);
                }
            });

        return deferred.promise;
    };

    $scope.addEntity = function (event) {
        service.addOrEdit = true;
        $mdDialog.show({
            clickOutsideToClose: false,
            controller: 'userRoleController',
            controllerAs: 'ctrl',
            focusOnOpen: true,
            targetEvent: event,
            templateUrl: 'addOrEditForm.html',
            fullscreen: false
        }).then(function () {
            $scope.roles.entities.push(service.tempEntity);
        });
    };

    $scope.editEntity = function (entity, event) {
        service.addOrEdit = false;
        service.tempEntity = entity;
        $mdDialog.show({
            clickOutsideToClose: false,
            controller: 'userRoleController',
            controllerAs: 'ctrl',
            focusOnOpen: true,
            targetEvent: event,
            templateUrl: 'addOrEditForm.html',
            fullscreen: false
        }).then(function () {
            for (var i = 0; i < $scope.roles.entities.length; i++) {
                var role = $scope.roles.entities[i];
                if (role.Id === service.tempEntity.Id) {
                    $scope.roles.entities[i] = service.tempEntity;
                }
            }
            $scope.roles.selected = [];
        });
    };

    $scope.delete = function (entities, event) {
        var confirm = $mdDialog.confirm()
            .title('Вы уверены, что хотите удалить выбранные права пользователя?')
            .ariaLabel('Pleeeeease noooooo!!!')
            .targetEvent(event)
            .ok('Да')
            .cancel('Нет');
        $mdDialog.show(confirm).then(
            function () {
                for (var i = 0; i < $scope.roles.entities.length; i++) {
                    var role = $scope.roles.entities[i];
                    for (var j = 0; j < entities.length; j++) {
                        if (entities[j].Id === role.Id && entities[j].RoleId === role.RoleId && entities[j].DepartmentId === role.DepartmentId) {
                            $scope.roles.entities[i].EntityState = 8;
                        }
                    }
                }
                $scope.roles.selected = [];
            }, function () { });
    };
});