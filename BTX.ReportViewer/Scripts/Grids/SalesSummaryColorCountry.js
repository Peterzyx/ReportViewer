//SalesByProducts
app.controller('SalesSummaryColorCountryCtrl', SalesSummaryColorCountryCtrl);

SalesSummaryColorCountryCtrl.$inject = ['$http', '$scope', 'GroupDropDown', 'uiGridConstants','GroupService'];
function SalesSummaryColorCountryCtrl($http, $scope, GroupDropDown, uiGridConstants, GroupService) {

    //UI Grid Setting
    $scope.grid = {
        columnDefs: [

            { name: 'Country', field: "Country", width: 200, cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;"><a ng-click="grid.appScope.salessubgrid(row.entity.Country)" style="cursor:pointer;">{{COL_FIELD}}</a></div>', footerCellTemplate: '<div class="ui-grid-cell-contents">Total:</div>' },

            { name: 'Nb Regular', field: "Nb_General", width: '5%', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'Nb Vintages Essential', field: "Nb_VintageEssential", width: '5%', visible: false, cellFilter: 'number:0', type: 'number' },
            { name: 'Nb Vintages', field: "Nb_Vintages", width: '5%', visible: false, cellFilter: 'number:0', type: 'number' },

            { name: 'LY_9LCases', field: "LY_9LCases", displayName: "LY", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_9LCases', field: "TY_9LCases", displayName: "TY", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_9LCasesPct', field: "TY_9LCasesPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Country_TY_9LCasesPct"></span></div>' },
            { name: 'LY_Units', field: "LY_Units", displayName: "LY", width: 120, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_Units', field: "TY_Units", displayName: "TY", width: 120, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_UnitsPct', field: "TY_UnitsPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Country_TY_UnitsPct"></span></div>' },
            { name: 'LY_TotalSalesAmount', field: "LY_TotalSalesAmount", displayName: "LY", width: 120, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TY_TotalSalesAmount', field: "TY_TotalSalesAmount", displayName: "TY", width: 120, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TY_TotalSalesAmountPct', field: "TY_TotalSalesAmountPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Country_TY_TotalSalesAmountPct"></span></div>' },

            { name: 'Week1', field: "Week1", displayName: "WK1", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'Week2', field: "Week2", displayName: "WK2", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'Week3', field: "Week3", displayName: "WK3", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'Week4', field: "Week4", displayName: "WK4", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },

            { name: 'LY_TD_9LCases', field: "LY_TD_9LCases", displayName: "TD LY", width: 90, cellFilter: 'number:0', type: 'number',visible:false, aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_TD_9LCases', field: "TY_TD_9LCases", displayName: "TD TY", width: 90, cellFilter: 'number:0', type: 'number', visible: false, aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_TD_9LCasesPct', field: "TY_TD_9LCasesPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', type: 'number', visible:false, footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Country_TY_TD_9LCasesPct"></span></div>' },
            { name: 'LY_TD_Units', field: "LY_TD_Units", displayName: "TD LY", width: 90, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_TD_Units', field: "TY_TD_Units", displayName: "TD TY", width: 90, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_TD_UnitsPct', field: "TY_TD_UnitsPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Country_TY_TD_UnitsPct"></span></div>' },
            { name: 'LY_TD_TotalSalesAmount', field: "LY_TD_TotalSalesAmount", displayName: "TD LY", width: 90, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TY_TD_TotalSalesAmount', field: "TY_TD_TotalSalesAmount", displayName: "TD TY", width: 90, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TY_TD_TotalSalesAmountPct', field: "TY_TD_TotalSalesAmountPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Country_TY_TD_TotalSalesAmountPct"></span></div>' },

            { name: 'LY6m_9LCases', field: "LY6m_9LCases", displayName: "6 Last Per LY", width: 110, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY6m_9LCases', field: "TY6m_9LCases", displayName: "6 Last Per TY", width: 110, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY6m_9LCasesPct', field: "TY6m_9LCasesPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Country_TY6m_9LCasesPct"></span></div>' },
            { name: 'LY6m_Units', field: "LY6m_Units", displayName: "6 Last Per LY", width: 110, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY6m_Units', field: "TY6m_Units", displayName: "6 Last Per TY", width: 110, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY6m_UnitsPct', field: "TY6m_UnitsPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Country_TY6m_UnitsPct"></span></div>' },
            { name: 'LY6m_TotalSalesAmount', field: "LY6m_TotalSalesAmount", displayName: "6 Last Per LY", width: 110, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TY6m_TotalSalesAmount', field: "TY6m_TotalSalesAmount", displayName: "6 Last Per TY", width: 110, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TY6m_TotalSalesAmountPct', field: "TY6m_TotalSalesAmountPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Country_TY6m_TotalSalesAmountPct"></span></div>' },

            { name: 'LYYTD_9LCases', field: "LYYTD_9LCases", displayName: "YTD LY", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TYYTD_9LCases', field: "TYYTD_9LCases", displayName: "YTD TY", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TYYTD_9LCasesPct', field: "TYYTD_9LCasesPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Country_TYYTD_9LCasesPct"></span></div>' },
            { name: 'LYYTD_Units', field: "LYYTD_Units", displayName: "YTD LY", width: 120, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TYYTD_Units', field: "TYYTD_Units", displayName: "YTD TY", width: 120, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TYYTD_UnitsPct', field: "TYYTD_UnitsPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Country_TYYTD_UnitsPct"></span></div>' },
            { name: 'LYYTD_SalesAmount', field: "LYYTD_SalesAmount", displayName: "YTD LY", width: 120, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TYYTD_SalesAmount', field: "TYYTD_SalesAmount", displayName: "YTD TY", width: 120, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TYYTD_SalesAmountPct', field: "TYYTD_SalesAmountPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Country_TYYTD_SalesAmountPct"></span></div>' },

            { name: 'LY13_OnPrem', field: "LY13_OnPrem", displayName: "13 Last Per LY", width: 115, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY13_OnPrem', field: "TY13_OnPrem", displayName: "13 Last per TY", width: 115, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY13_OnPremPct', field: "TY13_OnPremPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Country_TY13_OnPremPct"></span></div>' },

            { name: 'TY13Amount', field: "TY13Amount", displayName: "13 Last Per TY$", width: 120, cellFilter: 'currency:"$":0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TY13AmountPct', field: "TY13AmountPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Country_TY13AmountPct"></span></div>' }

        ],
        enableSorting: true,
        enableRowSelection: true,
        enableRowHeaderSelection: true,
        enableCellSelection: false,
        enableCellEditOnFocus: false,
        enableGridMenu: true,
        exporterMenuPdf: false,
        showColumnFooter: true,
        exporterCsvFilename: 'Sales Summary by Color Country Detail - ' + document.getElementById('ColorName').value + '.csv',
        exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location"))
    };

    $scope.grid.onRegisterApi = function (gridApi) {
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
        var setnames = document.getElementById('SalesSetNames').value;
        var unitsizes = document.getElementById('SalesUnitSizes').value;
        var pricefrom = parseFloat(document.getElementById('SalesPriceFrom').value);
        var priceto = parseFloat(document.getElementById('SalesPriceTo').value);
        var colorname = document.getElementById('ColorName').value;

        //Info used in setting the Page
        $scope.currentUser = document.getElementById('currentUserInfo').value;
        $scope.currentUserInfo = $scope.currentUser + "GroupSelected";

        $http({
            url: "/SalesSummaryColorCountry",
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
                "X-ColorName": colorname
            }
        }).
            then(
            function (result) {
                var len = result.data.length;
                $scope.grid.data = result.data.slice(1, len);
                var country_ty_9Lcasespct = document.getElementById("Country_TY_9LCasesPct");
                if (country_ty_9Lcasespct)
                    country_ty_9Lcasespct.textContent = (result.data[0].TY_9LCasesPct * 100).toFixed(2).toString() + "%";

                var country_ty_unitspct = document.getElementById("Country_TY_UnitsPct");
                if (country_ty_unitspct)
                    country_ty_unitspct.textContent = (result.data[0].TY_UnitsPct * 100).toFixed(2).toString() + "%";

                var country_ty_totalsalesamountpct = document.getElementById("Country_TY_TotalSalesAmountPct");
                if (country_ty_totalsalesamountpct)
                    country_ty_totalsalesamountpct.textContent = (result.data[0].TY_TotalSalesAmountPct * 100).toFixed(2).toString() + "%";

                var country_ty_td_9Lcasespct = document.getElementById("Country_TY_TD_9LCasesPct");
                if (country_ty_td_9Lcasespct)
                    country_ty_td_9Lcasespct.textContent = (result.data[0].TY_9LCasesPct * 100).toFixed(2).toString() + "%";

                var country_ty_td_unitspct = document.getElementById("Country_TY_TD_UnitsPct");
                if (coountry_ty_td_unitspct)
                    country_ty_td_unitspct.textContent = (result.data[0].TY_UnitsPct * 100).toFixed(2).toString() + "%";

                var country_ty_td_totalsalesamountpct = document.getElementById("Country_TY_TD_TotalSalesAmountPct");
                if (country_ty_td_totalsalesamountpct)
                    country_ty_td_totalsalesamountpct.textContent = (result.data[0].TY_TotalSalesAmountPct * 100).toFixed(2).toString() + "%";

                var country_ty6m_9Lcasespct = document.getElementById("Country_TY6m_9LCasesPct");
                if (country_ty6m_9Lcasespct)
                    country_ty6m_9Lcasespct.textContent = (result.data[0].TY6m_9LCasesPct * 100).toFixed(2).toString() + "%";

                var country_ty6m_unitspct = document.getElementById("Country_TY6m_UnitsPct");
                if (country_ty6m_unitspct)
                    country_ty6m_unitspct.textContent = (result.data[0].TY6m_UnitsPct * 100).toFixed(2).toString() + "%";

                var country_ty6m_totalsalesamountpct = document.getElementById("Country_TY6m_TotalSalesAmountPct");
                if (country_ty6m_totalsalesamountpct)
                    country_ty6m_totalsalesamountpct.textContent = (result.data[0].TY6m_TotalSalesAmountPct * 100).toFixed(2).toString() + "%";

                var country_tyytd_9Lcasespct = document.getElementById("Country_TYYTD_9LCasesPct");
                if (country_tyytd_9Lcasespct)
                    country_tyytd_9Lcasespct.textContent = (result.data[0].TYYTD_9LCasesPct * 100).toFixed(2).toString() + "%";

                var country_tyytd_unitspct = document.getElementById("Country_TYYTD_UnitsPct");
                if (country_tyytd_unitspct)
                    country_tyytd_unitspct.textContent = (result.data[0].TYYTD_UnitsPct * 100).toFixed(2).toString() + "%";

                var country_tyytd_totalsalesamountpct = document.getElementById("Country_TYYTD_SalesAmountPct");
                if (country_tyytd_totalsalesamountpct)
                    country_tyytd_totalsalesamountpct.textContent = (result.data[0].TYYTD_SalesAmountPct * 100).toFixed(2).toString() + "%";

                var country_ty13_onprempct = document.getElementById("Country_TY13_OnPremPct");
                if (country_ty13_onprempct)
                    country_ty13_onprempct.textContent = (result.data[0].TY13_OnPremPct * 100).toFixed(2).toString() + "%";

                var country_ty13amountpct = document.getElementById("Country_TY13AmountPct");
                if (country_ty13amountpct)
                    country_ty13amountpct.textContent = (result.data[0].TY13AmountPct * 100).toFixed(2).toString() + "%";
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

     //load subgrid
     $scope.salessubgrid = function (country) {
         var period = document.getElementById("SalesPeriod").value;
         var setnames = document.getElementById("SalesSetNames").value;
         var unitsizes = document.getElementById("SalesUnitSizes").value;
         var pricefrom = document.getElementById("SalesPriceFrom").value;
         var priceto = document.getElementById("SalesPriceTo").value;
         var colorname = document.getElementById("ColorName").value;
         
         //re-assign to global factory
         var selectedgroupid = parseInt(document.getElementById("SalesSelectedGroupId").value);
 
         //header option loading
         var headeroption = parseInt(document.getElementById('HeaderOption').value);
         //if (headeroption === 2) {
         //    var url = document.getElementById('SalesByBrandsUrl').value + "/" + period + "/" + agentid + "/" + agentname + "/" + setnames + "/" + unitsizes + "/" + pricefrom + "/" + priceto + "/" + selectedgroupid + "/" + headeroption;
         //} else {
         //    var url = document.getElementById('SalesByBrandsUrl').value + "/" + period + "/" + agentid + "/" + agentname + "/" + setnames + "/" + unitsizes + "/" + pricefrom + "/" + priceto + "/" + selectedgroupid;
         //};
 
         //create a dynamic form
         var f = document.createElement("form");
         f.setAttribute('id', "salesbycolorproductsubgridform");
         f.setAttribute('method', "post");
         f.setAttribute('action', document.getElementById('SalesSummaryColorCountryProductUrl').value);
         //target blank will post it to a new tab
         f.setAttribute("target", "_blank");
         //append the form to the bottom of the body
         document.body.appendChild(f);
         //create hidden elements and append them to the form
         f.appendChild(GroupDropDown.createFormElement("period", period));
         f.appendChild(GroupDropDown.createFormElement("colorname", colorname));
         f.appendChild(GroupDropDown.createFormElement("countryname", country));
         f.appendChild(GroupDropDown.createFormElement("setnames", setnames));
         f.appendChild(GroupDropDown.createFormElement("unitsizes", unitsizes));
         f.appendChild(GroupDropDown.createFormElement("pricefrom", pricefrom));
         f.appendChild(GroupDropDown.createFormElement("priceto", priceto));
         f.appendChild(GroupDropDown.createFormElement("selectedgroupid", selectedgroupid));
        // f.appendChild(GroupDropDown.createFormElement("salesrepid", salesrepid));
         if (headeroption === 2) {
             f.appendChild(GroupDropDown.createFormElement("headeroption", headeroption));
         }
         //submit form
         f.submit();
         //remove the newly created form after submit
         f.remove();
     }; 
}