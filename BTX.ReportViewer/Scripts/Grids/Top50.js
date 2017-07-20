//SalesByAgencyTop50
app.controller('top50Ctrl', Top50Ctrl);

Top50Ctrl.$inject = ['$http', '$scope', 'GroupDropDown','GroupService'];
function Top50Ctrl($http, $scope, GroupDropDown, GroupService) {

    //Accordion settings
    $scope.oneAtATime = true;
    $scope.status = {
        open: false
    };

    //UI Grid Setting
    $scope.gridOptions = {
        columnDefs: [
            {
                name: 'Rank', field: "xRank", width: 50, pinnedLeft: true, visible: true, type: 'number'
            },
            { name: 'Agent', field: "AgentName", width: '250', cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;"><a ng-click="grid.appScope.salesbyagencysubgrid(row.entity.AgentId, row.entity.AgentName)" style="cursor:pointer;">{{COL_FIELD}}</a></div>' },
            { name: 'Agent Id', field: 'AgentId', width: '50', visible: false },
            { name: 'Nb General', field: "Nb_General", width: '50', cellFilter: 'number:0', type: 'number' },
            { name: 'Nb Vintages Essential', field: "Nb_VintageEssential", width: '50', cellFilter: 'number:0', type: 'number' },
            { name: 'Nb Vintages', field: "Nb_Vintages", width: '50', cellFilter: 'number:0', type: 'number' },

            { name: 'TY_9LCases', field: "TY_9LCases", width: '100', cellFilter: 'number:0', type: 'number' },
            { name: 'LY_9LCases', field: "LY_9LCases", width: '100', cellFilter: 'number:0', type: 'number' },
            { name: 'TY_9LCasesPct', field: "TY_9LCasesPct", displayName: "Var %", width: '70', cellFilter: 'mapPercentage', type: 'number' },
            { name: 'TY_TotalSalesAmount', field: "TY_TotalSalesAmount", width: '100', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'LY_TotalSalesAmount', field: "LY_TotalSalesAmount", width: '100', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'TY_TotalSalesAmountPct', field: "TY_TotalSalesAmountPct", displayName: "Var %", width: '70', cellFilter: 'mapPercentage', visible: false, type: 'number' },
            { name: 'TY_Units', field: "TY_Units", width: '100', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'LY_Units', field: "LY_Units", width: '100', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'TY_UnitsPct', field: "TY_UnitsPct", displayName: "Var %", width: '70', cellFilter: 'mapPercentage', visible: false, type: 'number' },

            { name: 'TY6m_9LCases', field: "TY6m_9LCases", width: '100', cellFilter: 'number:0', type: 'number' },
            { name: 'LY6m_9LCases', field: "LY6m_9LCases", width: '100', cellFilter: 'number:0', type: 'number' },
            { name: 'TY6m_9LCasesPct', field: "TY6m_9LCasesPct", displayName: "Var %", width: '70', cellFilter: 'mapPercentage', type: 'number' },
            { name: 'TY6m_TotalSalesAmount', field: "TY6m_TotalSalesAmount", width: '100', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'LY6m_TotalSalesAmount', field: "LY6m_TotalSalesAmount", width: '100', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'TY6m_TotalSalesAmountPct', field: "TY6m_TotalSalesAmountPct", displayName: "Var %", width: '70', cellFilter: 'mapPercentage', visible: false, type: 'number' },
            { name: 'TY6m_Units', field: "TY6m_Units", width: '100', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'LY6m_Units', field: "LY6m_Units", width: '100', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'TY6m_UnitsPct', field: "TY6m_UnitsPct", width: '70', displayName: "Var %", cellFilter: 'mapPercentage', visible: false, type: 'number' },

            { name: 'TY1y_9LCases', field: "TY1y_9LCases", width: '100', cellFilter: 'number:0', type: 'number' },
            { name: 'LY1y_9LCases', field: "LY1y_9LCases", width: '100', cellFilter: 'number:0', type: 'number' },
            { name: 'TY1y_9LCasesPct', field: "TY1y_9LCasesPct", displayName: "Var %", width: '70', cellFilter: 'mapPercentage', type: 'number' },
            { name: 'TY1y_TotalSalesAmount', field: "TY1y_TotalSalesAmount", width: '100', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'LY1y_TotalSalesAmount', field: "LY1y_TotalSalesAmount", width: '100', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'TY1y_TotalSalesAmountPct', field: "TY1y_TotalSalesAmountPct", displayName: "Var %", width: '70', cellFilter: 'mapPercentage', visible: false, type: 'number' },
            { name: 'TY1y_Units', field: "TY1y_Units", width: '100', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'LY1y_Units', field: "LY1y_Units", width: '100', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'TY1y_UnitsPct', field: "TY1y_UnitsPct", displayName: "Var %", width: '70', cellFilter: 'mapPercentage', visible: false, type: 'number' },

            { name: 'Rol Year PerTY$', field: "TY1y_TotalSalesAmount", width: '130', cellFilter: 'currency:"$":0', type: 'number' },
            { name: 'Rol Year Pct', field: "TY1y_TotalSalesAmountPct", displayName: "Var %", width: '70', cellFilter: 'mapPercentage', type: 'number' },

            { name: 'On Prem 13 Per TY', field: "OnPrem13PerTY", width: '100', cellFilter: 'number:0', type: 'number' },
            { name: 'On Prem 13 Per LY', field: "OnPrem13PerLY", width: '100', cellFilter: 'number:0', type: 'number' },
            { name: 'TY_OnPrem13PerPct', field: "TY_OnPrem13PerPct", displayName: "Var %", width: '70', cellFilter: 'mapPercentage', type: 'number' },

            { name: 'Agencies 13 Per TY', field: "Agencies13PerTY", width: '100', cellFilter: 'number:0', type: 'number' },
            { name: 'Agencies 13 Per LY', field: "Agencies13PerLY", width: '100', cellFilter: 'number:0', type: 'number' },
            { name: 'TY_Agencies13PerPct', field: "TY_Agencies13PerPct", displayName: "Var %", width: '70', cellFilter: 'mapPercentage', type: 'number' }
        ],
        enableSorting: true,
        enableRowSelection: true,
        enableRowHeaderSelection: true,
        enableCellSelection: false,
        enableCellEditOnFocus: false,
        enableGridMenu: true,
        exporterMenuPdf: false,
        exporterCsvFilename: 'Market Reports Sales by Agency - Ontario - Last Completed Period: ' + $scope.selectedperiodkey + '.csv',
        exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location"))
    };

    $scope.gridOptions.onRegisterApi = function (gridApi) {
        $scope.gridApi = gridApi;
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
                $scope.gridOptions.exporterCsvFilename = 'Market Reports Sales by Agency - Ontario - Last Completed Period: ' + periodkey + '.csv'

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
                                $http({
                                    url: "/Top50Sales",
                                    dataType: 'json',
                                    method: 'GET',
                                    data: '',
                                    headers: {
                                        "Content-Type": "application/json",
                                        "X-PeriodKey": periodkey,
                                        "X-SetNames": strSetNames,
                                        "X-UnitSizes": strUnitSizes,
                                        "X-PriceFrom": $scope.pricefrom.value,
                                        "X-PriceTo": $scope.priceto.value
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
                                        $scope.tmpPeriod = periodkey;
                                        $scope.tmpSetNames = strSetNames;
                                        $scope.tmpUnitSizes = strUnitSizes;
                                        $scope.tmpPriceFrom = $scope.pricefrom.value;
                                        $scope.tmpPriceTo = $scope.priceto.value;
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
            $scope.gridOptions.columnDefs[6].visible = true;
            $scope.gridOptions.columnDefs[7].visible = true;
            $scope.gridOptions.columnDefs[8].visible = true;
            $scope.gridOptions.columnDefs[9].visible = false;
            $scope.gridOptions.columnDefs[10].visible = false;
            $scope.gridOptions.columnDefs[11].visible = false;
            $scope.gridOptions.columnDefs[12].visible = false;
            $scope.gridOptions.columnDefs[13].visible = false;
            $scope.gridOptions.columnDefs[14].visible = false;
            $scope.gridOptions.columnDefs[15].visible = true;
            $scope.gridOptions.columnDefs[16].visible = true;
            $scope.gridOptions.columnDefs[17].visible = true;
            $scope.gridOptions.columnDefs[18].visible = false;
            $scope.gridOptions.columnDefs[19].visible = false;
            $scope.gridOptions.columnDefs[20].visible = false;
            $scope.gridOptions.columnDefs[21].visible = false;
            $scope.gridOptions.columnDefs[22].visible = false;
            $scope.gridOptions.columnDefs[23].visible = false;
            $scope.gridOptions.columnDefs[24].visible = true;
            $scope.gridOptions.columnDefs[25].visible = true;
            $scope.gridOptions.columnDefs[26].visible = true;
            $scope.gridOptions.columnDefs[27].visible = false;
            $scope.gridOptions.columnDefs[28].visible = false;
            $scope.gridOptions.columnDefs[29].visible = false;
            $scope.gridOptions.columnDefs[30].visible = false;
            $scope.gridOptions.columnDefs[31].visible = false;
            $scope.gridOptions.columnDefs[32].visible = false;
            $scope.gridApi.grid.refresh();
        } else if ($scope.selectedgroup.id === 1) {
            $scope.gridOptions.columnDefs[6].visible = false;
            $scope.gridOptions.columnDefs[7].visible = false;
            $scope.gridOptions.columnDefs[8].visible = false;
            $scope.gridOptions.columnDefs[9].visible = true;
            $scope.gridOptions.columnDefs[10].visible = true;
            $scope.gridOptions.columnDefs[11].visible = true;
            $scope.gridOptions.columnDefs[12].visible = false;
            $scope.gridOptions.columnDefs[13].visible = false;
            $scope.gridOptions.columnDefs[14].visible = false;
            $scope.gridOptions.columnDefs[15].visible = false;
            $scope.gridOptions.columnDefs[16].visible = false;
            $scope.gridOptions.columnDefs[17].visible = false;
            $scope.gridOptions.columnDefs[18].visible = true;
            $scope.gridOptions.columnDefs[19].visible = true;
            $scope.gridOptions.columnDefs[20].visible = true;
            $scope.gridOptions.columnDefs[21].visible = false;
            $scope.gridOptions.columnDefs[22].visible = false;
            $scope.gridOptions.columnDefs[23].visible = false;
            $scope.gridOptions.columnDefs[24].visible = false;
            $scope.gridOptions.columnDefs[25].visible = false;
            $scope.gridOptions.columnDefs[26].visible = false;
            $scope.gridOptions.columnDefs[27].visible = true;
            $scope.gridOptions.columnDefs[28].visible = true;
            $scope.gridOptions.columnDefs[29].visible = true;
            $scope.gridOptions.columnDefs[30].visible = false;
            $scope.gridOptions.columnDefs[31].visible = false;
            $scope.gridOptions.columnDefs[32].visible = false;
            $scope.gridApi.grid.refresh();
        } else if ($scope.selectedgroup.id === 2) {
            $scope.gridOptions.columnDefs[6].visible = false;
            $scope.gridOptions.columnDefs[7].visible = false;
            $scope.gridOptions.columnDefs[8].visible = false;
            $scope.gridOptions.columnDefs[9].visible = false;
            $scope.gridOptions.columnDefs[10].visible = false;
            $scope.gridOptions.columnDefs[11].visible = false;
            $scope.gridOptions.columnDefs[12].visible = true;
            $scope.gridOptions.columnDefs[13].visible = true;
            $scope.gridOptions.columnDefs[14].visible = true;
            $scope.gridOptions.columnDefs[15].visible = false;
            $scope.gridOptions.columnDefs[16].visible = false;
            $scope.gridOptions.columnDefs[17].visible = false;
            $scope.gridOptions.columnDefs[18].visible = false;
            $scope.gridOptions.columnDefs[19].visible = false;
            $scope.gridOptions.columnDefs[20].visible = false;
            $scope.gridOptions.columnDefs[21].visible = true;
            $scope.gridOptions.columnDefs[22].visible = true;
            $scope.gridOptions.columnDefs[23].visible = true;
            $scope.gridOptions.columnDefs[24].visible = false;
            $scope.gridOptions.columnDefs[25].visible = false;
            $scope.gridOptions.columnDefs[26].visible = false;
            $scope.gridOptions.columnDefs[27].visible = false;
            $scope.gridOptions.columnDefs[28].visible = false;
            $scope.gridOptions.columnDefs[29].visible = false;
            $scope.gridOptions.columnDefs[30].visible = true;
            $scope.gridOptions.columnDefs[31].visible = true;
            $scope.gridOptions.columnDefs[32].visible = true;
            $scope.gridApi.grid.refresh();
        }

        //set export file name
        $scope.gridOptions.exporterCsvFilename = 'Market Reports Sales by Agency - Ontario - Last Completed Period: ' + strPeriod + '.csv';

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

            $http({
                url: "/Top50Sales",
                dataType: 'json',
                method: 'GET',
                data: '',
                headers: {
                    "Content-Type": "application/json",
                    "X-PeriodKey": strPeriod,
                    "X-SetNames": strSetNames,
                    "X-UnitSizes": strUnitSizes,
                    "X-PriceFrom": fromValue,
                    "X-PriceTo": toValue
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
                    $scope.tmpPeriod = strPeriod;
                    $scope.tmpSetNames = strSetNames;
                    $scope.tmpUnitSizes = strUnitSizes;
                    $scope.tmpPriceFrom = fromValue;
                    $scope.tmpPriceTo = toValue;
                });
        }
    }

    //dropdown
    $scope.periodkeydropboxitemselected = function (item) {
        $scope.selectedperiodkey = item;
        //set export file name
        $scope.gridOptions.exporterCsvFilename = 'Market Reports Sales by Agency - Ontario - Last Completed Period: ' + $scope.selectedperiodkey.label + '.csv'
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
            $scope.gridOptions.exporterCsvFilename = 'Market Reports Sales by Agency - Ontario - Last Completed Period: ' + strPeriodKey + '.csv'

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

    //load subgrid
    $scope.salesbyagencysubgrid = function (agentid, agentname) {
        var period = $scope.tmpPeriod;
        var setnames = $scope.tmpSetNames;
        var unitsizes = $scope.tmpUnitSizes;
        var pricefrom = $scope.tmpPriceFrom;
        var priceto = $scope.tmpPriceTo;

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
        f.setAttribute('id', "salesbyagencysubgridform");
        f.setAttribute('method', "post");
        f.setAttribute('action', document.getElementById('SalesByBrandsUrl').value);
        //target blank will post it to a new tab
        f.setAttribute("target", "_blank");
        //append the form to the bottom of the body
        document.body.appendChild(f);
        //create hidden elements and append them to the form
        f.appendChild(GroupDropDown.createFormElement("period", period));
        f.appendChild(GroupDropDown.createFormElement("agentid", agentid));
        f.appendChild(GroupDropDown.createFormElement("agentname", agentname));
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