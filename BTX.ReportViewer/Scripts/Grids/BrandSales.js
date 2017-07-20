//SalesByProducts
app.controller('brandSalesCtrl', BrandSalesCtrl);

BrandSalesCtrl.$inject = ['$http', '$scope', 'GroupDropDown','GroupService'];
function BrandSalesCtrl($http, $scope, GroupDropDown, GroupService) {

    //UI Grid Setting
    $scope.gridOptions = {
        columnDefs: [
            { name: 'CSPC', field: 'ProductId', width: '5%', pinnedLeft: true, visible: true },
            { name: 'Product', field: "ProductName", width: '15%', cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;">{{COL_FIELD}}</div>' },
            { name: 'Size', field: "UnitSizeML", width: '5%', type: 'number' },
            { name: 'Unit', field: "UnitsPerCase", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'Price', field: "AvgPrice", width: '5%', cellFilter: 'currency:"$":2', type: 'number' },
            { name: 'Type', field: "Type", width: '5%', visible: false },
            { name: 'Dist', field: "Dist", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'Dist_Var', field: "Dist_Var", displayName: "Dist Var", width: '5%', cellFilter: 'number:0', type: 'number' },

            { name: 'Nb General', field: "Nb_General", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'Nb Vintages Essential', field: "Nb_VintageEssential", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'Nb Vintages', field: "Nb_Vintages", width: '5%', cellFilter: 'number:0', type: 'number' },

            { name: 'TY_9LCases', field: "TY_9LCases", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'LY_9LCases', field: "LY_9LCases", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'TY_9LCasesPct', field: "TY_9LCasesPct", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', type: 'number' },
            { name: 'TY_TotalSalesAmount', field: "TY_TotalSalesAmount", width: '5%', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'LY_TotalSalesAmount', field: "LY_TotalSalesAmount", width: '5%', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'TY_TotalSalesAmountPct', field: "TY_TotalSalesAmountPct", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', visible: false, type: 'number' },
            { name: 'TY_Units', field: "TY_Units", width: '5%', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'LY_Units', field: "LY_Units", width: '5%', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'TY_UnitsPct', field: "TY_UnitsPct", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', visible: false, type: 'number' },

            { name: 'TY6m_9LCases', field: "TY6m_9LCases", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'LY6m_9LCases', field: "LY6m_9LCases", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'TY6m_9LCasesPct', field: "TY6m_9LCasesPct", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', type: 'number' },
            { name: 'TY6m_TotalSalesAmount', field: "TY6m_TotalSalesAmount", width: '5%', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'LY6m_TotalSalesAmount', field: "LY6m_TotalSalesAmount", width: '5%', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'TY6m_TotalSalesAmountPct', field: "TY6m_TotalSalesAmountPct", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', visible: false, type: 'number' },
            { name: 'TY6m_Units', field: "TY6m_Units", width: '5%', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'LY6m_Units', field: "LY6m_Units", width: '5%', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'TY6m_UnitsPct', field: "TY6m_UnitsPct", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', visible: false, type: 'number' },

            { name: 'TY1y_9LCases', field: "TY1y_9LCases", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'LY1y_9LCases', field: "LY1y_9LCases", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'TY1y_9LCasesPct', field: "TY1y_9LCasesPct", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', type: 'number' },
            { name: 'TY1y_TotalSalesAmount', field: "TY1y_TotalSalesAmount", width: '5%', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'LY1y_TotalSalesAmount', field: "LY1y_TotalSalesAmount", width: '5%', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'TY1y_TotalSalesAmountPct', field: "TY1y_TotalSalesAmountPct", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', visible: false, type: 'number' },
            { name: 'TY1y_Units', field: "TY1y_Units", width: '5%', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'LY1y_Units', field: "LY1y_Units", width: '5%', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'TY1y_UnitsPct', field: "TY1y_UnitsPct", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', visible: false, type: 'number' },

            { name: 'Rol Year PerTY$', field: "TY1y_TotalSalesAmount", width: '5%', cellFilter: 'currency:"$":0', type: 'number' },
            { name: 'Rol Year Pct', field: "TY1y_TotalSalesAmountPct", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', type: 'number' },

            { name: 'On Prem 13 Per TY', field: "OnPrem13PerTY", displayName: "License TY", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'On Prem 13 Per LY', field: "OnPrem13PerLY", displayName: "License LY", width: '5%', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'TY_OnPrem13PerPct', field: "TY_OnPrem13PerPct", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', type: 'number' },

            { name: 'Agencies 13 Per TY', field: "Agencies13PerTY", displayName: "Agencies TY", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'Agencies 13 Per LY', field: "Agencies13PerLY", displayName: "Agencies LY", width: '5%', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'TY_Agencies13PerPct', field: "TY_Agencies13PerPct", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', type: 'number' },

            { name: 'Promo', field: "Promo", width: '15%' }
        ],
        enableSorting: true,
        enableRowSelection: true,
        enableRowHeaderSelection: true,
        enableCellSelection: false,
        enableCellEditOnFocus: false,
        enableGridMenu: true,
        exporterMenuPdf: false,
        exporterCsvFilename: 'Market Reports Sales by Products - Brand ' + document.getElementById('SalesBrandName').value + ' - Agent ' + document.getElementById('SalesAgentName').value + '.csv',
        exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location"))
    };

    $scope.gridOptions.onRegisterApi = function (gridApi) {
        $scope.gridApi = gridApi;
    };

    $scope.groups = GroupDropDown.groups;
    $scope.selectedgroup = (GroupService.getCatGroup($scope.currentUserInfo) != '' && GroupService.getCatGroup($scope.currentUserInfo) != null) ? GroupService.getCatGroup($scope.currentUserInfo) : GroupDropDown.groups[document.getElementById('SalesSelectedGroupId').value];

    init();

    //initial loading when rendering the page
    function init() {
        //loading
        $scope.loading = true;
        var mybody = angular.element(document).find('body');
        mybody.addClass('waiting');

        var period = document.getElementById('SalesPeriod').value;
        //var agentid = parseInt(document.getElementById('SalesAgentId').value);
        var setnames = document.getElementById('SalesSetNames').value;
        //var brandid = parseInt(document.getElementById('SalesBrandId').value);
        var unitsizes = document.getElementById('SalesUnitSizes').value;
        var pricefrom = parseFloat(document.getElementById('SalesPriceFrom').value);
        var priceto = parseFloat(document.getElementById('SalesPriceTo').value);
        var brandname = document.getElementById('SalesBrandName').value;
        var agentname = document.getElementById('SalesAgentName').value;
        //Info used in setting the Page
        $scope.currentUser = document.getElementById('currentUserInfo').value;
        $scope.currentUserInfo = $scope.currentUser + "GroupSelected";

        $http({
            url: "/BrandSales",
            dataType: 'json',
            method: 'GET',
            data: '',
            headers: {
                "Content-Type": "application/json",
                "X-PeriodKey": period,
                "X-AgentName": agentname,
                "X-BrandName": brandname,
                "X-SetNames": setnames,
                "X-UnitSizes": unitsizes,
                "X-PriceFrom": pricefrom,
                "X-PriceTo": priceto
            }
        }).
            then(
            function (result) {
                $scope.gridOptions.data = result.data;
            },
            function (error) {
                InternalErrorHandler(error);
                $scope.loading = false;
                mybody.removeClass('waiting');
            }).finally(function () {
                $scope.loading = false;
                mybody.removeClass('waiting');
            });

        //determine column visibility
        RefreshClick();
    }

    function RefreshClick() {

        //update ColDefs
        if ($scope.selectedgroup.id === 0) {

            $scope.gridOptions.columnDefs[11].visible = true;
            $scope.gridOptions.columnDefs[12].visible = true;
            $scope.gridOptions.columnDefs[13].visible = true;
            $scope.gridOptions.columnDefs[14].visible = false;
            $scope.gridOptions.columnDefs[15].visible = false;
            $scope.gridOptions.columnDefs[16].visible = false;
            $scope.gridOptions.columnDefs[17].visible = false;
            $scope.gridOptions.columnDefs[18].visible = false;
            $scope.gridOptions.columnDefs[19].visible = false;

            $scope.gridOptions.columnDefs[20].visible = true;
            $scope.gridOptions.columnDefs[21].visible = true;
            $scope.gridOptions.columnDefs[22].visible = true;
            $scope.gridOptions.columnDefs[23].visible = false;
            $scope.gridOptions.columnDefs[24].visible = false;
            $scope.gridOptions.columnDefs[25].visible = false;
            $scope.gridOptions.columnDefs[26].visible = false;
            $scope.gridOptions.columnDefs[27].visible = false;
            $scope.gridOptions.columnDefs[28].visible = false;

            $scope.gridOptions.columnDefs[29].visible = true;
            $scope.gridOptions.columnDefs[30].visible = true;
            $scope.gridOptions.columnDefs[31].visible = true;
            $scope.gridOptions.columnDefs[32].visible = false;
            $scope.gridOptions.columnDefs[33].visible = false;
            $scope.gridOptions.columnDefs[34].visible = false;
            $scope.gridOptions.columnDefs[35].visible = false;
            $scope.gridOptions.columnDefs[36].visible = false;
            $scope.gridOptions.columnDefs[37].visible = false;

        } else if ($scope.selectedgroup.id === 1) {

            $scope.gridOptions.columnDefs[11].visible = false;
            $scope.gridOptions.columnDefs[12].visible = false;
            $scope.gridOptions.columnDefs[13].visible = false;
            $scope.gridOptions.columnDefs[14].visible = true;
            $scope.gridOptions.columnDefs[15].visible = true;
            $scope.gridOptions.columnDefs[16].visible = true;
            $scope.gridOptions.columnDefs[17].visible = false;
            $scope.gridOptions.columnDefs[18].visible = false;
            $scope.gridOptions.columnDefs[19].visible = false;

            $scope.gridOptions.columnDefs[20].visible = false;
            $scope.gridOptions.columnDefs[21].visible = false;
            $scope.gridOptions.columnDefs[22].visible = false;
            $scope.gridOptions.columnDefs[23].visible = true;
            $scope.gridOptions.columnDefs[24].visible = true;
            $scope.gridOptions.columnDefs[25].visible = true;
            $scope.gridOptions.columnDefs[26].visible = false;
            $scope.gridOptions.columnDefs[27].visible = false;
            $scope.gridOptions.columnDefs[28].visible = false;

            $scope.gridOptions.columnDefs[29].visible = false;
            $scope.gridOptions.columnDefs[30].visible = false;
            $scope.gridOptions.columnDefs[31].visible = false;
            $scope.gridOptions.columnDefs[32].visible = true;
            $scope.gridOptions.columnDefs[33].visible = true;
            $scope.gridOptions.columnDefs[34].visible = true;
            $scope.gridOptions.columnDefs[35].visible = false;
            $scope.gridOptions.columnDefs[36].visible = false;
            $scope.gridOptions.columnDefs[37].visible = false;

        } else if ($scope.selectedgroup.id === 2) {

            $scope.gridOptions.columnDefs[11].visible = false;
            $scope.gridOptions.columnDefs[12].visible = false;
            $scope.gridOptions.columnDefs[13].visible = false;
            $scope.gridOptions.columnDefs[14].visible = false;
            $scope.gridOptions.columnDefs[15].visible = false;
            $scope.gridOptions.columnDefs[16].visible = false;
            $scope.gridOptions.columnDefs[17].visible = true;
            $scope.gridOptions.columnDefs[18].visible = true;
            $scope.gridOptions.columnDefs[19].visible = true;

            $scope.gridOptions.columnDefs[20].visible = false;
            $scope.gridOptions.columnDefs[21].visible = false;
            $scope.gridOptions.columnDefs[22].visible = false;
            $scope.gridOptions.columnDefs[23].visible = false;
            $scope.gridOptions.columnDefs[24].visible = false;
            $scope.gridOptions.columnDefs[25].visible = false;
            $scope.gridOptions.columnDefs[26].visible = true;
            $scope.gridOptions.columnDefs[27].visible = true;
            $scope.gridOptions.columnDefs[28].visible = true;

            $scope.gridOptions.columnDefs[29].visible = false;
            $scope.gridOptions.columnDefs[30].visible = false;
            $scope.gridOptions.columnDefs[31].visible = false;
            $scope.gridOptions.columnDefs[32].visible = false;
            $scope.gridOptions.columnDefs[33].visible = false;
            $scope.gridOptions.columnDefs[34].visible = false;
            $scope.gridOptions.columnDefs[35].visible = true;
            $scope.gridOptions.columnDefs[36].visible = true;
            $scope.gridOptions.columnDefs[37].visible = true;

        }
    }
}