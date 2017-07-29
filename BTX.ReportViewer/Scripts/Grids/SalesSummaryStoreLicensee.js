//SalesTeamStoreSummary
app.controller('salesSummaryStoreLicenseeCtrl', SalesSummaryStoreLicenseeCtrl);

SalesSummaryStoreLicenseeCtrl.$inject = ['$http', '$scope', 'GroupDropDown', 'uiGridConstants','GroupService'];
function SalesSummaryStoreLicenseeCtrl($http, $scope, GroupDropDown, uiGridConstants, GroupService) {

    var indicator = parseInt(document.getElementById('StoreLicenseeId').value);

    var periodlist = [];
    periodlist.push({ name: "9L Cases 13P LY", visible: true });
    periodlist.push({ name: "9L Cases 13P TY", visible: true });

    periodlist.push({ name: "Sales 13P LY", visible: true });
    periodlist.push({ name: "Sales 13P TY", visible: true });

    periodlist.push({ name: "Units 13P LY", visible: true });
    periodlist.push({ name: "Units 13P TY", visible: true });
  

    periodlist.push({ name: "9L Cases 3P LY", visible: true });
    periodlist.push({ name: "9L Cases 3P TY", visible: true });

    periodlist.push({ name: "Sales 3P LY", visible: true });
    periodlist.push({ name: "Sales 3P TY", visible: true });

    periodlist.push({ name: "Units 3P LY", visible: true });
    periodlist.push({ name: "Units 3P TY", visible: true });


    if (indicator === 1) {
        $scope.gridOptions = {
            columnDefs: [
                { name: 'IsTotal', field: "IsTotal", width: 50, cellFilter: 'number:0', visible: false, type: 'number' },
                { name: 'Store No', field: "AccountNumber", width: 100, pinnedLeft: true, cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><a ng-click="grid.appScope.salespersonalstore(row.entity.AccountNumber, row.entity.AccountName)" style="cursor:pointer;">{{COL_FIELD}}</a></div>', type: 'number' },
                { name: 'Store Name', field: "AccountName", width: 400, cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;"><a ng-click="grid.appScope.salespersonalstore(row.entity.AccountNumber, row.entity.AccountName)" style="cursor:pointer;">{{COL_FIELD}}</a></div>', footerCellTemplate: '<div class="ui-grid-cell-contents">Total:</div>' },
                { name: 'Address', field: "Address", width: 200, cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;">{{COL_FIELD}}</div>' },
                { name: 'City', field: "City", width: 150, cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;">{{COL_FIELD}}</div>' },
                { name: 'Rank_9LEquivalentCases', field: "Rank_9LEquivalentCases", displayName: "Rank", width: 50, cellFilter: 'number:0', visible: false, type: 'number' },
                { name: 'Rank_SalesAmount', field: "Rank_SalesAmount", displayName: "Rank", width: 50, cellFilter: 'number:0', type: 'number' },
                { name: 'Rank_Units', field: "Rank_Units", displayName: "Rank", width: 50, cellFilter: 'number:0', visible: false, type: 'number' },

                //Store 13P
                { name: 'Store 9L Cases 13P LY', field: "Store_LY_13P_9LCases", width: 200, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
                { name: 'Store 9L Cases 13P TY', field: "Store_TY_13P_9LCases", width: 200, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
                { name: 'Store_13P_9LCasesPct', field: "Store_13P_9LCasesPct", displayName: "Var %", cellFilter: "mapPercentage", width: 100, visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Store_13P_9LCasesPct"></span></div>' },
                { name: 'Store Sales 13P LY', field: "Store_LY_13P_SalesAmount", width: 200, cellFilter: 'currency:"$":0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
                { name: 'Store Sales 13P TY', field: "Store_TY_13P_SalesAmount", width: 200, cellFilter: 'currency:"$":0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
                { name: 'SalesAmount_13P_StorePct', field: "Store_13P_SalesAmountPct", displayName: "Var %", cellFilter: "mapPercentage", width: 100, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Store_13P_SalesAmountPct"></span></div>' },
                { name: 'Store Units 13P LY', field: "Store_LY_13P_Units", width: 200, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
                { name: 'Store Units 13P TY', field: "Store_TY_13P_Units", width: 200, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
                { name: 'Units_13P_StorePct', field: "Store_13P_UnitsPct", displayName: "Var %", cellFilter: "mapPercentage", width: 100, visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Store_13P_UnitsPct"></span></div>' },

                //Store 3P
                { name: 'Store 9L Cases 3P LY', field: "Store_LY_3P_9LCases", width: 200, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
                { name: 'Store 9L Cases 3P TY', field: "Store_TY_3P_9LCases", width: 200, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
                { name: 'Store_3P_9LCasesPct', field: "Store_3P_9LCasesPct", displayName: "Var %", cellFilter: "mapPercentage", width: 100, visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Store_3P_9LCasesPct"></span></div>' },
                { name: 'Store Sales 3P LY', field: "Store_LY_3P_SalesAmount", width: 200, cellFilter: 'currency:"$":0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
                { name: 'Store Sales 3P TY', field: "Store_TY_3P_SalesAmount", width: 200, cellFilter: 'currency:"$":0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
                { name: 'SalesAmount_3P_StorePct', field: "Store_3P_SalesAmountPct", displayName: "Var %", cellFilter: "mapPercentage", width: 100, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Store_3P_SalesAmountPct"></span></div>' },
                { name: 'Store Units 3P LY', field: "Store_LY_3P_Units", width: 200, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
                { name: 'Store Units 3P TY', field: "Store_TY_3P_Units", width: 200, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
                { name: 'Units_3P_StorePct', field: "Store_3P_UnitsPct", displayName: "Var %", cellFilter: "mapPercentage", width: 100, visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Store_3P_UnitsPct"></span></div>' }

            ],
            enableSorting: true,
            enableRowSelection: true,
            enableRowHeaderSelection: true,
            enableCellSelection: false,
            enableCellEditOnFocus: false,
            enableGridMenu: true,
            exporterMenuPdf: false,
            exporterCsvFilename: 'Store Sales Territory by ' + document.getElementById('SalesRepName').value + " - Last Completed Period: " + document.getElementById('SalesPeriod').value + '.csv',
            exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
            showColumnFooter: true
        }
    } else {
        $scope.gridOptions = {
            headerTemplate:
            '<div role="rowgroup" class="ui-grid-header">'
            + '    <div class="ui-grid-top-panel">'
            + '        <div class="ui-grid-header-viewport">'
            + '            <div class="ui-grid-header-canvas">'
            + '                <div class="ui-grid-header-cell-wrapper" ng-style="colContainer.headerCellWrapperStyle()">'
            + '                    <div role="row" class="ui-grid-header-cell-row">'
            + '                        <div class="ui-grid-header-cell ui-grid-clearfix ui-grid-category" '
            + '                             ng-repeat="cat in grid.options.category" '
            + '                             ng-if="cat.visible && (colContainer.renderedColumns | filter:{colDef:{category: cat.name} }).length > 0">'
            + '                            {{ cat.name }}'
            + '                            <div class="ui-grid-header-cell ui-grid-clearfix {{cat.name}}" ng-class="{hidden: cat.name === \'\'}" style="width: 100px"'
            + '                                 ng-repeat="col in colContainer.renderedColumns | filter:{ colDef:{category: cat.name} }" '
            + '                                 ui-grid-header-cell '
            + '                                 col="col" '
            + '                                 render-index="$index">'
            + '                            </div>'
            + '                        </div>'
            + '                        <div class="ui-grid-header-cell ui-grid-clearfix" '
            + '                             ng-if="col.colDef.category === undefined" '
            + '                             ng-repeat="col in colContainer.renderedColumns track by col.uid" '
            + '                             ui-grid-header-cell '
            + '                             col="col" '
            + '                             render-index="$index">'
            + '                        </div>'
            + '                    </div>'
            + '                </div>'
            + '            </div>'
            + '        </div>'
            + '    </div>'
            + '</div>',
            category: periodlist,
            columnDefs: [
                { name: 'IsTotal', field: "IsTotal", width: 50, cellFilter: 'number:0', visible: false, type: 'number', pinnedLeft: true },
                { name: 'Licensee No', field: "AccountNumber", width: 100, pinnedLeft: true, cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><a ng-click="grid.appScope.salespersonallicensee(row.entity.AccountNumber, row.entity.AccountName)" style="cursor:pointer;">{{COL_FIELD}}</a></div>', type: 'number', pinnedLeft: true },
                { name: 'Licensee Name', field: "AccountName", width: 250, cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;"><a ng-click="grid.appScope.salespersonallicensee(row.entity.AccountNumber, row.entity.AccountName)" style="cursor:pointer;">{{COL_FIELD}}</a></div>', footerCellTemplate: '<div class="ui-grid-cell-contents">Total:</div>', pinnedLeft: true },
                { name: 'Address', field: "Address", width: 200, cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;">{{COL_FIELD}}</div>', pinnedLeft: true },
                { name: 'City', field: "City", width: 150, cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;">{{COL_FIELD}}</div>', pinnedLeft: true },

                { name: 'Tot_Rank_9LEquivalentCases', field: "Tot_Rank_9LEquivalentCases", displayName: 'Rank', width: 50, cellFilter: 'number:0', visible: false, type: 'number', pinnedLeft: true },
                { name: 'Tot_Rank_SalesAmount', field: "Tot_Rank_SalesAmount", displayName: 'Rank', width: 50, cellFilter: 'number:0', type: 'number', pinnedLeft: true },
                { name: 'Tot_Rank_Units', field: "Tot_Rank_Units", displayName: 'Rank', width: 50, cellFilter: 'number:0', visible: false, type: 'number', pinnedLeft: true },

                //Total 13P
                { name: 'Licensee 9L Cases 13P LY', field: "Lic_LY_13P_9LCases", displayName: "LCBO", width: 100, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>', category: periodlist[0].name },
                { name: 'Direct Sale 9L Cases 13P LY', field: "Dir_LY_13P_9LCases", displayName: "DD", width: 100, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>', category: periodlist[0].name },
                { name: 'Total 9L Cases 13P LY', field: "Tot_LY_13P_9LCases", displayName: "Total", width: 100, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>', category: periodlist[0].name },

                { name: 'Licensee 9L Cases 13P TY', field: "Lic_TY_13P_9LCases", displayName: "LCBO", width: 100, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>', category: periodlist[1].name },
                { name: 'Direct Sale 9L Cases 13P TY', field: "Dir_TY_13P_9LCases", displayName: "DD", width: 100, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>', category: periodlist[1].name },
                { name: 'Total 9L Cases 13P TY', field: "Tot_TY_13P_9LCases", displayName: "Total", width: 100, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>', category: periodlist[1].name},
                
                { name: 'Tot_13P_9LCasesPct', field: "Tot_13P_9LCasesPct", displayName: "Var %", cellFilter: "mapPercentage", width: 100, visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Tot_13P_9LCasesPct"></span></div>', category: periodlist[1].name },

                { name: 'Licensee Sales 13P LY', field: "Lic_LY_13P_SalesAmount", displayName: "LCBO", width: 100, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>', category: periodlist[2].name },
                { name: 'Direct Sale Sales 13P LY', field: "Dir_LY_13P_SalesAmount", displayName: "DD", width: 100, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>', category: periodlist[2].name},
                { name: 'Total Sales 13P LY', field: "Tot_LY_13P_SalesAmount", displayName: "Total", width: 100, cellFilter: 'currency:"$":0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>', category: periodlist[2].name},
                
                { name: 'Licensee Sales 13P TY', field: "Lic_TY_13P_SalesAmount", displayName: "LCBO", width: 100, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>', category: periodlist[3].name },
                { name: 'Direct Sale Sales 13P TY', field: "Dir_TY_13P_SalesAmount", displayName: "DD", width: 100, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>', category: periodlist[3].name},
                { name: 'Total Sales 13P TY', field: "Tot_TY_13P_SalesAmount", displayName: "Total", width: 100, cellFilter: 'currency:"$":0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>', category: periodlist[3].name},
                
                { name: 'Tot_13P_SalesAmountPct', field: "Tot_13P_SalesAmountPct", displayName: "Var %", cellFilter: "mapPercentage", width: 100, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Tot_13P_SalesAmountPct"></span></div>', category: periodlist[3].name },

                { name: 'Licensee Units 13P LY', field: "Lic_LY_13P_Units", displayName: "LCBO", width: 100, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>', category: periodlist[4].name},
                { name: 'Direct Sale Units 13P LY', field: "Dir_LY_13P_Units", displayName: "DD", width: 100, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>', category: periodlist[4].name},
                { name: 'Total Units 13P LY', field: "Tot_LY_13P_Units", displayName: "Total", width: 100, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>', category: periodlist[4].name},

                { name: 'Licensee Units 13P TY', field: "Lic_TY_13P_Units", displayName: "LCBO", width: 100, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>', category: periodlist[5].name},
                { name: 'Direct Sale Units 13P TY', field: "Dir_TY_13P_Units", displayName: "DD", width: 100, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>', category: periodlist[5].name},
                { name: 'Total Units 13P TY', field: "Tot_TY_13P_Units", displayName: "Total", width: 100, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>', category: periodlist[5].name},
                
                { name: 'Tot_13P_UnitsPct', field: "Tot_13P_UnitsPct", displayName: "Var %", cellFilter: "mapPercentage", width: 100, visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Tot_13P_UnitsPct"></span></div>', category: periodlist[5].name },

                //Total 3P
                { name: 'Licensee 9L Cases 3P LY', field: "Lic_LY_3P_9LCases", displayName: "LCBO", width: 100, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>', category: periodlist[6].name },
                { name: 'Direct Sale 9L Cases 3P LY', field: "Dir_LY_3P_9LCases", displayName: "DD", width: 100, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>', category: periodlist[6].name},
                { name: 'Total 9L Cases 3P LY', field: "Tot_LY_3P_9LCases", displayName: "Total", width: 100, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>', category: periodlist[6].name },

                { name: 'Licensee 9L Cases 3P TY', field: "Lic_TY_3P_9LCases", displayName: "LCBO", width: 100, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>', category: periodlist[7].name },
                { name: 'Direct Sale 9L Cases 3P TY', field: "Dir_TY_3P_9LCases", displayName: "DD", width: 100, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>', category: periodlist[7].name },
                { name: 'Total 9L Cases 3P TY', field: "Tot_TY_3P_9LCases", displayName: "Total", width: 100, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>', category: periodlist[7].name},
                
                { name: 'Tot_3P_9LCasesPct', field: "Tot_3P_9LCasesPct", displayName: "Var %", cellFilter: "mapPercentage", width: 100, visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Tot_3P_9LCasesPct"></span></div>', category: periodlist[7].name},

                { name: 'Licensee Sales 3P LY', field: "Lic_LY_3P_SalesAmount", displayName: "LCBO", width: 100, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>', category: periodlist[8].name},
                { name: 'Direct Sale Sales 3P LY', field: "Dir_LY_3P_SalesAmount", displayName: "DD", width: 100, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>', category: periodlist[8].name},
                { name: 'Total Sales 3P LY', field: "Tot_LY_3P_SalesAmount", displayName: "Total", width: 100, cellFilter: 'currency:"$":0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>', category: periodlist[8].name},
                
                { name: 'Licensee Sales 3P TY', field: "Lic_TY_3P_SalesAmount", displayName: "LCBO", width: 100, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>', category: periodlist[9].name },
                { name: 'Direct Sale Sales 3P TY', field: "Dir_TY_3P_SalesAmount", displayName: "DD", width: 100, cellFilter: 'currency:"$":0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>', category: periodlist[9].name},
                { name: 'Total Sales 3P TY', field: "Tot_TY_3P_SalesAmount", displayName: "Total", width: 100, cellFilter: 'currency:"$":0', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>', category: periodlist[9].name},

                { name: 'Tot_3P_SalesAmountPct', field: "Tot_3P_SalesAmountPct", displayName: "Var %", cellFilter: "mapPercentage", width: 100, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Tot_3P_SalesAmountPct"></span></div>', category: periodlist[9].name },

                { name: 'Licensee Units 3P LY', field: "Lic_LY_3P_Units", displayName: "LCBO", width: 100, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>', category: periodlist[10].name },
                { name: 'Direct Sale Units 3P LY', field: "Dir_LY_3P_Units", displayName: "DD", width: 100, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>', category: periodlist[10].name},
                { name: 'Total Units 3P LY', field: "Tot_LY_3P_Units", displayName: "Total", width: 100, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>', category: periodlist[10].name},
                
                { name: 'Licensee Units 3P TY', field: "Lic_TY_3P_Units", displayName: "LCBO", width: 100, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>', category: periodlist[11].name },
                { name: 'Direct Sale Units 3P TY', field: "Dir_TY_3P_Units", displayName: "DD", width: 100, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>', category: periodlist[11].name},
                { name: 'Total Units 3P TY', field: "Tot_TY_3P_Units", displayName: "Total", width: 100, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>', category: periodlist[11].name},
                
                { name: 'Tot_3P_UnitsPct', field: "Tot_3P_UnitsPct", displayName: "Var %", cellFilter: "mapPercentage", width: 100, visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Tot_3P_UnitsPct"></span></div>', category: periodlist[11].name},

                //Licensee 13P
               
                { name: 'Lic_13P_9LCasesPct', field: "Lic_13P_9LCasesPct", displayName: "Var %", cellFilter: "mapPercentage", width: 100, visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Lic_13P_9LCasesPct"></span></div>' },
                { name: 'Lic_13P_SalesAmountPct', field: "Lic_13P_SalesAmountPct", displayName: "Var %", cellFilter: "mapPercentage", visible: false, width: 100, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Lic_13P_SalesAmountPct"></span></div>' },
                { name: 'Lic_13P_UnitsPct', field: "Lic_13P_UnitsPct", displayName: "Var %", cellFilter: "mapPercentage", width: 100, visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Lic_13P_UnitsPct"></span></div>' },

                //Licensee 3P
                { name: 'Lic_3P_9LCasesPct', field: "Lic_3P_9LCasesPct", displayName: "Var %", cellFilter: "mapPercentage", width: 100, visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Lic_3P_9LCasesPct"></span></div>' },
                { name: 'Lic_3P_SalesAmountPct', field: "Lic_3P_SalesAmountPct", displayName: "Var %", cellFilter: "mapPercentage", visible: false, width: 100, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Lic_3P_SalesAmountPct"></span></div>' },
                { name: 'Lic_3P_UnitsPct', field: "Lic_3P_UnitsPct", displayName: "Var %", cellFilter: "mapPercentage", width: 100, visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Lic_3P_UnitsPct"></span></div>' },

                //Direct Sale 13P
                
                { name: 'Dir_13P_9LCasesPct', field: "Dir_13P_9LCasesPct", displayName: "Var %", cellFilter: "mapPercentage", width: 100, visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Dir_13P_9LCasesPct"></span></div>' },
                { name: 'Dir_13P_SalesAmountPct', field: "Dir_13P_SalesAmountPct", displayName: "Var %", cellFilter: "mapPercentage", width: 100, visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Dir_13P_SalesAmountPct"></span></div>' },
                { name: 'Dir_13P_UnitsPct', field: "Dir_13P_UnitsPct", displayName: "Var %", cellFilter: "mapPercentage", width: 100, visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Dir_13P_UnitsPct"></span></div>' },

                //Direct Sale 3P
                { name: 'Dir_3P_9LCasesPct', field: "Dir_3P_9LCasesPct", displayName: "Var %", cellFilter: "mapPercentage", width: 100, visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Dir_3P_9LCasesPct"></span></div>' },
                { name: 'Dir_3P_SalesAmountPct', field: "Dir_3P_SalesAmountPct", displayName: "Var %", cellFilter: "mapPercentage", width: 100, visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Dir_3P_SalesAmountPct"></span></div>' },
                { name: 'Dir_3P_UnitsPct', field: "Dir_3P_UnitsPct", displayName: "Var %", cellFilter: "mapPercentage", width: 100, visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Dir_3P_UnitsPct"></span></div>' }

            ],
            enableSorting: true,
            enableRowSelection: true,
            enableRowHeaderSelection: true,
            enableCellSelection: false,
            enableCellEditOnFocus: false,
            enableGridMenu: true,
            exporterMenuPdf: false,
            exporterCsvFilename: 'Licensee Sales Territory by ' + document.getElementById('SalesRepName').value + " - Last Completed Period: " + document.getElementById('SalesPeriod').value + '.csv',
            exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
            showColumnFooter: true
        }
    }


    $scope.gridOptions.onRegisterApi = function (gridApi) {
        $scope.gridApi = gridApi;
    };

    if (indicator === 2) {
        $scope.licgroups = GroupDropDown.licgroups;
        $scope.selectedlicgroup = GroupDropDown.licgroups[0];
        $scope.licdropboxitemselected = function (item) {
            $scope.selectedlicgroup = item;
        }
    }

    initUser();
    $scope.groups = GroupDropDown.groups;
    $scope.selectedgroup = (GroupService.getCatGroup($scope.currentUserInfo) != '' && GroupService.getCatGroup($scope.currentUserInfo) != null) ? GroupService.getCatGroup($scope.currentUserInfo) : GroupDropDown.groups[document.getElementById('SalesSelectedGroupId').value];

    $scope.groupdropboxitemselected = function (item) {
        GroupService.setCatGroup($scope.currentUserInfo, item)
        $scope.selectedgroup = item;
    }

    function initUser() {

        //Info used in setting the Page
        $scope.currentUser = document.getElementById('currentUserInfo').value;
        $scope.currentUserInfo = $scope.currentUser + "GroupSelected";
        $scope.currentUserPrdInfo = $scope.currentUser + "ProductCategorySelected";
    }

    init();

    //initial loading when rendering the page
    function init() {
        //loading
        $scope.loading = true;
        var mybody = angular.element(document).find('body');
        mybody.addClass('waiting');

        var userid = parseInt(document.getElementById('SalesUserId').value);
        var period = document.getElementById('SalesPeriod').value;

        if (indicator === 1) {
            $http({
                url: "/SalesSummaryStoreByTerritory",
                dataType: 'json',
                method: 'GET',
                data: '',
                headers: {
                    "Content-Type": "application/json",
                    "X-UserId": userid,
                    "X-PeriodKey": period,
                    "X-SortGroup": document.getElementById('SalesSelectedGroupId').value
                }
            }).
                then(
                function (result) {
                    var len = result.data.length;
                    $scope.gridOptions.data = result.data.slice(1, len);
                    //assign footer pct
                    var nineLitreElem = document.getElementById("Store_13P_9LCasesPct");
                    if (nineLitreElem)
                        nineLitreElem.textContent = (result.data[0].Store_13P_9LCasesPct * 100).toFixed(2).toString() + "%";
                    var salesAmtElem = document.getElementById("Store_13P_SalesAmountPct");
                    if (salesAmtElem)
                        salesAmtElem.textContent = (result.data[0].Store_13P_SalesAmountPct * 100).toFixed(2).toString() + "%";
                    var unitElem = document.getElementById("Store_13P_UnitsPct");
                    if (unitElem)
                        unitElem.textContent = (result.data[0].Store_13P_UnitsPct * 100).toFixed(2).toString() + "%";
                    nineLitreElem = document.getElementById("Store_3P_9LCasesPct");
                    if (nineLitreElem)
                        nineLitreElem.textContent = (result.data[0].Store_3P_9LCasesPct * 100).toFixed(2).toString() + "%";
                    salesAmtElem = document.getElementById("Store_3P_SalesAmountPct");
                    if (salesAmtElem)
                        salesAmtElem.textContent = (result.data[0].Store_3P_SalesAmountPct * 100).toFixed(2).toString() + "%";
                    unitElem = document.getElementById("Store_3P_UnitsPct");
                    if (unitElem)
                        unitElem.textContent = (result.data[0].Store_3P_UnitsPct * 100).toFixed(2).toString() + "%";
                },
                function (error) {
                    InternalErrorHandler(error);
                    $scope.loading = false;
                    mybody.removeClass('waiting');
                }).finally(function () {
                    $scope.loading = false;
                    mybody.removeClass('waiting');
                });
        } else {
            $http({
                url: "/SalesSummaryLicenseeByTerritory",
                dataType: 'json',
                method: 'GET',
                data: '',
                headers: {
                    "Content-Type": "application/json",
                    "X-UserId": userid,
                    "X-PeriodKey": period,
                    "X-SortGroup": document.getElementById('SalesSelectedGroupId').value
                }
            }).
                then(
                function (result) {
                    var len = result.data.length;
                    $scope.gridOptions.data = result.data.slice(1, len);
                    //assign footer pct
                    var nineLitreElem = document.getElementById("Tot_13P_9LCasesPct");
                    if (nineLitreElem)
                        nineLitreElem.textContent = (result.data[0].Tot_13P_9LCasesPct * 100).toFixed(2).toString() + "%";
                    var salesAmtElem = document.getElementById("Tot_13P_SalesAmountPct");
                    if (salesAmtElem)
                        salesAmtElem.textContent = (result.data[0].Tot_13P_SalesAmountPct * 100).toFixed(2).toString() + "%";
                    var unitElem = document.getElementById("Tot_13P_UnitsPct");
                    if (unitElem)
                        unitElem.textContent = (result.data[0].Tot_13P_UnitsPct * 100).toFixed(2).toString() + "%";
                    nineLitreElem = document.getElementById("Tot_3P_9LCasesPct");
                    if (nineLitreElem)
                        nineLitreElem.textContent = (result.data[0].Tot_3P_9LCasesPct * 100).toFixed(2).toString() + "%";
                    salesAmtElem = document.getElementById("Tot_3P_SalesAmountPct");
                    if (salesAmtElem)
                        salesAmtElem.textContent = (result.data[0].Tot_3P_SalesAmountPct * 100).toFixed(2).toString() + "%";
                    unitElem = document.getElementById("Tot_3P_UnitsPct");
                    if (unitElem)
                        unitElem.textContent = (result.data[0].Tot_3P_UnitsPct * 100).toFixed(2).toString() + "%";

                    nineLitreElem = document.getElementById("Lic_13P_9LCasesPct");
                    if (nineLitreElem)
                        nineLitreElem.textContent = (result.data[0].Lic_13P_9LCasesPct * 100).toFixed(2).toString() + "%";
                    salesAmtElem = document.getElementById("Lic_13P_SalesAmountPct");
                    if (salesAmtElem)
                        salesAmtElem.textContent = (result.data[0].Lic_13P_SalesAmountPct * 100).toFixed(2).toString() + "%";
                    unitElem = document.getElementById("Lic_13P_UnitsPct");
                    if (unitElem)
                        unitElem.textContent = (result.data[0].Lic_13P_UnitsPct * 100).toFixed(2).toString() + "%";
                    nineLitreElem = document.getElementById("Lic_3P_9LCasesPct");
                    if (nineLitreElem)
                        nineLitreElem.textContent = (result.data[0].Lic_3P_9LCasesPct * 100).toFixed(2).toString() + "%";
                    salesAmtElem = document.getElementById("Lic_3P_SalesAmountPct");
                    if (salesAmtElem)
                        salesAmtElem.textContent = (result.data[0].Lic_3P_SalesAmountPct * 100).toFixed(2).toString() + "%";
                    unitElem = document.getElementById("Lic_3P_UnitsPct");
                    if (unitElem)
                        unitElem.textContent = (result.data[0].Lic_3P_UnitsPct * 100).toFixed(2).toString() + "%";

                    nineLitreElem = document.getElementById("Dir_13P_9LCasesPct");
                    if (nineLitreElem)
                        nineLitreElem.textContent = (result.data[0].Dir_13P_9LCasesPct * 100).toFixed(2).toString() + "%";
                    salesAmtElem = document.getElementById("Dir_13P_SalesAmountPct");
                    if (salesAmtElem)
                        salesAmtElem.textContent = (result.data[0].Dir_13P_SalesAmountPct * 100).toFixed(2).toString() + "%";
                    unitElem = document.getElementById("Dir_13P_UnitsPct");
                    if (unitElem)
                        unitElem.textContent = (result.data[0].Dir_13P_UnitsPct * 100).toFixed(2).toString() + "%";
                    nineLitreElem = document.getElementById("Dir_3P_9LCasesPct");
                    if (nineLitreElem)
                        nineLitreElem.textContent = (result.data[0].Dir_3P_9LCasesPct * 100).toFixed(2).toString() + "%";
                    salesAmtElem = document.getElementById("Dir_3P_SalesAmountPct");
                    if (salesAmtElem)
                        salesAmtElem.textContent = (result.data[0].Dir_3P_SalesAmountPct * 100).toFixed(2).toString() + "%";
                    unitElem = document.getElementById("Dir_3P_UnitsPct");
                    if (unitElem)
                        unitElem.textContent = (result.data[0].Dir_3P_UnitsPct * 100).toFixed(2).toString() + "%";
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

        //determine column visibility
        RefreshClick();
    }

    //Refresh button click functions
    $scope.Reload = function () {
        RefreshClick();
        init();
    }

    function RefreshClick() {

        if (indicator === 1) {
            //update ColDefs
            if ($scope.selectedgroup.id === 0) {

                $scope.gridOptions.columnDefs[5].visible = true;
                $scope.gridOptions.columnDefs[6].visible = false;
                $scope.gridOptions.columnDefs[7].visible = false;

                $scope.gridOptions.columnDefs[8].visible = true;
                $scope.gridOptions.columnDefs[9].visible = true;
                $scope.gridOptions.columnDefs[10].visible = true;
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

                if ($scope.gridApi) {
                    $scope.gridApi.grid.sortColumn($scope.gridApi.grid.columns[6], uiGridConstants.ASC, true);
                    $scope.gridApi.grid.notifyDataChange(uiGridConstants.dataChange.ALL);
                }

            } else if ($scope.selectedgroup.id === 1) {

                $scope.gridOptions.columnDefs[5].visible = false;
                $scope.gridOptions.columnDefs[6].visible = true;
                $scope.gridOptions.columnDefs[7].visible = false;

                $scope.gridOptions.columnDefs[8].visible = false;
                $scope.gridOptions.columnDefs[9].visible = false;
                $scope.gridOptions.columnDefs[10].visible = false;
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

                if ($scope.gridApi) {
                    $scope.gridApi.grid.sortColumn($scope.gridApi.grid.columns[7], uiGridConstants.ASC, true);
                    $scope.gridApi.grid.notifyDataChange(uiGridConstants.dataChange.ALL);
                }

            } else if ($scope.selectedgroup.id === 2) {

                $scope.gridOptions.columnDefs[5].visible = false;
                $scope.gridOptions.columnDefs[6].visible = false;
                $scope.gridOptions.columnDefs[7].visible = true;

                $scope.gridOptions.columnDefs[8].visible = false;
                $scope.gridOptions.columnDefs[9].visible = false;
                $scope.gridOptions.columnDefs[10].visible = false;
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

                if ($scope.gridApi) {
                    $scope.gridApi.grid.sortColumn($scope.gridApi.grid.columns[8], uiGridConstants.ASC, true);
                    $scope.gridApi.grid.notifyDataChange(uiGridConstants.dataChange.ALL);
                }

            }

        } else {
            var i;
            for (i = 5; i < $scope.gridOptions.columnDefs.length; ++i) {
                $scope.gridOptions.columnDefs[i].visible = false;
            }
            var initialpos;
            initialpos = 7;
            ////if ($scope.selectedlicgroup.id === 0) {
            ////    initialpos = 7;
            ////} else if ($scope.selectedlicgroup.id === 1) {
            ////    initialpos = 28;
            ////} else {
            ////    initialpos = 43;
            ////};

            //update ColDefs
            if ($scope.selectedgroup.id === 0) {

                $scope.gridOptions.columnDefs[5].visible = true;

                $scope.gridOptions.columnDefs[initialpos + 1].visible = true;
                $scope.gridOptions.columnDefs[initialpos + 2].visible = true;
                $scope.gridOptions.columnDefs[initialpos + 3].visible = true;
                $scope.gridOptions.columnDefs[initialpos + 4].visible = true;
                $scope.gridOptions.columnDefs[initialpos + 5].visible = true;
                $scope.gridOptions.columnDefs[initialpos + 6].visible = true;
                $scope.gridOptions.columnDefs[initialpos + 7].visible = true;

                $scope.gridOptions.columnDefs[initialpos + 22].visible = true;
                $scope.gridOptions.columnDefs[initialpos + 23].visible = true;
                $scope.gridOptions.columnDefs[initialpos + 24].visible = true;
                $scope.gridOptions.columnDefs[initialpos + 25].visible = true;
                $scope.gridOptions.columnDefs[initialpos + 26].visible = true;
                $scope.gridOptions.columnDefs[initialpos + 27].visible = true;
                $scope.gridOptions.columnDefs[initialpos + 28].visible = true;

                if ($scope.gridApi) {
                    $scope.gridApi.grid.sortColumn($scope.gridApi.grid.columns[6], uiGridConstants.ASC, true);
                    $scope.gridApi.grid.notifyDataChange(uiGridConstants.dataChange.ALL);
                }

            } else if ($scope.selectedgroup.id === 1) {

                $scope.gridOptions.columnDefs[6].visible = true;

                $scope.gridOptions.columnDefs[initialpos + 8].visible = true;
                $scope.gridOptions.columnDefs[initialpos + 9].visible = true;
                $scope.gridOptions.columnDefs[initialpos + 10].visible = true;
                $scope.gridOptions.columnDefs[initialpos + 11].visible = true;
                $scope.gridOptions.columnDefs[initialpos + 12].visible = true;
                $scope.gridOptions.columnDefs[initialpos + 13].visible = true;
                $scope.gridOptions.columnDefs[initialpos + 14].visible = true;

                $scope.gridOptions.columnDefs[initialpos + 29].visible = true;
                $scope.gridOptions.columnDefs[initialpos + 30].visible = true;
                $scope.gridOptions.columnDefs[initialpos + 31].visible = true;
                $scope.gridOptions.columnDefs[initialpos + 32].visible = true;
                $scope.gridOptions.columnDefs[initialpos + 33].visible = true;
                $scope.gridOptions.columnDefs[initialpos + 34].visible = true;
                $scope.gridOptions.columnDefs[initialpos + 35].visible = true;

                if ($scope.gridApi) {
                    $scope.gridApi.grid.sortColumn($scope.gridApi.grid.columns[7], uiGridConstants.ASC, true);
                    $scope.gridApi.grid.notifyDataChange(uiGridConstants.dataChange.ALL);
                }

            } else if ($scope.selectedgroup.id === 2) {

                $scope.gridOptions.columnDefs[7].visible = true;

                $scope.gridOptions.columnDefs[initialpos + 15].visible = true;
                $scope.gridOptions.columnDefs[initialpos + 16].visible = true;
                $scope.gridOptions.columnDefs[initialpos + 17].visible = true;
                $scope.gridOptions.columnDefs[initialpos + 18].visible = true;
                $scope.gridOptions.columnDefs[initialpos + 19].visible = true;
                $scope.gridOptions.columnDefs[initialpos + 20].visible = true;
                $scope.gridOptions.columnDefs[initialpos + 21].visible = true;

                $scope.gridOptions.columnDefs[initialpos + 36].visible = true;
                $scope.gridOptions.columnDefs[initialpos + 37].visible = true;
                $scope.gridOptions.columnDefs[initialpos + 38].visible = true;
                $scope.gridOptions.columnDefs[initialpos + 39].visible = true;
                $scope.gridOptions.columnDefs[initialpos + 40].visible = true;
                $scope.gridOptions.columnDefs[initialpos + 41].visible = true;
                $scope.gridOptions.columnDefs[initialpos + 42].visible = true;
               

                if ($scope.gridApi) {
                    $scope.gridApi.grid.sortColumn($scope.gridApi.grid.columns[8], uiGridConstants.ASC, true);
                    $scope.gridApi.grid.notifyDataChange(uiGridConstants.dataChange.ALL);
                }
            }
        }

        if ($scope.gridApi) {
            $scope.gridApi.grid.refresh();
        }
    }

    //subgrid control for store
    $scope.salespersonalstore = function (accountnumber, accountname) {
        var userid = parseInt(document.getElementById('SalesUserId').value);
        var period = document.getElementById('SalesPeriod').value;
        var headeroption = parseInt(document.getElementById('HeaderOption').value);
        var salesname = document.getElementById('SalesRepName').value;
        var selectedgroupid = $scope.selectedgroup.id;

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
        f.appendChild(GroupDropDown.createFormElement("userid", userid));
        f.appendChild(GroupDropDown.createFormElement("period", period));
        f.appendChild(GroupDropDown.createFormElement("accountnumber", accountnumber));
        f.appendChild(GroupDropDown.createFormElement("salesname", salesname));
        f.appendChild(GroupDropDown.createFormElement("accountname", accountname));
        f.appendChild(GroupDropDown.createFormElement("selectedgroupid", selectedgroupid));
        if (headeroption === 2) {
            f.appendChild(GroupDropDown.createFormElement("headeroption", headeroption));
        }
        //submit form
        f.submit();
        //remove the newly created form after submit
        f.remove();
    }

    //subgrid control for licensee
    $scope.salespersonallicensee = function (accountnumber, accountname) {
        var userid = parseInt(document.getElementById('SalesUserId').value);
        var period = document.getElementById('SalesPeriod').value;
        var headeroption = parseInt(document.getElementById('HeaderOption').value);
        var salesname = document.getElementById('SalesRepName').value;
        var selectedgroupid = $scope.selectedgroup.id;

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
        f.appendChild(GroupDropDown.createFormElement("userid", userid));
        f.appendChild(GroupDropDown.createFormElement("period", period));
        f.appendChild(GroupDropDown.createFormElement("accountnumber", accountnumber));
        f.appendChild(GroupDropDown.createFormElement("salesname", salesname));
        f.appendChild(GroupDropDown.createFormElement("accountname", accountname));
        f.appendChild(GroupDropDown.createFormElement("selectedgroupid", selectedgroupid));
        if (headeroption === 2) {
            f.appendChild(GroupDropDown.createFormElement("headeroption", headeroption));
        }
        //submit form
        f.submit();
        //remove the newly created form after submit
        f.remove();
    }
}