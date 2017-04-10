﻿var potholeList = [];
var map;
var infoWindowList = [];

function placePotholes(currentPotholes, userType) {

    var cleveland = { lat: 41.505, lng: -81.681 };
    map = new google.maps.Map(document.getElementById('map'), {
        zoom: 15,
        center: cleveland
    });

    for (var i = 0; i < currentPotholes.length; i++) {

// takes lat long and gives street address
        $.ajax({
            url: "https://maps.googleapis.com/maps/api/geocode/json?latlng=40.714224,-73.961452&key=AIzaSyAPiyVpokXSqtRW8T58W_gFi9YutxN-ZlA",
            type: "GET",
            dataType: "json"
        }).done(function (data) {
            $("#rep1").html(data.results.formatted_address);
            alert(data.results.formatted_address);
        }).fail(function (xhr, status, error) {
            console.log(error);
        });


        var img = '/images/reddot.png';
        var titleText = 'Awaiting Inspection';

        if (currentPotholes[i].RepairEndDate != null) {
            img = '/images/greendot.png';
            titleText = 'Repair Complete';
        }
        else if (currentPotholes[i].RepairStartDate != null) {
            img = '/images/bluedot.png'
            titleText = 'Repair In Progress';
        }
        else if (currentPotholes[i].InspectDate != null) {
            img = '/images/yellowdot.png';
            titleText = 'Awaiting Repair';
        }

        var pothole = {
            pin: new google.maps.Marker({
                position: { lat: currentPotholes[i].Latitude, lng: currentPotholes[i].Longitude },
                map: map,
                icon: img,
                title: titleText,
            }),
            info: currentPotholes[i],
            id: currentPotholes[i].PotholeId,

        }

        pothole.pin.addListener('click', function (event) {

            closeAllInfoWindows();

            var infoWindow = new google.maps.InfoWindow({
                content: '<p>' + this.title + '</p>'
            })

            infoWindow.open(map, this);
            infoWindowList.push(infoWindow);
        })

        potholeList.push(pothole);
    }

}

function closeAllInfoWindows() {
    for (var i = 0; i < infoWindowList.length; i++) {
        infoWindowList[i].close();
    }
}