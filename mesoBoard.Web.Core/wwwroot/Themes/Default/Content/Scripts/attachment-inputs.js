$(function () {
    var divUploadFields = $("#UploadFields");
    var btnExtraUpload = $("#ExtraUpload");
    var UploadStartIndex = 3;
    var MaxUploads = 4;
    btnExtraUpload.click(function () {
        if (UploadStartIndex <= MaxUploads) {
            divUploadFields.append('<input type="file" name="file' + (UploadStartIndex++) + '" /><br />');
        }
        else {
            $("#UploadStatus").html('<div>Max ' + MaxUploads + ' file uploads</div>');
        }
    });
});
