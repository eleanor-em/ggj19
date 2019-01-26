const express = require('express')
const instancesController = require('./controllers/instance')
const router = new express.Router()

function errorHandler(err, res) {
    console.log(err)
    res.status(err.status || 500)
    res.json({ err })
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

router.post('/get', instancesController.getAny)
router.use((err, req, res, next) => errorHandler(err, res))
module.exports = router