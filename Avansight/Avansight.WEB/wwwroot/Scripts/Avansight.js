var ageChart, genderChart, visitDataChart;
var domoCharts = (function () {
    const ageOptions = {
        width: 600,
        height: 400
    };
    const genderOptions = {
        width: 600,
        height: 400
    };
    const visitDataOptions = {
        width: 1200,
        height: 600
    };
    function init(ageChartContainer, genderChartContainer, visitDataChartContainer) {
        var data = {
            rows: [
            ],
            columns: [
            ]
        };
        ageChart = new DomoPhoenix.Chart(DomoPhoenix.CHART_TYPE.BAR, data, ageOptions);
        ageChartContainer.appendChild(ageChart.canvas);
        ageChart.render();
        genderChart = new DomoPhoenix.Chart(DomoPhoenix.CHART_TYPE.PIE, data, genderOptions);
        genderChartContainer.appendChild(genderChart.canvas);
        genderChart.render();
        visitDataChart = new DomoPhoenix.Chart(DomoPhoenix.CHART_TYPE.LINE, data, visitDataOptions);
        visitDataChartContainer.appendChild(visitDataChart.canvas);
        visitDataChart.render();
        PatientGet();
    }
   
    return {
        init
    }
})();
function PatientGet() {

    $.ajax({
        type: "POST",
        contentType: "application/x-www-form-urlencoded; charset=utf-8",
        url: '/Patient/PatientGet',
        data: "data=PatientGet",
        dataType: "text",
        success: function (data) {
            var res = $.parseJSON(data);
            parsePatient(res);
        },
        error: function (xhr, err) {
            alert("readyState: " + xhr.readyState + "\nstatus: " + xhr.status);
            alert("responseText: " + xhr.responseText);
        }
    });
}
function parsePatient(patients) {
    var maleCount = 0;
    var femaleCount = 0;
    var age20Count = 0;
    var age30Count = 0;
    var age40Count = 0;
    var age50Count = 0;
    var age60Count = 0;

    for (var i = 0; i < patients.length; i++) {
        var obj = patients[i];
        if (obj.gender.trim() == "Male")
            maleCount++;
        else
            femaleCount++;
        if (obj.age > 20 && obj.age <= 30)
            age20Count++;
        if (obj.age > 30 && obj.age <= 40)
            age30Count++;
        if (obj.age > 40 && obj.age <= 50)
            age40Count++;
        if (obj.age > 50 && obj.age <= 60)
            age50Count++;
        if (obj.age > 60 && obj.age <= 70)
            age60Count++;
    }
    var genderdata = {
        // This is the data you get back from the Domo Data API
        rows: [
            ['Male', 'Male', maleCount],
            ['Female', 'Female', femaleCount],

        ],
        // You provide the names, types, and mappings of your ordered columns
        columns: [
            {
                type: DomoPhoenix.DATA_TYPE.STRING,
                name: 'Order Priority',
                mapping: DomoPhoenix.MAPPING.SERIES
            },
            {
                type: DomoPhoenix.DATA_TYPE.STRING,
                name: 'Customer Segment',
                mapping: DomoPhoenix.MAPPING.ITEM
            },
            {
                type: DomoPhoenix.DATA_TYPE.DOUBLE,
                name: 'Sales',
                mapping: DomoPhoenix.MAPPING.VALUE
            }
        ]
    };
    var agedata = {
        // This is the data you get back from the Domo Data API
        rows: [
            ['', '0-10', 0],
            ['', '10-20', 0],
            ['', '20-30', age20Count],
            ['', '30-40', age30Count],
            ['', '40-50', age40Count],
            ['', '50-60', age50Count],
            ['', '60-70', age60Count],

        ],
        // You provide the names, types, and mappings of your ordered columns
        columns: [
            {
                type: DomoPhoenix.DATA_TYPE.STRING,
                name: 'Order Priority',
                mapping: DomoPhoenix.MAPPING.SERIES
            },
            {
                type: DomoPhoenix.DATA_TYPE.STRING,
                name: 'Customer Segment',
                mapping: DomoPhoenix.MAPPING.ITEM
            },
            {
                type: DomoPhoenix.DATA_TYPE.DOUBLE,
                name: 'Sales',
                mapping: DomoPhoenix.MAPPING.VALUE
            }
        ]
    };

    ageChart.update(agedata);
    genderChart.update(genderdata);
}
var patientRows = [];
function parsetreatmentReading(treatmentReadings) {

    var select = document.getElementById("selectPatient");
    select.innerHTML = "";
    var options = [];

    // Optional: Clear all existing options first:
    // Populate list with options:

    //alert(JSON.stringify(treatmentReadings));

    patientRows = [];
    for (var i = 0; i < treatmentReadings.length; i++) {
        //alert(JSON.stringify(treatmentReadings[i]));
        var row = [];
        row[0] = treatmentReadings[i].patientId.toString();
        row[1] = treatmentReadings[i].visitWeek.trim();
        row[2] = treatmentReadings[i].reading;
        patientRows.push(row);
        options.push(treatmentReadings[i].patientId);

        //alert(JSON.stringify(patientRows));
    }

    var options = Array.from(new Set(options))
    for (var i = 0; i < options.length; i++) {
        var opt = options[i];
        select.innerHTML += "<option value=\"" + opt + "\">" + opt + "</option>";
    }

    var linedata = {
        // This is the data you get back from the Domo Data API
        rows: patientRows,
        // You provide the names, types, and mappings of your ordered columns
        columns: [
            {
                type: DomoPhoenix.DATA_TYPE.STRING,
                name: 'Order Priority',
                mapping: DomoPhoenix.MAPPING.SERIES
            },
            {
                type: DomoPhoenix.DATA_TYPE.STRING,
                name: 'Customer Segment',
                mapping: DomoPhoenix.MAPPING.ITEM
            },
            {
                type: DomoPhoenix.DATA_TYPE.DOUBLE,
                name: 'Sales',
                mapping: DomoPhoenix.MAPPING.VALUE
            }
        ],

    };
    visitDataChart.update(linedata);
}




