var Fastify = require('fastify')
var fastify = Fastify();
fastify.decorate('provider', require('./mongo.js'))


fastify.get('/clocks', async (req, res) => {
    var clocks = await fastify.provider.getClocks()
    res.send(clocks)
    return 
})

fastify.get('/clocks/ids', async (req, res) => {
    var clocks = await fastify.provider.getAllClockIds()
    res.send(clocks)
    return 
})

fastify.get('/clocks/:uuid', async (req, res) =>{
    let id = req.params.uuid
    console.log(id)
    var clock = await fastify.provider.getClockByUuid(id)
    console.log(clock)
    res.send(clock)
    return
})


async function run() {
    try {
        var port = 6969
        await fastify.listen( {port}, () => {
            console.log(fastify.printRoutes())
        })
        console.log(`Server running on port ${port}`)
    } catch (error) {
        console.log('error', error)
    }
}

run()