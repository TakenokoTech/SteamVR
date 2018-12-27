using System;
using System.Collections;
using System.Collections.Generic;
using Grpc.Core;
using protos;
using UnityEngine;
using UnityEngine.UI;

namespace apiprotocol {

    public class GrpcApi {

        private static bool finishedGet = true;
        private static bool finishedUpdate = true;

        public static String Get () {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch ();
            sw.Start ();
            if (!finishedGet) {
                return null;
            }
            try {
                finishedGet = false;
                Channel channel = new Channel ("127.0.0.1:9999", ChannelCredentials.Insecure);
                var client = new Position.PositionClient (channel);
                var reply = client.Get (new GetRequest { });
                // Debug.Log ("reply: " + reply.Message);
                channel.ShutdownAsync ().Wait ();
                return reply.Message;
            } catch (Exception e) {
                Debug.Log (e.StackTrace);
            } finally {
                sw.Stop ();
                Debug.Log (sw.ElapsedMilliseconds + "ms");
                finishedGet = true;
            }
            return null;
        }

        public static void Update (string str) {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch ();
            sw.Start ();
            if (!finishedUpdate) {
                return;
            }
            try {
                finishedUpdate = false;
                Channel channel = new Channel ("127.0.0.1:9999", ChannelCredentials.Insecure);
                var client = new Position.PositionClient (channel);
                var reply = client.Update (new UpdateRequest { Message = str });
                // Debug.Log ("reply");
                channel.ShutdownAsync ().Wait ();
            } catch (Exception e) {
                Debug.Log (e.StackTrace);
            } finally {
                sw.Stop ();
                Debug.Log (sw.ElapsedMilliseconds + "ms");
                finishedUpdate = true;
            }
        }
    }
}