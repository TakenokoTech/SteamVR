import grpc from "grpc";
import protoLoader from "@grpc/proto-loader";
import sample from "./sample.json";
import Log from "./utils/Log.mjs";

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
    Log.info("get" /**, new String(call.request).length*/);
    var label = "time";
    // console.time(label);
    callback(null, {
        message: aveterPosition["pos"]
    });
    // console.timeEnd(label);
}

function update(call, callback) {
    Log.info("update" /**, new String(call.request).length*/);
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
