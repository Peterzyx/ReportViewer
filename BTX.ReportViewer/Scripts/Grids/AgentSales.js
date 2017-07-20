//SalesByBrands
app.controller('agentSalesCtrl', AgentSalesCtrl);

AgentSalesCtrl.$inject = ['$http', '$scope', 'GroupDropDown','GroupService'];
function AgentSalesCtrl($http, $scope, GroupDropDown, GroupService) {

    //UI Grid Setting
    $scope.gridOptions = {
        columnDefs: [
            { name: 'Brand Id', field: 'BrandId', width: '5%', visible: false },
            { name: 'Brand', field: "BrandName", width: '15%', pinnedLeft: true, cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;"><a ng-click="grid.appScope.salesbyproductssubgrid(row.entity.BrandId, row.entity.BrandName)" style="cursor:pointer;">{{COL_FIELD}}</a></div>' },
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

            { name: 'On Prem 13 Per TY', field: "OnPrem13PerTY", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'On Prem 13 Per LY', field: "OnPrem13PerLY", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'TY_OnPrem13PerPct', field: "TY_OnPrem13PerPct", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', type: 'number' },

            { name: 'Agencies 13 Per TY', field: "Agencies13PerTY", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'Agencies 13 Per LY', field: "Agencies13PerLY", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'TY_Agencies13PerPct', field: "TY_Agencies13PerPct", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', type: 'number' }
        ],
        enableSorting: true,
        enableRowSelection: true,
        enableRowHeaderSelection: true,
        enableCellSelection: false,
        enableCellEditOnFocus: false,
        enableGridMenu: true,
        exporterMenuPdf: false,
        exporterCsvFilename: 'Market Reports Sales by Brands - ' + document.getElementById('SalesAgentName').value + ' - Last Completed Period: ' + document.getElementById('SalesPeriod').value + '.csv',
        exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location"))
    };


    $scope.gridOptions.onRegisterApi = function (gridApi) {
        $scope.gridApi = gridApi;
    };

    //dropdown

    $scope.groups = GroupDropDown.groups;
    $scope.selectedgroup = (GroupService.getCatGroup($scope.currentUserInfo) != '' && GroupService.getCatGroup($scope.currentUserInfo) != null) ? GroupService.getCatGroup($scope.currentUserInfo) : GroupDropDown.groups[document.getElementById('SalesSelectedGroupId').value];

    $scope.groupdropboxitemselected = function (item) {
        GroupService.setCatGroup($scope.currentUserInfo, item)
        $scope.selectedgroup = item;
    }

    init();

    //initial loading when rendering the page
    function init() {
        //loading
        $scope.loading = true;
        var mybody = angular.element(document).find('body');
        mybody.addClass('waiting');

        var period = document.getElementById('SalesPeriod').value;
        //var agentid = parseInt(document.getElementById('SalesAgentId').value);
        var agentname = document.getElementById('SalesAgentName').value;
        var setnames = document.getElementById('SalesSetNames').value;
        var unitsizes = document.getElementById('SalesUnitSizes').value;
        var pricefrom = parseFloat(document.getElementById('SalesPriceFrom').value);
        var priceto = parseFloat(document.getElementById('SalesPriceTo').value);
        //Info used in setting the Page
        $scope.currentUser = document.getElementById('currentUserInfo').value;
        $scope.currentUserInfo = $scope.currentUser + "GroupSelected";

        $http({
            url: "/AgentSales",
            dataType: 'json',
            method: 'GET',
            data: '',
            headers: {
                "Content-Type": "application/json",
                "X-PeriodKey": period,
                "X-AgentName": agentname,
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

        //determine visibility
        RefreshClick();
    }

    //Refresh button click functions
    $scope.Reload = function () {
        RefreshClick();
        $scope.gridApi.grid.refresh();
    }

    function RefreshClick() {

        //update ColDefs
        if ($scope.selectedgroup.id === 0) {

            $scope.gridOptions.columnDefs[5].visible = true;
            $scope.gridOptions.columnDefs[6].visible = true;
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
            $scope.gridOptions.columnDefs[26].visible = false;
            $scope.gridOptions.columnDefs[27].visible = false;
            $scope.gridOptions.columnDefs[28].visible = false;
            $scope.gridOptions.columnDefs[29].visible = false;
            $scope.gridOptions.columnDefs[30].visible = false;
            $scope.gridOptions.columnDefs[31].visible = false;

        } else if ($scope.selectedgroup.id === 1) {

            $scope.gridOptions.columnDefs[5].visible = false;
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
            $scope.gridOptions.columnDefs[26].visible = true;
            $scope.gridOptions.columnDefs[27].visible = true;
            $scope.gridOptions.columnDefs[28].visible = true;
            $scope.gridOptions.columnDefs[29].visible = false;
            $scope.gridOptions.columnDefs[30].visible = false;
            $scope.gridOptions.columnDefs[31].visible = false;

        } else if ($scope.selectedgroup.id === 2) {

            $scope.gridOptions.columnDefs[5].visible = false;
            $scope.gridOptions.columnDefs[6].visible = false;
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
            $scope.gridOptions.columnDefs[26].visible = false;
            $scope.gridOptions.columnDefs[27].visible = false;
            $scope.gridOptions.columnDefs[28].visible = false;
            $scope.gridOptions.columnDefs[29].visible = true;
            $scope.gridOptions.columnDefs[30].visible = true;
            $scope.gridOptions.columnDefs[31].visible = true;

        }
    };

    //load subgrid
    $scope.salesbyproductssubgrid = function (brandid, brandname) {
        var period = document.getElementById('SalesPeriod').value;
        var id = brandid;
        var setnames = document.getElementById('SalesSetNames').value;
        var unitsizes = document.getElementById('SalesUnitSizes').value;
        var pricefrom = parseFloat(document.getElementById('SalesPriceFrom').value);
        var priceto = parseFloat(document.getElementById('SalesPriceTo').value);
        var agentid = parseInt(document.getElementById('SalesAgentId').value);
        var agentname = document.getElementById('SalesAgentName').value;

        //re-assign global factory
        var selectedgroupid = $scope.selectedgroup.id;

        //header option loading
        var headeroption = parseInt(document.getElementById('HeaderOption').value);
        //if (headeroption === 2) {
        //    var url = document.getElementById('SalesByProductsUrl').value + "/" + period + "/" + agentid + "/" + agentname + "/" + setnames + "/" + brandid + "/" + brandname + "/" + unitsizes + "/" + pricefrom + "/" + priceto + "/" + selectedgroupid + "/" + headeroption;
        //} else {
        //    var url = document.getElementById('SalesByProductsUrl').value + "/" + period + "/" + agentid + "/" + agentname + "/" + setnames + "/" + brandid + "/" + brandname + "/" + unitsizes + "/" + pricefrom + "/" + priceto + "/" + selectedgroupid;
        //};

        //create a dynamic form
        var f = document.createElement("form");
        f.setAttribute('id', "salesbyproductssubgridform");
        f.setAttribute('method', "post");
        f.setAttribute('action', document.getElementById('SalesByProductsUrl').value);
        //target blank will post it to a new tab
        f.setAttribute("target", "_blank");
        //append the form to the bottom of the body
        document.body.appendChild(f);
        //create hidden elements and append them to the form
        f.appendChild(GroupDropDown.createFormElement("period", period));
        f.appendChild(GroupDropDown.createFormElement("agentid", agentid));
        f.appendChild(GroupDropDown.createFormElement("agentname", agentname));
        f.appendChild(GroupDropDown.createFormElement("setnames", setnames));
        f.appendChild(GroupDropDown.createFormElement("brandid", id));
        f.appendChild(GroupDropDown.createFormElement("brandname", brandname));
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