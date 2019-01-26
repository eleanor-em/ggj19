const mongoose = require('mongoose')
const Item = require('../models/item')
const Instance = require('../models/instance')
require('dotenv').config()

mongoose.set('useCreateIndex', true)
mongoose.connect(process.env.DATABASE_URL, {
    useNewUrlParser: true
})

async function populate() {
    try {
        const items = await Item.find().select('_id name');
        if (items != []) {
            for (const item of items) {
                console.log(item.name)
                const instance = new Instance({
                    item: item._id,
                    sender: 'GGJ'
                })
                await instance.save()
            }
            process.exit()
        }
    } catch (err) {
        console.log(err);
    }
}
populate()