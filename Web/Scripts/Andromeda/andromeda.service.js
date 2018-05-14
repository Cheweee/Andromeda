app.service('service', function ($http, $timeout) {
    var loginUrl = '/Base/Login';
    var logoutUrl = '/Base/Logout';
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
    var createLimitOptions = function () { return [5, 10, 15, 20, 30, 50]; };
    //Creating query for md-data-table with sort field definition
    this.createQuery = function (order, getEntitiesUrl) {
        query = {
            filter: createFilter(),
            order: order,
            limit: 10,
            limitOptions: createLimitOptions(),
            page: 1,
            total: 0,
            entities: [],
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
                this.allSelected = this.selected.length === this.entities.length;
            },
            toggleAll: function () {
                if (this.allSelected) {
                    this.selected = [];
                    angular.forEach(this.entities, function (item) {
                        item.selected = false;
                    });
                }
                else {
                    for (i = 0; i < this.entities.length; i++) {
                        var item = this.entities[i];
                        if (!item.selected) {
                            this.selected.push(item);
                            item.selected = true;
                        }
                    }
                }
            },
            getEntities: function () {
                var self = query;
                $http({
                    dataType: jsonDataType,
                    method: getMethod,
                    params: model = { Page: self.page, Limit: self.limit, Order: self.order, Search: self.filter.search },
                    url: getEntitiesUrl
                }).then(function (response) {
                    if (response.data.Result) {
                        self.entities = response.data.Entities;
                        self.total = response.data.Total;
                        self.page = response.data.Page;
                        self.selected = [];
                    }
                });
            }
        };
        return query;
    };
    //Get objects collection by model
    this.getEntities = function (url, model) {
        return $http({
            dataType: jsonDataType,
            method: getMethod,
            params: model,
            url: url
        });
    };
    //Get object by id
    this.getEntityById = function (url, id) {
        return $http({
            method: getMethod,
            url: url,
            params: { id: id },
            dataType: jsonDataType
        });
    };
    //Get object by name
    this.getEntityByName = function (url, name) {
        return $http({
            method: getMethod,
            url: url,
            params: name,
            dataType: jsonDataType
        });
    };
    //Add or edit object
    this.addOrEditEntity = function (url, entity) {
        return $http({
            method: postMethod,
            url: url,
            data: { entity: entity },
            dataType: jsonDataType
        });
    };
    //Delete objects collection
    this.deleteEntities = function (url, entities) {
        return $http({
            method: postMethod,
            url: url,
            data: JSON.stringify(entities),
            dataType: jsonDataType
        });
    };
    this.changeEntities = function (url, model) {
        return $http({
            method: postMethod,
            url: url,
            data: model,
            dataType: jsonDataType
        });
    };
    this.postMethod = function (url) {
        return $http({
            method: postMethod,
            url: url
        });
    };
});