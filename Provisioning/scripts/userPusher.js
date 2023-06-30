const data = require('./users.json')

const users = data.map(d => ({ username: d.user, password: d.password, uuid: d.uuid }))
const users_clocks = data.map(d => ({ user_uuid: d.uuid, clock_uuid: d.clock }))

const sql = require('mssql')
const connstr = 'Server=tcp:cper2-gruppo3.database.windows.net,1433;Initial Catalog=authdb;Persist Security Info=False;User ID=auth_admin;Password=6P]Md*Q}Vf.&~]j;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'

const f = async () => {
    try {
        // make sure that any items are correctly URL encoded in the connection string
        await sql.connect(connstr)
        for (const el of users) {
            const result = await sql.query`INSERT INTO [dbo].[users]
            ([username]
            ,[password]
            ,[uuid])
            VALUES
            (${el.username},
            ${el.password},
            ${el.uuid})`
        }
        for (const el of users_clocks) {
            console.log(el)
            const result = await sql.query`INSERT INTO [dbo].[users_clocks]
            ([user_uuid]
            ,[clock_uuid])
            VALUES
            (${el.user_uuid},
            ${el.clock_uuid})`
        }
    } catch (err) {
        console.log(err)
    }
}

f()