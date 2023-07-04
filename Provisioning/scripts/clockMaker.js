const fs = require('fs');
const uuid = require('uuid').v4

let ts = new Date()
let devices = []

exports.f = () => {
    try {
        for (let batch = 0; batch < 5; batch++) {
            for (let device = 0; device < 10; device++) {
                devices.push({
                    data_batch: ts,
                    n_batch: batch,
                    _id: uuid()
                })
            }
            ts = new Date(ts.setHours(ts.getHours() + 2))
        }

        fs.writeFileSync('./output/first_five_batches.json', JSON.stringify(devices))

        console.log('done.')
    }
    catch (error) {
        console.log(error.message)
    }
}
