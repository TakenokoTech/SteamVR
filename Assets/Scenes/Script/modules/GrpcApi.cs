using System;
using System.Collections;
using System.Collections.Generic;
using Grpc.Core;
using protos;
using UnityEngine;
using UnityEngine.UI;

namespace apiprotocol {

    public class GrpcApi {

        private static bool finished = true;

        public static String Get () {
            if (!finished) {
                return null;
            }
            try {
                finished = false;
                Channel channel = new Channel ("127.0.0.1:9999", ChannelCredentials.Insecure);
                var client = new Position.PositionClient (channel);
                var reply = client.Get (new GetRequest { });
                Debug.Log ("reply: " + reply.Message);
                channel.ShutdownAsync ().Wait ();
                return reply.Message;
            } catch (Exception e) {
                Debug.Log (e.StackTrace);
            } finally {
                finished = true;
            }
            return null;
        }

        public static void Update (string str) {
            if (!finished) {
                return;
            }
            try {
                finished = false;
                Channel channel = new Channel ("127.0.0.1:9999", ChannelCredentials.Insecure);
                var client = new Position.PositionClient (channel);
                var reply = client.Update (new UpdateRequest { Message = str });
                Debug.Log ("reply");
                channel.ShutdownAsync ().Wait ();
            } catch (Exception e) {
                Debug.Log (e.StackTrace);
            } finally {
                finished = true;
            }
        }
    }
}