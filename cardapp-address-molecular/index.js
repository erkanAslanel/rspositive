const { ServiceBroker } = require("moleculer");
const ApiService = require("moleculer-web");
const addressService = require("./service/addressService");
const { MoleculerError } = require("moleculer").Errors;

let transporter = process.env.TRANSPORTER || "TCP";

const broker = new ServiceBroker({
    nodeID: "node-1",
    logger: true,
    transporter: {
        type: "TCP",
        options: {
            udpDiscovery: false,
            urls: [
                
            ],
            port:6001
        }
    }
});

broker.createService({
    mixins: [ApiService],
    name: "address",
    settings: {
        port: 3001
    },
    actions: {


        createAddress: { 
            params: {
                userId: "string",
                addressName: "string",
                addressDetail: "string"

            },
            handler(ctx) {

                var command = ctx.params;

                return new Promise(function (resolve, reject) {

                    addressService.createAddress(command, function (err, resp) {

                        if (err) {
                            throw new MoleculerError("Something happened", 501, "ERR_SOMETHING");
                        }

                      
                        ctx.meta.$statusCode=201;
                        return resolve("created_2");

                    })
                })


            }
        }
    }
});



broker.start();

