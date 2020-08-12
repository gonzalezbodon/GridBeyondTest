// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    GBT_Statistics.reload();
});

$('#btnLoadData').on('click', function (e) {
    GBT_Data.reload();
});

$('#btnClearData').on('click', function (e) {
    GBT_Data.clear();
});

//Object in charge of managing the alert element 
GBT_Information = {
    update: function (type, message) {
        switch (type.toUpperCase()) {
            case 'SUCCESS':
                $("#alertInformation").removeClass("alert-primary").removeClass("alert-danger").addClass("alert-success");
                break;
            case 'ERROR':
                $("#alertInformation").removeClass("alert-success").removeClass("alert-primary").addClass("alert-danger");
                break;
            case 'INFO':
                $("#alertInformation").removeClass("alert-success").removeClass("alert-danger").addClass("alert-primary");
                break;
        }
        
        $("#alertInformationText").html(message);
    }
}

//Object in charge of the statistics 
GBT_Statistics = {
    reload: function () {
        $.ajax({
            type: 'get',
            url: '/api/marketregister/statistics',
            datatype: "json",
            success: function (result) {
                if (result.totalItems == 0) {
                    GBT_Statistics.clear();
                    return;
                }
                let dateStart = new Date(result.mostExpensiveHourDate);
                let formattedDateStart = ("0" + dateStart.getDate()).slice(-2) + "/" + ("0" + dateStart.getMonth() + 1).slice(-2) + "/" + dateStart.getFullYear() + " " + ("0" + dateStart.getHours()).slice(-2) + ":" + ("0" + dateStart.getMinutes()).slice(-2);
                let dateEnd = new Date(result.mostExpensiveHourDateEnd);
                let formattedDateEnd = ("0" + dateEnd.getDate()).slice(-2) + "/" + ("0" + dateEnd.getMonth() + 1).slice(-2) + "/" + dateEnd.getFullYear() + " " + ("0" + dateEnd.getHours()).slice(-2) + ":" + ("0" + dateEnd.getMinutes()).slice(-2);
                $("#statisticsTotalItems").html(result.totalItems);
                $("#statisticsTextMinimum").html(parseFloat(result.minPrice).toFixed(3)+"€");
                $("#statisticsTextMaximum").html(parseFloat(result.maxPrice).toFixed(3) + "€");
                $("#statisticsTextAverage").html(parseFloat(result.avgPrice).toFixed(3) + "€");
                $("#statisticsExpensiveHour").html("From " + formattedDateStart);
                $("#statisticsExpensiveHourEnd").html("To " + formattedDateEnd);
                $("#statisticsExpensivePrice").html(parseFloat(result.mostExpensiveHourPrice).toFixed(3) + "€");
            },
            error: function (xmlhttprequest) {
                GBT_Statistics.clear();
            },
            complete: function () {
                
            }
        });
    },
    clear: function () {
        $("#statisticsTotalItems").html("-");
        $("#statisticsTextMinimum").html("-");
        $("#statisticsTextMaximum").html("-");
        $("#statisticsTextAverage").html("-");
        $("#statisticsExpensiveHour").html("-");
        $("#statisticsExpensiveHourEnd").html("-");
        $("#statisticsExpensivePrice").html("-");
    }
}

//Object in charge of managing data
GBT_Data = {
    reload: function () {
        $("#loaderDataManagement").css("display", "inline-block");
        $("#btnLoadData").css("display", "none");
        $("#btnClearData").css("display", "none");
        GBT_Information.update("INFO", "Loading data");

        $.ajax({
            type: 'post',
            url: '/api/marketregister',
            datatype: "json",
            success: function (result) {
                GBT_Information.update(result.code == 0 ? "SUCCESS" : "ERROR", result.message + '. <a href="javascript:location.reload()">Reload table</a> to see data');
                if (result.code == 0) GBT_Statistics.reload();
            },
            error: function (xmlhttprequest) {
                GBT_Information.update("ERROR", "Unknown error loading data");
            },
            complete: function () {
                $("#loaderDataManagement").css("display", "none");
                $("#btnLoadData").css("display", "inline");
                $("#btnClearData").css("display", "inline");
            }
        });
    },
    clear: function () {
        $("#loaderDataManagement").css("display", "inline-block");
        $("#btnLoadData").css("display", "none");
        $("#btnClearData").css("display", "none");
        $("#alertInformation").removeClass("alert-success").removeClass("alert-danger").addClass("alert-primary");
        $("#alertInformationText").html("Removing data");
        $.ajax({
            type: 'delete',
            url: '/api/marketregister',
            datatype: "json",
            success: function (result) {
                GBT_Information.update(result.code == 0 ? "SUCCESS" : "ERROR", result.message + '. <a href="javascript:location.reload()">Reload table</a> to clear data');
                if (result.code == 0) GBT_Statistics.clear();
            },
            error: function (xmlhttprequest) {
                GBT_Information.update("ERROR", "Unknown error deleting data");
            },
            complete: function () {
                $("#loaderDataManagement").css("display", "none");
                $("#btnLoadData").css("display", "inline");
                $("#btnClearData").css("display", "inline");
            }
        });
    }
}