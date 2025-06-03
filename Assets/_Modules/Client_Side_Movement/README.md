# Activity 101: Move the client model

## instructione
Make a client model move around the platform and make sure that other clients are able to see each others' movements on their screens.

## what i did
- all movement is client-side instead of server-side to reduce sluggishness. this isnt really a good idea since its vulerable to cheating (speedhacks) but it CAN be protected by having server-side speed validation (flyhack checking, speedhack checking, etc)
- using [ClientNetworkTransform](https://docs-multiplayer.unity3d.com/netcode/current/components/networktransform/#owner-authoritative-mode) for client side movement. i dont wanna bother trying to reduce lag
