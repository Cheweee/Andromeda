app.service('service', function ($http, $timeout) {
    var loginUrl = '/Base/Login';
    var logoutUrl = '/Base/Logout';
    this.dialogId = null;
    this.openDialog = function (dialogId, id) {
        this.dialogId = dialogId;
        this.addOrEdit = !id;
        this.id = id;
    };
    this.closeDialog = function () {
        this.dialogId = null;
        this.id = null;
        this.addOrEdit = null;
    };
    //Current action for add_or_edit_obj method
    this.addOrEdit = false;
    //Current object for exchange data between different controllers
    this.id = null;

    //JSON data type for request data
    var jsonDataType = 'json';
    //GET HTTP method
    var getMethod = 'get';
    //POST HTTP method
    var postMethod = 'post';

    this.login = function (login, password, needRemember) {
        return $http({
            method: postMethod,
            url: loginUrl,
            data: JSON.stringify({ login: login, password: password, remember: needRemember }),
            dataType: jsonDataType
        });
    };
    this.logout = function () {
        return $http({
            method: getMethod,
            url: logoutUrl
        });
    };
    this.transformChip = function (chip) {
        if (angular.isObject(chip)) {
            return chip;
        }
        return { name: chip, type: 'new' };
    };
    this.openPage = function (url) {
        return $http({
            method: getMethod,
            url: url
        });
    };
    //POST Page Loaded method
    this.pageLoaded = function () {
        return $http({
            method: postMethod,
            url: '/Base/PageLoaded'
        });
    };
    //Filter options for md-data-table
    var createFilter = function () {
        return {
            search: '',
            show: false,
            options: {
                debounce: 500
            },
            remove: function () {
                this.search = '';
                this.show = false;
            }
        };
    };
    var createLimitOptions = function () { return [5, 10, 15]; };
    //Creating query for md-data-table with sort field definition
    this.createQuery = function (order, getObjsUrl) {
        query = {
            filter: createFilter(),
            order: order,
            limit: 10,
            limit_options: createLimitOptions(),
            page: 1,
            total: 0,
            objs: [],
            selected: [],
            allSelected: false,
            toggle: function (item) {
                if (!item.selected) {
                    this.selected.push(item);
                }
                else {
                    var index = this.selected.indexOf(item);
                    this.selected.splice(index, 1);
                    item.roles = [];
                }
                this.allSelected = this.selected.length === this.objs.length;
            },
            toggleAll: function () {
                if (this.allSelected) {
                    this.selected = [];
                    angular.forEach(this.objs, function (item) {
                        item.selected = false;
                    });
                }
                else {
                    for (i = 0; i < this.objs.length; i++) {
                        var item = this.objs[i];
                        if (!item.selected) {
                            this.selected.push(item);
                            item.selected = true;
                        }
                    }
                }
            },
            getObjs: function () {
                var self = query;
                $http({
                    dataType: jsonDataType,
                    method: getMethod,
                    params: model = { Page: self.page, Limit: self.limit, Order: self.order, Search: self.filter.search },
                    url: getObjsUrl
                }).then(function (response) {
                    if (response.data.result === 'OK') {
                        self.objs = response.data.rows;
                        self.total = response.data.total;
                        self.page = response.data.page;
                        self.selected = [];
                    }
                })
            }
        };
        return query;
    };
    //Get objects collection by model
    this.getObjs = function (url, model) {
        return $http({
            dataType: jsonDataType,
            method: getMethod,
            params: model,
            url: url
        });
    };
    //Get object by model
    this.getObj = function (url, model) {
        return $http({
            method: getMethod,
            url: url,
            params: model,
            dataType: jsonDataType
        });
    };
    //Add or edit object
    this.addOrEditObj = function (url, obj) {
        return $http({
            method: postMethod,
            url: url,
            data: JSON.stringify(obj),
            dataType: jsonDataType
        });
    };
    //Delete objects collection
    this.deleteObjs = function (url, objs) {
        return $http({
            method: postMethod,
            url: url,
            data: JSON.stringify(objs),
            dataType: jsonDataType
        });
    };
});