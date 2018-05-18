app.controller('userRoleController', function ($scope, $q, $timeout, $mdDialog, $cookies, $filter, service) {
    $scope.roles = [];
	$scope.searchRole = '';
	$scope.selectedRole = null;

	$scope.getRoles = function () {
        var deferred = $q.defer();
        service.getEntities('/Administration/GetUserRoleRoles', model = { Search: $scope.searchRole, SearchId: $cookies.get('entityId') })
            .then(function (response) {
                if (response.data.Result) {
                    var rows = response.data.Entities;
                    $scope.roles = rows;
                    deferred.resolve(rows);
                }
            });

		return deferred.promise;
    };

    $scope.departments = [];
    $scope.searchDepartment = '';
    $scope.selectedDepartment = null;

    $scope.getDepartments = function () {
        var deferred = $q.defer();
        service.getEntities('/Administration/GetUserRoleDepartments', model = { Search: $scope.searchDepartment, SearchId: $cookies.get('entityId'), SecondSearchId: $scope.selectedRole.Id })
            .then(function (response) {
                if (response.data.Result) {
                    var rows = response.data.Entities;
                    $scope.departments = rows;
                    deferred.resolve(rows);
                }
            });

        return deferred.promise;
    };

    $scope.entity = {
        Id: null,
        UserId: null,
		RoleId: null,
		DepartmentId: null
	};

	$scope.settings = {
		delay: 500,
		loading: false,
		message: 'Загрузка данных',
		addition: false,
		addOrEdit: service.addOrEdit,
		closeDialog: function () {
            $mdDialog.hide();
        },
        createTimer: function () {
            $timeout(function () {
                // loading
                $scope.loading = false;
            }, $scope.settings.delay);
        }
	};

    $scope.loadDialog = function () {
        $scope.message = 'Загрузка данных';
        $scope.entity.UserId = $cookies.get('entityId');
        if (!$scope.settings.addOrEdit) {
            $scope.loading = true;
            $scope.entity.Id = service.tempEntity.Id;
            $scope.entity.RoleId = service.tempEntity.RoleId;
            $scope.entity.UserId = service.tempEntity.UserId;
            $scope.entity.DepartmentId = service.tempEntity.DepartmentId;
            $scope.getRoles().then(function () {
                var rows = $scope.roles;
                for (i = 0; i < rows.length; i++) {
                    if ($scope.entity !== null) {
                        if ($scope.entity.RoleId === rows[i].Id) {
                            $scope.selectedRole = rows[i];
                        }
                    }
                }
            });
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
            $scope.settings.createTimer();
        }
    };

    $scope.clearFields = function () {
        $scope.entity.DepartmentId = null;
        $scope.entity.RoleId = null;
        $scope.selectedDepartment = null;
        $scope.selectedRole = null;

		if (!$scope.settings.addOrEdit) {
			$scope.loadDialog();
        }
	};

    $scope.confirm = function () {
        var departmentId = null;
        if ($scope.selectedDepartment) {
            departmentId = $scope.selectedDepartment.Id;
        }
        var entity = {
            Id: $scope.entity.Id,
            RoleId: $scope.selectedRole.Id,
            DepartmentId: departmentId,
            UserId: $scope.entity.UserId,
            Name: $scope.selectedRole.Name + (departmentId ? ' ' + $scope.selectedDepartment.ShortName : '') 
        };

        $scope.message = 'Сохранение изменений';
        $scope.loading = true;
        entity.EntityState = service.addOrEdit ? 4 : 16;
        //      var method = service.addOrEdit ? "/Administration/AddUserRole" : "/Administration/ModifyUserRole";
        //service.addOrEditEntity(method, entity)
        //          .then(function (response) {
        //              if (response.data.Result) {
        //                  $scope.settings.closeDialog();
        //              }
        //          });
        service.tempEntity = entity;
        $scope.settings.closeDialog();
    };
});