$(function () {
    var _utility = utility;
    $("#register").click(function () {
        var loginName = document.getElementById("loginName").value;
        var firstPassWord = document.getElementById("passWord").value;
        var secondPassWord = document.getElementById("passWordAgain").value;
        var errorMsgList = document.getElementsByClassName("ErrorMsg");
        for (var i = 0; i < errorMsgList.length; i++) {
            errorMsgList[i].remove();
        }
        if (firstPassWord != secondPassWord) {
            loginArea = document.getElementById("passwordArea");
            var createNode = document.createElement("div");
            createNode.setAttribute("class", "ErrorMsg");
            createNode.innerText = "The password is not correct!";
            loginArea.appendChild(createNode);
            return;
        }
        if (loginName.replace(/^\s+|\s+$/g, '') == "" || firstPassWord.replace(/^\s+|\s+$/g, '') == "" || secondPassWord.replace(/^\s+|\s+$/g, '') == "") {
            alert("Can't Empty!");
            return;
        }
        var data = {};
        data.LoginName = loginName.replace(/^\s+|\s+$/g, '');
        data.PassWord = firstPassWord;
        data.Role = 2;
        _saveUserToDB(data);
    });

    function _saveUserToDB(data) {
        _utility.ajax("Register/Register", "POST", data, saveSuccessCallBack);
    }

    function saveSuccessCallBack(result) {
        if (result) {
            loginArea = document.getElementById("loginArea");
            var createNode = document.createElement("div");
            createNode.setAttribute("class", "ErrorMsg");
            createNode.innerText = "The login name is Exists!";
            loginArea.appendChild(createNode);
            return;
        } else {
            debugger;
            window.location.href = "/Login/Index";
        }
    }

})