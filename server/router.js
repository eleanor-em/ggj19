const express = require('express')
const itemsController = require('./controllers/item')
const instancesController = require('./controllers/instance')
const router = new express.Router()

function errorHandler(err, res) {
    if (err) {
        console.log(err)
        res.status(err.status || 500)
        res.json({ err })
    } else {
        res.status(200)
        res.send('ok')
    }
}

router.use(async (req, res, next) => {
    try {
        if (req.body.auth == process.env.AUTH) {
            next()
        } else {
            res.json({ err: 'authentication failure'})
        }
    } catch (err) {
        errorHandler(err, res)
    }
})

router.get('/', instancesController.list)
router.post('/get', instancesController.getAny)
router.post('/put', instancesController.addOne)
router.get('/item', itemsController.list)
router.post('/item', itemsController.add)
router.use((err, req, res, next) => errorHandler(err, res))
module.exports = router