const data = require('./users.json')
const url = 'https://cper2g3earth4sportazurefunction.azurewebsites.net/api/register'
const users = data.map(d => ({ username: d.user, password: d.password, uuid: d.clock }))

exports.f = async () => {
    try {
        for (const el of users) {
            await fetch(url, {
                method: "POST",
                body: JSON.stringify(el)
            })
        }
    } catch (err) {
        console.log(err)
    }
}
