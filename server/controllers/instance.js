const Instance = require('../models/instance')
const Item = require('../models/item')

module.exports = {
    getAny: async (req, res, next) => {
        try {
            const instanceCount = await Instance.countDocuments()
            const index = Math.floor(Math.random() * instanceCount)
            const instance = await Instance.findOne().skip(index).populate('item')
            res.json(instance)
        } catch (err) {
            next(err)
        }
    }
}