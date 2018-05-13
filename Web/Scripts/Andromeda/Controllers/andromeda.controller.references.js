app.controller('referenceController', function ($scope, $timeout, $mdDialog, $q, service) {
    $scope.query = null;
    $scope.deleteMethod;
    $scope.deleteMessage;
    $scope.addOrEditController;
    $scope.fullscreen = false;

    $scope.init = function (order, getMethod, deleteMethod, deleteMessage, addOrEditController, fullscreen) {
        $scope.query = service.createQuery(order, getMethod);
        $scope.deleteMethod = deleteMethod;
        $scope.deleteMessage = deleteMessage;
        $scope.addOrEditController = addOrEditController;
        $scope.fullscreen = fullscreen;
        $scope.$watch('query.filter.search', function () { if ($scope.query !== null) $scope.query.getEntities(); });
    };
    
    $scope.add = function (event) {
        service.openDialog('addOrEditForm.html');
    };

    $scope.edit = function (obj, event) {
        service.openDialog('addOrEditForm.html', obj.Id);
    };

    $scope.$watch(function () { return service.dialogId; }, function (value) {
        if ($scope.query === null) {
            return;
        }
        $scope.dialog = value;
        if (!value) {
            $scope.query.getEntities();
        }
    });

    $scope.addEntity = function (event) {
        service.addOrEdit = true;
        $mdDialog.show({
            clickOutsideToClose: false,
            controller: $scope.addOrEditController,
            controllerAs: 'ctrl',
            focusOnOpen: true,
            targetEvent: event,
            templateUrl: 'addOrEditForm.html',
            fullscreen: $scope.fullscreen
        }).then($scope.query.getEntities);
    };

    $scope.editEntity = function (entity, event) {
        service.addOrEdit = false;
        service.id = entity.Id;
        $mdDialog.show({
            clickOutsideToClose: false,
            controller: $scope.addOrEditController,
            controllerAs: 'ctrl',
            focusOnOpen: true,
            targetEvent: event,
            templateUrl: 'addOrEditForm.html',
            fullscreen: $scope.fullscreen
        }).then($scope.query.getEntities);
    };

    $scope.delete = function (entities, event) {
        var confirm = $mdDialog.confirm()
            .title($scope.deleteMessage)
            .ariaLabel('Pleeeeease noooooo!!!')
            .targetEvent(event)
            .ok('Да')
            .cancel('Нет');
        $mdDialog.show(confirm).then(
            function () {
                service.deleteEntities($scope.deleteMethod, entities).then($scope.query.getEntities);
            }, function () { });
    };
});