$(document).delegate("select", "change", function () {
    //capture the option
    var $target = $("option:selected", $(this));
    var patientRow = [];
    for (var i = 0; i < patientRows.length; i++) {

        if (patientRows[i][0] == $target.val()) {
            patientRow.push(patientRows[i]);
        }
    }
    var newdata = {
        // This is the data you get back from the Domo Data API
        rows: patientRow,
        // You provide the names, types, and mappings of your ordered columns
        columns: [
            {
                type: DomoPhoenix.DATA_TYPE.STRING,
                name: 'Order Priority',
                mapping: DomoPhoenix.MAPPING.SERIES
            },
            {
                type: DomoPhoenix.DATA_TYPE.STRING,
                name: 'Customer Segment',
                mapping: DomoPhoenix.MAPPING.ITEM
            },
            {
                type: DomoPhoenix.DATA_TYPE.DOUBLE,
                name: 'Sales',
                mapping: DomoPhoenix.MAPPING.VALUE
            }
        ],

    };

    visitDataChart.update(newdata);

});
function TreatmentReadingGet() {
    $.ajax({
        type: "POST",
        contentType: "application/x-www-form-urlencoded; charset=utf-8",
        url: '/Patient/TreatmentReadingGet',
        data: "data=TreatmentReadingGet",
        dataType: "text",
        success: function (data) {
            var res = $.parseJSON(data);
            parsetreatmentReading(res);

        },
        error: function (xhr, err) {
            alert("readyState: " + xhr.readyState + "\nstatus: " + xhr.status);
            alert("responseText: " + xhr.responseText);
        }
    });
}

$(function () {
    $("#Simulate").click(function () {
        $('#tab2').hide()
        $('#tab1').show();
        PatientGet();
    });
});
$(function () {
    $("#Visit").click(function () {
        $('#tab1').hide()
        $('#tab2').show();
        TreatmentReadingGet();
    });
});
$(function () {
    $("#all").click(function () {
        var newdata = {
            // This is the data you get back from the Domo Data API
            rows: patientRows,
            // You provide the names, types, and mappings of your ordered columns
            columns: [
                {
                    type: DomoPhoenix.DATA_TYPE.STRING,
                    name: 'Order Priority',
                    mapping: DomoPhoenix.MAPPING.SERIES
                },
                {
                    type: DomoPhoenix.DATA_TYPE.STRING,
                    name: 'Customer Segment',
                    mapping: DomoPhoenix.MAPPING.ITEM
                },
                {
                    type: DomoPhoenix.DATA_TYPE.DOUBLE,
                    name: 'Sales',
                    mapping: DomoPhoenix.MAPPING.VALUE
                }
            ],

        };
        visitDataChart.update(newdata);
    });
});
$(function () {
    $("#generate").click(function () {
        var size = $("#size").val();
        var male = $("#male").val();
        var female = $("#female").val();
        var age20 = $("#age20").val();
        var age30 = $("#age30").val();
        var age40 = $("#age40").val();
        var age50 = $("#age50").val()//;
        var age60 = $("#age60").val();
        if (parseInt(size) < 10) {
            alert("Sample Size minimum 10");
            return;
        }

        if (parseInt(male) < 1 || parseInt(female) < 1 || parseInt(age20) < 1 || parseInt(age30) < 1 || parseInt(age40) < 1 || parseInt(age50) < 1 || parseInt(age60) < 1) {
            alert("minimum of 1 for the weight");
            return;
        }

        var jsondata = { "size": size, "male": male, "female": female, "age20": age20, "age30": age30, "age40": age40, "age50": age50, "age60": age60 };
        $.ajax({
            type: "POST",
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            url: '/Patient/PatientSet',
            data: "data=" + JSON.stringify(jsondata),
            dataType: "text",
            success: function (data) {
                var res = $.parseJSON(data);
                parsePatient(res);
            },
            error: function (xhr, err) {
                alert("readyState: " + xhr.readyState + "\nstatus: " + xhr.status);
                alert("responseText: " + xhr.responseText);
            }
        })
        return false;
    });
});
$(function () {
    $("#save").click(function () {
        $.ajax({
            type: "POST",
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            url: 'Patient/SaveRecord',
            data: "data=",
            dataType: "text",
            success: function (data) {
                var res = $.parseJSON(data);
                alert("Save Success");
            },
            error: function (xhr, err) {
                alert("readyState: " + xhr.readyState + "\nstatus: " + xhr.status);
                alert("responseText: " + xhr.responseText);
            }
        })
        return false;
    });
});