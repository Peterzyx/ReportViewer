//WeeklySalesOntario

app.controller('LandingCtrl', ['$scope', '$http', '$filter', 'GroupDropDown', 'GroupService', function ($scope, $http, $filter, GroupDropDown, GroupService) {
    //var today = new Date();
    $scope.gridLCBO = {
        enableFiltering: false,
        onRegisterApi: function (gridApi) {
            $scope.gridLCBOApi = gridApi

        },
        columnDefs: [
            { name: 'Store Number', field: "StoreNumber", width: '7%', cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><a ng-click="grid.appScope.salespersonalstore(row.entity.StoreNumber, row.entity.StoreName)" style="cursor:pointer;">{{COL_FIELD}}</a></div>' },
            { name: 'StoreName', field: "StoreName", width: '*', cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;"><a ng-click="grid.appScope.salespersonalstore(row.entity.StoreNumber, row.entity.StoreName)" style="cursor:pointer;">{{COL_FIELD}}</a></div>' },
            { name: 'City', field: "City", width: '*', cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;">{{COL_FIELD}}</div>' },
            { name: 'Address', field: "Address", width: '*', cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;">{{COL_FIELD}}</div>' },
            { name: 'PostalCode', field: "PostalCode", width: '*', cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;">{{COL_FIELD}}</div>' },
            { name: 'Region', field: "Region", width: '*', cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;">{{COL_FIELD}}</div>' }
        ],
        enableSorting: true,
        enableRowSelection: true,
        enableRowHeaderSelection: true,
        enableCellSelection: false,
        enableCellEditOnFocus: false,
        enableGridMenu: true,
        exporterMenuPdf: false
    };

    $scope.gridLic = {
        enableFiltering: false,
        onRegisterApi: function (gridApi) {
            $scope.gridLicApi = gridApi;
        },
        columnDefs: [
            { name: 'Licensee Number', field: "StoreNumber", width: '7%', cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><a ng-click="grid.appScope.salespersonallicensee(row.entity.StoreNumber, row.entity.StoreName)" style="cursor:pointer;">{{COL_FIELD}}</a></div>' },
            { name: 'Licensee Name', field: "StoreName", width: '*', cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;"><a ng-click="grid.appScope.salespersonallicensee(row.entity.StoreNumber, row.entity.StoreName)" style="cursor:pointer;">{{COL_FIELD}}</a></div>' },
            { name: 'City', field: "City", width: '*', cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;">{{COL_FIELD}}</div>' },
            { name: 'Address', field: "Address", width: '*', cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;">{{COL_FIELD}}</div>' },
            { name: 'PostalCode', field: "PostalCode", width: '*', cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;">{{COL_FIELD}}</div>' },
            { name: 'Region', field: "Region", width: '*', cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;">{{COL_FIELD}}</div>' }
        ],
        enableSorting: true,
        enableRowSelection: true,
        enableRowHeaderSelection: true,
        enableCellSelection: false,
        enableCellEditOnFocus: false,
        enableGridMenu: true,
        exporterMenuPdf: false
    };

    $scope.gridCSPC = {
        enableFiltering: false,
        onRegisterApi: function (gridApi) {
            $scope.gridCSPCApi = gridApi;
        },
        columnDefs: [
            { name: 'CSPC', field: "CSPC", width: '7%', cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:right;"><a ng-click="grid.appScope.storelicenseeforproduct(row.entity.CSPC, row.entity.ProductName)" style="cursor:pointer;">{{COL_FIELD}}</a></div>' },
            { name: 'ProductName', field: "ProductName", width: '*', cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;"><a ng-click="grid.appScope.storelicenseeforproduct(row.entity.CSPC, row.entity.ProductName)" style="cursor:pointer;">{{COL_FIELD}}</a></div>' },
            { name: 'Category', field: "Category", width: '*', cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;">{{COL_FIELD}}</div>' },
            { name: 'Subcategory', field: "Subcategory", width: '*', cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;">{{COL_FIELD}}</div>' },
            { name: 'Brand', field: "Brand", width: '*', cellTemplate: '<div class="ui-grid-cell-contents" style="text-align:left;">{{COL_FIELD}}</div>' },
        ],
        enableSorting: true,
        enableRowSelection: true,
        enableRowHeaderSelection: true,
        enableCellSelection: false,
        enableCellEditOnFocus: false,
        enableGridMenu: true,
        exporterMenuPdf: false
    };

    var dummyLCBO = [];
    var dummyLic = [];
    var dummyCSPC = [];
    $scope.searchTextLCBO = "";
    $scope.searchTextLic = "";
    $scope.searchTextCSPC = "";
   
    
    init();
   

    //dropdown
    //$scope.periodkeydropboxitemselected = function (item) {
    //    $scope.selectedperiodkey = item;
    //}

    //$scope.groups = GroupDropDown.groups;
    //$scope.selectedgroup = (GroupService.getCatGroup($scope.currentUserInfo) != '' && GroupService.getCatGroup($scope.currentUserInfo) != null) ? GroupService.getCatGroup($scope.currentUserInfo) : GroupDropDown.groups[1];

    //$scope.groupdropboxitemselected = function (item) {
    //    GroupService.setCatGroup($scope.currentUserInfo, item)
    //    $scope.selectedgroup = item;   
    //}

    //initial loading when rendering the page
    function init() {

        //cursor & page loading
        $scope.loadingLCBO = true;
        $scope.loadingLic = true;
        $scope.loadingCSPC = true;
       
        //Info used in setting the Page
        var currentUser = document.getElementById('currentUserInfo').value;
        $scope.currentUserInfo = currentUser + "GroupSelected";

        $http({
            url: "/Search",
            dataType: 'json',
            method: 'GET'
        }).
            then(
            function (result) {
                $scope.gridLCBO.data = result.data;
                dummyLCBO = result.data;
             
            },
            function (error) {
                InternalErrorHandler(error);
            }).
            finally(function () {
            })

        $http({
            url: "/LicSearch",
            dataType: 'json',
            method: 'GET'
        }).
            then(
            function (result) {
                $scope.gridLic.data = result.data;
                dummyLic = result.data;
             
            },
            function (error) {
                InternalErrorHandler(error);
            }).
            finally(function () {
            })

        $http({
            url: "/CSPCSearch",
            dataType: 'json',
            method: 'GET'
        }).
            then(
            function (result) {
                $scope.gridCSPC.data = result.data;
                dummyCSPC = result.data;
              
            },
            function (error) {
                InternalErrorHandler(error);
            }).
            finally(function () {
            })

    }


    $scope.refreshDataLCBO = function () {
        $scope.loadingLCBO = false;
        $scope.gridLCBO.data = $filter('filter')(dummyLCBO, $scope.searchTextLCBO);
        localStorage.setItem("StoreNumber", $scope.searchTextLCBO);
    }

    $scope.refreshDataLic = function () {
        $scope.loadingLic = false;
        $scope.gridLic.data = $filter('filter')(dummyLic, $scope.searchTextLic);
        localStorage.setItem("LicenseNo", $scope.searchTextLic);
    }

    $scope.refreshDataCSPC = function () {
        $scope.loadingCSPC = false;
        $scope.gridCSPC.data = $filter('filter')(dummyCSPC, $scope.searchTextCSPC);
        localStorage.setItem("CSPCNo", $scope.searchTextCSPC);
    }

    //subgrid control for each store
    $scope.salespersonalstore = function (storenumber, storename) {
        
        var headeroption = parseInt(document.getElementById('HeaderOption').value);
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
        f.appendChild(GroupDropDown.createFormElement("userid", 0));
        f.appendChild(GroupDropDown.createFormElement("period", ""));
        f.appendChild(GroupDropDown.createFormElement("accountnumber", storenumber));
        f.appendChild(GroupDropDown.createFormElement("salesname", ""));
        f.appendChild(GroupDropDown.createFormElement("accountname", storename));
        f.appendChild(GroupDropDown.createFormElement("selectedgroupid", 0));
        if (headeroption === 2) {
            f.appendChild(GroupDropDown.createFormElement("headeroption", headeroption));
        }
        //submit form
        f.submit();
        //remove the newly created form after submit
        f.remove();
    }

    //subgrid control for each licensee
    $scope.salespersonallicensee = function (licenseenumber, licenseename) {
        var headeroption = parseInt(document.getElementById('HeaderOption').value);

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
        f.appendChild(GroupDropDown.createFormElement("userid", 0));
        f.appendChild(GroupDropDown.createFormElement("period", ""));
        f.appendChild(GroupDropDown.createFormElement("accountnumber", licenseenumber));
        f.appendChild(GroupDropDown.createFormElement("salesname", ""));
        f.appendChild(GroupDropDown.createFormElement("accountname", licenseename));
        f.appendChild(GroupDropDown.createFormElement("selectedgroupid", 0));
        if (headeroption === 2) {
            f.appendChild(GroupDropDown.createFormElement("headeroption", headeroption));
        }
        //submit form
        f.submit();
        //remove the newly created form after submit
        f.remove();
    }

    $scope.storelicenseeforproduct = function (cspc, productname) {
        var headeroption = parseInt(document.getElementById('HeaderOption').value);

        //create a dynamic form
        var f = document.createElement("form");
        f.setAttribute('id', "storelicenseeforproductsubgridform");
        f.setAttribute('method', "post");
        f.setAttribute('action', document.getElementById('StoreLicenseeForProductUrl').value);
        //target blank will post it to a new tab
        f.setAttribute("target", "_blank");
        //append the form to the bottom of the body
        document.body.appendChild(f);
        //create hidden elements and append them to the form
        f.appendChild(GroupDropDown.createFormElement("productname", productname));
        f.appendChild(GroupDropDown.createFormElement("cspc", cspc));
        f.appendChild(GroupDropDown.createFormElement("period", ""));
        f.appendChild(GroupDropDown.createFormElement("selectedgroupid", 0));
        if (headeroption === 2) {
            f.appendChild(GroupDropDown.createFormElement("headeroption", headeroption));
        }
        //submit form
        f.submit();
        //remove the newly created form after submit
        f.remove();
    }
}])




