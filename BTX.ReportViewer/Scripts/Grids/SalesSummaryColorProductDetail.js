//SalesByProducts
app.controller('SalesSummaryColorProductDetailCtrl', SalesSummaryColorProductDetailCtrl);

SalesSummaryColorProductDetailCtrl.$inject = ['$http', '$scope', 'GroupDropDown'];
function SalesSummaryColorProductDetailCtrl($http, $scope, GroupDropDown) {

    //UI Grid Setting
    $scope.grid = {
        columnDefs: [

            { name: 'Product', field: "Product", width: '15%' },

            { name: 'Nb Regular', field: "Nb_General", width: '5%', cellFilter: 'number:0', type: 'number', visible:false },
            { name: 'Nb Vintages Essential', field: "Nb_VintageEssential", width: '5%', cellFilter: 'number:0', type: 'number', visible: false},
            { name: 'Nb Vintages', field: "Nb_Vintages", width: '5%', cellFilter: 'number:0', type: 'number', visible: false},

            { name: 'LY_9LCases', field: "LY_9LCases", displayName: "P12 LY", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'TY_9LCases', field: "TY_9LCases", displayName: "P12 TY", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'TY_9LCasesPct', field: "TY_9LCasesPct", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', type: 'number' },
            { name: 'LY_Units', field: "LY_Units", displayName: "P12 LY", width: '5%', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'TY_Units', field: "TY_Units", displayName: "P12 TY", width: '5%', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'TY_UnitsPct', field: "TY_UnitsPct", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', visible: false, type: 'number' },
            { name: 'LY_TotalSalesAmount', field: "LY_TotalSalesAmount", displayName: "P12 LY", width: '5%', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'TY_TotalSalesAmount', field: "TY_TotalSalesAmount", displayName: "P12 TY", width: '5%', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'TY_TotalSalesAmountPct', field: "TY_TotalSalesAmountPct", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', visible: false, type: 'number' },

            { name: 'Week1', field: "Week1", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'Week2', field: "Week2", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'Week3', field: "Week3", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'Week4', field: "Week4", width: '5%', cellFilter: 'number:0', type: 'number' },

            { name: 'LY_TD_9LCases', field: "LY_9LCases", displayName: "P12 TD LY", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'TY_TD_9LCases', field: "TY_9LCases", displayName: "P12 TD TY", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'TY_TD_9LCasesPct', field: "TY_9LCasesPct", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', type: 'number' },
            { name: 'LY_TD_Units', field: "LY_Units", displayName: "P12 LY", width: '5%', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'TY_TD_Units', field: "TY_Units", displayName: "P12 TY", width: '5%', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'TY_TD_UnitsPct', field: "TY_UnitsPct", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', visible: false, type: 'number' },
            { name: 'LY_TD_TotalSalesAmount', field: "LY_TotalSalesAmount", displayName: "P12 LY", width: '5%', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'TY_TD_TotalSalesAmount', field: "TY_TotalSalesAmount", displayName: "P12 TY", width: '5%', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'TY_TD_TotalSalesAmountPct', field: "TY_TotalSalesAmountPct", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', visible: false, type: 'number' },

            { name: 'LY6m_9LCases', field: "LY6m_9LCases", displayName: "6 Last Per LY", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'TY6m_9LCases', field: "TY6m_9LCases", displayName: "6 Last Per TY", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'TY6m_9LCasesPct', field: "TY6m_9LCasesPct", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', type: 'number' },
            { name: 'LY6m_Units', field: "LY6m_Units", displayName: "6 Last Per LY", width: '5%', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'TY6m_Units', field: "TY6m_Units", displayName: "6 Last Per TY", width: '5%', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'TY6m_UnitsPct', field: "TY6m_UnitsPct", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', visible: false, type: 'number' },
            { name: 'LY6m_TotalSalesAmount', field: "LY6m_TotalSalesAmount", displayName: "6 Last Per LY", width: '5%', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'TY6m_TotalSalesAmount', field: "TY6m_TotalSalesAmount", displayName: "6 Last Per TY", width: '5%', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'TY6m_TotalSalesAmountPct', field: "TY6m_TotalSalesAmountPct", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', visible: false, type: 'number' },

            { name: 'LYYTD_9LCases', field: "LYYTD_9LCases", displayName: "YTD LY", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'TYYTD_9LCases', field: "TYYTD_9LCases", displayName: "YTD TY", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'TYYTD_9LCasesPct', field: "TYYTD_9LCasesPct", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', type: 'number' },
            { name: 'LYYTD_Units', field: "LYYTD_Units", displayName: "YTD LY", width: '5%', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'TYYTD_Units', field: "TYYTD_Units", displayName: "YTD TY", width: '5%', cellFilter: 'number:0', visible: false, type: 'number' },
            { name: 'TYYTD_UnitsPct', field: "TYYTD_UnitsPct", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', visible: false, type: 'number' },
            { name: 'LYYTD_SalesAmount', field: "LYYTD_SalesAmount", displayName: "YTD LY", width: '5%', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'TYYTD_SalesAmount', field: "TYYTD_SalesAmount", displayName: "YTD TY", width: '5%', cellFilter: 'currency:"$":0', visible: false, type: 'number' },
            { name: 'TYYTD_SalesAmountPct', field: "TYYTD_SalesAmountPct", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', visible: false, type: 'number' },

            { name: 'LY13_OnPrem', field: "LY13_OnPrem", displayName: "13 Last Per LY", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'TY13_OnPrem', field: "TY13_OnPrem", displayName: "13 Last per TY", width: '5%', cellFilter: 'number:0', type: 'number' },
            { name: 'TY13_OnPremPct', field: "TY13_OnPremPct", displayName: "Var %", width: '5%', cellFilter: 'mapPercentage', type: 'number' },
        ],
        enableSorting: true,
        enableRowSelection: true,
        enableRowHeaderSelection: true,
        enableCellSelection: false,
        enableCellEditOnFocus: false,
        enableGridMenu: true,
        exporterMenuPdf: false,
        exporterCsvFilename: 'Sales Summary by Color Product Detail - Color ' + document.getElementById('ColorName').value + '.csv',
        exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location"))
    };

    $scope.grid.onRegisterApi = function (gridApi) {
        $scope.gridApi = gridApi;
    };

    $scope.groups = GroupDropDown.groups;
    $scope.selectedgroup = GroupDropDown.groups[document.getElementById('SalesSelectedGroupId').value];

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
        var category = document.getElementById('Category').value;
      //  var salesrepid = parseInt(document.getElementById('SalesRepId').value);
        var groupid = parseInt(document.getElementById('SalesSelectedGroupId').value);

        $http({
            url: "/SalesSummaryColorProductDetail",
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
                "X-Category": category,
             //   "X-SalesRepId": salesrepid,
                "X-Group": groupid
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

        //determine column visibility
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

            $scope.grid.columnDefs[17].visible = true;
            $scope.grid.columnDefs[18].visible = true;
            $scope.grid.columnDefs[19].visible = true;
            $scope.grid.columnDefs[20].visible = false;
            $scope.grid.columnDefs[21].visible = false;
            $scope.grid.columnDefs[22].visible = false;
            $scope.grid.columnDefs[23].visible = false;
            $scope.grid.columnDefs[24].visible = false;
            $scope.grid.columnDefs[25].visible = false;


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

        } else if (selectedgroupid === 1) {

            $scope.grid.columnDefs[4].visible = false;
            $scope.grid.columnDefs[5].visible = false;
            $scope.grid.columnDefs[6].visible = false;
            $scope.grid.columnDefs[7].visible = true;
            $scope.grid.columnDefs[8].visible = true;
            $scope.grid.columnDefs[9].visible = true;
            $scope.grid.columnDefs[10].visible = false;
            $scope.grid.columnDefs[11].visible = false;
            $scope.grid.columnDefs[12].visible = false;

            $scope.grid.columnDefs[17].visible = false;
            $scope.grid.columnDefs[18].visible = false;
            $scope.grid.columnDefs[19].visible = false;
            $scope.grid.columnDefs[20].visible = true;
            $scope.grid.columnDefs[21].visible = true;
            $scope.grid.columnDefs[22].visible = true;
            $scope.grid.columnDefs[23].visible = false;
            $scope.grid.columnDefs[24].visible = false;
            $scope.grid.columnDefs[25].visible = false;


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

        } else if (selectedgroupid === 2) {

            $scope.grid.columnDefs[4].visible = false;
            $scope.grid.columnDefs[5].visible = false;
            $scope.grid.columnDefs[6].visible = false;
            $scope.grid.columnDefs[7].visible = false;
            $scope.grid.columnDefs[8].visible = false;
            $scope.grid.columnDefs[9].visible = false;
            $scope.grid.columnDefs[10].visible = true;
            $scope.grid.columnDefs[11].visible = true;
            $scope.grid.columnDefs[12].visible = true;

            $scope.grid.columnDefs[17].visible = false;
            $scope.grid.columnDefs[18].visible = false;
            $scope.grid.columnDefs[19].visible = false;
            $scope.grid.columnDefs[20].visible = false;
            $scope.grid.columnDefs[21].visible = false;
            $scope.grid.columnDefs[22].visible = false;
            $scope.grid.columnDefs[23].visible = true;
            $scope.grid.columnDefs[24].visible = true;
            $scope.grid.columnDefs[25].visible = true;


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