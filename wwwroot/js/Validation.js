function userNamePasswordCheck(event) {
    event.preventDefault();
    var userName = document.getElementById('UserName').value;
    var passWord = document.getElementById('Password').value;

    if ((userName != "") && (passWord != "")) {
        document.getElementById('Login').disabled = false;
    }
    else {
        document.getElementById('Login').disabled = true;
    }
}

function indentStatusCheck(event) {
    var status = document.getElementById('UpdateStatus').value;

    if (status == 'Select Status') {
        document.getElementById('ApprovalSubmit').disabled = true;
    }
    else if (status == "") {
        document.getElementById('ApprovalSubmit').disabled = true;
    }
    else if (status == document.getElementById("ApprovalStatus").value) {
        document.getElementById('ApprovalSubmit').disabled = true;
    }
    else {
        document.getElementById('ApprovalSubmit').disabled = false;
    }
}