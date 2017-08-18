$(function () {
    initData();
})

function initData() {
    document.getElementById("productId").innerText = productInfo.Id;
    document.getElementById("productName").innerText = productInfo.Name;
    document.getElementById("description").innerText = productInfo.Description;
}