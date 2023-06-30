const {MongoClient} = require('mongodb');
require('dotenv').config()
const connstr = process.env.MONGO
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

module.exports.getAllClockIds = async function(){
    const options = {  
        projection: { _id: 1}  
      };
    let clocks = await client.db('provisioning').collection("devices").find({}, options).toArray(function (err, result) {
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

