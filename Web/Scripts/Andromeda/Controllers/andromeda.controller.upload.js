app.controller('uploadController', function ($scope, $timeout, $mdDialog, service, FileUploader) {
    $scope.uploader = new FileUploader({
        url: '/CirriculumDevelopment/UploadWorkingCirriculumFile',
        filters: [{
            name: 'excelFilter',
            fn: function (item, options) {
                var type = '|' + item.type.slice(item.type.lastIndexOf('/') + 1) + '|';
                return '|xls|xlsx|vnd.ms-excel|vnd.openxmlformats-officedocument.spreadsheetml.sheet|'.indexOf(type) !== -1;
            }
        }],
        queueLimit: 1
    });

    $scope.fileName = '';

    $scope.settings = {
        message: 'Загрузка данных',
        closeDialog: function () {
            $mdDialog.hide();
        },
        createTimer: function () {
            return $timeout(function () {
                // loading
                $scope.settings.loading = false;
            }, this.delay);
        }
    };

    $scope.confirm = function () {
        $scope.settings.loading = true;
        $scope.uploader.uploadItem(0);
    };

    $scope.cancel = function () {
        $scope.uploader.clearQueue();
    };
    
    $scope.uploadFile = function () {
        $scope.uploader.clearQueue();
        $scope.fileUpload = angular.element(document.getElementById('fileUpload'));
        $scope.fileUpload.click();
    };

    $scope.uploader.onSuccessItem = function (item, response, status, headers) {
        $scope.settings.createTimer().then(function () {
            $scope.uploader.clearQueue();
            if (response.Result) {
                service.entity = response.Entity;

            }
            $scope.settings.closeDialog();
        });
    };
});