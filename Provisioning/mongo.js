const {MongoClient} = require('mongodb');
const connstr = "mongodb://cper2g3:u71Y8f*1%40Y!v@99.80.67.24:27017"
let client = new MongoClient(connstr);


module.exports.getClocks = async function(){
    let clocks = await client.db('provisioning').collection("devices").find({}).toArray(function (err, result) {
        if (err) {
            return(err)
        } else {
            console.log(result);
        }
    })
    return clocks
} 

module.exports.getClockByUuid = async function(id){
    const query = { _id: id };
    let clock = await client.db('provisioning').collection("devices").findOne(query)
    return clock
}