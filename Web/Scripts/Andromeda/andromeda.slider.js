app.directive('andromedaSlider', function () {
    return {
        bindingToController: true,
        controllerAs: '$sliderController',
        controller: function ($scope, $transclude) {
            $scope.slides = [];
            $scope.selectedIndex = 0;
            this.add_slide = function (slide) {
                slide.slideIndex = $scope.slides.length;
                $scope.slides.push(slide);
            };
        },
        replace: true,
        restrict: 'AE',
        scope: {
            selectedIndex: '=?'
        },
        template: '<div class="slider" ng-transclude></div>',
        transclude: true
    };
});
app.directive('andromedaSlide', function () {
    return {
        bindingToController: true,
        controller: function ($scope) {
            $scope.slideIndex = 0;
            $scope.selectedIndex = 0;
            $scope.$watch(function () {
                if ($scope.$parent.$parent.selectedIndex !== undefined) {
                    return $scope.$parent.$parent.selectedIndex;
                }
                if ($scope.$parent.$parent.$parent.selectedIndex !== undefined) {
                    return $scope.$parent.$parent.$parent.selectedIndex;
                }
                return null;
            },
                function (value) {
                    if (value !== undefined) {
                        $scope.selectedIndex = value;
                    }
                });

            $scope.$watch(function () { return $scope.name; },
                function (value) { });
        },
        link: function (scope, element, attrs, slider_control) {
            slider_control.add_slide(scope);
        },
        replace: true,
        require: '^andromedaSlider',
        restrict: 'AE',
        template: '<div ng-transclude ng-show="slideIndex == selectedIndex" ng-class="{\'left\': slideIndex == selectedIndex-1,\'right\': slideIndex == selectedIndex+1}" class="slide"></div>',
        transclude: true,
        scope: {}
    };
});