function showDataAlert() {

    var confirmStatus = confirm("Are you sure you want to delete?");
    if (confirmStatus == true) {
        return true;
    }
    else {
        return false;
    }


}