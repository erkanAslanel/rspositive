(function (service) {

    var addressList = [];


    service.createAddress = function (command, next) {
 
        addressList.push(command);

        console.log(addressList);
        next(null);

      
    };



})(module.exports);