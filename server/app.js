const express = require('express')
const mongoose = require('mongoose')
const router = require('./router')

const app = express()
require('dotenv').config()

mongoose.set('uesCreateIndex', true)
mongoose.connect(process.env.DATABASE_URL, {
    useNewUrlParser: true
})

app.use(express.json())
app.use('/server', router)
app.all('/', (req, res) => {
    res.send('Server online');
})

module.exports = app