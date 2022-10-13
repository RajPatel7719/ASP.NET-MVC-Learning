$(document).ready(function () {

    let ddlState = $("#StateID").val()
    let ddlCountry = $("#CountryID").val()

    ISCountryBlank(ddlCountry)
    IsStateBlank(ddlState)

    $("#CountryID").change(function () {
        ISCountryBlank(ddlCountry)
        if ($("#CountryID").val() == "") {
        }
        else {
            var url = "/Users/GetStatesByCountry";
            var countryID = $("#CountryID").val();

            $.post(url, { CountryID: countryID }, function (data) {
                $("#StateID").empty();
                if (data) {
                    $("#StateID").append($('<option/>', {
                        value: "",
                        text: "Select a State"
                    }));
                    $.each(data, function (i, states) {
                        $("#StateID").append($('<option/>', {
                            value: states.StateID,
                            text: states.StateName
                        }));
                    });
                }
                $("#StateID").prop("disabled", false);
            });
        }
    });

    $("#StateID").change(function () {
        IsStateBlank(ddlState)
        if ($("#StateID").val() == "") {
        }
        else {
            var url = "/Users/GetCityByState";
            var stateID = $("#StateID").val();

            $("#CityID").prop("disabled", true);

            $.post(url, { StateID: stateID }, function (data) {
                $("#CityID").empty();
                if (data) {
                    $("#CityID").prop("disabled", false)
                    $("#CityID").append($('<option/>', {
                        value: "",
                        text: "Select a City"
                    }));
                    $.each(data, function (i, cities) {
                        $("#CityID").append($('<option/>', {
                            value: cities.CityID,
                            text: cities.CityName
                        }));
                    });
                    
                }
            });
        }
    });

    function NumericOnly(e) {
        var regex = new RegExp("^[0-9]+$");
        var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
        if (regex.test(str)) {
            return true;
        }
        e.preventDefault();
        return false;
    };

    function ISCountryBlank(country) {
        if (country == "") {
            $("#StateID").prop("disabled", true);
            $("#CityID").prop("disabled", true);
            $("#StateID").html("<option>Select State</option>");
            $("#CityID").html("<option>Select City</option>");
        }
    }

    function IsStateBlank(state) {
        if (state == "") {
            $("#CityID").prop("disabled", true);
            $("#CityID").html("<option>Select City</option>");
        }
    }

    $('#Phone_Number').keypress(function (e) {
        NumericOnly(e);
    });

    function isEmail(email) {
        var expr = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        if (expr.test(email)) {
            return true;
        }
        else {
            $("#email").removeClass("hide");
            return false;
        }
    }

    $("#create").click(function () {

        let firstName = $("#First_Name").val().trim();
        let lastName = $("#Last_Name").val().trim();
        let phoneNumber = $("#Phone_Number").val().trim().length;
        let email = $("#Email").val().trim();
        let gender = $(".Gender").is(":checked");

        if (firstName == '') {
            $("#firstName").removeClass("hide");
            return false;
        }
        else {
            $("#firstName").addClass("hide");
        }
        if (lastName == '') {
            $("#lastName").removeClass("hide");
            return false;
        }
        else {
            $("#lastName").addClass("hide");
        }
        if (phoneNumber == '') {
            $("#phoneNumber").removeClass("hide");
            return false;
        } else if (phoneNumber < 10 || phoneNumber > 10) {
            $("#phoneNumber").removeClass("hide");
            $("#phoneNumber").text("Please Enter Valid Phone Number")
            return false;
        } else {
            $("#phoneNumber").addClass("hide");
        }
        if (email == '') {
            $("#email").removeClass("hide");
            return false;
        } else if (!isEmail(email)) {
            $("#email").removeClass("hide");
            $("#email").text("Please Enter Valid Email")
            return false;
        } else {
            $("#email").addClass("hide");
        }
        if (!gender) {
            $("#gender").removeClass("hide");
            return false;
        } else {
            $("#gender").addClass("hide");
        }

    });
});