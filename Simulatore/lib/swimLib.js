const uuid = require('uuid').v4;

const poolLength = process.env.POOL_LENGTH ?? 200;
const interval = process.env.INTERVAL ?? 10;
const clock_id = process.env.CLOCK_ID
const url = process.env.API_URL + "/" + clock_id

class Data {
    constructor() {
        this.sessionUUID = uuid();
        this.pools = 0;
        this.distance = 0;
        this.bpm = randomNumberBetween(80, 120);
        this.gps = {
            latitude: (Math.random() * 180) - 90,
            longitude: (Math.random() * 360) - 180
        };
        this.timestamp = new Date();
    }

    update() {
        // aggiorno i BPM:
        this.bpm = randomNumberBetween(80, 120);

        // salvo temporaneamente la posizione attuale per poter calcolare la distanza da quella nuova:
        let prevLat = this.gps.latitude;
        let prevLon = this.gps.longitude;

        // aggiorno la posizione:
        let rand4gps = Math.random(); // direzione casuale
        let newLat = prevLat;
        let newLon = prevLon;
        if (rand4gps < 0.15) {
            newLat += movement();
            newLon += movement();
        }
        else if (rand4gps >= 0.15 && rand4gps < 0.3) {
            newLat += movement();
            newLon -= movement();
        }
        else if (rand4gps >= 0.3 && rand4gps < 0.45) {
            newLat -= movement();
            newLon += movement();
        }
        else if (rand4gps >= 0.45 && rand4gps < 0.6) {
            newLat = prevLat - movement();
        }
        else if (rand4gps >= 0.6 && rand4gps < 0.75) {
            newLat = prevLat + movement();
        }
        else if (rand4gps >= 0.75 && rand4gps < 0.9) {
            newLon = prevLon - movement();
        }
        else if (rand4gps >= 0.9) {
            newLon = prevLon + movement();
        }
        if (newLat >= 90) {
            newLat = 90 + (90 - newLat);
            newLon -= 180;
        }
        else if (newLat < -90) {
            newLat = -90 + (-90 - newLat);
            newLon -= 180;
        }
        if (newLon > 180) {
            newLon = ((newLon + 180) % 360) - 180;
        }
        else if (newLon <= -180) {
            newLon = ((newLon - 180) % 360) + 180;
        };
        this.gps.latitude = newLat;
        this.gps.longitude = newLon;

        // ora che ho la nuova posizione calcolo distanza percorsa e vasche:
        this.distance += calcDistance(prevLat, prevLon, newLat, newLon)
        this.pools = Math.floor(this.distance / poolLength)

        // aggiorno il timestamp:
        this.timestamp = new Date();
    };
};


async function postJSON(data) {
    try {
        const response = await fetch(url, {
            method: "POST", // or 'PUT'
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(data),
        });

        const result = await response.json();
        console.log("Success:", result);
    } catch (error) {
        console.error("Error:", error);
    }
}

function swim(clock_id) {
    let data = new Data();
    postJSON(data)
    console.log(data)
    const clear = setInterval(async function () {
        data.update()
        postJSON(data);
        console.log(data)
    }, interval * 1000)
    return clear
}

function randomNumberBetween(min, max) {
    return Math.floor(Math.random() * (max - min) + min)
};

function movement() {
    return Math.random() / 10000 // 0.0001 dovrebbe essere ~100m all'equatore
}

function calcDistance(lat1, lon1, lat2, lon2) {
    // tutta sta roba è copiaincollata da qui:
    // https://www.movable-type.co.uk/scripts/latlong.html
    const R = 6371e3; // metres
    const r1 = lat1 * Math.PI / 180; // φ, λ in radians
    const r2 = lat2 * Math.PI / 180;
    const dr = (lat2 - lat1) * Math.PI / 180;
    const dl = (lon2 - lon1) * Math.PI / 180;

    const a = Math.sin(dr / 2) * Math.sin(dr / 2) +
        Math.cos(r1) * Math.cos(r2) *
        Math.sin(dl / 2) * Math.sin(dl / 2);
    const c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));

    return R * c; // in metres
}

module.exports = {
    swim
}