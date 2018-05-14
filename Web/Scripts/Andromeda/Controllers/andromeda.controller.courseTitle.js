app.controller('courseTitleController', function ($scope, $q, $timeout, $mdDialog, $filter, service) {
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

    $scope.departments = [];
    $scope.searchDepartment = '';
    $scope.selectedDepartment = null;

    $scope.getDepartments = function () {
        var deferred = $q.defer();
        service.getEntities('/References/GetCourseTitleDepartments'
            , model = { Search: $scope.searchDepartment }).then(function (response) {
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
		Name: '',
        ShortName: '',
        DepartmentName: ''
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
			service.getEntityById('/References/GetCourseTitle', service.id)
                .then(function (args) {
                    if (args.data.Result) {
                        $scope.entity.Id = args.data.Entity.Id;
                        $scope.entity.Name = args.data.Entity.Name;
                        $scope.entity.ShortName = args.data.Entity.ShortName;
                        $scope.entity.DepartmentId = args.data.Entity.DepartmentId;
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
        $scope.entity.DepartmentId = null;
        $scope.selectedDepartment = null;
	};

	$scope.confirm = function () {
		var entity = {
			Id: $scope.entity.Id,
			Name: $scope.entity.Name,
            ShortName: $scope.entity.ShortName,
            DepartmentId: $scope.selectedDepartment.Id
		};

		$scope.message = 'Сохранение изменений';
		$scope.loading = true;
		result = true;
		var method = service.addOrEdit ? "/References/AddCourseTitle" : "/References/ModifyCourseTitle";
		service.addOrEditEntity(method, entity)
            .then(function (response) {
                if (response.data.Result) {
                    $scope.closeDialog();
                }
            });
	};
});