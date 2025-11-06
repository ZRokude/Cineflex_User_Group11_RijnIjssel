async function getPublicIP() {
    const response = await fetch('https://api.ipify.org?format=json');
    const data = await response.json();
    return data.ip;
} //JON: get the IP adres for the email

// Map initialization function
window.initMap = function (userLat, userLon) {
    // Initialize map centered on a location (latitude, longitude)
    var map = L.map('map').setView([51.505, -0.09], 13);

    // Add OpenStreetMap tiles (completely free!)
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '© OpenStreetMap contributors',
        maxZoom: 19
    }).addTo(map);

    // Marker op gebruikerslocatie
    L.marker([51.987492, 5.932322]).addTo(map)
        .bindPopup('You are here!')
        .openPopup();

    L.marker([51.9851, 5.8987]).addTo(map)
        .bindPopup('Arnhem, Netherlands!')
        .openPopup();

    // Add a marker on Ede
    L.marker([52.0408, 5.6672]).addTo(map)
        .bindPopup('Ede, Netherlands!')
        .openPopup();


    console.log('Map initialized successfully');
}