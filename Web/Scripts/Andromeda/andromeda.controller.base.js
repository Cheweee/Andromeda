//Creating a basic app controller for managing menus and user data
app.controller('baseController', function ($scope, $mdToast, $window, $timeout, service) {
    $scope.message = 'Загрузка...';
    var createTimer = function (need_stop) {
        $timeout(function () {
            // loading
            $scope.loading = !need_stop;
        }, 500);
    };
    $scope.access = true;
    $scope.user = {
        Name: '',
        LastName: '',
        Login: '',
        Password: '',
        Remember: false
    };

    //Creating menu item
    var createItem = function (href, name, text, icon) {
        return {
            HRef: href,
            Name: name,
            Text: text,
            Icon: icon,
            Color: 'primary-500',
            IsSelected: false
        };
    };
    $scope.loading = false;

    //Home menu item
    $scope.home = createItem('/Base/Home', 'home', 'Начальная страница', 'home');

    //Selected menu item
    $scope.selectedMenuItem = null;

    $scope.login = function () {
        $scope.loading = true;
        $scope.message = "Проверка данных пользователя...";
        service.login($scope.user.Login, $scope.user.Password, $scope.user.Remember)
            .then(function (response) {
                if (response.data.Result) {
                    $scope.selectItem($scope.home);
                }
                else {
                    $scope.loading = false;
                    $mdToast.show(
                        $mdToast.simple()
                            .textContent(response.data.Message)
                            .position('bottom left')
                            .hideDelay(3000));
                }
            });
    };

    $scope.logout = function () {
        $scope.loading = true;
        service.logout().then(function (response) {
            if (response.status === 200) {
                $window.location.href = '/Base/Login';
            }
        });
    };

    $scope.account = function () {
        $window.location.href = '/Base/Account';
    };
    $scope.help = function () {
        $window.location.href = '/Base/Help';
    };
    $scope.settings = function () {
        $window.location.href = '/Base/Settings';
    };

    $scope.cancel = function () {
        $scope.user.Login = '';
        $scope.user.Password = '';
    };

    //Function choosing menu item
    $scope.selectItem = function (item) {
        if (item === $scope.selectedMenuItem) {
            return;
        }
        $scope.loading = true;
        if ($scope.selectedMenuItem !== null) {
            $scope.selectedMenuItem.Color = 'primary-500';
            $scope.selectedMenuItem.IsSelected = false;
        }
        item.IsSelected = true;
        $scope.selectedMenuItem = item;
        $scope.selectedMenuItem.Color = 'accent-500';
        $window.location.href = $scope.selectedMenuItem.HRef;
    };

    $scope.loadMenu = function (item) {
        if (item === $scope.selectedMenuItem) {
            return;
        }
        if ($scope.selectedMenuItem !== null) {
            $scope.selectedMenuItem.Color = 'primary-500';
            $scope.selectedMenuItem.IsSelected = false;
        }
        item.IsSelected = true;
        $scope.selectedMenuItem = item;
        $scope.selectedMenuItem.Color = 'accent-500';
        createTimer(true);
    };

    $scope.pageLoaded = function (itemName) {
        $scope.loading = true;

        service.pageLoaded().then(function (response) {
            if (itemName === $scope.home.Name) {
                $scope.loadMenu($scope.home);
            }

            if (response.data.Result) {
                $scope.references = response.data.AccesibleReferences;
                $scope.pages = response.data.AccesiblePages;
                $scope.administration = response.data.AccesibleAdministration;

                var itemFounded = false;
                angular.forEach($scope.references, function (item) {
                    if (itemFounded)
                        return;
                    if (item.Name === itemName) {
                        $scope.loadMenu(item);
                        itemFounded = true;
                    }
                });
                angular.forEach($scope.pages, function (item) {
                    if (itemFounded)
                        return;
                    if (item.Name === itemName) {
                        $scope.loadMenu(item);
                        itemFounded = true;
                    }
                });
                angular.forEach($scope.administration, function (item) {
                    if (itemFounded)
                        return;
                    if (item.Name === itemName) {
                        $scope.loadMenu(item);
                        itemFounded = true;
                    }
                });
            }

            $scope.access = response.data.Result !== 2;
                

            $scope.loading = false;
        });
    };

    //References menu items collection
    $scope.references = [];

    //Pages menu items collection
    $scope.pages = [];

    //Administration menu items collection
    $scope.administration = [];
});