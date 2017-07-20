//SalesSummaryBySegment
app.controller('salesSummaryBySegmentCtrl', SalesSummaryBySegmentCtrl);

SalesSummaryBySegmentCtrl.$inject = ['$http', '$scope', 'GroupDropDown', 'uiGridConstants','GroupService'];
function SalesSummaryBySegmentCtrl($http, $scope, GroupDropDown, uiGridConstants, GroupService) {

    //Accordion settings
    $scope.oneAtATime = true;
    $scope.status = {
        open: false
    };

    //UI Grid Color Setting
    $scope.gridColor = {
        columnDefs: [
            
            { name: 'Color', field: "Color", width: 200, cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;"><a ng-click="grid.appScope.salesbycolorsubgrid(row.entity.Color)" style="cursor:pointer;">{{COL_FIELD}}</a></div>', footerCellTemplate: '<div class="ui-grid-cell-contents">Total:</div>' },

            { name: 'Nb Regular', field: "Nb_General", width: '5%', cellFilter: 'number:0', type: 'number', visible: false, },
            { name: 'Nb Vintages Essential', field: "Nb_VintageEssential", width: '5%', cellFilter: 'number:0', type: 'number', visible: false, },
            { name: 'Nb Vintages', field: "Nb_Vintages", width: '5%', cellFilter: 'number:0', type: 'number', visible: false, },

            { name: 'LY_9LCases', field: "LY_9LCases", displayName: "LY", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_9LCases', field: "TY_9LCases", displayName: "TY", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_9LCasesPct', field: "TY_9LCasesPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Color_TY_9LCasesPct"></span></div>' },
            { name: 'LY_Units', field: "LY_Units", displayName:"LY", width: 120, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_Units', field: "TY_Units", displayName: "TY", width: 120, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_UnitsPct', field: "TY_UnitsPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Color_TY_UnitsPct"></span></div>' },
            { name: 'LY_TotalSalesAmount', field: "LY_TotalSalesAmount", displayName: "LY", width: 120, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TY_TotalSalesAmount', field: "TY_TotalSalesAmount", displayName: "TY", width: 120, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TY_TotalSalesAmountPct', field: "TY_TotalSalesAmountPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Color_TY_TotalSalesAmountPct"></span></div>' },

            { name: 'Week1', field: "Week1", displayName: "WK1", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'Week2', field: "Week2", displayName: "WK2", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'Week3', field: "Week3", displayName: "WK3", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'Week4', field: "Week4", displayName: "WK4", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },

          
            { name: 'LY_TD_9LCases', field: "LY_TD_9LCases", displayName: "LY", width: 90, cellFilter: 'number:0', type: 'number', visible: false, aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_TD_9LCases', field: "TY_TD_9LCases", displayName: "TY", width: 90, cellFilter: 'number:0', type: 'number', visible: false, aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_TD_9LCasesPct', field: "TY_TD_9LCasesPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', type: 'number', visible:false, footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Color_TY_TD_9LCasesPct"></span></div>' },
            { name: 'LY_TD_Units', field: "LY_TD_Units", displayName: "LY", width: 90, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_TD_Units', field: "TY_TD_Units", displayName: "TY", width: 90, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_TD_UnitsPct', field: "TY_TD_UnitsPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Color_TY_TD_UnitsPct"></span></div>' },
            { name: 'LY_TD_TotalSalesAmount', field: "LY_TD_TotalSalesAmount", displayName: "LY", width: 90, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TY_TD_TotalSalesAmount', field: "TY_TD_TotalSalesAmount", displayName: "TY", width: 90, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TY_TD_TotalSalesAmountPct', field: "TY_TD_TotalSalesAmountPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Color_TY_TD_TotalSalesAmountPct"></span></div>' },

            { name: 'LY6m_9LCases', field: "LY6m_9LCases", displayName: "6 Last Per LY", width: 110, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY6m_9LCases', field: "TY6m_9LCases", displayName: "6 Last Per TY", width: 110, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY6m_9LCasesPct', field: "TY6m_9LCasesPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Color_TY6m_9LCasesPct"></span></div>' },
            { name: 'LY6m_Units', field: "LY6m_Units", displayName: "6 Last Per LY", width: 110, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY6m_Units', field: "TY6m_Units", displayName: "6 Last Per TY", width: 110, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY6m_UnitsPct', field: "TY6m_UnitsPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Color_TY6m_UnitsPct"></span></div>' },
            { name: 'LY6m_TotalSalesAmount', field: "LY6m_TotalSalesAmount", displayName: "6 Last Per LY", width: 110, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TY6m_TotalSalesAmount', field: "TY6m_TotalSalesAmount", displayName: "6 Last Per TY", width: 110, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TY6m_TotalSalesAmountPct', field: "TY6m_TotalSalesAmountPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Color_TY6m_TotalSalesAmountPct"></span></div>' },

            { name: 'LYYTD_9LCases', field: "LYYTD_9LCases", displayName: "YTD LY", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TYYTD_9LCases', field: "TYYTD_9LCases", displayName: "YTD TY", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TYYTD_9LCasesPct', field: "TYYTD_9LCasesPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Color_TYYTD_9LCasesPct"></span></div>' },
            { name: 'LYYTD_Units', field: "LYYTD_Units", displayName: "YTD LY", width: 120, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TYYTD_Units', field: "TYYTD_Units", displayName: "YTD TY", width: 120, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TYYTD_UnitsPct', field: "TYYTD_UnitsPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Color_TYYTD_UnitsPct"></span></div>' },
            { name: 'LYYTD_SalesAmount', field: "LYYTD_SalesAmount", displayName: "YTD LY", width: 120, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TYYTD_SalesAmount', field: "TYYTD_SalesAmount", displayName: "YTD TY", width: 120, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TYYTD_SalesAmountPct', field: "TYYTD_SalesAmountPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Color_TYYTD_SalesAmountPct"></span></div>' },

            { name: 'LY13_OnPrem', field: "LY13_OnPrem", displayName: "13 Last Per LY", width: 115, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY13_OnPrem', field: "TY13_OnPrem", displayName: "13 Last per TY", width: 115, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY13_OnPremPct', field: "TY13_OnPremPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Color_TY13_OnPremPct"></span></div>' },

            { name: 'TY13Amount', field: "TY13Amount", displayName: "13 Last Per TY$", width: 120, cellFilter: 'currency:"$":0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TY13AmountPct', field: "TY13AmountPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Color_TY13AmountPct"></span></div>' }
        ],
        enableSorting: true,
        enableRowSelection: true,
        enableRowHeaderSelection: true,
        enableCellSelection: false,
        enableCellEditOnFocus: false,
        enableGridMenu: true,
        exporterMenuPdf: false,
        showColumnFooter: true,
        exporterCsvFilename: 'Sales Summary by Color - Ontario - Last Completed Period: ' + $scope.selectedperiodkey + '.csv',
        exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location"))
    };

    $scope.gridColor.onRegisterApi = function (gridApi) {
        $scope.gridColorApi = gridApi;
    };

    //UI Grid Country Setting
    $scope.gridCountry = {
        columnDefs: [
            { name: 'Country', field: "Country", width: 200, cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;"><a ng-click="grid.appScope.salesbycountrysubgrid(row.entity.Country)" style="cursor:pointer;">{{COL_FIELD}}</a></div>', footerCellTemplate: '<div class="ui-grid-cell-contents">Total:</div>' },

            { name: 'Nb Regular', field: "Nb_General", width: '5%', cellFilter: 'number:0', type: 'number', visible: false, },
            { name: 'Nb Vintages Essential', field: "Nb_VintageEssential", width: '5%', cellFilter: 'number:0', type: 'number', visible: false, },
            { name: 'Nb Vintages', field: "Nb_Vintages", width: '5%', cellFilter: 'number:0', type: 'number', visible: false, },

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


            { name: 'LY_TD_9LCases', field: "LY_TD_9LCases", displayName: "TD LY", width: 90, cellFilter: 'number:0', type: 'number', visible:false, aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_TD_9LCases', field: "TY_TD_9LCases", displayName: "TD TY", width: 90, cellFilter: 'number:0', type: 'number', visible:false, aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
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
            { name: 'TY13AmountPct', field: "TY13AmountPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Country_TY13AmountPct"></span></div>' },
        ],
        enableSorting: true,
        enableRowSelection: true,
        enableRowHeaderSelection: true,
        enableCellSelection: false,
        enableCellEditOnFocus: false,
        enableGridMenu: true,
        exporterMenuPdf: false,
        showColumnFooter: true,
        exporterCsvFilename: 'Sales Summary by Country - Ontario - Last Completed Period: ' + $scope.selectedperiodkey + '.csv',
        exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location"))
    };

    $scope.gridCountry.onRegisterApi = function (gridApi) {
        $scope.gridCountryApi = gridApi;
    };

    //UI Grid price band Setting
    $scope.gridPriceBand = {
        columnDefs: [
            { name: 'PriceBand', field: "PriceBand", displayName: "Price Band", width: 200, cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left; "><a ng-click="grid.appScope.salesbypricebandsubgrid(row.entity.PriceBand)" style="cursor:pointer;">{{COL_FIELD}}</a></div>', footerCellTemplate: '<div class="ui-grid-cell-contents">Total:</div>' },

            { name: 'Nb Regular', field: "Nb_General", width: '5%', cellFilter: 'number:0', type: 'number', visible: false, },
            { name: 'Nb Vintages Essential', field: "Nb_VintageEssential", width: '5%', cellFilter: 'number:0', type: 'number', visible: false, },
            { name: 'Nb Vintages', field: "Nb_Vintages", width: '5%', cellFilter: 'number:0', type: 'number', visible: false, },

            { name: 'LY_9LCases', field: "LY_9LCases", displayName: "LY", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_9LCases', field: "TY_9LCases", displayName: "TY", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_9LCasesPct', field: "TY_9LCasesPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="PriceBand_TY_9LCasesPct"></span></div>' },
            { name: 'LY_Units', field: "LY_Units", displayName: "LY", width: 120, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_Units', field: "TY_Units", displayName: "TY", width: 120, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_UnitsPct', field: "TY_UnitsPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="PriceBand_TY_UnitsPct"></span></div>' },
            { name: 'LY_TotalSalesAmount', field: "LY_TotalSalesAmount", displayName: "LY", width: 120, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TY_TotalSalesAmount', field: "TY_TotalSalesAmount", displayName: "TY", width: 120, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TY_TotalSalesAmountPct', field: "TY_TotalSalesAmountPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="PriceBand_TY_TotalSalesAmountPct"></span></div>' },

            { name: 'Week1', field: "Week1", displayName: "WK1", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'Week2', field: "Week2", displayName: "WK2", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'Week3', field: "Week3", displayName: "WK3", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'Week4', field: "Week4", displayName: "WK4", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },


            { name: 'LY_TD_9LCases', field: "LY_TD_9LCases", displayName: "TD LY", width: 90, cellFilter: 'number:0', type: 'number', visible:false, aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_TD_9LCases', field: "TY_TD_9LCases", displayName: "TD TY", width: 90, cellFilter: 'number:0', type: 'number', visible: false, aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_TD_9LCasesPct', field: "TY_TD_9LCasesPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', type: 'number', visible:false, footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="PriceBand_TY_TD_9LCasesPct"></span></div>' },
            { name: 'LY_TD_Units', field: "LY_TD_Units", displayName: "TD LY", width: 90, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_TD_Units', field: "TY_TD_Units", displayName: "TD TY", width: 90, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_TD_UnitsPct', field: "TY_TD_UnitsPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="PriceBand_TY_TD_UnitsPct"></span></div>' },
            { name: 'LY_TD_TotalSalesAmount', field: "LY_TD_TotalSalesAmount", displayName: "TD LY", width: 90, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TY_TD_TotalSalesAmount', field: "TY_TD_TotalSalesAmount", displayName: "TD TY", width: 90, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TY_TD_TotalSalesAmountPct', field: "TY_TD_TotalSalesAmountPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="PriceBand_TY_TD_TotalSalesAmountPct"></span></div>' },

            { name: 'LY6m_9LCases', field: "LY6m_9LCases", displayName: "6 Last Per LY", width: 110, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY6m_9LCases', field: "TY6m_9LCases", displayName: "6 Last Per TY", width: 110, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY6m_9LCasesPct', field: "TY6m_9LCasesPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="PriceBand_TY6m_9LCasesPct"></span></div>' },
            { name: 'LY6m_Units', field: "LY6m_Units", displayName: "6 Last Per LY", width: 110, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY6m_Units', field: "TY6m_Units", displayName: "6 Last Per TY", width: 110, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY6m_UnitsPct', field: "TY6m_UnitsPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="PriceBand_TY6m_UnitsPct"></span></div>' },
            { name: 'LY6m_TotalSalesAmount', field: "LY6m_TotalSalesAmount", displayName: "6 Last Per LY", width: 110, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TY6m_TotalSalesAmount', field: "TY6m_TotalSalesAmount", displayName: "6 Last Per TY", width: 110, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TY6m_TotalSalesAmountPct', field: "TY6m_TotalSalesAmountPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="PriceBand_TY6m_TotalSalesAmountPct"></span></div>' },

            { name: 'LYYTD_9LCases', field: "LYYTD_9LCases", displayName: "YTD LY", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TYYTD_9LCases', field: "TYYTD_9LCases", displayName: "YTD TY", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TYYTD_9LCasesPct', field: "TYYTD_9LCasesPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="PriceBand_TYYTD_9LCasesPct"></span></div>' },
            { name: 'LYYTD_Units', field: "LYYTD_Units", displayName: "YTD LY", width: 120, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TYYTD_Units', field: "TYYTD_Units", displayName: "YTD TY", width: 120, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TYYTD_UnitsPct', field: "TYYTD_UnitsPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="PriceBand_TYYTD_UnitsPct"></span></div>' },
            { name: 'LYYTD_SalesAmount', field: "LYYTD_SalesAmount", displayName: "YTD LY", width: 120, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TYYTD_SalesAmount', field: "TYYTD_SalesAmount", displayName: "YTD TY", width: 120, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TYYTD_SalesAmountPct', field: "TYYTD_SalesAmountPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="PriceBand_TYYTD_SalesAmountPct"></span></div>' },

            { name: 'LY13_OnPrem', field: "LY13_OnPrem", displayName: "13 Last Per LY", width: 115, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY13_OnPrem', field: "TY13_OnPrem", displayName: "13 Last per TY", width: 115, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY13_OnPremPct', field: "TY13_OnPremPct", displayName: "Var %", width: 115, cellFilter: 'mapPercentage', type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="PriceBand_TY13_OnPremPct"></span></div>' },

            { name: 'TY13Amount', field: "TY13Amount", displayName: "13 Last Per TY$", width: 120, cellFilter: 'currency:"$":0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TY13AmountPct', field: "TY13AmountPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="PriceBand_TY13AmountPct"></span></div>' },
        ],
        enableSorting: true,
        enableRowSelection: true,
        enableRowHeaderSelection: true,
        enableCellSelection: false,
        enableCellEditOnFocus: false,
        enableGridMenu: true,
        exporterMenuPdf: false,
        showColumnFooter: true,
        exporterCsvFilename: 'Sales Summary by Price Band - Ontario - Last Completed Period: ' + $scope.selectedperiodkey + '.csv',
        exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location"))
    };

    $scope.gridPriceBand.onRegisterApi = function (gridApi) {
        $scope.gridPriceBandApi = gridApi;
    };

    //UI Grid My Category Setting
    $scope.gridMyCategory = {
        columnDefs: [
            { name: 'MyCategory', field: "MyCategory", displayName: "Category", width: 200, cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;"><a ng-click="grid.appScope.salesbymycategorysubgrid(row.entity.MyCategory)" style="cursor:pointer;">{{COL_FIELD}}</a></div>', footerCellTemplate: '<div class="ui-grid-cell-contents">Total:</div>' },

            { name: 'Nb Regular', field: "Nb_General", width: '5%', cellFilter: 'number:0', type: 'number', visible: false, },
            { name: 'Nb Vintages Essential', field: "Nb_VintageEssential", width: '5%', cellFilter: 'number:0', type: 'number', visible: false, },
            { name: 'Nb Vintages', field: "Nb_Vintages", width: '5%', cellFilter: 'number:0', type: 'number', visible: false, },

            { name: 'LY_9LCases', field: "LY_9LCases", displayName: "LY", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_9LCases', field: "TY_9LCases", displayName: "TY", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_9LCasesPct', field: "TY_9LCasesPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="MyCategory_TY_9LCasesPct"></span></div>' },
            { name: 'LY_Units', field: "LY_Units", displayName: "LY", width: 120, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_Units', field: "TY_Units", displayName: "TY", width: 120, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_UnitsPct', field: "TY_UnitsPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="MyCategory_TY_UnitsPct"></span></div>' },
            { name: 'LY_TotalSalesAmount', field: "LY_TotalSalesAmount", displayName: "LY", width: 120, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TY_TotalSalesAmount', field: "TY_TotalSalesAmount", displayName: "TY", width: 120, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TY_TotalSalesAmountPct', field: "TY_TotalSalesAmountPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="MyCategory_TY_TotalSalesAmountPct"></span></div>' },

            { name: 'Week1', field: "Week1", displayName: "WK1", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'Week2', field: "Week2", displayName: "WK2", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'Week3', field: "Week3", displayName: "WK3", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'Week4', field: "Week4", displayName: "WK4", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },


            { name: 'LY_TD_9LCases', field: "LY_TD_9LCases", displayName: "TD LY", width: 90, cellFilter: 'number:0', type: 'number', visible:false, aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_TD_9LCases', field: "TY_TD_9LCases", displayName: "TD TY", width: 90, cellFilter: 'number:0', type: 'number', visible:false, aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_TD_9LCasesPct', field: "TY_TD_9LCasesPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', type: 'number', visible:false, footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="MyCategory_TY_TD_9LCasesPct"></span></div>' },
            { name: 'LY_TD_Units', field: "LY_TD_Units", displayName: "TD LY", width: 90, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_TD_Units', field: "TY_TD_Units", displayName: "TD TY", width: 90, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_TD_UnitsPct', field: "TY_TD_UnitsPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="MyCategory_TY_TD_UnitsPct"></span></div>' },
            { name: 'LY_TD_TotalSalesAmount', field: "LY_TD_TotalSalesAmount", displayName: "TD LY", width: 90, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TY_TD_TotalSalesAmount', field: "TY_TD_TotalSalesAmount", displayName: "TD TY", width: 90, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TY_TD_TotalSalesAmountPct', field: "TY_TD_TotalSalesAmountPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="MyCategory_TY_TD_TotalSalesAmountPct"></span></div>' },

            { name: 'LY6m_9LCases', field: "LY6m_9LCases", displayName: "6 Last Per LY", width: 110, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY6m_9LCases', field: "TY6m_9LCases", displayName: "6 Last Per TY", width: 110, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY6m_9LCasesPct', field: "TY6m_9LCasesPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="MyCategory_TY6m_9LCasesPct"></span></div>' },
            { name: 'LY6m_Units', field: "LY6m_Units", displayName: "6 Last Per LY", width: 110, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY6m_Units', field: "TY6m_Units", displayName: "6 Last Per TY", width: 110, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY6m_UnitsPct', field: "TY6m_UnitsPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="MyCategory_TY6m_UnitsPct"></span></div>' },
            { name: 'LY6m_TotalSalesAmount', field: "LY6m_TotalSalesAmount", displayName: "6 Last Per LY", width: 110, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TY6m_TotalSalesAmount', field: "TY6m_TotalSalesAmount", displayName: "6 Last Per TY", width: 110, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TY6m_TotalSalesAmountPct', field: "TY6m_TotalSalesAmountPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="MyCategory_TY6m_TotalSalesAmountPct"></span></div>' },

            { name: 'LYYTD_9LCases', field: "LYYTD_9LCases", displayName: "YTD LY", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TYYTD_9LCases', field: "TYYTD_9LCases", displayName: "YTD TY", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TYYTD_9LCasesPct', field: "TYYTD_9LCasesPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="MyCategory_TYYTD_9LCasesPct"></span></div>' },
            { name: 'LYYTD_Units', field: "LYYTD_Units", displayName: "YTD LY", width: 120, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TYYTD_Units', field: "TYYTD_Units", displayName: "YTD TY", width: 120, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TYYTD_UnitsPct', field: "TYYTD_UnitsPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="MyCategory_TYYTD_UnitsPct"></span></div>' },
            { name: 'LYYTD_SalesAmount', field: "LYYTD_SalesAmount", displayName: "YTD LY", width: 120, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TYYTD_SalesAmount', field: "TYYTD_SalesAmount", displayName: "YTD TY", width: 120, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TYYTD_SalesAmountPct', field: "TYYTD_SalesAmountPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="MyCategory_TYYTD_SalesAmountPct"></span></div>' },

            { name: 'LY13_OnPrem', field: "LY13_OnPrem", displayName: "13 Last Per LY", width: 115, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY13_OnPrem', field: "TY13_OnPrem", displayName: "13 Last per TY", width: 115, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY13_OnPremPct', field: "TY13_OnPremPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="MyCategory_TY13_OnPremPct"></span></div>' },

            { name: 'TY13Amount', field: "TY13Amount", displayName: "13 Last Per TY$", width: 120, cellFilter: 'currency:"$":0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TY13AmountPct', field: "TY13AmountPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="MyCategory_TY13AmountPct"></span></div>' },
        ],
        enableSorting: true,
        enableRowSelection: true,
        enableRowHeaderSelection: true,
        enableCellSelection: false,
        enableCellEditOnFocus: false,
        enableGridMenu: true,
        exporterMenuPdf: false,
        showColumnFooter: true,
        exporterCsvFilename: 'Sales Summary by My Category - Ontario - Last Completed Period: ' + $scope.selectedperiodkey + '.csv',
        exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location"))
    };

    $scope.gridMyCategory.onRegisterApi = function (gridApi) {
        $scope.gridMyCategoryApi = gridApi;
    };

    //UI Grid Varietal Setting
    $scope.gridVarietal = {
        columnDefs: [
            { name: 'Varietal', field: "Varietal", width: 200, cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;"><a ng-click="grid.appScope.salesbyvarietalsubgrid(row.entity.Varietal)" style="cursor:pointer;">{{COL_FIELD}}</a></div>', footerCellTemplate: '<div class="ui-grid-cell-contents">Total:</div>' },

            { name: 'Nb Regular', field: "Nb_General", width: '5%', cellFilter: 'number:0', type: 'number', visible: false, },
            { name: 'Nb Vintages Essential', field: "Nb_VintageEssential", width: '5%', cellFilter: 'number:0', type: 'number', visible: false, },
            { name: 'Nb Vintages', field: "Nb_Vintages", width: '5%', cellFilter: 'number:0', type: 'number', visible: false, },

            { name: 'LY_9LCases', field: "LY_9LCases", displayName: "LY", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_9LCases', field: "TY_9LCases", displayName: "TY", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_9LCasesPct', field: "TY_9LCasesPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Varietal_TY_9LCasesPct"></span></div>' },
            { name: 'LY_Units', field: "LY_Units", displayName: "LY", width: 120, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_Units', field: "TY_Units", displayName: "TY", width: 120, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_UnitsPct', field: "TY_UnitsPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Varietal_TY_UnitsPct"></span></div>' },
            { name: 'LY_TotalSalesAmount', field: "LY_TotalSalesAmount", displayName: "LY", width: 120, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TY_TotalSalesAmount', field: "TY_TotalSalesAmount", displayName: "TY", width: 120, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TY_TotalSalesAmountPct', field: "TY_TotalSalesAmountPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Varietal_TY_TotalSalesAmountPct"></span></div>' },

            { name: 'Week1', field: "Week1", displayName: "WK1", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'Week2', field: "Week2", displayName: "WK2", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'Week3', field: "Week3", displayName: "WK3", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'Week4', field: "Week4", displayName: "WK4", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },


            { name: 'LY_TD_9LCases', field: "LY_TD_9LCases", displayName: "TD LY", width: 90, cellFilter: 'number:0', type: 'number', visible:false, aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_TD_9LCases', field: "TY_TD_9LCases", displayName: "TD TY", width: 90, cellFilter: 'number:0', type: 'number', visible:false, aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_TD_9LCasesPct', field: "TY_TD_9LCasesPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', type: 'number', visible:false, footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Varietal_TY_TD_9LCasesPct"></span></div>' },
            { name: 'LY_TD_Units', field: "LY_TD_Units", displayName: "TD LY", width: 90, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_TD_Units', field: "TY_TD_Units", displayName: "TD TY", width: 90, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY_TD_UnitsPct', field: "TY_TD_UnitsPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Varietal_TY_TD_UnitsPct"></span></div>' },
            { name: 'LY_TD_TotalSalesAmount', field: "LY_TD_TotalSalesAmount", displayName: "TD LY", width: 90, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TY_TD_TotalSalesAmount', field: "TY_TD_TotalSalesAmount", displayName: "TD TY", width: 90, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TY_TD_TotalSalesAmountPct', field: "TY_TD_TotalSalesAmountPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Varietal_TY_TD_TotalSalesAmountPct"></span></div>' },

            { name: 'LY6m_9LCases', field: "LY6m_9LCases", displayName: "6 Last Per LY", width: 110, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY6m_9LCases', field: "TY6m_9LCases", displayName: "6 Last Per TY", width: 110, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY6m_9LCasesPct', field: "TY6m_9LCasesPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Varietal_TY6m_9LCasesPct"></span></div>' },
            { name: 'LY6m_Units', field: "LY6m_Units", displayName: "6 Last Per LY", width: 110, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY6m_Units', field: "TY6m_Units", displayName: "6 Last Per TY", width: 110, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY6m_UnitsPct', field: "TY6m_UnitsPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Varietal_TY6m_UnitsPct"></span></div>' },
            { name: 'LY6m_TotalSalesAmount', field: "LY6m_TotalSalesAmount", displayName: "6 Last Per LY", width: 110, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TY6m_TotalSalesAmount', field: "TY6m_TotalSalesAmount", displayName: "6 Last Per TY", width: 110, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TY6m_TotalSalesAmountPct', field: "TY6m_TotalSalesAmountPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Varietal_TY6m_TotalSalesAmountPct"></span></div>' },

            { name: 'LYYTD_9LCases', field: "LYYTD_9LCases", displayName: "YTD LY", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TYYTD_9LCases', field: "TYYTD_9LCases", displayName: "YTD TY", width: 120, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TYYTD_9LCasesPct', field: "TYYTD_9LCasesPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Varietal_TYYTD_9LCasesPct"></span></div>' },
            { name: 'LYYTD_Units', field: "LYYTD_Units", displayName: "YTD LY", width: 120, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TYYTD_Units', field: "TYYTD_Units", displayName: "YTD TY", width: 120, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TYYTD_UnitsPct', field: "TYYTD_UnitsPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Varietal_TYYTD_UnitsPct"></span></div>' },
            { name: 'LYYTD_SalesAmount', field: "LYYTD_SalesAmount", displayName: "YTD LY", width: 120, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TYYTD_SalesAmount', field: "TYYTD_SalesAmount", displayName: "YTD TY", width: 120, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TYYTD_SalesAmountPct', field: "TYYTD_SalesAmountPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Varietal_TYYTD_SalesAmountPct"></span></div>' },

            { name: 'LY13_OnPrem', field: "LY13_OnPrem", displayName: "13 Last Per LY", width: 115, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY13_OnPrem', field: "TY13_OnPrem", displayName: "13 Last per TY", width: 115, cellFilter: 'number:0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'TY13_OnPremPct', field: "TY13_OnPremPct", displayName: "Var %", width: 115, cellFilter: 'mapPercentage', type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Varietal_TY13_OnPremPct"></span></div>' },

            { name: 'TY13Amount', field: "TY13Amount", displayName: "13 Last Per TY$", width: 120, cellFilter: 'currency:"$":0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'TY13AmountPct', field: "TY13AmountPct", displayName: "Var %", width: 70, cellFilter: 'mapPercentage', type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Varietal_TY13AmountPct"></span></div>' },
        ],
        enableSorting: true,
        enableRowSelection: true,
        enableRowHeaderSelection: true,
        enableCellSelection: false,
        enableCellEditOnFocus: false,
        enableGridMenu: true,
        exporterMenuPdf: false,
        showColumnFooter: true,
        exporterCsvFilename: 'Sales Summary by Varietal - Ontario - Last Completed Period: ' + $scope.selectedperiodkey + '.csv',
        exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location"))
    };

    $scope.gridVarietal.onRegisterApi = function (gridApi) {
        $scope.gridVarietalApi = gridApi;
    }; 
    
    //Loading the data from gridFilters section
    $scope.pricefrom = {
        value: 0
    };
    $scope.priceto = {
        value: 100000
    };
    $scope.dropdownsettings = {
        displayProp: 'label',
        smartButtonMaxItems: 4,
        smartButtonTextConverter: function (itemText) { return itemText; },
        scrollableHeight: '400px',
        scrollable: true,
        searchField: 'label',
        enableSearch: true
    };

    init();

    //initial loading when rendering the page
    function init() {

        //cursor & page loading
        $scope.loading = true;
        var mybody = angular.element(document).find('body');
        mybody.addClass('waiting');

        //Info used in setting the Page
        $scope.currentUser = document.getElementById('currentUserInfo').value;
        $scope.currentUserInfo = $scope.currentUser + "GroupSelected";

        var periodkey = {};
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

                //set export file name
                $scope.gridColor.exporterCsvFilename = 'Market Reports Sales Summary by Color - Ontario - Last Completed Period: ' + periodkey + '.csv'
                $scope.gridCountry.exporterCsvFilename = 'Market Reports Sales Summary by Country - Ontario - Last Completed Period: ' + periodkey + '.csv'
                $scope.gridVarietal.exporterCsvFilename = 'Market Reports Sales Summary by Varietal - Ontario - Last Completed Period: ' + periodkey + '.csv'
                $scope.gridMyCategory.exporterCsvFilename = 'Market Reports Sales Summary by Category - Ontario - Last Completed Period: ' + periodkey + '.csv'
                $scope.gridPriceBand.exporterCsvFilename = 'Market Reports Sales Summary by Price Band - Ontario - Last Completed Period: ' + periodkey + '.csv'
             
                $http({
                    url: "/SetNames",
                    dataType: 'json',
                    method: 'POST',
                    data: '',
                    headers: {
                        "Content-Type": "application/json"
                    }
                }).
                    then(
                    function (resultsetnames) {
                        $scope.setnamesdata = resultsetnames.data;
                        $scope.setnamesmodel = JSON.parse(JSON.stringify($scope.setnamesdata));
                        var strSetNames = setnamesToString($scope.setnamesmodel);
                        $http({
                            url: "/UnitSizeMLs",
                            dataType: 'json',
                            method: 'POST',
                            data: '',
                            headers: {
                                "Content-Type": "application/json",
                                "X-PeriodKey": periodkey,
                                "X-SetNames": strSetNames
                            }
                        }).
                            then(
                            function (resultunitsizes) {
                                $scope.unitsizesdata = resultunitsizes.data;
                                $scope.unitsizesmodel = JSON.parse(JSON.stringify($scope.unitsizesdata));
                                var strUnitSizes = setnamesToString($scope.unitsizesmodel);

                                //Color
                                $http({
                                    url: "/SalesSummaryByColor",
                                    dataType: 'json',
                                    method: 'GET',
                                    data: '',
                                    headers: {
                                        "Content-Type": "application/json",
                                        "X-PeriodKey": periodkey,
                                        "X-SetNames": strSetNames,
                                        "X-UnitSizes": strUnitSizes,
                                        "X-PriceFrom": $scope.pricefrom.value,
                                        "X-PriceTo": $scope.priceto.value,
                                        "X-Group": $scope.selectedgroup.id
                                    }
                                }).
                                    then(
                                    function (result) {
                                        var len = result.data.length;
                                        $scope.gridColor.data = result.data.slice(1, len);
                                        
                                        var color_ty_9Lcasespct = document.getElementById("Color_TY_9LCasesPct");
                                        if (color_ty_9Lcasespct)
                                            color_ty_9Lcasespct.textContent = (result.data[0].TY_9LCasesPct * 100).toFixed(2).toString() + "%";

                                        var color_ty_unitspct = document.getElementById("Color_TY_UnitsPct");
                                        if (color_ty_unitspct)
                                            color_ty_unitspct.textContent = (result.data[0].TY_UnitsPct * 100).toFixed(2).toString() + "%";

                                        var color_ty_totalsalesamountpct = document.getElementById("Color_TY_TotalSalesAmountPct");
                                        if (color_ty_totalsalesamountpct)
                                            color_ty_totalsalesamountpct.textContent = (result.data[0].TY_TotalSalesAmountPct * 100).toFixed(2).toString() + "%";

                                        var color_ty_td_9Lcasespct = document.getElementById("Color_TY_TD_9LCasesPct");
                                        if (color_ty_td_9Lcasespct)
                                            color_ty_td_9Lcasespct.textContent = (result.data[0].TY_9LCasesPct * 100).toFixed(2).toString() + "%";

                                        var color_ty_td_unitspct = document.getElementById("Color_TY_TD_UnitsPct");
                                        if (color_ty_td_unitspct)
                                            color_ty_td_unitspct.textContent = (result.data[0].TY_UnitsPct * 100).toFixed(2).toString() + "%";

                                        var color_ty_td_totalsalesamountpct = document.getElementById("Color_TY_TD_TotalSalesAmountPct");
                                        if (color_ty_td_totalsalesamountpct)
                                            color_ty_td_totalsalesamountpct.textContent = (result.data[0].TY_TotalSalesAmountPct * 100).toFixed(2).toString() + "%";

                                        var color_ty6m_9Lcasespct = document.getElementById("Color_TY6m_9LCasesPct");
                                        if (color_ty6m_9Lcasespct)
                                            color_ty6m_9Lcasespct.textContent = (result.data[0].TY6m_9LCasesPct * 100).toFixed(2).toString() + "%";

                                        var color_ty6m_unitspct = document.getElementById("Color_TY6m_UnitsPct");
                                        if (color_ty6m_unitspct)
                                            color_ty6m_unitspct.textContent = (result.data[0].TY6m_UnitsPct * 100).toFixed(2).toString() + "%";

                                        var color_ty6m_totalsalesamountpct = document.getElementById("Color_TY6m_TotalSalesAmountPct");
                                        if (color_ty6m_totalsalesamountpct)
                                            color_ty6m_totalsalesamountpct.textContent = (result.data[0].TY6m_TotalSalesAmountPct * 100).toFixed(2).toString() + "%";

                                        var color_tyytd_9Lcasespct = document.getElementById("Color_TYYTD_9LCasesPct");
                                        if (color_tyytd_9Lcasespct)
                                            color_tyytd_9Lcasespct.textContent = (result.data[0].TYYTD_9LCasesPct * 100).toFixed(2).toString() + "%";

                                        var color_tyytd_unitspct = document.getElementById("Color_TYYTD_UnitsPct");
                                        if (color_tyytd_unitspct)
                                            color_tyytd_unitspct.textContent = (result.data[0].TYYTD_UnitsPct * 100).toFixed(2).toString() + "%";

                                        var color_tyytd_totalsalesamountpct = document.getElementById("Color_TYYTD_SalesAmountPct");
                                        if (color_tyytd_totalsalesamountpct)
                                            color_tyytd_totalsalesamountpct.textContent = (result.data[0].TYYTD_SalesAmountPct * 100).toFixed(2).toString() + "%";

                                        var color_ty13_onprempct = document.getElementById("Color_TY13_OnPremPct");
                                        if (color_ty13_onprempct)
                                            color_ty13_onprempct.textContent = (result.data[0].TY13_OnPremPct * 100).toFixed(2).toString() + "%";

                                        var color_ty13amountpct = document.getElementById("Color_TY13AmountPct");
                                        if (color_ty13amountpct)
                                            color_ty13amountpct.textContent = (result.data[0].TY13AmountPct * 100).toFixed(2).toString() + "%";
                                    },
                                    function (error) {
                                        InternalErrorHandler(error);
                                        $scope.loading = false;
                                        mybody.removeClass('waiting');
                                    }).finally(function () {
                                        $scope.loading = false;
                                        mybody.removeClass('waiting');
                                        $scope.tmpPeriod = periodkey;
                                        $scope.tmpSetNames = strSetNames;
                                        $scope.tmpUnitSizes = strUnitSizes;
                                        $scope.tmpPriceFrom = $scope.pricefrom.value;
                                        $scope.tmpPriceTo = $scope.priceto.value;
                                        $scope.tmpGroup = $scope.selectedgroup.id;
                                        
                                    });
                                

                                //Country
                                $http({
                                    url: "/SalesSummaryByCountry",
                                    dataType: 'json',
                                    method: 'GET',
                                    data: '',
                                    headers: {
                                        "Content-Type": "application/json",
                                        "X-PeriodKey": periodkey,
                                        "X-SetNames": strSetNames,
                                        "X-UnitSizes": strUnitSizes,
                                        "X-PriceFrom": $scope.pricefrom.value,
                                        "X-PriceTo": $scope.priceto.value,
                                        "X-Group": $scope.selectedgroup.id
                                    }
                                }). 
                                    then(
                                    function (result) {
                                        var len = result.data.length;
                                        $scope.gridCountry.data = result.data.slice(1, len);
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
                                        if (country_ty_td_unitspct)
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
                                        $scope.tmpPeriod = periodkey;
                                        $scope.tmpSetNames = strSetNames;
                                        $scope.tmpUnitSizes = strUnitSizes;
                                        $scope.tmpPriceFrom = $scope.pricefrom.value;
                                        $scope.tmpPriceTo = $scope.priceto.value;
                                        $scope.tmpGroup = $scope.selectedgroup.id;
                                    });

                                //My category
                                $http({
                                    url: "/SalesSummaryByMyCategory",
                                    dataType: 'json',
                                    method: 'GET',
                                    data: '',
                                    headers: {
                                        "Content-Type": "application/json",
                                        "X-PeriodKey": periodkey,
                                        "X-SetNames": strSetNames,
                                        "X-UnitSizes": strUnitSizes,
                                        "X-PriceFrom": $scope.pricefrom.value,
                                        "X-PriceTo": $scope.priceto.value,
                                        "X-Group": $scope.selectedgroup.id
                                    }
                                }).
                                    then(
                                    function (result) {
                                        var len = result.data.length;
                                        $scope.gridMyCategory.data = result.data.slice(1, len);
                                        var mycategory_ty_9Lcasespct = document.getElementById("MyCategory_TY_9LCasesPct");
                                        if (mycategory_ty_9Lcasespct)
                                            mycategory_ty_9Lcasespct.textContent = (result.data[0].TY_9LCasesPct * 100).toFixed(2).toString() + "%";

                                        var mycategory_ty_unitspct = document.getElementById("MyCategory_TY_UnitsPct");
                                        if (mycategory_ty_unitspct)
                                            mycategory_ty_unitspct.textContent = (result.data[0].TY_UnitsPct * 100).toFixed(2).toString() + "%";

                                        var mycategory_ty_totalsalesamountpct = document.getElementById("MyCategory_TY_TotalSalesAmountPct");
                                        if (mycategory_ty_totalsalesamountpct)
                                            mycategory_ty_totalsalesamountpct.textContent = (result.data[0].TY_TotalSalesAmountPct * 100).toFixed(2).toString() + "%";

                                        var mycategory_ty_td_9Lcasespct = document.getElementById("MyCategory_TY_TD_9LCasesPct");
                                        if (mycategory_ty_td_9Lcasespct)
                                            mycategory_ty_td_9Lcasespct.textContent = (result.data[0].TY_9LCasesPct * 100).toFixed(2).toString() + "%";

                                        var mycategory_ty_td_unitspct = document.getElementById("MyCategory_TY_TD_UnitsPct");
                                        if (mycategory_ty_td_unitspct)
                                            mycategory_ty_td_unitspct.textContent = (result.data[0].TY_UnitsPct * 100).toFixed(2).toString() + "%";

                                        var mycategory_ty_td_totalsalesamountpct = document.getElementById("MyCategory_TY_TD_TotalSalesAmountPct");
                                        if (mycategory_ty_td_totalsalesamountpct)
                                            mycategory_ty_td_totalsalesamountpct.textContent = (result.data[0].TY_TotalSalesAmountPct * 100).toFixed(2).toString() + "%";

                                        var mycategory_ty6m_9Lcasespct = document.getElementById("MyCategory_TY6m_9LCasesPct");
                                        if (mycategory_ty6m_9Lcasespct)
                                            mycategory_ty6m_9Lcasespct.textContent = (result.data[0].TY6m_9LCasesPct * 100).toFixed(2).toString() + "%";

                                        var mycategory_ty6m_unitspct = document.getElementById("MyCategory_TY6m_UnitsPct");
                                        if (mycategory_ty6m_unitspct)
                                            mycategory_ty6m_unitspct.textContent = (result.data[0].TY6m_UnitsPct * 100).toFixed(2).toString() + "%";

                                        var mycategory_ty6m_totalsalesamountpct = document.getElementById("MyCategory_TY6m_TotalSalesAmountPct");
                                        if (mycategory_ty6m_totalsalesamountpct)
                                            mycategory_ty6m_totalsalesamountpct.textContent = (result.data[0].TY6m_TotalSalesAmountPct * 100).toFixed(2).toString() + "%";

                                        var mycategory_tyytd_9Lcasespct = document.getElementById("MyCategory_TYYTD_9LCasesPct");
                                        if (mycategory_tyytd_9Lcasespct)
                                            mycategory_tyytd_9Lcasespct.textContent = (result.data[0].TYYTD_9LCasesPct * 100).toFixed(2).toString() + "%";

                                        var mycategory_tyytd_unitspct = document.getElementById("MyCategory_TYYTD_UnitsPct");
                                        if (mycategory_tyytd_unitspct)
                                            mycategory_tyytd_unitspct.textContent = (result.data[0].TYYTD_UnitsPct * 100).toFixed(2).toString() + "%";

                                        var mycategory_tyytd_totalsalesamountpct = document.getElementById("MyCategory_TYYTD_SalesAmountPct");
                                        if (mycategory_tyytd_totalsalesamountpct)
                                            mycategory_tyytd_totalsalesamountpct.textContent = (result.data[0].TYYTD_SalesAmountPct * 100).toFixed(2).toString() + "%";

                                        var mycategory_ty13_onprempct = document.getElementById("MyCategory_TY13_OnPremPct");
                                        if (mycategory_ty13_onprempct)
                                            mycategory_ty13_onprempct.textContent = (result.data[0].TY13_OnPremPct * 100).toFixed(2).toString() + "%";

                                        var mycategory_ty13amountpct = document.getElementById("MyCategory_TY13AmountPct");
                                        if (mycategory_ty13amountpct)
                                            mycategory_ty13amountpct.textContent = (result.data[0].TY13AmountPct * 100).toFixed(2).toString() + "%";
                                    },
                                    function (error) {
                                        InternalErrorHandler(error);
                                        $scope.loading = false;
                                        mybody.removeClass('waiting');
                                    }).finally(function () {
                                        $scope.loading = false;
                                        mybody.removeClass('waiting');
                                        $scope.tmpPeriod = periodkey;
                                        $scope.tmpSetNames = strSetNames;
                                        $scope.tmpUnitSizes = strUnitSizes;
                                        $scope.tmpPriceFrom = $scope.pricefrom.value;
                                        $scope.tmpPriceTo = $scope.priceto.value;
                                        $scope.tmpGroup = $scope.selectedgroup.id;
                                    });

                                //Price Band
                                $http({
                                    url: "/SalesSummaryByPriceBand",
                                    dataType: 'json',
                                    method: 'GET',
                                    data: '',
                                    headers: {
                                        "Content-Type": "application/json",
                                        "X-PeriodKey": periodkey,
                                        "X-SetNames": strSetNames,
                                        "X-UnitSizes": strUnitSizes,
                                        "X-PriceFrom": $scope.pricefrom.value,
                                        "X-PriceTo": $scope.priceto.value,
                                        "X-Group": $scope.selectedgroup.id
                                    }
                                }).
                                    then(
                                    function (result) {
                                        var len = result.data.length;
                                        $scope.gridPriceBand.data = result.data.slice(1, len);
                                        var priceband_ty_9Lcasespct = document.getElementById("PriceBand_TY_9LCasesPct");
                                        if (priceband_ty_9Lcasespct)
                                            priceband_ty_9Lcasespct.textContent = (result.data[0].TY_9LCasesPct * 100).toFixed(2).toString() + "%";

                                        var priceband_ty_unitspct = document.getElementById("PriceBand_TY_UnitsPct");
                                        if (priceband_ty_unitspct)
                                            priceband_ty_unitspct.textContent = (result.data[0].TY_UnitsPct * 100).toFixed(2).toString() + "%";

                                        var priceband_ty_totalsalesamountpct = document.getElementById("PriceBand_TY_TotalSalesAmountPct");
                                        if (priceband_ty_totalsalesamountpct)
                                            priceband_ty_totalsalesamountpct.textContent = (result.data[0].TY_TotalSalesAmountPct * 100).toFixed(2).toString() + "%";

                                        var priceband_ty_td_9Lcasespct = document.getElementById("PriceBand_TY_TD_9LCasesPct");
                                        if (priceband_ty_td_9Lcasespct)
                                            priceband_ty_td_9Lcasespct.textContent = (result.data[0].TY_9LCasesPct * 100).toFixed(2).toString() + "%";

                                        var priceband_ty_td_unitspct = document.getElementById("PriceBand_TY_TD_UnitsPct");
                                        if (priceband_ty_td_unitspct)
                                            priceband_ty_td_unitspct.textContent = (result.data[0].TY_UnitsPct * 100).toFixed(2).toString() + "%";

                                        var priceband_ty_td_totalsalesamountpct = document.getElementById("PriceBand_TY_TD_TotalSalesAmountPct");
                                        if (priceband_ty_td_totalsalesamountpct)
                                            priceband_ty_td_totalsalesamountpct.textContent = (result.data[0].TY_TotalSalesAmountPct * 100).toFixed(2).toString() + "%";

                                        var priceband_ty6m_9Lcasespct = document.getElementById("PriceBand_TY6m_9LCasesPct");
                                        if (priceband_ty6m_9Lcasespct)
                                            priceband_ty6m_9Lcasespct.textContent = (result.data[0].TY6m_9LCasesPct * 100).toFixed(2).toString() + "%";

                                        var priceband_ty6m_unitspct = document.getElementById("PriceBand_TY6m_UnitsPct");
                                        if (priceband_ty6m_unitspct)
                                            priceband_ty6m_unitspct.textContent = (result.data[0].TY6m_UnitsPct * 100).toFixed(2).toString() + "%";

                                        var priceband_ty6m_totalsalesamountpct = document.getElementById("PriceBand_TY6m_TotalSalesAmountPct");
                                        if (priceband_ty6m_totalsalesamountpct)
                                            priceband_ty6m_totalsalesamountpct.textContent = (result.data[0].TY6m_TotalSalesAmountPct * 100).toFixed(2).toString() + "%";

                                        var priceband_tyytd_9Lcasespct = document.getElementById("PriceBand_TYYTD_9LCasesPct");
                                        if (priceband_tyytd_9Lcasespct)
                                            priceband_tyytd_9Lcasespct.textContent = (result.data[0].TYYTD_9LCasesPct * 100).toFixed(2).toString() + "%";

                                        var priceband_tyytd_unitspct = document.getElementById("PriceBand_TYYTD_UnitsPct");
                                        if (priceband_tyytd_unitspct)
                                            priceband_tyytd_unitspct.textContent = (result.data[0].TYYTD_UnitsPct * 100).toFixed(2).toString() + "%";

                                        var priceband_tyytd_totalsalesamountpct = document.getElementById("PriceBand_TYYTD_SalesAmountPct");
                                        if (priceband_tyytd_totalsalesamountpct)
                                            priceband_tyytd_totalsalesamountpct.textContent = (result.data[0].TYYTD_SalesAmountPct * 100).toFixed(2).toString() + "%";

                                        var priceband_ty13_onprempct = document.getElementById("PriceBand_TY13_OnPremPct");
                                        if (priceband_ty13_onprempct)
                                            priceband_ty13_onprempct.textContent = (result.data[0].TY13_OnPremPct * 100).toFixed(2).toString() + "%";

                                        var priceband_ty13amountpct = document.getElementById("PriceBand_TY13AmountPct");
                                        if (priceband_ty13amountpct)
                                            priceband_ty13amountpct.textContent = (result.data[0].TY13AmountPct * 100).toFixed(2).toString() + "%";
                                    },
                                    function (error) {
                                        InternalErrorHandler(error);
                                        $scope.loading = false;
                                        mybody.removeClass('waiting');
                                    }).finally(function () {
                                        $scope.loading = false;
                                        mybody.removeClass('waiting');
                                        $scope.tmpPeriod = periodkey;
                                        $scope.tmpSetNames = strSetNames;
                                        $scope.tmpUnitSizes = strUnitSizes;
                                        $scope.tmpPriceFrom = $scope.pricefrom.value;
                                        $scope.tmpPriceTo = $scope.priceto.value;
                                        $scope.tmpGroup = $scope.selectedgroup.id;
                                    });
                                //Varietal
                                $http({
                                    url: "/SalesSummaryByVarietal",
                                    dataType: 'json',
                                    method: 'GET',
                                    data: '',
                                    headers: {
                                        "Content-Type": "application/json",
                                        "X-PeriodKey": periodkey,
                                        "X-SetNames": strSetNames,
                                        "X-UnitSizes": strUnitSizes,
                                        "X-PriceFrom": $scope.pricefrom.value,
                                        "X-PriceTo": $scope.priceto.value,
                                        "X-Group": $scope.selectedgroup.id
                                    }
                                }).
                                    then(
                                    function (result) {
                                        var len = result.data.length;
                                        $scope.gridVarietal.data = result.data.slice(1, len);
                                        var varietal_ty_9Lcasespct = document.getElementById("Varietal_TY_9LCasesPct");
                                        if (varietal_ty_9Lcasespct)
                                            varietal_ty_9Lcasespct.textContent = (result.data[0].TY_9LCasesPct * 100).toFixed(2).toString() + "%";

                                        var varietal_ty_unitspct = document.getElementById("varietal_TY_UnitsPct");
                                        if (varietal_ty_unitspct)
                                            varietal_ty_unitspct.textContent = (result.data[0].TY_UnitsPct * 100).toFixed(2).toString() + "%";

                                        var varietal_ty_totalsalesamountpct = document.getElementById("Varietal_TY_TotalSalesAmountPct");
                                        if (varietal_ty_totalsalesamountpct)
                                            varietal_ty_totalsalesamountpct.textContent = (result.data[0].TY_TotalSalesAmountPct * 100).toFixed(2).toString() + "%";

                                        var varietal_ty_td_9Lcasespct = document.getElementById("Varietal_TY_TD_9LCasesPct");
                                        if (varietal_ty_td_9Lcasespct)
                                            varietal_ty_td_9Lcasespct.textContent = (result.data[0].TY_9LCasesPct * 100).toFixed(2).toString() + "%";

                                        var varietal_ty_td_unitspct = document.getElementById("Varietal_TY_TD_UnitsPct");
                                        if (varietal_ty_td_unitspct)
                                            varietal_ty_td_unitspct.textContent = (result.data[0].TY_UnitsPct * 100).toFixed(2).toString() + "%";

                                        var varietal_ty_td_totalsalesamountpct = document.getElementById("Varietal_TY_TD_TotalSalesAmountPct");
                                        if (varietal_ty_td_totalsalesamountpct)
                                            varietal_ty_td_totalsalesamountpct.textContent = (result.data[0].TY_TotalSalesAmountPct * 100).toFixed(2).toString() + "%";

                                        var varietal_ty6m_9Lcasespct = document.getElementById("Varietal_TY6m_9LCasesPct");
                                        if (varietal_ty6m_9Lcasespct)
                                            varietal_ty6m_9Lcasespct.textContent = (result.data[0].TY6m_9LCasesPct * 100).toFixed(2).toString() + "%";

                                        var varietal_ty6m_unitspct = document.getElementById("Varietal_TY6m_UnitsPct");
                                        if (varietal_ty6m_unitspct)
                                            varietal_ty6m_unitspct.textContent = (result.data[0].TY6m_UnitsPct * 100).toFixed(2).toString() + "%";

                                        var varietal_ty6m_totalsalesamountpct = document.getElementById("Varietal_TY6m_TotalSalesAmountPct");
                                        if (varietal_ty6m_totalsalesamountpct)
                                            varietal_ty6m_totalsalesamountpct.textContent = (result.data[0].TY6m_TotalSalesAmountPct * 100).toFixed(2).toString() + "%";

                                        var varietal_tyytd_9Lcasespct = document.getElementById("Varietal_TYYTD_9LCasesPct");
                                        if (varietal_tyytd_9Lcasespct)
                                            varietal_tyytd_9Lcasespct.textContent = (result.data[0].TYYTD_9LCasesPct * 100).toFixed(2).toString() + "%";

                                        var varietal_tyytd_unitspct = document.getElementById("Varietal_TYYTD_UnitsPct");
                                        if (varietal_tyytd_unitspct)
                                            varietal_tyytd_unitspct.textContent = (result.data[0].TYYTD_UnitsPct * 100).toFixed(2).toString() + "%";

                                        var varietal_tyytd_totalsalesamountpct = document.getElementById("Varietal_TYYTD_SalesAmountPct");
                                        if (varietal_tyytd_totalsalesamountpct)
                                            varietal_tyytd_totalsalesamountpct.textContent = (result.data[0].TYYTD_SalesAmountPct * 100).toFixed(2).toString() + "%";

                                        var varietal_ty13_onprempct = document.getElementById("Varietal_TY13_OnPremPct");
                                        if (varietal_ty13_onprempct)
                                            varietal_ty13_onprempct.textContent = (result.data[0].TY13_OnPremPct * 100).toFixed(2).toString() + "%";

                                        var varietal_ty13amountpct = document.getElementById("Varietal_TY13AmountPct");
                                        if (varietal_ty13amountpct)
                                            varietal_ty13amountpct.textContent = (result.data[0].TY13AmountPct * 100).toFixed(2).toString() + "%";
                                    },
                                    
                                    function (error) {
                                        InternalErrorHandler(error);
                                        $scope.loading = false;
                                        mybody.removeClass('waiting');
                                    }).finally(function () {
                                        $scope.loading = false;
                                        mybody.removeClass('waiting');
                                        $scope.tmpPeriod = periodkey;
                                        $scope.tmpSetNames = strSetNames;
                                        $scope.tmpUnitSizes = strUnitSizes;
                                        $scope.tmpPriceFrom = $scope.pricefrom.value;
                                        $scope.tmpPriceTo = $scope.priceto.value;
                                        $scope.tmpGroup = $scope.selectedgroup.id;
                                    }); 
                            },
                            function (errorunitsizes) {
                                InternalErrorHandler(errorunitsizes);
                            });
                    },
                    function (errorsetnames) {
                        InternalErrorHandler(errorsetnames);
                })
        
            },
            function (perioderror) {
                InternalErrorHandler(perioderror);
            });

        //$scope.gridColor.columnDefs[4].displayName = " LY";
        //$scope.gridColor.columnDefs[5].displayName = " TY";
    }

    $scope.Reload = function () {
        RefreshClick();
    }
    //Refresh button click functions
    function RefreshClick() {
        var strPeriod = $scope.selectedperiodkey.label;
        var strSetNames = setnamesToString($scope.setnamesmodel);
        var strUnitSizes = setnamesToString($scope.unitsizesmodel);
        var fromValue = $scope.pricefrom.value;
        var toValue = $scope.priceto.value;


        //update ColDefs
        if ($scope.selectedgroup.id === 0) {
            //Color
            $scope.gridColor.columnDefs[4].visible = true;
            $scope.gridColor.columnDefs[5].visible = true;
            $scope.gridColor.columnDefs[6].visible = true;
            $scope.gridColor.columnDefs[7].visible = false;
            $scope.gridColor.columnDefs[8].visible = false;
            $scope.gridColor.columnDefs[9].visible = false;
            $scope.gridColor.columnDefs[10].visible = false;
            $scope.gridColor.columnDefs[11].visible = false;
            $scope.gridColor.columnDefs[12].visible = false;

            $scope.gridColor.columnDefs[17].visible = false;
            $scope.gridColor.columnDefs[18].visible = false;
            $scope.gridColor.columnDefs[19].visible = false;
            $scope.gridColor.columnDefs[20].visible = false;
            $scope.gridColor.columnDefs[21].visible = false;
            $scope.gridColor.columnDefs[22].visible = false;
            $scope.gridColor.columnDefs[23].visible = false;
            $scope.gridColor.columnDefs[24].visible = false;
            $scope.gridColor.columnDefs[25].visible = false;

            $scope.gridColor.columnDefs[26].visible = true;
            $scope.gridColor.columnDefs[27].visible = true;
            $scope.gridColor.columnDefs[28].visible = true;
            $scope.gridColor.columnDefs[29].visible = false;
            $scope.gridColor.columnDefs[30].visible = false;
            $scope.gridColor.columnDefs[31].visible = false;
            $scope.gridColor.columnDefs[32].visible = false;
            $scope.gridColor.columnDefs[33].visible = false;
            $scope.gridColor.columnDefs[34].visible = false;

            $scope.gridColor.columnDefs[35].visible = true;
            $scope.gridColor.columnDefs[36].visible = true;
            $scope.gridColor.columnDefs[37].visible = true;
            $scope.gridColor.columnDefs[38].visible = false;
            $scope.gridColor.columnDefs[39].visible = false;
            $scope.gridColor.columnDefs[40].visible = false;
            $scope.gridColor.columnDefs[41].visible = false;
            $scope.gridColor.columnDefs[42].visible = false;
            $scope.gridColor.columnDefs[43].visible = false;

            $scope.gridColorApi.grid.refresh();

            //Country
            $scope.gridCountry.columnDefs[4].visible = true;
            $scope.gridCountry.columnDefs[5].visible = true;
            $scope.gridCountry.columnDefs[6].visible = true;
            $scope.gridCountry.columnDefs[7].visible = false;
            $scope.gridCountry.columnDefs[8].visible = false;
            $scope.gridCountry.columnDefs[9].visible = false;
            $scope.gridCountry.columnDefs[10].visible = false;
            $scope.gridCountry.columnDefs[11].visible = false;
            $scope.gridCountry.columnDefs[12].visible = false;

            //$scope.gridCountry.columnDefs[17].visible = true;
            //$scope.gridCountry.columnDefs[18].visible = true;
            //$scope.gridCountry.columnDefs[19].visible = true;
            //$scope.gridCountry.columnDefs[20].visible = false;
            //$scope.gridCountry.columnDefs[21].visible = false;
            //$scope.gridCountry.columnDefs[22].visible = false;
            //$scope.gridCountry.columnDefs[23].visible = false;
            //$scope.gridCountry.columnDefs[24].visible = false;
            //$scope.gridCountry.columnDefs[25].visible = false;

            $scope.gridCountry.columnDefs[26].visible = true;
            $scope.gridCountry.columnDefs[27].visible = true;
            $scope.gridCountry.columnDefs[28].visible = true;
            $scope.gridCountry.columnDefs[29].visible = false;
            $scope.gridCountry.columnDefs[30].visible = false;
            $scope.gridCountry.columnDefs[31].visible = false;
            $scope.gridCountry.columnDefs[32].visible = false;
            $scope.gridCountry.columnDefs[33].visible = false;
            $scope.gridCountry.columnDefs[34].visible = false;

            $scope.gridCountry.columnDefs[35].visible = true;
            $scope.gridCountry.columnDefs[36].visible = true;
            $scope.gridCountry.columnDefs[37].visible = true;
            $scope.gridCountry.columnDefs[38].visible = false;
            $scope.gridCountry.columnDefs[39].visible = false;
            $scope.gridCountry.columnDefs[40].visible = false;
            $scope.gridCountry.columnDefs[41].visible = false;
            $scope.gridCountry.columnDefs[42].visible = false;
            $scope.gridCountry.columnDefs[43].visible = false;

            $scope.gridCountryApi.grid.refresh();

            //My Category
            $scope.gridMyCategory.columnDefs[4].visible = true;
            $scope.gridMyCategory.columnDefs[5].visible = true;
            $scope.gridMyCategory.columnDefs[6].visible = true;
            $scope.gridMyCategory.columnDefs[7].visible = false;
            $scope.gridMyCategory.columnDefs[8].visible = false;
            $scope.gridMyCategory.columnDefs[9].visible = false;
            $scope.gridMyCategory.columnDefs[10].visible = false;
            $scope.gridMyCategory.columnDefs[11].visible = false;
            $scope.gridMyCategory.columnDefs[12].visible = false;

            //$scope.gridMyCategory.columnDefs[17].visible = true;
            //$scope.gridMyCategory.columnDefs[18].visible = true;
            //$scope.gridMyCategory.columnDefs[19].visible = true;
            //$scope.gridMyCategory.columnDefs[20].visible = false;
            //$scope.gridMyCategory.columnDefs[21].visible = false;
            //$scope.gridMyCategory.columnDefs[22].visible = false;
            //$scope.gridMyCategory.columnDefs[23].visible = false;
            //$scope.gridMyCategory.columnDefs[24].visible = false;
            //$scope.gridMyCategory.columnDefs[25].visible = false;

            $scope.gridMyCategory.columnDefs[26].visible = true;
            $scope.gridMyCategory.columnDefs[27].visible = true;
            $scope.gridMyCategory.columnDefs[28].visible = true;
            $scope.gridMyCategory.columnDefs[29].visible = false;
            $scope.gridMyCategory.columnDefs[30].visible = false;
            $scope.gridMyCategory.columnDefs[31].visible = false;
            $scope.gridMyCategory.columnDefs[32].visible = false;
            $scope.gridMyCategory.columnDefs[33].visible = false;
            $scope.gridMyCategory.columnDefs[34].visible = false;

            $scope.gridMyCategory.columnDefs[35].visible = true;
            $scope.gridMyCategory.columnDefs[36].visible = true;
            $scope.gridMyCategory.columnDefs[37].visible = true;
            $scope.gridMyCategory.columnDefs[38].visible = false;
            $scope.gridMyCategory.columnDefs[39].visible = false;
            $scope.gridMyCategory.columnDefs[40].visible = false;
            $scope.gridMyCategory.columnDefs[41].visible = false;
            $scope.gridMyCategory.columnDefs[42].visible = false;
            $scope.gridMyCategory.columnDefs[43].visible = false;

            $scope.gridMyCategoryApi.grid.refresh();

            //Varietal
            $scope.gridVarietal.columnDefs[4].visible = true;
            $scope.gridVarietal.columnDefs[5].visible = true;
            $scope.gridVarietal.columnDefs[6].visible = true;
            $scope.gridVarietal.columnDefs[7].visible = false;
            $scope.gridVarietal.columnDefs[8].visible = false;
            $scope.gridVarietal.columnDefs[9].visible = false;
            $scope.gridVarietal.columnDefs[10].visible = false;
            $scope.gridVarietal.columnDefs[11].visible = false;
            $scope.gridVarietal.columnDefs[12].visible = false;

            //$scope.gridVarietal.columnDefs[17].visible = true;
            //$scope.gridVarietal.columnDefs[18].visible = true;
            //$scope.gridVarietal.columnDefs[19].visible = true;
            //$scope.gridVarietal.columnDefs[20].visible = false;
            //$scope.gridVarietal.columnDefs[21].visible = false;
            //$scope.gridVarietal.columnDefs[22].visible = false;
            //$scope.gridVarietal.columnDefs[23].visible = false;
            //$scope.gridVarietal.columnDefs[24].visible = false;
            //$scope.gridVarietal.columnDefs[25].visible = false;

            $scope.gridVarietal.columnDefs[26].visible = true;
            $scope.gridVarietal.columnDefs[27].visible = true;
            $scope.gridVarietal.columnDefs[28].visible = true;
            $scope.gridVarietal.columnDefs[29].visible = false;
            $scope.gridVarietal.columnDefs[30].visible = false;
            $scope.gridVarietal.columnDefs[31].visible = false;
            $scope.gridVarietal.columnDefs[32].visible = false;
            $scope.gridVarietal.columnDefs[33].visible = false;
            $scope.gridVarietal.columnDefs[34].visible = false;

            $scope.gridVarietal.columnDefs[35].visible = true;
            $scope.gridVarietal.columnDefs[36].visible = true;
            $scope.gridVarietal.columnDefs[37].visible = true;
            $scope.gridVarietal.columnDefs[38].visible = false;
            $scope.gridVarietal.columnDefs[39].visible = false;
            $scope.gridVarietal.columnDefs[40].visible = false;
            $scope.gridVarietal.columnDefs[41].visible = false;
            $scope.gridVarietal.columnDefs[42].visible = false;
            $scope.gridVarietal.columnDefs[43].visible = false;

            $scope.gridVarietalApi.grid.refresh(); 

            //Price Band
            $scope.gridPriceBand.columnDefs[4].visible = true;
            $scope.gridPriceBand.columnDefs[5].visible = true;
            $scope.gridPriceBand.columnDefs[6].visible = true;
            $scope.gridPriceBand.columnDefs[7].visible = false;
            $scope.gridPriceBand.columnDefs[8].visible = false;
            $scope.gridPriceBand.columnDefs[9].visible = false;
            $scope.gridPriceBand.columnDefs[10].visible = false;
            $scope.gridPriceBand.columnDefs[11].visible = false;
            $scope.gridPriceBand.columnDefs[12].visible = false;

            //$scope.gridPriceBand.columnDefs[17].visible = true;
            //$scope.gridPriceBand.columnDefs[18].visible = true;
            //$scope.gridPriceBand.columnDefs[19].visible = true;
            //$scope.gridPriceBand.columnDefs[20].visible = false;
            //$scope.gridPriceBand.columnDefs[21].visible = false;
            //$scope.gridPriceBand.columnDefs[22].visible = false;
            //$scope.gridPriceBand.columnDefs[23].visible = false;
            //$scope.gridPriceBand.columnDefs[24].visible = false;
            //$scope.gridPriceBand.columnDefs[25].visible = false;

            $scope.gridPriceBand.columnDefs[26].visible = true;
            $scope.gridPriceBand.columnDefs[27].visible = true;
            $scope.gridPriceBand.columnDefs[28].visible = true;
            $scope.gridPriceBand.columnDefs[29].visible = false;
            $scope.gridPriceBand.columnDefs[30].visible = false;
            $scope.gridPriceBand.columnDefs[31].visible = false;
            $scope.gridPriceBand.columnDefs[32].visible = false;
            $scope.gridPriceBand.columnDefs[33].visible = false;
            $scope.gridPriceBand.columnDefs[34].visible = false;

            $scope.gridPriceBand.columnDefs[35].visible = true;
            $scope.gridPriceBand.columnDefs[36].visible = true;
            $scope.gridPriceBand.columnDefs[37].visible = true;
            $scope.gridPriceBand.columnDefs[38].visible = false;
            $scope.gridPriceBand.columnDefs[39].visible = false;
            $scope.gridPriceBand.columnDefs[40].visible = false;
            $scope.gridPriceBand.columnDefs[41].visible = false;
            $scope.gridPriceBand.columnDefs[42].visible = false;
            $scope.gridPriceBand.columnDefs[43].visible = false;

            $scope.gridPriceBandApi.grid.refresh(); 
        } else if ($scope.selectedgroup.id === 1) {
            //color
            $scope.gridColor.columnDefs[4].visible = false;
            $scope.gridColor.columnDefs[5].visible = false;
            $scope.gridColor.columnDefs[6].visible = false;
            $scope.gridColor.columnDefs[7].visible = false;
            $scope.gridColor.columnDefs[8].visible = false;
            $scope.gridColor.columnDefs[9].visible = false;
            $scope.gridColor.columnDefs[10].visible = true;
            $scope.gridColor.columnDefs[11].visible = true;
            $scope.gridColor.columnDefs[12].visible = true;

            //$scope.gridColor.columnDefs[17].visible = false;
            //$scope.gridColor.columnDefs[18].visible = false;
            //$scope.gridColor.columnDefs[19].visible = false;
            //$scope.gridColor.columnDefs[20].visible = false;
            //$scope.gridColor.columnDefs[21].visible = false;
            //$scope.gridColor.columnDefs[22].visible = false;
            //$scope.gridColor.columnDefs[23].visible = true;
            //$scope.gridColor.columnDefs[24].visible = true;
            //$scope.gridColor.columnDefs[25].visible = true;

            $scope.gridColor.columnDefs[26].visible = false;
            $scope.gridColor.columnDefs[27].visible = false;
            $scope.gridColor.columnDefs[28].visible = false;
            $scope.gridColor.columnDefs[29].visible = false;
            $scope.gridColor.columnDefs[30].visible = false;
            $scope.gridColor.columnDefs[31].visible = false;
            $scope.gridColor.columnDefs[32].visible = true;
            $scope.gridColor.columnDefs[33].visible = true;
            $scope.gridColor.columnDefs[34].visible = true;

            $scope.gridColor.columnDefs[35].visible = false;
            $scope.gridColor.columnDefs[36].visible = false;
            $scope.gridColor.columnDefs[37].visible = false;
            $scope.gridColor.columnDefs[38].visible = false;
            $scope.gridColor.columnDefs[39].visible = false;
            $scope.gridColor.columnDefs[40].visible = false;
            $scope.gridColor.columnDefs[41].visible = true;
            $scope.gridColor.columnDefs[42].visible = true;
            $scope.gridColor.columnDefs[43].visible = true;
            $scope.gridColorApi.grid.refresh();

            //Country
            $scope.gridCountry.columnDefs[4].visible = false;
            $scope.gridCountry.columnDefs[5].visible = false;
            $scope.gridCountry.columnDefs[6].visible = false;
            $scope.gridCountry.columnDefs[7].visible = false;
            $scope.gridCountry.columnDefs[8].visible = false;
            $scope.gridCountry.columnDefs[9].visible = false;
            $scope.gridCountry.columnDefs[10].visible = true;
            $scope.gridCountry.columnDefs[11].visible = true;
            $scope.gridCountry.columnDefs[12].visible = true;

            //$scope.gridCountry.columnDefs[17].visible = false;
            //$scope.gridCountry.columnDefs[18].visible = false;
            //$scope.gridCountry.columnDefs[19].visible = false;
            //$scope.gridCountry.columnDefs[20].visible = false;
            //$scope.gridCountry.columnDefs[21].visible = false;
            //$scope.gridCountry.columnDefs[22].visible = false;
            //$scope.gridCountry.columnDefs[23].visible = true;
            //$scope.gridCountry.columnDefs[24].visible = true;
            //$scope.gridCountry.columnDefs[25].visible = true;

            $scope.gridCountry.columnDefs[26].visible = false;
            $scope.gridCountry.columnDefs[27].visible = false;
            $scope.gridCountry.columnDefs[28].visible = false;
            $scope.gridCountry.columnDefs[29].visible = false;
            $scope.gridCountry.columnDefs[30].visible = false;
            $scope.gridCountry.columnDefs[31].visible = false;
            $scope.gridCountry.columnDefs[32].visible = true;
            $scope.gridCountry.columnDefs[33].visible = true;
            $scope.gridCountry.columnDefs[34].visible = true;

            $scope.gridCountry.columnDefs[35].visible = false;
            $scope.gridCountry.columnDefs[36].visible = false;
            $scope.gridCountry.columnDefs[37].visible = false;
            $scope.gridCountry.columnDefs[38].visible = false;
            $scope.gridCountry.columnDefs[39].visible = false;
            $scope.gridCountry.columnDefs[40].visible = false;
            $scope.gridCountry.columnDefs[41].visible = true;
            $scope.gridCountry.columnDefs[42].visible = true;
            $scope.gridCountry.columnDefs[43].visible = true;
            $scope.gridCountryApi.grid.refresh();

            //My Category
            $scope.gridMyCategory.columnDefs[4].visible = false;
            $scope.gridMyCategory.columnDefs[5].visible = false;
            $scope.gridMyCategory.columnDefs[6].visible = false;
            $scope.gridMyCategory.columnDefs[7].visible = false;
            $scope.gridMyCategory.columnDefs[8].visible = false;
            $scope.gridMyCategory.columnDefs[9].visible = false;
            $scope.gridMyCategory.columnDefs[10].visible = true;
            $scope.gridMyCategory.columnDefs[11].visible = true;
            $scope.gridMyCategory.columnDefs[12].visible = true;

            //$scope.gridMyCategory.columnDefs[17].visible = false;
            //$scope.gridMyCategory.columnDefs[18].visible = false;
            //$scope.gridMyCategory.columnDefs[19].visible = false;
            //$scope.gridMyCategory.columnDefs[20].visible = false;
            //$scope.gridMyCategory.columnDefs[21].visible = false;
            //$scope.gridMyCategory.columnDefs[22].visible = false;
            //$scope.gridMyCategory.columnDefs[23].visible = true;
            //$scope.gridMyCategory.columnDefs[24].visible = true;
            //$scope.gridMyCategory.columnDefs[25].visible = true;

            $scope.gridMyCategory.columnDefs[26].visible = false;
            $scope.gridMyCategory.columnDefs[27].visible = false;
            $scope.gridMyCategory.columnDefs[28].visible = false;
            $scope.gridMyCategory.columnDefs[29].visible = false;
            $scope.gridMyCategory.columnDefs[30].visible = false;
            $scope.gridMyCategory.columnDefs[31].visible = false;
            $scope.gridMyCategory.columnDefs[32].visible = true;
            $scope.gridMyCategory.columnDefs[33].visible = true;
            $scope.gridMyCategory.columnDefs[34].visible = true;

            $scope.gridMyCategory.columnDefs[35].visible = false;
            $scope.gridMyCategory.columnDefs[36].visible = false;
            $scope.gridMyCategory.columnDefs[37].visible = false;
            $scope.gridMyCategory.columnDefs[38].visible = false;
            $scope.gridMyCategory.columnDefs[39].visible = false;
            $scope.gridMyCategory.columnDefs[40].visible = false;
            $scope.gridMyCategory.columnDefs[41].visible = true;
            $scope.gridMyCategory.columnDefs[42].visible = true;
            $scope.gridMyCategory.columnDefs[43].visible = true;
            $scope.gridMyCategoryApi.grid.refresh();

            //Varietal
            $scope.gridVarietal.columnDefs[4].visible = false;
            $scope.gridVarietal.columnDefs[5].visible = false;
            $scope.gridVarietal.columnDefs[6].visible = false;
            $scope.gridVarietal.columnDefs[7].visible = false;
            $scope.gridVarietal.columnDefs[8].visible = false;
            $scope.gridVarietal.columnDefs[9].visible = false;
            $scope.gridVarietal.columnDefs[10].visible = true;
            $scope.gridVarietal.columnDefs[11].visible = true;
            $scope.gridVarietal.columnDefs[12].visible = true;

            //$scope.gridVarietal.columnDefs[17].visible = false;
            //$scope.gridVarietal.columnDefs[18].visible = false;
            //$scope.gridVarietal.columnDefs[19].visible = false;
            //$scope.gridVarietal.columnDefs[20].visible = false;
            //$scope.gridVarietal.columnDefs[21].visible = false;
            //$scope.gridVarietal.columnDefs[22].visible = false;
            //$scope.gridVarietal.columnDefs[23].visible = true;
            //$scope.gridVarietal.columnDefs[24].visible = true;
            //$scope.gridVarietal.columnDefs[25].visible = true;

            $scope.gridVarietal.columnDefs[26].visible = false;
            $scope.gridVarietal.columnDefs[27].visible = false;
            $scope.gridVarietal.columnDefs[28].visible = false;
            $scope.gridVarietal.columnDefs[29].visible = false;
            $scope.gridVarietal.columnDefs[30].visible = false;
            $scope.gridVarietal.columnDefs[31].visible = false;
            $scope.gridVarietal.columnDefs[32].visible = true;
            $scope.gridVarietal.columnDefs[33].visible = true;
            $scope.gridVarietal.columnDefs[34].visible = true;

            $scope.gridVarietal.columnDefs[35].visible = false;
            $scope.gridVarietal.columnDefs[36].visible = false;
            $scope.gridVarietal.columnDefs[37].visible = false;
            $scope.gridVarietal.columnDefs[38].visible = false;
            $scope.gridVarietal.columnDefs[39].visible = false;
            $scope.gridVarietal.columnDefs[40].visible = false;
            $scope.gridVarietal.columnDefs[41].visible = true;
            $scope.gridVarietal.columnDefs[42].visible = true;
            $scope.gridVarietal.columnDefs[43].visible = true;
            $scope.gridVarietalApi.grid.refresh(); 

            //Price Band
            $scope.gridPriceBand.columnDefs[4].visible = false;
            $scope.gridPriceBand.columnDefs[5].visible = false;
            $scope.gridPriceBand.columnDefs[6].visible = false;
            $scope.gridPriceBand.columnDefs[7].visible = false;
            $scope.gridPriceBand.columnDefs[8].visible = false;
            $scope.gridPriceBand.columnDefs[9].visible = false;
            $scope.gridPriceBand.columnDefs[10].visible = true;
            $scope.gridPriceBand.columnDefs[11].visible = true;
            $scope.gridPriceBand.columnDefs[12].visible = true;

            //$scope.gridPriceBand.columnDefs[17].visible = false;
            //$scope.gridPriceBand.columnDefs[18].visible = false;
            //$scope.gridPriceBand.columnDefs[19].visible = false;
            //$scope.gridPriceBand.columnDefs[20].visible = false;
            //$scope.gridPriceBand.columnDefs[21].visible = false;
            //$scope.gridPriceBand.columnDefs[22].visible = false;
            //$scope.gridPriceBand.columnDefs[23].visible = true;
            //$scope.gridPriceBand.columnDefs[24].visible = true;
            //$scope.gridPriceBand.columnDefs[25].visible = true;

            $scope.gridPriceBand.columnDefs[26].visible = false;
            $scope.gridPriceBand.columnDefs[27].visible = false;
            $scope.gridPriceBand.columnDefs[28].visible = false;
            $scope.gridPriceBand.columnDefs[29].visible = false;
            $scope.gridPriceBand.columnDefs[30].visible = false;
            $scope.gridPriceBand.columnDefs[31].visible = false;
            $scope.gridPriceBand.columnDefs[32].visible = true;
            $scope.gridPriceBand.columnDefs[33].visible = true;
            $scope.gridPriceBand.columnDefs[34].visible = true;

            $scope.gridPriceBand.columnDefs[35].visible = false;
            $scope.gridPriceBand.columnDefs[36].visible = false;
            $scope.gridPriceBand.columnDefs[37].visible = false;
            $scope.gridPriceBand.columnDefs[38].visible = false;
            $scope.gridPriceBand.columnDefs[39].visible = false;
            $scope.gridPriceBand.columnDefs[40].visible = false;
            $scope.gridPriceBand.columnDefs[41].visible = true;
            $scope.gridPriceBand.columnDefs[42].visible = true;
            $scope.gridPriceBand.columnDefs[43].visible = true;
            $scope.gridPriceBandApi.grid.refresh(); 
        } else if ($scope.selectedgroup.id === 2) {
            //color
            $scope.gridColor.columnDefs[4].visible = false;
            $scope.gridColor.columnDefs[5].visible = false;
            $scope.gridColor.columnDefs[6].visible = false;
            $scope.gridColor.columnDefs[7].visible = true;
            $scope.gridColor.columnDefs[8].visible = true;
            $scope.gridColor.columnDefs[9].visible = true;
            $scope.gridColor.columnDefs[10].visible = false;
            $scope.gridColor.columnDefs[11].visible = false;
            $scope.gridColor.columnDefs[12].visible = false;

            //$scope.gridColor.columnDefs[17].visible = false;
            //$scope.gridColor.columnDefs[18].visible = false;
            //$scope.gridColor.columnDefs[19].visible = false;
            //$scope.gridColor.columnDefs[20].visible = true;
            //$scope.gridColor.columnDefs[21].visible = true;
            //$scope.gridColor.columnDefs[22].visible = true;
            //$scope.gridColor.columnDefs[23].visible = false;
            //$scope.gridColor.columnDefs[24].visible = false;
            //$scope.gridColor.columnDefs[25].visible = false;

            $scope.gridColor.columnDefs[26].visible = false;
            $scope.gridColor.columnDefs[27].visible = false;
            $scope.gridColor.columnDefs[28].visible = false;
            $scope.gridColor.columnDefs[29].visible = true;
            $scope.gridColor.columnDefs[30].visible = true;
            $scope.gridColor.columnDefs[31].visible = true;
            $scope.gridColor.columnDefs[32].visible = false;
            $scope.gridColor.columnDefs[33].visible = false;
            $scope.gridColor.columnDefs[34].visible = false;

            $scope.gridColor.columnDefs[35].visible = false;
            $scope.gridColor.columnDefs[36].visible = false;
            $scope.gridColor.columnDefs[37].visible = false;
            $scope.gridColor.columnDefs[38].visible = true;
            $scope.gridColor.columnDefs[39].visible = true;
            $scope.gridColor.columnDefs[40].visible = true;
            $scope.gridColor.columnDefs[41].visible = false;
            $scope.gridColor.columnDefs[42].visible = false;
            $scope.gridColor.columnDefs[43].visible = false;
            $scope.gridColorApi.grid.refresh();

            //Country
            $scope.gridCountry.columnDefs[4].visible = false;
            $scope.gridCountry.columnDefs[5].visible = false;
            $scope.gridCountry.columnDefs[6].visible = false;
            $scope.gridCountry.columnDefs[7].visible = true;
            $scope.gridCountry.columnDefs[8].visible = true;
            $scope.gridCountry.columnDefs[9].visible = true;
            $scope.gridCountry.columnDefs[10].visible = false;
            $scope.gridCountry.columnDefs[11].visible = false;
            $scope.gridCountry.columnDefs[12].visible = false;

            //$scope.gridCountry.columnDefs[17].visible = false;
            //$scope.gridCountry.columnDefs[18].visible = false;
            //$scope.gridCountry.columnDefs[19].visible = false;
            //$scope.gridCountry.columnDefs[20].visible = true;
            //$scope.gridCountry.columnDefs[21].visible = true;
            //$scope.gridCountry.columnDefs[22].visible = true;
            //$scope.gridCountry.columnDefs[23].visible = false;
            //$scope.gridCountry.columnDefs[24].visible = false;
            //$scope.gridCountry.columnDefs[25].visible = false;

            $scope.gridCountry.columnDefs[26].visible = false;
            $scope.gridCountry.columnDefs[27].visible = false;
            $scope.gridCountry.columnDefs[28].visible = false;
            $scope.gridCountry.columnDefs[29].visible = true;
            $scope.gridCountry.columnDefs[30].visible = true;
            $scope.gridCountry.columnDefs[31].visible = true;
            $scope.gridCountry.columnDefs[32].visible = false;
            $scope.gridCountry.columnDefs[33].visible = false;
            $scope.gridCountry.columnDefs[34].visible = false;

            $scope.gridCountry.columnDefs[35].visible = false;
            $scope.gridCountry.columnDefs[36].visible = false;
            $scope.gridCountry.columnDefs[37].visible = false;
            $scope.gridCountry.columnDefs[38].visible = true;
            $scope.gridCountry.columnDefs[39].visible = true;
            $scope.gridCountry.columnDefs[40].visible = true;
            $scope.gridCountry.columnDefs[41].visible = false;
            $scope.gridCountry.columnDefs[42].visible = false;
            $scope.gridCountry.columnDefs[43].visible = false;
            $scope.gridCountryApi.grid.refresh();

            //My Category
            $scope.gridMyCategory.columnDefs[4].visible = false;
            $scope.gridMyCategory.columnDefs[5].visible = false;
            $scope.gridMyCategory.columnDefs[6].visible = false;
            $scope.gridMyCategory.columnDefs[7].visible = true;
            $scope.gridMyCategory.columnDefs[8].visible = true;
            $scope.gridMyCategory.columnDefs[9].visible = true;
            $scope.gridMyCategory.columnDefs[10].visible = false;
            $scope.gridMyCategory.columnDefs[11].visible = false;
            $scope.gridMyCategory.columnDefs[12].visible = false;

            //$scope.gridMyCategory.columnDefs[17].visible = false;
            //$scope.gridMyCategory.columnDefs[18].visible = false;
            //$scope.gridMyCategory.columnDefs[19].visible = false;
            //$scope.gridMyCategory.columnDefs[20].visible = true;
            //$scope.gridMyCategory.columnDefs[21].visible = true;
            //$scope.gridMyCategory.columnDefs[22].visible = true;
            //$scope.gridMyCategory.columnDefs[23].visible = false;
            //$scope.gridMyCategory.columnDefs[24].visible = false;
            //$scope.gridMyCategory.columnDefs[25].visible = false;

            $scope.gridMyCategory.columnDefs[26].visible = false;
            $scope.gridMyCategory.columnDefs[27].visible = false;
            $scope.gridMyCategory.columnDefs[28].visible = false;
            $scope.gridMyCategory.columnDefs[29].visible = true;
            $scope.gridMyCategory.columnDefs[30].visible = true;
            $scope.gridMyCategory.columnDefs[31].visible = true;
            $scope.gridMyCategory.columnDefs[32].visible = false;
            $scope.gridMyCategory.columnDefs[33].visible = false;
            $scope.gridMyCategory.columnDefs[34].visible = false;

            $scope.gridMyCategory.columnDefs[35].visible = false;
            $scope.gridMyCategory.columnDefs[36].visible = false;
            $scope.gridMyCategory.columnDefs[37].visible = false;
            $scope.gridMyCategory.columnDefs[38].visible = true;
            $scope.gridMyCategory.columnDefs[39].visible = true;
            $scope.gridMyCategory.columnDefs[40].visible = true;
            $scope.gridMyCategory.columnDefs[41].visible = false;
            $scope.gridMyCategory.columnDefs[42].visible = false;
            $scope.gridMyCategory.columnDefs[43].visible = false;
            $scope.gridMyCategoryApi.grid.refresh();

            //Varietal
            $scope.gridVarietal.columnDefs[4].visible = false;
            $scope.gridVarietal.columnDefs[5].visible = false;
            $scope.gridVarietal.columnDefs[6].visible = false;
            $scope.gridVarietal.columnDefs[7].visible = true;
            $scope.gridVarietal.columnDefs[8].visible = true;
            $scope.gridVarietal.columnDefs[9].visible = true;
            $scope.gridVarietal.columnDefs[10].visible = false;
            $scope.gridVarietal.columnDefs[11].visible = false;
            $scope.gridVarietal.columnDefs[12].visible = false;

            //$scope.gridVarietal.columnDefs[17].visible = false;
            //$scope.gridVarietal.columnDefs[18].visible = false;
            //$scope.gridVarietal.columnDefs[19].visible = false;
            //$scope.gridVarietal.columnDefs[20].visible = true;
            //$scope.gridVarietal.columnDefs[21].visible = true;
            //$scope.gridVarietal.columnDefs[22].visible = true;
            //$scope.gridVarietal.columnDefs[23].visible = false;
            //$scope.gridVarietal.columnDefs[24].visible = false;
            //$scope.gridVarietal.columnDefs[25].visible = false;

            $scope.gridVarietal.columnDefs[26].visible = false;
            $scope.gridVarietal.columnDefs[27].visible = false;
            $scope.gridVarietal.columnDefs[28].visible = false;
            $scope.gridVarietal.columnDefs[29].visible = true;
            $scope.gridVarietal.columnDefs[30].visible = true;
            $scope.gridVarietal.columnDefs[31].visible = true;
            $scope.gridVarietal.columnDefs[32].visible = false;
            $scope.gridVarietal.columnDefs[33].visible = false;
            $scope.gridVarietal.columnDefs[34].visible = false;

            $scope.gridVarietal.columnDefs[35].visible = false;
            $scope.gridVarietal.columnDefs[36].visible = false;
            $scope.gridVarietal.columnDefs[37].visible = false;
            $scope.gridVarietal.columnDefs[38].visible = true;
            $scope.gridVarietal.columnDefs[39].visible = true;
            $scope.gridVarietal.columnDefs[40].visible = true;
            $scope.gridVarietal.columnDefs[41].visible = false;
            $scope.gridVarietal.columnDefs[42].visible = false;
            $scope.gridVarietal.columnDefs[43].visible = false;
            $scope.gridVarietalApi.grid.refresh(); 

            //Price Band
            $scope.gridPriceBand.columnDefs[4].visible = false;
            $scope.gridPriceBand.columnDefs[5].visible = false;
            $scope.gridPriceBand.columnDefs[6].visible = false;
            $scope.gridPriceBand.columnDefs[7].visible = true;
            $scope.gridPriceBand.columnDefs[8].visible = true;
            $scope.gridPriceBand.columnDefs[9].visible = true;
            $scope.gridPriceBand.columnDefs[10].visible = false;
            $scope.gridPriceBand.columnDefs[11].visible = false;
            $scope.gridPriceBand.columnDefs[12].visible = false;

            //$scope.gridPriceBand.columnDefs[17].visible = false;
            //$scope.gridPriceBand.columnDefs[18].visible = false;
            //$scope.gridPriceBand.columnDefs[19].visible = false;
            //$scope.gridPriceBand.columnDefs[20].visible = true;
            //$scope.gridPriceBand.columnDefs[21].visible = true;
            //$scope.gridPriceBand.columnDefs[22].visible = true;
            //$scope.gridPriceBand.columnDefs[23].visible = false;
            //$scope.gridPriceBand.columnDefs[24].visible = false;
            //$scope.gridPriceBand.columnDefs[25].visible = false;

            $scope.gridPriceBand.columnDefs[26].visible = false;
            $scope.gridPriceBand.columnDefs[27].visible = false;
            $scope.gridPriceBand.columnDefs[28].visible = false;
            $scope.gridPriceBand.columnDefs[29].visible = true;
            $scope.gridPriceBand.columnDefs[30].visible = true;
            $scope.gridPriceBand.columnDefs[31].visible = true;
            $scope.gridPriceBand.columnDefs[32].visible = false;
            $scope.gridPriceBand.columnDefs[33].visible = false;
            $scope.gridPriceBand.columnDefs[34].visible = false;

            $scope.gridPriceBand.columnDefs[35].visible = false;
            $scope.gridPriceBand.columnDefs[36].visible = false;
            $scope.gridPriceBand.columnDefs[37].visible = false;
            $scope.gridPriceBand.columnDefs[38].visible = true;
            $scope.gridPriceBand.columnDefs[39].visible = true;
            $scope.gridPriceBand.columnDefs[40].visible = true;
            $scope.gridPriceBand.columnDefs[41].visible = false;
            $scope.gridPriceBand.columnDefs[42].visible = false;
            $scope.gridPriceBand.columnDefs[43].visible = false;
            $scope.gridPriceBandApi.grid.refresh(); 
        }

       // $scope.gridApi.grid.refresh();
        

        //set export file name
        $scope.gridColor.exporterCsvFilename = 'Market Reports Sales Summary by Color - Ontario - Last Completed Period: ' + strPeriod + '.csv';
        $scope.gridCountry.exporterCsvFilename = 'Market Reports Sales Summary by Country - Ontario - Last Completed Period: ' + strPeriod + '.csv';
        $scope.gridVarietal.exporterCsvFilename = 'Market Reports Sales Summary by Varietal - Ontario - Last Completed Period: ' + strPeriod + '.csv';
        $scope.gridMyCategory.exporterCsvFilename = 'Market Reports Sales Summary by Category - Ontario - Last Completed Period: ' + strPeriod + '.csv';
        $scope.gridMyCategory.exporterCsvFilename = 'Market Reports Sales Summary by Price Band - Ontario - Last Completed Period: ' + strPeriod + '.csv';
        //determine if valid to update grid
        var indicator;
        indicator = 0;

        if ((!fromValue && fromValue !== 0) || !toValue) {
            WarningHandler('Retail Price Value Is Not Correctly Entered');
            indicator += 1;
        }

        if (fromValue >= toValue) {
            WarningHandler('Retail Price Value Is Not Correctly Entered');
            indicator += 1;
        }

        if (strSetNames === "0") {
            WarningHandler('You Have To Select Value(s) In Set Names');
            indicator += 1;
        }

        if (strUnitSizes === "0") {
            WarningHandler('You Have To Select Value(s) In Unit Sizes');
            indicator += 1;
        }

        if (indicator === 0) {
            //loading
            $scope.loading = true;
            var mybody = angular.element(document).find('body');
            mybody.addClass('waiting');

            //Color
            $http({
                url: "/SalesSummaryByColor",
                dataType: 'json',
                method: 'GET',
                data: '',
                headers: {
                    "Content-Type": "application/json",
                    "X-PeriodKey": strPeriod,
                    "X-SetNames": strSetNames,
                    "X-UnitSizes": strUnitSizes,
                    "X-PriceFrom": $scope.pricefrom.value,
                    "X-PriceTo": $scope.priceto.value,
                    "X-Group" : $scope.selectedgroup.id

                }
            }).
                then(
                function (result) {
                  //  $scope.gridColor.data = result.data;
                    var len = result.data.length;
                    $scope.gridColor.data = result.data.slice(1, len);
                    var color_ty_9Lcasespct = document.getElementById("Color_TY_9LCasesPct");
                    if (color_ty_9Lcasespct)
                        color_ty_9Lcasespct.textContent = (result.data[0].TY_9LCasesPct * 100).toFixed(2).toString() + "%";

                    var color_ty_unitspct = document.getElementById("Color_TY_UnitsPct");
                    if (color_ty_unitspct)
                        color_ty_unitspct.textContent = (result.data[0].TY_UnitsPct * 100).toFixed(2).toString() + "%";

                    var color_ty_totalsalesamountpct = document.getElementById("Color_TY_TotalSalesAmountPct");
                    if (color_ty_totalsalesamountpct)
                        color_ty_totalsalesamountpct.textContent = (result.data[0].TY_TotalSalesAmountPct * 100).toFixed(2).toString() + "%";

                    var color_ty_td_9Lcasespct = document.getElementById("Color_TY_TD_9LCasesPct");
                    if (color_ty_td_9Lcasespct)
                        color_ty_td_9Lcasespct.textContent = (result.data[0].TY_9LCasesPct * 100).toFixed(2).toString() + "%";

                    var color_ty_td_unitspct = document.getElementById("Color_TY_TD_UnitsPct");
                    if (color_ty_td_unitspct)
                        color_ty_td_unitspct.textContent = (result.data[0].TY_UnitsPct * 100).toFixed(2).toString() + "%";

                    var color_ty_td_totalsalesamountpct = document.getElementById("Color_TY_TD_TotalSalesAmountPct");
                    if (color_ty_td_totalsalesamountpct)
                        color_ty_td_totalsalesamountpct.textContent = (result.data[0].TY_TotalSalesAmountPct * 100).toFixed(2).toString() + "%";

                    var color_ty6m_9Lcasespct = document.getElementById("Color_TY6m_9LCasesPct");
                    if (color_ty6m_9Lcasespct)
                        color_ty6m_9Lcasespct.textContent = (result.data[0].TY6m_9LCasesPct * 100).toFixed(2).toString() + "%";

                    var color_ty6m_unitspct = document.getElementById("Color_TY6m_UnitsPct");
                    if (color_ty6m_unitspct)
                        color_ty6m_unitspct.textContent = (result.data[0].TY6m_UnitsPct * 100).toFixed(2).toString() + "%";

                    var color_ty6m_totalsalesamountpct = document.getElementById("Color_TY6m_TotalSalesAmountPct");
                    if (color_ty6m_totalsalesamountpct)
                        color_ty6m_totalsalesamountpct.textContent = (result.data[0].TY6m_TotalSalesAmountPct * 100).toFixed(2).toString() + "%";

                    var color_tyytd_9Lcasespct = document.getElementById("Color_TYYTD_9LCasesPct");
                    if (color_tyytd_9Lcasespct)
                        color_tyytd_9Lcasespct.textContent = (result.data[0].TYYTD_9LCasesPct * 100).toFixed(2).toString() + "%";

                    var color_tyytd_unitspct = document.getElementById("Color_TYYTD_UnitsPct");
                    if (color_tyytd_unitspct)
                        color_tyytd_unitspct.textContent = (result.data[0].TYYTD_UnitsPct * 100).toFixed(2).toString() + "%";

                    var color_tyytd_totalsalesamountpct = document.getElementById("Color_TYYTD_SalesAmountPct");
                    if (color_tyytd_totalsalesamountpct)
                        color_tyytd_totalsalesamountpct.textContent = (result.data[0].TYYTD_SalesAmountPct * 100).toFixed(2).toString() + "%";

                    var color_ty13_onprempct = document.getElementById("Color_TY13_OnPremPct");
                    if (color_ty13_onprempct)
                        color_ty13_onprempct.textContent = (result.data[0].TY13_OnPremPct * 100).toFixed(2).toString() + "%";

                    var color_ty13amountpct = document.getElementById("Color_TY13AmountPct");
                    if (color_ty13amountpct)
                        color_ty13amountpct.textContent = (result.data[0].TY13AmountPct * 100).toFixed(2).toString() + "%";
                },
                function (error) {
                    InternalErrorHandler(error);
                    $scope.loading = false;
                    mybody.removeClass('waiting');
                }).finally(function () {
                    $scope.loading = false;
                    mybody.removeClass('waiting');
                    $scope.tmpPeriod = strPeriod;
                    $scope.tmpSetNames = strSetNames;
                    $scope.tmpUnitSizes = strUnitSizes;
                    $scope.tmpPriceFrom = $scope.pricefrom.value;
                    $scope.tmpPriceTo = $scope.priceto.value;
                    $scope.tmpGroup = $scope.selectedgroup.id;
                });

            //Country
            $http({
                url: "/SalesSummaryByCountry",
                dataType: 'json',
                method: 'GET',
                data: '',
                headers: {
                    "Content-Type": "application/json",
                    "X-PeriodKey": strPeriod,
                    "X-SetNames": strSetNames,
                    "X-UnitSizes": strUnitSizes,
                    "X-PriceFrom": $scope.pricefrom.value,
                    "X-PriceTo": $scope.priceto.value,
                    "X-Group": $scope.selectedgroup.id
                }
            }).
                then(
                function (result) {
                    //$scope.gridColor.data = result.data;
                    var len = result.data.length;
                    $scope.gridCountry.data = result.data.slice(1, len);
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
                    if (country_ty_td_unitspct)
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
                    $scope.tmpPeriod = strPeriod;
                    $scope.tmpSetNames = strSetNames;
                    $scope.tmpUnitSizes = strUnitSizes;
                    $scope.tmpPriceFrom = $scope.pricefrom.value;
                    $scope.tmpPriceTo = $scope.priceto.value;
                    $scope.tmpGroup = $scope.selectedgroup.id;
                });

            //My Category
            $http({
                url: "/SalesSummaryByMyCategory",
                dataType: 'json',
                method: 'GET',
                data: '',
                headers: {
                    "Content-Type": "application/json",
                    "X-PeriodKey": strPeriod,
                    "X-SetNames": strSetNames,
                    "X-UnitSizes": strUnitSizes,
                    "X-PriceFrom": $scope.pricefrom.value,
                    "X-PriceTo": $scope.priceto.value,
                    "X-Group": $scope.selectedgroup.id
                }
            }).
                then(
                function (result) {
                    var len = result.data.length;
                    $scope.gridMyCategory.data = result.data.slice(1, len);
                    var mycategory_ty_9Lcasespct = document.getElementById("MyCategory_TY_9LCasesPct");
                    if (mycategory_ty_9Lcasespct)
                        mycategory_ty_9Lcasespct.textContent = (result.data[0].TY_9LCasesPct * 100).toFixed(2).toString() + "%";

                    var mycategory_ty_unitspct = document.getElementById("MyCategory_TY_UnitsPct");
                    if (mycategory_ty_unitspct)
                        mycategory_ty_unitspct.textContent = (result.data[0].TY_UnitsPct * 100).toFixed(2).toString() + "%";

                    var mycategory_ty_totalsalesamountpct = document.getElementById("MyCategory_TY_TotalSalesAmountPct");
                    if (mycategory_ty_totalsalesamountpct)
                        mycategory_ty_totalsalesamountpct.textContent = (result.data[0].TY_TotalSalesAmountPct * 100).toFixed(2).toString() + "%";

                    var mycategory_ty_td_9Lcasespct = document.getElementById("MyCategory_TY_TD_9LCasesPct");
                    if (mycategory_ty_td_9Lcasespct)
                        mycategory_ty_td_9Lcasespct.textContent = (result.data[0].TY_9LCasesPct * 100).toFixed(2).toString() + "%";

                    var mycategory_ty_td_unitspct = document.getElementById("MyCategory_TY_TD_UnitsPct");
                    if (mycategory_ty_td_unitspct)
                        mycategory_ty_td_unitspct.textContent = (result.data[0].TY_UnitsPct * 100).toFixed(2).toString() + "%";

                    var mycategory_ty_td_totalsalesamountpct = document.getElementById("MyCategory_TY_TD_TotalSalesAmountPct");
                    if (mycategory_ty_td_totalsalesamountpct)
                        mycategory_ty_td_totalsalesamountpct.textContent = (result.data[0].TY_TotalSalesAmountPct * 100).toFixed(2).toString() + "%";

                    var mycategory_ty6m_9Lcasespct = document.getElementById("MyCategory_TY6m_9LCasesPct");
                    if (mycategory_ty6m_9Lcasespct)
                        mycategory_ty6m_9Lcasespct.textContent = (result.data[0].TY6m_9LCasesPct * 100).toFixed(2).toString() + "%";

                    var mycategory_ty6m_unitspct = document.getElementById("MyCategory_TY6m_UnitsPct");
                    if (mycategory_ty6m_unitspct)
                        mycategory_ty6m_unitspct.textContent = (result.data[0].TY6m_UnitsPct * 100).toFixed(2).toString() + "%";

                    var mycategory_ty6m_totalsalesamountpct = document.getElementById("MyCategory_TY6m_TotalSalesAmountPct");
                    if (mycategory_ty6m_totalsalesamountpct)
                        mycategory_ty6m_totalsalesamountpct.textContent = (result.data[0].TY6m_TotalSalesAmountPct * 100).toFixed(2).toString() + "%";

                    var mycategory_tyytd_9Lcasespct = document.getElementById("MyCategory_TYYTD_9LCasesPct");
                    if (mycategory_tyytd_9Lcasespct)
                        mycategory_tyytd_9Lcasespct.textContent = (result.data[0].TYYTD_9LCasesPct * 100).toFixed(2).toString() + "%";

                    var mycategory_tyytd_unitspct = document.getElementById("MyCategory_TYYTD_UnitsPct");
                    if (mycategory_tyytd_unitspct)
                        mycategory_tyytd_unitspct.textContent = (result.data[0].TYYTD_UnitsPct * 100).toFixed(2).toString() + "%";

                    var mycategory_tyytd_totalsalesamountpct = document.getElementById("MyCategory_TYYTD_SalesAmountPct");
                    if (mycategory_tyytd_totalsalesamountpct)
                        mycategory_tyytd_totalsalesamountpct.textContent = (result.data[0].TYYTD_SalesAmountPct * 100).toFixed(2).toString() + "%";

                    var mycategory_ty13_onprempct = document.getElementById("MyCategory_TY13_OnPremPct");
                    if (mycategory_ty13_onprempct)
                        mycategory_ty13_onprempct.textContent = (result.data[0].TY13_OnPremPct * 100).toFixed(2).toString() + "%";

                    var mycategory_ty13amountpct = document.getElementById("MyCategory_TY13AmountPct");
                    if (mycategory_ty13amountpct)
                        mycategory_ty13amountpct.textContent = (result.data[0].TY13AmountPct * 100).toFixed(2).toString() + "%";
                },
                function (error) {
                    InternalErrorHandler(error);
                    $scope.loading = false;
                    mybody.removeClass('waiting');
                }).finally(function () {
                    $scope.loading = false;
                    mybody.removeClass('waiting');
                    $scope.tmpPeriod = strPeriod;
                    $scope.tmpSetNames = strSetNames;
                    $scope.tmpUnitSizes = strUnitSizes;
                    $scope.tmpPriceFrom = $scope.pricefrom.value;
                    $scope.tmpPriceTo = $scope.priceto.value;
                    $scope.tmpGroup = $scope.selectedgroup.id;
                });
            //Price Band
            $http({
                url: "/SalesSummaryByPriceBand",
                dataType: 'json',
                method: 'GET',
                data: '',
                headers: {
                    "Content-Type": "application/json",
                    "X-PeriodKey": strPeriod,
                    "X-SetNames": strSetNames,
                    "X-UnitSizes": strUnitSizes,
                    "X-PriceFrom": $scope.pricefrom.value,
                    "X-PriceTo": $scope.priceto.value,
                    "X-Group": $scope.selectedgroup.id
                }
            }).
                then(
                function (result) {
                    var len = result.data.length;
                    $scope.gridPriceBand.data = result.data.slice(1, len);
                    var priceband_ty_9Lcasespct = document.getElementById("PriceBand_TY_9LCasesPct");
                    if (priceband_ty_9Lcasespct)
                        priceband_ty_9Lcasespct.textContent = (result.data[0].TY_9LCasesPct * 100).toFixed(2).toString() + "%";

                    var priceband_ty_unitspct = document.getElementById("PriceBand_TY_UnitsPct");
                    if (priceband_ty_unitspct)
                        priceband_ty_unitspct.textContent = (result.data[0].TY_UnitsPct * 100).toFixed(2).toString() + "%";

                    var priceband_ty_totalsalesamountpct = document.getElementById("PriceBand_TY_TotalSalesAmountPct");
                    if (priceband_ty_totalsalesamountpct)
                        priceband_ty_totalsalesamountpct.textContent = (result.data[0].TY_TotalSalesAmountPct * 100).toFixed(2).toString() + "%";

                    var priceband_ty_td_9Lcasespct = document.getElementById("PriceBand_TY_TD_9LCasesPct");
                    if (priceband_ty_td_9Lcasespct)
                        priceband_ty_td_9Lcasespct.textContent = (result.data[0].TY_9LCasesPct * 100).toFixed(2).toString() + "%";

                    var priceband_ty_td_unitspct = document.getElementById("PriceBand_TY_TD_UnitsPct");
                    if (priceband_ty_td_unitspct)
                        priceband_ty_td_unitspct.textContent = (result.data[0].TY_UnitsPct * 100).toFixed(2).toString() + "%";

                    var priceband_ty_td_totalsalesamountpct = document.getElementById("PriceBand_TY_TD_TotalSalesAmountPct");
                    if (priceband_ty_td_totalsalesamountpct)
                        priceband_ty_td_totalsalesamountpct.textContent = (result.data[0].TY_TotalSalesAmountPct * 100).toFixed(2).toString() + "%";

                    var priceband_ty6m_9Lcasespct = document.getElementById("PriceBand_TY6m_9LCasesPct");
                    if (priceband_ty6m_9Lcasespct)
                        priceband_ty6m_9Lcasespct.textContent = (result.data[0].TY6m_9LCasesPct * 100).toFixed(2).toString() + "%";

                    var priceband_ty6m_unitspct = document.getElementById("PriceBand_TY6m_UnitsPct");
                    if (priceband_ty6m_unitspct)
                        priceband_ty6m_unitspct.textContent = (result.data[0].TY6m_UnitsPct * 100).toFixed(2).toString() + "%";

                    var priceband_ty6m_totalsalesamountpct = document.getElementById("PriceBand_TY6m_TotalSalesAmountPct");
                    if (priceband_ty6m_totalsalesamountpct)
                        priceband_ty6m_totalsalesamountpct.textContent = (result.data[0].TY6m_TotalSalesAmountPct * 100).toFixed(2).toString() + "%";

                    var priceband_tyytd_9Lcasespct = document.getElementById("PriceBand_TYYTD_9LCasesPct");
                    if (priceband_tyytd_9Lcasespct)
                        priceband_tyytd_9Lcasespct.textContent = (result.data[0].TYYTD_9LCasesPct * 100).toFixed(2).toString() + "%";

                    var priceband_tyytd_unitspct = document.getElementById("PriceBand_TYYTD_UnitsPct");
                    if (priceband_tyytd_unitspct)
                        priceband_tyytd_unitspct.textContent = (result.data[0].TYYTD_UnitsPct * 100).toFixed(2).toString() + "%";

                    var priceband_tyytd_totalsalesamountpct = document.getElementById("PriceBand_TYYTD_SalesAmountPct");
                    if (priceband_tyytd_totalsalesamountpct)
                        priceband_tyytd_totalsalesamountpct.textContent = (result.data[0].TYYTD_SalesAmountPct * 100).toFixed(2).toString() + "%";

                    var priceband_ty13_onprempct = document.getElementById("PriceBand_TY13_OnPremPct");
                    if (priceband_ty13_onprempct)
                        priceband_ty13_onprempct.textContent = (result.data[0].TY13_OnPremPct * 100).toFixed(2).toString() + "%";

                    var priceband_ty13amountpct = document.getElementById("PriceBand_TY13AmountPct");
                    if (priceband_ty13amountpct)
                        priceband_ty13amountpct.textContent = (result.data[0].TY13AmountPct * 100).toFixed(2).toString() + "%";
                },
                function (error) {
                    InternalErrorHandler(error);
                    $scope.loading = false;
                    mybody.removeClass('waiting');
                }).finally(function () {
                    $scope.loading = false;
                    mybody.removeClass('waiting');
                    $scope.tmpPeriod = strPeriod;
                    $scope.tmpSetNames = strSetNames;
                    $scope.tmpUnitSizes = strUnitSizes;
                    $scope.tmpPriceFrom = $scope.pricefrom.value;
                    $scope.tmpPriceTo = $scope.priceto.value;
                    $scope.tmpGroup = $scope.selectedgroup.id;
                });

            //Varietal
            $http({
                url: "/SalesSummaryByVarietal",
                dataType: 'json',
                method: 'GET',
                data: '',
                headers: {
                    "Content-Type": "application/json",
                    "X-PeriodKey": strPeriod,
                    "X-SetNames": strSetNames,
                    "X-UnitSizes": strUnitSizes,
                    "X-PriceFrom": $scope.pricefrom.value,
                    "X-PriceTo": $scope.priceto.value,
                    "X-Group": $scope.selectedgroup.id
                }
            }).
                then(
                function (result) {
                    var len = result.data.length;
                    $scope.gridVarietal.data = result.data.slice(1, len);
                    var varietal_ty_9Lcasespct = document.getElementById("Varietal_TY_9LCasesPct");
                    if (varietal_ty_9Lcasespct)
                        varietal_ty_9Lcasespct.textContent = (result.data[0].TY_9LCasesPct * 100).toFixed(2).toString() + "%";

                    var varietal_ty_unitspct = document.getElementById("varietal_TY_UnitsPct");
                    if (varietal_ty_unitspct)
                        varietal_ty_unitspct.textContent = (result.data[0].TY_UnitsPct * 100).toFixed(2).toString() + "%";

                    var varietal_ty_totalsalesamountpct = document.getElementById("Varietal_TY_TotalSalesAmountPct");
                    if (varietal_ty_totalsalesamountpct)
                        varietal_ty_totalsalesamountpct.textContent = (result.data[0].TY_TotalSalesAmountPct * 100).toFixed(2).toString() + "%";

                    var varietal_ty_td_9Lcasespct = document.getElementById("Varietal_TY_TD_9LCasesPct");
                    if (varietal_ty_td_9Lcasespct)
                        varietal_ty_td_9Lcasespct.textContent = (result.data[0].TY_9LCasesPct * 100).toFixed(2).toString() + "%";

                    var varietal_ty_td_unitspct = document.getElementById("Varietal_TY_TD_UnitsPct");
                    if (varietal_ty_td_unitspct)
                        varietal_ty_td_unitspct.textContent = (result.data[0].TY_UnitsPct * 100).toFixed(2).toString() + "%";

                    var varietal_ty_td_totalsalesamountpct = document.getElementById("Varietal_TY_TD_TotalSalesAmountPct");
                    if (varietal_ty_td_totalsalesamountpct)
                        varietal_ty_td_totalsalesamountpct.textContent = (result.data[0].TY_TotalSalesAmountPct * 100).toFixed(2).toString() + "%";

                    var varietal_ty6m_9Lcasespct = document.getElementById("Varietal_TY6m_9LCasesPct");
                    if (varietal_ty6m_9Lcasespct)
                        varietal_ty6m_9Lcasespct.textContent = (result.data[0].TY6m_9LCasesPct * 100).toFixed(2).toString() + "%";

                    var varietal_ty6m_unitspct = document.getElementById("Varietal_TY6m_UnitsPct");
                    if (varietal_ty6m_unitspct)
                        varietal_ty6m_unitspct.textContent = (result.data[0].TY6m_UnitsPct * 100).toFixed(2).toString() + "%";

                    var varietal_ty6m_totalsalesamountpct = document.getElementById("Varietal_TY6m_TotalSalesAmountPct");
                    if (varietal_ty6m_totalsalesamountpct)
                        varietal_ty6m_totalsalesamountpct.textContent = (result.data[0].TY6m_TotalSalesAmountPct * 100).toFixed(2).toString() + "%";

                    var varietal_tyytd_9Lcasespct = document.getElementById("Varietal_TYYTD_9LCasesPct");
                    if (varietal_tyytd_9Lcasespct)
                        varietal_tyytd_9Lcasespct.textContent = (result.data[0].TYYTD_9LCasesPct * 100).toFixed(2).toString() + "%";

                    var varietal_tyytd_unitspct = document.getElementById("Varietal_TYYTD_UnitsPct");
                    if (varietal_tyytd_unitspct)
                        varietal_tyytd_unitspct.textContent = (result.data[0].TYYTD_UnitsPct * 100).toFixed(2).toString() + "%";

                    var varietal_tyytd_totalsalesamountpct = document.getElementById("Varietal_TYYTD_SalesAmountPct");
                    if (varietal_tyytd_totalsalesamountpct)
                        varietal_tyytd_totalsalesamountpct.textContent = (result.data[0].TYYTD_SalesAmountPct * 100).toFixed(2).toString() + "%";

                    var varietal_ty13_onprempct = document.getElementById("Varietal_TY13_OnPremPct");
                    if (varietal_ty13_onprempct)
                        varietal_ty13_onprempct.textContent = (result.data[0].TY13_OnPremPct * 100).toFixed(2).toString() + "%";

                    var Varietal_ty13amountpct = document.getElementById("Varietal_TY13AmountPct");
                    if (varietal_ty13amountpct)
                        varietal_ty13amountpct.textContent = (result.data[0].TY13AmountPct * 100).toFixed(2).toString() + "%";
                },
                function (error) {
                    InternalErrorHandler(error);
                    $scope.loading = false;
                    mybody.removeClass('waiting');
                }).finally(function () {
                    $scope.loading = false;
                    mybody.removeClass('waiting');
                    $scope.tmpPeriod = strPeriod;
                    $scope.tmpSetNames = strSetNames;
                    $scope.tmpUnitSizes = strUnitSizes;
                    $scope.tmpPriceFrom = $scope.pricefrom.value;
                    $scope.tmpPriceTo = $scope.priceto.value;
                    $scope.tmpGroup = $scope.selectedgroup.id;
                }); 
        }
    }

    //dropdown
    $scope.periodkeydropboxitemselected = function (item) {
        $scope.selectedperiodkey = item;
        //set export file name
        $scope.gridColor.exporterCsvFilename = 'Market Reports Sales Summary by Color - Ontario - Last Completed Period: ' + $scope.selectedperiodkey.label + '.csv'
        $scope.gridCountry.exporterCsvFilename = 'Market Reports Sales Summary by Country - Ontario - Last Completed Period: ' + $scope.selectedperiodkey.label + '.csv'
        $scope.gridVarietal.exporterCsvFilename = 'Market Reports Sales Summary by Varietal - Ontario - Last Completed Period: ' + $scope.selectedperiodkey.label + '.csv'
        $scope.gridMyCategory.exporterCsvFilename = 'Market Reports Sales Summary by Category - Ontario - Last Completed Period: ' + $scope.selectedperiodkey.label + '.csv'
        $scope.gridPriceBand.exporterCsvFilename = 'Market Reports Sales Summary by Price Band - Ontario - Last Completed Period: ' + $scope.selectedperiodkey.label + '.csv'
    }

    $scope.groups = GroupDropDown.groups;
    $scope.selectedgroup = (GroupService.getCatGroup($scope.currentUserInfo) != '' && GroupService.getCatGroup($scope.currentUserInfo) != null) ? GroupService.getCatGroup($scope.currentUserInfo) : GroupDropDown.groups[0];

    $scope.groupdropboxitemselected = function (item) {
        GroupService.setCatGroup($scope.currentUserInfo, item)
        $scope.selectedgroup = item;
    }

    //cascading parameters refreshing
    $scope.Update = {
        onSelectionChanged: function () {

            var strPeriodKey = $scope.selectedperiodkey.label;
            var strSetNames = setnamesToString($scope.setnamesmodel);

            //set export file name
            $scope.gridColor.exporterCsvFilename = 'Market Reports Sales Summary by Color - Ontario - Last Completed Period: ' + strPeriodKey + '.csv'
            $scope.gridCountry.exporterCsvFilename = 'Market Reports Sales Summary by Country - Ontario - Last Completed Period: ' + $scope.selectedperiodkey.label + '.csv'
            $scope.gridVarietal.exporterCsvFilename = 'Market Reports Sales Summary by Varietal - Ontario - Last Completed Period: ' + $scope.selectedperiodkey.label + '.csv'
            $scope.gridMyCategory.exporterCsvFilename = 'Market Reports Sales Summary by Category - Ontario - Last Completed Period: ' + $scope.selectedperiodkey.label + '.csv'
            $scope.gridPriceBand.exporterCsvFilename = 'Market Reports Sales Summary by Price Band - Ontario - Last Completed Period: ' + $scope.selectedperiodkey.label + '.csv'

            if ($scope.setnamesmodel.length > 0) {
                $http({
                    url: "/UnitSizeMLs",
                    dataType: 'json',
                    method: 'POST',
                    data: '',
                    headers: {
                        "Content-Type": "application/json",
                        "X-PeriodKey": strPeriodKey,
                        "X-SetNames": strSetNames
                    }
                }).
                    then(
                    function (result) {
                        $scope.unitsizesdata = result.data;
                        $scope.unitsizesmodel = JSON.parse(JSON.stringify($scope.unitsizesdata));
                    },
                    function (error) {
                        InternalErrorHandler(error);
                    })
            } else {
                $scope.unitsizesdata = [];
                $scope.unitsizesmodel = [];
            }
        }
    };

    //load color subgrid
    $scope.salesbycolorsubgrid = function (colorname) {
        var period = $scope.tmpPeriod;
        var setnames = $scope.tmpSetNames;
        var unitsizes = $scope.tmpUnitSizes;
        var pricefrom = $scope.tmpPriceFrom;
        var priceto = $scope.tmpPriceTo;
        //console.log(colorname);
        //re-assign to global factory
        var selectedgroupid = $scope.selectedgroup.id;

        //header option loading
        var headeroption = parseInt(document.getElementById('HeaderOption').value);
        //if (headeroption === 2) {
        //    var url = document.getElementById('SalesByBrandsUrl').value + "/" + period + "/" + agentid + "/" + agentname + "/" + setnames + "/" + unitsizes + "/" + pricefrom + "/" + priceto + "/" + selectedgroupid + "/" + headeroption;
        //} else {
        //    var url = document.getElementById('SalesByBrandsUrl').value + "/" + period + "/" + agentid + "/" + agentname + "/" + setnames + "/" + unitsizes + "/" + pricefrom + "/" + priceto + "/" + selectedgroupid;
        //};

        //create a dynamic form
        var f = document.createElement("form");
        f.setAttribute('id', "salesbycolorsubgridform");
        f.setAttribute('method', "post");
        f.setAttribute('action', document.getElementById('SalesSummaryColorCountryUrl').value);
        //target blank will post it to a new tab
        f.setAttribute("target", "_blank");
        //append the form to the bottom of the body
        document.body.appendChild(f);
        //create hidden elements and append them to the form
        f.appendChild(GroupDropDown.createFormElement("period", period));
        f.appendChild(GroupDropDown.createFormElement("colorname", colorname));
        f.appendChild(GroupDropDown.createFormElement("setnames", setnames));
        f.appendChild(GroupDropDown.createFormElement("unitsizes", unitsizes));
        f.appendChild(GroupDropDown.createFormElement("pricefrom", pricefrom));
        f.appendChild(GroupDropDown.createFormElement("priceto", priceto));
        f.appendChild(GroupDropDown.createFormElement("selectedgroupid", selectedgroupid));
        if (headeroption === 2) {
            f.appendChild(GroupDropDown.createFormElement("headeroption", headeroption));
        }
        //submit form
        f.submit();
        //remove the newly created form after submit
        f.remove();
    };

    //load country subgrid
    $scope.salesbycountrysubgrid = function (countryname) {
        var period = $scope.tmpPeriod;
        var setnames = $scope.tmpSetNames;
        var unitsizes = $scope.tmpUnitSizes;
        var pricefrom = $scope.tmpPriceFrom;
        var priceto = $scope.tmpPriceTo;
        //console.log(colorname);
        //re-assign to global factory
        var selectedgroupid = $scope.selectedgroup.id;

        //header option loading
        var headeroption = parseInt(document.getElementById('HeaderOption').value);
        //if (headeroption === 2) {
        //    var url = document.getElementById('SalesByBrandsUrl').value + "/" + period + "/" + agentid + "/" + agentname + "/" + setnames + "/" + unitsizes + "/" + pricefrom + "/" + priceto + "/" + selectedgroupid + "/" + headeroption;
        //} else {
        //    var url = document.getElementById('SalesByBrandsUrl').value + "/" + period + "/" + agentid + "/" + agentname + "/" + setnames + "/" + unitsizes + "/" + pricefrom + "/" + priceto + "/" + selectedgroupid;
        //};

        //create a dynamic form
        var f = document.createElement("form");
        f.setAttribute('id', "salesbycountrysubgridform");
        f.setAttribute('method', "post");
        f.setAttribute('action', document.getElementById('SalesSummaryCountryColorUrl').value);
        //target blank will post it to a new tab
        f.setAttribute("target", "_blank");
        //append the form to the bottom of the body
        document.body.appendChild(f);
        //create hidden elements and append them to the form
        f.appendChild(GroupDropDown.createFormElement("period", period));
        f.appendChild(GroupDropDown.createFormElement("countryname", countryname));
        f.appendChild(GroupDropDown.createFormElement("setnames", setnames));
        f.appendChild(GroupDropDown.createFormElement("unitsizes", unitsizes));
        f.appendChild(GroupDropDown.createFormElement("pricefrom", pricefrom));
        f.appendChild(GroupDropDown.createFormElement("priceto", priceto));
        f.appendChild(GroupDropDown.createFormElement("selectedgroupid", selectedgroupid));
        if (headeroption === 2) {
            f.appendChild(GroupDropDown.createFormElement("headeroption", headeroption));
        }
        //submit form
        f.submit();
        //remove the newly created form after submit
        f.remove();
    };

    //load varietal subgrid
    $scope.salesbyvarietalsubgrid = function (varietalname) {
        var period = $scope.tmpPeriod;
        var setnames = $scope.tmpSetNames;
        var unitsizes = $scope.tmpUnitSizes;
        var pricefrom = $scope.tmpPriceFrom;
        var priceto = $scope.tmpPriceTo;
        //console.log(colorname);
        //re-assign to global factory
        var selectedgroupid = $scope.selectedgroup.id;

        //header option loading
        var headeroption = parseInt(document.getElementById('HeaderOption').value);
        //if (headeroption === 2) {
        //    var url = document.getElementById('SalesByBrandsUrl').value + "/" + period + "/" + agentid + "/" + agentname + "/" + setnames + "/" + unitsizes + "/" + pricefrom + "/" + priceto + "/" + selectedgroupid + "/" + headeroption;
        //} else {
        //    var url = document.getElementById('SalesByBrandsUrl').value + "/" + period + "/" + agentid + "/" + agentname + "/" + setnames + "/" + unitsizes + "/" + pricefrom + "/" + priceto + "/" + selectedgroupid;
        //};

        //create a dynamic form
        var f = document.createElement("form");
        f.setAttribute('id', "salesbyvarietalsubgridform");
        f.setAttribute('method', "post");
        f.setAttribute('action', document.getElementById('SalesSummaryVarietalProductUrl').value);
        //target blank will post it to a new tab
        f.setAttribute("target", "_blank");
        //append the form to the bottom of the body
        document.body.appendChild(f);
        //create hidden elements and append them to the form
        f.appendChild(GroupDropDown.createFormElement("period", period));
        f.appendChild(GroupDropDown.createFormElement("varietalname", varietalname));
        f.appendChild(GroupDropDown.createFormElement("setnames", setnames));
        f.appendChild(GroupDropDown.createFormElement("unitsizes", unitsizes));
        f.appendChild(GroupDropDown.createFormElement("pricefrom", pricefrom));
        f.appendChild(GroupDropDown.createFormElement("priceto", priceto));
        f.appendChild(GroupDropDown.createFormElement("selectedgroupid", selectedgroupid));
        if (headeroption === 2) {
            f.appendChild(GroupDropDown.createFormElement("headeroption", headeroption));
        }
        //submit form
        f.submit();
        //remove the newly created form after submit
        f.remove();
    };

    //load my category subgrid
    $scope.salesbymycategorysubgrid = function (mycategoryname) {
        var period = $scope.tmpPeriod;
        var setnames = $scope.tmpSetNames;
        var unitsizes = $scope.tmpUnitSizes;
        var pricefrom = $scope.tmpPriceFrom;
        var priceto = $scope.tmpPriceTo;
        //console.log(colorname);
        //re-assign to global factory
        var selectedgroupid = $scope.selectedgroup.id;

        //header option loading
        var headeroption = parseInt(document.getElementById('HeaderOption').value);
        //if (headeroption === 2) {
        //    var url = document.getElementById('SalesByBrandsUrl').value + "/" + period + "/" + agentid + "/" + agentname + "/" + setnames + "/" + unitsizes + "/" + pricefrom + "/" + priceto + "/" + selectedgroupid + "/" + headeroption;
        //} else {
        //    var url = document.getElementById('SalesByBrandsUrl').value + "/" + period + "/" + agentid + "/" + agentname + "/" + setnames + "/" + unitsizes + "/" + pricefrom + "/" + priceto + "/" + selectedgroupid;
        //};

        //create a dynamic form
        var f = document.createElement("form");
        f.setAttribute('id', "salesbymycategorysubgridform");
        f.setAttribute('method', "post");
        f.setAttribute('action', document.getElementById('SalesSummaryMyCategoryCountryUrl').value);
        //target blank will post it to a new tab
        f.setAttribute("target", "_blank");
        //append the form to the bottom of the body
        document.body.appendChild(f);
        //create hidden elements and append them to the form
        f.appendChild(GroupDropDown.createFormElement("period", period));
        f.appendChild(GroupDropDown.createFormElement("mycategoryname", mycategoryname));
        f.appendChild(GroupDropDown.createFormElement("setnames", setnames));
        f.appendChild(GroupDropDown.createFormElement("unitsizes", unitsizes));
        f.appendChild(GroupDropDown.createFormElement("pricefrom", pricefrom));
        f.appendChild(GroupDropDown.createFormElement("priceto", priceto));
        f.appendChild(GroupDropDown.createFormElement("selectedgroupid", selectedgroupid));
        if (headeroption === 2) {
            f.appendChild(GroupDropDown.createFormElement("headeroption", headeroption));
        }
        //submit form
        f.submit();
        //remove the newly created form after submit
        f.remove();
    };

    //load priceband subgrid
    $scope.salesbypricebandsubgrid = function (pricebandname) {
        var period = $scope.tmpPeriod;
        var setnames = $scope.tmpSetNames;
        var unitsizes = $scope.tmpUnitSizes;
        var pricefrom = $scope.tmpPriceFrom;
        var priceto = $scope.tmpPriceTo;
        //console.log(colorname);
        //re-assign to global factory
        var selectedgroupid = $scope.selectedgroup.id;

        //header option loading
        var headeroption = parseInt(document.getElementById('HeaderOption').value);
        //if (headeroption === 2) {
        //    var url = document.getElementById('SalesByBrandsUrl').value + "/" + period + "/" + agentid + "/" + agentname + "/" + setnames + "/" + unitsizes + "/" + pricefrom + "/" + priceto + "/" + selectedgroupid + "/" + headeroption;
        //} else {
        //    var url = document.getElementById('SalesByBrandsUrl').value + "/" + period + "/" + agentid + "/" + agentname + "/" + setnames + "/" + unitsizes + "/" + pricefrom + "/" + priceto + "/" + selectedgroupid;
        //};

        //create a dynamic form
        var f = document.createElement("form");
        f.setAttribute('id', "salesbypricebandsubgridform");
        f.setAttribute('method', "post");
        f.setAttribute('action', document.getElementById('SalesSummaryPriceBandCategoryUrl').value);
        //target blank will post it to a new tab
        f.setAttribute("target", "_blank");
        //append the form to the bottom of the body
        document.body.appendChild(f);
        //create hidden elements and append them to the form
        f.appendChild(GroupDropDown.createFormElement("period", period));
        f.appendChild(GroupDropDown.createFormElement("pricebandname", pricebandname));
        f.appendChild(GroupDropDown.createFormElement("setnames", setnames));
        f.appendChild(GroupDropDown.createFormElement("unitsizes", unitsizes));
        f.appendChild(GroupDropDown.createFormElement("pricefrom", pricefrom));
        f.appendChild(GroupDropDown.createFormElement("priceto", priceto));
        f.appendChild(GroupDropDown.createFormElement("selectedgroupid", selectedgroupid));
        if (headeroption === 2) {
            f.appendChild(GroupDropDown.createFormElement("headeroption", headeroption));
        }
        //submit form
        f.submit();
        //remove the newly created form after submit
        f.remove();
    };

}