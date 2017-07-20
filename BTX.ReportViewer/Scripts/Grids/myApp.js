"use strict";
var app = angular.module('myApp',
    [
        'ngTouch',
        'ngSanitize',
        'ngAnimate',
        'ui.grid',
        'ui.grid.autoResize',
        'ui.grid.pinning',
        'ui.grid.selection',
        'ui.grid.edit',
        'ui.grid.expandable',
        'ui.grid.cellNav',
        'ui.grid.grouping',
        'ui.bootstrap',
        'ui.grid.resizeColumns',
        'angularjs-dropdown-multiselect',
        'ui.grid.exporter',
        'ngRoute'
    ])
//.controller('top50Ctrl', Top50Ctrl)
//.controller('agentSalesCtrl', AgentSalesCtrl)
//.controller('brandSalesCtrl', BrandSalesCtrl)
//.controller('salesSummaryCtrl', SalesSummaryCtrl)
//.controller('salesSummaryStoreLicenseeCtrl', SalesSummaryStoreLicenseeCtrl)
//.controller('salesPersonalStoreCtrl', SalesStoreCtrl)
//.controller('salesPersonalLicenseeCtrl', SalesLicCtrl)
//.controller('salesTeamCtrl', SalesTeamCtrl)
//.controller('weeklySalesCtrl', WeeklySalesCtrl)
.filter('mapPercentage', function () {
    return function (input) {
        return (input * 100).toFixed(2) + "%";
    }
})
.filter('highlight', function () {
    return function (text, search, caseSensitive) {
        if (search || angular.isNumber(search)) {
            text = text.toString();
            search = search.toString();
            if (caseSensitive) {
                return text.split(search).join('<span class="ui-match">' + search + '</span>');
            } else {
                return text.replace(new RegExp(search, 'gi'), '<span class="ui-match">$&</span>');
            }
        } else {
            return text;
        }
    }
})
.factory('GroupDropDown', function () {
    return {
        groups: [{ id: 0, label: '9L Cases' }, { id: 1, label: 'Total Sales Amount' }, { id: 2, label: 'Units' }],
        licgroups: [{ id: 0, label: 'Total' }, { id: 1, label: 'Licensees' }, { id: 2, label: 'Direct Sales' }],
        createFormElement: function (id, value) {
            var elem = document.createElement("input");
            elem.setAttribute("type", "hidden");
            elem.setAttribute("id", id);
            elem.setAttribute("name", id);
            elem.setAttribute("value", value);
            return elem;
        }
    }
    })
.factory('GroupService', function () {
   // localstorage to persist the user Metrics
    var setCatGroup = function (cname, cvalue) {
        localStorage.setItem(cname, JSON.stringify(cvalue));
    }

    var getCatGroup = function (c_name) {
        var value = JSON.parse(localStorage.getItem(c_name));
        return value;
    }
    return {
        setCatGroup: setCatGroup,
        getCatGroup: getCatGroup

    };
    })
    .factory('CategoryService', function () {     
     // localstorage to persist the user Products options
        var setProductCategory = function (cname, cvalue) {
            localStorage.setItem(cname, cvalue);
        }

        var getProductCategory = function (c_name) {
           return localStorage.getItem(c_name);
            
        }


        return {
            setProductCategory: setProductCategory,
            getProductCategory: getProductCategory
        };
    })
    ;

app.config(['$qProvider', function ($qProvider) {
    $qProvider.errorOnUnhandledRejections(false);
}]);

//helper of dropdown objects to list of string
function setnamesToString(obj) {
    var str = '';
    if (obj.length > 0) {
        for (var p in obj) {
            if (obj.hasOwnProperty(p)) {
                str += obj[p].id + ',';
            }
        }
    }
    //remove the last comma
    var outputStr = str.slice(0, -1);
    return outputStr;
}

function InternalErrorHandler(error) {
    toastr.error(error.data, 'Internal Error');
}

function WarningHandler(warning) {
    toastr.warning(warning, 'Warning');
}