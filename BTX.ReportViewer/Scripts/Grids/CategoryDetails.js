//SalesByProducts
app.controller('CategoryDetailsCtrl', CategoryDetailsCtrl);

CategoryDetailsCtrl.$inject = ['$http', '$scope', 'GroupDropDown', 'uiGridConstants'];
function CategoryDetailsCtrl($http, $scope, GroupDropDown, uiGridConstants) {

    //UI Grid Setting
    $scope.grid = {
        columnDefs: [

            { name: 'Code', field: "Code", width: '*', cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;">{{COL_FIELD}}</div>' },
            { name: 'Product', field: "ProductName", width: 300, cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;">{{COL_FIELD}}</div>' },
            { name: 'Size', field: "UnitSizeML", width: "*" },

            { name: 'TY Promo Turn', field: "SalesAmount_PromoTurn_TY", width: '*', cellFilter: 'currency:"$":2', type: 'number' },
            { name: 'LY Promo Turn', field: "SalesAmount_PromoTurn_LY", width: '*', cellFilter: 'currency:"$":2', type: 'number' },
            { name: 'SalesAmount_PromoTurn_Pct', field: "SalesAmount_PromoTurn_Pct", displayName: "Var %", width: '*', cellFilter: 'mapPercentage', type: 'number' },

            { name: 'TY 6 month', field: "SalesAmount_6P_TY", width: '*', cellFilter: 'currency:"$":2', type: 'number' },
            { name: 'LY 6 month', field: "SalesAmount_6P_LY", width: '*', cellFilter: 'currency:"$":2', type: 'number' },
            { name: 'SalesAmount_6P_Pct', field: "SalesAmount_6P_Pct", displayName: "Var %", width: '*', cellFilter: 'mapPercentage', type: 'number' },

            { name: 'TY 13 month', field: "SalesAmount_13P_TY", width: '*', cellFilter: 'currency:"$":2', type: 'number' },
            { name: 'LY 13 month', field: "SalesAmount_13P_LY", width: '*', cellFilter: 'currency:"$":2', type: 'number' },
            { name: 'SalesAmount_13P_Pct', field: "SalesAmount_13P_Pct", displayName: "Var %", width: '*', cellFilter: 'mapPercentage', type: 'number' },

            { name: 'Promo Code', field: "Promo", width: 100, cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;">{{COL_FIELD}}</div>' },
            { name: 'Inventory', field: "StoreInv", width: 100, cellFilter: 'number:0', type: 'number' }

        ],
        enableSorting: true,
        enableRowSelection: true,
        enableRowHeaderSelection: true,
        enableCellSelection: false,
        enableCellEditOnFocus: false,
        enableGridMenu: true,
        exporterMenuPdf: false,
        exporterCsvFilename: 'Sales Category Details - ' + document.getElementById('SalesCategory').value + ' - ' + document.getElementById('SalesPeriod').value + '.csv',
        exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location"))
    };

    $scope.grid.onRegisterApi = function (gridApi) {
        $scope.gridApi = gridApi;
    };
    
    init();

    //initial loading when rendering the page
    function init() {
        //loading
        $scope.loading = true;
        var mybody = angular.element(document).find('body');
        mybody.addClass('waiting');

        var period = document.getElementById('SalesPeriod').value;
        var category = document.getElementById('SalesCategory').value;
        var isclientonly = parseInt(document.getElementById('IsClientOnly').value);
        var accountnumber = parseInt(document.getElementById('SalesAccountNumber').value);

        $http({
            url: "/SalesCategoryDetails",
            dataType: 'json',
            method: 'GET',
            data: '',
            headers: {
                "Content-Type": "application/json",
                "X-PeriodKey": period,
                "X-Category": category,
                "X-ClientOnly": isclientonly,
                "X-AccountNumber": accountnumber
            }
        }).
            then(
            function (result) {
                $scope.grid.data = result.data;
            },
            function (error) {
                InternalErrorHandler(error);
                $scope.loading = false;
                mybody.removeClass('waiting');
            }).finally(function () {
                $scope.loading = false;
                mybody.removeClass('waiting');
            });
    }
}