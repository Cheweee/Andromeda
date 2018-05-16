app.controller('userController', function ($scope, $q, $timeout, $cookies, $filter, service) {
    $scope.entity = {
        Id: null,
        Name: '',
        Patronimyc : '',
        LastName: '',
        Login: '',
        Password: '',
        AcademicDegrees: [],
        AcademicTitles: []
    };

    $scope.settings = {
        searchAcademicDegree: '',
        searchAcademicTitle: '',
        delay: 500,
        loading: false,
        message: 'Загрузка данных',
        addition: false,
        transformChip: service.transformChip,
        addOrEdit: $cookies.get('addOrEdit'),
        createTimer: function () {
            $timeout(function () {
                // loading
                $scope.loading = false;
            }, this.delay);
        }
    };

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
                        $scope.getUserAcademicDegrees();
                        $scope.getUserAcademicTitles();
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
        $scope.entity.Login = '';
        $scope.entity.Password = '';
        $scope.entity.LastName = '';
        $scope.entity.Patronimyc = '';
        $scope.entity.AcademicDegrees = [];
        $scope.entity.AcademicTitles = [];
    };

    $scope.confirm = function () {
        var entity = {
            Id: $scope.entity.Id,
            Name: $scope.entity.Name,
            LastName: $scope.entity.LastName,
            Patronimyc: $scope.entity.Patronimyc,
            Login: $scope.entity.Login,
            Password: $scope.entity.Password
        };

        $scope.message = 'Сохранение изменений';
        $scope.loading = true;
        result = true;
        var method = service.addOrEdit ? "/Administration/AddUser" : "/Administration/ModifyUser";
        service.addOrEditEntity(method, entity)
            .then(function (response) {
                if (response.data.Result) {
                    $scope.closeDialog();
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

    $scope.getNotUserAcademicTitles = function () {
        var deferred = $q.defer();
        service.getEntities('/Administration/GetNotUserAcademicTitles'
            , model = { Search: $scope.settings.searchAcademicTitle }).then(function (response) {
                if (response.data.Result) {
                    var rows = response.data.Entities;
                    for (i = 0; i < $scope.entity.AcademicTitles.length; i++) {
                        var index = rows.findIndex(o => o.Id === $scope.entity.AcademicTitles[i].Id);
                        if (index >= 0) {
                            rows.splice(index, 1);
                        }
                    }
                    deferred.resolve(rows);
                }
            });

        return deferred.promise;
    };

    $scope.getUserAcademicTitles = function () {
        service.getEntities('/Administration/GetUserAcademicTitles'
            , model = { SearchId: $scope.entity.Id }).then(function (response) {
                if (response.data.Result) {
                    $scope.entity.AcademicTitles = response.data.Entities;
                }
                $scope.loading = false;
            });
    };
});