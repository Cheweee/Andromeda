app.controller('typeOfProjectController', function ($scope, $q, $timeout, $mdDialog, $filter, service) {
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

    $scope.entity = {
        Id: null,
        Name: '',
        ShortName: ''
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
            service.getEntityById('/References/GetTypeOfProject',service.id)
                .then(function (args) {
                    if (args.data.Result) {
                        createTimer();
                        $scope.entity.Id = args.data.Entity.Id;
                        $scope.entity.Name = args.data.Entity.Name;
                        $scope.entity.ShortName = args.data.Entity.ShortName;
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
    };

    $scope.confirm = function () {
        var entity = {
            Id: $scope.entity.Id,
            Name: $scope.entity.Name,
            ShortName: $scope.entity.ShortName
        };

        $scope.message = 'Сохранение изменений';
        $scope.loading = true;
        result = true;
        var method = service.addOrEdit ? "/References/AddTypeOfProjects" : "/References/ModifyTypeOfProjects";
        service.addOrEditEntity(method, entity)
            .then(function (response) {
                if (response.data.Result) {
                    $scope.closeDialog();   
                }
            });
    };
});