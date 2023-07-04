
const url = 'https://randomuser.me/api';
const fs = require('fs');

let users = []

exports.f = async () => {
    try {
        const clock_ids = require('./../output/first_five_batches.json');
        for (const clock of clock_ids) {
            let user = (await fetch(url).then(res => res.json())).results[0].login

            users.push({
                uuid: user.uuid,
                user: user.username,
                password: user.password,
                clock: clock._id
            })
        }

        fs.writeFileSync('./output/users.json', JSON.stringify(users))
        console.log('done.')
    }
    catch (error) {
        console.log(error.message)
    }
}
