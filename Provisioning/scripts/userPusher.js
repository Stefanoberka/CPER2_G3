const data = require('./users.json')
const url = 'https://cper2g3earth4sportazurefunction.azurewebsites.net/api/register'
const users = data.map(d => ({ username: d.user, password: d.password, uuid: d.clock }))

const f = async () => {
    try {
        // make sure that any items are correctly URL encoded in the connection string
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

f()