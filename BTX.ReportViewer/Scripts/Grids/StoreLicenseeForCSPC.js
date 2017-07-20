//WeeklySalesOntario

app.controller('StoreLicenseeForCSPCCtrl', ['$scope', '$http', '$filter', 'GroupDropDown', 'GroupService', function ($scope, $http, $filter, GroupDropDown, GroupService) {
    //var today = new Date();
    $scope.gridLCBO = {
        enableFiltering: false,
        onRegisterApi: function (gridApi) {
            $scope.gridLCBOApi = gridApi

        },
        columnDefs: [
            { name: 'Store Number', field: "Code", width: '5%', cellTemplate: '<div class="ui-grid-cell-contents"><a ng-click="grid.appScope.salespersonalstore(row.entity.Code, row.entity.AccountName)" style="cursor:pointer;">{{COL_FIELD}}</a></div>' },
            { name: 'Store Name', field: "AccountName", width: '20%', cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;"><a ng-click="grid.appScope.salespersonalstore(row.entity.Code, row.entity.AccountName)" style="cursor:pointer;">{{COL_FIELD}}</a></div>' },
            { name: 'City', field: "City", width: '5%', cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;">{{COL_FIELD}}</div>' },
            { name: 'Address', field: "Address", width: '10%', cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;">{{COL_FIELD}}</div>' },
            { name: 'Rank9LCases', field: "Rank9LCases", displayName: "Rank", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'RankSalesAmount', field: "RankSalesAmount", displayName: "Rank", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'RankUnits', field: "RankUnits", displayName: "Rank", width: '5%', cellFilter: 'number:0', type: 'number' },

            { name: 'TY_13P_9LCases', field: "TY_13P_9LCases", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'LY_13P_9LCases', field: "LY_13P_9LCases", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'Pct_13P_9LCases', field: "Pct_13P_9LCases", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', type: 'number' },
            { name: 'TY_13P_SalesAmount', field: "TY_13P_SalesAmount", width: '5%', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'LY_13P_SalesAmount', field: "LY_13P_SalesAmount", width: '5%', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'Pct_13P_SalesAmount', field: "Pct_13P_SalesAmount", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', visible: false, type: 'number' },
            { name: 'TY_13P_Units', field: "TY_13P_Units", width: '5%', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'LY_13P_Units', field: "LY_13P_Units", width: '5%', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'Pct_13P_Units', field: "Pct_13P_Units", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', visible: false, type: 'number' },

            { name: 'TY_3P_9LCases', field: "TY_3P_9LCases", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'LY_3P_9LCases', field: "LY_3P_9LCases", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'Pct_3P_9LCases', field: "Pct_3P_9LCases", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', type: 'number' },
            { name: 'TY_3P_SalesAmount', field: "TY_3P_SalesAmount", width: '5%', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'LY_3P_SalesAmount', field: "LY_3P_SalesAmount", width: '5%', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'Pct_3P_SalesAmount', field: "Pct_3P_SalesAmount", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', visible: false, type: 'number' },
            { name: 'TY_3P_Units', field: "TY_3P_Units", width: '5%', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'LY_3P_Units', field: "LY_3P_Units", width: '5%', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'Pct_3P_Units', field: "Pct_3P_Units", width: '5%', displayName: "Var %", cellFilter: 'mapPercentage', visible: false, type: 'number' },

            { name: 'TY_13P_9LCases_DirectSales', field: "TY_13P_9LCases_DirectSales", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'LY_13P_9LCases_DirectSales', field: "LY_13P_9LCases_DirectSales", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'Pct_13P_9LCases_DirectSales', field: "Pct_13P_9LCases_DirectSales", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', type: 'number' },
            { name: 'TY_13P_SalesAmount_DirectSales', field: "TY_13P_SalesAmount_DirectSales", width: '5%', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'LY_13P_SalesAmount_DirectSales', field: "LY_13P_SalesAmount_DirectSales", width: '5%', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'Pct_13P_SalesAmount_DirectSales', field: "Pct_13P_SalesAmount_DirectSales", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', visible: false, type: 'number' },
            { name: 'TY_13P_Units_DirectSales', field: "TY_13P_Units_DirectSales", width: '5%', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'LY_13P_Units_DirectSales', field: "LY_13P_Units_DirectSales", width: '5%', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'Pct_13P_Units_DirectSales', field: "Pct_13P_Units_DirectSales", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', visible: false, type: 'number' },

            { name: 'TY_3P_9LCases_DirectSales', field: "TY_3P_9LCases_DirectSales", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'LY_3P_9LCases_DirectSales', field: "LY_3P_9LCases_DirectSales", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'Pct_3P_9LCases_DirectSales', field: "Pct_3P_9LCases_DirectSales", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', type: 'number' },
            { name: 'TY_3P_SalesAmount_DirectSales', field: "TY_3P_SalesAmount_DirectSales", width: '5%', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'LY_3P_SalesAmount_DirectSales', field: "LY_3P_SalesAmount_DirectSales", width: '5%', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'Pct_3P_SalesAmount_DirectSales', field: "Pct_3P_SalesAmount_DirectSales", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', visible: false, type: 'number' },
            { name: 'TY_3P_Units_DirectSales', field: "TY_3P_Units_DirectSales", width: '5%', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'LY_3P_Units_DirectSales', field: "LY_3P_Units_DirectSales", width: '5%', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'Pct_3P_Units_DirectSales', field: "Pct_3P_Units_DirectSales", width: '5%', displayName: "Var %", cellFilter: 'mapPercentage', visible: false, type: 'number' },

            { name: 'TY_13P_9LCases_Licensee', field: "TY_13P_9LCases_Licensee", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'LY_13P_9LCases_Licensee', field: "LY_13P_9LCases_Licensee", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'Pct_13P_9LCases_Licensee', field: "Pct_13P_9LCases_Licensee", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', type: 'number' },
            { name: 'TY_13P_SalesAmount_Licensee', field: "TY_13P_SalesAmount_Licensee", width: '5%', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'LY_13P_SalesAmount_Licensee', field: "LY_13P_SalesAmount_Licensee", width: '5%', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'Pct_13P_SalesAmount_Licensee', field: "Pct_13P_SalesAmount_Licensee", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', visible: false, type: 'number' },
            { name: 'TY_13P_Units_Licensee', field: "TY_13P_Units_Licensee", width: '5%', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'LY_13P_Units_Licensee', field: "LY_13P_Units_Licensee", width: '5%', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'Pct_13P_Units_Licensee', field: "Pct_13P_Units_Licensee", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', visible: false, type: 'number' },

            { name: 'TY_3P_9LCases_Licensee', field: "TY_3P_9LCases_Licensee", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'LY_3P_9LCases_Licensee', field: "LY_3P_9LCases_Licensee", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'Pct_3P_9LCases_Licensee', field: "Pct_3P_9LCases_Licensee", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', type: 'number' },
            { name: 'TY_3P_SalesAmount_Licensee', field: "TY_3P_SalesAmount_Licensee", width: '5%', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'LY_3P_SalesAmount_Licensee', field: "LY_3P_SalesAmount_Licensee", width: '5%', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'Pct_3P_SalesAmount_Licensee', field: "Pct_3P_SalesAmount_Licensee", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', visible: false, type: 'number' },
            { name: 'TY_3P_Units_Licensee', field: "TY_3P_Units_Licensee", width: '5%', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'LY_3P_Units_Licensee', field: "LY_3P_Units_Licensee", width: '5%', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'Pct_3P_Units_Licensee', field: "Pct_3P_Units_Licensee", width: '5%', displayName: "Var %", cellFilter: 'mapPercentage', visible: false, type: 'number' }
        ],
        enableSorting: true,
        enableRowSelection: true,
        enableRowHeaderSelection: true,
        enableCellSelection: false,
        enableCellEditOnFocus: false,
        enableGridMenu: true,
        exporterMenuPdf: false
    };

    $scope.gridLic = {
        enableFiltering: false,
        onRegisterApi: function (gridApi) {
            $scope.gridLicApi = gridApi;
        },
        columnDefs: [
            { name: 'Licensee Number', field: "Code", width: '5%', cellTemplate: '<div class="ui-grid-cell-contents"><a ng-click="grid.appScope.salespersonallicensee(row.entity.Code, row.entity.AccountName)" style="cursor:pointer;">{{COL_FIELD}}</div>' },
            { name: 'Licensee Name', field: "AccountName", width: '20%', cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;"><a ng-click="grid.appScope.salespersonallicensee(row.entity.Code, row.entity.AccountName)" style="cursor:pointer;">{{COL_FIELD}}</a></div>' },
            { name: 'City', field: "City", width: '5%', cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;">{{COL_FIELD}}</div>' },
            { name: 'Address', field: "Address", width: '10%', cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;">{{COL_FIELD}}</div>' },
            { name: 'Rank9LCases', field: "Rank9LCases", displayName: "Rank", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'RankSalesAmount', field: "RankSalesAmount", displayName: "Rank", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'RankUnits', field: "RankUnits", displayName: "Rank", width: '5%', cellFilter: 'number:0', type: 'number' },

            { name: 'TY_13P_9LCases', field: "TY_13P_9LCases", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'LY_13P_9LCases', field: "LY_13P_9LCases", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'Pct_13P_9LCases', field: "Pct_13P_9LCases", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', type: 'number' },
            { name: 'TY_13P_SalesAmount', field: "TY_13P_SalesAmount", width: '5%', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'LY_13P_SalesAmount', field: "LY_13P_SalesAmount", width: '5%', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'Pct_13P_SalesAmount', field: "Pct_13P_SalesAmount", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', visible: false, type: 'number' },
            { name: 'TY_13P_Units', field: "TY_13P_Units", width: '5%', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'LY_13P_Units', field: "LY_13P_Units", width: '5%', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'Pct_13P_Units', field: "Pct_13P_Units", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', visible: false, type: 'number' },

            { name: 'TY_3P_9LCases', field: "TY_3P_9LCases", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'LY_3P_9LCases', field: "LY_3P_9LCases", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'Pct_3P_9LCases', field: "Pct_3P_9LCases", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', type: 'number' },
            { name: 'TY_3P_SalesAmount', field: "TY_3P_SalesAmount", width: '5%', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'LY_3P_SalesAmount', field: "LY_3P_SalesAmount", width: '5%', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'Pct_3P_SalesAmount', field: "Pct_3P_SalesAmount", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', visible: false, type: 'number' },
            { name: 'TY_3P_Units', field: "TY_3P_Units", width: '5%', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'LY_3P_Units', field: "LY_3P_Units", width: '5%', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'Pct_3P_Units', field: "Pct_3P_Units", width: '5%', displayName: "Var %", cellFilter: 'mapPercentage', visible: false, type: 'number' },

            { name: 'TY_13P_9LCases_DirectSales', field: "TY_13P_9LCases_DirectSales", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'LY_13P_9LCases_DirectSales', field: "LY_13P_9LCases_DirectSales", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'Pct_13P_9LCases_DirectSales', field: "Pct_13P_9LCases_DirectSales", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', type: 'number' },
            { name: 'TY_13P_SalesAmount_DirectSales', field: "TY_13P_SalesAmount_DirectSales", width: '5%', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'LY_13P_SalesAmount_DirectSales', field: "LY_13P_SalesAmount_DirectSales", width: '5%', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'Pct_13P_SalesAmount_DirectSales', field: "Pct_13P_SalesAmount_DirectSales", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', visible: false, type: 'number' },
            { name: 'TY_13P_Units_DirectSales', field: "TY_13P_Units_DirectSales", width: '5%', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'LY_13P_Units_DirectSales', field: "LY_13P_Units_DirectSales", width: '5%', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'Pct_13P_Units_DirectSales', field: "Pct_13P_Units_DirectSales", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', visible: false, type: 'number' },

            { name: 'TY_3P_9LCases_DirectSales', field: "TY_3P_9LCases_DirectSales", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'LY_3P_9LCases_DirectSales', field: "LY_3P_9LCases_DirectSales", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'Pct_3P_9LCases_DirectSales', field: "Pct_3P_9LCases_DirectSales", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', type: 'number' },
            { name: 'TY_3P_SalesAmount_DirectSales', field: "TY_3P_SalesAmount_DirectSales", width: '5%', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'LY_3P_SalesAmount_DirectSales', field: "LY_3P_SalesAmount_DirectSales", width: '5%', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'Pct_3P_SalesAmount_DirectSales', field: "Pct_3P_SalesAmount_DirectSales", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', visible: false, type: 'number' },
            { name: 'TY_3P_Units_DirectSales', field: "TY_3P_Units_DirectSales", width: '5%', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'LY_3P_Units_DirectSales', field: "LY_3P_Units_DirectSales", width: '5%', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'Pct_3P_Units_DirectSales', field: "Pct_3P_Units_DirectSales", width: '5%', displayName: "Var %", cellFilter: 'mapPercentage', visible: false, type: 'number' },

            { name: 'TY_13P_9LCases_Licensee', field: "TY_13P_9LCases_Licensee", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'LY_13P_9LCases_Licensee', field: "LY_13P_9LCases_Licensee", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'Pct_13P_9LCases_Licensee', field: "Pct_13P_9LCases_Licensee", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', type: 'number' },
            { name: 'TY_13P_SalesAmount_Licensee', field: "TY_13P_SalesAmount_Licensee", width: '5%', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'LY_13P_SalesAmount_Licensee', field: "LY_13P_SalesAmount_Licensee", width: '5%', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'Pct_13P_SalesAmount_Licensee', field: "Pct_13P_SalesAmount_Licensee", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', visible: false, type: 'number' },
            { name: 'TY_13P_Units_Licensee', field: "TY_13P_Units_Licensee", width: '5%', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'LY_13P_Units_Licensee', field: "LY_13P_Units_Licensee", width: '5%', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'Pct_13P_Units_Licensee', field: "Pct_13P_Units_Licensee", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', visible: false, type: 'number' },

            { name: 'TY_3P_9LCases_Licensee', field: "TY_3P_9LCases_Licensee", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'LY_3P_9LCases_Licensee', field: "LY_3P_9LCases_Licensee", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'Pct_3P_9LCases_Licensee', field: "Pct_3P_9LCases_Licensee", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', type: 'number' },
            { name: 'TY_3P_SalesAmount_Licensee', field: "TY_3P_SalesAmount_Licensee", width: '5%', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'LY_3P_SalesAmount_Licensee', field: "LY_3P_SalesAmount_Licensee", width: '5%', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'Pct_3P_SalesAmount_Licensee', field: "Pct_3P_SalesAmount_Licensee", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', visible: false, type: 'number' },
            { name: 'TY_3P_Units_Licensee', field: "TY_3P_Units_Licensee", width: '5%', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'LY_3P_Units_Licensee', field: "LY_3P_Units_Licensee", width: '5%', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'Pct_3P_Units_Licensee', field: "Pct_3P_Units_Licensee", width: '5%', displayName: "Var %", cellFilter: 'mapPercentage', visible: false, type: 'number' }
        ],
        enableSorting: true,
        enableRowSelection: true,
        enableRowHeaderSelection: true,
        enableCellSelection: false,
        enableCellEditOnFocus: false,
        enableGridMenu: true,
        exporterMenuPdf: false
    };

    init();

    $scope.periodkeydropboxitemselected = function (item) {
        $scope.selectedperiodkey = item;
    }

    $scope.groups = GroupDropDown.groups;
    $scope.selectedgroup = (GroupService.getCatGroup($scope.currentUserInfo) != '' && GroupService.getCatGroup($scope.currentUserInfo) != null) ? GroupService.getCatGroup($scope.currentUserInfo) : GroupDropDown.groups[1];

    $scope.groupdropboxitemselected = function (item) {
        GroupService.setCatGroup($scope.currentUserInfo, item)
        $scope.selectedgroup = item;
    }

    //initial loading when rendering the page
    function init() {

        //Info used in setting the Page
        $scope.currentUser = document.getElementById('currentUserInfo').value;
        $scope.currentUserInfo = $scope.currentUser + "GroupSelected";

        //cursor & page loading
        $scope.loadingLCBO = true;
        $scope.loadingLic = true;
        var mybody = angular.element(document).find('body');
        mybody.addClass('waiting')

        var periodkey = document.getElementById("CSPCPeriod").value;
        var cspc = document.getElementById("CSPC").value;
        var groupid = parseInt(document.getElementById("SelectedGroupId").value);

        if (periodkey == "") {
            $http({
                url: "/PeriodKeys",
                dataType: 'json',
                method: 'POST',
                data: '',
                headers: {
                    "Content-Type": "application/json"
                }
            }).
                then(
                function (periodresult) {
                    $scope.periodkeys = periodresult.data;
                    $scope.selectedperiodkey = $scope.periodkeys[0];
                    periodkey = $scope.selectedperiodkey.label;

                    $http({
                        url: "/CSPCSearchStore",
                        dataType: 'json',
                        method: 'GET',
                        data: '',
                        headers: {
                            "Content-Type": "application/json",
                            "X-PeriodKey": periodkey,
                            "X-CSPC": cspc,
                            "X-GroupId": groupid
                        }
                    }).
                        then(
                        function (result) {
                            $scope.gridLCBO.data = result.data;
                        },
                        function (error) {
                            InternalErrorHandler(error);
                            $scope.loadingLCBO = false;
                            if (!$scope.loadingLic)
                                mybody.removeClass('waiting');
                        }).
                        finally(function () {
                            $scope.loadingLCBO = false;
                            if (!$scope.loadingLic)
                                mybody.removeClass('waiting');
                        })

                    $http({
                        url: "/CSPCSearchLicensee",
                        dataType: 'json',
                        method: 'GET',
                        data: '',
                        headers: {
                            "Content-Type": "application/json",
                            "X-PeriodKey": periodkey,
                            "X-CSPC": cspc,
                            "X-GroupId": groupid
                        }
                    }).
                        then(
                        function (result) {
                            $scope.gridLic.data = result.data;
                        },
                        function (error) {
                            InternalErrorHandler(error);
                            $scope.loadingLic = false;
                            if (!$scope.loadingLCBO)
                                mybody.removeClass('waiting');
                        }).
                        finally(function () {
                            $scope.loadingLic = false;
                            if (!$scope.loadingLCBO)
                                mybody.removeClass('waiting');
                        })
                },
                function (perioderror) {
                    InternalErrorHandler(perioderror);
                });
        } else {
            $http({
                url: "/CSPCSearchStore",
                dataType: 'json',
                method: 'GET',
                data: '',
                headers: {
                    "Content-Type": "application/json",
                    "X-PeriodKey": periodkey,
                    "X-CSPC": cspc,
                    "X-GroupId": groupid
                }
            }).
                then(
                function (result) {
                    $scope.gridLCBO.data = result.data;
                },
                function (error) {
                    InternalErrorHandler(error);
                    $scope.loadingLCBO = false;
                    if (!$scope.loadingLic)
                        mybody.removeClass('waiting');
                }).
                finally(function () {
                    $scope.loadingLCBO = false;
                    if (!$scope.loadingLic)
                        mybody.removeClass('waiting');
                })

            $http({
                url: "/CSPCSearchLicensee",
                dataType: 'json',
                method: 'GET',
                data: '',
                headers: {
                    "Content-Type": "application/json",
                    "X-PeriodKey": periodkey,
                    "X-CSPC": cspc,
                    "X-GroupId": groupid
                }
            }).
                then(
                function (result) {
                    $scope.gridLic.data = result.data;
                },
                function (error) {
                    InternalErrorHandler(error);
                    $scope.loadingLic = false;
                    if (!$scope.loadingLCBO)
                        mybody.removeClass('waiting');
                }).
                finally(function () {
                    $scope.loadingLic = false;
                    if (!$scope.loadingLCBO)
                        mybody.removeClass('waiting');
                })
        }
        
        AdjustColumnVisibility(groupid);
    }

    function AdjustColumnVisibility(groupid) {
        
        //9L Cases
        if (groupid === 0) {
            $scope.gridLCBO.columnDefs[4].visible = true;
            $scope.gridLCBO.columnDefs[5].visible = false;
            $scope.gridLCBO.columnDefs[6].visible = false;

            $scope.gridLic.columnDefs[4].visible = true;
            $scope.gridLic.columnDefs[5].visible = false;
            $scope.gridLic.columnDefs[6].visible = false;
        //Sales Amount
        } else if (groupid === 1) {
            $scope.gridLCBO.columnDefs[4].visible = false;
            $scope.gridLCBO.columnDefs[5].visible = true;
            $scope.gridLCBO.columnDefs[6].visible = false;

            $scope.gridLic.columnDefs[4].visible = false;
            $scope.gridLic.columnDefs[5].visible = true;
            $scope.gridLic.columnDefs[6].visible = false;
        //Units
        } else {
            $scope.gridLCBO.columnDefs[4].visible = false;
            $scope.gridLCBO.columnDefs[5].visible = false;
            $scope.gridLCBO.columnDefs[6].visible = true;

            $scope.gridLic.columnDefs[4].visible = false;
            $scope.gridLic.columnDefs[5].visible = false;
            $scope.gridLic.columnDefs[6].visible = true;
        }

        SetEachColumnVisibility(7, groupid);
        SetEachColumnVisibility(16, groupid);
        SetEachColumnVisibility(25, groupid);
        SetEachColumnVisibility(34, groupid);
        SetEachColumnVisibility(43, groupid);
        SetEachColumnVisibility(52, groupid);
    };

    function SetEachColumnVisibility(initialstep, groupid) {
        for (var i = 0; i < 9; ++i) {
            if (groupid === 0) {
                if (i >= 0 && i < 3) {
                    $scope.gridLCBO.columnDefs[initialstep + i].visible = true;
                    $scope.gridLic.columnDefs[initialstep + i].visible = true;
                } else {
                    $scope.gridLCBO.columnDefs[initialstep + i].visible = false;
                    $scope.gridLic.columnDefs[initialstep + i].visible = false;
                }
            } else if (groupid === 1) {
                if (i >= 3 && i < 6) {
                    $scope.gridLCBO.columnDefs[initialstep + i].visible = true;
                    $scope.gridLic.columnDefs[initialstep + i].visible = true;
                } else {
                    $scope.gridLCBO.columnDefs[initialstep + i].visible = false;
                    $scope.gridLic.columnDefs[initialstep + i].visible = false;
                }
            } else {
                if (i >= 6 && i < 9) {
                    $scope.gridLCBO.columnDefs[initialstep + i].visible = true;
                    $scope.gridLic.columnDefs[initialstep + i].visible = true;
                } else {
                    $scope.gridLCBO.columnDefs[initialstep + i].visible = false;
                    $scope.gridLic.columnDefs[initialstep + i].visible = false;
                }
            }
        }
    }

    //Refresh button click functions
    $scope.Reload = function () {
        var periodkey = $scope.selectedperiodkey.label;
        var groupid = $scope.selectedgroup.id;
        var cspc = document.getElementById("CSPC").value;

        $scope.loadingLic = true;
        $scope.loadingLCBO = true;
        var mybody = angular.element(document).find('body');
        mybody.addClass('waiting');

        $http({
            url: "/CSPCSearchStore",
            dataType: 'json',
            method: 'GET',
            data: '',
            headers: {
                "Content-Type": "application/json",
                "X-PeriodKey": periodkey,
                "X-CSPC": cspc,
                "X-GroupId": groupid
            }
        }).
            then(
            function (result) {
                $scope.gridLCBO.data = result.data;
            },
            function (error) {
                InternalErrorHandler(error);
                $scope.loadingLCBO = false;
                if (!$scope.loadingLic)
                    mybody.removeClass('waiting');
            }).
            finally(function () {
                $scope.loadingLCBO = false;
                if (!$scope.loadingLic)
                    mybody.removeClass('waiting');
            })

        $http({
            url: "/CSPCSearchLicensee",
            dataType: 'json',
            method: 'GET',
            data: '',
            headers: {
                "Content-Type": "application/json",
                "X-PeriodKey": periodkey,
                "X-CSPC": cspc,
                "X-GroupId": groupid
            }
        }).
            then(
            function (result) {
                $scope.gridLic.data = result.data;
            },
            function (error) {
                InternalErrorHandler(error);
                $scope.loadingLic = false;
                if (!$scope.loadingLCBO)
                    mybody.removeClass('waiting');
            }).
            finally(function () {
                $scope.loadingLic = false;
                if (!$scope.loadingLCBO)
                    mybody.removeClass('waiting');
            })

        AdjustColumnVisibility(groupid);
    }

    //subgrid control for each store
    $scope.salespersonalstore = function (storenumber, storename) {

        var period = document.getElementById("CSPCPeriod").value;
        var headeroption = parseInt(document.getElementById('HeaderOption').value);
        var selectedgroupid = parseInt(document.getElementById("SelectedGroupId").value);

        //create a dynamic form
        var f = document.createElement("form");
        f.setAttribute('id', "salesteamthreesubgridsform");
        f.setAttribute('method', "post");
        f.setAttribute('action', document.getElementById('SalesPersonalStoreUrl').value);
        //target blank will post it to a new tab
        f.setAttribute("target", "_blank");
        //append the form to the bottom of the body
        document.body.appendChild(f);
        //create hidden elements and append them to the form
        f.appendChild(GroupDropDown.createFormElement("userid", 0));
        f.appendChild(GroupDropDown.createFormElement("period", period));
        f.appendChild(GroupDropDown.createFormElement("accountnumber", storenumber));
        f.appendChild(GroupDropDown.createFormElement("salesname", ""));
        f.appendChild(GroupDropDown.createFormElement("accountname", storename));
        f.appendChild(GroupDropDown.createFormElement("selectedgroupid", selectedgroupid));
        if (headeroption === 2) {
            f.appendChild(GroupDropDown.createFormElement("headeroption", headeroption));
        }
        //submit form
        f.submit();
        //remove the newly created form after submit
        f.remove();
    }

    //subgrid control for each licensee
    $scope.salespersonallicensee = function (licenseenumber, licenseename) {

        var period = document.getElementById("CSPCPeriod").value;
        var headeroption = parseInt(document.getElementById('HeaderOption').value);
        var selectedgroupid = parseInt(document.getElementById("SelectedGroupId").value);

        //create a dynamic form
        var f = document.createElement("form");
        f.setAttribute('id', "salesteamthreesubgridsform");
        f.setAttribute('method', "post");
        f.setAttribute('action', document.getElementById('SalesPersonalLicenseeUrl').value);
        //target blank will post it to a new tab
        f.setAttribute("target", "_blank");
        //append the form to the bottom of the body
        document.body.appendChild(f);
        //create hidden elements and append them to the form
        f.appendChild(GroupDropDown.createFormElement("userid", 0));
        f.appendChild(GroupDropDown.createFormElement("period", period));
        f.appendChild(GroupDropDown.createFormElement("accountnumber", licenseenumber));
        f.appendChild(GroupDropDown.createFormElement("salesname", ""));
        f.appendChild(GroupDropDown.createFormElement("accountname", licenseename));
        f.appendChild(GroupDropDown.createFormElement("selectedgroupid", selectedgroupid));
        if (headeroption === 2) {
            f.appendChild(GroupDropDown.createFormElement("headeroption", headeroption));
        }
        //submit form
        f.submit();
        //remove the newly created form after submit
        f.remove();
    }
    
}])




