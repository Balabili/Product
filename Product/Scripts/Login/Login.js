$(function () {
    var _utility = utility;

    $("#register").click(function () {
        window.open("/Register/Index");
    })

    $("#loginBtn").click(function () {
        var data = {};
        var loginName = document.getElementById("loginName").value;
        var password = document.getElementById("password").value;
        data.loginName = loginName;
        data.password = password;
        _utility.ajax("Login/Login", "POST", data, saveSuccessCallBack);
    })

    function saveSuccessCallBack(result) {
        if (result) {
            window.location.href = "/Product/Index";
        }else {
            alert("账号或密码错误");
        }
    }
})