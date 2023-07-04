const fs = require('fs')
const path = require('path')
const scripsList = fs.readdirSync('./scripts').map(file => file.replace('.js', ''))

const main = async () => {
    const arg = process.argv[2]
    if (!scripsList.includes(arg)) {
        console.log('script not found');
    }
    else {
        const {f} = require(path.join(__dirname, `scripts/${arg}.js`));
        f();
    }
}

main()