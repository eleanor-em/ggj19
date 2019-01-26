const Instance = require('../models/instance')
const Item = require('../models/item')

module.exports = {
    list: async(req,res, next) => {
        try {
            const instances = await Instance.find().populate('item', 'name')
            res.json(instances)
        } catch (err) {
            next(err)
        }
    },
    getAny: async (req, res, next) => {
        try {
            const instanceCount = await Instance.countDocuments()
            const index = Math.floor(Math.random() * instanceCount)
            const instance = await Instance.findOneAndDelete().skip(index).populate('item')
            res.json(instance)
        } catch (err) {
            next(err)
        }
    },
    addOne: async (req, res, next) => {
        try {
            const item = req.body.item;
            const sender = req.body.sender;
            if (typeof item !== 'undefined' && typeof sender !== 'undefined') {
                const itemDoc = await Item.findOne({ name: item })
                if (itemDoc != null) {
                    const id = itemDoc._id;
                    const instance = new Instance({
                        item: id,
                        sender
                    })
                    await instance.save()
                    console.log(`saved instance of ${item}, sent by ${sender}`)
                    res.status(200).send('ok')
                } else {
                    next('no such item')
                }
            } else {
                next('malformed request')
            }
        } catch (err) {
            next(err)
        }
    }
}