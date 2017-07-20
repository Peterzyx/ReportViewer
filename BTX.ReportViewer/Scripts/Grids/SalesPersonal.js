//SalesPersonal
app.controller('salesPersonalStoreCtrl', SalesStoreCtrl);
app.controller('salesPersonalLicenseeCtrl', SalesLicCtrl);

SalesStoreCtrl.$inject = ['$http', '$scope', 'GroupDropDown', 'uiGridConstants', 'GroupService','CategoryService'];
function SalesStoreCtrl($http, $scope, GroupDropDown, uiGridConstants, GroupService, CategoryService) {
    $scope.gridOptionsProducts = {
        columnDefs: [
            { name: 'Code', field: "Code", width: 100, pinnedLeft: true, cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;">{{COL_FIELD}}</div>', type: 'number' },
            { name: 'Product', field: "ProductName", width: 400, cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;">{{COL_FIELD}}</div>', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;">Total:</div>' },
            { name: 'Size', field: "UnitSizeML", width: 100, cellFilter: 'number:0', type: 'number' },
            { name: 'Listed/Delisted', field: "IsListed", width: 100, cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;">{{COL_FIELD}}</div>' },
            { name: 'Set/Subset', field: "Category", width: 200, cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;">{{COL_FIELD}}</div>' },
            
            //Promo Turn
            { name: 'TY_PromoTurn_9LCases', field: "TY_PromoTurn_9LCases", displayName: 'TY Promo Turn', width: 100, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'LY_PromoTurn_9LCases', field: "LY_PromoTurn_9LCases", displayName: 'LY Promo Turn', width: 100, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'PromoTurn_9LCases_Pct', field: "PromoTurn_9LCases_Pct", displayName: "Var %", cellFilter: "mapPercentage", width: 100, visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="PromoTurn_9LCasesPct"></span></div>' },
            { name: 'TY_PromoTurn_SalesAmount', field: "TY_PromoTurn_SalesAmount", displayName: 'TY Promo Turn', width: 100, cellFilter: 'currency:"$":2', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'LY_PromoTurn_SalesAmount', field: "LY_PromoTurn_SalesAmount", displayName: 'LY Promo Turn', width: 100, cellFilter: 'currency:"$":2', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'PromoTurn_SalesAmount_Pct', field: "PromoTurn_SalesAmount_Pct", displayName: "Var %", cellFilter: "mapPercentage", width: 100, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="PromoTurn_SalesAmountPct"></span></div>' },
            { name: 'TY_PromoTurn_Units', field: "TY_PromoTurn_Units", displayName: 'TY Promo Turn', width: 100, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'LY_PromoTurn_Units', field: "LY_PromoTurn_Units", displayName: 'LY Promo Turn', width: 100, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'PromoTurn_Units_Pct', field: "PromoTurn_Units_Pct", displayName: "Var %", cellFilter: "mapPercentage", width: 100, visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="PromoTurn_UnitsPct"></span></div>' },

            //Store 6P
            { name: 'Store 9L Cases 6P TY', field: "Store_TY_6P_9LCases", displayName: 'TY 6 Month', width: 100, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'Store 9L Cases 6P LY', field: "Store_LY_6P_9LCases", displayName: 'LY 6 Month', width: 100, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'Store_6P_9LCasesPct', field: "Store_6P_9LCasesPct", displayName: "Var %", cellFilter: "mapPercentage", width: 100, visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Store_6P_9LCasesPct"></span></div>' },
            { name: 'Store Sales 6P TY', field: "Store_TY_6P_SalesAmount", displayName: 'TY 6 Month', width: 100, cellFilter: 'currency:"$":2', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'Store Sales 6P LY', field: "Store_LY_6P_SalesAmount", displayName: 'LY 6 Month', width: 100, cellFilter: 'currency:"$":2', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'Store_6P_SalesAmountPct', field: "Store_6P_SalesAmountPct", displayName: "Var %", cellFilter: "mapPercentage", width: 100, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Store_6P_SalesAmountPct"></span></div>' },
            { name: 'Store Units 6P TY', field: "Store_TY_6P_Units", displayName: 'TY 6 Month', width: 100, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'Store Units 6P LY', field: "Store_LY_6P_Units", displayName: 'LY 6 Month', width: 100, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'Store_6P_UnitsPct', field: "Store_6P_UnitsPct", displayName: "Var %", cellFilter: "mapPercentage", width: 100, visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Store_6P_UnitsPct"></span></div>' },

            //Store 13P
            { name: 'Store 9L Cases 13P TY', field: "Store_TY_13P_9LCases", displayName: 'TY 13 Month', width: 100, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'Store 9L Cases 13P LY', field: "Store_LY_13P_9LCases", displayName: 'LY 13 Month', width: 100, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'Store_13P_9LCasesPct', field: "Store_13P_9LCasesPct", displayName: "Var %", cellFilter: "mapPercentage", width: 100, visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Store_13P_9LCasesPct"></span></div>' },
            { name: 'Store Sales 13P TY', field: "Store_TY_13P_SalesAmount", displayName: 'TY 13 Month', width: 100, cellFilter: 'currency:"$":2', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'Store Sales 13P LY', field: "Store_LY_13P_SalesAmount", displayName: 'LY 13 Month', width: 100, cellFilter: 'currency:"$":2', type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|currency:"$":0}}</div>' },
            { name: 'Store_13P_SalesAmountPct', field: "Store_13P_SalesAmountPct", displayName: "Var %", cellFilter: "mapPercentage", width: 100, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Store_13P_SalesAmountPct"></span></div>' },
            { name: 'Store Units 13P TY', field: "Store_TY_13P_Units", displayName: 'TY 13 Month', width: 100, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'Store Units 13P LY', field: "Store_LY_13P_Units", displayName: 'LY 13 Month', width: 100, cellFilter: 'number:0', visible: false, type: 'number', aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true, footerCellTemplate: '<div class="ui-grid-cell-contents">{{col.getAggregationValue()|number:0}}</div>' },
            { name: 'Store_13P_UnitsPct', field: "Store_13P_UnitsPct", displayName: "Var %", cellFilter: "mapPercentage", width: 100, visible: false, type: 'number', footerCellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><span id="Store_13P_UnitsPct"></span></div>' },
            
            //Not Defined
            { name: 'Promo Code', field: "Promo", width: 100, cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;">{{COL_FIELD}}</div>' },
            { name: 'Inventory', field: "StoreInv", width: 100, cellFilter: 'number:0', type: 'number' }

        ],
        enableSorting: true,
        enableRowSelection: true,
        enableRowHeaderSelection: true,
        enableCellSelection: false,
        enableCellEditOnFocus: false,
        enableGridMenu: true,
        exporterMenuPdf: false,
        exporterCsvFilename: 'Store Sales Products - ' + document.getElementById('SalesName').value + ' - ' + document.getElementById('SalesAccountName').value + ' - ' + document.getElementById('SalesPeriod').value + '.csv',
        exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
        showColumnFooter: true
    };

    $scope.gridOptionsCategory = {
        columnDefs: [
            { name: 'Rank', field: "xRank", width: 50, type: 'number' },
            { name: 'Category', field: "SetName", width: '*', cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;"><a ng-click="grid.appScope.categorydetails(row.entity.SetName)" style="cursor:pointer;">{{COL_FIELD}}</a></div>' },

            { name: 'TY Promo Turn', field: "SalesAmount_PromoTurn_TY", width: '*', cellFilter: 'currency:"$":2', type: 'number' },
            { name: 'LY Promo Turn', field: "SalesAmount_PromoTurn_LY", width: '*', cellFilter: 'currency:"$":2', type: 'number' },
            { name: 'SalesAmount_PromoTurn_Pct', field: "SalesAmount_PromoTurn_Pct", displayName: "Var %", width: '*', cellFilter: 'mapPercentage', type: 'number' },

            { name: 'TY 6 month', field: "SalesAmount_6P_TY", width: '*', cellFilter: 'currency:"$":2', type: 'number' },
            { name: 'LY 6 month', field: "SalesAmount_6P_LY", width: '*', cellFilter: 'currency:"$":2', type: 'number' },
            { name: 'SalesAmount_6P_Pct', field: "SalesAmount_6P_Pct", displayName: "Var %", width: '*', cellFilter: 'mapPercentage', type: 'number' },

            { name: 'TY 13 month', field: "SalesAmount_13P_TY", width: '*', cellFilter: 'currency:"$":2', type: 'number' },
            { name: 'LY 13 month', field: "SalesAmount_13P_LY", width: '*', cellFilter: 'currency:"$":2', type: 'number' },
            { name: 'SalesAmount_13P_Pct', field: "SalesAmount_13P_Pct", displayName: "Var %", width: '*', cellFilter: 'mapPercentage', type: 'number' },

            { name: 'Promo Code', field: "Promo", width: 200, visible:false },
            { name: 'Inventory', field: "StoreInv", width: 100, cellFilter: 'number:0', type: 'number' }
        ],
        enableSorting: true,
        enableRowSelection: true,
        enableRowHeaderSelection: true,
        enableCellSelection: false,
        enableCellEditOnFocus: false,
        enableGridMenu: true,
        exporterMenuPdf: false,
        exporterCsvFilename: 'Store Sales Categories - ' + document.getElementById('SalesName').value + ' - ' + document.getElementById('SalesAccountName').value + ' - ' + document.getElementById('SalesPeriod').value + '.csv',
        exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
        //expandableRowTemplate: '<div ui-grid="row.entity.subGridOptions" style="height:190px;"></div>',
        //expandableRowHeight: 200,
        ////subGridVariable will be available in subGrid scope
        //expandableRowScope: {
        //    subGridVariable: 'subGridScopeVariable'
        //}
    };

    $scope.gridOptionsProductsNotInStore = {
        columnDefs: [
            { name: 'Week', field: "Period", width: 100, pinnedLeft: true, visible: false, cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;">{{COL_FIELD}}</div>' },
            { name: 'Code', field: "ProductNumber", width: 200, type: 'number' },
            { name: 'Product', field: "ProductName", width: 400, cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;">{{COL_FIELD}}</div>' },
            { name: 'Toronto', field: "Toronto", width: 200, cellFilter: 'number:0', type: 'number' },
            { name: 'Ottawa', field: "Ottawa", width: 200, cellFilter: 'number:0', type: 'number' },
            { name: 'Whitby', field: "Whitby", width: 200, cellFilter: 'number:0', type: 'number' },
            { name: 'London', field: "London", width: 200, cellFilter: 'number:0', type: 'number' },
            { name: 'Thunder Bay', field: "ThunderBay", width: 200, cellFilter: 'number:0', type: 'number' },
            { name: 'Mississauga', field: "Mississauga", width: 200, cellFilter: 'number:0', type: 'number' },
            { name: 'Toronto Toronto', field: "TorontoToronto", width: 200, cellFilter: 'number:0', type: 'number' },
        ],
        enableSorting: true,
        enableRowSelection: true,
        enableRowHeaderSelection: true,
        enableCellSelection: false,
        enableCellEditOnFocus: false,
        enableGridMenu: true,
        exporterMenuPdf: false,
        exporterCsvFilename: 'Store Sales Available Products Not Here - ' + document.getElementById('SalesName').value + ' - ' + document.getElementById('SalesAccountName').value + ' - ' + document.getElementById('SalesPeriod').value + '.csv',
        exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location"))
    };

    $scope.gridOptionsProducts.onRegisterApi = function (gridApi) {
        $scope.gridApiProducts = gridApi;
    };

    $scope.gridOptionsCategory.onRegisterApi = function (gridApi) {
        $scope.gridApiCategory = gridApi;
    };

    $scope.gridOptionsProductsNotInStore.onRegisterApi = function (gridApi) {
        $scope.gridApiProductsNotInStore = gridApi;
    };

  
    $scope.groupdropboxitemselected = function (item) {
        GroupService.setCatGroup($scope.currentUserInfo , item)
        $scope.selectedgroup = item;  
    }

    $scope.periodkey1dropboxitemselected = function (item) {
        $scope.selectedperiodkey1 = item;
    }
    $scope.periodkey2dropboxitemselected = function (item) {
        $scope.selectedperiodkey2 = item;
    }
    $scope.periodkey3dropboxitemselected = function (item) {
        $scope.selectedperiodkey3 = item;
    }

    initUser();
    $scope.groups = GroupDropDown.groups;
    $scope.selectedgroup = (GroupService.getCatGroup($scope.currentUserInfo) != '' && GroupService.getCatGroup($scope.currentUserInfo) != null) ? GroupService.getCatGroup($scope.currentUserInfo) : GroupDropDown.groups[document.getElementById('SalesSelectedGroupId').value];


    function setProductionOption() {
        $scope.clientCatValue = CategoryService.getProductCategory($scope.currentUserPrdInfo) != '' && CategoryService.getProductCategory($scope.currentUserPrdInfo) != null ? CategoryService.getProductCategory($scope.currentUserPrdInfo) : "0";
        setParameter($scope.clientCatValue);
    }

    function setParameter(item) {    
        $scope.isclientonly = { value: item.toString() };     
    }


    $scope.addCategory = function (item) {
        setParameter(item);
        CategoryService.setProductCategory($scope.currentUserPrdInfo, item);
        return item;
    }


    init();
    setProductionOption();

    function initUser() {

        //Info used in setting the Page
        $scope.currentUser = document.getElementById('currentUserInfo').value;
        $scope.currentUserInfo = $scope.currentUser + "GroupSelected";
        $scope.currentUserPrdInfo = $scope.currentUser + "ProductCategorySelected";

    }
   

    //initial loading when rendering the page
    function init() {
        //loading   
        $scope.loadingProducts = true;
        $scope.loadingCategory = true;
        $scope.loadingProductsNotInStore = true;
        var mybody = angular.element(document).find('body');
        mybody.addClass('waiting');

        var userid = parseInt(document.getElementById('SalesUserId').value);
        var period = document.getElementById('SalesPeriod').value;
        var accountnumber = document.getElementById('SalesAccountNumber').value;

        if (period == "") {
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
                    $scope.selectedperiodkey1 = $scope.periodkeys[0];
                    $scope.selectedperiodkey2 = $scope.periodkeys[0];
                    $scope.selectedperiodkey3 = $scope.periodkeys[0];
                    period = $scope.selectedperiodkey1.label;

                    ProductsGridLoad(userid, period, accountnumber);
                    CategoriesGridLoad(userid, period, accountnumber, $scope.clientCatValue);
                    $http({
                        url: "/SalesTeamProductsNotInStore",
                        dataType: 'json',
                        method: 'GET',
                        data: '',
                        headers: {
                            "Content-Type": "application/json",
                            "X-UserId": userid,
                            "X-PeriodKey": period,
                            "X-AccountNumber": accountnumber
                        }
                    }).
                        then(
                        function (result) {
                            $scope.gridOptionsProductsNotInStore.data = result.data;
                        },
                        function (error) {
                            InternalErrorHandler(error);
                            $scope.loadingProductsNotInStore = false;
                            if (!$scope.loadingCategory && !$scope.loadingProducts)
                                mybody.removeClass('waiting');
                        }).finally(function () {
                            $scope.loadingProductsNotInStore = false;
                            if (!$scope.loadingCategory && !$scope.loadingProducts)
                                mybody.removeClass('waiting');
                        });
                },
                function (perioderror) {
                    InternalErrorHandler(perioderror);
                });
        } else {

            ProductsGridLoad(userid, period, accountnumber);
            CategoriesGridLoad(userid, period, accountnumber, $scope.clientCatValue);
            $http({
                url: "/SalesTeamProductsNotInStore",
                dataType: 'json',
                method: 'GET',
                data: '',
                headers: {
                    "Content-Type": "application/json",
                    "X-UserId": userid,
                    "X-PeriodKey": period,
                    "X-AccountNumber": accountnumber
                }
            }).
                then(
                function (result) {
                    $scope.gridOptionsProductsNotInStore.data = result.data;
                },
                function (error) {
                    InternalErrorHandler(error);
                    $scope.loadingProductsNotInStore = false;
                    if (!$scope.loadingCategory && !$scope.loadingProducts)
                        mybody.removeClass('waiting');
                }).finally(function () {
                    $scope.loadingProductsNotInStore = false;
                    if (!$scope.loadingCategory && !$scope.loadingProducts)
                        mybody.removeClass('waiting');
                });
        }
        

        

        //determine column visibility
        RefreshClick();
    }

    //Refresh button click functions
    $scope.Reload = function () {
        RefreshClick();
        var userid = parseInt(document.getElementById('SalesUserId').value);
        var period = document.getElementById('SalesPeriod').value;
        if (period == "") {
            period = $scope.selectedperiodkey1.label;
        }
        var accountnumber = document.getElementById('SalesAccountNumber').value;
        $scope.loadingProducts = true;
        var mybody = angular.element(document).find('body');
        mybody.addClass('waiting');
        ProductsGridLoad(userid, period, accountnumber);
    }

    $scope.ReloadCategory = function () {
        var userid = parseInt(document.getElementById('SalesUserId').value);
        var period = document.getElementById('SalesPeriod').value;
        if (period == "") {
            period = $scope.selectedperiodkey2.label;
        }
        var accountnumber = document.getElementById('SalesAccountNumber').value;
        $scope.loadingCategory = true;
        var mybody = angular.element(document).find('body');
        mybody.addClass('waiting');
        CategoriesGridLoad(userid, period, accountnumber, $scope.addCategory($scope.isclientonly.value));
    }

    $scope.ReloadAvailable = function () {
        var period = document.getElementById('SalesPeriod').value;
        if (period == "") {
            period = $scope.selectedperiodkey3.label;
        }
        var userid = parseInt(document.getElementById('SalesUserId').value);
        var accountnumber = document.getElementById('SalesAccountNumber').value;

        $scope.loadingProductsNotInStore = true;
        var mybody = angular.element(document).find('body');
        mybody.addClass('waiting');

        $http({
            url: "/SalesTeamProductsNotInStore",
            dataType: 'json',
            method: 'GET',
            data: '',
            headers: {
                "Content-Type": "application/json",
                "X-UserId": userid,
                "X-PeriodKey": period,
                "X-AccountNumber": accountnumber
            }
        }).
            then(
            function (result) {
                $scope.gridOptionsProductsNotInStore.data = result.data;
            },
            function (error) {
                InternalErrorHandler(error);
                $scope.loadingProductsNotInStore = false;
                if (!$scope.loadingCategory && !$scope.loadingProducts)
                    mybody.removeClass('waiting');
            }).finally(function () {
                $scope.loadingProductsNotInStore = false;
                if (!$scope.loadingCategory && !$scope.loadingProducts)
                    mybody.removeClass('waiting');
            });
    }

    //Refresh Products
    function ProductsGridLoad(userid, period, accountnumber) {
        var mybody = angular.element(document).find('body');
        $http({
            url: "/SalesTeamStoreProduct",
            dataType: 'json',
            method: 'GET',
            data: '',
            headers: {
                "Content-Type": "application/json",
                "X-UserId": userid,
                "X-PeriodKey": period,
                "X-AccountNumber": accountnumber
            }
        }).
            then(
            function (result) {
                var len = result.data.length;
                $scope.gridOptionsProducts.data = result.data.slice(1, len);
                //assign footer pct
                var nineLitreElem = document.getElementById("PromoTurn_9LCasesPct");
                if (nineLitreElem)
                    nineLitreElem.textContent = (result.data[0].PromoTurn_9LCases_Pct * 100).toFixed(2).toString() + "%";
                var salesAmtElem = document.getElementById("PromoTurn_SalesAmountPct");
                if (salesAmtElem)
                    salesAmtElem.textContent = (result.data[0].PromoTurn_SalesAmount_Pct * 100).toFixed(2).toString() + "%";
                var unitElem = document.getElementById("PromoTurn_UnitsPct");
                if (unitElem)
                    unitElem.textContent = (result.data[0].PromoTurn_Units_Pct * 100).toFixed(2).toString() + "%";
                nineLitreElem = document.getElementById("Store_6P_9LCasesPct");
                if (nineLitreElem)
                    nineLitreElem.textContent = (result.data[0].Store_6P_9LCasesPct * 100).toFixed(2).toString() + "%";
                var salesAmtElem = document.getElementById("Store_6P_SalesAmountPct");
                if (salesAmtElem)
                    salesAmtElem.textContent = (result.data[0].Store_6P_SalesAmountPct * 100).toFixed(2).toString() + "%";
                var unitElem = document.getElementById("Store_6P_UnitsPct");
                if (unitElem)
                    unitElem.textContent = (result.data[0].Store_6P_UnitsPct * 100).toFixed(2).toString() + "%";
                nineLitreElem = document.getElementById("Store_13P_9LCasesPct");
                if (nineLitreElem)
                    nineLitreElem.textContent = (result.data[0].Store_13P_9LCasesPct * 100).toFixed(2).toString() + "%";
                salesAmtElem = document.getElementById("Store_13P_SalesAmountPct");
                if (salesAmtElem)
                    salesAmtElem.textContent = (result.data[0].Store_13P_SalesAmountPct * 100).toFixed(2).toString() + "%";
                unitElem = document.getElementById("Store_13P_UnitsPct");
                if (unitElem)
                    unitElem.textContent = (result.data[0].Store_13P_UnitsPct * 100).toFixed(2).toString() + "%";
            },
            function (error) {
                InternalErrorHandler(error);
                $scope.loadingProducts = false;
                if (!$scope.loadingCategory && !$scope.loadingProductsNotInStore)
                    mybody.removeClass('waiting');
            }).finally(function () {
                $scope.loadingProducts = false;
                if (!$scope.loadingCategory && !$scope.loadingProductsNotInStore)
                    mybody.removeClass('waiting');
            });
    }

    //Refresh Categories
    function CategoriesGridLoad(userid, period, accountnumber, isclientonly) {
        var mybody = angular.element(document).find('body');
        $http({
            url: "/SalesTeamStoreCategory",
            dataType: 'json',
            method: 'GET',
            data: '',
            headers: {
                "Content-Type": "application/json",
                "X-UserId": userid,
                "X-PeriodKey": period,
                "X-AccountNumber": accountnumber,
                "X-ClientOnly": isclientonly
            }
        }).
            then(
            function (result) {
                for (var i = 0; i < result.data.length; i++) {
                    result.data[i].subGridOptions = {
                        appScopeProvider: $scope,
                        columnDefs: [
                            { name: 'Product Rank', field: "xRankSetByProduct", width: '*', type: 'number', visible: false },
                            { name: 'Code', field: "CSPC", width: 100 },
                            { name: 'Product', field: "ProductName", width: 200, cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;">{{COL_FIELD}}</div>' },
                            { name: 'Size', field: "UnitSizeML", width: 100 },
                            { name: 'List/Delist', field: "IsListed", width: 100, cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;">{{COL_FIELD}}</div>' },
                            { name: 'Set/Subset', field: "SetName", width: 120, cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;">{{COL_FIELD}}</div>' },

                            { name: 'TY Promo turn (last turn)', field: "TY_Promo_Turn", width: 100 },
                            { name: 'LY Promo turn', field: "LY_Promo_Turn", width: 100 },

                            { name: 'TY 6 month', field: "SalesAmount_6P_TY", width: '*', cellFilter: 'currency:"$":2', type: 'number' },
                            { name: 'LY 6 month', field: "SalesAmount_6P_LY", width: '*', cellFilter: 'currency:"$":2', type: 'number' },
                            { name: 'SalesAmount_6P_Pct', field: "SalesAmount_6P_Pct", displayName: "Var %", width: '*', cellFilter: 'mapPercentage', type: 'number' },
                            { name: 'Store Sales 13P TY', field: "SalesAmount_13P_TY", width: '*', cellFilter: 'currency:"$":2', type: 'number' },
                            { name: 'Store Sales 13P LY', field: "SalesAmount_13P_LY", width: '*', cellFilter: 'currency:"$":2', type: 'number' },
                            { name: 'SalesAmount_13P_Pct', field: "SalesAmount_13P_Pct", displayName: "Var %", width: '*', cellFilter: 'mapPercentage', type: 'number' },

                            { name: 'Promotion Code', field: "Promo", width: 200 },
                            { name: 'Inventory', field: "InventoryCount", width: 100, cellFilter: 'number:0', type: 'number' }
                        ],
                        data: result.data[i].Details
                    }
                }
                $scope.gridOptionsCategory.data = result.data;
            },
            function (error) {
                InternalErrorHandler(error);
                $scope.loadingCategory = false;
                if (!$scope.loadingProducts && !$scope.loadingProductsNotInStore)
                    mybody.removeClass('waiting');
            }).finally(function () {
                $scope.loadingCategory = false;
                if (!$scope.loadingProducts && !$scope.loadingProductsNotInStore)
                    mybody.removeClass('waiting');
            });
    }

    function RefreshClick() {

        //update ColDefs
        if ($scope.selectedgroup.id === 0) {

            //Products
            $scope.gridOptionsProducts.columnDefs[5].visible = true;
            $scope.gridOptionsProducts.columnDefs[6].visible = true;
            $scope.gridOptionsProducts.columnDefs[7].visible = true;
            $scope.gridOptionsProducts.columnDefs[8].visible = false;
            $scope.gridOptionsProducts.columnDefs[9].visible = false;
            $scope.gridOptionsProducts.columnDefs[10].visible = false;
            $scope.gridOptionsProducts.columnDefs[11].visible = false;
            $scope.gridOptionsProducts.columnDefs[12].visible = false;
            $scope.gridOptionsProducts.columnDefs[13].visible = false;

            $scope.gridOptionsProducts.columnDefs[14].visible = true;
            $scope.gridOptionsProducts.columnDefs[15].visible = true;
            $scope.gridOptionsProducts.columnDefs[16].visible = true;
            $scope.gridOptionsProducts.columnDefs[17].visible = false;
            $scope.gridOptionsProducts.columnDefs[18].visible = false;
            $scope.gridOptionsProducts.columnDefs[19].visible = false;
            $scope.gridOptionsProducts.columnDefs[20].visible = false;
            $scope.gridOptionsProducts.columnDefs[21].visible = false;
            $scope.gridOptionsProducts.columnDefs[22].visible = false;

            $scope.gridOptionsProducts.columnDefs[23].visible = true;
            $scope.gridOptionsProducts.columnDefs[24].visible = true;
            $scope.gridOptionsProducts.columnDefs[25].visible = true;
            $scope.gridOptionsProducts.columnDefs[26].visible = false;
            $scope.gridOptionsProducts.columnDefs[27].visible = false;
            $scope.gridOptionsProducts.columnDefs[28].visible = false;
            $scope.gridOptionsProducts.columnDefs[29].visible = false;
            $scope.gridOptionsProducts.columnDefs[30].visible = false;
            $scope.gridOptionsProducts.columnDefs[31].visible = false;

        } else if ($scope.selectedgroup.id === 1) {

            //Products
            $scope.gridOptionsProducts.columnDefs[5].visible = false;
            $scope.gridOptionsProducts.columnDefs[6].visible = false;
            $scope.gridOptionsProducts.columnDefs[7].visible = false;
            $scope.gridOptionsProducts.columnDefs[8].visible = true;
            $scope.gridOptionsProducts.columnDefs[9].visible = true;
            $scope.gridOptionsProducts.columnDefs[10].visible = true;
            $scope.gridOptionsProducts.columnDefs[11].visible = false;
            $scope.gridOptionsProducts.columnDefs[12].visible = false;
            $scope.gridOptionsProducts.columnDefs[13].visible = false;

            $scope.gridOptionsProducts.columnDefs[14].visible = false;
            $scope.gridOptionsProducts.columnDefs[15].visible = false;
            $scope.gridOptionsProducts.columnDefs[16].visible = false;
            $scope.gridOptionsProducts.columnDefs[17].visible = true;
            $scope.gridOptionsProducts.columnDefs[18].visible = true;
            $scope.gridOptionsProducts.columnDefs[19].visible = true;
            $scope.gridOptionsProducts.columnDefs[20].visible = false;
            $scope.gridOptionsProducts.columnDefs[21].visible = false;
            $scope.gridOptionsProducts.columnDefs[22].visible = false;

            $scope.gridOptionsProducts.columnDefs[23].visible = false;
            $scope.gridOptionsProducts.columnDefs[24].visible = false;
            $scope.gridOptionsProducts.columnDefs[25].visible = false;
            $scope.gridOptionsProducts.columnDefs[26].visible = true;
            $scope.gridOptionsProducts.columnDefs[27].visible = true;
            $scope.gridOptionsProducts.columnDefs[28].visible = true;
            $scope.gridOptionsProducts.columnDefs[29].visible = false;
            $scope.gridOptionsProducts.columnDefs[30].visible = false;
            $scope.gridOptionsProducts.columnDefs[31].visible = false;

        } else if ($scope.selectedgroup.id === 2) {

            //Products
            $scope.gridOptionsProducts.columnDefs[5].visible = false;
            $scope.gridOptionsProducts.columnDefs[6].visible = false;
            $scope.gridOptionsProducts.columnDefs[7].visible = false;
            $scope.gridOptionsProducts.columnDefs[8].visible = false;
            $scope.gridOptionsProducts.columnDefs[9].visible = false;
            $scope.gridOptionsProducts.columnDefs[10].visible = false;
            $scope.gridOptionsProducts.columnDefs[11].visible = true;
            $scope.gridOptionsProducts.columnDefs[12].visible = true;
            $scope.gridOptionsProducts.columnDefs[13].visible = true;

            $scope.gridOptionsProducts.columnDefs[14].visible = false;
            $scope.gridOptionsProducts.columnDefs[15].visible = false;
            $scope.gridOptionsProducts.columnDefs[16].visible = false;
            $scope.gridOptionsProducts.columnDefs[17].visible = false;
            $scope.gridOptionsProducts.columnDefs[18].visible = false;
            $scope.gridOptionsProducts.columnDefs[19].visible = false;
            $scope.gridOptionsProducts.columnDefs[20].visible = true;
            $scope.gridOptionsProducts.columnDefs[21].visible = true;
            $scope.gridOptionsProducts.columnDefs[22].visible = true;

            $scope.gridOptionsProducts.columnDefs[23].visible = false;
            $scope.gridOptionsProducts.columnDefs[24].visible = false;
            $scope.gridOptionsProducts.columnDefs[25].visible = false;
            $scope.gridOptionsProducts.columnDefs[26].visible = false;
            $scope.gridOptionsProducts.columnDefs[27].visible = false;
            $scope.gridOptionsProducts.columnDefs[28].visible = false;
            $scope.gridOptionsProducts.columnDefs[29].visible = true;
            $scope.gridOptionsProducts.columnDefs[30].visible = true;
            $scope.gridOptionsProducts.columnDefs[31].visible = true;

        }
        if ($scope.gridApiProducts) {
            $scope.gridApiProducts.grid.refresh();
        }
    }



    $scope.categorydetails = function (category) {

        var isclientonly = parseInt($scope.addCategory($scope.isclientonly.value));
        var period = document.getElementById('SalesPeriod').value;
        if (period == "") {
            period = $scope.selectedperiodkey2.label;
        }
        var headeroption = parseInt(document.getElementById('HeaderOption').value);
        var accountnumber = document.getElementById('SalesAccountNumber').value;
        var accountname = document.getElementById('SalesAccountName').value;

        //create a dynamic form
        var f = document.createElement("form");
        f.setAttribute('id', "categorydetails");
        f.setAttribute('method', "post");
        f.setAttribute('action', document.getElementById('CategoryDetailsUrl').value);
        //target blank will post it to a new tab
        f.setAttribute("target", "_blank");
        //append the form to the bottom of the body
        document.body.appendChild(f);
        //create hidden elements and append them to the form
        f.appendChild(GroupDropDown.createFormElement("category", category));
        f.appendChild(GroupDropDown.createFormElement("accountnumber", accountnumber));
        f.appendChild(GroupDropDown.createFormElement("accountname", accountname));
        f.appendChild(GroupDropDown.createFormElement("period", period));
        f.appendChild(GroupDropDown.createFormElement("isclientonly", isclientonly));
        if (headeroption === 2) {
            f.appendChild(GroupDropDown.createFormElement("headeroption", headeroption));
        }
        //submit form
        f.submit();
        //remove the newly created form after submit
        f.remove();
    }
}

SalesLicCtrl.$inject = ['$http', '$scope', 'GroupDropDown', 'uiGridConstants'];
function SalesLicCtrl($http, $scope, GroupDropDown, uiGridConstants) {

    var currentperiod = { name: document.getElementById('SalesPeriod').value, visible: true };
    if (currentperiod.name == "") {
        currentperiod = { name: '2017-P3', visible: true };
    }
    var periodlist = [currentperiod];
    for (var i = 0; i < 12; ++i) {
        currentperiod = { name: previousperiod(currentperiod.name), visible: true };
        periodlist.push(currentperiod);
    }

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
            { name: 'Category', field: "MyCategory", width: 50, visible: false, pinnedLeft: true, cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;">{{COL_FIELD}}</div>' },
            { name: 'Code', field: "CSPC", width: 100, pinnedLeft: true, cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;">{{COL_FIELD}}</div>', type: 'number' },
            { name: 'Product', field: "ProductName", width: 200, pinnedLeft: true, cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;">{{COL_FIELD}}</div>' },
            { name: 'Size', field: "UnitSizeML", width: 100, cellFilter: 'number:0', type: 'number', pinnedLeft: true },
            { name: 'Price', field: "Price", width: 100, pinnedLeft: true },

            { name: 'LCBO Sales Cur Per', field: "LCBO_Sales_CP", displayName: 'LCBO', width: 100, cellFilter: 'currency:"$":2', type: 'number', category: periodlist[0].name },
            { name: 'Direct Sales Cur Per', field: "Direct_Sales_CP", displayName: 'DD', width: 100, cellFilter: 'currency:"$":2', type: 'number', category: periodlist[0].name },

            { name: 'LCBO Sales 1 Per', field: "LCBO_Sales_1P", displayName: 'LCBO', width: 100, cellFilter: 'currency:"$":2', type: 'number', category: periodlist[1].name },
            { name: 'Direct Sales 1 Per', field: "Direct_Sales_1P", displayName: 'DD', width: 100, cellFilter: 'currency:"$":2', type: 'number', category: periodlist[1].name },

            { name: 'LCBO Sales 2 Per', field: "LCBO_Sales_2P", displayName: 'LCBO', width: 100, cellFilter: 'currency:"$":2', type: 'number', category: periodlist[2].name },
            { name: 'Direct Sales 2 Per', field: "Direct_Sales_2P", displayName: 'DD', width: 100, cellFilter: 'currency:"$":2', type: 'number', category: periodlist[2].name },

            { name: 'LCBO Sales 3 Per', field: "LCBO_Sales_3P", displayName: 'LCBO', width: 100, cellFilter: 'currency:"$":2', type: 'number', category: periodlist[3].name },
            { name: 'Direct Sales 3 Per', field: "Direct_Sales_3P", displayName: 'DD', width: 100, cellFilter: 'currency:"$":2', type: 'number', category: periodlist[3].name },

            { name: 'LCBO Sales 4 Per', field: "LCBO_Sales_4P", displayName: 'LCBO', width: 100, cellFilter: 'currency:"$":2', type: 'number', category: periodlist[4].name },
            { name: 'Direct Sales 4 Per', field: "Direct_Sales_4P", displayName: 'DD', width: 100, cellFilter: 'currency:"$":2', type: 'number', category: periodlist[4].name },

            { name: 'LCBO Sales 5 Per', field: "LCBO_Sales_5P", displayName: 'LCBO', width: 100, cellFilter: 'currency:"$":2', type: 'number', category: periodlist[5].name },
            { name: 'Direct Sales 5 Per', field: "Direct_Sales_5P", displayName: 'DD', width: 100, cellFilter: 'currency:"$":2', type: 'number', category: periodlist[5].name },

            { name: 'LCBO Sales 6 Per', field: "LCBO_Sales_6P", displayName: 'LCBO', width: 100, cellFilter: 'currency:"$":2', type: 'number', category: periodlist[6].name },
            { name: 'Direct Sales 6 Per', field: "Direct_Sales_6P", displayName: 'DD', width: 100, cellFilter: 'currency:"$":2', type: 'number', category: periodlist[6].name },

            { name: 'LCBO Sales 7 Per', field: "LCBO_Sales_7P", displayName: 'LCBO', width: 100, cellFilter: 'currency:"$":2', type: 'number', category: periodlist[7].name },
            { name: 'Direct Sales 7 Per', field: "Direct_Sales_7P", displayName: 'DD', width: 100, cellFilter: 'currency:"$":2', type: 'number', category: periodlist[7].name },

            { name: 'LCBO Sales 8 Per', field: "LCBO_Sales_8P", displayName: 'LCBO', width: 100, cellFilter: 'currency:"$":2', type: 'number', category: periodlist[8].name },
            { name: 'Direct Sales 8 Per', field: "Direct_Sales_8P", displayName: 'DD', width: 100, cellFilter: 'currency:"$":2', type: 'number', category: periodlist[8].name },

            { name: 'LCBO Sales 9 Per', field: "LCBO_Sales_9P", displayName: 'LCBO', width: 100, cellFilter: 'currency:"$":2', type: 'number', category: periodlist[9].name },
            { name: 'Direct Sales 9 Per', field: "Direct_Sales_9P", displayName: 'DD', width: 100, cellFilter: 'currency:"$":2', type: 'number', category: periodlist[9].name },

            { name: 'LCBO Sales 10 Per', field: "LCBO_Sales_10P", displayName: 'LCBO', width: 100, cellFilter: 'currency:"$":2', type: 'number', category: periodlist[10].name },
            { name: 'Direct Sales 10 Per', field: "Direct_Sales_10P", displayName: 'DD', width: 100, cellFilter: 'currency:"$":2', type: 'number', category: periodlist[10].name },

            { name: 'LCBO Sales 11 Per', field: "LCBO_Sales_11P", displayName: 'LCBO', width: 100, cellFilter: 'currency:"$":2', type: 'number', category: periodlist[11].name },
            { name: 'Direct Sales 11 Per', field: "Direct_Sales_11P", displayName: 'DD', width: 100, cellFilter: 'currency:"$":2', type: 'number', category: periodlist[11].name },

            { name: 'LCBO Sales 12 Per', field: "LCBO_Sales_12P", displayName: 'LCBO', width: 100, cellFilter: 'currency:"$":2', type: 'number', category: periodlist[12].name },
            { name: 'Direct Sales 12 Per', field: "Direct_Sales_12P", displayName: 'DD', width: 100, cellFilter: 'currency:"$":2', type: 'number', category: periodlist[12].name }
        ],
        onRegisterApi: function (gridApi) {
            $scope.gridApi = gridApi;
        },
        enableSorting: true,
        enableRowSelection: true,
        enableRowHeaderSelection: true,
        enableCellSelection: false,
        enableCellEditOnFocus: false,
        enableGridMenu: true,
        exporterMenuPdf: false,
        exporterCsvFilename: 'Licensee Sales Details - ' + document.getElementById('SalesAccountName').value + ' - ' + document.getElementById('SalesPeriod').value + '.csv',
        exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
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

    $scope.periodkeydropboxitemselected = function (item) {
        $scope.selectedperiodkey = item;
    }

    init();
    
    //initial loading when rendering the page
    function init() {
        //loading
        $scope.loading = true;
        var mybody = angular.element(document).find('body');
        mybody.addClass('waiting');

        var period = document.getElementById('SalesPeriod').value;
        var accountname = document.getElementById('SalesAccountName').value;

        if (period == "") {
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
                    $scope.selectedperiodkey = $.grep($scope.periodkeys, function (e) { return e.id == '2017-P3' })[0];
                    period = $scope.selectedperiodkey.label;

                    $http({
                        url: "/MyCategories",
                        dataType: 'json',
                        method: 'POST',
                        data: '',
                        headers: {
                            "Content-Type": "application/json"
                        }
                    }).
                        then(
                        function (categoriesresult) {
                            $scope.categoriesdata = categoriesresult.data;
                            $scope.categoriesmodel = JSON.parse(JSON.stringify($scope.categoriesdata));

                            $http({
                                url: "/GetLicenseeDetails",
                                dataType: 'json',
                                method: 'GET',
                                data: '',
                                headers: {
                                    "Content-Type": "application/json",
                                    "X-PeriodKey": period,
                                    "X-AccountName": accountname
                                }
                            }).
                                then(
                                function (result) {
                                    $scope.gridOptions.data = result.data;
                                    $scope.dummyData = $scope.gridOptions.data;
                                },
                                function (error) {
                                    InternalErrorHandler(error);
                                    $scope.loading = false;
                                    mybody.removeClass('waiting');
                                }).finally(function () {
                                    $scope.loading = false;
                                    mybody.removeClass('waiting');
                                    $scope.gridApi.grid.refresh();
                                });
                        },
                        function (categorieserror) {
                            InternalErrorHandler(categorieserror);
                        });
                },
                function (perioderror) {
                    InternalErrorHandler(perioderror);
                });
        } else {
            $http({
                url: "/MyCategories",
                dataType: 'json',
                method: 'POST',
                data: '',
                headers: {
                    "Content-Type": "application/json"
                }
            }).
                then(
                function (categoriesresult) {
                    $scope.categoriesdata = categoriesresult.data;
                    $scope.categoriesmodel = JSON.parse(JSON.stringify($scope.categoriesdata));

                    $http({
                        url: "/GetLicenseeDetails",
                        dataType: 'json',
                        method: 'GET',
                        data: '',
                        headers: {
                            "Content-Type": "application/json",
                            "X-PeriodKey": period,
                            "X-AccountName": accountname
                        }
                    }).
                        then(
                        function (result) {
                            $scope.gridOptions.data = result.data;
                            $scope.dummyData = $scope.gridOptions.data;
                        },
                        function (error) {
                            InternalErrorHandler(error);
                            $scope.loading = false;
                            mybody.removeClass('waiting');
                        }).finally(function () {
                            $scope.loading = false;
                            mybody.removeClass('waiting');
                            $scope.gridApi.grid.refresh();
                        });
                },
                function (categorieserror) {
                    InternalErrorHandler(categorieserror);
                });
        }
        
    };

    $scope.Update = {
        onSelectionChanged: function () {
            var selecteddata = [];
            for (var i = 0; i < $scope.categoriesmodel.length; ++i) {
                selecteddata.push($scope.categoriesmodel[i].id);
            }
            
            $scope.gridOptions.data = [];
            for (var i = 0; i < $scope.dummyData.length; ++i) {
                if ($.inArray($scope.dummyData[i].MyCategory, selecteddata) != -1) {
                    $scope.gridOptions.data.push($scope.dummyData[i]);
                }
            }
        }
    };
    
    function previousperiod(period) {
        var array = period.split('-P');
        var output;
        if (array[1] == 1) {
            output = (parseInt(array[0]) - 1).toString() + '-P12';
        } else {
            output = array[0] + '-P' + (parseInt(array[1]) - 1).toString();
        }
        return output;
    };

    //Refresh button click functions
    $scope.Reload = function () {
        var period = $scope.selectedperiodkey.label;
        var accountname = document.getElementById('SalesAccountName').value;
        $scope.loading = true;
        var mybody = angular.element(document).find('body');
        mybody.addClass('waiting');

        var currentperiod = { name: period, visible: true };
        
        var periodlist = [currentperiod];
        $scope.gridOptions.columnDefs[5].category = periodlist[0].name;
        $scope.gridOptions.columnDefs[6].category = periodlist[0].name;
        for (var i = 0; i < 12; ++i) {
            currentperiod = { name: previousperiod(currentperiod.name), visible: true };
            periodlist.push(currentperiod);
            $scope.gridOptions.columnDefs[i*2+7].category = periodlist[i+1].name;
            $scope.gridOptions.columnDefs[i*2+8].category = periodlist[i+1].name;
        }
        $scope.gridOptions.category = periodlist;
        
        $http({
            url: "/MyCategories",
            dataType: 'json',
            method: 'POST',
            data: '',
            headers: {
                "Content-Type": "application/json"
            }
        }).
            then(
            function (categoriesresult) {
                $scope.categoriesdata = categoriesresult.data;
                $scope.categoriesmodel = JSON.parse(JSON.stringify($scope.categoriesdata));

                $http({
                    url: "/GetLicenseeDetails",
                    dataType: 'json',
                    method: 'GET',
                    data: '',
                    headers: {
                        "Content-Type": "application/json",
                        "X-PeriodKey": period,
                        "X-AccountName": accountname
                    }
                }).
                    then(
                    function (result) {
                        $scope.gridOptions.data = result.data;
                        $scope.dummyData = $scope.gridOptions.data;
                    },
                    function (error) {
                        InternalErrorHandler(error);
                        $scope.loading = false;
                        mybody.removeClass('waiting');
                    }).finally(function () {
                        $scope.loading = false;
                        mybody.removeClass('waiting');
                        $scope.gridApi.grid.refresh();
                    });
            },
            function (categorieserror) {
                InternalErrorHandler(categorieserror);
            });
    }
}