$(function () {

    _getItems();

    //1.初始化Table
    var oTable = new TableInit();
    oTable.Init();

    //2.初始化Button的点击事件
    var oButtonInit = new ButtonInit();
    oButtonInit.Init();

    var addBtn = document.getElementById("btn_add");
    var editBtn = document.getElementById("btn_edit");
    var deleteBtn = document.getElementById("btn_delete");
    var viewDetailBtn = document.getElementById("btn_view");

    addBtn.onclick = function () {
        document.getElementById("add_productName").value = "";
        document.getElementById("add_description").value = "";
        $('#btn_submit').css("display", "inline-block");
        $('#btn_update').css("display", "none");
        $('#AddProductModal').modal();
    }

    editBtn.onclick = function () {
        var selectData = $("#tb_departments").bootstrapTable('getSelections');
        if (selectData.length != 0) {
            productId = selectData[0].Id;
            document.getElementById("add_productName").value = selectData[0].Name;
            document.getElementById("add_description").value = selectData[0].Description;
            $('#btn_submit').css("display", "none");
            $('#btn_update').css("display", "inline-block");
            $('#AddProductModal').modal();
        }
    }

    deleteBtn.onclick = function () {
        var selectData = $("#tb_departments").bootstrapTable('getSelections');
        if (selectData.length != 0) {
            utility.ajax("/Product/DeleteProduct", "POST", { id: selectData[0].Id }, deleteSuccessCallBack);
        } else {
            alert("Nothing to delete");
        }
    }

    viewDetailBtn.onclick = function () {
        window.open("/User/ViewProductDetails?id=" + user.Id);
    }

    $("#btn_submit").click(function () {
        var productName = document.getElementById("add_productName").value;
        var productDescription = document.getElementById("add_description").value;
        var data = {};
        data.productName = productName.replace(/^\s+|\s+$/g, '');
        data.productDescription = productDescription.replace(/^\s+|\s+$/g, '');
        saveProductToDB(data);
    });

    $("#btn_update").click(function () {
        var productName = document.getElementById("add_productName").value;
        var description = document.getElementById("add_description").value;
        var data = {};
        data.id = productId;
        data.productName = productName.replace(/^\s+|\s+$/g, '');
        data.description = description.replace(/^\s+|\s+$/g, '');
        updateProductToDB(data);
    });
});
var productId = "";

function _getItems() {
    if (user.Role != 2) {
        var adminNav = document.getElementById("isAdmin");
        adminNav.removeAttribute("style");
    } else {
        var toolbar = document.getElementById("toolbar");
        toolbar.setAttribute("style", "display:none;");
    }
}

var TableInit = function () {
    var oTableInit = new Object();
    //初始化Table
    oTableInit.Init = function () {
        $('#tb_departments').bootstrapTable({
            url: '/Product/GetProducts',         //请求后台的URL（*）
            method: 'get',                      //请求方式（*）
            toolbar: '#toolbar',                //工具按钮用哪个容器
            striped: true,                      //是否显示行间隔色
            cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: true,                   //是否显示分页（*）
            sortable: false,                     //是否启用排序
            sortOrder: "asc",                   //排序方式
            queryParams: oTableInit.queryParams,//传递参数（*）
            sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
            pageNumber: 1,                       //初始化加载第一页，默认第一页
            pageSize: 10,                       //每页的记录行数（*）
            pageList: [10, 25, 50, 100],        //可供选择的每页的行数（*）
            search: false,                       //是否显示表格搜索，此搜索是客户端搜索，不会进服务端，所以，个人感觉意义不大
            strictSearch: true,
            singleSelect: true,
            showColumns: true,                  //是否显示所有的列
            showRefresh: true,                  //是否显示刷新按钮
            minimumCountColumns: 2,             //最少允许的列数
            clickToSelect: true,                //是否启用点击选中行
            height: 500,                        //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
            uniqueId: "ID",                     //每一行的唯一标识，一般为主键列
            showToggle: true,                    //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                   //是否显示父子表
            columns: [
                {
                    checkbox: true,
                    visible: isAdmin()
                }, {
                    field: 'Id',
                    title: 'Product Id',
                    formatter: 'productIdFormatter',
                    events: viewEvent
                }, {
                    field: 'Name',
                    title: 'Product Name'
                }, {
                    field: 'Description',
                    title: 'Product Description',
                }]
        });
    };

    //得到查询的参数
    oTableInit.queryParams = function (params) {
        var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
            limit: params.limit,   //页面大小
            offset: params.offset,  //页码
            departmentname: $("#txt_search_departmentname").val(),
            statu: $("#txt_search_statu").val()
        };
        return temp;
    };
    return oTableInit;
};
function isAdmin() {
    return user.Role != 2;
}

var ButtonInit = function () {
    var oInit = new Object();
    var postdata = {};

    oInit.Init = function () {
        //初始化页面上面的按钮事件
    };

    return oInit;
};

function productIdFormatter(value, row, index) {
    return ['<a class="view" href="javascript:void(0)">' + row.Id + '</a>'];
}

window.viewEvent = {
    'click .view': function (e, value, row, index) {
        window.open("/Product/ProductDetail?id=" + value);
    }
}

function viewProductDetailSuccessCallBack() {

}

function saveProductToDB(data) {
    utility.ajax("/Product/AddProduct", "POST", data, addProductSuccessCallBack);
}

function updateProductToDB(data) {
    utility.ajax("/Product/UpdateProduct", "POST", data, updateProductSuccessCallBack);
}

function addProductSuccessCallBack(result) {
    if (result) {
        alert("Add successful");
        $("#tb_departments").bootstrapTable('refresh');
        $('#AddProductModal').modal('hide');
    } else {
        alert("Add Product Failed");
    }
}

function updateProductSuccessCallBack(result) {
    alert("update successful");
    $("#tb_departments").bootstrapTable('refresh');
    $('#AddProductModal').modal('hide');
}

function deleteSuccessCallBack(resule) {
    alert("delete successful");
    $("#tb_departments").bootstrapTable('refresh');
}