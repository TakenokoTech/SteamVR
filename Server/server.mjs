import grpc from "grpc";
import protoLoader from "@grpc/proto-loader";
import sample from "./sample.json";

const aveterPosition = {};

const avaterProto = grpc.loadPackageDefinition(
    protoLoader.loadSync("../Protos/avater.proto", {
        keepCase: true,
        longs: String,
        enums: String,
        defaults: true,
        oneofs: true
    })
).avater;

function get(call, callback) {
    console.log("get", call.request.message);
    callback(null, {
        message: JSON.stringify(sample) /** aveterPosition["pos"] */
    });
}

function update(call, callback) {
    console.log("update", call.request.message);
    aveterPosition["pos"] = call.request.message;
    callback(null, { message: "Hello " + call.request.message });
}

function main() {
    const server = new grpc.Server();
    server.addService(avaterProto.Position.service, {
        get: get,
        update: update
    });
    server.bind("127.0.0.1:9999", grpc.ServerCredentials.createInsecure());
    server.start();
}
main();
