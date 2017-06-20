
/*-------------------------Common Begin------------------------------*/
function fShowProcessBar() {
    var screenHeight = document.body.offsetHeight;
    $("#divProcess").css("height", screenHeight);
    $("#divShield").css("height", screenHeight);


    $("#divProcess").show();
    $("#processPersent").html("0% ");
    $("#divProcessContent").show();
    $("#processNeedTime").html("计算中...");
}

function fUpdateProcess(persent, needSeconds) {

    //更新进度
    //var processhtml = '<div  class="progress" data-percent="' + persent + '%">'
    //    + '<div class="bar" style="width: ' + persent + '%;">'
    //    + '</div>'
    //    + '</div>';

    //$("#divProcessBar").html("")
    //$("#divProcessBar").html(processhtml);

    //更新预计剩余时间
    $("#processPersent").html("");
    $("#processPersent").html(persent + "% ");

    $("#processNeedTime").html("");
    $("#processNeedTime").html(fGetTimeStringBySeconds(needSeconds));

}

function fGetTimeStringBySeconds(seconds) {
    if (seconds > 60) {
        var minute = parseInt(seconds / 60);
        var andSeconds = parseInt(seconds) % 60;

        return minute.toString() + "分钟" + andSeconds + "秒";
    }
    else {
        return seconds.toString() + "秒";
    }
}

/*-------------------------Common End------------------------------*/

function fSaveFile() {
    debugger;
    var postObject = new Object();

    postObject.EntityClassName = $("#hfEntityClassName").val();

    //提交前显示进度条
    fShowProcessBar();

    debugger;
    $.ajax({
        url: '/File/SaveFile',
        type: 'POST',
        cache: false,
        data: new FormData($('#formSimpleUpload')[0]),
        processData: false,
        contentType: false,
        beforeSend: function () {
            console.log("正在进行，请稍候");
        },
        success: function (result) {
            debugger;
            fPorcessFile(result.FileName, result.EntityClassName);
        },
        error: function (responseStr) {
            console.log("error");
        }
    })


}

function fPorcessFile(fileName, entityClassName) {
    $("#processFrame").attr("src", "/File/ProcessFile?FileName=" + fileName + "&EntityClassName=" + entityClassName);
}

function fProcessResult(msg, errorFileName, entityClassName)
{
    window.location = "/File/ProcessResult?Msg=" + msg + "&FileName=" + errorFileName + "&EntityClassName=" + entityClassName;
}
