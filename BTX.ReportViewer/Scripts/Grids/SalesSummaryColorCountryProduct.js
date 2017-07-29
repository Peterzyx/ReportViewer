//SalesByProducts
app.controller('SalesSummaryColorCountryProductCtrl', SalesSummaryColorCountryProductCtrl);

SalesSummaryColorCountryProductCtrl.$inject = ['$http', '$scope', 'GroupDropDown', 'uiGridConstants','GroupService'];
function SalesSummaryColorCountryProductCtrl($http, $scope, GroupDropDown, uiGridConstants, GroupService) {

    //UI Grid Setting
    $scope.grid = {
        columnDefs: [

            { name: 'Product', field: "Product", width: 350, cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;">{{COL_FIELD}}</a></div>', footerCellTemplate: '<div class="ui-grid-cell-contents">Total:</div>' },

            { name: 'Nb Regular', field: "Nb_General", width: '5%', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'Nb Vintages Essential', field: "Nb_VintageEssential", width: '5%', visible: false, cellFilter: 'number:0', type: 'number' },
            { name: 'Nb Vintages', field: "Nb_Vintages", width: '5%', visible: false, cellFilter: 'number:0', type: 'number' },

            { name: 'LY_9LCases', field: "LY_9LCases", displayName: "LY", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_9LCases', field: "TY_9LCases", displayName: "TY", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_9LCasesPct', field: "TY_9LCasesPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Product_TY_9LCasesPct"></span></div>' },
            { name: 'LY_Units', field: "LY_Units", displayName: "LY", width: 120, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_Units', field: "TY_Units", displayName: "TY", width: 120, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_UnitsPct', field: "TY_UnitsPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Product_TY_UnitsPct"></span></div>' },
            { name: 'LY_TotalSalesAmount', field: "LY_TotalSalesAmount", displayName: "LY", width: 120, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TY_TotalSalesAmount', field: "TY_TotalSalesAmount", displayName: "TY", width: 120, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TY_TotalSalesAmountPct', field: "TY_TotalSalesAmountPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Product_TY_TotalSalesAmountPct"></span></div>' },

            { name: 'Week1', field: "Week1", displayName: "WK1", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'Week2', field: "Week2", displayName: "WK2", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'Week3', field: "Week3", displayName: "WK3", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'Week4', field: "Week4", displayName: "WK4", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },

            { name: 'LY_TD_9LCases', field: "LY_TD_9LCases", displayName: "P12 TD LY", width: 90, cellFilter: 'number:0', type: 'number',visible:false, aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_TD_9LCases', field: "TY_TD_9LCases", displayName: "P12 TD TY", width: 90, cellFilter: 'number:0', type: 'number', visible:false, aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_TD_9LCasesPct', field: "TY_TD_9LCasesPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', type: 'number', visible:false, footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Product_TY_TD_9LCasesPct"></span></div>' },
            { name: 'LY_TD_Units', field: "LY_TD_Units", displayName: "P12 TD LY", width: 90, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_TD_Units', field: "TY_TD_Units", displayName: "P12 TD TY", width: 90, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_TD_UnitsPct', field: "TY_TD_UnitsPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Product_TY_TD_UnitsPct"></span></div>' },
            { name: 'LY_TD_TotalSalesAmount', field: "LY_TD_TotalSalesAmount", displayName: "P12 TD LY", width: 90, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TY_TD_TotalSalesAmount', field: "TY_TD_TotalSalesAmount", displayName: "P12 TD TY", width: 90, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TY_TD_TotalSalesAmountPct', field: "TY_TD_TotalSalesAmountPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Product_TY_TD_TotalSalesAmountPct"></span></div>' },

            { name: 'LY6m_9LCases', field: "LY6m_9LCases", displayName: "6 Last Per LY", width: 110, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY6m_9LCases', field: "TY6m_9LCases", displayName: "6 Last Per TY", width: 110, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY6m_9LCasesPct', field: "TY6m_9LCasesPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Product_TY6m_9LCasesPct"></span></div>' },
            { name: 'LY6m_Units', field: "LY6m_Units", displayName: "6 Last Per LY", width: 110, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY6m_Units', field: "TY6m_Units", displayName: "6 Last Per TY", width: 110, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY6m_UnitsPct', field: "TY6m_UnitsPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Product_TY6m_UnitsPct"></span></div>' },
            { name: 'LY6m_TotalSalesAmount', field: "LY6m_TotalSalesAmount", displayName: "6 Last Per LY", width: 110, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TY6m_TotalSalesAmount', field: "TY6m_TotalSalesAmount", displayName: "6 Last Per TY", width: 110, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TY6m_TotalSalesAmountPct', field: "TY6m_TotalSalesAmountPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Product_TY6m_TotalSalesAmountPct"></span></div>' },

            { name: 'LYYTD_9LCases', field: "LYYTD_9LCases", displayName: "YTD LY", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TYYTD_9LCases', field: "TYYTD_9LCases", displayName: "YTD TY", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TYYTD_9LCasesPct', field: "TYYTD_9LCasesPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Product_TYYTD_9LCasesPct"></span></div>' },
            { name: 'LYYTD_Units', field: "LYYTD_Units", displayName: "YTD LY", width: 120, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TYYTD_Units', field: "TYYTD_Units", displayName: "YTD TY", width: 120, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TYYTD_UnitsPct', field: "TYYTD_UnitsPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Product_TYYTD_UnitsPct"></span></div>' },
            { name: 'LYYTD_SalesAmount', field: "LYYTD_SalesAmount", displayName: "YTD LY", width: 120, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TYYTD_SalesAmount', field: "TYYTD_SalesAmount", displayName: "YTD TY", width: 120, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TYYTD_SalesAmountPct', field: "TYYTD_SalesAmountPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Product_TYYTD_SalesAmountPct"></span></div>' },

            { name: 'LY13_OnPrem', field: "LY13_OnPrem", displayName: "13 Last Per LY", width: 115, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY13_OnPrem', field: "TY13_OnPrem", displayName: "13 Last per TY", width: 115, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY13_OnPremPct', field: "TY13_OnPremPct", displayName: "Var %", width: 115, cellFilter: 'mapPercentage', type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Product_TY13_OnPremPct"></span></div>' },

            { name: 'TY13Amount', field: "TY13Amount", displayName: "13 Last Per TY$", width: 120, cellFilter: 'currency:"$":0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TY13AmountPct', field: "TY13AmountPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Product_TY13AmountPct"></span></div>' }

        ],
        enableSorting: true,
        enableRowSelection: true,
        enableRowHeaderSelection: true,
        enableCellSelection: false,
        enableCellEditOnFocus: false,
        enableGridMenu: true,
        exporterMenuPdf: false,
        showColumnFooter: true,
        exporterCsvFilename: 'Sales Summary by Color - ' + document.getElementById('ColorName').value + ' Country ' + document.getElementById('CountryName').value + '.csv',
        exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location"))
    };

    $scope.grid.onRegisterApi = function (gridApi) {
        $scope.gridApi = gridApi;
    };

    initUser();
    $scope.groups = GroupDropDown.groups;
    $scope.selectedgroup = (GroupService.getCatGroup($scope.currentUserInfo) != '' && GroupService.getCatGroup($scope.currentUserInfo) != null) ? GroupService.getCatGroup($scope.currentUserInfo) : GroupDropDown.groups[document.getElementById('SalesSelectedGroupId').value];

    init();

    function initUser() {

        //Info used in setting the Page
        $scope.currentUser = document.getElementById('currentUserInfo').value;
        $scope.currentUserInfo = $scope.currentUser + "GroupSelected";
        $scope.currentUserPrdInfo = $scope.currentUser + "ProductCategorySelected";

    }

    //initial loading when rendering the page
    function init() {
        //loading
        $scope.loading = true;
        var mybody = angular.element(document).find('body');
        mybody.addClass('waiting');

        var period = document.getElementById('SalesPeriod').value;
        var setnames = document.getElementById('SalesSetNames').value;
        var unitsizes = document.getElementById('SalesUnitSizes').value;
        var pricefrom = parseFloat(document.getElementById('SalesPriceFrom').value);
        var priceto = parseFloat(document.getElementById('SalesPriceTo').value);
        var colorname = document.getElementById('ColorName').value;
        var countryname = document.getElementById('CountryName').value;

        $http({
            url: "/SalesSummaryColorCountryProduct",
            dataType: 'json',
            method: 'GET',
            data: '',
            headers: {
                "Content-Type": "application/json",
                "X-PeriodKey": period,
                "X-SetNames": setnames,
                "X-UnitSizes": unitsizes,
                "X-PriceFrom": pricefrom,
                "X-PriceTo": priceto,
                "X-ColorName": colorname,
                "X-CountryName": countryname
            }
        }).
            then(
            function (result) {
                var len = result.data.length;
                $scope.grid.data = result.data.slice(1, len);
                var product_ty_9Lcasespct = document.getElementById("Product_TY_9LCasesPct");
                if (product_ty_9Lcasespct)
                    product_ty_9Lcasespct.textContent = (result.data[0].TY_9LCasesPct * 100).toFixed(2).toString() + "%";

                var product_ty_unitspct = document.getElementById("Product_TY_UnitsPct");
                if (product_ty_unitspct)
                    product_ty_unitspct.textContent = (result.data[0].TY_UnitsPct * 100).toFixed(2).toString() + "%";

                var product_ty_totalsalesamountpct = document.getElementById("Product_TY_TotalSalesAmountPct");
                if (product_ty_totalsalesamountpct)
                    product_ty_totalsalesamountpct.textContent = (result.data[0].TY_TotalSalesAmountPct * 100).toFixed(2).toString() + "%";

                var product_ty_td_9Lcasespct = document.getElementById("Product_TY_TD_9LCasesPct");
                if (product_ty_td_9Lcasespct)
                    product_ty_td_9Lcasespct.textContent = (result.data[0].TY_9LCasesPct * 100).toFixed(2).toString() + "%";

                var product_ty_td_unitspct = document.getElementById("Product_TY_TD_UnitsPct");
                if (product_ty_td_unitspct)
                    product_ty_td_unitspct.textContent = (result.data[0].TY_UnitsPct * 100).toFixed(2).toString() + "%";

                var product_ty_td_totalsalesamountpct = document.getElementById("Product_TY_TD_TotalSalesAmountPct");
                if (product_ty_td_totalsalesamountpct)
                    product_ty_td_totalsalesamountpct.textContent = (result.data[0].TY_TotalSalesAmountPct * 100).toFixed(2).toString() + "%";

                var product_ty6m_9Lcasespct = document.getElementById("Product_TY6m_9LCasesPct");
                if (product_ty6m_9Lcasespct)
                    product_ty6m_9Lcasespct.textContent = (result.data[0].TY6m_9LCasesPct * 100).toFixed(2).toString() + "%";

                var product_ty6m_unitspct = document.getElementById("Product_TY6m_UnitsPct");
                if (product_ty6m_unitspct)
                    product_ty6m_unitspct.textContent = (result.data[0].TY6m_UnitsPct * 100).toFixed(2).toString() + "%";

                var product_ty6m_totalsalesamountpct = document.getElementById("Product_Y6m_TotalSalesAmountPct");
                if (product_ty6m_totalsalesamountpct)
                    product_ty6m_totalsalesamountpct.textContent = (result.data[0].TY6m_TotalSalesAmountPct * 100).toFixed(2).toString() + "%";

                var product_tyytd_9Lcasespct = document.getElementById("Product_TYYTD_9LCasesPct");
                if (product_tyytd_9Lcasespct)
                    product_tyytd_9Lcasespct.textContent = (result.data[0].TYYTD_9LCasesPct * 100).toFixed(2).toString() + "%";

                var product_tyytd_unitspct = document.getElementById("Product_TYYTD_UnitsPct");
                if (product_tyytd_unitspct)
                    product_tyytd_unitspct.textContent = (result.data[0].TYYTD_UnitsPct * 100).toFixed(2).toString() + "%";

                var product_tyytd_totalsalesamountpct = document.getElementById("Product_TYYTD_SalesAmountPct");
                if (product_tyytd_totalsalesamountpct)
                    product_tyytd_totalsalesamountpct.textContent = (result.data[0].TYYTD_SalesAmountPct * 100).toFixed(2).toString() + "%";

                var product_ty13_onprempct = document.getElementById("Product_TY13_OnPremPct");
                if (product_ty13_onprempct)
                    product_ty13_onprempct.textContent = (result.data[0].TY13_OnPremPct * 100).toFixed(2).toString() + "%";

                var product_ty13amountpct = document.getElementById("Product_TY13AmountPct");
                if (product_ty13amountpct)
                    product_ty13amountpct.textContent = (result.data[0].TY13AmountPct * 100).toFixed(2).toString() + "%";
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
       // console.log(document.getElementById("SalesSelectedGroupId").value);
        RefreshClick();
    }

    function RefreshClick() {
        var selectedgroupid = parseInt(document.getElementById("SalesSelectedGroupId").value);
        //update ColDefs
        if (selectedgroupid === 0) {

            $scope.grid.columnDefs[4].visible = true;
            $scope.grid.columnDefs[5].visible = true;
            $scope.grid.columnDefs[6].visible = true;
            $scope.grid.columnDefs[7].visible = false;
            $scope.grid.columnDefs[8].visible = false;
            $scope.grid.columnDefs[9].visible = false;
            $scope.grid.columnDefs[10].visible = false;
            $scope.grid.columnDefs[11].visible = false;
            $scope.grid.columnDefs[12].visible = false;

            //$scope.grid.columnDefs[17].visible = true;
            //$scope.grid.columnDefs[18].visible = true;
            //$scope.grid.columnDefs[19].visible = true;
            //$scope.grid.columnDefs[20].visible = false;
            //$scope.grid.columnDefs[21].visible = false;
            //$scope.grid.columnDefs[22].visible = false;
            //$scope.grid.columnDefs[23].visible = false;
            //$scope.grid.columnDefs[24].visible = false;
            //$scope.grid.columnDefs[25].visible = false;


            $scope.grid.columnDefs[26].visible = true;
            $scope.grid.columnDefs[27].visible = true;
            $scope.grid.columnDefs[28].visible = true;
            $scope.grid.columnDefs[29].visible = false;
            $scope.grid.columnDefs[30].visible = false;
            $scope.grid.columnDefs[31].visible = false;
            $scope.grid.columnDefs[32].visible = false;
            $scope.grid.columnDefs[33].visible = false;
            $scope.grid.columnDefs[34].visible = false;

            $scope.grid.columnDefs[35].visible = true;
            $scope.grid.columnDefs[36].visible = true;
            $scope.grid.columnDefs[37].visible = true;
            $scope.grid.columnDefs[38].visible = false;
            $scope.grid.columnDefs[39].visible = false;
            $scope.grid.columnDefs[40].visible = false;
            $scope.grid.columnDefs[41].visible = false;
            $scope.grid.columnDefs[42].visible = false;
            $scope.grid.columnDefs[43].visible = false;

        } else if (selectedgroupid === 2) {

            $scope.grid.columnDefs[4].visible = false;
            $scope.grid.columnDefs[5].visible = false;
            $scope.grid.columnDefs[6].visible = false;
            $scope.grid.columnDefs[7].visible = true;
            $scope.grid.columnDefs[8].visible = true;
            $scope.grid.columnDefs[9].visible = true;
            $scope.grid.columnDefs[10].visible = false;
            $scope.grid.columnDefs[11].visible = false;
            $scope.grid.columnDefs[12].visible = false;

            //$scope.grid.columnDefs[17].visible = false;
            //$scope.grid.columnDefs[18].visible = false;
            //$scope.grid.columnDefs[19].visible = false;
            //$scope.grid.columnDefs[20].visible = true;
            //$scope.grid.columnDefs[21].visible = true;
            //$scope.grid.columnDefs[22].visible = true;
            //$scope.grid.columnDefs[23].visible = false;
            //$scope.grid.columnDefs[24].visible = false;
            //$scope.grid.columnDefs[25].visible = false;


            $scope.grid.columnDefs[26].visible = false;
            $scope.grid.columnDefs[27].visible = false;
            $scope.grid.columnDefs[28].visible = false;
            $scope.grid.columnDefs[29].visible = true;
            $scope.grid.columnDefs[30].visible = true;
            $scope.grid.columnDefs[31].visible = true;
            $scope.grid.columnDefs[32].visible = false;
            $scope.grid.columnDefs[33].visible = false;
            $scope.grid.columnDefs[34].visible = false;

            $scope.grid.columnDefs[35].visible = false;
            $scope.grid.columnDefs[36].visible = false;
            $scope.grid.columnDefs[37].visible = false;
            $scope.grid.columnDefs[38].visible = true;
            $scope.grid.columnDefs[39].visible = true;
            $scope.grid.columnDefs[40].visible = true;
            $scope.grid.columnDefs[41].visible = false;
            $scope.grid.columnDefs[42].visible = false;
            $scope.grid.columnDefs[43].visible = false;

        } else if (selectedgroupid === 1) {

            $scope.grid.columnDefs[4].visible = false;
            $scope.grid.columnDefs[5].visible = false;
            $scope.grid.columnDefs[6].visible = false;
            $scope.grid.columnDefs[7].visible = false;
            $scope.grid.columnDefs[8].visible = false;
            $scope.grid.columnDefs[9].visible = false;
            $scope.grid.columnDefs[10].visible = true;
            $scope.grid.columnDefs[11].visible = true;
            $scope.grid.columnDefs[12].visible = true;

            //$scope.grid.columnDefs[17].visible = false;
            //$scope.grid.columnDefs[18].visible = false;
            //$scope.grid.columnDefs[19].visible = false;
            //$scope.grid.columnDefs[20].visible = false;
            //$scope.grid.columnDefs[21].visible = false;
            //$scope.grid.columnDefs[22].visible = false;
            //$scope.grid.columnDefs[23].visible = true;
            //$scope.grid.columnDefs[24].visible = true;
            //$scope.grid.columnDefs[25].visible = true;


            $scope.grid.columnDefs[26].visible = false;
            $scope.grid.columnDefs[27].visible = false;
            $scope.grid.columnDefs[28].visible = false;
            $scope.grid.columnDefs[29].visible = false;
            $scope.grid.columnDefs[30].visible = false;
            $scope.grid.columnDefs[31].visible = false;
            $scope.grid.columnDefs[32].visible = true;
            $scope.grid.columnDefs[33].visible = true;
            $scope.grid.columnDefs[34].visible = true;

            $scope.grid.columnDefs[35].visible = false;
            $scope.grid.columnDefs[36].visible = false;
            $scope.grid.columnDefs[37].visible = false;
            $scope.grid.columnDefs[38].visible = false;
            $scope.grid.columnDefs[39].visible = false;
            $scope.grid.columnDefs[40].visible = false;
            $scope.grid.columnDefs[41].visible = true;
            $scope.grid.columnDefs[42].visible = true;
            $scope.grid.columnDefs[43].visible = true;

        }
    }
}