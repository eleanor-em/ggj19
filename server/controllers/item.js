const Item = require('../models/item')

module.exports = {
    add: async (req, res, next) => {
        try {
            const item = new Item({
                name: req.body.name,
                description: req.body.description,
                solid: req.body.solid,
                owner: req.body.owner
            })
            await item.save()
            res.send('ok')
        } catch (err) {
            next(err)
        }
    },
    list: async (req, res, next) => {
        try {
            const data = await Item.find()
            res.json(data)
        } catch (err) {
            next(rer)
        }
    }
}