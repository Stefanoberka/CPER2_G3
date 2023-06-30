const clock_ids = require('./clocks_ids.json');
const url = 'https://randomuser.me/api';
const fs = require('fs');

let users = []

async function a() {
    for (const e of clock_ids) {
        let user = (await fetch(url).then(res => res.json())).results[0].login

        users.push({ uuid: user.uuid, user: user.username, password: user.password, clock: e._id })
    }

    fs.writeFileSync('users.json', JSON.stringify(users))
}

a()

