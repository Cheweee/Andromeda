//Creating a basic app controller for managing menus and user data
app.controller('baseController', function ($scope, $mdToast, $window, $timeout, $location, service) {
    $scope.message = 'Загрузка...';
    var createTimer = function (need_stop) {
        $timeout(function () {
            // loading
            $scope.loading = !need_stop;
        }, 500);
    };
    $scope.user = {
        Name: '',
        LastName: '',
        Login: '',
        Password: '',
        Remember: false
    };

    //Creating menu item
    var create_item = function (href, name, text, icon) {
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
    $scope.home = create_item('/Base/Home', 'home', 'Начальная страница', 'home');

    //Selected menu item
    $scope.selected_menu_item = null;

    $scope.login = function () {
        $scope.loading = true;
        $scope.message = "Проверка данных пользователя...";
        service.login($scope.user.Login, $scope.user.Password, $scope.user.Remember)
            .then(function (response) {
                if (response.data.result === 'Ok') {
                    $scope.select_item($scope.home);
                }
                else {
                    $mdToast.show(
                        $mdToast.simple()
                            .textContent(response.data.message)
                            .position('bottom left')
                            .hideDelay(3000));
                }
                $timeout(function () {
                    $scope.message = 'Загрузка...';
                }, 500);
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

    $scope.cancel = function () {
        $scope.user.Login = '';
        $scope.user.Password = '';
    };

    //Function choosing menu item
    $scope.select_item = function (item) {
        if (item === $scope.selected_menu_item) {
            return;
        }
        $scope.loading = true;
        if ($scope.selected_menu_item !== null) {
            $scope.selected_menu_item.Color = 'primary-500';
            $scope.selected_menu_item.IsSelected = false;
        }
        item.IsSelected = true;
        $scope.selected_menu_item = item;
        $scope.selected_menu_item.Color = 'accent-500';
        $window.location.href = $scope.selected_menu_item.HRef;
    };

    $scope.load_menu = function (item) {
        if (item === $scope.selected_menu_item) {
            return;
        }
        if ($scope.selected_menu_item !== null) {
            $scope.selected_menu_item.Color = 'primary-500';
            $scope.selected_menu_item.IsSelected = false;
        }
        item.IsSelected = true;
        $scope.selected_menu_item = item;
        $scope.selected_menu_item.Color = 'accent-500';
        createTimer(true);
    };

    $scope.pageLoaded = function (item_name) {
        $scope.loading = true;

        service.pageLoaded().then(function (args) {
            if (args.data.result === 'OK') {
                $scope.references = args.data.references;
                $scope.pages = args.data.pages;
                $scope.administration = args.data.administration;
            }
            var item_founded = false;
            angular.forEach($scope.references, function (item) {
                if (item_founded)
                    return;
                if (item.Name === item_name) {
                    $scope.load_menu(item);
                    item_founded = true;
                }
            });
            angular.forEach($scope.pages, function (item) {
                if (item_founded)
                    return;
                if (item.Name === item_name) {
                    $scope.load_menu(item);
                    item_founded = true;
                }
            });
            angular.forEach($scope.administration, function (item) {
                if (item_founded)
                    return;
                if (item.Name === item_name) {
                    $scope.load_menu(item);
                    item_founded = true;
                }
            });

            if (!item_founded) {
                $scope.load_menu($scope.home);
            }
        });
    };

    //References menu items collection
    $scope.references = [
        create_item('/CirriculumDevelopment/LevelsOfHigherEducation', 'levels_of_higher_education', 'Уровни высшего образования', 'school'),
        create_item('/CirriculumDevelopment/TypesOfProjects', 'types_of_projects', 'Типы работ', 'format_list_numbered'),
        create_item('/CirriculumDevelopment/TypesOfProjects', 'coure_titles', 'Наименования дисциплин', 'title'),
        create_item('/CirriculumDevelopment/Faculties', 'faculties', 'Факультеты и институты', 'business'),
        create_item('/CirriculumDevelopment/Departments', 'departments', 'Кафедры', 'account_balance'),
        create_item('', 'academic_title', 'Ученые звания', 'star'),
        create_item('', 'academic_degrees', 'Ученые степени', 'supervisor_account')
    ];

    //Pages menu items collection
    $scope.pages = [
        create_item('', 'seats', 'Должности', 'event_seat'),
        create_item('', 'professors', 'Преподаватели', 'assignment_ind'),
        create_item('', 'areas_of_training', 'Направления подготовки', 'layers'),
        create_item('', 'working_cirriculum', 'Рабочие планы', 'chrome_reader_mode')
    ];

    //Administration menu items collection
    $scope.administration = [
        create_item('/Administration/Users', 'users', 'Пользователи', 'people'),
        create_item('/Administration/Roles', 'roles', 'Роли', 'work'),
        create_item('/Administration/Rights', 'rights', 'Права ролей', 'accessibility')
    ];
});