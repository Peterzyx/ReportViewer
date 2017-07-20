//SalesSummaryByTerritory
app.controller('salesSummaryCtrl', SalesSummaryCtrl);

SalesSummaryCtrl.$inject = ['$http', '$scope', 'GroupDropDown', 'uiGridConstants','GroupService'];
function SalesSummaryCtrl($http, $scope, GroupDropDown, uiGridConstants, GroupService) {
    //UI Grid Setting
    $scope.gridOptions3 = {
        columnDefs: [
            { name: 'IsTotal', field: "isTotal", width: 50, cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'Terr No', field: "TerrNo", width: 100, pinnedLeft: true, visible: false },
            { name: 'Sales Rep Name', field: "SalesRepName", width: 200, cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;">{{COL_FIELD}}</div>', footerCellTemplate: '<div class="ui-grid-cell-contents">Total:</div>' },

            //Store
            { name: 'All 9L Cases 13P LY', field: "All_LY_13P_9LCases", width: 200, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'All 9L Cases 13P TY', field: "All_TY_13P_9LCases", width: 200, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: '9LEquivalentCases_13P_StorePct', field: "All_13P_9LCasesPct", displayName: "Var %", cellFilter: "mapPercentage", width: 100, visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="9LCasesGrid3"></span></div>' },
            { name: 'All Sales 13P LY', field: "All_LY_13P_SalesAmount", width: 200, cellFilter: 'currency:"$":0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'All Sales 13P TY', field: "All_TY_13P_SalesAmount", width: 200, cellFilter: 'currency:"$":0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'SalesAmount_13P_StorePct', field: "All_13P_SalesAmountPct", displayName: "Var %", cellFilter: "mapPercentage", width: 100, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="SalesAmtGrid3"></span></div>' },
            { name: 'All Units 13P LY', field: "All_LY_13P_Units", width: 200, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'All Units 13P TY', field: "All_TY_13P_Units", width: 200, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'Units_13P_StorePct', field: "All_13P_UnitsPct", displayName: "Var %", cellFilter: "mapPercentage", width: 100, visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="UnitGrid3"></span></div>' }

        ],
        enableSorting: true,
        enableRowSelection: true,
        enableRowHeaderSelection: true,
        enableCellSelection: false,
        enableCellEditOnFocus: false,
        enableGridMenu: true,
        exporterMenuPdf: false,
        exporterCsvFilename: 'Store Sales Summary.csv',
        exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
        showColumnFooter: true
    };


    $scope.gridOptions1 = {
        columnDefs: [
            { name: 'IsTotal', field: "isTotal", width: 50, cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'Terr No', field: "TerrNo", width: 100, pinnedLeft: true, visible: false },
            { name: 'Sales Rep Name', field: "SalesRepName", width: 200, cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;"><a ng-click="grid.appScope.salesteamsubgrid(row.entity.TerrNo, row.entity.SalesRepName, 1)" style="cursor:pointer;">{{COL_FIELD}}</a></div>', footerCellTemplate: '<div class="ui-grid-cell-contents">Total:</div>' },

            //Store
            { name: 'Store 9L Cases 13P LY', field: "Store_LY_13P_9LCases", width: 200, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'Store 9L Cases 13P TY', field: "Store_TY_13P_9LCases", width: 200, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: '9LEquivalentCases_13P_StorePct', field: "Store_13P_9LCasesPct", displayName: "Var %", cellFilter: "mapPercentage", width: 100, visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="9LCasesGrid1"></span></div>' },
            { name: 'Store Sales 13P LY', field: "Store_LY_13P_SalesAmount", width: 200, cellFilter: 'currency:"$":0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'Store Sales 13P TY', field: "Store_TY_13P_SalesAmount", width: 200, cellFilter: 'currency:"$":0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'SalesAmount_13P_StorePct', field: "Store_13P_SalesAmountPct", displayName: "Var %", cellFilter: "mapPercentage", width: 100, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="SalesAmtGrid1"></span></div>' },
            { name: 'Store Units 13P LY', field: "Store_LY_13P_Units", width: 200, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'Store Units 13P TY', field: "Store_TY_13P_Units", width: 200, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'Units_13P_StorePct', field: "Store_13P_UnitsPct", displayName: "Var %", cellFilter: "mapPercentage", width: 100, visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="UnitGrid1"></span></div>' }

        ],
        enableSorting: true,
        enableRowSelection: true,
        enableRowHeaderSelection: true,
        enableCellSelection: false,
        enableCellEditOnFocus: false,
        enableGridMenu: true,
        exporterMenuPdf: false,
        exporterCsvFilename: 'Store Sales Summary.csv',
        exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
        showColumnFooter: true
    };

    $scope.gridOptions2 = {
        columnDefs: [
            { name: 'IsTotal', field: "isTotal", width: 50, cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'Terr No', field: "TerrNo", width: 100, pinnedLeft: true, visible: false },
            { name: 'Sales Rep Name', field: "SalesRepName", width: 200, cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;"><a ng-click="grid.appScope.salesteamsubgrid(row.entity.TerrNo, row.entity.SalesRepName, 2)" style="cursor:pointer;">{{COL_FIELD}}</a></div>', footerCellTemplate: '<div class="ui-grid-cell-contents">Total:</div>' },

            //Licensee
            { name: 'Lic 9L Cases 13P LY', field: "Lic_LY_13P_9LCases", width: 200, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'Lic 9L Cases 13P TY', field: "Lic_TY_13P_9LCases", width: 200, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: '9LEquivalentCases_13P_LicPct', field: "Lic_13P_9LCasesPct", displayName: "Var %", cellFilter: "mapPercentage", width: 100, visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="9LCasesGrid2"></span></div>' },
            { name: 'Lic Sales 13P LY', field: "Lic_LY_13P_SalesAmount", width: 200, cellFilter: 'currency:"$":0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'Lic Sales 13P TY', field: "Lic_TY_13P_SalesAmount", width: 200, cellFilter: 'currency:"$":0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'SalesAmount_13P_LicPct', field: "Lic_13P_SalesAmountPct", displayName: "Var %", cellFilter: "mapPercentage", width: 100, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="SalesAmtGrid2"></span></div>' },
            { name: 'Lic Units 13P LY', field: "Lic_LY_13P_Units", width: 200, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'Lic Units 13P TY', field: "Lic_TY_13P_Units", width: 200, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'Lic Units_13P_StorePct', field: "Lic_13P_UnitsPct", displayName: "Var %", cellFilter: "mapPercentage", width: 100, visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="UnitGrid2"></span></div>' }

        ],
        enableSorting: true,
        enableRowSelection: true,
        enableRowHeaderSelection: true,
        enableCellSelection: false,
        enableCellEditOnFocus: false,
        enableGridMenu: true,
        exporterMenuPdf: false,
        exporterCsvFilename: 'Licensee Sales Summary.csv',
        exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
        showColumnFooter: true
    };

    $scope.gridOptions1.onRegisterApi = function (gridApi) {
        $scope.gridApi1 = gridApi;
    };

    $scope.gridOptions2.onRegisterApi = function (gridApi) {
        $scope.gridApi2 = gridApi;
    };

    $scope.gridOptions3.onRegisterApi = function (gridApi) {
        $scope.gridApi3 = gridApi;
    };
    //dropdown
    $scope.periodkeydropboxitemselected = function (item) {
        $scope.selectedperiodkey = item;
    }

    $scope.groups = GroupDropDown.groups;
    $scope.selectedgroup = (GroupService.getCatGroup($scope.currentUserInfo) != '' && GroupService.getCatGroup($scope.currentUserInfo) != null) ? GroupService.getCatGroup($scope.currentUserInfo) : GroupDropDown.groups[1];

    $scope.groupdropboxitemselected = function (item) {
        GroupService.setCatGroup($scope.currentUserInfo, item)
        $scope.selectedgroup = item;
    }

    init();

    //initial loading when rendering the page
    function init() {
        //loading
        $scope.loading1 = true;
        $scope.loading2 = true;
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
                $scope.gridOptions1.exporterCsvFilename = 'Store Sales Summary - Last Completed Period: ' + periodkey + '.csv';
                $scope.gridOptions2.exporterCsvFilename = 'Licensee Sales Summary - Last Completed Period: ' + periodkey + '.csv';
                $scope.gridOptions3.exporterCsvFilename = 'All Sales Summary - Last Completed Period: ' + periodkey + '.csv';

                //Grid1
                $http({
                    url: "/SalesTeamSummaryStore",
                    dataType: 'json',
                    method: 'GET',
                    data: '',
                    headers: {
                        "Content-Type": "application/json",
                        "X-PeriodKey": periodkey
                    }
                }).
                    then(
                    function (result) {
                        var len = result.data.length;
                        $scope.gridOptions1.data = result.data.slice(1, len);
                        //assign footer pct
                        var nineLitreElem = document.getElementById("9LCasesGrid1");
                        if (nineLitreElem)
                            nineLitreElem.textContent = (result.data[0].Store_13P_9LCasesPct * 100).toFixed(2).toString() + "%";
                        var salesAmtElem = document.getElementById("SalesAmtGrid1");
                        if (salesAmtElem)
                            salesAmtElem.textContent = (result.data[0].Store_13P_SalesAmountPct * 100).toFixed(2).toString() + "%";
                        var unitElem = document.getElementById("UnitGrid1");
                        if (unitElem)
                            unitElem.textContent = (result.data[0].Store_13P_UnitsPct * 100).toFixed(2).toString() + "%";
                    },
                    function (error) {
                        InternalErrorHandler(error);
                        $scope.loading1 = false;
                        if (!$scope.loading2) {
                            mybody.removeClass('waiting');
                        }
                    }).finally(function () {
                        $scope.loading1 = false;
                        if (!$scope.loading2) {
                            mybody.removeClass('waiting');
                        }
                    });

                //Grid2
                $http({
                    url: "/SalesTeamSummaryLicensee",
                    dataType: 'json',
                    method: 'GET',
                    data: '',
                    headers: {
                        "Content-Type": "application/json",
                        "X-PeriodKey": periodkey
                    }
                }).
                    then(
                    function (result) {
                        var len = result.data.length;
                        $scope.gridOptions2.data = result.data.slice(1, len);
                        //assign footer pct
                        var nineLitreElem = document.getElementById("9LCasesGrid2");
                        if (nineLitreElem)
                            nineLitreElem.textContent = (result.data[0].Lic_13P_9LCasesPct * 100).toFixed(2).toString() + "%";
                        var salesAmtElem = document.getElementById("SalesAmtGrid2");
                        if (salesAmtElem)
                            salesAmtElem.textContent = (result.data[0].Lic_13P_SalesAmountPct * 100).toFixed(2).toString() + "%";
                        var unitElem = document.getElementById("UnitGrid2");
                        if (unitElem)
                            unitElem.textContent = (result.data[0].Lic_13P_UnitsPct * 100).toFixed(2).toString() + "%";
                    },
                    function (error) {
                        InternalErrorHandler(error);
                        $scope.loading2 = false;
                        if (!$scope.loading1) {
                            mybody.removeClass('waiting');
                        }
                    }).finally(function () {
                        $scope.loading2 = false;
                        if (!$scope.loading1) {
                            mybody.removeClass('waiting');
                        }
                    });

                //Grid3
                $http({
                    url: "/SalesTeamSummaryAll",
                    dataType: 'json',
                    method: 'GET',
                    data: '',
                    headers: {
                        "Content-Type": "application/json",
                        "X-PeriodKey": periodkey
                    }
                }).
                    then(
                    function (result) {
                        var len = result.data.length;
                        $scope.gridOptions3.data = result.data.slice(1, len);
                        //assign footer pct
                        var nineLitreElem = document.getElementById("9LCasesGrid3");
                        if (nineLitreElem)
                            nineLitreElem.textContent = (result.data[0].All_13P_9LCasesPct * 100).toFixed(2).toString() + "%";
                        var salesAmtElem = document.getElementById("SalesAmtGrid3");
                        if (salesAmtElem)
                            salesAmtElem.textContent = (result.data[0].All_13P_SalesAmountPct * 100).toFixed(2).toString() + "%";
                        var unitElem = document.getElementById("UnitGrid3");
                        if (unitElem)
                            unitElem.textContent = (result.data[0].All_13P_UnitsPct * 100).toFixed(2).toString() + "%";
                    },
                    function (error) {
                        InternalErrorHandler(error);
                        $scope.loading3 = false;
                        if (!$scope.loading1) {
                            mybody.removeClass('waiting');
                        }
                    }).finally(function () {
                        $scope.loading3 = false;
                        if (!$scope.loading1) {
                            mybody.removeClass('waiting');
                        }
                    });
            },
            function (perioderror) {
                InternalErrorHandler(perioderror);
            });
    }

    $scope.Reload = function () {
        RefreshClick();
    }

    //refresh visibility
    function RefreshClick() {

        var strPeriod = $scope.selectedperiodkey.label;

        //loading
        $scope.loading1 = true;
        $scope.loading2 = true;
        var mybody = angular.element(document).find('body');
        mybody.addClass('waiting');

        //update ColDefs
        if ($scope.selectedgroup.id === 0) {

            //Grid1
            $scope.gridOptions1.columnDefs[3].visible = true;
            $scope.gridOptions1.columnDefs[4].visible = true;
            $scope.gridOptions1.columnDefs[5].visible = true;
            $scope.gridOptions1.columnDefs[6].visible = false;
            $scope.gridOptions1.columnDefs[7].visible = false;
            $scope.gridOptions1.columnDefs[8].visible = false;
            $scope.gridOptions1.columnDefs[9].visible = false;
            $scope.gridOptions1.columnDefs[10].visible = false;
            $scope.gridOptions1.columnDefs[11].visible = false;

            //Grid2
            $scope.gridOptions2.columnDefs[3].visible = true;
            $scope.gridOptions2.columnDefs[4].visible = true;
            $scope.gridOptions2.columnDefs[5].visible = true;
            $scope.gridOptions2.columnDefs[6].visible = false;
            $scope.gridOptions2.columnDefs[7].visible = false;
            $scope.gridOptions2.columnDefs[8].visible = false;
            $scope.gridOptions2.columnDefs[9].visible = false;
            $scope.gridOptions2.columnDefs[10].visible = false;
            $scope.gridOptions2.columnDefs[11].visible = false;

            //Grid3
            $scope.gridOptions3.columnDefs[3].visible = true;
            $scope.gridOptions3.columnDefs[4].visible = true;
            $scope.gridOptions3.columnDefs[5].visible = true;
            $scope.gridOptions3.columnDefs[6].visible = false;
            $scope.gridOptions3.columnDefs[7].visible = false;
            $scope.gridOptions3.columnDefs[8].visible = false;
            $scope.gridOptions3.columnDefs[9].visible = false;
            $scope.gridOptions3.columnDefs[10].visible = false;
            $scope.gridOptions3.columnDefs[11].visible = false;

        } else if ($scope.selectedgroup.id === 1) {

            //Grid1
            $scope.gridOptions1.columnDefs[3].visible = false;
            $scope.gridOptions1.columnDefs[4].visible = false;
            $scope.gridOptions1.columnDefs[5].visible = false;
            $scope.gridOptions1.columnDefs[6].visible = true;
            $scope.gridOptions1.columnDefs[7].visible = true;
            $scope.gridOptions1.columnDefs[8].visible = true;
            $scope.gridOptions1.columnDefs[9].visible = false;
            $scope.gridOptions1.columnDefs[10].visible = false;
            $scope.gridOptions1.columnDefs[11].visible = false;

            //Grid2
            $scope.gridOptions2.columnDefs[3].visible = false;
            $scope.gridOptions2.columnDefs[4].visible = false;
            $scope.gridOptions2.columnDefs[5].visible = false;
            $scope.gridOptions2.columnDefs[6].visible = true;
            $scope.gridOptions2.columnDefs[7].visible = true;
            $scope.gridOptions2.columnDefs[8].visible = true;
            $scope.gridOptions2.columnDefs[9].visible = false;
            $scope.gridOptions2.columnDefs[10].visible = false;
            $scope.gridOptions2.columnDefs[11].visible = false;

            //Grid3
            $scope.gridOptions3.columnDefs[3].visible = false;
            $scope.gridOptions3.columnDefs[4].visible = false;
            $scope.gridOptions3.columnDefs[5].visible = false;
            $scope.gridOptions3.columnDefs[6].visible = true;
            $scope.gridOptions3.columnDefs[7].visible = true;
            $scope.gridOptions3.columnDefs[8].visible = true;
            $scope.gridOptions3.columnDefs[9].visible = false;
            $scope.gridOptions3.columnDefs[10].visible = false;
            $scope.gridOptions3.columnDefs[11].visible = false;

        } else if ($scope.selectedgroup.id === 2) {

            //Grid1
            $scope.gridOptions1.columnDefs[3].visible = false;
            $scope.gridOptions1.columnDefs[4].visible = false;
            $scope.gridOptions1.columnDefs[5].visible = false;
            $scope.gridOptions1.columnDefs[6].visible = false;
            $scope.gridOptions1.columnDefs[7].visible = false;
            $scope.gridOptions1.columnDefs[8].visible = false;
            $scope.gridOptions1.columnDefs[9].visible = true;
            $scope.gridOptions1.columnDefs[10].visible = true;
            $scope.gridOptions1.columnDefs[11].visible = true;

            //Grid2
            $scope.gridOptions2.columnDefs[3].visible = false;
            $scope.gridOptions2.columnDefs[4].visible = false;
            $scope.gridOptions2.columnDefs[5].visible = false;
            $scope.gridOptions2.columnDefs[6].visible = false;
            $scope.gridOptions2.columnDefs[7].visible = false;
            $scope.gridOptions2.columnDefs[8].visible = false;
            $scope.gridOptions2.columnDefs[9].visible = true;
            $scope.gridOptions2.columnDefs[10].visible = true;
            $scope.gridOptions2.columnDefs[11].visible = true;

            //Grid3
            $scope.gridOptions3.columnDefs[3].visible = false;
            $scope.gridOptions3.columnDefs[4].visible = false;
            $scope.gridOptions3.columnDefs[5].visible = false;
            $scope.gridOptions3.columnDefs[6].visible = false;
            $scope.gridOptions3.columnDefs[7].visible = false;
            $scope.gridOptions3.columnDefs[8].visible = false;
            $scope.gridOptions3.columnDefs[9].visible = true;
            $scope.gridOptions3.columnDefs[10].visible = true;
            $scope.gridOptions3.columnDefs[11].visible = true;
        }
        $scope.gridApi1.grid.refresh();
        $scope.gridApi2.grid.refresh();
        $scope.gridApi3.grid.refresh();

        //Grid1
        $http({
            url: "/SalesTeamSummaryStore",
            dataType: 'json',
            method: 'GET',
            data: '',
            headers: {
                "Content-Type": "application/json",
                "X-PeriodKey": strPeriod
            }
        }).
            then(
            function (result) {
                var len = result.data.length;
                $scope.gridOptions1.data = result.data.slice(1, len);
                //assign footer pct
                var nineLitreElem = document.getElementById("9LCasesGrid1");
                if (nineLitreElem)
                    nineLitreElem.textContent = (result.data[0].Store_13P_9LCasesPct * 100).toFixed(2).toString() + "%";
                var salesAmtElem = document.getElementById("SalesAmtGrid1");
                if (salesAmtElem)
                    salesAmtElem.textContent = (result.data[0].Store_13P_SalesAmountPct * 100).toFixed(2).toString() + "%";
                var unitElem = document.getElementById("UnitGrid1");
                if (unitElem)
                    unitElem.textContent = (result.data[0].Store_13P_UnitsPct * 100).toFixed(2).toString() + "%";
            },
            function (error) {
                InternalErrorHandler(error);
                $scope.loading1 = false;
                if (!$scope.loading2) {
                    mybody.removeClass('waiting');
                }
            }).finally(function () {
                $scope.loading1 = false;
                if (!$scope.loading2) {
                    mybody.removeClass('waiting');
                };
                $scope.gridOptions1.exporterCsvFilename = 'Store Sales Summary - Last Completed Period: ' + strPeriod + '.csv';
            });

        //Grid2
        $http({
            url: "/SalesTeamSummaryLicensee",
            dataType: 'json',
            method: 'GET',
            data: '',
            headers: {
                "Content-Type": "application/json",
                "X-PeriodKey": strPeriod
            }
        }).
            then(
            function (result) {
                var len = result.data.length;
                $scope.gridOptions2.data = result.data.slice(1, len);
                //assign footer pct
                var nineLitreElem = document.getElementById("9LCasesGrid2");
                if (nineLitreElem)
                    nineLitreElem.textContent = (result.data[0].Lic_13P_9LCasesPct * 100).toFixed(2).toString() + "%";
                var salesAmtElem = document.getElementById("SalesAmtGrid2");
                if (salesAmtElem)
                    salesAmtElem.textContent = (result.data[0].Lic_13P_SalesAmountPct * 100).toFixed(2).toString() + "%";
                var unitElem = document.getElementById("UnitGrid2");
                if (unitElem)
                    unitElem.textContent = (result.data[0].Lic_13P_UnitsPct * 100).toFixed(2).toString() + "%";
            },
            function (error) {
                InternalErrorHandler(error);
                $scope.loading2 = false;
                if (!$scope.loading1) {
                    mybody.removeClass('waiting');
                }
            }).finally(function () {
                $scope.loading2 = false;
                if (!$scope.loading1) {
                    mybody.removeClass('waiting');
                };
                $scope.gridOptions2.exporterCsvFilename = 'Licensee Sales Summary - Last Completed Period: ' + strPeriod + '.csv';
            });
        //Grid3
        $http({
            url: "/SalesTeamSummaryAll",
            dataType: 'json',
            method: 'GET',
            data: '',
            headers: {
                "Content-Type": "application/json",
                "X-PeriodKey": strPeriod
            }
        }).
            then(
            function (result) {
                var len = result.data.length;
                $scope.gridOptions3.data = result.data.slice(1, len);
                //assign footer pct
                var nineLitreElem = document.getElementById("9LCasesGrid3");
                if (nineLitreElem)
                    nineLitreElem.textContent = (result.data[0].All_13P_9LCasesPct * 100).toFixed(2).toString() + "%";
                var salesAmtElem = document.getElementById("SalesAmtGrid3");
                if (salesAmtElem)
                    salesAmtElem.textContent = (result.data[0].All_13P_SalesAmountPct * 100).toFixed(2).toString() + "%";
                var unitElem = document.getElementById("UnitGrid3");
                if (unitElem)
                    unitElem.textContent = (result.data[0].All_13P_UnitsPct * 100).toFixed(2).toString() + "%";
            },
            function (error) {
                InternalErrorHandler(error);
                $scope.loading3 = false;
                if (!$scope.loading1) {
                    mybody.removeClass('waiting');
                }
            }).finally(function () {
                $scope.loading3 = false;
                if (!$scope.loading1) {
                    mybody.removeClass('waiting');
                };
                $scope.gridOptions3.exporterCsvFilename = 'All Sales Summary - Last Completed Period: ' + strPeriod + '.csv';
            });
    }

    //load subgrid
    $scope.salesteamsubgrid = function (userid, salesname, indicator) {

        //re-assign to global factory
        var periodkey = $scope.selectedperiodkey.label;
        var selectedgroupid = $scope.selectedgroup.id;

        //header option loading
        var headeroption = parseInt(document.getElementById('HeaderOption').value);

        //create a dynamic form
        var f = document.createElement("form");
        f.setAttribute('id', "salesteamsubgridform");
        f.setAttribute('method', "post");
        f.setAttribute('action', document.getElementById('SalesTeamStoreLicenseeUrl').value);
        //target blank will post it to a new tab
        f.setAttribute("target", "_blank");
        //append the form to the bottom of the body
        document.body.appendChild(f);
        //create hidden elements and append them to the form
        f.appendChild(GroupDropDown.createFormElement("userid", userid));
        f.appendChild(GroupDropDown.createFormElement("salesname", salesname));
        f.appendChild(GroupDropDown.createFormElement("period", periodkey));
        f.appendChild(GroupDropDown.createFormElement("selectedgroupid", selectedgroupid));
        if (indicator === 1) {
            f.appendChild(GroupDropDown.createFormElement("storelicensee", "Store"));
        } else if (indicator === 2) {
            f.appendChild(GroupDropDown.createFormElement("storelicensee", "Licensee"));
        }

        if (headeroption === 2) {
            f.appendChild(GroupDropDown.createFormElement("headeroption", headeroption));
        }
        //submit form
        f.submit();
        //remove the newly created form after submit
        f.remove();
    };
}