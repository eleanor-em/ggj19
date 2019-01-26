const mongoose = require('mongoose')
const { Schema } = mongoose;

const instanceSchema = new Schema({
    item: {
        type: Schema.Types.ObjectId,
        ref: 'Item',
        required: true
    },
    sender: {
        type: String,
        required: true
    }
})

module.exports = mongoose.model('Instance', instanceSchema)