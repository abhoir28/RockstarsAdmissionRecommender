'use strict';

var mongoose = require("mongoose");

console.log('Loading function');

exports.handler = (event, context, callback) => {

     
        mongoose.connect("mongodb://172.31.9.174/test",{useMongoClient: true});
       
        //var query = {$and:[{ "department": event.department},{"category":event.category},{"subCategory":event.subCategory},{"minCet":{ $lt: event.minCet }}]};

        var query = {$and:[{"location":event.location},{"category":event.category},{"subCategory":event.subCategory},{ "department": event.department},{"minCet":{ $lt: event.minCet }}]};

       var Schema = mongoose.Schema;

       var userSchema = new Schema({
            collegeCode:String,
            collegeName :String,
            category : String,
            fees:String,
            department :String,
            subCategory:String,
            minCet :Number,
            webName :String,
            address :String,
            description:String,
            imgUrl:String,
            location:String
        });
        var userModel;
        if (mongoose.models.userModel) {
            userModel = mongoose.model("tests");
        }
        else {
             userModel = mongoose.model("tests",userSchema);
        }
       

        userModel.find(query,function(err,userObj){
       
           if(err){
                context.done();
                console.log(err);
                callback(null,err);
            }

           else{
                context.succeed(userObj);
                console.log("Found :",userObj); 
                
                callback(null, JSON.stringify(userObj));
                delete mongoose.connection.models['tests'];
            }
        });


};