var app = angular.module('andromedaApp', ['ngMaterial', 'ngAnimate', 'ngMessages', 'ngCookies', 'md.data.table'])
    //Creating a theme provider and choosing colors of application
    .config(function ($mdThemingProvider) {
        $mdThemingProvider
            //Choosing the default theme
            .theme('default')
            //Chosing a color for primary palette
            .primaryPalette('indigo')
            //Choosing a color for accent palette
            .accentPalette('orange');
    });