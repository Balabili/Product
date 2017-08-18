$(function () {
    _getItems();
    //1.初始化Table
    var oTable = new TableInit();
    oTable.Init();

    //2.初始化Button的点击事件
    var oButtonInit = new ButtonInit();
    oButtonInit.Init();

    $("#logout").click(function () {
        window.location.href = "/Login/Logout";
    });

    $("#btn_delete").click(function () {
        var selectData = $("#tb_departments").bootstrapTable('getSelections');
        if (selectData.length != 0) {
            utility.ajax("/Admin/DeleteUser", "POST", { userId: selectData[0].Id }, deleteSuccessCallBack);
        } else {
            alert("Nothing to delete");
        }
    });

    $("#btn_add").click(function () {
        document.getElementById("add_loginName").value = "";
        document.getElementById("add_password").value = "";
        document.getElementById("add_passwordAgain").value = "";
        document.getElementById("add_role").value = 2;
        $('#btn_submit').css("display", "inline-block");
        $('#btn_update').css("display", "none");
        $('#AddUserModal').modal();
    });

    $("#btn_edit").click(function () {
        var selectData = $("#tb_departments").bootstrapTable('getSelections');
        if (selectData.length != 0) {
            document.getElementById("add_loginName").value = selectData[0].LoginName;
            document.getElementById("add_password").value = selectData[0].PassWord;
            document.getElementById("add_passwordAgain").value = selectData[0].PassWord;
            document.getElementById("add_role").value = selectData[0].Role;
            $('#btn_submit').css("display", "none");
            $('#btn_update').css("display", "inline-block");
            $('#AddUserModal').modal();
        }
    });

    $("#btn_submit").click(function () {
        var loginName = document.getElementById("add_loginName").value;
        var password = document.getElementById("add_password").value;
        var passwordRepeat = document.getElementById("add_passwordAgain").value;
        var role = document.getElementById("add_role").value;
        if (password != passwordRepeat) {
            return;
        }
        var data = {};
        data.loginName = loginName.replace(/^\s+|\s+$/g, '');
        data.password = password.replace(/^\s+|\s+$/g, '');
        data.role = role;
        saveUserToDB(data);
    });

    $("#btn_update").click(function () {
        var loginName = document.getElementById("add_loginName").value;
        var password = document.getElementById("add_password").value;
        var passwordRepeat = document.getElementById("add_passwordAgain").value;
        var role = document.getElementById("add_role").value;
        if (password != passwordRepeat) {
            return;
        }
        var data = {};
        data.loginName = loginName.replace(/^\s+|\s+$/g, '');
        data.password = password.replace(/^\s+|\s+$/g, '');
        data.role = role;
        updateUserToDB(data);
    });

});

function _getItems() {
    if (user.Role != 2) {
        var adminNav = document.getElementById("isAdmin");
        adminNav.removeAttribute("style");
    }
    if (user.Role != 0) {
        var toolbar = document.getElementById("toolbar");
        toolbar.setAttribute("style", "display:none;");
    }
}

function deleteSuccessCallBack(result) {
    var LoginNames = [];
    LoginNames.push(result);
    $("#tb_departments").bootstrapTable('remove', { field: 'LoginName', values: LoginNames });
    alert("DELETE SUCCESSFUL");
}

function saveUserToDB(data) {
    utility.ajax("/Admin/AddUser", "POST", data, saveUserSuccessCallBack);
}

function updateUserToDB(data) {
    utility.ajax("/Admin/UpdateUser", "POST", data, updateUserSuccessCallBack);
}

function saveUserSuccessCallBack(result) {
    alert("save successful");
    $('#AddUserModal').modal('hide');
    var appendItem = [];
    appendItem.push(result);
    $("#tb_departments").bootstrapTable('append', appendItem);
}

function updateUserSuccessCallBack(result) {
    if (result) {
        $('#AddUserModal').modal('hide');
        $("#tb_departments").bootstrapTable('refresh');
        alert("update successful");
    }
}

var TableInit = function () {
    var oTableInit = new Object();
    //初始化Table
    oTableInit.Init = function () {
        $('#tb_departments').bootstrapTable({
            url: '/Admin/GetUsers',         //请求后台的URL（*）
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
            pageSize: 25,                       //每页的记录行数（*）
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
                    formatter: 'disableSuperAdmin',
                    visible: hideCheckbox()
                }, {
                    field: 'LoginName',
                    title: 'Login Name'
                }, {
                    field: 'PassWord',
                    title: 'PassWord'
                }, {
                    field: 'Role',
                    title: 'Role',
                    formatter: 'roleFormatter'
                }, {
                    field: 'Record',
                    title: 'Browse Record',
                    formatter: 'recordFormatter',
                    events: viewEvent
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

var ButtonInit = function () {
    var oInit = new Object();
    var postdata = {};

    oInit.Init = function () {
        //初始化页面上面的按钮事件
    };

    return oInit;
};

function hideCheckbox() {
    return user.Role == 0;
}

function disableSuperAdmin(value, row, index) {
    if (row.Role == 0) {
        return { disabled: true };
    }
}

function roleFormatter(value, row, index) {
    var roleName = "";
    switch (row.Role) {
        case 0: roleName = "Super Admin"; break;
        case 1: roleName = "Admin"; break;
        case 2: roleName = "End User"; break;
    }
    return roleName;
}

function recordFormatter() {
    return ['<a class="view" href="javascript:void(0)">View Record</a>'];
}

window.viewEvent = {
    'click .view': function (e, value, row, index) {
        window.open("/User/ViewProductDetails?id=" + row.Id);
    }
}